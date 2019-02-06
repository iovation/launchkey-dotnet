using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain.ServiceManager
{
    public class TimeFence
    {
        /// <summary>
        /// The name for this time fence
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Days that this time fence is active
        /// </summary>
        public List<DayOfWeek> Days { get; }

        /// <summary>
        /// Hour within each day that this time fence begins
        /// </summary>
        public int StartHour { get; }

        /// <summary>
        /// Minute within the hour that this time fence begins
        /// </summary>
        public int StartMinute { get; }

        /// <summary>
        /// Hour within each day that this time fence ends
        /// </summary>
        public int EndHour { get; }

        /// <summary>
        /// Minute within the hour that this time fence ends
        /// </summary>
        public int EndMinute { get; }

        /// <summary>
        /// The time zone that this time fence is located in. This should be an IANA time zone identifier.
        /// For information on IANA Time Zones, see https://www.iana.org/time-zones
        /// For a mapping of Windows Time Zones to IANA Time Zones, see http://unicode.org/cldr/charts/32/supplemental/zone_tzid.html
        /// </summary>
        public string TimeZone { get; }

        /// <summary>
        /// Construct a new time fence. 
        /// </summary>
        /// <param name="name">The name of the time fence</param>
        /// <param name="days">The days this time fence is active</param>
        /// <param name="startHour">Hour within each day that this time fence begins</param>
        /// <param name="startMinute">Minute within the hour that this time fence begins</param>
        /// <param name="endHour">Hour within each day that this time fence ends</param>
        /// <param name="endMinute">Minute within the hour that this time fence ends</param>
        /// <param name="timeZone">
        /// The time zone that this time fence is located in. This should be an IANA time zone identifier.
        /// For information on IANA Time Zones, see https://www.iana.org/time-zones
        /// For a mapping of Windows Time Zones to IANA Time Zones, see http://unicode.org/cldr/charts/32/supplemental/zone_tzid.html
        /// </param>
        public TimeFence(string name, List<DayOfWeek> days, int startHour, int startMinute, int endHour, int endMinute, string timeZone)
        {
            Name = name;
            Days = days;
            StartHour = startHour;
            StartMinute = startMinute;
            EndHour = endHour;
            EndMinute = endMinute;
            TimeZone = timeZone;
        }
    }
}