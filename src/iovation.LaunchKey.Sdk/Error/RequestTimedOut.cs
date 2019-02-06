using System;

namespace iovation.LaunchKey.Sdk.Error
{
    /// <summary>
    /// Thrown when a timeout (408) occurs unexpectedly
    /// </summary>
    [Serializable]
    public class RequestTimedOut : CommunicationErrorException
    {
        public RequestTimedOut(string message) : base(message)
        {
        }
    }
}