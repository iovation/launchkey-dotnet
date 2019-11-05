using System.ComponentModel;
using Newtonsoft.Json;
using JsonSubTypes;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    [JsonConverter(typeof(JsonSubtypes), "Type")]
    [JsonSubtypes.KnownSubType(typeof(GeoCircleFence), "GEO_CIRCLE")]
    [JsonSubtypes.KnownSubType(typeof(TerritoryFence), "TERRITORY")]
    [JsonSubtypes.FallBackSubType(typeof(GeoCircleFence))]
    public interface IFence
    {
        [DefaultValue("GEO_CIRCLE")]
        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Populate)]
        string Type { get; set; }

        Sdk.Domain.Service.Policy.IFence FromTransport();
    }
}