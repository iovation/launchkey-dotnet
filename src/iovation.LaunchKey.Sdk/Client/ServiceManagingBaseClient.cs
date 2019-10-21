using System.Collections.Generic;
using System.Diagnostics;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport.Domain;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;

namespace iovation.LaunchKey.Sdk.Client
{
    public class ServiceManagingBaseClient
    {
        internal static DomainPolicy.IPolicy GetDomainPolicyFromTransportPolicy(IPolicy policy)
        {
            DomainPolicy.IPolicy returnPolicy = null;
            if(policy.Type == null || policy.Type == "LEGACY")
            {
                AuthPolicy convertedLegacyPolicy = (AuthPolicy)policy;
                //Grab a parsed object to save having to reimplement policy logic
                ServicePolicy parsedLegacyPolicy = ServicePolicy.FromTransport(convertedLegacyPolicy);
                List<DomainPolicy.IFence> convertedGeoFences = new List<DomainPolicy.IFence>();
                List<AuthPolicy.TimeFence> convertedTimeFences = new List<AuthPolicy.TimeFence>();
                foreach (var factor in convertedLegacyPolicy.Factors)
                {
                    if (factor.Factor == AuthPolicy.FactorType.Geofence)
                    {
                        foreach (var location in factor.Attributes.Locations)
                            convertedGeoFences.Add(
                                new DomainPolicy.GeoCircleFence(
                                    name: location.Name,
                                    latitude: location.Latitude,
                                    longitude: location.Longitude,
                                    radius: location.Radius
                                )
                            );
                    }
                    else if (factor.Factor == AuthPolicy.FactorType.TimeFence)
                    {
                        convertedTimeFences = factor.Attributes.TimeFences;
                    }
                }



                return new DomainPolicy.LegacyPolicy(
                    fences: convertedGeoFences,
                    denyRootedJailbroken: parsedLegacyPolicy.JailbreakDetection,
                    amount: parsedLegacyPolicy.RequiredFactors,
                    inherenceRequired: parsedLegacyPolicy.RequireInherenceFactor,
                    knowledgeRequired: parsedLegacyPolicy.RequireKnowledgeFactor,
                    possessionRequired: parsedLegacyPolicy.RequirePossessionFactor,
                    timeRestrictions: convertedTimeFences
                );
            }
            else if (policy.Type == "COND_GEO")
            {
                ConditionalGeoFencePolicy convertedPolicy = (ConditionalGeoFencePolicy)policy;
                DomainPolicy.IPolicy inside = GetDomainPolicyFromTransportPolicy(convertedPolicy.Inside);
                DomainPolicy.IPolicy outside = GetDomainPolicyFromTransportPolicy(convertedPolicy.Outside);
                List<DomainPolicy.IFence> fences = GetDomainFencesFromTransportFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.ConditionalGeoFencePolicy(
                    inside: inside,
                    outside: outside,
                    fences: fences,
                    denyRootedJailbroken: convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: convertedPolicy.DenyEmulatorSimulator
                );
            }
            else if (policy.Type == "FACTORS")
            {
                FactorsPolicy convertedPolicy = (FactorsPolicy)policy;
                List<DomainPolicy.IFence> fences = GetDomainFencesFromTransportFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.FactorsPolicy(
                    fences: fences,
                    denyRootedJailbroken: convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: convertedPolicy.DenyEmulatorSimulator,
                    requireKnowledgeFactor: convertedPolicy.Factors.Contains("KNOWLEDGE"),
                    requirePossessionFactor: convertedPolicy.Factors.Contains("POSSESSION"),
                    requireInherenceFactor: convertedPolicy.Factors.Contains("INHERENCE")
                );
            }
            else if (policy.Type == "METHOD_AMOUNT")
            {
                MethodAmountPolicy convertedPolicy = (MethodAmountPolicy)policy;
                List<DomainPolicy.IFence> fences = GetDomainFencesFromTransportFences(convertedPolicy.Fences);
                returnPolicy = new DomainPolicy.MethodAmountPolicy(
                    fences: fences,
                    amount: convertedPolicy.Amount,
                    denyRootedJailbroken: convertedPolicy.DenyRootedJailbroken,
                    denyEmulatorSimulator: convertedPolicy.DenyEmulatorSimulator
                );
            }
            else
            {
                throw new UnknownPolicyException(string.Format("The Policy {0} was not a known Policy type", policy.Type));
            }


            return returnPolicy;
        }

