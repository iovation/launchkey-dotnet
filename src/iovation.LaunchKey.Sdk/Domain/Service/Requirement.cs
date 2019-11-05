using System;
namespace iovation.LaunchKey.Sdk.Domain.Service
{
    public enum Requirement
    {
        /// <summary>
        /// Other exists only to allow for forward compatibility to future requirements
        /// </summary>
        OTHER = 0,

        /// <summary>
        /// AuthPolicy requirement was the type Amount
        /// </summary>
        AMOUNT,

        /// <summary>
        /// AuthPolicy requirement was of the type Types
        /// </summary>
        TYPES,

        /// <summary>
        /// AuthPolicy requirement was of the type ConditionalGeofence
        /// </summary>
        COND_GEO
    }
}
