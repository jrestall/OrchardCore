using System.Collections.Generic;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Topics
{
    /// <summary>
    /// Defines a default wildcard <see cref="WebHookTopic"/> which matches all events.
    /// </summary>
    public class WildcardWebHookTopic : IWebHookTopicProvider
    {
        public static readonly WebHookTopic WildcardTopic = new WebHookTopic("*", "All events");

        public IEnumerable<WebHookTopic> GetFilters()
        {
            return new[] { WildcardTopic };
        }
    }
}