using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
    class OrgExamples
    {
        public static int DoServiceAuth(string orgId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string serviceId, string userId, string apiURL, bool? useWebhook)
        {
            var serviceClient = ClientFactories.MakeOrganizationServiceClient(orgId, privateKey, serviceId, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoAuthorizationRequest(serviceClient, userId, useWebhook);
        }

        public static int DoDirectoryDeviceList(string orgId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string directoryId, string userId, string apiURL)
        {
            var directoryClient = ClientFactories.MakeOrganizationDirectoryClient(orgId, privateKey, directoryId, apiURL, encryptionPrivateKeys);
            try
            {
                var deviceListResponse = directoryClient.GetLinkedDevices(userId);

                Console.WriteLine();
                Console.WriteLine($"Devices ({deviceListResponse.Count} found): ");
                Console.WriteLine("-------- ");
                Console.WriteLine();

                foreach (var device in deviceListResponse)
                {
                    Console.WriteLine($" > Device {device.Id}");
                    Console.WriteLine($"    Type: {device.Type}");
                    Console.WriteLine($"    Status: {device.Status}");
                    Console.WriteLine($"    Name: {device.Name}");
                }
                Console.WriteLine();
            }
            catch (BaseException e)
            {
                Console.WriteLine($"There was an error listing the devices for the user with ID {userId}. Error: {e.Message}");
            }

            return 0;
        }

        public static int DoUpdateDirectoryWebhookUrl(string orgId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string directoryId, string apiURL, string webkhookUrl)
        {
            var orgClient = ClientFactories.MakeOrganizationClient(orgId, privateKey, apiURL);
            var directory = orgClient.GetDirectory(Guid.Parse(directoryId));
            orgClient.UpdateDirectory(Guid.Parse(directoryId), directory.Active, directory.AndroidKey, null, true, webkhookUrl);
            return 0;
        }

    }
}
