using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class DirectoryV3DevicesDeleteRequest
	{
		[JsonProperty("identifier")]
		public string Identifier { get; }

		[JsonProperty("device_id")]
		public Guid DeviceId { get; }

		public DirectoryV3DevicesDeleteRequest(string identifier, Guid deviceGuid)
		{
			Identifier = identifier;
			DeviceId = deviceGuid;
		}
	}
}