        internal static IPolicy GetTransportPolicyFromDomainPolicy(DomainPolicy.IPolicy policy, bool nestedPolicy = false)
        {
            IPolicy returnPolicy = null;

            bool? denyEmulatorSimulator = nestedPolicy ? null : policy.DenyEmulatorSimulator;
            bool? denyRootedJailbroken = nestedPolicy ? null : policy.DenyRootedJailbroken;

            if (policy.GetType() == typeof(DomainPolicy.LegacyPolicy))
            {
                List<AuthPolicy.Location> convertedFences = null;
                convertedFences = GetTransportLocationsFromDomainGeoCircleFences((policy as DomainPolicy.LegacyPolicy)?.Fences);

                returnPolicy = new AuthPolicy(
                    any: (policy as DomainPolicy.LegacyPolicy)?.Amount,
                    requireKnowledgeFactor: (policy as DomainPolicy.LegacyPolicy)?.KnowledgeRequired,
                    requireInherenceFactor: (policy as DomainPolicy.LegacyPolicy)?.InherenceRequired,
                    requirePossessionFactor: (policy as DomainPolicy.LegacyPolicy)?.PossessionRequired,
                    deviceIntegrity: denyRootedJailbroken,
                    locations: convertedFences,
                    timeFences: (policy as DomainPolicy.LegacyPolicy)?.TimeRestrictions
                    );
            }
            else if (policy.GetType() == typeof(DomainPolicy.ConditionalGeoFencePolicy))
            {
                IPolicy inside = GetTransportPolicyFromDomainPolicy((policy as DomainPolicy.ConditionalGeoFencePolicy)?.Inside, true);
                IPolicy outside = GetTransportPolicyFromDomainPolicy((policy as DomainPolicy.ConditionalGeoFencePolicy)?.Outside, true);
                List<TransportFence> fences = GetTransportFencesFromDomainFences((policy as DomainPolicy.ConditionalGeoFencePolicy)?.Fences);
                returnPolicy = new ConditionalGeoFencePolicy(
                    inside: inside,
                    outside: outside,
                    fences: fences,
                    denyRootedJailbroken: denyRootedJailbroken,
                    denyEmulatorSimulator: denyEmulatorSimulator
                );
            }
            else if (policy.GetType() == typeof(DomainPolicy.FactorsPolicy))
            {
                List<TransportFence> fences = GetTransportFencesFromDomainFences((policy as DomainPolicy.FactorsPolicy)?.Fences);

                List<string> factors = new List<string>();
                if ((policy as DomainPolicy.FactorsPolicy).RequireInherenceFactor) factors.Add("INHERENCE");
                if ((policy as DomainPolicy.FactorsPolicy).RequireKnowledgeFactor) factors.Add("KNOWLEDGE");
                if ((policy as DomainPolicy.FactorsPolicy).RequirePossessionFactor) factors.Add("POSSESSION");

                returnPolicy = new FactorsPolicy(
                    denyRootedJailbroken: denyRootedJailbroken,
                    denyEmulatorSimulator: denyEmulatorSimulator,
                    fences: fences,
                    factors: factors
                );
            }
            else if (policy.GetType() == typeof(DomainPolicy.MethodAmountPolicy))
            {
                List<TransportFence> fences = GetTransportFencesFromDomainFences((policy as DomainPolicy.MethodAmountPolicy)?.Fences);
                returnPolicy = new MethodAmountPolicy(
                    fences: fences,
                    amount: (policy as DomainPolicy.MethodAmountPolicy).Amount,
                    denyRootedJailbroken: denyRootedJailbroken,
                    denyEmulatorSimulator: denyEmulatorSimulator
                );
            }
            else
            {
                throw new UnknownPolicyException("Unknown Policy type");
            }

            return returnPolicy;
        }

        internal static List<DomainPolicy.IFence> GetDomainFencesFromTransportFences(List<TransportFence> fences)
        {
            List<DomainPolicy.IFence> convertedFences = new List<DomainPolicy.IFence>();
            if(fences != null)
            {
                foreach (TransportFence fence in fences)
                {
                    if (fence.Type == null || fence.Type == "GEO_CIRCLE")
                    {
                        convertedFences.Add(
                            new DomainPolicy.GeoCircleFence(
                                name: fence.Name,
                                latitude: fence.Latitude.Value,
                                longitude: fence.Longitude.Value,
                                radius: fence.Radius.Value
                            )
                        );
                    }
                    else if (fence.Type == "TERRITORY")
                    {
                        convertedFences.Add(
                            new DomainPolicy.TerritoryFence(
                                name: fence.Name,
                                country: fence.Country,
                                administrativeArea: fence.AdministrativeArea,
                                postalCode: fence.PostalCode
                            )
                        );
                    }
                    else
                    {
                        throw new UnknownFenceTypeException(string.Format("Unknown Fence type {0}", fence.GetType()));
                    }
                }
            }

            return convertedFences;

        }

