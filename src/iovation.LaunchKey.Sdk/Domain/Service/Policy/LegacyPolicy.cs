using System.Collections.Generic;
using static iovation.LaunchKey.Sdk.Transport.Domain.AuthPolicy;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents an IPolicy implementation of the Legacy style policies
    /// </summary>
    public class LegacyPolicy : IPolicy
    {
        /// <summary>
        /// Whether to allow or deny rooted or jailbroken devices
        /// </summary>
        public bool? DenyRootedJailbroken { get; }

        /// <summary>
        /// Whether to allow or deny emulator or simulator devices
        /// </summary>
        public bool? DenyEmulatorSimulator { get; } = false;

        /// <summary>
        /// List containing any Fence objects for the Authorization Policy
        /// </summary>
        public List<IFence> Fences { get; }

        /// <summary>
        /// The amount of factors required for the Authorization Request to be valid
        /// </summary>
        public int? Amount { get; }

        /// <summary>
        /// Whether an Inherence factor is required
        /// </summary>
        public bool InherenceRequired { get; }

        /// <summary>
        /// Whether a Knowledge factor is required
        /// </summary>
        public bool KnowledgeRequired { get; }

        /// <summary>
        /// Whether a Possession factor is required
        /// </summary>
        public bool PossessionRequired { get; }

        /// <summary>
        /// List of server-side time restrictions
        /// </summary>
        public List<TimeFence> TimeRestrictions {get;}

        public LegacyPolicy(
            List<IFence> fences,
            bool? denyRootedJailbroken = false,
            int? amount = 0,
            bool? inherenceRequired = false,
            bool? knowledgeRequired = false,
            bool? possessionRequired = false,
            List<TimeFence> timeRestrictions = null
            )
        {
            DenyRootedJailbroken = denyRootedJailbroken ?? false;
            Fences = fences ?? new List<IFence>();
            Amount = amount ?? 0;
            InherenceRequired = inherenceRequired ?? false;
            KnowledgeRequired = knowledgeRequired ?? false;
            PossessionRequired = possessionRequired ?? false;
            TimeRestrictions = timeRestrictions ?? new List<TimeFence>();
        }
    }
}
