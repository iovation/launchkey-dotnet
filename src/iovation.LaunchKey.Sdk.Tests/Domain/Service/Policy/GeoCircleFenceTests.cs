using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportDomain = iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Domain.Service.Policy
{
    [TestClass]
    public class GeoCircleFenceTests
    {
        [TestMethod]
        public void Test_Constructor()
        {
            GeoCircleFence geoCircleFence = new GeoCircleFence(
                20, -20, 15000, "GEOFENCE"
            );

            Assert.AreEqual(20, geoCircleFence.Latitude);
            Assert.AreEqual(-20, geoCircleFence.Longitude);
            Assert.AreEqual(15000, geoCircleFence.Radius);
            Assert.AreEqual("GEOFENCE", geoCircleFence.Name);
        }

        [TestMethod]
        public void Test_Name_Can_Be_Null()
        {
            GeoCircleFence geoCircleFence = new GeoCircleFence(
                20, -20, 15000, null
            );

            Assert.AreEqual(20, geoCircleFence.Latitude);
            Assert.AreEqual(-20, geoCircleFence.Longitude);
            Assert.AreEqual(15000, geoCircleFence.Radius);
            Assert.AreEqual(null, geoCircleFence.Name);
        }

        [TestMethod]
        public void Test_To_Transport_Works()
        {
            TransportDomain.GeoCircleFence expected = new TransportDomain.GeoCircleFence(
                null,
                20,
                -20,
                1250
            );

            GeoCircleFence geoCircleFence = new GeoCircleFence(
                20, -20, 1250, null
            );

            TransportDomain.GeoCircleFence actual = (TransportDomain.GeoCircleFence)geoCircleFence.ToTransport();

            Assert.AreEqual(actual.Latitude, expected.Latitude);
            Assert.AreEqual(actual.Longitude, expected.Longitude);
            Assert.AreEqual(actual.Radius, expected.Radius);
            Assert.AreEqual(actual.Name, expected.Name);
        }
    }
}