        internal static List<TransportFence> GetTransportFencesFromDomainFences(List<DomainPolicy.IFence> fences)
        {
            List<TransportFence> convertedFences = new List<TransportFence>();

            if(fences != null)
            {
                foreach (DomainPolicy.IFence fence in fences)
                {
                    if (fence.GetType() == typeof(DomainPolicy.GeoCircleFence))
                    {
                        DomainPolicy.GeoCircleFence convertedFence = (DomainPolicy.GeoCircleFence)fence;
                        convertedFences.Add(
                            new TransportFence(
                                name: convertedFence.Name,
                                latitude: convertedFence.Latitude,
                                longitude: convertedFence.Longitude,
                                radius: convertedFence.Radius,
                                type: "GEO_CIRCLE"
                            )
                        );
                    }
                    else if (fence.GetType() == typeof(DomainPolicy.TerritoryFence))
                    {
                        DomainPolicy.TerritoryFence convertedFence = (DomainPolicy.TerritoryFence)fence;
                        convertedFences.Add(
                            new TransportFence(
                                name: convertedFence.Name,
                                country: convertedFence.Country,
                                administrativeArea: convertedFence.AdministrativeArea,
                                postalCode: convertedFence.PostalCode,
                                type: "TERRITORY"
                            )
                        );
                    }
                    else
                    {
                        throw new UnknownFenceTypeException(string.Format("Unknown Fence type {0}", fence.GetType()));
                    }
                }
            }

            return convertedFences;

        }

        internal static List<AuthPolicy.Location> GetTransportLocationsFromDomainGeoCircleFences(List<DomainPolicy.IFence> fences)
        {
            List<AuthPolicy.Location> locations = new List<AuthPolicy.Location>();
            if(fences != null)
            {
                foreach (DomainPolicy.IFence fence in fences)
                {
                    if (fence.GetType() == typeof(DomainPolicy.GeoCircleFence))
                    {
                        locations.Add(new AuthPolicy.Location(
                            (fence as DomainPolicy.GeoCircleFence).Name,
                            (fence as DomainPolicy.GeoCircleFence).Radius,
                            (fence as DomainPolicy.GeoCircleFence).Latitude,
                            (fence as DomainPolicy.GeoCircleFence).Longitude
                        ));
                    }
                    else
                    {
                        Trace.TraceWarning($"A Fence besides GeoCircleFence was present while using legacy functionality. This fence has been skipped from being processed.");
                    }
                }
            }
            return locations;
        }

        internal static ServicePolicy GetDomainServicePolicyFromDomainLegacyPolicy(DomainPolicy.LegacyPolicy legacyPolicy)
        {
            List<AuthPolicy.Location> convertedLocations = GetTransportLocationsFromDomainGeoCircleFences(legacyPolicy.Fences);

            bool? knowledgeRequired = legacyPolicy.KnowledgeRequired;
            bool? possessionRequired = legacyPolicy.PossessionRequired;
            bool? inherenceRequired = legacyPolicy.InherenceRequired;

            if (legacyPolicy.InherenceRequired == false && legacyPolicy.KnowledgeRequired == false && legacyPolicy.PossessionRequired == false)
            {
                knowledgeRequired = null;
                possessionRequired = null;
                inherenceRequired = null;
            }

            int? amount = legacyPolicy.Amount;
            if (amount == 0)
                amount = null;

            bool? denyRootedJailbroken = legacyPolicy.DenyRootedJailbroken;
            if (legacyPolicy.DenyRootedJailbroken == false)
                denyRootedJailbroken = null;

            AuthPolicy convertedPolicy = new AuthPolicy(
                amount,
                knowledgeRequired,
                inherenceRequired,
                possessionRequired,
                denyRootedJailbroken,
                convertedLocations,
                legacyPolicy.TimeRestrictions
                );
            return ServicePolicy.FromTransport(convertedPolicy);
        }
    }
}
