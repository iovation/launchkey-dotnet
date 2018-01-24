using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Domain.Service
{
	/// <summary>
	/// Represents an authorization response from the API
	/// </summary>
	public class AuthorizationResponse
	{
		/// <summary>
		/// The originating authorization request ID.
		/// </summary>
		public string AuthorizationRequestId { get; }

		/// <summary>
		/// Whether or not the user approved the authorization request.
		/// </summary>
		public bool Authorized { get; }

		/// <summary>
		/// Hashed user identifier to track a specific user across services. This value will be used by the Session Ended Webhook to identify the user that is logging out.
		/// </summary>
		public string ServiceUserHash { get; }

		/// <summary>
		/// uniquely identifies the user across the entire Organization
		/// </summary>
		public string OrganizationUserHash { get; }

		/// <summary>
		/// A value uniquely and permanently identifying the User associated with the Authorization Request.
		/// </summary>
		public string UserPushId { get; }

		/// <summary>
		/// Unique identifier for the device the user used to respond to the Authorzation Request
		/// </summary>
		public string DeviceId { get; }

		/// <summary>
		/// A list of strings containing of up to 5 codes. The list is intended for for device validation in conjunction with a Device ID. See LaunchKey docs for additional information and guidance.
		/// </summary>
		public List<string> DevicePins { get; }

		public AuthorizationResponse(string authorizationRequestId, bool authorized, string serviceUserHash, string organizationUserHash, string userPushId, string deviceId, List<string> devicePins)
		{
			AuthorizationRequestId = authorizationRequestId;
			Authorized = authorized;
			ServiceUserHash = serviceUserHash;
			OrganizationUserHash = organizationUserHash;
			UserPushId = userPushId;
			DeviceId = deviceId;
			DevicePins = devicePins;
		}
	}
}
