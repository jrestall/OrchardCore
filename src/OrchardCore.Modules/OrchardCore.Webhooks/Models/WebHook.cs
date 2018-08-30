using System.Collections.Generic;

namespace OrchardCore.WebHooks.Models
{
    public class WebHook
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public string Secret { get; set; }

        public IList<string> Topics { get; set; }

        /// <summary>
        /// Optional array of fields that should be included in the webhook subscription.
        /// </summary>
        public IList<string> Fields { get; set; }

        /// <summary>
        /// A JavaScript expression that evaluates to true or false that filters the triggering events. 
        /// </summary>
        public string FilterExpression { get; set; }

        public bool Enabled { get; set; } = true;

        public bool ValidateSsl { get; set; } = true;
    }
}