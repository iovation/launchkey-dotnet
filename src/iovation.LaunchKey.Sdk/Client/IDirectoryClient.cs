using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Directory;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IDirectoryClient : IServiceManagingClient
    {
        /// <summary>
        /// Links a device to a user.
        /// </summary>
        /// <param name="userId">The user ID to use when linking the device. This should be in the format of a GUID.</param>
        /// <returns>response data, which includes a the key to type into the phone, and a scannable QR code URL.</returns>
        DirectoryUserDeviceLinkData LinkDevice(string userId, int? ttl = null);

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
    }
}