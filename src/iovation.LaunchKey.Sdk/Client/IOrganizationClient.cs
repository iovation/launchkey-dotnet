using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain;
using iovation.LaunchKey.Sdk.Domain.Organization;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IOrganizationClient : IServiceManagingClient
    {
        /// <summary>
        /// Creates a directory within the organization
        /// </summary>
        /// <param name="name">The name of the directory</param>
        /// <returns>The unique ID of the directory created</returns>
        Guid CreateDirectory(string name);

        /// <summary>
        /// Updates an existing directory within the organization
        /// </summary>
        /// <param name="directoryId">The unique ID of the directory to update</param>
        /// <param name="active">Whether or not the directory should be active</param>
        /// <param name="androidKey">GCM push key</param>
        /// <param name="iosP12">APNS push certificate in .P12 format that has been Base64 Encoded</param>
        /// <param name="webhookUrl"> The URL that directory webhooks will be sent to</param>
        void UpdateDirectory(Guid directoryId, bool active, string androidKey, string iosP12, bool? denialContextInquiryEnabled = null, string webhookUrl = null);

        /// <summary>
        /// Get a directory from this organization
        /// </summary>
        /// <param name="directoryId">The unique ID of the directory to fetch</param>
        /// <returns>The directory object</returns>
        Directory GetDirectory(Guid directoryId);

        /// <summary>
        /// Get multiple directories from this organization in a single API call
        /// </summary>
        /// <param name="directoryIds">A list of IDs representing the directories to fetch</param>
        /// <returns>A list of directory objects</returns>
        List<Directory> GetDirectories(List<Guid> directoryIds);

        /// <summary>
        /// Get all directories within this organization
        /// </summary>
        /// <returns>The directories for this organization</returns>
        List<Directory> GetAllDirectories();

        /// <summary>
        /// Request the Platform API to generate a new Authenticator SDK Key and add to the Directory identified by the
        /// provided Directory ID. Once generated and added, it will be returned as the response.
        /// </summary>
        /// <param name="directoryId">The directory to create the SDK key for</param>
        /// <returns></returns>
        Guid GenerateAndAddDirectorySdkKey(Guid directoryId);

        /// <summary>
        /// Remove an SDK key from a directory within the organization
        /// </summary>
        /// <param name="directoryId">The directory to remove the SDK Key from</param>
        /// <param name="sdkKey">The SDK Key to remove</param>
        void RemoveDirectorySdkKey(Guid directoryId, Guid sdkKey);

        /// <summary>
        /// Get all SDK keys associated with a directory within this organization
        /// </summary>
        /// <param name="directoryId">The directory to query for SDK keys</param>
        /// <returns>A list of SDK Keys</returns>
        List<Guid> GetAllDirectorySdkKeys(Guid directoryId);

        /// <summary>
        /// Get a list of Public Keys for a Directory
        /// </summary>
        /// <param name="directoryId">The directory to retrieve keys for</param>
        /// <returns></returns>
        List<PublicKey> GetDirectoryPublicKeys(Guid directoryId);

        /// <summary>
        /// Add a Public Key for a Directory
        /// </summary>
        /// <param name="directoryId">The directory to add the key for</param>
        /// <param name="publicKeyPem">The public key (in PEM format) to add</param>
        /// <param name="active">Whether or not the key should be active</param>
        /// <param name="expires">The time at which the key should no longer be active</param>
        /// <param name="keyType">KeyType enum to identify whether the key is an encryption key, signature key, or a dual use key
        /// <returns>The key ID</returns>
        string AddDirectoryPublicKey(Guid directoryId, string publicKeyPem, bool active, DateTime? expires, KeyType keyType);

        /// <summary>
        /// Add a Public Key for a Directory
        /// </summary>
        /// <param name="directoryId">The directory to add the key for</param>
        /// <param name="publicKeyPem">The public key (in PEM format) to add</param>
        /// <param name="active">Whether or not the key should be active</param>
        /// <param name="expires">The time at which the key should no longer be active</param>
        /// <returns>The key ID</returns>
        string AddDirectoryPublicKey(Guid directoryId, string publicKeyPem, bool active, DateTime? expires);

        /// <summary>
        /// Update a Public Key for a Directory
        /// </summary>
        /// <param name="directoryId">The directory to update a key within</param>
        /// <param name="keyId">The ID of the key to update</param>
        /// <param name="active">Whether or not the key should be active</param>
        /// <param name="expires">The time at which the key should no longer be active</param>
        void UpdateDirectoryPublicKey(Guid directoryId, string keyId, bool active, DateTime? expires);

        /// <summary>
        /// Remove a Public Key from a Service. You may not remove the only Public Key from a Service.
        /// To deactivate a key, rather than remove, see <see cref="UpdateDirectoryPublicKey(Guid, string, bool, DateTime?)"/>
        /// </summary>
        /// <param name="directoryId">The directory to remove a public key from</param>
        /// <param name="keyId">The key to remove</param>
        void RemoveDirectoryPublicKey(Guid directoryId, string keyId);
    }
}