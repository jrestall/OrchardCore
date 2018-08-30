using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OrchardCore.Environment.Shell;
using OrchardCore.WebHooks.Models;

namespace OrchardCore.WebHooks.Services
{
    /// <summary>
    /// Provides a base implementation of <see cref="IWebHookSender"/> that defines the default format
    /// for HTTP requests sent as WebHooks.
    /// </summary>
    public class WebHookSender : IWebHookSender
    {
        private const string HeaderPrefix = "X-Orchard-";
        private const string HeaderIdName = HeaderPrefix + "Id";
        private const string HeaderEventName = HeaderPrefix + "Event";
        private const string HeaderTenantName = HeaderPrefix + "Tenant";
        private const string SignatureHeaderName = HeaderPrefix + "Signature";

        private const string SignatureHeaderKey = "sha256";
        private const string SignatureHeaderValueTemplate = SignatureHeaderKey + "={0}";

        private readonly IHttpClientFactory _clientFactory;
        private readonly ShellSettings _shellSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebHookSender"/> class.
        /// </summary>
        public WebHookSender(
            IHttpClientFactory clientFactory,
            ShellSettings shellSettings,
            ILogger<WebHookSender> logger)
        {
            Logger = logger;
            _clientFactory = clientFactory;
            _shellSettings = shellSettings;
        }

        public ILogger Logger { get; set; }

        /// <inheritdoc />
        public Task SendNotificationsAsync(IEnumerable<WebHook> webHooks, string topic, Func<JObject> payload)
        {
            // Send all webhooks in parallel
            return Task.WhenAll(webHooks.Select(webHook => SendWebHookAsync(webHook, topic, payload)));
        }

        private async Task SendWebHookAsync(WebHook webHook, string topic, Func<JObject> payload)
        {
            try
            {
                // Setup and send WebHook request
                var request = CreateWebHookRequest(webHook, topic, payload);

                var clientName = webHook.ValidateSsl ? "webhooks" : "webhooks_insecure";
                var client = _clientFactory.CreateClient(clientName);

                var response = await client.SendAsync(request);

                var message = $"WebHook '{webHook.Id}' resulted in status code '{response.StatusCode}'.";
                Logger.LogInformation(message);

                if (response.IsSuccessStatusCode)
                {
                    // If we get a successful response then we are done.
                    //await OnWebHookSuccess(webHook);
                    // TODO: Log webhook events
                }
                else if (response.StatusCode == HttpStatusCode.Gone)
                {
                    // If we get a 410 Gone then we are also done.
                    //await OnWebHookGone(webHook);
                }
            }
            catch (Exception ex)
            {
                var message = $"Failed to submit WebHook {webHook.Id} due to failure: {ex.Message}";
                Logger.LogError(message, ex);
            }
        }


        /// <summary>
        /// Creates an <see cref="HttpRequestMessage"/> containing the headers and body given a <paramref name="webHook"/>.
        /// </summary>
        /// <param name="webHook">A <see cref="WebHook"/> to be sent.</param>
        /// <param name="topic"></param>
        /// <param name="payload"></param>
        /// <returns>A filled in <see cref="HttpRequestMessage"/>.</returns>
        protected virtual HttpRequestMessage CreateWebHookRequest(WebHook webHook, string topic, Func<JObject> payload)
        {
            if (webHook == null)
            {
                throw new ArgumentNullException(nameof(webHook));
            }

            // Create WebHook request
            var request = new HttpRequestMessage(HttpMethod.Post, webHook.Url);

            // Fill in request body based on WebHook and payload
            var body = CreateWebHookRequestBody(webHook, payload);
            SignWebHookRequest(webHook, request, body);

            AddWebHookMetadata(webHook, topic, request);

            // Add extra request or entity headers
            foreach (var kvp in webHook.Headers)
            {
                if (request.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value)) continue;
                if (request.Content.Headers.TryAddWithoutValidation(kvp.Key, kvp.Value)) continue;

                var message = $"Could not add header field \'{kvp.Key}\' to the WebHook request for WebHook ID \'{webHook.Id}\'.";
                Logger.LogError(message);
            }

            return request;
        }

        /// <summary>
        /// Creates a <see cref="JObject"/> used as the <see cref="HttpRequestMessage"/> entity body for a webhook notification.
        /// </summary>
        /// <param name="webHook">A <see cref="WebHook"/> to be sent.</param>
        /// <param name="payload">The object representing the data to be sent with the webhook notification.</param>
        /// <returns>An initialized <see cref="JObject"/>.</returns>
        protected virtual JObject CreateWebHookRequestBody(WebHook webHook, Func<JObject> payload)
        {
            var body = payload == null ? new JObject() : payload();
            
            // Only include fields in the notification that the user has specified.
            if (webHook.Fields != null && webHook.Fields.Any())
            {
                body.Properties()
                    .Where(prop => !webHook.Fields.Contains(prop.Name))
                    .ToList()
                    .ForEach(prop => prop.Remove());
            }

            return body;
        }

        /// <summary>
        /// Adds a SHA 256 signature to the <paramref name="body"/> and adds it to the <paramref name="request"/> as an
        /// HTTP header to the <see cref="HttpRequestMessage"/> along with the entity body.
        /// </summary>
        /// <param name="webHook">The current <see cref="WebHook"/>.</param>
        /// <param name="request">The request to add the signature to.</param>
        /// <param name="body">The body to sign and add to the request.</param>
        protected virtual void SignWebHookRequest(WebHook webHook, HttpRequestMessage request, JObject body)
        {
            if (webHook == null)
            {
                var message = $"Invalid \'{GetType().Name}\' instance: \'WebHook\' cannot be null.";
                throw new ArgumentException(message, nameof(webHook));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var secret = Encoding.UTF8.GetBytes(webHook.Secret);
            using (var hasher = new HMACSHA256(secret))
            {
                var serializedBody = body.ToString();
                request.Content = new StringContent(serializedBody, Encoding.UTF8, "application/json");

                var data = Encoding.UTF8.GetBytes(serializedBody);
                var sha256 = hasher.ComputeHash(data);
                var headerValue = string.Format(CultureInfo.InvariantCulture, SignatureHeaderValueTemplate, ByteArrayToString(sha256));
                request.Headers.Add(SignatureHeaderName, headerValue);
            }
        }

        private void AddWebHookMetadata(WebHook webHook, string topic, HttpRequestMessage request)
        {
            request.Headers.Add(HeaderIdName, webHook.Id);
            request.Headers.Add(HeaderEventName, topic);
            request.Headers.Add(HeaderTenantName, _shellSettings.Name);
        }

        public static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-","");
        }
    }
}