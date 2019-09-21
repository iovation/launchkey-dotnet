using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class MethodAmountPolicy : IPolicy
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

        [JsonProperty("amount")]
        public double Amount { get; set; }

        public MethodAmountPolicy(
            double amount,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false,
            List<TransportFence> fences = null)
        {
            Amount = amount;
            Fences = fences ?? new List<TransportFence>();
            DenyEmulatorSimulator = denyEmulatorSimulator;
            DenyRootedJailbroken = denyRootedJailbroken;
            Type = "METHOD_AMOUNT";
        }

    }
}