using System.Collections.Generic;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.ViewModels
{
    public class EditWebHookViewModel
    {
        public List<WebHookEvent> Events { get; set; }

        public WebHook WebHook { get; set; } = new WebHook();
    }
}