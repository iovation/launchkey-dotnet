using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class KeysPostResponse
	{
		[JsonProperty("key_id")]
		public string Id { get; set; }
	}
}