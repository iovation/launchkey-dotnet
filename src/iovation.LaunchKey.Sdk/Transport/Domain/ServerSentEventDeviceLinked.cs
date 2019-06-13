using System;
namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServerSentEventDeviceLinked : IServerSentEvent
    {

        public DeviceLinkCompletion DeviceLinkCompletion { get; }

        public ServerSentEventDeviceLinked(DeviceLinkCompletion deviceLinkCompletion)
        {
            DeviceLinkCompletion = deviceLinkCompletion;
        }
    }
}
