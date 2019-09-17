using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public interface IPolicy
    {
        Boolean DenyRootedJailbroken { get; }
        Boolean DenyEmulatorSimulator { get; }
        List<IFence> Fences { get; }
    }
}
