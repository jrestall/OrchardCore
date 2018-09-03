using System;
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

        public IEnumerable<string> NormalizeEvents(IEnumerable<string> submittedEvents)
        {
            if(submittedEvents == null) throw new ArgumentNullException(nameof(submittedEvents));

            return Enumerable.Empty<string>();
        }
    }
}