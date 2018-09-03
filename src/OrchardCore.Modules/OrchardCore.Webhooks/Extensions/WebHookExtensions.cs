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
        /// Determines whether a given <paramref name="@event"/> matches the subscribed topics for a given <see cref="WebHook"/>.
        /// The action can either match a topic directly or match a wildcard.
        /// </summary>
        /// <param name="webHook">The <see cref="WebHook"/> instance to operate on.</param>
        /// <param name="topic">The topic to match against the subscribed <paramref name="webHook"/> topics.</param>
        /// <returns><c>true</c> if the <paramref name="@event"/> matches, otherwise <c>false</c>.</returns>
        public static bool MatchesEvent(this WebHook webHook, string @event)
        {
            return webHook != null && (webHook.Events.Contains(WildcardWebHookEvent.WildcardEvent.Id) || webHook.Events.Contains(@event));
        }
    }
}