using System;

namespace iovation.LaunchKey.Sdk.Domain.Directory
{
	/// <summary>
	/// Represents a device's linked state.
	/// </summary>
	public class DeviceStatus
	{
		/// <summary>
		/// A device that is in a 'LinkPending' state. This device has begun link but the user has not completed it. This device is not auth ready.
		/// </summary>
		public static readonly DeviceStatus LinkPending = new DeviceStatus(0, false, "LinkPending");

		/// <summary>
		/// A device that is linked to a user and ready to use.
		/// </summary>
		public static readonly DeviceStatus Linked = new DeviceStatus(1, true, "Linked");

		/// <summary>
		/// A device that is in the process of being unlinked from a user. This device is not auth ready.
		/// </summary>
		public static readonly DeviceStatus UnlinkPending = new DeviceStatus(2, false, "UnlinkPending");

		/// <summary>
		/// The numeric status code for this status
		/// </summary>
		public int StatusCode { get; }

		/// <summary>
		/// Whether or not this state represents an active, usable state
		/// </summary>
		public bool IsActive { get; }

		/// <summary>
		/// A human-readable description of the state
		/// </summary>
		public string Text { get; }

		private DeviceStatus(int statusCode, bool isActive, string text)
		{
			StatusCode = statusCode;
			IsActive = isActive;
			Text = text;
		}

		public static DeviceStatus FromCode(int code)
		{
			if (code == 0) return LinkPending;
			if (code == 1) return Linked;
			if (code == 2) return UnlinkPending;
			throw new ArgumentException($"Invalid status code: {code}");
		}

		public override string ToString()
		{
			return Text;
		}
	}
}