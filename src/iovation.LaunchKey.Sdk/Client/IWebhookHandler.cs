using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Webhook;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IWebhookHandler
    {
        [System.Obsolete("HandleWebhook(headers,body) is obsolete. Please use HandleWebhook(headers, body, method, path)")]
        IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body);
        /// <summary>
        /// Process a Webhook payload received from the LaunchKey WebHook service.
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="method">The HTTP method of the received request. Optional. Include for stricter security checks.</param>
        /// <param name="path">The HTTP path of the received request. Optional. Include for stricter security checks.</param>
        /// <returns></returns>
        [System.Obsolete("HandleWebhook is deprecated. Please use HandleAdvancedWebhook(headers, body, method, path) instead")]
        IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null);

        /// <summary>
        /// Process an advanced webhook payload received from the LaunchKey service.
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="method"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        IWebhookPackage HandleAdvancedWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null);
    }
}
