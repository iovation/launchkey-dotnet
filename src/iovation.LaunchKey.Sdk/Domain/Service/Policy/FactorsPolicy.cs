using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class FactorsPolicy : IPolicy
    {

        public Boolean RequireKnowledgeFactor { get; }
        public Boolean RequirePossessionFactor { get; }
        public Boolean RequireInherenceFactor { get; }
        public Boolean? DenyRootedJailbroken { get; }
        public Boolean? DenyEmulatorSimulator { get; }
        public List<IFence> Fences { get; }

        public FactorsPolicy(
            List<IFence> fences,
            Boolean requireKnowledgeFactor = false,
            Boolean requirePossessionFactor = false,
            Boolean requireInherenceFactor = false,
            Boolean denyRootedJailbroken = false,
            Boolean denyEmulatorSimulator = false
            )
        {
            RequireKnowledgeFactor = requireKnowledgeFactor;
            RequirePossessionFactor = requirePossessionFactor;
            RequireInherenceFactor = requireInherenceFactor;
            DenyRootedJailbroken = denyRootedJailbroken;
            DenyEmulatorSimulator = denyEmulatorSimulator;
            Fences = fences;
        }
    }
}
