using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents a Conditional Geofence Authorization Policy
    /// </summary>
    public class ConditionalGeoFencePolicy : IPolicy
    {
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

        /// <summary>
        /// The Policy object that corresponds to devices within the specified Fences
        /// </summary>
        public IPolicy Inside { get; }

        /// <summary>
        /// The Policy object that corresponds to devices that are not within the specified Fences
        /// </summary>
        public IPolicy Outside { get; }

        /// <summary>
        /// Initializes object that represents a Conditional Geofence Authorization Policy
        /// Inside, Outside, and Fences must be supplied.
        /// Both Inside and Outside policies cannot have DenyEmulatorSimulator or DenyRootedJailbroken set to true and cannot set fences
        /// Fences must have at least one value
        /// </summary>
        /// <param name="inside"></param>
        /// <param name="outside"></param>
        /// <param name="fences"></param>
        /// <param name="denyRootedJailbroken"></param>
        /// <param name="denyEmulatorSimulator"></param>
        public ConditionalGeoFencePolicy(
            IPolicy inside,
            IPolicy outside,
            List<IFence> fences,
            bool? denyRootedJailbroken = false, 
            bool? denyEmulatorSimulator = false
            )
        {

            if(!(outside is MethodAmountPolicy) && !(outside is FactorsPolicy))
            {
                throw new InvalidPolicyAttributes(
                    "Inside and Outside policies must be one of the following: " +
                    "[\"FactorsPolicy\", \"MethodAmountPolicy\"]"
                );
            }

            if (!(inside is MethodAmountPolicy) && !(inside is FactorsPolicy))
            {
                throw new InvalidPolicyAttributes(
                    "Inside and Outside policies must be one of the following: " +
                    "[\"FactorsPolicy\", \"MethodAmountPolicy\"]"
                );
            }

            if (outside.DenyEmulatorSimulator == true || inside.DenyEmulatorSimulator == true)
            {
                throw new InvalidPolicyAttributes(
                    "Setting DenyRootedJailbroken is not allowed on Inside " +
                    "or Outside Policy objects"
                );
            }

            if (outside.DenyRootedJailbroken == true || inside.DenyRootedJailbroken == true)
            {
                throw new InvalidPolicyAttributes(
                    "Setting DenyRootedJailbroken is not allowed on Inside " +
                    "or Outside Policy objects"
                );
            }

            if(outside.Fences != null)
            {
                if(outside.Fences.Count != 0)
                {
                    throw new InvalidPolicyAttributes(
                        "Fences are not allowed on Inside or Outside Policy objects"
                    );
                }
            }

            if(inside.Fences != null)
            {
                if(inside.Fences.Count != 0)
                {
                    throw new InvalidPolicyAttributes(
                        "Fences are not allowed on Inside or Outside Policy objects"
                    );
                }
            }

            Inside = inside;
            Outside = outside;
            Fences = fences ?? new List<IFence>();
            DenyRootedJailbroken = denyRootedJailbroken ?? false;
            DenyEmulatorSimulator = denyEmulatorSimulator ?? false;
        }
    }
}
