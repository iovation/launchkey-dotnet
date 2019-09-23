using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class MethodAmountPolicy : IPolicy
    {

        public Double Amount { get; }
        public bool? DenyRootedJailbroken { get; }
        public bool? DenyEmulatorSimulator { get; }
        public List<IFence> Fences { get; }

        public MethodAmountPolicy(
            List<IFence> fences,
            Double amount = 0,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false
            )
        {
            Amount = amount;
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<IFence>();
        }
    }
}
