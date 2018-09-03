using System;
using System.Collections.Generic;
using System.Linq;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Events
{
    /// <summary>
    /// Defines a default wildcard <see cref="WebHookEvent"/> which matches all events.
    /// </summary>
    public class WildcardWebHookEvent : IWebHookEventProvider
    {
        public static readonly WebHookEvent WildcardEvent = new WebHookEvent("*", category: "Root");

        public IEnumerable<WebHookEvent> GetEvents()
        {
            return new[] { WildcardEvent };
        }

        public IEnumerable<string> NormalizeEvents(IEnumerable<string> submittedEvents)
        {
            if(submittedEvents == null) throw new ArgumentNullException(nameof(submittedEvents));

            // If there are no submitted events then add our wildcard event.
            if (submittedEvents.Count() == 0)
            {
                return new HashSet<string>(new[] {WildcardEvent.Name});
            }

            return new HashSet<string>();
        }
    }
}