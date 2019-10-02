using System;
using System.Collections.Generic;
using System.Linq;
using iovation.LaunchKey.Sdk.Domain.Service.Policy;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
using iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow.Tables;
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

        public static List<IFence> GeoCircleFenceFromTable(Table table)
        {
            var fences = new List<IFence>();

            foreach(var fence in table.CreateSet<GeofenceTableRow>())
            {
                fences.Add(new GeoCircleFence(
                        latitude: fence.Latitude,
                        longitude: fence.Longitude,
                        radius: fence.Radius,
                        name: fence.Name
                ));
            }

            return fences;
        }

        public static List<IFence> TerritoryFenceFromTable(Table table)
        {
            var fences = new List<IFence>();

            foreach (var fence in table.CreateSet<TerritoryFenceTableRow>())
            {
                fences.Add(new TerritoryFence(
                    country: fence.Country,
                    administrativeArea: fence.AdministrativeArea,
                    postalCode: fence.PostalCode,
                    name: fence.Name
                ));
            }

            return fences;
        }
    }
}
