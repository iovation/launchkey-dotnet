using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport.Domain;
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
			Assert.IsTrue(authPolicy.Factors[0].Priority == 1);
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
			Assert.IsTrue(deviceFactor.Priority == 1);
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
			Assert.IsTrue(policy.MinimumRequirements[0].Possession == 0);
			Assert.IsTrue(policy.MinimumRequirements[0].Knowledge == 0);
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
	}
}
