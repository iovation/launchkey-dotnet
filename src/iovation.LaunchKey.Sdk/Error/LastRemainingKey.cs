using System;

namespace iovation.LaunchKey.Sdk.Error
{
	public class LastRemainingKey : InvalidRequestException
	{
		public LastRemainingKey(string message) : base(message)
		{
		}

		public LastRemainingKey(string message, Exception innerException) : base(message, innerException)
		{
		}

		public LastRemainingKey(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}