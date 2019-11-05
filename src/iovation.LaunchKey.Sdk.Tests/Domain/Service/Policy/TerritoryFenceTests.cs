using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class TerritoryFenceTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            TerritoryFence territoryFence = new TerritoryFence("US", "US-CA", "89011", "TERRITORY FENCE");

            Assert.AreEqual("US", territoryFence.Country);
            Assert.AreEqual("US-CA", territoryFence.AdministrativeArea);
            Assert.AreEqual("89011", territoryFence.PostalCode);
            Assert.AreEqual("TERRITORY FENCE", territoryFence.Name);
        }

        [TestMethod]
        public void Test_AdminArea_Can_Be_Null()
        {
            TerritoryFence territoryFence = new TerritoryFence("US", null, "89011", "TERRITORY FENCE");

            Assert.AreEqual("US", territoryFence.Country);
            Assert.AreEqual(null, territoryFence.AdministrativeArea);
            Assert.AreEqual("89011", territoryFence.PostalCode);
            Assert.AreEqual("TERRITORY FENCE", territoryFence.Name);
        }

        [TestMethod]
        public void Test_PostalCode_Can_Be_Null()
        {
            TerritoryFence territoryFence = new TerritoryFence("US", null, null, "TERRITORY FENCE");

            Assert.AreEqual("US", territoryFence.Country);
            Assert.AreEqual(null, territoryFence.AdministrativeArea);
            Assert.AreEqual(null, territoryFence.PostalCode);
            Assert.AreEqual("TERRITORY FENCE", territoryFence.Name);
        }

        [TestMethod]
        public void Test_Name_Can_Be_Null()
        {
            TerritoryFence territoryFence = new TerritoryFence("US", null, null, null);

            Assert.AreEqual("US", territoryFence.Country);
            Assert.AreEqual(null, territoryFence.AdministrativeArea);
            Assert.AreEqual(null, territoryFence.PostalCode);
            Assert.AreEqual(null, territoryFence.Name);
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            TransportDomain.TerritoryFence expected = new TransportDomain.TerritoryFence(
                "US",
                "US-CA",
                "89011",
                "TERRITORY FENCE"
            );

            TerritoryFence territoryFence = new TerritoryFence("US", "US-CA", "89011", "TERRITORY FENCE");

            TransportDomain.TerritoryFence actual = (TransportDomain.TerritoryFence)territoryFence.ToTransport();

            Assert.AreEqual(actual.Country, territoryFence.Country);
            Assert.AreEqual(actual.AdministrativeArea, territoryFence.AdministrativeArea);
            Assert.AreEqual(actual.PostalCode, territoryFence.PostalCode);
            Assert.AreEqual(actual.Name, territoryFence.Name);
        }
    }
}