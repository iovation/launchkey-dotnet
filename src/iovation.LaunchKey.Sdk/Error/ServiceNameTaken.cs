using System;

namespace iovation.LaunchKey.Sdk.Error
{
	public class ServiceNameTaken : InvalidRequestException
	{
		public ServiceNameTaken(string message) : base(message)
		{
		}

		public ServiceNameTaken(string message, Exception innerException) : base(message, innerException)
		{
		}

		public ServiceNameTaken(string message, Exception innerException, string errorCode) : base(message, innerException, errorCode)
		{
		}
	}
}