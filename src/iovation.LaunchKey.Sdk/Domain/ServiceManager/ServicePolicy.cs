using System;
using System.Collections.Generic;
using System.Linq;

namespace iovation.LaunchKey.Sdk.Domain.ServiceManager
{
	/// <summary>
	/// Represents an authentication policy used for dynamic policies per request
	/// </summary>
	public class ServicePolicy
	{
		/// <summary>
		/// Number of required factors, if any
		/// </summary>
		public int? RequiredFactors { get; set; }

		/// <summary>
		/// Whether this policy requires the 'knowledge' factor
		/// </summary>
		public bool? RequireKnowledgeFactor { get; set; }

		/// <summary>
		/// Whether this policy requires the 'inherence' factor
		/// </summary>
		public bool? RequireInherenceFactor { get; set; }

		/// <summary>
		/// Whether this policy requires the 'posession' factor
		/// </summary>
		public bool? RequirePossessionFactor { get; set; }

		/// <summary>
		/// A list of geofences this policy requires
		/// </summary>
		public List<Location> Locations { get; set; }

		/// <summary>
		/// Whether or not to enforce Jailbreak detection (device integrity) on this request
		/// </summary>
		public bool? JailbreakDetection { get; set; }

		/// <summary>
		/// A list of time fences this policy requires
		/// </summary>
		public List<TimeFence> TimeFences { get; set; }

		/// <summary>
		/// Create a service auth policy based on several options.
		/// </summary>
		/// <example>
		/// new AuthPolicy(
		///   requiredFactors: 2
		/// )
		/// new AuthPolicy(
		///   requireKnowledgeFactor: true
		/// )
		/// new AuthPolicy(
		///   requireKnowledgeFactor: true,
		///   jailBreakDetection: true
		/// )
		/// </example>
		/// <param name="requiredFactors">The minimum number of factors to allow. Note: this cannot be set when <paramref name="requireKnowledgeFactor"/>, <paramref name="requireInherenceFactor" /> or <paramref name="requirePosessionFactor"/> is set.</param>
		/// <param name="requireKnowledgeFactor">Whether this request should enforce the knowledge factor</param>
		/// <param name="requireInherenceFactor">Whether this request should enforce the inherence factor</param>
		/// <param name="requirePossessionFactor">Whether this request should enforce the possession factor</param>
		/// <param name="jailbreakDetection">Whether the service should enforce device integrity on this policy (i.e. device has been jailbroken or the application pirated)</param>
		/// <param name="locations">A list of valid geofences for this policy</param>
		/// <param name="timeFences">A list of time fences for this this policy</param>
		public ServicePolicy(
			int? requiredFactors = null,
			bool? requireKnowledgeFactor = null,
			bool? requireInherenceFactor = null,
			bool? requirePossessionFactor = null,
			bool? jailbreakDetection = null,
			List<Location> locations = null,
			List<TimeFence> timeFences = null)
		{
			Locations = locations ?? new List<Location>();
			TimeFences = timeFences ?? new List<TimeFence>();
			RequiredFactors = requiredFactors;
			RequireKnowledgeFactor = requireKnowledgeFactor;
			RequireInherenceFactor = requireInherenceFactor;
			RequirePossessionFactor = requirePossessionFactor;
			JailbreakDetection = jailbreakDetection;
		}

		internal Transport.Domain.AuthPolicy ToTransport()
		{
			return new Transport.Domain.AuthPolicy(
				RequiredFactors,
				RequireKnowledgeFactor,
				RequireInherenceFactor,
				RequirePossessionFactor,
				JailbreakDetection,
				Locations?.Select(
					loc => new Transport.Domain.AuthPolicy.Location(loc.Name, loc.Radius, loc.Latitude, loc.Longitude)
				).ToList(),
				TimeFences?.Select(
					time => new Transport.Domain.AuthPolicy.TimeFence(
						time.Name,
						time.Days.Select(d => d.ToString()).ToList(),
						time.StartHour,
						time.EndHour,
						time.StartMinute,
						time.EndMinute,
						time.TimeZone
					)
				).ToList()
			);
		}

		private static bool? ConvertNullableIntToBool(int? num)
		{
			if (num == null) return null;
			return num == 1;
		}

		internal static ServicePolicy FromTransport(Transport.Domain.AuthPolicy authPolicy)
		{
			bool? jailbreakDetection = null;
			List<Location> locations = null;
			List<TimeFence> timeFences = null;

			foreach (var factor in authPolicy.Factors)
			{
				if (factor.Factor == Transport.Domain.AuthPolicy.FactorType.DeviceIntegrity)
				{
					jailbreakDetection = factor.Attributes.FactorEnabled == 1;
				}
				else if (factor.Factor == Transport.Domain.AuthPolicy.FactorType.TimeFence)
				{
					if (timeFences == null)
						timeFences = new List<TimeFence>();

					foreach (var timeFence in factor.Attributes.TimeFences)
					{
						timeFences.Add(
							new TimeFence(
								timeFence.Name,
								timeFence.Days.Select(day => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day, true)).ToList(),
								timeFence.StartHour,
								timeFence.StartMinute,
								timeFence.EndHour,
								timeFence.EndMinute,
								timeFence.TimeZone
							)
						);
					}
				}
				else if (factor.Factor == Transport.Domain.AuthPolicy.FactorType.Geofence)
				{
					if (locations == null)
						locations = new List<Location>();

					foreach (var geoFence in factor.Attributes.Locations)
					{
						locations.Add(
							new Location(
								geoFence.Name,
								geoFence.Radius,
								geoFence.Latitude,
								geoFence.Longitude
							)
						);
					}
				}
			}

			var minRequirements = authPolicy.MinimumRequirements.FirstOrDefault();
			if (minRequirements != null)
			{
				return new ServicePolicy(
					minRequirements.Any,
					ConvertNullableIntToBool(minRequirements.Knowledge),
					ConvertNullableIntToBool(minRequirements.Inherence),
					ConvertNullableIntToBool(minRequirements.Possession),
					jailbreakDetection,
					locations,
					timeFences
				);
			}
			else
			{
				return new ServicePolicy(
					jailbreakDetection: jailbreakDetection,
					locations: locations,
					timeFences: timeFences
				);
			}
		}
	}
}
