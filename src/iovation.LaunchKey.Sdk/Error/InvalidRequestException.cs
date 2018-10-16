using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Thrown when a request is rejected by the LaunchKey API servers, or the nature of the request is deemed invalid by the SDK client code.
	/// </summary>
	[Serializable]
	public class InvalidRequestException : CommunicationErrorException
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

		public static InvalidRequestException FromErrorCode(string errorCode, string errorMessage)
		{
			switch (errorCode)
			{
				case "ARG-001": return new InvalidParameters(errorMessage, null, errorCode);
				case "ARG-002": return new InvalidRoute(errorMessage, null, errorCode);
				case "SVC-001": return new ServiceNameTaken(errorMessage, null, errorCode);
				case "SVC-002": return new InvalidPolicyInput(errorMessage, null, errorCode);
				case "SVC-003": return new PolicyFailure(errorMessage, null, errorCode);
				case "SVC-004": return new ServiceNotFound(errorMessage, null, errorCode);
				case "DIR-001": return new InvalidDirectoryIdentifier(errorMessage, null, errorCode);
				case "KEY-001": return new InvalidPublicKey(errorMessage, null, errorCode);
				case "KEY-002": return new PublicKeyAlreadyInUse(errorMessage, null, errorCode);
				case "KEY-003": return new PublicKeyDoesNotExist(errorMessage, null, errorCode);
				case "KEY-004": return new LastRemainingKey(errorMessage, null, errorCode);
				case "ORG-003": return new DirectoryNameInUse(errorMessage, null, errorCode);
				case "ORG-005": return new LastRemainingSDKKey(errorMessage, null, errorCode);
				case "ORG-006": return new InvalidSDKKey(errorMessage, null, errorCode);
				default: return new InvalidRequestException(errorMessage, null, errorCode);
			}
		}
	}
}