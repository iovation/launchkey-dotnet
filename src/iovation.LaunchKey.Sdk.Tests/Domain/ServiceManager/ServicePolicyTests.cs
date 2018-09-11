using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

namespace iovation.LaunchKey.Sdk.Tests.Domain.ServiceManager
{
	[TestClass]
	public class ServicePolicyTests
	{
		[TestMethod]
		public void ToTransport_Variant1()
		{
			var servicePolicy = new ServicePolicy(
				requiredFactors: 1,
				jailbreakDetection: true
			);
			var result = servicePolicy.ToTransport();

			Assert.IsTrue(result.Factors.Count == 1);
			Assert.IsTrue(result.Factors[0].Factor == Sdk.Transport.Domain.AuthPolicy.FactorType.DeviceIntegrity);
			Assert.IsTrue(result.MinimumRequirements.Count == 1);
			Assert.IsTrue(result.MinimumRequirements[0].Any == 1);
		}

		[TestMethod]
		public void ToTransport_Variant2()
		{
			var servicePolicy = new ServicePolicy(
				requireKnowledgeFactor: true
			);
			var result = servicePolicy.ToTransport();

			Assert.IsTrue(result.MinimumRequirements[0].Knowledge == 1);
			Assert.IsTrue(result.MinimumRequirements[0].Inherence == 0);
			Assert.IsTrue(result.MinimumRequirements[0].Possession == 0);
			Assert.IsTrue(result.MinimumRequirements[0].Any == null);
		}

		[TestMethod]
		public void ToTransport_Variant3()
		{
			var servicePolicy = new ServicePolicy(
				locations: new List<Location>
				{
					new Location("john's house", 5.0, 100, 200),
					new Location("the middle of the pacific ocean", 10, 200, 500)
				}
			);

			var result = servicePolicy.ToTransport();

			Assert.IsTrue(result.MinimumRequirements.Count == 0);
			Assert.IsTrue(result.Factors.Count == 1);
			Assert.IsTrue(result.Factors[0].Factor == Sdk.Transport.Domain.AuthPolicy.FactorType.Geofence);
			Assert.IsTrue(result.Factors[0].Attributes.Locations.Count == 2);

			Assert.AreEqual(result.Factors[0].Attributes.Locations[0].Name, "john's house");
			Assert.AreEqual(result.Factors[0].Attributes.Locations[0].Radius, 5, 0.01);
			Assert.AreEqual(result.Factors[0].Attributes.Locations[0].Latitude, 100, 0.01);
			Assert.AreEqual(result.Factors[0].Attributes.Locations[0].Longitude, 200, 0.01);

			Assert.AreEqual(result.Factors[0].Attributes.Locations[1].Name, "the middle of the pacific ocean");
			Assert.AreEqual(result.Factors[0].Attributes.Locations[1].Radius, 10, 0.01);
			Assert.AreEqual(result.Factors[0].Attributes.Locations[1].Latitude, 200, 0.01);
			Assert.AreEqual(result.Factors[0].Attributes.Locations[1].Longitude, 500, 0.01);
		}

		[TestMethod]
		public void ToTransport_Variant4()
		{
			var servicePolicy = new ServicePolicy(
				timeFences: new List<TimeFence>
				{
					new TimeFence(
						"Weekend nights",
						new List<DayOfWeek> { DayOfWeek.Friday, DayOfWeek.Saturday },
						20, 1,
						23, 2,
						"America/New_York"
					)
				}
			);

			var result = servicePolicy.ToTransport();

			Assert.IsTrue(result.MinimumRequirements.Count == 0);
			Assert.IsTrue(result.Factors.Count == 1);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences.Count == 1);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].Name == "Weekend nights");
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].Days.Count == 2);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].Days[0] == "Friday");
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].Days[1] == "Saturday");
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].StartHour == 20);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].EndHour == 23);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].StartMinute == 1);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].EndMinute == 2);
			Assert.IsTrue(result.Factors[0].Attributes.TimeFences[0].TimeZone == "America/New_York");
		}
	}
}
