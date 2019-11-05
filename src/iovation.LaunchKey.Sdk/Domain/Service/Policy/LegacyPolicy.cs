using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

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
        public bool? DenyEmulatorSimulator { get; } = null;

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
        public List<TimeFence> TimeRestrictions { get; }

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

        /// <summary>
        /// Returns the Transport object that can be used in the transport for
        /// sending to the LaunchKey API
        /// </summary>
        /// <returns>Returns this objects representation to Sdk.Transport.Domain.AuthPolicy</returns>
        public TransportDomain.IPolicy ToTransport()
        {
            List<TransportDomain.AuthPolicy.Location> fences = new List<TransportDomain.AuthPolicy.Location>();
            List<TransportDomain.AuthPolicy.TimeFence> timeFences = new List<TransportDomain.AuthPolicy.TimeFence>();
            foreach (IFence fence in Fences)
            {
                if (fence.GetType() == typeof(GeoCircleFence))
                {
                    fences.Add(new TransportDomain.AuthPolicy.Location(
                        (fence as GeoCircleFence).Name,
                        (fence as GeoCircleFence).Radius,
                        (fence as GeoCircleFence).Latitude,
                        (fence as GeoCircleFence).Longitude
                    ));
                }
                else
                {
                    Trace.TraceWarning($"A Fence besides GeoCircleFence was present while using legacy functionality. This fence has been skipped from being processed.");
                }
            }

            foreach(TimeFence timeFence in TimeRestrictions)
            {
                timeFences.Add(
                    new TransportDomain.AuthPolicy.TimeFence(
                        name: timeFence.Name,
                        days: timeFence.Days.Select(day => day.ToString()).ToList(),
                        startHour: timeFence.StartHour,
                        endHour: timeFence.EndHour,
                        startMinute: timeFence.StartMinute,
                        endMinute: timeFence.EndMinute,
                        timeZone: timeFence.TimeZone
                    )
                );
            }

            int? amount = Amount;

            if (amount == 0)
                amount = null;

            bool? deviceIntegrity = DenyRootedJailbroken;
            if (deviceIntegrity == false)
                deviceIntegrity = null;

            bool? knowledgeRequired = KnowledgeRequired;
            bool? possessionRequired = PossessionRequired;
            bool? inherenceRequired = InherenceRequired;

            if (InherenceRequired == false && KnowledgeRequired == false && PossessionRequired == false)
            {
                knowledgeRequired = null;
                possessionRequired = null;
                inherenceRequired = null;
            }

            return new TransportDomain.AuthPolicy(
                any: amount,
                requireKnowledgeFactor: knowledgeRequired,
                requireInherenceFactor: inherenceRequired,
                requirePossessionFactor: possessionRequired,
                deviceIntegrity: deviceIntegrity,
                locations: fences,
                timeFences: timeFences
            );
        }
    }
}
