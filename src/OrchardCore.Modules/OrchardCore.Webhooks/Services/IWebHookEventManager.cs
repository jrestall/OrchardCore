using System;
using System.Collections.Generic;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookEventManager
    {
        List<WebHookEvent> GetAllWebHookEventsAsync();
    }

    /// <summary>
    /// Provides an implementation of <see cref="IWebHookEventManager"/> which provides the set of 
    /// registered <see cref="IWebHookEventProvider"/> instances.
    /// </summary>
    public class WebHookEventManagaer : IWebHookEventManager
    {
        private readonly IEnumerable<IWebHookEventProvider> _providers;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHookEventManagaer"/> class with the 
        /// given <paramref name="providers"/>.
        /// </summary>
        public WebHookEventManagaer(IEnumerable<IWebHookEventProvider> providers)
        {
            _providers = providers ?? throw new ArgumentNullException(nameof(providers));
        }

        public List<WebHookEvent> GetAllWebHookEventsAsync()
        {
            var events = new List<WebHookEvent>();
            foreach (var eventProvider in _providers)
            {
                events.AddRange(eventProvider.GetEvents());
            }

            return events;
        }
    }
}
