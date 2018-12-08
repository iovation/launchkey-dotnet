using System;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceV3AuthsGetResponse
	{
		public ServiceV3AuthsGetResponse(
			EntityIdentifier requestingEntity,
			Guid serviceId,
			string serviceUserHash,
			string organizationUserHash,
			string userPushId,
			Guid authorizationRequestId,
			bool response,
			string deviceId,
			string[] devicePins,
			string type,
			string reason,
			string denialReason)
		{
			RequestingEntity = requestingEntity;
			ServiceId = serviceId;
			ServiceUserHash = serviceUserHash;
			OrganizationUserHash = organizationUserHash;
			UserPushId = userPushId;
			AuthorizationRequestId = authorizationRequestId;
			Response = response;
			DeviceId = deviceId;
			DevicePins = devicePins;
			Type = type;
			Reason = reason;
			DenialReason = denialReason;
		}

		public EntityIdentifier RequestingEntity { get; }
		public Guid ServiceId { get; }
		public string ServiceUserHash { get; }
		public string OrganizationUserHash { get; }
		public string UserPushId { get; }
		public Guid AuthorizationRequestId { get; }
		public bool Response { get; }
		public string DeviceId { get; }
		public string[] DevicePins { get; }
		public string Type { get; }
		public string Reason { get; }
		public string DenialReason { get; }
	}
}