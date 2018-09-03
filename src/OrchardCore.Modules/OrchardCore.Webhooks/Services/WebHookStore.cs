using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrchardCore.WebHooks.Models;
using YesSql;

namespace OrchardCore.WebHooks.Services
{
    public class WebHookStore : IWebHookStore
    {
        private readonly ISession _session;
        private WebHookList _webHookList;

        public WebHookStore(ISession session)
        {
            _session = session;
        }

        public async Task<WebHookList> GetAllWebHooksAsync()
        {
            if (_webHookList != null)
            {
                return _webHookList;
            }

            _webHookList = await _session.Query<WebHookList>().FirstOrDefaultAsync();

            if (_webHookList == null)
            {
                _webHookList = new WebHookList();
                _session.Save(_webHookList);
            }

            return _webHookList;
        }

        public async Task<WebHook> GetWebHookAsync(string id)
        {
            var webHookList = await GetAllWebHooksAsync();
            return webHookList.WebHooks.FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public async Task DeleteWebHookAsync(string id)
        {
            var webHookList = await GetAllWebHooksAsync();
            var webHook = await GetWebHookAsync(id);

            if (webHook != null)
            {
                webHookList.WebHooks.Remove(webHook);
                _session.Save(webHookList);
            }
        }

        public async Task<WebHook> CreateWebHookAsync(WebHook webHook)
        {
            var webHookList = await GetAllWebHooksAsync();

            webHook.Id = Guid.NewGuid().ToString("n");
            SanitizeWebHook(webHook);
            
            webHookList.WebHooks.Add(webHook);

            _session.Save(webHookList);

            return webHook;
        }

        public async Task<bool> TryUpdateWebHook(WebHook webHook)
        {
            var webHookList = await GetAllWebHooksAsync();
            var webHookToUpdate = await GetWebHookAsync(webHook.Id);

            if (webHookToUpdate == null)
            {
                return false;
            }

            var index = webHookList.WebHooks.IndexOf(webHookToUpdate);

            SanitizeWebHook(webHook);
            webHookList.WebHooks[index] = webHook;

            _session.Save(webHookList);

            return true;
        }

        public void SanitizeWebHook(WebHook webHook)
        {
            // Remove empty custom headers
            webHook.Headers = webHook.Headers.Where(header => !string.IsNullOrWhiteSpace(header.Key)).ToList();
        }
    }
}