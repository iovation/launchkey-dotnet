using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents a Factors based Authorization Policy
    /// </summary>
    public class FactorsPolicy : IPolicy
    {
        /// <summary>
        /// Whether to require a Knowledge factor in the Authorization Request
        /// </summary>
        public bool RequireKnowledgeFactor { get; }

        /// <summary>
        /// Whether to require a Possession factor in the Authorization Request
        /// </summary>
        public bool RequirePossessionFactor { get; }

        /// <summary>
        /// Whether to require an Inherence factor in the Authorization Request
        /// </summary>
        public bool RequireInherenceFactor { get; }

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
        /// Fences can be an empty list or null
        /// All other parameters default to false
        /// </summary>
        /// <param name="fences"></param>
        /// <param name="requireKnowledgeFactor"></param>
        /// <param name="requirePossessionFactor"></param>
        /// <param name="requireInherenceFactor"></param>
        /// <param name="denyRootedJailbroken"></param>
        /// <param name="denyEmulatorSimulator"></param>
        public FactorsPolicy(
            List<IFence> fences,
            bool requireKnowledgeFactor = false,
            bool requirePossessionFactor = false,
            bool requireInherenceFactor = false,
            bool? denyRootedJailbroken = false,
            bool? denyEmulatorSimulator = false
            )
        {
            RequireKnowledgeFactor = requireKnowledgeFactor;
            RequirePossessionFactor = requirePossessionFactor;
            RequireInherenceFactor = requireInherenceFactor;
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences ?? new List<IFence>();
        }
    }
}
