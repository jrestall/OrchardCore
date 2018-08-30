using System.Collections.Generic;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookTopicProvider
    {
        IEnumerable<WebHookTopic> GetFilters();
    }
}