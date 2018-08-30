using System.Collections.Generic;
using System.Linq;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Topics
{
    public class ContentTypesWebHookTopics : IWebHookTopicProvider
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;

        private readonly IEnumerable<string> _contentEvents = new[]
        {
            ContentEvents.Created, 
            ContentEvents.Updated,
            ContentEvents.Removed,
            ContentEvents.Published,
            ContentEvents.Unpublished
        }; 

        public ContentTypesWebHookTopics(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public IEnumerable<WebHookTopic> GetFilters()
        {
            // Only allow the user to manage events for content types that are creatabale in the UI
            var typeDefinitions = _contentDefinitionManager.ListTypeDefinitions()
                .Where(definition => definition.Settings.ToObject<ContentTypeSettings>().Creatable);
            
            // Add a filter for each content type e.g. Article.Created
            foreach (var typeDefinition in typeDefinitions)
            {
                foreach (var contentEvent in _contentEvents)
                {
                    yield return CreateTopic(typeDefinition.Name.ToLower(), contentEvent);
                }
            }

            // Add a filter for all content type events e.g. Content.Created, Content.Published
            foreach (var contentEvent in _contentEvents)
            {
                yield return CreateTopic("content", contentEvent);
            } 
        }

        private WebHookTopic CreateTopic(string topicName, string eventName)
        {
            return new WebHookTopic($"{topicName}.{eventName}");
        }
    }
}