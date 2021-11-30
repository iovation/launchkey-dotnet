using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ServiceV3TotpPostRequest
    {
        public ServiceV3TotpPostRequest(string identifier, string otp)
        {
            Identifier = identifier;
            Otp = otp;
        }
        
        [JsonProperty("identifier")]
        public string Identifier { get; }

        [JsonProperty("otp")]
        public string Otp { get; }
    }
}