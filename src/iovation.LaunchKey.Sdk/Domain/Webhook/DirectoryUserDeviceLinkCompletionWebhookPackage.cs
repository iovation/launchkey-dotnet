using iovation.LaunchKey.Sdk.Domain.Directory;

namespace iovation.LaunchKey.Sdk.Domain.Webhook
{
    /// <summary>
    /// A Webhook package containing a device link completion response
    /// </summary>
    public class DirectoryUserDeviceLinkCompletionWebhookPackage : IWebhookPackage
    {
        /// <summary>
        /// The device link data
        /// </summary>
        public DeviceLinkCompletion DeviceLinkData { get; }

        public DirectoryUserDeviceLinkCompletionWebhookPackage( DeviceLinkCompletion directoryUserDeviceLinkData)
        {
            DeviceLinkData = directoryUserDeviceLinkData;
        }
    }
}