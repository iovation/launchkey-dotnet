using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a JWT-related error occurs
	/// </summary>
	[Serializable]
	public class JwtError : BaseException
	{
		public JwtError(string message) : base(message) { }
		public JwtError(string message, Exception innerException) : base(message, innerException) { }
	}
}