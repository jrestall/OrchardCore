using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.WebHooks.Extensions;

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

        public async Task NotifyAsync(string topic, Func<JObject> payload)
        {
            if (topic == null) throw new ArgumentNullException(nameof(topic));

            // Get all WebHooks for tenant
            var webHooksList = await _store.GetAllWebHooksAsync();

            // Match any webhooks against the triggered event e.g. *.*, content.created, asset.updated
            var matchedWebHooks = webHooksList.WebHooks.Where(x => x.Enabled && x.MatchesTopic(topic));
            
            // Filter the webhooks against the user provided JavaScript expression
            // var filteredWebHooks = Filter(matchedWebHooks, content);
            
            // Send the notification to the matching webhooks
            await _sender.SendNotificationsAsync(matchedWebHooks, topic, payload); 
        }
    }
}
