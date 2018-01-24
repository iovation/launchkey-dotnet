using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class DirectoryV3DevicesPostResponse
	{
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("qrcode")]
		public string QrCode { get; set; }
	}
}