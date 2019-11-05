using System;
namespace iovation.LaunchKey.Sdk.Error
{
    public class InvalidPolicyAttributes : BaseException
    {
        public InvalidPolicyAttributes(string message) : base(message)
        {
        }

        public InvalidPolicyAttributes(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidPolicyAttributes(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}
