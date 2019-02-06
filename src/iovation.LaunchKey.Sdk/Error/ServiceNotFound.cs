using System;

namespace iovation.LaunchKey.Sdk.Error
{
    public class ServiceNotFound : InvalidRequestException
    {
        public ServiceNotFound(string message) : base(message)
        {
        }

        public ServiceNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ServiceNotFound(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }
    }
}