using System;

namespace iovation.LaunchKey.Sdk.Domain.Directory
{
	/// <summary>
	/// Represents an active user session within a service
	/// </summary>
	public class Session
	{
		/// <summary>
		/// The Service ID for which this session belongs
		/// </summary>
		public Guid ServiceId { get; }

		/// <summary>
		/// The name of the service for which this session belongs
		/// </summary>
		public string ServiceName { get; }

		/// <summary>
		/// An HTTP URL of an image showing the image icon of the service for which this session belongs
		/// </summary>
		public string ServiceIcon { get; }

		/// <summary>
		/// The originating authentication request ID.
		/// </summary>
		public Guid? AuthRequest { get; }

		/// <summary>
		/// The date this session began
		/// </summary>
		public DateTime Created { get; }

		public Session(Guid serviceId, string serviceName, string serviceIcon, Guid? authRequest, DateTime created)
		{
			ServiceId = serviceId;
			ServiceName = serviceName;
			ServiceIcon = serviceIcon;
			AuthRequest = authRequest;
			Created = created;
		}
	}
}