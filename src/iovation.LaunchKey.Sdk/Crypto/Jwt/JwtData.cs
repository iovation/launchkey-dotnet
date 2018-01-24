using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Crypto.Jwt
{
	public class JwtData
	{
		public string Issuer { get; }
		public string Subject { get; }
		public string Audience { get; }
		public string KeyId { get; }

		public JwtData(string issuer, string subject, string audience, string keyId)
		{
			Issuer = issuer;
			Subject = subject;
			Audience = audience;
			KeyId = keyId;
		}
	}
}