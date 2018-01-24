using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class ServiceV3SessionsDeleteRequest
	{
		[JsonProperty("username")]
		public string Username { get; }

		public ServiceV3SessionsDeleteRequest(string username)
		{
			Username = username;
		}
	}
}