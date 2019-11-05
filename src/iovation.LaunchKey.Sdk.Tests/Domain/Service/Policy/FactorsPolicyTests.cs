using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class FactorsPolicyTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            List<IFence> fences = new List<IFence>()
            {
                new TerritoryFence("US")
            };
            FactorsPolicy policy = new FactorsPolicy(fences, true, true, true, true, true);

            Assert.AreEqual(true, policy.DenyEmulatorSimulator);
            Assert.AreEqual(true, policy.DenyRootedJailbroken);
            Assert.AreEqual(true, policy.RequireInherenceFactor);
            Assert.AreEqual(true, policy.RequireKnowledgeFactor);
            Assert.AreEqual(true, policy.RequirePossessionFactor);
            Assert.AreEqual(1, policy.Fences.Count);
            Assert.AreEqual("US", (policy.Fences[0] as TerritoryFence)?.Country);
        }

        [TestMethod]
        public void Test_Deny_Attributes_Can_Be_Null_For_Nested_Policies()
        {
            FactorsPolicy factorsPolicy = new FactorsPolicy(null, true, true, true, null, null);

            Assert.IsNull(factorsPolicy.DenyEmulatorSimulator);
            Assert.IsNull(factorsPolicy.DenyRootedJailbroken);
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            var expected = new TransportDomain.FactorsPolicy(
                new List<string>() { "KNOWLEDGE", "POSSESSION", "INHERENCE" },
                false,
                false,
                new List<TransportDomain.IFence>()
            );

            var factorsPolicy = new FactorsPolicy(
                null, true, true, true, false, false
            );

            TransportDomain.FactorsPolicy actual = (TransportDomain.FactorsPolicy)factorsPolicy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.FactorsPolicy));
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Factors.Count, actual.Factors.Count);
            CollectionAssert.AreEquivalent(expected.Factors, actual.Factors);
            CollectionAssert.AreEquivalent(expected.Fences, actual.Fences);
        }

        [TestMethod]
        public void Test_Empty_To_Transport_Works()
        {
            var expected = new TransportDomain.FactorsPolicy(
                new List<string>(),
                false,
                false,
                new List<TransportDomain.IFence>()
            );

            var factorsPolicy = new FactorsPolicy(
                null, false, false, false, false, false
            );

            TransportDomain.FactorsPolicy actual = (TransportDomain.FactorsPolicy)factorsPolicy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.FactorsPolicy));
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Factors.Count, actual.Factors.Count);
            CollectionAssert.AreEquivalent(expected.Factors, actual.Factors);
            CollectionAssert.AreEquivalent(expected.Fences, actual.Fences);
        }
    }
}