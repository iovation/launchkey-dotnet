using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

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
        public List<IFence> Fences { get; set; }

        [JsonProperty("inside")]
        public IPolicy Inside { get; set; }

        [JsonProperty("outside")]
        public IPolicy Outside { get; set; }

        public ConditionalGeoFencePolicy(
            IPolicy inside,
            IPolicy outside,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false,
            List<IFence> fences = null
            )
        {
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<IFence>();
            Inside = inside;
            Outside = outside;
            Type = "COND_GEO";
        }

        public DomainPolicy.IPolicy FromTransport()
        {
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>();
            foreach (IFence fence in Fences)
            {
                fences.Add(fence.FromTransport());
            }

            return new DomainPolicy.ConditionalGeoFencePolicy(
                    inside: Inside.FromTransport(),
                    outside: Outside.FromTransport(),
                    fences: fences,
                    denyRootedJailbroken: DenyRootedJailbroken,
                    denyEmulatorSimulator: DenyEmulatorSimulator
            );
        }
    }
}