using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.Webhook;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IDirectoryClient : IServiceManagingClient
    {
        /// <summary>
        /// Links a device to a user.
        /// </summary>
        /// <param name="userId">The user ID to use when linking the device. This should be in the format of a GUID.</param>
        /// <param name="ttl">The number of seconds the linking code returned in the response will be valid.</param>
        /// <returns>response data, which includes a the key to type into the phone, and a scannable QR code URL.</returns>
        DeviceLinkData LinkDevice(string userId, int? ttl = null);

        /// <summary>
        /// Gets a list of devices linked to a user.
        /// </summary>
        /// <param name="userId">The user ID of the user. This should be in the format of a GUID.</param>
        /// <returns>The list of devices linked with the user. May be empty.</returns>
        List<Device> GetLinkedDevices(string userId);

        /// <summary>
        /// Given a user id and a device id, unlink the device from the user. The device id can be retrieved via GetLinkedDevices().
        /// </summary>
        /// <param name="userId">The user ID of the user. This should be in the format of a GUID.</param>
        /// <param name="deviceId">The device ID of the device.</param>
        void UnlinkDevice(string userId, string deviceId);

        /// <summary>
        /// Given a user id, retrieve a list of all active sessions for the user
        /// </summary>
        /// <param name="userId">The user ID of the user. This should be in the format of a GUID.</param>
        /// <returns></returns>
        List<Session> GetAllServiceSessions(string userId);

        /// <summary>
        /// Terminates all sessions associated with a user.
        /// </summary>
        /// <param name="userId">The user ID of the user. This should be in the format of a GUID.</param>
        void EndAllServiceSessions(string userId);

        /// <summary>
        /// Process a Webhook payload received from the LaunchKey WebHook service.
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <param name="method">The HTTP method of the received request. Optional. Include for stricter security checks.</param>
        /// <param name="path">The HTTP path of the received request. Optional. Include for stricter security checks.</param>
        /// <returns></returns>
        IWebhookPackage HandleWebhook(Dictionary<string, List<string>> headers, string body, string method = null, string path = null);

    }
}