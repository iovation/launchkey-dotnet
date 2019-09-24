using System.Collections.Generic;
using static iovation.LaunchKey.Sdk.Transport.Domain.AuthPolicy;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class LegacyPolicy : IPolicy
    {

        public bool? DenyRootedJailbroken { get; }
        public bool? DenyEmulatorSimulator { get; } = false;
        public List<IFence> Fences { get; }
        public int? Amount { get; }
        public bool InherenceRequired { get; }
        public bool KnowledgeRequired { get; }
        public bool PossessionRequired { get; }
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
