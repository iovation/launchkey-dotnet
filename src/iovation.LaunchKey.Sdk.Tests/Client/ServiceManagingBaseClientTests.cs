using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Domain.Webhook;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using serviceDomain = iovation.LaunchKey.Sdk.Domain.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainPolicy = iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests.Client
{
    [TestClass]
    public class ServiceManagingBaseClientTests
    {
        [TestMethod]
        public void TestGetDomainPolicyFromTransport_Legacy()
        {
            AuthPolicy transportPolicy = new AuthPolicy(null, false, false, true, false, null, null);
            DomainPolicy.LegacyPolicy expected = new DomainPolicy.LegacyPolicy(
                null, false, null, false, false, true, null
                );

            DomainPolicy.LegacyPolicy actual = (DomainPolicy.LegacyPolicy)ServiceManagingBaseClient.GetDomainPolicyFromTransportPolicy(transportPolicy);
            Assert.AreEqual(expected.Amount, actual.Amount);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Fences, actual.Fences);
            Assert.AreEqual(expected.InherenceRequired, actual.InherenceRequired);
            Assert.AreEqual(expected.KnowledgeRequired, actual.KnowledgeRequired);
            Assert.AreEqual(expected.PossessionRequired, actual.PossessionRequired);
            Assert.AreEqual(expected.TimeRestrictions, actual.TimeRestrictions);
        }

        [TestMethod]
        public void TestGetDomainPolicyFromTransport_CondGeo()
        {
            MethodAmountPolicy insidePolicy = new MethodAmountPolicy(amount: 1);
            MethodAmountPolicy outsidePolicy = new MethodAmountPolicy(amount: 1);

            ConditionalGeoFencePolicy transportPolicy = new ConditionalGeoFencePolicy(
                inside: insidePolicy,
                outside: outsidePolicy,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: true,
                fences: null
                );

            DomainPolicy.MethodAmountPolicy expectedNestedPolicy = new DomainPolicy.MethodAmountPolicy(
                null, 1, null, null
                );
            DomainPolicy.ConditionalGeoFencePolicy expected = new DomainPolicy.ConditionalGeoFencePolicy(
                inside: expectedNestedPolicy,
                outside: expectedNestedPolicy,
                fences: null,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: true
                );

            DomainPolicy.ConditionalGeoFencePolicy actual = (DomainPolicy.ConditionalGeoFencePolicy)ServiceManagingBaseClient.GetDomainPolicyFromTransportPolicy(transportPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Fences.Count, actual.Fences.Count);

            DomainPolicy.MethodAmountPolicy actualInsidePolicy = (DomainPolicy.MethodAmountPolicy)actual.Inside;
            DomainPolicy.MethodAmountPolicy actualOutsidePolicy = (DomainPolicy.MethodAmountPolicy)actual.Outside;

            Assert.AreEqual(false, actualInsidePolicy.DenyRootedJailbroken);
            Assert.AreEqual(false, actualInsidePolicy.DenyEmulatorSimulator);
            Assert.AreEqual(0, actualInsidePolicy.Fences.Count);

            Assert.AreEqual(false, actualOutsidePolicy.DenyRootedJailbroken);
            Assert.AreEqual(false, actualOutsidePolicy.DenyEmulatorSimulator);
            Assert.AreEqual(0, actualOutsidePolicy.Fences.Count);
        }

        [TestMethod]
        public void TestGetDomainPolicyFromTransport_Factors()
        {
            FactorsPolicy transportPolicy = new FactorsPolicy(
                new List<string>() { "KNOWLEDGE"},
                true,
                true,
                null
            );

            DomainPolicy.FactorsPolicy expected = new DomainPolicy.FactorsPolicy(
                null, true, false, false, true, true
            );

            DomainPolicy.FactorsPolicy actual = (DomainPolicy.FactorsPolicy)ServiceManagingBaseClient.GetDomainPolicyFromTransportPolicy(transportPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(0, actual.Fences.Count);
            Assert.AreEqual(expected.RequireInherenceFactor, actual.RequireInherenceFactor);
            Assert.AreEqual(expected.RequireKnowledgeFactor, actual.RequireKnowledgeFactor);
            Assert.AreEqual(expected.RequirePossessionFactor, actual.RequirePossessionFactor);
        }

        [TestMethod]
        public void TestGetDomainPolicyFromTransport_MethodAmount()
        {
            MethodAmountPolicy transportPolicy = new MethodAmountPolicy(
                2,
                true,
                true,
                null
            );

            DomainPolicy.MethodAmountPolicy expected = new DomainPolicy.MethodAmountPolicy(
                null, 2, true, true
            );

            DomainPolicy.MethodAmountPolicy actual = (DomainPolicy.MethodAmountPolicy)ServiceManagingBaseClient.GetDomainPolicyFromTransportPolicy(transportPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(0, actual.Fences.Count);
            Assert.AreEqual(expected.Amount, actual.Amount);
        }

        [TestMethod]
        public void TestGetTransportPolicyFromDomain_Legacy()
        {
            DomainPolicy.LegacyPolicy domainPolicy = new DomainPolicy.LegacyPolicy(
                null, false, null, false, false, true, null
            );
            AuthPolicy expected = new AuthPolicy(null, false, false, true, false, null, null);

            AuthPolicy actual = (AuthPolicy)ServiceManagingBaseClient.GetTransportPolicyFromDomainPolicy(domainPolicy);
            Assert.AreEqual(expected.Factors[0].Attributes.FactorEnabled, actual.Factors[0].Attributes.FactorEnabled);
            Assert.AreEqual(expected.Factors[0].Attributes.Locations, actual.Factors[0].Attributes.Locations);
            Assert.AreEqual(expected.Factors[0].Attributes.TimeFences, actual.Factors[0].Attributes.TimeFences);
            Assert.AreEqual(expected.Factors[0].Factor, actual.Factors[0].Factor);
            Assert.AreEqual(expected.Factors[0].Priority, actual.Factors[0].Priority);
            Assert.AreEqual(expected.Factors[0].Requirement, actual.Factors[0].Requirement);
        }

        [TestMethod]
        public void TestGetTransportPolicyFromDomain_CondGeo()
        {
            MethodAmountPolicy insidePolicy = new MethodAmountPolicy(amount: 1);
            MethodAmountPolicy outsidePolicy = new MethodAmountPolicy(amount: 1);

            ConditionalGeoFencePolicy expected = new ConditionalGeoFencePolicy(
                inside: insidePolicy,
                outside: outsidePolicy,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: true,
                fences: null
                );

            DomainPolicy.MethodAmountPolicy nestedTransportPolicy = new DomainPolicy.MethodAmountPolicy(
                null, 1, null, null
                );
            DomainPolicy.ConditionalGeoFencePolicy domainPolicy = new DomainPolicy.ConditionalGeoFencePolicy(
                inside: nestedTransportPolicy,
                outside: nestedTransportPolicy,
                fences: null,
                denyRootedJailbroken: false,
                denyEmulatorSimulator: true
                );

            ConditionalGeoFencePolicy actual = (ConditionalGeoFencePolicy)ServiceManagingBaseClient.GetTransportPolicyFromDomainPolicy(domainPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Fences.Count, actual.Fences.Count);

            MethodAmountPolicy actualInsidePolicy = (MethodAmountPolicy)actual.Inside;
            MethodAmountPolicy actualOutsidePolicy = (MethodAmountPolicy)actual.Outside;

            Assert.AreEqual(null, actualInsidePolicy.DenyRootedJailbroken);
            Assert.AreEqual(null, actualInsidePolicy.DenyEmulatorSimulator);
            Assert.AreEqual(0, actualInsidePolicy.Fences.Count);

            Assert.AreEqual(null, actualOutsidePolicy.DenyRootedJailbroken);
            Assert.AreEqual(null, actualOutsidePolicy.DenyEmulatorSimulator);
            Assert.AreEqual(0, actualOutsidePolicy.Fences.Count);
        }

        [TestMethod]
        public void TestGetTransportPolicyFromDomain_Factors()
        {
            FactorsPolicy expected = new FactorsPolicy(
                new List<string>() { "KNOWLEDGE" },
                true,
                true,
                null
            );

            DomainPolicy.FactorsPolicy domainPolicy = new DomainPolicy.FactorsPolicy(
                null, true, false, false, true, true
            );

            FactorsPolicy actual = (FactorsPolicy)ServiceManagingBaseClient.GetTransportPolicyFromDomainPolicy(domainPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Fences.Count, actual.Fences.Count);
            Assert.AreEqual(expected.Factors.Count, actual.Factors.Count);
            Assert.AreEqual(expected.Factors[0], actual.Factors[0]);
        }

        [TestMethod]
        public void TestGetTransportPolicyFromDomain_MethodAmount()
        {
            MethodAmountPolicy expected = new MethodAmountPolicy(
                2,
                true,
                true,
                null
            );

            DomainPolicy.MethodAmountPolicy domainPolicy = new DomainPolicy.MethodAmountPolicy(
                null, 2, true, true
            );

            MethodAmountPolicy actual = (MethodAmountPolicy)ServiceManagingBaseClient.GetTransportPolicyFromDomainPolicy(domainPolicy);
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Fences.Count, actual.Fences.Count);
            Assert.AreEqual(expected.Amount, actual.Amount);
        }

        [TestMethod]
        public void TestGetDomainFencesFromTransportFences_null()
        {
            List<TransportFence> fences = null;
            List<DomainPolicy.IFence> actual = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void TestGetDomainFencesFromTransportFences_GeoCircle()
        {
            var transportFence = new TransportFence(name: "TestFence", type: "GEO_CIRCLE", latitude: 150, longitude: 20, radius: 1200);
            List<TransportFence> fences = new List<TransportFence>()
            {
                transportFence
            };
            List<DomainPolicy.IFence> actual = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            DomainPolicy.GeoCircleFence domainFence = (DomainPolicy.GeoCircleFence)actual[0];

            Assert.AreEqual(transportFence.Name, domainFence.Name);
            Assert.AreEqual(transportFence.Latitude, domainFence.Latitude);
            Assert.AreEqual(transportFence.Longitude, domainFence.Longitude);
            Assert.AreEqual(transportFence.Radius, domainFence.Radius);
        }

        [TestMethod]
        public void TestGetDomainFencesFromTransportFences_Territory()
        {
            var transportFence = new TransportFence(name: "TestFence", type: "TERRITORY", country: "US", postalCode: "98454");
            List<TransportFence> fences = new List<TransportFence>()
            {
                transportFence
            };
            List<DomainPolicy.IFence> actual = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            DomainPolicy.TerritoryFence domainFence = (DomainPolicy.TerritoryFence)actual[0];

            Assert.AreEqual(transportFence.Name, domainFence.Name);
            Assert.AreEqual(transportFence.Country, domainFence.Country);
            Assert.AreEqual(transportFence.PostalCode, domainFence.PostalCode);
        }

        [TestMethod]
        public void TestGetDomainFencesFromTransportFences_Territory_GeoCircle()
        {
            var transportFenceGeo = new TransportFence(name: "TestFence", type: "GEO_CIRCLE", latitude: 150, longitude: 20, radius: 1200);
            var transportFenceTer = new TransportFence(name: "TestFence", type: "TERRITORY", country: "US", postalCode: "98454");
            List<TransportFence> fences = new List<TransportFence>()
            {
                transportFenceGeo,
                transportFenceTer
            };
            List<DomainPolicy.IFence> actual = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            DomainPolicy.GeoCircleFence geocircleFence = (DomainPolicy.GeoCircleFence)actual[0];

            Assert.AreEqual(transportFenceGeo.Name, geocircleFence.Name);
            Assert.AreEqual(transportFenceGeo.Latitude, geocircleFence.Latitude);
            Assert.AreEqual(transportFenceGeo.Longitude, geocircleFence.Longitude);
            Assert.AreEqual(transportFenceGeo.Radius, geocircleFence.Radius);

            DomainPolicy.TerritoryFence territoryFence = (DomainPolicy.TerritoryFence)actual[1];

            Assert.AreEqual(transportFenceTer.Name, territoryFence.Name);
            Assert.AreEqual(transportFenceTer.Country, territoryFence.Country);
            Assert.AreEqual(transportFenceTer.PostalCode, territoryFence.PostalCode);
        }

        [TestMethod]
        public void TestGetDomainFencesFromTransportFences_ThrowsException()
        {
            List<TransportFence> fences = new List<TransportFence>()
            {
                new TransportFence(name: "TestFence", type: "NOTAVALIDFENCE", latitude: 150, longitude: 20, radius: 1200)
            };
            //List<DomainPolicy.IFence> actual = ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences);
            Assert.ThrowsException<UnknownFenceTypeException>(() => ServiceManagingBaseClient.GetDomainFencesFromTransportFences(fences));
        }

        [TestMethod]
        public void TestGetTransportFencesFromDomainFences_null()
        {
            List<DomainPolicy.IFence> fences = null;
            List<TransportFence> actual = ServiceManagingBaseClient.GetTransportFencesFromDomainFences(fences);
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void TestGetTransportFencesFromDomainFences_GeoCircle()
        {
            DomainPolicy.GeoCircleFence domainGeoCircle = new DomainPolicy.GeoCircleFence(100, 50, 1200, "TestGeoCircle");
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>()
            {
                domainGeoCircle
            };
            List<TransportFence> actual = ServiceManagingBaseClient.GetTransportFencesFromDomainFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            TransportFence transportFence = actual[0];

            Assert.AreEqual(transportFence.Name, transportFence.Name);
            Assert.AreEqual(transportFence.Latitude, transportFence.Latitude);
            Assert.AreEqual(transportFence.Longitude, transportFence.Longitude);
            Assert.AreEqual(transportFence.Radius, transportFence.Radius);
        }

        [TestMethod]
        public void TestGetTransportFencesFromDomainFences_Territory()
        {
            DomainPolicy.TerritoryFence domainTerritory = new DomainPolicy.TerritoryFence(name: "TestFence", country: "US", postalCode: "98454");
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>()
            {
                domainTerritory
            };
            List<TransportFence> actual = ServiceManagingBaseClient.GetTransportFencesFromDomainFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            TransportFence transportFence = actual[0];

            Assert.AreEqual(domainTerritory.Name, transportFence.Name);
            Assert.AreEqual(domainTerritory.Country, transportFence.Country);
            Assert.AreEqual(domainTerritory.PostalCode, transportFence.PostalCode);
        }

        [TestMethod]
        public void TestGetTransportFencesFromDomainFences_Territory_GeoCircle()
        {
            DomainPolicy.GeoCircleFence domainGeoCircle = new DomainPolicy.GeoCircleFence(100, 50, 1200, "TestGeoCircle");
            DomainPolicy.TerritoryFence domainTerritory = new DomainPolicy.TerritoryFence(name: "TestFence", country: "US", postalCode: "98454");
            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>()
            {
                domainGeoCircle,
                domainTerritory
            };
            List<TransportFence> actual = ServiceManagingBaseClient.GetTransportFencesFromDomainFences(fences);
            Assert.AreEqual(fences.Count, actual.Count);

            TransportFence geocircleFence = actual[0];

            Assert.AreEqual(domainGeoCircle.Name, geocircleFence.Name);
            Assert.AreEqual(domainGeoCircle.Latitude, geocircleFence.Latitude);
            Assert.AreEqual(domainGeoCircle.Longitude, geocircleFence.Longitude);
            Assert.AreEqual(domainGeoCircle.Radius, geocircleFence.Radius);

            TransportFence territoryFence = actual[1];

            Assert.AreEqual(domainTerritory.Name, territoryFence.Name);
            Assert.AreEqual(domainTerritory.Country, territoryFence.Country);
            Assert.AreEqual(domainTerritory.PostalCode, territoryFence.PostalCode);
        }

        [TestMethod]
        public void TestGetTransportLocationsFromDomainGeoCircleFences_null()
        {
            List<DomainPolicy.IFence> fences = null;
            List<AuthPolicy.Location> locations = ServiceManagingBaseClient.GetTransportLocationsFromDomainGeoCircleFences(fences);
            Assert.AreEqual(0, locations.Count);
        }

        [TestMethod]
        public void TestGetTransportLocationsFromDomainGeoCircleFences_multiplefences()
        {
            DomainPolicy.GeoCircleFence fenceOne = new DomainPolicy.GeoCircleFence(
                100, 100, 1200, "Fence1"
                );

            DomainPolicy.GeoCircleFence fenceTwo = new DomainPolicy.GeoCircleFence(
                100, 100, 1200, "Fence2"
                );

            List<DomainPolicy.IFence> fences = new List<DomainPolicy.IFence>()
            {
                fenceOne, fenceTwo
            };
            List<AuthPolicy.Location> locations = ServiceManagingBaseClient.GetTransportLocationsFromDomainGeoCircleFences(fences);

            AuthPolicy.Location locationOne = locations[0];
            AuthPolicy.Location locationTwo = locations[1];

            Assert.AreEqual(fences.Count, locations.Count);
            Assert.AreEqual(fenceOne.Longitude, locationOne.Longitude);
            Assert.AreEqual(fenceOne.Latitude, locationOne.Latitude);
            Assert.AreEqual(fenceOne.Radius, locationOne.Radius);
            Assert.AreEqual(fenceOne.Name, locationOne.Name);

            Assert.AreEqual(fenceTwo.Longitude, locationTwo.Longitude);
            Assert.AreEqual(fenceTwo.Latitude, locationTwo.Latitude);
            Assert.AreEqual(fenceTwo.Radius, locationTwo.Radius);
            Assert.AreEqual(fenceTwo.Name, locationTwo.Name);
        }

    }
}
