using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class LegacyPolicyTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            List<IFence> fences = new List<IFence>()
            {
                new GeoCircleFence(20, -20, 1500, "TEST FENCE")
            };
            LegacyPolicy policy = new LegacyPolicy(
                fences,
                true,
                2,
                null,
                null,
                null,
                null
                );

            Assert.AreEqual(null, policy.DenyEmulatorSimulator);
            Assert.AreEqual(true, policy.DenyRootedJailbroken);
            Assert.AreEqual(2, policy.Amount);
            Assert.AreEqual(1, policy.Fences.Count);
            Assert.AreEqual(20, (policy.Fences[0] as GeoCircleFence)?.Latitude);
            Assert.AreEqual(-20, (policy.Fences[0] as GeoCircleFence)?.Longitude);
            Assert.AreEqual(1500, (policy.Fences[0] as GeoCircleFence)?.Radius);
            Assert.AreEqual("TEST FENCE", (policy.Fences[0] as GeoCircleFence)?.Name);
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            var expected = new TransportDomain.AuthPolicy(
                1,
                null,
                null,
                null,
                null,
                null,
                null
            );

            var policy = new LegacyPolicy(
                null,
                false,
                1,
                false,
                false,
                false,
                null
            );

            TransportDomain.AuthPolicy actual = (TransportDomain.AuthPolicy)policy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.AuthPolicy));
            Assert.AreEqual(null, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.MinimumRequirements[0].Any, actual.MinimumRequirements[0].Any);
            Assert.AreEqual(expected.MinimumRequirements[0].Possession, actual.MinimumRequirements[0].Possession);
            Assert.AreEqual(expected.MinimumRequirements[0].Knowledge, actual.MinimumRequirements[0].Knowledge);
            Assert.AreEqual(expected.MinimumRequirements[0].Inherence, actual.MinimumRequirements[0].Inherence);
            Assert.AreEqual(expected.MinimumRequirements[0].Requirement, actual.MinimumRequirements[0].Requirement);
        }
    }
}