using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Domain.Webhook;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IDirectoryClient : IServiceManagingClient, IWebhookHandler
    {
        /// <summary>
        /// Links a device to a user.
        /// </summary>
        /// <param name="userId">The user ID to use when linking the device. This should be in the format of a GUID.</param>
        /// <param name="ttl">The number of seconds the linking code returned in the response will be valid.</param>
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
        
        /// <summary>
        /// Generate and return TOTP secret data for the given user identifier.
        ///
        /// Note that a user can only have a single TOTP configured. Submitting this request when there is an
        /// existing configuration will overwrite any previous settings 
        /// </summary>
        /// <param name="userId">
        ///     Unique value identifying the End User in the your system. It is the permanent link for the End User
        ///     between the your application(s) and the iovation LaunchKey API. This will be used for validating as
        ///     well as managing TOTP.
        /// </param>
        DirectoryUserTotp GenerateUserTotp(string userId);
        
        /// <summary>
        /// Removes a TOTP configuration from a given user.
        /// </summary>
        /// <param name="userId">
        ///     Unique value identifying the End User in the your system. This value was used to create the Directory
        ///     User and Link Device
        /// </param>
        void RemoveUserTotp(string userId);
    }
}