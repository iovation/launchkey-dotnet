using System;
namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    public class TerritoryFence : IFence
    {
        public String Country { get; }
        public String AdministrativeArea { get; }
        public String PostalCode { get; }
        public String Name { get; }

        public TerritoryFence(
            String country,
            String administrativeArea=null,
            String postalCode=null,
            String name=null)
        {
            Country = country;
            AdministrativeArea = administrativeArea;
            PostalCode = postalCode;
            Name = name;
        }
    }
}
