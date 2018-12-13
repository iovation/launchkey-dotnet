using System;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Exception raised when an authorization request is made for a User who has an existing authorization request
	/// currently in progress.
	/// </summary>
	[Serializable]
	public class AuthorizationInProgress : InvalidRequestException
	{
		/// <summary>
		/// Identifier of the existing Authorization Request that caused this exception
		/// </summary>
		public string AuthorizationRequestId { get; }

		/// <summary>
		/// Is the authorization in progress from the same Service requesting the new
		/// Authorization Request
		/// </summary>
		public bool FromSameService { get; }

		/// <summary>
		///  When the Authorization Request identified by authorizationRequestId will expire.
		/// </summary>
		public DateTime? Expires { get; }

		public AuthorizationInProgress(string message, Exception innerException, string errorCode, string authorizationrequestId, Boolean fromSameService, DateTime? expires) : base(message, innerException, errorCode)
		{
			AuthorizationRequestId = authorizationrequestId;
			FromSameService = fromSameService;
			Expires = expires;
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj) == false) return false;
			if (this.GetType() != obj.GetType()) return false;
			var exception = obj as AuthorizationInProgress;
			return (this.AuthorizationRequestId == exception.AuthorizationRequestId 
				&& this.FromSameService == exception.FromSameService 
				&& this.Expires == exception.Expires);
		}

		public override int GetHashCode()
		{
			int hashCode = base.GetHashCode() ^ GetType().GetHashCode();
			hashCode = hashCode ^ FromSameService.GetHashCode();
			if (AuthorizationRequestId != null) hashCode = hashCode ^ AuthorizationRequestId.GetHashCode();
			if (Expires != null) hashCode = hashCode ^ Expires.GetHashCode();
			return hashCode;
		}

		public override string ToString()
		{
			return $"{GetType().Name}{{message='{Message}', ErrorCode='{ErrorCode}', AuthorizationRequestId='{AuthorizationRequestId}', FromSameService={FromSameService}, Expires='{Expires}'}}";
		}
	}
}