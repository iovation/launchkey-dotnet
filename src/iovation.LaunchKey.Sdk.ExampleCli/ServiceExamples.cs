using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Service;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
    class ServiceExamples
    {
        public static int DoSessionStart(string username, string serviceId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string apiURL)
        {
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoSessionStart(serviceClient, username);
        }

        public static int DoSessionEnd(string username, string serviceId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string apiURL)
        {
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoSessionEnd(serviceClient, username);
        }

        public static int DoServiceAuthorization(string username, string serviceId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string apiURL, string context, int? ttl, string title, string pushTitle, string pushBody, int? fraudDenialreasons, int? nonFraudDenialreasons, bool? useWebhook = false, bool? advancedWebhook = false)
        {
            // Don't require the user to submit both flags to use webhooks
            if (advancedWebhook == true)
            {
                useWebhook = true;
            }
            
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoAuthorizationRequest(serviceClient, username, useWebhook, advancedWebhook, context, null, title, ttl, pushTitle, pushBody, fraudDenialreasons, nonFraudDenialreasons);
        }

        public static int DoServiceAuthorizationWithPolicy(string username, string serviceId, string privateKey, IEnumerable<string> encryptionPrivateKeys, bool jailbreakDetection, int? factors, string geofence, string apiURL)
        {
            List<Location> locations = null;
            // parse geofence input
            if (!string.IsNullOrWhiteSpace(geofence))
            {
                try
                {
                    var parts = geofence.Split(':');
                    if (parts.Length != 3)
                    {
                        Console.WriteLine("geofence should be in the format lat:lon:radius:name");
                        return 1;
                    }
                    var lat = double.Parse(parts[0]);
                    var lon = double.Parse(parts[1]);
                    var rad = double.Parse(parts[2]);
                    var name = parts[3];

                    locations = new List<Location>();
                    locations.Add(new Location(rad, lat, lon, name));
                }
                catch (FormatException)
                {
                    Console.WriteLine("geofence parsing failed");
                    return 1;
                }
            }
            Console.WriteLine($"Using policy: factors: {factors}, locations: {locations?.Count}, geofence: {geofence}, jailbreak: {jailbreakDetection}");
                var policy = new AuthPolicy(
                    jailbreakDetection: jailbreakDetection,
                    locations: locations,
                    requiredFactors: factors
                );

            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoAuthorizationRequest(serviceClient, username, false, policy: policy);
        }

        public static object DoServiceAuthorizationCancel(string serviceId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string apiURL, string authorizationRequestId)
        {
            var serviceClient = ClientFactories.MakeServiceClient(serviceId, privateKey, apiURL, encryptionPrivateKeys);

            return SharedServiceHelpers.DoAuthorizationCancel(serviceClient, authorizationRequestId);
        }
    }
}
