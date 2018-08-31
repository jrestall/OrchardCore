using System.Collections.Generic;
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
    }
}