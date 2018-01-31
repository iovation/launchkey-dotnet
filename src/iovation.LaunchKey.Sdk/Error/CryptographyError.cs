using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Errors associated with cryptographic troubles; failing to load crypto libraries, failed signature checks or other crypto failures
	/// </summary>
	[Serializable]
	public class CryptographyError : BaseException
	{
		public CryptographyError(string message) : base(message)
		{
		}

		public CryptographyError(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}