using System;

namespace OrchardCore.WebHooks.Models
{
    /// <summary>
    /// Defines a filter which can be applied when registering a WebHook. 
    /// The filter determines which event notifications will get dispatched to a given WebHook. 
    /// That is, depending on which filters a WebHook is created with, it will only see event 
    /// notifications that match one or more of those filters.
    /// </summary>
    public class WebHookEvent
    {
        /// <summary>
        /// Gets or sets the unique identifier of the event, e.g. <c>article.updated</c>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the event, e.g. <c>Article Updated</c>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of the event.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the event. Used for grouping in the UI.
        /// </summary>
        public string Category { get; set; }

        public WebHookEvent(string id, string name = null, string description = null, string category = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            
            Name = name;
            Description = description;
            Category = category;
        }
    }
}