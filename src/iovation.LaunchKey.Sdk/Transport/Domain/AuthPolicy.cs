using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class AuthPolicy : IPolicy
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum MinimumRequirementType
        {
            [EnumMember(Value = "authenticated")]
            Authenticated,

            [EnumMember(Value = "enabled")]
            Enabled
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum FactorType
        {
            [EnumMember(Value = "geofence")]
            Geofence,

            [EnumMember(Value = "device integrity")]
            DeviceIntegrity,

            [EnumMember(Value = "timefence")]
            TimeFence
        }

        [JsonConverter(typeof(StringEnumConverter))]
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
            public int? Knowledge { get; set; }

            [JsonProperty("inherence")]
            public int? Inherence { get; set; }

            [JsonProperty("possession")]
            public int? Possession { get; set; }
        }

        public class AuthPolicyFactor
        {
            [JsonProperty("factor")]
            public FactorType Factor { get; set; }

            [JsonProperty("requirement")]
            public FactorRequirementType Requirement { get; set; }

            [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
            public int? Priority { get; set; }

            [JsonProperty("attributes")]
            public AuthPolicyFactorAttributes Attributes { get; set; }
        }

        public class AuthPolicyFactorAttributes
        {
            [JsonProperty("factor enabled", NullValueHandling = NullValueHandling.Ignore)]
            public int? FactorEnabled { get; set; }

            [JsonProperty("locations", NullValueHandling = NullValueHandling.Ignore)]
            public List<Location> Locations { get; set; }

            [JsonProperty("time fences", NullValueHandling = NullValueHandling.Ignore)]
            public List<TimeFence> TimeFences { get; set; }
        }

        public class Location
        {
            [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
            public string Name { get; set; }

            [JsonProperty("radius")]
            public double Radius { get; set; }

            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }

            public Location(string name, double radius, double latitude, double longitude)
            {
                Name = name;
                Radius = radius;
                Latitude = latitude;
                Longitude = longitude;
            }

            public Location()
            {
            }

        }

        public class TimeFence
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("days")]
            public List<string> Days { get; set; }

            [JsonProperty("start hour")]
            public int StartHour { get; set; }

            [JsonProperty("end hour")]
            public int EndHour { get; set; }

            [JsonProperty("start minute")]
            public int StartMinute { get; set; }

            [JsonProperty("end minute")]
            public int EndMinute { get; set; }

            [JsonProperty("timezone")]
            public string TimeZone { get; set; }

            public TimeFence(string name, List<string> days, int startHour, int endHour, int startMinute, int endMinute, string timeZone)
            {
                Name = name;
                Days = days;
                StartHour = startHour;
                EndHour = endHour;
                StartMinute = startMinute;
                EndMinute = endMinute;
                TimeZone = timeZone;
            }

            internal TimeFence()
            {
            }
        }

        public class JWEAuthPolicy
        {
            [JsonProperty("requirement")]
            public string Requirement { get; set; }

            [JsonProperty("geofences")]
            public Location[] Geofences { get; set; }

            [JsonProperty("amount")]
            public int Amount { get; set; }

            [JsonProperty("types")]
            public List<string> Types { get; set; }

            public override string ToString()
            {
                return string.Format("Requirement: {0},\n Geofences: {1},\n Amount: {2},\n Types: {3}",Requirement, Geofences.ToString(), Amount, Types);
            }
        }

        public class AuthMethod
        {
            [JsonProperty("method")]
            public string Method { get; set; }

            [JsonProperty("set", NullValueHandling = NullValueHandling.Include)]
            public bool? Set { get; set; }

            [JsonProperty("active")]
            public bool Active { get; set; }

            [JsonProperty("allowed")]
            public bool Allowed { get; set; }

            [JsonProperty("supported")]
            public bool Supported { get; set; }

            [JsonProperty("user_required", NullValueHandling = NullValueHandling.Include)]
            public bool? UserRequired { get; set; }

            [JsonProperty("passed", NullValueHandling = NullValueHandling.Include)]
            public bool? Passed { get; set; }

            [JsonProperty("error", NullValueHandling = NullValueHandling.Include)]
            public bool? Error { get; set; }

            public override string ToString()
            {
                return string.Format("Auth Method: {0} \n Set: {1} \n Active: {2} \n Allowed: {3} \n Supported: {4} \n UserRequired: {5} \n Passed: {6} \n Error: {7} \n", 
                    Method, Set, Active, Allowed, Supported, UserRequired, Passed, Error);
            }

        }

        private int? IntFromNullableBool(bool? val)
        {
            if (val == null) return null;
            return val.Value ? 1 : 0;
        }

        public AuthPolicy(
            int? any,
            bool? requireKnowledgeFactor,
            bool? requireInherenceFactor,
            bool? requirePossessionFactor,
            bool? deviceIntegrity,
            List<Location> locations,
            List<TimeFence> timeFences = null)
        {
            MinimumRequirements = new List<MinimumRequirement>();
            Factors = new List<AuthPolicyFactor>();

            if (any != null || requireKnowledgeFactor != null || requireInherenceFactor != null || requirePossessionFactor != null)
            {
                MinimumRequirements.Add(new MinimumRequirement
                {
                    Requirement = MinimumRequirementType.Authenticated,
                    Any = any,
                    Knowledge = IntFromNullableBool(requireKnowledgeFactor),
                    Inherence = IntFromNullableBool(requireInherenceFactor),
                    Possession = IntFromNullableBool(requirePossessionFactor)
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

            if (deviceIntegrity != null)
            {
                Factors.Add(new AuthPolicyFactor
                {
                    Factor = FactorType.DeviceIntegrity,
                    Requirement = FactorRequirementType.ForcedRequirement,
                    Attributes = new AuthPolicyFactorAttributes
                    {
                        FactorEnabled = deviceIntegrity.Value ? 1 : 0,
                        Locations = null
                    }
                });
            }

            if (timeFences != null && timeFences.Count > 0)
            {
                Factors.Add(new AuthPolicyFactor
                {
                    Factor = FactorType.TimeFence,
                    Priority = 1,
                    Requirement = FactorRequirementType.ForcedRequirement,
                    Attributes = new AuthPolicyFactorAttributes
                    {
                        TimeFences = timeFences
                    }
                });
            }

            //Type = "LEGACY";
        }
    }
}