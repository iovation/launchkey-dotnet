using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// An interface that all Policy objects implement
    /// </summary>
    public interface IPolicy
    {
        Boolean? DenyRootedJailbroken { get; }
        Boolean? DenyEmulatorSimulator { get; }
        List<IFence> Fences { get; }
    }
}
