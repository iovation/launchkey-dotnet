using System;
namespace iovation.LaunchKey.Sdk.Error
{
    public class UnknownPolicyException : BaseException
    {
        public UnknownPolicyException(string message) : base(message)
        {
        }

        public UnknownPolicyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnknownPolicyException(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}
