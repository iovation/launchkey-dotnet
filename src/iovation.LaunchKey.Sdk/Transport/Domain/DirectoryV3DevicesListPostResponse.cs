using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DirectoryV3DevicesListPostResponse
    {
        [JsonProperty("devices")]
        public List<Device> Devices { get; }

        public DirectoryV3DevicesListPostResponse(List<Device> devices)
        {
            Devices = devices;
        }

        public class Device
        {
            [JsonProperty("id")]
            public Guid Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("status")]
            public int Status { get; set; }

            [JsonProperty("created")]
            public DateTime Created { get; set; }

            [JsonProperty("updated")]
            public DateTime Updated { get; set; }
        }
    }
}