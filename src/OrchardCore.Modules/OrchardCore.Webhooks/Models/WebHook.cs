using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrchardCore.WebHooks.Models
{
    public class WebHook
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string HttpMethod { get; set; }
        
        [Required]
        [Url(ErrorMessage = "Invalid webhook URL.")]
        public string Url { get; set; }

        [Required]
        public string ContentType { get; set; }

        public IList<KeyValuePair<string, string>> Headers { get; set; } = new List<KeyValuePair<string, string>>();

        [Required]
        public string Secret { get; set; }

        public IList<string> Events { get; set; } = new List<string>();

        public string PayloadTemplate { get; set; }

        public bool Enabled { get; set; } = true;

        public bool ValidateSsl { get; set; } = true;
    }

    public class WebHookHeader
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}