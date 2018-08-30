using System.Collections.Generic;
using System.Threading.Tasks;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookStore
    {
        Task<WebHookList> GetAllWebHooksAsync();

        Task<WebHook> GetWebHookAsync(string id);

        Task DeleteWebHookAsync(string id);

        Task<WebHook> CreateWebHookAsync(WebHook webHook);

        Task<bool> TryUpdateWebHook(WebHook webHook);
    }
}