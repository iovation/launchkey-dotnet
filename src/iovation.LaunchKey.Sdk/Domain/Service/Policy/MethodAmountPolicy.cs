using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents a method amount Authorization Policy
    /// </summary>
    public class MethodAmountPolicy : IPolicy
    {
        /// <summary>
        /// The amount of factors required for the Authorization Request to be valid
        /// </summary>
        public Double Amount { get; }

        /// <summary>
        /// Whether to allow or deny rooted or jailbroken devices
        /// </summary>
        public bool? DenyRootedJailbroken { get; }

        /// <summary>
        /// Whether to allow or deny emulator or simulator devices
        /// </summary>
        public bool? DenyEmulatorSimulator { get; }

        /// <summary>
        /// List containing any Fence objects for the Authorization Policy
        /// </summary>
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
