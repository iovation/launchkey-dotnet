using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

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
        public List<IFence> Fences { get; set; }

        [JsonProperty("factors")]
        public List<string> Factors { get; set; }

        public FactorsPolicy(
            List<string> factors,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false, 
            List<IFence> fences = null
            )
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<IFence>();
            Factors = factors ?? new List<string>();
            Type = "FACTORS";
        }

        public DomainPolicy.IPolicy FromTransport()
        {
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>();
            foreach (IFence fence in Fences)
            {
                fences.Add(fence.FromTransport());
            }

            return new DomainPolicy.FactorsPolicy(
                fences: fences,
                requireKnowledgeFactor: Factors.Contains("KNOWLEDGE"),
                requireInherenceFactor: Factors.Contains("INHERENCE"),
                requirePossessionFactor: Factors.Contains("POSSESSION"),
                denyEmulatorSimulator: DenyEmulatorSimulator,
                denyRootedJailbroken: DenyRootedJailbroken                
            );
        }
    }
}