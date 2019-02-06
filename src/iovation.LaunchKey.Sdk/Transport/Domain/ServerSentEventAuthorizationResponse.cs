using System;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServerSentEventAuthorizationResponse : ServiceV3AuthsGetResponse, IServerSentEvent
    {
        public ServerSentEventAuthorizationResponse(EntityIdentifier requestingEntity, Guid serviceId, string serviceUserHash, string organizationUserHash, string userPushId, Guid authorizationRequestId, bool response, string deviceId, string[] devicePins, string type, string reason, string denialReason)
            : base(requestingEntity, serviceId, serviceUserHash, organizationUserHash, userPushId, authorizationRequestId, response, deviceId, devicePins, type, reason, denialReason)
        {
        }
    }
}