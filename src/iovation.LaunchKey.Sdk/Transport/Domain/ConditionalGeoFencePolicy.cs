using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ConditionalGeoFencePolicy : IPolicy
    {
        public string Type { get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_rooted_jailbroken")]
        public bool? DenyRootedJailbroken { get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_emulator_simulator")]
        public bool? DenyEmulatorSimulator { get; set; }

        [JsonProperty("fences")]
        public List<TransportFence> Fences { get; set; }

        [JsonProperty("inside")]
        public IPolicy Inside { get; set; }

        [JsonProperty("outside")]
        public IPolicy Outside { get; set; }

        public ConditionalGeoFencePolicy(
            IPolicy inside,
            IPolicy outside,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false,
            List<TransportFence> fences = null
            )
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<TransportFence>();
            Inside = inside;
            Outside = outside;
            Type = "COND_GEO";
        }

    }
}