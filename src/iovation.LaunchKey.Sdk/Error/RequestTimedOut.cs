namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a timeout (408) occurs unexpectedly
	/// </summary>
	public class RequestTimedOut : CommunicationErrorException
	{
		public RequestTimedOut(string message) : base(message)
		{
		}
	}
}