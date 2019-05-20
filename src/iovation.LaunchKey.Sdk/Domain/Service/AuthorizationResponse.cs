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

    /// <summary>
    /// The response type. This is a general categorization of the response.
    /// </summary>
    public enum AuthorizationResponseType
    {
        /// <summary>
        /// The user authorized the authorization request
        /// </summary>
        AUTHORIZED,

        /// <summary>
        /// The user denied the authorization request
        /// </summary>
        DENIED,

        /// <summary>
        /// The user failed to complete the authentication request
        /// </summary>
        FAILED,

        /// <summary>
        /// Other exists only to allow for forward compatibility to future response types
        /// </summary>
        OTHER
    }

    /// <summary>
    /// The reason for the response value.
    /// </summary>
    public enum AuthorizationResponseReason
    {
        /// <summary>
        /// User satisfies all request policy requirements; successfully authenticates with all applicable methods
        /// </summary>
        APPROVED,

        /// <summary>
        /// User satisfies all request policy requirements; chooses to deny request rather than proceed with
        /// authentication
        /// </summary>
        DISAPPROVED,

        /// <summary>
        /// User satisfies all request policy requirements; chooses to deny request because they believe it to be
        /// fraudulent in some manner
        /// </summary>
        FRAUDULENT,

        /// <summary>
        /// Authenticator fails to satisfy request policy; authentication not allowed
        /// </summary>
        POLICY,

        /// <summary>
        /// Authenticator satisfies all request policy requirements, but permission on device prevents auth method
        /// verification
        /// </summary>
        PERMISSION,

        /// <summary>
        /// Authenticator satisfies all request policy requirements, but user fails to successfully authenticate with
        /// all required authentication methods
        /// </summary>
        AUTHENTICATION,

        /// <summary>
        /// Authenticator fails to satisfy request policy because authenticator configuration is incompatible with the
        /// request policy (i.e. requiring an auth method that is configured to be unavailable to end users)
        /// </summary>
        CONFIGURATION,

        /// <summary>
        /// User can't receive or respond to request because a Local Auth Request is pending authorization
        /// </summary>
        BUSY_LOCAL,

        /// <summary>
        /// Authenticator fails to obtain use of a sensor that was required by policy on the device. 
        /// i.e. wearables was enabled but Bluetooth was not enabled
        /// </summary>
        SENSOR,

        /// <summary>
        /// Other exists only to allow for forward compatibility to future response reasons
        /// </summary>
        OTHER
    }
}