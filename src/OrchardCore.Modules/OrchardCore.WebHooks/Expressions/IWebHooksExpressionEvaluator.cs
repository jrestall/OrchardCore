using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Expressions
{
    public interface IWebHooksExpressionEvaluator
    {
        Task<JObject> RenderAsync(WebHook webHook, WebHookNotificationContext context);
    }
}