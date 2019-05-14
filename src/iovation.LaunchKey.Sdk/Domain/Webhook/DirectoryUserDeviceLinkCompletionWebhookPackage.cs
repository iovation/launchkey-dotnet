using iovation.LaunchKey.Sdk.Domain.Directory;

namespace iovation.LaunchKey.Sdk.Domain.Webhook
{
    public class DirectoryUserDeviceLinkCompletionWebhookPackage : IWebhookPackage
    {
        public DeviceLinkCompletion DeviceLinkData { get; }

        public DirectoryUserDeviceLinkCompletionWebhookPackage( DeviceLinkCompletion directoryUserDeviceLinkData)
        {
            DeviceLinkData = directoryUserDeviceLinkData;
        }
    }
}