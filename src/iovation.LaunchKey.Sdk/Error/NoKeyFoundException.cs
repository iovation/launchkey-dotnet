using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when the API asks us to use a key we cannot retrieve.
	/// </summary>
	[Serializable]
	public class NoKeyFoundException : BaseException
	{
		public NoKeyFoundException(string message) : base(message)
		{
		}

		public NoKeyFoundException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public NoKeyFoundException(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}