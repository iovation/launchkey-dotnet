using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.Service
{
	/// <summary>
	/// Represents an authentication policy used for dynamic policies per request
	/// </summary>
	public class AuthPolicy
	{
		/// <summary>
		/// Number of required factors, if any
		/// </summary>
		public int? RequiredFactors { get; }

		/// <summary>
		/// Whether this policy requires the 'knowledge' factor
		/// </summary>
		public bool? RequireKnowledgeFactor { get; }

		/// <summary>
		/// Whether this policy requires the 'inherence' factor
		/// </summary>
		public bool? RequireInherenceFactor { get; }

		/// <summary>
		/// Whether this policy requires the 'posession' factor
		/// </summary>
		public bool? RequirePosessionFactor { get; }

		/// <summary>
		/// A list of geofences this policy requires
		/// </summary>
		public List<Location> Locations { get; }

		/// <summary>
		/// Whether or not to enforce Jailbreak detection (device integrity) on this request
		/// </summary>
		public bool JailbreakDetection { get; }

		/// <summary>
		/// Create an auth policy based on several options.
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
		/// <param name="requirePosessionFactor">Whether this request should enforce the posession factor</param>
		/// <param name="jailbreakDetection">Whether the service should enforce device integrity on this request (i.e. device has been jailbroken or the application pirated)</param>
		/// <param name="locations">A list of valid geofences for this request</param>
		public AuthPolicy(
			int? requiredFactors = null,
			bool? requireKnowledgeFactor = null,
			bool? requireInherenceFactor = null,
			bool? requirePosessionFactor = null,
			bool jailbreakDetection = false,
			List<Location> locations = null)
		{
			if (requiredFactors.HasValue && (requireKnowledgeFactor.HasValue || requireInherenceFactor.HasValue || requirePosessionFactor.HasValue))
				throw new ArgumentException($"Invalid argument combination; requiredFactors should only be set in absence of requireKnowledgeFactor, requireInherenceFactor and requirePosessionFactor, as they are mutually exclusive");

			Locations = locations ?? new List<Location>();
			RequiredFactors = requiredFactors;
			RequireKnowledgeFactor = requireKnowledgeFactor;
			RequireInherenceFactor = requireInherenceFactor;
			RequirePosessionFactor = requirePosessionFactor;
			JailbreakDetection = jailbreakDetection;
		}
	}
}
