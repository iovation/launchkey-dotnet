using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Transport
{
    public interface ITransport
    {
        /// <summary>
        /// Get the current system time from the API
        /// </summary>
        /// <returns></returns>
        PublicV3PingGetResponse PublicV3PingGet();

        /// <summary>
        /// Get a public key via fingerprint or the current key if fingerprint is null.
        /// </summary>
        /// <param name="publicKeyFingerprint"> MD5 fingerprint of the public key to be retrieved. If null, current key is assumed.</param>
        /// <returns>public key</returns>
        PublicV3PublicKeyGetResponse PublicV3PublicKeyGet(string publicKeyFingerprint);

        /// <summary>
        /// Initiates an authorization request
        /// </summary>
        /// <param name="request">the request data</param>
        /// <param name="subject">the entity identifier of the Service for which the authorization reurequestqest is made</param>
        /// <returns>response details</returns>
        ServiceV3AuthsPostResponse ServiceV3AuthsPost(ServiceV3AuthsPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Cancels an existing and pending authorization request.
        /// </summary>
        /// <param name="authRequestId">the unique authorization request id for the request being canceled</param>
        /// <param name="subject">the service ID associated with the auth request</param>
        void ServiceV3AuthsDelete(Guid authRequestId, EntityIdentifier subject);

        /// <summary>
        /// Retrieves the status of a pending authorization request. Will return NULL until a decision is made.
        /// </summary>
        /// <param name="authRequestId">the unique authorization request id for the request being checked</param>
        /// <param name="subject">the service ID associated with the auth request</param>
        /// <returns>response details if the request is complete, null if it is pending</returns>
        ServiceV3AuthsGetResponse ServiceV3AuthsGet(Guid authRequestId, EntityIdentifier subject);

        /// <summary>
        /// Starts a session for a user with the service ID specified
        /// </summary>
        /// <param name="request">request details</param>
        /// <param name="subject">the service ID associated with the session add request</param>
        void ServiceV3SessionsPost(ServiceV3SessionsPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Ends a session for a user with the service ID specified
        /// </summary>
        /// <param name="request">request details</param>
        /// <param name="subject">the service ID associated with the session end request</param>
        void ServiceV3SessionsDelete(ServiceV3SessionsDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// links a device to a directory user
        /// </summary>
        /// <param name="request">request details</param>
        /// <param name="subject">the directory ID associated with the link request</param>
        /// <returns>response details</returns>
        DirectoryV3DevicesPostResponse DirectoryV3DevicesPost(DirectoryV3DevicesPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// unlinks a device from a directory user
        /// </summary>
        /// <param name="request">request details</param>
        /// <param name="subject">the directory ID associated with the device unlink request</param>
        void DirectoryV3DevicesDelete(DirectoryV3DevicesDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// retrieve a list of all linked devices for a directory user
        /// </summary>
        /// <param name="request">request details</param>
        /// <param name="subject">the directory ID associated with the directory user</param>
        /// <returns>the response details; a list of linked devices</returns>
        DirectoryV3DevicesListPostResponse DirectoryV3DevicesListPost(DirectoryV3DevicesListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// retrieves a list of all open sessions for a directory user
        /// </summary>
        /// <param name="request">the request details</param>
        /// <param name="subject">the directory ID associated with the directory user</param>
        /// <returns>the response details; a list of open sessions</returns>
        DirectoryV3SessionsListPostResponse DirectoryV3SessionsListPost(DirectoryV3SessionsListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// ends all sessions associated with a directory user
        /// </summary>
        /// <param name="request">the request details</param>
        /// <param name="subject">the directory ID associated with the directory user</param>
        void DirectoryV3SessionsDelete(DirectoryV3SessionsDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// process a server-sent event, and return a properly decoded and verified response object
        /// </summary>
        /// <param name="method">The HTTP method from the HTTP request</param>
        /// <param name="path">The HTTP request path from the HTTP request</param>
        /// <param name="headers">the HTTP headers from the HTTP request received in the Webhook</param>
        /// <param name="body">the HTTP body from the HTTP request received in the Webhook</param>
        /// <returns>decoded and verified response details</returns>
        IServerSentEvent HandleServerSentEvent(Dictionary<string, List<string>> headers, string body, string method = null, string path = null);

        /// <summary>
        /// Creates a service for the given organization
        /// </summary>
        /// <param name="request">The service details</param>
        /// <param name="subject">The organization to create the service for</param>
        /// <returns>the response with the unique ID of the service created</returns>
        ServicesPostResponse OrganizationV3ServicesPost(ServicesPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Updates a service for the given organization
        /// </summary>
        /// <param name="request">The service to update</param>
        /// <param name="subject">The organization the service belongs to</param>
        /// <returns>the response with the unique ID of the service updated</returns>
        void OrganizationV3ServicesPatch(ServicesPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get a list of specific services for an organization
        /// </summary>
        /// <param name="request">The list of services to fetch</param>
        /// <param name="subject">The organization the services belong to</param>
        /// <returns>the response from the LaunchKey API</returns>
        ServicesListPostResponse OrganizationV3ServicesListPost(ServicesListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all services for an organization
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>the response from the LaunchKey API</returns>
        ServicesGetResponse OrganizationV3ServicesGet(EntityIdentifier subject);


        /// <summary>
        /// Create a directory for the given organization
        /// </summary>
        /// <param name="request">The directory details</param>
        /// <param name="subject">The organization to create the directory within</param>
        /// <returns>the response from the LaunchKey API</returns>
        OrganizationV3DirectoriesPostResponse OrganizationV3DirectoriesPost(OrganizationV3DirectoriesPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Update a directory for the given organization
        /// </summary>
        /// <param name="request">The directory details</param>
        /// <param name="subject">The organization to create the directory within</param>
        void OrganizationV3DirectoriesPatch(OrganizationV3DirectoriesPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all directories for the given organization
        /// </summary>
        /// <param name="subject">The organization to retrieve from</param>
        /// <returns>A list of directories within the organization</returns>
        OrganizationV3DirectoriesGetResponse OrganizationV3DirectoriesGet(EntityIdentifier subject);

        /// <summary>
        /// Get a list of specific directories for the given organization
        /// </summary>
        /// <param name="request">A list of directory IDs to retrieve</param>
        /// <param name="subject">The organization to retrieve from</param>
        /// <returns></returns>
        OrganizationV3DirectoriesListPostResponse OrganizationV3DirectoriesListPost(OrganizationV3DirectoriesListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Creates a service for the given directory
        /// </summary>
        /// <param name="request">The service details</param>
        /// <param name="subject">The directory to create the service for</param>
        /// <returns>the response with the unique ID of the service created</returns>
        ServicesPostResponse DirectoryV3ServicesPost(ServicesPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Updates a service for the given directory
        /// </summary>
        /// <param name="request">The service to update</param>
        /// <param name="subject">The directory the service belongs to</param>
        /// <returns>the response with the unique ID of the service updated</returns>
        void DirectoryV3ServicesPatch(ServicesPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get a list of specific services for an directory
        /// </summary>
        /// <param name="request">The list of services to fetch</param>
        /// <param name="subject">The directory the services belong to</param>
        /// <returns>the response from the LaunchKey API</returns>
        ServicesListPostResponse DirectoryV3ServicesListPost(ServicesListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Create an Authenticator SDK Key for a directory
        /// </summary>
        /// <param name="request">The directory to add the key for</param>
        /// <param name="subject">The organization the directory belongs to</param>
        /// <returns>The response from the LaunchKey API</returns>
        OrganizationV3DirectorySdkKeysPostResponse OrganizationV3DirectorySdkKeysPost(OrganizationV3DirectorySdkKeysPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Delete an Authenticator SDK Key for a directory
        /// </summary>
        /// <param name="request">Information identifying the key to delete</param>
        /// <param name="subject">The organization the directory belongs to</param>
        void OrganizationV3DirectorySdkKeysDelete(OrganizationV3DirectorySdkKeysDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all SDK keys associated with a directory within the given organization
        /// </summary>
        /// <param name="request">The directory to retrieve keys for</param>
        /// <param name="subject">The organization the directory being queried belongs to</param>
        /// <returns>The response from the LaunchKey API</returns>
        OrganizationV3DirectorySdkKeysListPostResponse OrganizationV3DirectorySdkKeysListPost(OrganizationV3DirectorySdkKeysListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all services for a directory
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>the response from the LaunchKey API</returns>
        ServicesGetResponse DirectoryV3ServicesGet(EntityIdentifier subject);

        /// <summary>
        /// Get all public keys for an organization service
        /// </summary>
        /// <param name="request">The service to request for</param>
        /// <param name="subject">The organization the service belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysListPostResponse OrganizationV3ServiceKeysListPost(ServiceKeysListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Add a public key for an organization service
        /// </summary>
        /// <param name="request">The request object with information about the service and key</param>
        /// <param name="subject">The organization the service belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysPostResponse OrganizationV3ServiceKeysPost(ServiceKeysPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Create a key for an Organization Service
        /// </summary>
        /// <param name="request">The request object with information about the service and key to update</param>
        /// <param name="subject">The organization the service belongs to</param>
        void OrganizationV3ServiceKeysPatch(ServiceKeysPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Remove a key from an Organization Service
        /// </summary>
        /// <param name="request"></param>
        /// <param name="subject">The organization the service belongs to</param>
        void OrganizationV3ServiceKeysDelete(ServiceKeysDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all public keys for an organization directory
        /// </summary>
        /// <param name="request">The directory to request for</param>
        /// <param name="subject">The organization the directory belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysListPostResponse OrganizationV3DirectoryKeysListPost(DirectoryKeysListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Add a public key for an organization directory
        /// </summary>
        /// <param name="request">The request object with information about the directory and key</param>
        /// <param name="subject">The organization the directory belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysPostResponse OrganizationV3DirectoryKeysPost(DirectoryKeysPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Create a key for an Organization Directory
        /// </summary>
        /// <param name="request"></param>
        /// <param name="subject"></param>
        void OrganizationV3DirectoryKeysPatch(DirectoryKeysPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Remove a key from an Organization Directory
        /// </summary>
        /// <param name="request"></param>
        /// <param name="subject"></param>
        void OrganizationV3DirectoryKeysDelete(DirectoryKeysDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get all public keys for an directory service
        /// </summary>
        /// <param name="request">The service to request for</param>
        /// <param name="subject">The directory the service belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysListPostResponse DirectoryV3ServiceKeysListPost(ServiceKeysListPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Add a public key for an directory service
        /// </summary>
        /// <param name="request">The request object with information about the service and key</param>
        /// <param name="subject">The directory the service belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        KeysPostResponse DirectoryV3ServiceKeysPost(ServiceKeysPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Create a key for an Directory Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The directory the service belongs to</param>
        void DirectoryV3ServiceKeysPatch(ServiceKeysPatchRequest request, EntityIdentifier subject);

        /// <summary>
        /// Remove a key from an Directory Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The directory the service belongs to</param>
        void DirectoryV3ServiceKeysDelete(ServiceKeysDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// Set the default Policy for an Organization Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The organization the service belongs to</param>
        void OrganizationV3ServicePolicyPut(ServicePolicyPutRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get the default Policy of an Organization Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The organization the service belongs to</param>
        /// <returns>The response from the API</returns>
        AuthPolicy OrganizationV3ServicePolicyItemPost(ServicePolicyItemPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Delete the default Policy of an Organization Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The organization the service belongs to</param>
        void OrganizationV3ServicePolicyDelete(ServicePolicyDeleteRequest request, EntityIdentifier subject);

        /// <summary>
        /// Set the default Policy for a Directory Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The directory the service belongs to</param>
        void DirectoryV3ServicePolicyPut(ServicePolicyPutRequest request, EntityIdentifier subject);

        /// <summary>
        /// Get the default Policy of a Directory Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The directory the service belongs to</param>
        /// <returns>the response from the LaunchKey API</returns>
        AuthPolicy DirectoryV3ServicePolicyItemPost(ServicePolicyItemPostRequest request, EntityIdentifier subject);

        /// <summary>
        /// Delete the default Policy of a Directory Service
        /// </summary>
        /// <param name="request">The request object</param>
        /// <param name="subject">The directory the service belongs to</param>
        void DirectoryV3ServicePolicyDelete(ServicePolicyDeleteRequest request, EntityIdentifier subject);
    }
}