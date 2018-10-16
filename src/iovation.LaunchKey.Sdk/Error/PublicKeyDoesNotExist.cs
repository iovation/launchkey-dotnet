using System;

namespace iovation.LaunchKey.Sdk.Error
{
	public class PublicKeyDoesNotExist : InvalidRequestException
	{
		public PublicKeyDoesNotExist(string message) : base(message)
		{
		}

		public PublicKeyDoesNotExist(string message, Exception innerException) : base(message, innerException)
		{
		}

		public PublicKeyDoesNotExist(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}