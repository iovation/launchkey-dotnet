using System;

namespace iovation.LaunchKey.Sdk.Time
{
    /// <summary>
    /// Provides conversion services for unix timestamps. Used throughout the API to translate between DateTime objects and unix integer timestamps
    /// </summary>
    public interface IUnixTimeConverter
    {
        /// <summary>
        /// Converts a DateTime to a unix timestamp. Will handle both local and universal times
        /// </summary>
        /// <param name="time">The DateTime to convert</param>
        /// <returns>A unix timestamp</returns>
        long GetUnixTimestamp(DateTime time);

        /// <summary>
        /// Converts a Unix timestamp to a DateTime.
        /// </summary>
        /// <param name="unixTimestamp">The Unix timestamp to convert</param>
        /// <returns>A DateTime object. Universal time.</returns>
        DateTime GetDateTime(long unixTimestamp);
    }
}