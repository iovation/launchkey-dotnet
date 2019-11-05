using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

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

        /// <summary>
        /// Returns the Transport object that can be used in the transport for
        /// sending to the LaunchKey API
        /// </summary>
        /// <returns>Returns this objects representation to Sdk.Transport.Domain.IPolicy</returns>
        public TransportDomain.IPolicy ToTransport()
        {
            List<TransportDomain.IFence> fences = new List<TransportDomain.IFence>();
            foreach (IFence fence in Fences)
            {
                fences.Add(fence.ToTransport());
            }

            /*
                * These values are explcitly set to null because of the shared JSON serialization with nested Policies and root Policies
                * API does not accept these keys in nested policies so we have to set it to null so it does not serialize
                * These values are verified in the constructor to not be True
            */
            TransportDomain.IPolicy insidePolicy = Inside.ToTransport();
            insidePolicy.DenyEmulatorSimulator = null;
            insidePolicy.DenyRootedJailbroken = null;

            TransportDomain.IPolicy outsidePolicy = Outside.ToTransport();
            outsidePolicy.DenyEmulatorSimulator = null;
            outsidePolicy.DenyRootedJailbroken = null;

            return new TransportDomain.ConditionalGeoFencePolicy(
                inside: insidePolicy,
                outside: outsidePolicy,
                denyRootedJailbroken: DenyRootedJailbroken,
                denyEmulatorSimulator: DenyEmulatorSimulator,
                fences: fences
            );
        }
    }
}
