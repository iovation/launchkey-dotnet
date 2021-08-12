using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Webhook;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IWebhookHandler
    {
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
