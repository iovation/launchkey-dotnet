using System;
namespace iovation.LaunchKey.Sdk.Domain.Service.Policy
{
    /// <summary>
    /// Object that represents a Territory that can be used in an Authorization Request
    /// </summary>
    public class TerritoryFence : IFence
    {
        /// <summary>
        /// The ISO 3166-1 Alpha-2 code representation of a country. Ex: US, GB, DE
        /// </summary>
        public String Country { get; }

        /// <summary>
        /// The ISO 3166-2 subdivision code representation of a subdivision. Ex: US-NV, GB-ENG, AU-VIC
        /// </summary>
        public String AdministrativeArea { get; }

        /// <summary>
        /// A string representation of a local postal code
        /// </summary>
        public String PostalCode { get; }

        /// <summary>
        /// The name of the Fence
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Object that represents a Territory that can be used in an Authorization Request
        /// Country is the only required field
        /// </summary>
        /// <example>
        /// new TerritoryFence("US", "US-CA", "90002", "Los Angeles")
        /// new TerritoryFence("US", administrativeArea: "US-CA", name: "California")
        /// new TerritoryFence("US", name: "United States")
        /// </example>
        /// <param name="country"></param>
        /// <param name="administrativeArea"></param>
        /// <param name="postalCode"></param>
        /// <param name="name"></param>
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
