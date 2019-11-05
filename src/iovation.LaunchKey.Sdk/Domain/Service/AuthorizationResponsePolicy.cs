using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Domain.Service
{
    public class AuthorizationResponsePolicy
    {
        /// <summary>
        /// The policy type requirement of this policy
        /// </summary>
        public Requirement? Requirement { get; }

        /// <summary>
        /// The amount of factors this policy required
        /// </summary>
        public int? Amount { get; }

        /// <summary>
        /// A list of Fences this policy required
        /// </summary>
        public List<IFence> Fences { get; }

        /// <summary>
        /// Whether the type Inherence was required
        /// </summary>
        public bool? InherenceRequired { get; }

        /// <summary>
        /// Whether the type Knowledge was required
        /// </summary>
        public bool? KnowledgeRequired { get; }

        /// <summary>
        /// Whether the type Possession was required
        /// </summary>
        public bool? PossessionRequired { get; }

        /// <summary>
        /// Creates a representation of the last policy requirement executed on the device
        /// </summary>
        /// <param name="requirement"></param>
        /// <param name="amount"></param>
        /// <param name="fences"></param>
        /// <param name="inherenceRequired"></param>
        /// <param name="knowledgeRequired"></param>
        /// <param name="possessionRequired"></param>
        public AuthorizationResponsePolicy(Requirement? requirement, int? amount = null, List<IFence> fences = null, bool? inherenceRequired = null, bool? knowledgeRequired = null, bool? possessionRequired = null)
        {
            Requirement = requirement;
            Amount = amount;
            Fences = fences ?? new List<IFence>();
            InherenceRequired = inherenceRequired;
            KnowledgeRequired = knowledgeRequired;
            PossessionRequired = possessionRequired;
        }
    }
}
