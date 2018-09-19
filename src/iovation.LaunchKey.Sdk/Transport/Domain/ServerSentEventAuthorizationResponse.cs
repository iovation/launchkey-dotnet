using System;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServerSentEventAuthorizationResponse : ServiceV3AuthsGetResponse, IServerSentEvent
	{
		public ServerSentEventAuthorizationResponse(EntityIdentifier requestingEntity, Guid serviceId, string serviceUserHash, string organizationUserHash, string userPushId, Guid authorizationRequestId, bool response, string deviceId, string[] devicePins)
			: base(requestingEntity, serviceId, serviceUserHash, organizationUserHash, userPushId, authorizationRequestId, response, deviceId, devicePins)
		{
		}
	}
}