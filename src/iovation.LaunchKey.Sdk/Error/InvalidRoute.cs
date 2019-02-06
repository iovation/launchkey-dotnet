using System;

namespace iovation.LaunchKey.Sdk.Error
{
    public class InvalidRoute : InvalidRequestException
    {
        public InvalidRoute(string message) : base(message)
        {
        }

        public InvalidRoute(string message, Exception innerException) : base(message, innerException)
        {
        }

        public InvalidRoute(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}