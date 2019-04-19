using System;
namespace iovation.LaunchKey.Sdk.Domain.Service
{
    /// <summary>
    /// Represents an authentication method in an authentication response
    /// </summary>
    public class AuthMethod
    {
        /// <summary>
        /// Represents the type of method
        /// </summary>

        //MAKE THIS AN ENUM
        public AuthMethodType Method { get; set; }


        /// <summary>
        /// Whether the subscriber has set up this auth method
        /// </summary>
        public bool? Set { get; set; }

        /// <summary>
        /// Whether this auth method can be used during a request
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Whether the subscriber has explicitly disallowed this auth method
        /// </summary>
        public bool Allowed { get; set; }

        /// <summary>
        /// Whether the auth method is supported on the subscriber's device
        /// </summary>
        public bool Supported { get; set; }

        /// <summary>
        /// Whether the subscriber requires this auth method with every request
        /// </summary>
        public bool? UserRequired { get; set; }

        /// <summary>
        /// Whether the method was successfully verified or not
        /// </summary>
        public bool? Passed { get; set; }

        /// <summary>
        /// Whether there was an error preventing verification of this auth method
        /// </summary>
        public bool? Error { get; set; }

        /// <summary>
        /// Creates an auth method based on several options
        /// </summary>
        /// <example>
        /// new AuthMethod(
        ///     "face",
        ///     false,
        ///     false,
        ///     true,
        ///     true,
        ///     null,
        ///     null,
        ///     null
        /// )
        /// </example>
        /// <param name="method">A string representing the auth method</param>
        /// <param name="set">Whether the subscriber set up this method. Can be null</param>
        /// <param name="active">Whether the method can be used</param>
        /// <param name="allowed">Whether the subscriber has disallowed this method</param>
        /// <param name="supported">Whether the device supports this method</param>
        /// <param name="userRequired">Whether the subscriber requires this method. Can be null</param>
        /// <param name="passed">Whether the method was verified or not. Can be null</param>
        /// <param name="error">Whether there was an error. Can be null</param>
        public AuthMethod(
            AuthMethodType method = AuthMethodType.OTHER,
            bool? set = null,
            bool active = false,
            bool allowed = false,
            bool supported = false,
            bool? userRequired = null, 
            bool? passed = null,
            bool? error = null
        )
        {

            Method = method;
            Set = set;
            Active = active;
            Allowed = allowed;
            Supported = supported;
            UserRequired = userRequired;
            Passed = passed;
            Error = error;

        }

        public override string ToString()
        {
            return string.Format("Auth Method: {0} \n Set: {1} \n Active: {2} \n Allowed: {3} \n Supported: {4} \n UserRequired: {5} \n Passed: {6} \n Error: {7} \n",
                Method, Set, Active, Allowed, Supported, UserRequired, Passed, Error);
        }

    }

    /// <summary>
    /// The response type. This is a general categorization of the response.
    /// </summary>
    public enum AuthMethodType
    {
        PIN_CODE,

        CIRCLE_CODE,

        GEOFENCING,

        LOCATIONS,

        WEARABLES,

        FINGERPRINT,

        FACE,

        OTHER
    }
}
