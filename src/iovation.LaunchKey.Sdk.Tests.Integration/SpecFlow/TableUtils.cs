using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Tables;
using iovation.LaunchKey.Sdk.Transport.Domain;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
{
    public class TableUtils
    {
		public static List<Location> LocationsFromTable(Table table)
		{
			var locations = new List<Location>();

			foreach (var row in table.CreateSet<GeofenceTableRow>())
			{
				locations.Add(new Location(row.Name, row.Radius, row.Latitude, row.Longitude));
			}

			return locations;
		}

		public static List<TimeFence> TimeFencesFromTable(Table table)
		{
			var timeFences = new List<TimeFence>();

			foreach (var row in table.CreateSet<TimeFenceTableRow>())
			{
				timeFences.Add(new TimeFence(
					row.Name,
					row.Days.Split(',').Select(d => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), d)).ToList(),
					row.StartHour,
					row.StartMinute,
					row.EndHour,
					row.EndMinute,
					row.TimeZone
				));
			}

			return timeFences;
		}
    }
}
