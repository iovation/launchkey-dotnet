using System;
namespace iovation.LaunchKey.Sdk.Error
{
    /// <summary>
    /// Thrown when an action is attempted against an authorization request that
    /// has previously been canceled.
    /// </summary>
    [Serializable]
    public class AuthorizationRequestCanceled : InvalidRequestException
    {
        public AuthorizationRequestCanceled() : base("Authorization request is cancled")
        {
        }

        public AuthorizationRequestCanceled(string message) : base(message)
        {
        }

        public AuthorizationRequestCanceled(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthorizationRequestCanceled(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}
