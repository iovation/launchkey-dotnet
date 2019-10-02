using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class FactorsPolicy : IPolicy
    {
        public string Type { get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_rooted_jailbroken", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DenyRootedJailbroken {get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_emulator_simulator", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DenyEmulatorSimulator {get; set; }

        [JsonProperty("fences")]
        public List<TransportFence> Fences { get; set; }

        [JsonProperty("factors")]
        public List<string> Factors { get; set; }

        public FactorsPolicy(
            List<string> factors,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false, 
            List<TransportFence> fences = null
            )
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<TransportFence>();
            Factors = factors ?? new List<string>();
            Type = "FACTORS";
        }


    }
}