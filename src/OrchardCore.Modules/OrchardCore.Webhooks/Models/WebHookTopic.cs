using System;

namespace OrchardCore.WebHooks.Models
{
    /// <summary>
    /// Defines a filter which can be applied when registering a WebHook. 
    /// The filter determines which event notifications will get dispatched to a given WebHook. 
    /// That is, depending on which filters a WebHook is created with, it will only see event 
    /// notifications that match one or more of those filters.
    /// </summary>
    public class WebHookTopic
    {
        /// <summary>
        /// Gets or sets the name of the filter, e.g. <c>Article Update</c>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the filter.
        /// </summary>
        public string Description { get; set; }

        public WebHookTopic(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public WebHookTopic(string name, string description) : this(name)
        {
            Description = description;
        }
    }
}