using System.ComponentModel;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public interface IPolicy
    {
        [DefaultValue("LEGACY")]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
         string Type { get; set; }
    }
}