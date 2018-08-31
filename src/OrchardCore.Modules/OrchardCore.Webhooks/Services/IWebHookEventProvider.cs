using System.Collections.Generic;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookEventProvider
    {
        IEnumerable<WebHookEvent> GetEvents();
    }
}