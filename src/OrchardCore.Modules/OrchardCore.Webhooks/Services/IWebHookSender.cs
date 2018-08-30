using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    /// <summary>
    /// Provides an abstraction for sending out WebHooks as provided by <see cref="IWebHookManager"/>. Implementation
    /// can control the format of the WebHooks as well as how they are sent including retry policies and error handling.
    /// </summary>
    public interface IWebHookSender
    {
        /// <summary>
        /// Sends out the given collection of <paramref name="webHooks"/> using whatever mechanism defined by the
        /// <see cref="IWebHookSender"/> implementation.
        /// </summary>
        /// <param name="webHooks">The collection of <see cref="WebHook"/> instances to process.</param>
        /// <param name="topic">The topic that triggered the notifications.</param>
        /// <param name="payload">The object to be sent with the webhook.</param>
        Task SendNotificationsAsync(IEnumerable<WebHook> webHooks, string topic, Func<JObject> payload);
    }
}