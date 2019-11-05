using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class MethodAmountPolicy : IPolicy
    {
        public string Type { get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_rooted_jailbroken", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DenyRootedJailbroken { get; set; }

        [DefaultValue(false)]
        [JsonProperty("deny_emulator_simulator", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DenyEmulatorSimulator { get; set; }

        [JsonProperty("fences")]
        public List<IFence> Fences { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        public MethodAmountPolicy(
            double amount,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false,
            List<IFence> fences = null)
        {
            Amount = amount;
            Fences = fences ?? new List<IFence>();
            DenyEmulatorSimulator = denyEmulatorSimulator;
            DenyRootedJailbroken = denyRootedJailbroken;
            Type = "METHOD_AMOUNT";
        }

        public DomainPolicy.IPolicy FromTransport()
        {
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>();
            foreach (IFence fence in Fences)
            {
                fences.Add(fence.FromTransport());
            }

            return new DomainPolicy.MethodAmountPolicy(
                    fences: fences,
                    amount: Amount,
                    denyRootedJailbroken: DenyRootedJailbroken,
                    denyEmulatorSimulator: DenyEmulatorSimulator
            );
        }
    }
}