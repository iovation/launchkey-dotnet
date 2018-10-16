using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Jose;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class AuthPolicyTests
	{
		[TestMethod]
		public void Constructor_ShouldPopulateFactorBasedOnDeviceIntegrity()
		{
			var authPolicy = new AuthPolicy(
				null,
				null,
				null,
				null,
				true,
				null
			);

			Assert.IsTrue(authPolicy.Factors != null);
			Assert.IsTrue(authPolicy.Factors.Count == 1);
			Assert.IsTrue(authPolicy.Factors[0].Factor == AuthPolicy.FactorType.DeviceIntegrity);
			Assert.IsTrue(authPolicy.Factors[0].Attributes.FactorEnabled == 1);
			Assert.IsTrue(authPolicy.Factors[0].Attributes.Locations == null);
			Assert.IsTrue(authPolicy.Factors[0].Requirement == AuthPolicy.FactorRequirementType.ForcedRequirement);
		}

		[TestMethod]
		public void Constructor_ShouldNotAddGeofenceFactorIfNullLocations()
		{
			var policy = new AuthPolicy(null, null, null, null, null, null);

			Assert.IsTrue(policy.Factors.Count == 0);
		}

		[TestMethod]
		public void Constructor_ShouldNotAddGeofenceFactorIfEmptyLocations()
		{
			var policy = new AuthPolicy(null, null, null, null, null, new List<AuthPolicy.Location>());

			Assert.IsTrue(policy.Factors.Count == 0);
		}

		[TestMethod]
		public void Constructor_ShouldPopulateGeofences()
		{
			var policy = new AuthPolicy(null, null, null, null, null, new List<AuthPolicy.Location>
				{
					new AuthPolicy.Location
					{
						Latitude = 13.37,
						Longitude = 44.44,
						Radius = 10
					}
				}
			);

			Assert.IsTrue(policy.Factors != null);
			Assert.IsTrue(policy.Factors.Count == 1);
			Assert.IsTrue(policy.Factors[0].Factor == AuthPolicy.FactorType.Geofence);
			Assert.IsTrue(policy.Factors[0].Priority == 1);
			Assert.IsTrue(policy.Factors[0].Requirement == AuthPolicy.FactorRequirementType.ForcedRequirement);
			Assert.IsTrue(policy.Factors[0].Attributes != null);
			Assert.IsTrue(policy.Factors[0].Attributes.Locations != null);
			Assert.IsTrue(Math.Abs(policy.Factors[0].Attributes.Locations[0].Latitude - 13.37) < 0.01);
			Assert.IsTrue(Math.Abs(policy.Factors[0].Attributes.Locations[0].Longitude - 44.44) < 0.01);
			Assert.IsTrue(Math.Abs(policy.Factors[0].Attributes.Locations[0].Radius - 10) < 0.01);
		}

		[TestMethod]
		public void Constructor_ShouldAllowGeofencesAndDeviceIntegrity()
		{
			var policy = new AuthPolicy(null, null, null, null, true, new List<AuthPolicy.Location>
				{
					new AuthPolicy.Location
					{
						Latitude = 13.37,
						Longitude = 44.44,
						Radius = 10
					}
				}
			);

			Assert.IsTrue(policy.Factors != null);
			Assert.IsTrue(policy.Factors.Count == 2);

			var deviceFactor = policy.Factors.Single(f => f.Factor == AuthPolicy.FactorType.DeviceIntegrity);
			var geoFactor = policy.Factors.Single(f => f.Factor == AuthPolicy.FactorType.Geofence);

			Assert.IsTrue(geoFactor.Factor == AuthPolicy.FactorType.Geofence);
			Assert.IsTrue(geoFactor.Priority == 1);
			Assert.IsTrue(geoFactor.Requirement == AuthPolicy.FactorRequirementType.ForcedRequirement);
			Assert.IsTrue(geoFactor.Attributes != null);
			Assert.IsTrue(geoFactor.Attributes.Locations != null);

			Assert.IsTrue(Math.Abs(geoFactor.Attributes.Locations[0].Latitude - 13.37) < 0.01);
			Assert.IsTrue(Math.Abs(geoFactor.Attributes.Locations[0].Longitude - 44.44) < 0.01);
			Assert.IsTrue(Math.Abs(geoFactor.Attributes.Locations[0].Radius - 10) < 0.01);

			Assert.IsTrue(deviceFactor.Factor == AuthPolicy.FactorType.DeviceIntegrity);
			Assert.IsTrue(deviceFactor.Requirement == AuthPolicy.FactorRequirementType.ForcedRequirement);
		}

		[TestMethod]
		public void Constructor_ShouldIncludeAny()
		{
			var policy = new AuthPolicy(99, null, null, null, null, null);

			Assert.IsTrue(policy.MinimumRequirements != null);
			Assert.IsTrue(policy.MinimumRequirements.Count == 1);
			Assert.IsTrue(policy.MinimumRequirements[0].Any == 99);
		}

		[TestMethod]
		public void Constructor_ShouldIncludeFlags()
		{
			var policy = new AuthPolicy(null, null, true, null, null, null);

			Assert.IsTrue(policy.MinimumRequirements[0].Inherence == 1);
			Assert.IsTrue(policy.MinimumRequirements[0].Possession == null);
			Assert.IsTrue(policy.MinimumRequirements[0].Knowledge == null);
			Assert.IsTrue(!policy.MinimumRequirements[0].Any.HasValue);
		}

		[TestMethod]
		public void Constructor_ShouldIncludeDeviceIntegrityFlags()
		{
			var policy = new AuthPolicy(null, null, null, null, null, null);
			Assert.IsTrue(policy.Factors.Count == 0);

			var policyWithIntegrityFalse = new AuthPolicy(null, null, null, null, false, null);
			Assert.IsTrue(policyWithIntegrityFalse.Factors.Count == 1);
			Assert.IsTrue(policyWithIntegrityFalse.Factors[0].Factor == AuthPolicy.FactorType.DeviceIntegrity);
			Assert.IsTrue(policyWithIntegrityFalse.Factors[0].Attributes.FactorEnabled == 0);

			var policyWithIntegrityTrue = new AuthPolicy(null, null, null, null, true, null);
			Assert.IsTrue(policyWithIntegrityTrue.Factors.Count == 1);
			Assert.IsTrue(policyWithIntegrityTrue.Factors[0].Factor == AuthPolicy.FactorType.DeviceIntegrity);
			Assert.IsTrue(policyWithIntegrityTrue.Factors[0].Attributes.FactorEnabled == 1);
		}

		[TestMethod]
		public void Constructor_ShouldCreateTimeFences()
		{
			var timeFence = new AuthPolicy.TimeFence("fence", new List<string> { }, 1, 5, 0, 0, "America/New York");
			var timeFences = new List<AuthPolicy.TimeFence>
			{
				timeFence
			};

			var policy = new AuthPolicy(null, null, null, null, null, null, timeFences);

			Assert.IsTrue(policy.Factors.Count == 1);
			Assert.IsTrue(policy.Factors[0].Factor == AuthPolicy.FactorType.TimeFence);
			Assert.IsTrue(policy.Factors[0].Priority == 1);
			Assert.IsTrue(policy.Factors[0].Requirement == AuthPolicy.FactorRequirementType.ForcedRequirement);
			Assert.AreEqual(timeFence, policy.Factors[0].Attributes.TimeFences[0]);
		}

		[TestMethod]
		public void Serialization_FullObject_SerializesAsExpected()
		{
			var expected =
				"{" +
				"\"minimum_requirements\":[{" +
				"\"requirement\":\"authenticated\"," +
				"\"any\":2," +
				"\"knowledge\":1," +
				"\"inherence\":1," +
				"\"possession\":1" +
				"}]," +
				"\"factors\":[" +
				"{" +
				"\"factor\":\"geofence\"," +
				"\"requirement\":\"forced requirement\"," +
				"\"priority\":1," +
				"\"attributes\":{" +
				"\"locations\":[" +
				"{\"radius\":1.1,\"latitude\":2.1,\"longitude\":3.1}," +
				"{\"radius\":1.2,\"latitude\":2.2,\"longitude\":3.2}" +
				"]" +
				"}" +
				"},{" +
				"\"factor\":\"device integrity\"," +
				"\"requirement\":\"forced requirement\"," +
				"\"attributes\":{" +
				"\"factor enabled\":1" +
				"}" +
				"},{" +
				"\"factor\":\"timefence\"," +
				"\"requirement\":\"forced requirement\"," +
				"\"priority\":1," +
				"\"attributes\":{" +
				"\"time fences\":[" +
				"{\"name\":\"tf1\",\"days\":[\"Monday\",\"Tuesday\",\"Wednesday\"],\"start hour\":0,\"end hour\":23,\"start minute\":0,\"end minute\":59,\"timezone\":\"America/Los_Angeles\"}," +
				"{\"name\":\"tf2\",\"days\":[\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start hour\":0,\"end hour\":23,\"start minute\":0,\"end minute\":59,\"timezone\":\"America/Los_Angeles\"}" +
				"]" +
				"}" +
				"}" +
				"]" +
				"}";


			var geoFences = new List<AuthPolicy.Location>
			{
				new AuthPolicy.Location {Radius = 1.1, Latitude = 2.1, Longitude = 3.1},
				new AuthPolicy.Location {Radius = 1.2, Latitude = 2.2, Longitude = 3.2}
			};
			var timeFences = new List<AuthPolicy.TimeFence>
			{
				new AuthPolicy.TimeFence("tf1", new List<string> {"Monday", "Tuesday", "Wednesday"}, 0, 23, 0, 59, "America/Los_Angeles"),
				new AuthPolicy.TimeFence("tf2", new List<string> {"Thursday", "Friday", "Saturday", "Sunday"}, 0, 23, 0, 59, "America/Los_Angeles")
			};

			var policy = new AuthPolicy(2, true, true, true, true, geoFences, timeFences);
			var json = new JsonNetJsonEncoder();
			var encoded = json.EncodeObject(policy);

			Assert.AreEqual(expected, encoded);
		}

		[TestMethod]
		public void Serialization_MapsNoMinimumRequirementsWhenNoneAreSpecified()
		{
			var expected =
				"{" +
				"\"minimum_requirements\":[]," +
				"\"factors\":[" +
				"{" +
				"\"factor\":\"device integrity\"," +
				"\"requirement\":\"forced requirement\"," +
				"\"attributes\":{" +
				"\"factor enabled\":1" +
				"}" +
				"}" +
				"]" +
				"}";
			var policy = new AuthPolicy(null, null, null, null, true, null, null);
			var actual = new JsonNetJsonEncoder().EncodeObject(policy);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialization_MapperMapsNoMinimumRequirementsAndNoFactorsWhenNoneAreSpecified()
		{
			var expected =
				"{" +
				"\"minimum_requirements\":[]," +
				"\"factors\":[]" +
				"}";

			var policy = new AuthPolicy(null, null, null, null, null, null, null);
			var actual = new JsonNetJsonEncoder().EncodeObject(policy);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialization_ObjectParsesAsExpected()
		{
			var json =
				"{" +
						"\"minimum_requirements\":[{" +
						"\"requirement\":\"authenticated\"," +
						"\"any\":2," +
						"\"knowledge\":1," +
						"\"inherence\":1," +
						"\"possession\":1" +
						"}]," +
						"\"factors\":[" +
						"{" +
						"\"factor\":\"geofence\"," +
						"\"requirement\":\"forced requirement\"," +
						"\"priority\":1," +
						"\"attributes\":{" +
						"\"locations\":[" +
						"{\"radius\":1.1,\"latitude\":2.1,\"longitude\":3.1}," +
						"{\"radius\":1.2,\"latitude\":2.2,\"longitude\":3.2}" +
						"]" +
						"}" +
						"},{" +
						"\"factor\":\"device integrity\"," +
						"\"attributes\":{" +
						"\"factor enabled\":\"1\"" +
						"}" +
						"},{" +
						"\"factor\":\"timefence\"," +
						"\"requirement\":\"forced requirement\"," +
						"\"priority\":1," +
						"\"attributes\":{" +
						"\"time fences\":[" +
						"{\"name\":\"tf1\",\"days\":[\"Monday\",\"Tuesday\",\"Wednesday\"],\"start hour\":0,\"end hour\":23,\"start minute\":0,\"end minute\":59,\"timezone\":\"America/Los_Angeles\"}," +
						"{\"name\":\"tf2\",\"days\":[\"Thursday\",\"Friday\",\"Saturday\",\"Sunday\"],\"start hour\":0,\"end hour\":23,\"start minute\":0,\"end minute\":59,\"timezone\":\"America/Los_Angeles\"}" +
						"]" +
						"}" +
						"}" +
						"]}";

			var geoFences = new List<AuthPolicy.Location>
			{
				new AuthPolicy.Location(null, 1.1, 2.1, 3.1),
				new AuthPolicy.Location(null, 1.2, 2.2, 3.2)
			};

			var timeFences = new List<AuthPolicy.TimeFence>
			{
				new AuthPolicy.TimeFence("tf1", new List<string>{"Monday", "Tuesday", "Wednesday"}, 0, 23, 0, 59, "America/Los_Angeles"),
				new AuthPolicy.TimeFence("tf2", new List<string>{"Thursday", "Friday", "Saturday", "Sunday"}, 0, 23, 0, 59, "America/Los_Angeles")
			};

			var expected = new AuthPolicy(2, true, true, true, true, geoFences, timeFences);
			var actual = new JsonNetJsonEncoder().DecodeObject<AuthPolicy>(json);

			actual.ShouldCompare(expected);
		}

		[TestMethod]
		public void Serialization_ObjectParsesNoMinimumRequirementsWhenThereAreNone()
		{
			var json =
				"{" +
				"\"minimum_requirements\":[]," +
				"\"factors\":[" +
				"{" +
				"\"factor\":\"device integrity\"," +
				"\"attributes\":{" +
				"\"factor enabled\":\"1\"" +
				"}" +
				"}" +
				"]" +
				"}";

			var expected = new AuthPolicy(null, null, null, null, true, null);
			var actual = new JsonNetJsonEncoder().DecodeObject<AuthPolicy>(json);
			actual.ShouldCompare(expected);
		}

		[TestMethod]
		public void Serialization_ObjectParsesMinimumRequirementsAllOnlyWhenOnlySpecified()
		{
			var json =
				"{" +
				"\"minimum_requirements\":[{" +
				"\"requirement\":\"authenticated\"," +
				"\"any\":2" +
				"}]," +
				"\"factors\":[]" +
				"}";
			var expected = new AuthPolicy(2, null, null, null, null, null);
			var actual = new JsonNetJsonEncoder().DecodeObject<AuthPolicy>(json);
			actual.ShouldCompare(expected);
		}

		[TestMethod]
		public void Serialization_ObjectParsesMinimumRequirementsTypesOnlyWhenOnlySpecified()
		{
			var json =
				"{" +
				"\"minimum_requirements\":[{" +
				"\"requirement\":\"authenticated\"," +
				"\"knowledge\":1," +
				"\"inherence\":1," +
				"\"possession\":1" +
				"}]," +
				"\"factors\":[]" +
				"}";
			var expected = new AuthPolicy(null, true, true, true, null, null);
			var actual = new JsonNetJsonEncoder().DecodeObject<AuthPolicy>(json);
			actual.ShouldCompare(expected);
		}
	}
}
