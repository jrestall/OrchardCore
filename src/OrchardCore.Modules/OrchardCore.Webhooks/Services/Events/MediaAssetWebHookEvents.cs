using System.Collections.Generic;
using System.Linq;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Events
{
    public class MediaAssetWebHookEvents : IWebHookEventProvider
    {
        public IEnumerable<WebHookEvent> GetEvents()
        {
            return Enumerable.Empty<WebHookEvent>();
        }
    }
}