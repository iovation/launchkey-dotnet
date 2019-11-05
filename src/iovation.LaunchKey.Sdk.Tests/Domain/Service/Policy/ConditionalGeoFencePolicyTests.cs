using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Error;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class ConditionalGeoFencePolicyTests
    {

        public static MethodAmountPolicy DEFAULT_METHOD_AMOUNT_POLICY = new MethodAmountPolicy(
            amount: 2,
            denyEmulatorSimulator: false,
            denyRootedJailbroken: false,
            fences: null
            );
        public static FactorsPolicy DEFAULT_FACTORS_POLICY = new FactorsPolicy(
                fences: null,
                requireInherenceFactor: true,
                requirePossessionFactor: true,
                requireKnowledgeFactor: true,
                denyEmulatorSimulator: false,
                denyRootedJailbroken: false
            );

        public static GeoCircleFence DEFAULT_GEOCIRCLE_FENCE = new GeoCircleFence(
                name: "TestGeoCircleFence",
                latitude: 60,
                longitude: -55,
                radius: 1200
            );

        public static TerritoryFence DEFAULT_TERRITORY_FENCE = new TerritoryFence(
            country: "US",
            administrativeArea: "US-CA",
            postalCode: null,
            name: "TestTerritoryFence"
            );

        [TestMethod]
        public void TestCreateCondGeoPolicy()
        {
            ConditionalGeoFencePolicy testPolicy = new ConditionalGeoFencePolicy(
                    inside: DEFAULT_METHOD_AMOUNT_POLICY,
                    outside: DEFAULT_METHOD_AMOUNT_POLICY,
                    fences: null, 
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                );

            Assert.AreEqual(false, testPolicy.DenyEmulatorSimulator);
            Assert.AreEqual(false, testPolicy.DenyRootedJailbroken);
            Assert.IsInstanceOfType(testPolicy.Fences, typeof(List<IFence>));
            Assert.AreEqual(0, testPolicy.Fences.Count);
            Assert.IsInstanceOfType(testPolicy.Inside, typeof(MethodAmountPolicy));
            Assert.IsInstanceOfType(testPolicy.Outside, typeof(MethodAmountPolicy));
        }

        [TestMethod]
        public void TestFencesAreCreated()
        {
            List<IFence> fences = new List<IFence>() {
                DEFAULT_GEOCIRCLE_FENCE,
                DEFAULT_TERRITORY_FENCE
            };
            ConditionalGeoFencePolicy testPolicy = new ConditionalGeoFencePolicy(
                    DEFAULT_METHOD_AMOUNT_POLICY,
                    DEFAULT_METHOD_AMOUNT_POLICY,
                    fences: fences,
                    denyEmulatorSimulator: true,
                    denyRootedJailbroken: true
                );

            Assert.AreEqual(true, testPolicy.DenyEmulatorSimulator);
            Assert.AreEqual(true, testPolicy.DenyRootedJailbroken);
            CompareDefaultFences(testPolicy.Fences);
            Assert.IsInstanceOfType(testPolicy.Inside, typeof(MethodAmountPolicy));
            Assert.IsInstanceOfType(testPolicy.Outside, typeof(MethodAmountPolicy));
        }

        [TestMethod]
        public void TestInsideNestedConditionalGeofenceThrows()
        {
            ConditionalGeoFencePolicy nestedCondGeo = new ConditionalGeoFencePolicy(
                inside: DEFAULT_METHOD_AMOUNT_POLICY,
                outside: DEFAULT_METHOD_AMOUNT_POLICY,
                fences: null,
                denyEmulatorSimulator: true,
                denyRootedJailbroken: true
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: nestedCondGeo,
                    outside: DEFAULT_FACTORS_POLICY,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );

        }

        [TestMethod]
        public void TestOutsideNestedConditionalGeofenceThrows()
        {
            ConditionalGeoFencePolicy nestedCondGeo = new ConditionalGeoFencePolicy(
                DEFAULT_METHOD_AMOUNT_POLICY,
                DEFAULT_METHOD_AMOUNT_POLICY,
                fences: null,
                denyEmulatorSimulator: true,
                denyRootedJailbroken: true
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: DEFAULT_FACTORS_POLICY,
                    outside: nestedCondGeo,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );

        }

        [TestMethod]
        public void TestNestedDenyEmulatorSimulatorCantBeTrue()
        {
            MethodAmountPolicy invalidMethodPolicy = new MethodAmountPolicy(
                amount: 1,
                denyEmulatorSimulator: true,
                denyRootedJailbroken: false,
                fences: null
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: DEFAULT_FACTORS_POLICY,
                    outside: invalidMethodPolicy,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );
        }

        [TestMethod]
        public void TestNestedDenyRootedJailbrokenCantBeTrue()
        {
            MethodAmountPolicy invalidMethodPolicy = new MethodAmountPolicy(
                amount: 1,
                denyEmulatorSimulator: false,
                denyRootedJailbroken: true,
                fences: null
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: DEFAULT_FACTORS_POLICY,
                    outside: invalidMethodPolicy,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );
        }

        [TestMethod]
        public void TestInsideNestedFencesCantBeSet()
        {
            List<IFence> fences = new List<IFence>() {
                DEFAULT_GEOCIRCLE_FENCE,
                DEFAULT_TERRITORY_FENCE
            };

            MethodAmountPolicy invalidMethodPolicy = new MethodAmountPolicy(
                amount: 1,
                denyEmulatorSimulator: false,
                denyRootedJailbroken: true,
                fences: fences
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: invalidMethodPolicy,
                    outside: DEFAULT_FACTORS_POLICY,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );
        }

        [TestMethod]
        public void TestOutsideNestedFencesCantBeSet()
        {
            List<IFence> fences = new List<IFence>() {
                DEFAULT_GEOCIRCLE_FENCE,
                DEFAULT_TERRITORY_FENCE
            };

            MethodAmountPolicy invalidMethodPolicy = new MethodAmountPolicy(
                amount: 1,
                denyEmulatorSimulator: false,
                denyRootedJailbroken: true,
                fences: fences
            );

            Assert.ThrowsException<InvalidPolicyAttributes>(() =>
                new ConditionalGeoFencePolicy(
                    inside: DEFAULT_FACTORS_POLICY,
                    outside: invalidMethodPolicy,
                    fences: null,
                    denyEmulatorSimulator: false,
                    denyRootedJailbroken: false
                )
            );
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            var expected = new TransportDomain.ConditionalGeoFencePolicy(
                DEFAULT_METHOD_AMOUNT_POLICY.ToTransport(),
                DEFAULT_FACTORS_POLICY.ToTransport(),
                false,
                false,
                new List<TransportDomain.IFence>()
            );

            var policy = new ConditionalGeoFencePolicy(
                DEFAULT_METHOD_AMOUNT_POLICY,
                DEFAULT_FACTORS_POLICY,
                null,
                false,
                false
            );

            TransportDomain.ConditionalGeoFencePolicy actual = (TransportDomain.ConditionalGeoFencePolicy)policy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.ConditionalGeoFencePolicy));
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.IsInstanceOfType(actual.Inside, typeof(TransportDomain.MethodAmountPolicy));
            Assert.IsInstanceOfType(actual.Outside, typeof(TransportDomain.FactorsPolicy));
            CollectionAssert.AreEquivalent(expected.Fences, actual.Fences);
        }

        public void CompareDefaultFences(List<IFence> actualFences)
        {
            foreach (IFence fence in actualFences)
            {
                if (fence.GetType() == typeof(GeoCircleFence))
                {
                    GeoCircleFence castFence = (GeoCircleFence)fence;
                    Assert.AreEqual(DEFAULT_GEOCIRCLE_FENCE.Name, castFence.Name);
                    Assert.AreEqual(DEFAULT_GEOCIRCLE_FENCE.Latitude, castFence.Latitude);
                    Assert.AreEqual(DEFAULT_GEOCIRCLE_FENCE.Longitude, castFence.Longitude);
                    Assert.AreEqual(DEFAULT_GEOCIRCLE_FENCE.Radius, castFence.Radius);
                }
                else if (fence.GetType() == typeof(TerritoryFence))
                {
                    TerritoryFence castFence = (TerritoryFence)fence;
                    Assert.AreEqual(DEFAULT_TERRITORY_FENCE.Name, castFence.Name);
                    Assert.AreEqual(DEFAULT_TERRITORY_FENCE.Country, castFence.Country);
                    Assert.AreEqual(DEFAULT_TERRITORY_FENCE.AdministrativeArea, castFence.AdministrativeArea);
                    Assert.AreEqual(DEFAULT_TERRITORY_FENCE.PostalCode, castFence.PostalCode);
                }
            }
        }
    }
}