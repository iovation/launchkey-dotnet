using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class ConditionalGeoFencePolicy : IPolicy
    {
        public string Type { get; set; }

        [JsonProperty("deny_rooted_jailbroken")]
        public bool? DenyRootedJailbroken { get; set; }

        [JsonProperty("deny_emulator_simulator")]
        public bool? DenyEmulatorSimulator { get; set; }

        [JsonProperty("fences")]
        public List<IFence> Fences { get; set; }

        [JsonProperty("inside")]
        public IPolicy Inside { get; set; }

        [JsonProperty("outside")]
        public IPolicy Outside { get; set; }

        public ConditionalGeoFencePolicy(bool? denyRootedJailbroken,
            bool? denyEmulatorSimulator, List<IFence> fences, IPolicy inside,
            IPolicy outside)
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences;
            Inside = inside;
            Outside = outside;
        }

    }
}