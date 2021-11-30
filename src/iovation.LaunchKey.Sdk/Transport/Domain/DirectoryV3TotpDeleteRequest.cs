using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3TotpDeleteRequest
    {
        
        public DirectoryV3TotpDeleteRequest(string identifier)
        {
            Identifier = identifier;
        }
        
        [JsonProperty("identifier")]
        public string Identifier { get; }

    }
}