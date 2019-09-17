using Newtonsoft.Json;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class MethodAmountPolicy : IPolicy
    {
        public string Type { get; set; }

        [JsonProperty("deny_rooted_jailbroken")]
        public bool? DenyRootedJailbroken {get; set; }

        [JsonProperty("deny_emulator_simulator")]
        public bool? DenyEmulatorSimulator {get; set; }

        [JsonProperty("fences")]
        public List<IFence> Fences { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        public MethodAmountPolicy(bool denyRootedJailbroken,
            bool denyEmulatorSimulator, List<IFence> fences, double amount)
        {
            Amount = amount;
            Fences = fences;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            DenyRootedJailbroken = denyRootedJailbroken; 
        }

    }
}