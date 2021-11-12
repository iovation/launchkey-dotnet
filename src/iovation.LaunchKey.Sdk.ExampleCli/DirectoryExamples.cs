using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.ExampleCli
{
    /// <summary>
    /// All samples in this class perform actions using Directory Credentials.
    /// e.g., service and directory operations using a private key issued from a Directory,
    /// acting on the directory itself and its child services
    /// </summary>
    class DirectoryExamples
    {
        /// <summary>
        /// Start a session for a directory service and user
        /// </summary>
        public static int DoDirectoryServiceSessionStart(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string serviceId, string userId, string apiURL)
        {
            var serviceClient = ClientFactories.MakeDirectoryServiceClient(directoryId, privateKey, serviceId, apiURL);
            return SharedServiceHelpers.DoSessionStart(serviceClient, userId);
        }

        /// <summary>
        /// End a session for a directory service and user
        /// </summary>
        public static int DoDirectoryServiceSessionEnd(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string serviceId, string userId, string apiURL)
        {
            var serviceClient = ClientFactories.MakeDirectoryServiceClient(directoryId, privateKey, serviceId, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoSessionEnd(serviceClient, userId);
        }

        /// <summary>
        /// Authorize a directory user against a directory service
        /// </summary>
        public static int DoDirectoryServiceAuth(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string serviceId, string userId, string apiURL, bool? useWebhook)
        {
            var serviceClient = ClientFactories.MakeDirectoryServiceClient(directoryId, privateKey, serviceId, apiURL, encryptionPrivateKeys);
            return SharedServiceHelpers.DoAuthorizationRequest(serviceClient, userId, useWebhook);
        }

        /// <summary>
        /// List all devices associated with a user for the given directory and user id
        /// </summary>
        public static int DoDeviceList(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string userId, string apiURL)
        {
            try
            {
                var directoryClient = ClientFactories.MakeDirectoryClient(directoryId, privateKey, apiURL, encryptionPrivateKeys);
                Console.WriteLine("Sending request to get linked devices ... ");
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

        /// <summary>
        /// Unlink a device from a user
        /// </summary>
        public static int DoDeviceUnlink(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string userId, string deviceId, string apiURL)
        {
            try
            {
                var directoryClient = ClientFactories.MakeDirectoryClient(directoryId, privateKey, apiURL, encryptionPrivateKeys);
                Console.WriteLine("Sending request to unlink device ... ");
                directoryClient.UnlinkDevice(userId, deviceId);
                Console.WriteLine($"Successfully sent unlink request for device id {deviceId} on user with ID {userId}.");
            }
            catch (BaseException e)
            {
                Console.WriteLine($"There was an error unlinking the device {deviceId} for the user with ID {userId}. Error: {e.Message}");
            }

            return 0;
        }

        /// <summary>
        /// Link a device to a user. This starts the process which must be completed via the authenticator app for this directory
        /// </summary>
        public static int DoDeviceLink(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string userId, string apiURL, int? ttl, bool useWebhook)
        {
            try
            {
                var directoryClient = ClientFactories.MakeDirectoryClient(directoryId, privateKey, apiURL, encryptionPrivateKeys);
                Console.WriteLine("Sending request to begin device link ... ");
                var deviceLinkResponse = directoryClient.LinkDevice(userId, ttl);
                Console.WriteLine($"Successfully sent link request. \n Device ID: {deviceLinkResponse.DeviceId} \n Use the following code to complete the link: {deviceLinkResponse.Code}");

                if ( useWebhook == true )
                {
                    Console.WriteLine($"You wanted to retrieve the webhook so I will open a port!");
                    var openWebhookPort = SharedServiceHelpers.HandleWebhook(directoryClient);
                }

            }
            catch (BaseException e)
            {
                Console.WriteLine($"There was an error beginning the device link for the user with ID {userId}. Error: {e.Message}");
            }

            return 0;
        }

        /// <summary>
        /// List all active sessions across all services for a user within this directory
        /// </summary>
        public static int DoSessionList(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string userId, string apiURL)
        {
            try
            {
                var directoryClient = ClientFactories.MakeDirectoryClient(directoryId, privateKey, apiURL, encryptionPrivateKeys);
                Console.WriteLine("Sending request to get list of sessions ... ");
                var sessions = directoryClient.GetAllServiceSessions(userId);

                Console.WriteLine();
                Console.WriteLine($"Sessions ({sessions.Count} found): ");
                Console.WriteLine("-------- ");
                Console.WriteLine();

                foreach (var session in sessions)
                {
                    Console.WriteLine($"Session: serviceId = {session.ServiceId}, serviceName = {session.ServiceName}, created = {session.Created}");
                }
            }
            catch (BaseException e)
            {
                Console.WriteLine($"There was an error getting the active session list for the user {userId} in the directory {directoryId}. Error: {e.Message}");
            }

            return 0;
        }

        /// <summary>
        /// Purges all user sessions across all services for a user within this directory
        /// </summary>
        public static int DoSessionPurge(string directoryId, string privateKey, IEnumerable<string> encryptionPrivateKeys, string userId, string apiURL)
        {
            try
            {
                var directoryClient = ClientFactories.MakeDirectoryClient(directoryId, privateKey, apiURL, encryptionPrivateKeys);
                Console.WriteLine("Sending request to end all sessions ... ");
                directoryClient.EndAllServiceSessions(userId);
                Console.WriteLine("Done.");
            }
            catch (BaseException e)
            {
                Console.WriteLine($"There was an error ending the sessions for the user {userId} in the directory {directoryId}. Error: {e.Message}");
            }

            return 0;
        }
    }
}
