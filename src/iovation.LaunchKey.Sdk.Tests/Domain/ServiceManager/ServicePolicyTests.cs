using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Transport.Domain;

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

		[TestMethod]
		public void FromTransport_Variant1()
		{
			var authPolicy = new AuthPolicy(1, null, null, null, true, new List<AuthPolicy.Location>
				{
					new AuthPolicy.Location("n", 1, 2, 3),
					new AuthPolicy.Location("x", 2, 4, 6)
				},
				new List<AuthPolicy.TimeFence>
				{
					new AuthPolicy.TimeFence("y", new List<string> { "Monday" }, 1, 2, 3, 4, "A")
				});

			var servicePolicy = ServicePolicy.FromTransport(authPolicy);

			Assert.IsTrue(servicePolicy.RequiredFactors == 1);
			Assert.IsTrue(servicePolicy.RequireInherenceFactor == false);
			Assert.IsTrue(servicePolicy.RequireKnowledgeFactor == false);
			Assert.IsTrue(servicePolicy.RequirePossessionFactor == false);
			
			Assert.IsTrue(servicePolicy.TimeFences.Count == 1);
			Assert.IsTrue(servicePolicy.TimeFences[0].Days.Count == 1);
			Assert.IsTrue(servicePolicy.TimeFences[0].Days[0] == DayOfWeek.Monday);
			Assert.IsTrue(servicePolicy.TimeFences[0].Name == "y");
			Assert.IsTrue(servicePolicy.TimeFences[0].TimeZone == "A");
			Assert.IsTrue(servicePolicy.TimeFences[0].StartHour == 1);
			Assert.IsTrue(servicePolicy.TimeFences[0].EndHour == 2);
			Assert.IsTrue(servicePolicy.TimeFences[0].StartMinute == 3);
			Assert.IsTrue(servicePolicy.TimeFences[0].EndMinute == 4);

			Assert.IsTrue(servicePolicy.Locations.Count == 2);
			Assert.IsTrue(servicePolicy.Locations[0].Name == "n");
			Assert.AreEqual(servicePolicy.Locations[0].Latitude, 2, 0.01);
			Assert.AreEqual(servicePolicy.Locations[0].Longitude, 3, 0.01);
			Assert.AreEqual(servicePolicy.Locations[0].Radius, 1, 0.01);

			Assert.IsTrue(servicePolicy.Locations[1].Name == "x");
			Assert.AreEqual(servicePolicy.Locations[1].Latitude, 4, 0.01);
			Assert.AreEqual(servicePolicy.Locations[1].Longitude, 6, 0.01);
			Assert.AreEqual(servicePolicy.Locations[1].Radius, 2, 0.01);
		}
	}
}
