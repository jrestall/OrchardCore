using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.WebHooks.Extensions;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public class WebHookManager : IWebHookManager
    {
        private readonly IWebHookSender _sender;
        private readonly IWebHookStore _store;

        public WebHookManager(IWebHookSender sender, IWebHookStore store)
        {
            _sender = sender;
            _store = store;
        }

        public async Task NotifyAsync(string eventName, JObject defaultPayload, Dictionary<string, object> properties)
        {
            if (eventName == null) throw new ArgumentNullException(nameof(eventName));

            // Get all WebHooks for tenant
            var webHooksList = await _store.GetAllWebHooksAsync();

            // Match any webhooks against the triggered event e.g. *.*, content.created, asset.updated
            var matchedWebHooks = webHooksList.WebHooks.Where(x => x.Enabled && x.MatchesEvent(eventName));
            
            var context = new WebHookNotificationContext
            {
                EventName = eventName,
                DefaultPayload = defaultPayload,
                Properties = properties
            };

            // Send the notification to the matching webhooks
            await _sender.SendNotificationsAsync(matchedWebHooks, context); 
        }
    }
}
