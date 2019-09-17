using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class ConditionalGeoFencePolicy : IPolicy
    {
        public bool DenyRootedJailbroken { get; }
        public bool DenyEmulatorSimulator { get; }
        public List<IFence> Fences { get; }
        public IPolicy Inside { get; }
        public IPolicy Outside { get; }

        public ConditionalGeoFencePolicy(
            IPolicy inside,
            IPolicy outside,
            List<IFence> fences,
            bool denyRootedJailbroken = false, 
            bool denyEmulatorSimulator = false
            )
        {

            if(!(outside is MethodAmountPolicy) || !(outside is FactorsPolicy))
            {
                throw new InvalidPolicyAttributes(
                    "Inside and Outside policies must be one of the following: " +
                    "[\"FactorsPolicy\", \"MethodAmountPolicy\"]"
                );
            }

            if (!(inside is MethodAmountPolicy) || !(inside is FactorsPolicy))
            {
                throw new InvalidPolicyAttributes(
                    "Inside and Outside policies must be one of the following: " +
                    "[\"FactorsPolicy\", \"MethodAmountPolicy\"]"
                );
            }

            if (outside.DenyEmulatorSimulator != false || inside.DenyEmulatorSimulator != false)
            {
                throw new InvalidPolicyAttributes(
                    "Setting DenyRootedJailbroken is not allowed on Inside " +
                    "or Outside Policy objects"
                );
            }

            if (outside.DenyRootedJailbroken != false || inside.DenyRootedJailbroken != false)
            {
                throw new InvalidPolicyAttributes(
                    "Setting DenyRootedJailbroken is not allowed on Inside " +
                    "or Outside Policy objects"
                );
            }

            if (outside.Fences.Count != 0 || inside.Fences.Count != 0)
            {
                throw new InvalidPolicyAttributes(
                    "Fences are not allowed on Inside or Outside Policy objects"
                );
            }

            Inside = inside;
            Outside = outside;
            Fences = fences ?? new List<IFence>();
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
        }
    }
}
