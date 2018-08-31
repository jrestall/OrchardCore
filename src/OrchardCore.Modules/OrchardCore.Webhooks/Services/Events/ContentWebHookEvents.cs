using System.Collections.Generic;
using System.Linq;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Events
{
    public class ContentWebHookEvents : IWebHookEventProvider
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public ContentWebHookEvents(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public IEnumerable<WebHookEvent> GetEvents()
        {
            // Only allow the user to manage events for content types that are creatabale in the UI
            var typeDefinitions = _contentDefinitionManager.ListTypeDefinitions()
                .Where(definition => definition.Settings.ToObject<ContentTypeSettings>().Creatable);
            
            // Add a filter for each content type e.g. article.created
            foreach (var typeDefinition in typeDefinitions)
            {
                foreach (var contentEvent in ContentEvents.AllEvents)
                {
                    yield return CreateEvent(typeDefinition.Name.ToLower(), contentEvent, typeDefinition.DisplayName);
                }
            }

            // Add a filter for all content type events e.g. content.created, content.published
            foreach (var contentEvent in ContentEvents.AllEvents)
            {
                yield return CreateEvent("content", contentEvent);
            }
        }

        private WebHookEvent CreateEvent(string eventIdentifier, string subEventIdentifier, string eventName = null)
        {
            return new WebHookEvent($"{eventIdentifier}.{subEventIdentifier}", eventName, category: "Content");
        }
    }
}