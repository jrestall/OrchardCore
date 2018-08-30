using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OrchardCore.WebHooks.Services
{
    public interface IWebHookManager
    {
        Task NotifyAsync(string topic, Func<JObject> payload);
    }
}