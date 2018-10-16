using System;

namespace iovation.LaunchKey.Sdk.Error
{
	public class DirectoryNameInUse : InvalidRequestException
	{
		public DirectoryNameInUse(string message) : base(message)
		{
		}

		public DirectoryNameInUse(string message, Exception innerException) : base(message, innerException)
		{
		}

		public DirectoryNameInUse(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}