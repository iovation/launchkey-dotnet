using Newtonsoft.Json;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class FactorsPolicy : IPolicy
    {
        public string Type { get; set; }

        [JsonProperty("deny_rooted_jailbroken")]
        public bool? DenyRootedJailbroken {get; set; }

        [JsonProperty("deny_emulator_simulator")]
        public bool? DenyEmulatorSimulator {get; set; }

        [JsonProperty("fences")]
        public List<IFence> Fences { get; set; }

        [JsonProperty("factors")]
        public List<string> Factors { get; set; }

        public FactorsPolicy(
            bool? denyRootedJailbroken,
            bool? denyEmulatorSimulator, 
            List<IFence> fences,
            List<string> factors)
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences;
            Factors = factors;
        }


    }
}