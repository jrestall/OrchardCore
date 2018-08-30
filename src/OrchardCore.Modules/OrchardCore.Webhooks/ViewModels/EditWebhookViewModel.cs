using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.ViewModels
{
    public class EditWebHookViewModel
    {
        public WebHook WebHook { get; set; } = new WebHook();
    }
}