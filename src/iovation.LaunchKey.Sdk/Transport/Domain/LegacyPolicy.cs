using Newtonsoft.Json;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class LegacyPolicy : IPolicy
    {
        public string Type { get; set; }

        public bool DenyRootedJailbroken { get; set; }
        public bool DenyEmulatorSimulator { get; set; } = false;
        public List<GeoCircleFence> Fences { get; set; }
        public int? Amount { get; set; }
        public bool InherenceRequired { get; set; }
        public bool KnowledgeRequired { get; set; }
        public bool PossessionRequired { get; set; }
        public List<AuthPolicy.TimeFence> TimeRestrictions { get; set; }

        public LegacyPolicy(List<GeoCircleFence> fences,
            List<AuthPolicy.TimeFence> timeRestrictions,
            bool denyRootedJailbroken = false,
            int? amount = 0,
            bool isInherence = false,
            bool isKnowledge = false,
            bool isPossession = false
            )
        {
            Fences = fences;
            TimeRestrictions = timeRestrictions;
            DenyRootedJailbroken = denyRootedJailbroken;
            Amount = amount;
            InherenceRequired = isInherence;
            KnowledgeRequired = isKnowledge;
            PossessionRequired = isPossession;
        }
    }
}