using System.Collections.Generic;

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
        /// Unique identifier for the device the user used to respond to the Authorization Request
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// A list of strings containing of up to 5 codes. The list is intended for for device validation in conjunction with a Device ID. See LaunchKey docs for additional information and guidance.
        /// </summary>
        public List<string> DevicePins { get; }

        /// <summary>
        /// The response type. This is a general categorization of the response.
        /// </summary>
        public AuthorizationResponseType? Type { get; }

        /// <summary>
        /// The reason for the response value.
        /// </summary>
        public AuthorizationResponseReason? Reason { get; }

        /// <summary>
        /// ID for DenialReason in list provided to createAuthorizationRequest which was selected by the user when denying the request.
        /// </summary>
        public string DenialReason { get; }

        /// <summary>
        /// Was the authorization request identified as fraudulent?
        /// </summary>
        public bool? Fraud { get; }

        /// <summary>
        /// The criteria the user had to fulfill to authorize the request successfully
        /// </summary>
        public AuthPolicy AuthPolicy { get; }

        /// <summary>
        /// A list of all Auth Methods and the role they played in this Authorization request
        /// </summary>
        public IList<AuthMethod> AuthMethods { get; }

        public AuthorizationResponse(string authorizationRequestId, bool authorized, string serviceUserHash, string organizationUserHash, string userPushId, string deviceId, List<string> devicePins, AuthorizationResponseType? type, AuthorizationResponseReason? reason, string denialReason, bool? fraud, AuthPolicy authPolicy, IList<AuthMethod> authMethods)
        {
            AuthorizationRequestId = authorizationRequestId;
            Authorized = authorized;
            ServiceUserHash = serviceUserHash;
            OrganizationUserHash = organizationUserHash;
            UserPushId = userPushId;
            DeviceId = deviceId;
            DevicePins = devicePins;
            Type = type;
            Reason = reason;
            DenialReason = denialReason;
            Fraud = fraud;
            AuthPolicy = authPolicy;
            AuthMethods = authMethods;
        }
    }
}