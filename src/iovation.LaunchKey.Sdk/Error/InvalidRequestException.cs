using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a request is rejected by the LaunchKey API servers, or the nature of the request is deemed invalid by the SDK client code.
	/// </summary>
	[Serializable]
	public class InvalidRequestException : BaseException
	{
		public InvalidRequestException(string message) : base(message)
		{
		}

		public InvalidRequestException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public InvalidRequestException(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}