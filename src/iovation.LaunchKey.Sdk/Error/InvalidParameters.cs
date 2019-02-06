using System;

namespace iovation.LaunchKey.Sdk.Error
{
    public class InvalidParameters : InvalidRequestException
    {
        public InvalidParameters(string message) : base(message)
        {
        }

        public InvalidParameters(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidParameters(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}