using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class MethodAmountPolicy : IPolicy
    {

        public Double Amount { get; }
        public Boolean DenyRootedJailbroken { get; }
        public Boolean DenyEmulatorSimulator { get; }
        public List<IFence> Fences { get; }

        public MethodAmountPolicy(
            List<IFence> fences,
            Double amount = 0,
            Boolean denyRootedJailbroken = false,
            Boolean denyEmulatorSimulator = false
            )
        {
            Amount = amount;
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences;
        }
    }
}
