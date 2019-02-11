using System;
namespace iovation.LaunchKey.Sdk.Error
{
    /// <summary>
    /// Thrown when an action is attempted against an authorization request
    /// bute cannot complete as the authorization request already has a response.
    /// </summary>
    public class AuthorizationResponseExists : InvalidRequestException
    {
        public AuthorizationResponseExists() : base("Authorization response exists")
        {
        }

        public AuthorizationResponseExists(string message) : base(message)
        {
        }

        public AuthorizationResponseExists(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthorizationResponseExists(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}
