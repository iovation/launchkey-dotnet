using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class FactorsPolicy : IPolicy
    {

        public bool RequireKnowledgeFactor { get; }
        public bool RequirePossessionFactor { get; }
        public bool RequireInherenceFactor { get; }
        public bool? DenyRootedJailbroken { get; }
        public bool? DenyEmulatorSimulator { get; }
        public List<IFence> Fences { get; }

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
