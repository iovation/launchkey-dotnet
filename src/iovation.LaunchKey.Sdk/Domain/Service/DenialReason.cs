using System;
using System.Collections.Generic;
using System.Text;

namespace iovation.LaunchKey.Sdk.Domain.Service
{
	/// <summary>
	/// Denial reason record. These are sent during an authorization request and presented to the
	/// user to select from when they deny the request.
	/// </summary>
	public class DenialReason
	{
		public DenialReason(string id, string reason, bool fraud)
		{
			Id = id;
			Reason = reason;
			Fraud = fraud;
		}

		/// <summary>
		/// ID of the reason
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Reason test for the reason
		/// </summary>
		public string Reason { get; }

		/// <summary>
		/// Is the reason fraud?
		/// </summary>
		public bool Fraud { get; }
	}
}
