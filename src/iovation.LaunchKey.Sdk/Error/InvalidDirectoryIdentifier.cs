using System;

namespace iovation.LaunchKey.Sdk.Error
{
	public class InvalidDirectoryIdentifier : InvalidRequestException
	{
		public InvalidDirectoryIdentifier(string message) : base(message)
		{
		}

		public InvalidDirectoryIdentifier(string message, Exception innerException) : base(message, innerException)
		{
		}

		public InvalidDirectoryIdentifier(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}