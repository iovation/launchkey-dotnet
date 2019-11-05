using System.ComponentModel;
using Newtonsoft.Json;
using JsonSubTypes;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(FactorsPolicy), "FACTORS")]
    [JsonSubtypes.KnownSubType(typeof(MethodAmountPolicy), "METHOD_AMOUNT")]
    [JsonSubtypes.KnownSubType(typeof(ConditionalGeoFencePolicy), "COND_GEO")]
    [JsonSubtypes.FallBackSubType(typeof(AuthPolicy))]
    public interface IPolicy
    {
        [DefaultValue("LEGACY")]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
        string Type { get; set; }

        bool? DenyEmulatorSimulator { get; set; }
        bool? DenyRootedJailbroken { get; set; }

        Sdk.Domain.Service.Policy.IPolicy FromTransport();
    }
}