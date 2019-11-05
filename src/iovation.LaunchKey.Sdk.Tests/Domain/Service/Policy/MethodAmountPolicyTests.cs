using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class MethodAmountPolicyTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            List<IFence> fences = new List<IFence>()
            {
                new TerritoryFence("US")
            };
            MethodAmountPolicy policy = new MethodAmountPolicy(fences, 1, true, true);

            Assert.AreEqual(true, policy.DenyEmulatorSimulator);
            Assert.AreEqual(true, policy.DenyRootedJailbroken);
            Assert.AreEqual(1, policy.Amount);
            Assert.AreEqual(1, policy.Fences.Count);
            Assert.AreEqual("US", (policy.Fences[0] as TerritoryFence)?.Country);
        }

        [TestMethod]
        public void Test_Deny_Attributes_Can_Be_Null_For_Nested_Policies()
        {
            MethodAmountPolicy factorsPolicy = new MethodAmountPolicy(null, 2, null, null);

            Assert.IsNull(factorsPolicy.DenyEmulatorSimulator);
            Assert.IsNull(factorsPolicy.DenyRootedJailbroken);
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            var expected = new TransportDomain.MethodAmountPolicy(
                2,
                false,
                false,
                new List<TransportDomain.IFence>()
            );

            var policy = new MethodAmountPolicy(
                null, 2, false, false
            );

            TransportDomain.MethodAmountPolicy actual = (TransportDomain.MethodAmountPolicy)policy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.MethodAmountPolicy));
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Amount, actual.Amount);
            CollectionAssert.AreEquivalent(expected.Fences, actual.Fences);
        }

        [TestMethod]
        public void Test_Empty_To_Transport_Works()
        {
            var expected = new TransportDomain.MethodAmountPolicy(
                2,
                null,
                null,
                new List<TransportDomain.IFence>()
            );

            var policy = new MethodAmountPolicy(
                null, 2, null, null
            );

            TransportDomain.MethodAmountPolicy actual = (TransportDomain.MethodAmountPolicy)policy.ToTransport();

            Assert.IsInstanceOfType(actual, typeof(TransportDomain.MethodAmountPolicy));
            Assert.AreEqual(expected.DenyEmulatorSimulator, actual.DenyEmulatorSimulator);
            Assert.AreEqual(expected.DenyRootedJailbroken, actual.DenyRootedJailbroken);
            Assert.AreEqual(expected.Amount, actual.Amount);
            CollectionAssert.AreEquivalent(expected.Fences, actual.Fences);
        }
    }
}