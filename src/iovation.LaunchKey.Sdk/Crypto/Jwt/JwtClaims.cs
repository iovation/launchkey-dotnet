using System;
using iovation.LaunchKey.Sdk.Json;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Crypto.Jwt
{
	public class JwtClaimsResponse
	{
		[JsonProperty("func")]
		public string ContentHashAlgorithm { get; set; }

		[JsonProperty("hash")]
		public string ContentHash { get; set; }

		[JsonProperty("status")]
		public int StatusCode { get; set; }

		[JsonProperty("cache")]
		public string CacheControlHeader { get; set; }

		[JsonProperty("location")]
		public string LocationHeader { get; set; }
	}

	public class JwtClaimsRequest
	{
		[JsonProperty("func")]
		public string ContentHashAlgorithm { get; set; }

		[JsonProperty("hash")]
		public string ContentHash { get; set; }

		[JsonProperty("meth")]
		public string Method { get; set; }

		[JsonProperty("path")]
		public string Path { get; set; }
	}



	public class JwtClaims
	{
		[JsonProperty("jti")]
		public string TokenId { get; set; }

		[JsonProperty("iss")]
		public string Issuer { get; set; }

		[JsonProperty("sub")]
		public string Subject { get; set; }

		[JsonProperty("aud")]
		public string Audience { get; set; }

		[JsonProperty("iat")]
		[JsonConverter(typeof(UnixTimestampJsonDateConverter))]
		public DateTime IssuedAt { get; set; }

		[JsonProperty("nbf")]
		[JsonConverter(typeof(UnixTimestampJsonDateConverter))]
		public DateTime NotBefore { get; set; }

		[JsonProperty("exp")]
		[JsonConverter(typeof(UnixTimestampJsonDateConverter))]
		public DateTime ExpiresAt { get; set; }

		[JsonProperty("response")]
		public JwtClaimsResponse Response { get; set; }

		[JsonProperty("request")]
		public JwtClaimsRequest Request { get; set; }

		public static JwtClaims FromJson(string json)
		{
			return JsonConvert.DeserializeObject<JwtClaims>(json);
		}
	}
}