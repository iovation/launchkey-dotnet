using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class DirectoryV3SessionsListPostResponse
	{
		[JsonProperty("sessions")]
		public List<Session> Sessions { get; set; }

		public DirectoryV3SessionsListPostResponse(List<Session> sessions)
		{
			Sessions = sessions;
		}

		public class Session
		{
			[JsonProperty("service_id")]
			public Guid ServiceId { get; set; }

			[JsonProperty("service_name")]
			public string ServiceName { get; set; }

			[JsonProperty("service_icon")]
			public string ServiceIcon { get; set; }

			[JsonProperty("auth_request")]
			public Guid? AuthRequest { get; set; }

			[JsonProperty("date_created")]
			public DateTime Created { get; set; }
		}
	}
}