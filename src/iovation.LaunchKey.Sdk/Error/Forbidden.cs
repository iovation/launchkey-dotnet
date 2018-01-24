namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when an HTTP 403 occurs.
	/// </summary>
	public class Forbidden : CommunicationErrorException
	{
		public Forbidden(string message) : base(message)
		{
		}
	}
}