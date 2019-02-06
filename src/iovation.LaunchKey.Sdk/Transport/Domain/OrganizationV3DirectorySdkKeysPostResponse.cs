using System;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class OrganizationV3DirectorySdkKeysPostResponse
    {
        [JsonProperty("sdk_key")]
        public Guid SdkKey { get; set; }
    }
}