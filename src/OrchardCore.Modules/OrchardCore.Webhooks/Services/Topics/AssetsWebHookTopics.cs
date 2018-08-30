using System.Collections.Generic;
using System.Linq;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services.Topics
{
    public class AssetsWebHookTopics : IWebHookTopicProvider
    {
        public IEnumerable<WebHookTopic> GetFilters()
        {
            return Enumerable.Empty<WebHookTopic>();
        }
    }
}