using OrchardCore.WebHooks.Models;
using OrchardCore.WebHooks.Services.Events;

namespace OrchardCore.WebHooks.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="WebHook"/>.
    /// </summary>
    public static class WebHookExtensions
    {
        /// <summary>
        /// Determines whether a given <paramref name="topic"/> matches the subscribed topics for a given <see cref="WebHook"/>.
        /// The action can either match a topic directly or match a wildcard.
        /// </summary>
        /// <param name="webHook">The <see cref="WebHook"/> instance to operate on.</param>
        /// <param name="topic">The topic to match against the subscribed <paramref name="webHook"/> topics.</param>
        /// <returns><c>true</c> if the <paramref name="topic"/> matches, otherwise <c>false</c>.</returns>
        public static bool MatchesTopic(this WebHook webHook, string topic)
        {
            return webHook != null && (webHook.Topics.Contains(WildcardWebHookEvent.WildcardEvent.Name) || webHook.Topics.Contains(topic));
        }
    }
}