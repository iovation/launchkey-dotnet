namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a 403 occurs
	/// </summary>
	public class Unauthorized : CommunicationErrorException
	{
		public Unauthorized(string message) : base(message)
		{
		}
	}
}