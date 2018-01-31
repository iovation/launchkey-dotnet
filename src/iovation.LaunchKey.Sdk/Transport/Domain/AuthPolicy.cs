using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class AuthPolicy
	{
		public enum MinimumRequirementType
		{
			[EnumMember(Value = "authenticated")]
			Authenticated,

			[EnumMember(Value = "enabled")]
			Enabled
		}

		public enum FactorType
		{
			[EnumMember(Value = "geofence")]
			Geofence,

			[EnumMember(Value = "device integrity")]
			DeviceIntegrity
		}

		public enum FactorRequirementType
		{
			[EnumMember(Value = "forced requirement")]
			ForcedRequirement,

			[EnumMember(Value = "allowed")]
			Allowed
		}
		
		[JsonProperty("minimum_requirements")]
		public List<MinimumRequirement> MinimumRequirements { get; set; }

		[JsonProperty("factors")]
		public List<AuthPolicyFactor> Factors { get; set; }

		public class MinimumRequirement
		{
			[JsonProperty("requirement")]
			public MinimumRequirementType Requirement { get; set; }

			[JsonProperty("any")]
			public int? Any { get; set; }

			[JsonProperty("knowledge")]
			public int Knowledge { get; set; }

			[JsonProperty("inherence")]
			public int Inherence { get; set; }

			[JsonProperty("possession")]
			public int Possession { get; set; }
		}

		public class AuthPolicyFactor
		{
			[JsonProperty("factor")]
			public FactorType Factor { get; set; }

			[JsonProperty("requirement")]
			public FactorRequirementType Requirement { get; set; }

			[JsonProperty("priority")]
			public int Priority { get; set; }
			
			[JsonProperty("attributes")]
			public AuthPolicyFactorAttributes Attributes { get; set; }
		}

		public class AuthPolicyFactorAttributes
		{
			[JsonProperty("factor enabled", NullValueHandling = NullValueHandling.Ignore)]
			public int? FactorEnabled { get; set; }

			[JsonProperty("locations", NullValueHandling = NullValueHandling.Ignore)]
			public List<Location> Locations { get; set; }
		}

		public class Location
		{
			[JsonProperty("radius")]
			public double Radius { get; set; }

			[JsonProperty("latitude")]
			public double Latitude { get; set; }

			[JsonProperty("longitude")]
			public double Longitude { get; set; }
		}

		private int IntFromNullableBool(bool? val)
		{
			if (val == null) return 0;
			return val.Value ? 1 : 0;
		}

		public AuthPolicy(
			int? any,
			bool? requireKnowledgeFactor,
			bool? requireInherenceFactor,
			bool? requirePosessionFactor,
			bool? deviceIntegrity,
			List<Location> locations)
		{
			MinimumRequirements = new List<MinimumRequirement>();
			Factors = new List<AuthPolicyFactor>();

			if (any != null || requireKnowledgeFactor != null || requireInherenceFactor != null || requirePosessionFactor != null)
			{
				MinimumRequirements.Add(new MinimumRequirement
				{
					Requirement = MinimumRequirementType.Authenticated,
					Any = any,
					Knowledge = IntFromNullableBool(requireKnowledgeFactor),
					Inherence = IntFromNullableBool(requireInherenceFactor),
					Possession = IntFromNullableBool(requirePosessionFactor)
				});
			}
			if (deviceIntegrity != null)
			{
				Factors.Add(new AuthPolicyFactor
				{
					Factor = FactorType.DeviceIntegrity,
					Priority = 1,
					Requirement = FactorRequirementType.ForcedRequirement,
					Attributes = new AuthPolicyFactorAttributes
					{
						FactorEnabled = deviceIntegrity.Value ? 1 : 0,
						Locations = null
					}
				});
			}
			if (locations != null && locations.Count > 0)
			{
				Factors.Add(new AuthPolicyFactor
				{
					Factor = FactorType.Geofence,
					Priority = 1,
					Requirement = FactorRequirementType.ForcedRequirement,
					Attributes = new AuthPolicyFactorAttributes
					{
						Locations = locations
					}
				});
			}
		}
	}
}