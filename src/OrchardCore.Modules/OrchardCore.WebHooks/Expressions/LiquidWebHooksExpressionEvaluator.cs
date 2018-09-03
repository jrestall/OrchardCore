using System;
using System.Threading.Tasks;
using Fluid;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using OrchardCore.Liquid;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Expressions
{
    public class LiquidWebHooksExpressionEvaluator : IWebHooksExpressionEvaluator
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILiquidTemplateManager _liquidTemplateManager;

        public LiquidWebHooksExpressionEvaluator(
            IServiceProvider serviceProvider,
            ILiquidTemplateManager liquidTemplateManager
        )
        {
            _serviceProvider = serviceProvider;
            _liquidTemplateManager = liquidTemplateManager;
        }

        public async Task<JObject> RenderAsync(WebHook webHook, WebHookNotificationContext context)
        {
            var templateContext = await CreateTemplateContextAsync(webHook, context);
            var result = await _liquidTemplateManager.RenderAsync(webHook.PayloadTemplate, templateContext);
            return string.IsNullOrWhiteSpace(result) ? new JObject() : JObject.Parse(result);
        }

        private async Task<TemplateContext> CreateTemplateContextAsync(WebHook webHook, WebHookNotificationContext context)
        {
            var templateContext = new TemplateContext();
            var services = _serviceProvider;
            
            templateContext.SetValue(nameof(WebHook), webHook);
            templateContext.SetValue("EventName", context.EventName);

            // Add webhook notification properties e.g. Model.Content, Model.Media.
            foreach (var item in context.Properties)
            {
                templateContext.SetValue("Model." + item.Key, item.Value);
            }

            // Add services.
            templateContext.AmbientValues.Add("Services", services);

            // Add UrlHelper, if we have an MVC Action context.
            var actionContext = services.GetService<IActionContextAccessor>()?.ActionContext;
            if (actionContext != null)
            {
                var urlHelperFactory = services.GetRequiredService<IUrlHelperFactory>();
                var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
                templateContext.AmbientValues.Add("UrlHelper", urlHelper);
            }

            // Give modules a chance to add more things to the template context.
            foreach (var handler in services.GetServices<ILiquidTemplateEventHandler>())
            {
                await handler.RenderingAsync(templateContext);
            }

            return templateContext;
        }
    }
}