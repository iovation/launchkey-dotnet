using System;

namespace iovation.LaunchKey.Sdk.Error
{
    /// <summary>
    /// Base class for all communication related exceptions (i.e. those caused by transport-level errors)
    /// </summary>
    [Serializable]
    public class CommunicationErrorException : BaseException
    {
        public CommunicationErrorException(string message) : base(message)
        {
        }

        public CommunicationErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CommunicationErrorException(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
        {
        }

        public static CommunicationErrorException FromStatusCode(int statusCode, string message)
        {
            switch (statusCode)
            {
                case 401: return new Unauthorized(message);
                case 403: return new Forbidden(message);
                case 404: return new EntityNotFound(message);
                case 408: return new RequestTimedOut(message);
                case 429: return new RateLimited(message);
                default: return new CommunicationErrorException(message);
            }
        }
    }
}