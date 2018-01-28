using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when an authorization request has failed due to a timeout
	/// </summary>
	[Serializable]
	public class AuthorizationRequestTimedOutError : BaseException
	{
		public AuthorizationRequestTimedOutError() : base("Authorization timed out") { }
		public AuthorizationRequestTimedOutError(string message) : base(message) { }
		public AuthorizationRequestTimedOutError(string message, Exception innerException) : base(message, innerException) { }
		public AuthorizationRequestTimedOutError(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode) {}
	}
}