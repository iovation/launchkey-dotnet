using System.Collections.Generic;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3TotpPostResponse
    {
        [JsonProperty("secret")]
        public string Secret { get; }
        
        [JsonProperty("algorithm")]
        public string Algorithm { get; }
        
        [JsonProperty("period")]
        public int Period { get; }
        
        [JsonProperty("digits")]
        public int Digits { get; }
        
        public DirectoryV3TotpPostResponse(string secret, string algorithm, int period, int digits)
        {
            Secret = secret;
            Algorithm = algorithm;
            Period = period;
            Digits = digits;
        }
    }
}