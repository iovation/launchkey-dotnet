using System;

namespace iovation.LaunchKey.Sdk.Domain.Directory
{
	/// <summary>
	/// A linked device
	/// </summary>
	public class Device
	{
		/// <summary>
		/// The unique ID of this device
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// The name the user provided when linking the device
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The linking status of this device (Pending, Linked, UnlinkPending)
		/// </summary>
		public DeviceStatus Status { get; }

		/// <summary>
		/// The type of device (iOS, Android, etc.)
		/// </summary>
		public string Type { get; }

		/// <summary>
		/// The date this device was linked
		/// </summary>
		public DateTime Created { get; }

		/// <summary>
		/// The date this device was updated
		/// </summary>
		public DateTime Updated { get; }

		public Device(string id, string name, DeviceStatus status, string type, DateTime created, DateTime updated)
		{
			Id = id;
			Name = name;
			Status = status;
			Type = type;
			Created = created;
			Updated = updated;
		}
	}
}