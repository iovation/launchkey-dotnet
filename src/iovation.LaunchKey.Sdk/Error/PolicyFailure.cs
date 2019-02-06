using System;

namespace iovation.LaunchKey.Sdk.Error
{
    public class PolicyFailure : InvalidRequestException
    {
        public PolicyFailure(string message) : base(message)
        {
        }

        public PolicyFailure(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PolicyFailure(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}