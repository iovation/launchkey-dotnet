using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class OrganizationV3DirectoriesPatchRequest
	{
		[JsonProperty("directory_id")]
		public Guid DirectoryId { get; }

		[JsonProperty("active")]
		public bool Active { get; }

		[JsonProperty("android_key")]
		public string AndroidKey { get; }

		[JsonProperty("ios_p12")]
		public string IosP12 { get; }

		public OrganizationV3DirectoriesPatchRequest(Guid directoryId, bool active, string androidKey, string iosP12)
		{
			DirectoryId = directoryId;
			Active = active;
			AndroidKey = androidKey;
			IosP12 = iosP12;
		}
	}
}