using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a 403 occurs
	/// </summary>
	[Serializable]
	public class Unauthorized : CommunicationErrorException
	{
		public Unauthorized(string message) : base(message)
		{
		}
	}
}