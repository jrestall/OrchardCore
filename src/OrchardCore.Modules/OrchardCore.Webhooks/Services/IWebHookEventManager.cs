using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookEventManager
    {
        Task<List<WebHookEvent>> GetAllWebHookEventsAsync();

        Task<HashSet<string>> NormalizeEventsAsync(IEnumerable<string> submittedEvents);
    }
}
