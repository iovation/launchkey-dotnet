using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Time;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Transport.WebClient;

namespace iovation.LaunchKey.Sdk
{
    /// <summary>
    /// This class creates the various factories used for interacting with the LaunchKey API.
    /// Creating an instance of this class directly is not necessary -- see <see cref="FactoryFactoryBuilder"/>, which provides
    /// useful defaults.
    /// </summary>
    public class FactoryFactory
    {
        private readonly ICrypto _crypto;
        private readonly IHttpClient _httpClient;
        private readonly ICache _keyCache;
        private readonly string _apiBaseUrl;
        private readonly string _apiIdentifier;
        private readonly int _requestExpireSeconds;
        private readonly int _offsetTtl;
        private readonly int _currentPublicKeyTtl;
        private readonly EntityKeyMap _entityKeyMap;

        /// <summary>
        /// Create an instance
        /// </summary>
        /// <param name="crypto">The crypto provider to use</param>
        /// <param name="httpClient">The HTTP client to use</param>
        /// <param name="keyCache">The caching provider to use for storing public keys</param>
        /// <param name="apiBaseUrl">The API base URL to use</param>
        /// <param name="apiIdentifier">The API audience identifier to use</param>
        /// <param name="requestExpireSeconds">The default expire time for requests in-flight</param>
        /// <param name="offsetTtl">The amount of time to cache our server-to-client time-drift</param>
        /// <param name="currentPublicKeyTtl">The amount of time to cache the server's public key</param>
        /// <param name="entityKeyMap">A list of private keys tied to entities</param>
        public FactoryFactory(ICrypto crypto, IHttpClient httpClient, ICache keyCache, string apiBaseUrl, string apiIdentifier, int requestExpireSeconds, int offsetTtl, int currentPublicKeyTtl, EntityKeyMap entityKeyMap)
        {
            _crypto = crypto;
            _httpClient = httpClient;
            _keyCache = keyCache;
            _apiBaseUrl = apiBaseUrl;
            _apiIdentifier = apiIdentifier;
            _requestExpireSeconds = requestExpireSeconds;
            _offsetTtl = offsetTtl;
            _currentPublicKeyTtl = currentPublicKeyTtl;
            _entityKeyMap = entityKeyMap;
        }

        /// <summary>
        /// Creates a factory using service credentials. Allows interacting with just services, as service credentials have no child services or directories.
        /// </summary>
        /// <param name="serviceId">The unique service ID of the service</param>
        /// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
        /// <returns>A configured ServiceFactory, ready to create ServiceClients</returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use MakeServiceFactory(string serviceId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public ServiceFactory MakeServiceFactory(string serviceId, string privateKeyPem)
        {
            var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(_crypto.LoadRsaPrivateKey(privateKeyPem));
            return MakeServiceFactory(serviceId, new List<string> {privateKeyPem}, fingerprint);
        }

        /// <summary>
        /// Creates a factory with multiple service credentials allowing for use of single purpose keys. Allows interacting with just services, as service credentials have no child services or directories.
        /// </summary>
        /// <param name="serviceId">The unique service ID of the service</param>
        /// <param name="privateKeyPems">A list of private keys to use for the entity. Should be the key itself -- not a path.</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param> 
        /// <returns>A configured ServiceFactory, ready to create ServiceClients</returns>
        [Obsolete("Reading in PEM files directly is deprecated. Please use MakeServiceFactory(string serviceId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public ServiceFactory MakeServiceFactory(string serviceId, List<string> privateKeyPems, string currentKeyId)
        {
            var keys = new Dictionary<string, RSA>();
            var id = new EntityIdentifier(EntityType.Service, Guid.Parse(serviceId));
            foreach(string key in privateKeyPems)
            {
                var parsedKey = _crypto.LoadRsaPrivateKey(key); 
                var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedKey);
                keys.Add(fingerprint, parsedKey);
            }

            return new ServiceFactory(MakeTransport(id, keys, currentKeyId), Guid.Parse(serviceId));
        }

        /// <summary>
        /// Creates a factory with multiple directory credentials allowing for use of single purpose keys. Allows interacting with any directories or services within the organization.
        /// </summary>
        /// <param name="serviceId">The unique service ID of the service</param>
        /// <param name="privateKeys">A dictionary where the key is the MD5 hash of the private key and the value is a System.Security.Cryptography.RSA object</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param>
        /// <returns></returns>
        public ServiceFactory MakeServiceFactory(string serviceId, Dictionary<string, RSA> privateKeys, string currentKeyId)
        {
            if (privateKeys.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(privateKeys));
            var id = new EntityIdentifier(EntityType.Service, Guid.Parse(serviceId));
            return new ServiceFactory(MakeTransport(id, privateKeys, currentKeyId), Guid.Parse(serviceId));            
        }

        /// <summary>
        /// Creates a factory using directory credentials. Allows interacting with the directory itself and any child services within the directory.
        /// </summary>
        /// <param name="directoryId">The unique directory ID of the directory</param>
        /// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
        /// <returns></returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use MakeDirectoryFactory(string directoryId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public DirectoryFactory MakeDirectoryFactory(string directoryId, string privateKeyPem)
        {
            var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(_crypto.LoadRsaPrivateKey(privateKeyPem));
            return MakeDirectoryFactory(directoryId, new List<string> {privateKeyPem}, fingerprint);
        }

        /// <summary>
        /// Creates a factory with multiple directory credentials allowing for use of single purpose keys. Allows interacting with the directory itself and any child services within the directory.
        /// </summary>
        /// <param name="directoryId">The unique directory ID of the directory</param>
        /// <param name="privateKeyPems">A list of private keys to use for the entity. Should be the key itself -- not a path.</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param>  
        /// <returns></returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use MakeDirectoryFactory(string directoryId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public DirectoryFactory MakeDirectoryFactory(string directoryId, List<string> privateKeyPems, string currentKeyId)
        {
            var keys = new Dictionary<string, RSA>();
            var id = new EntityIdentifier(EntityType.Directory, Guid.Parse(directoryId));
            foreach(string key in privateKeyPems)
            {
                var parsedKey = _crypto.LoadRsaPrivateKey(key); 
                var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedKey);
                keys.Add(fingerprint, parsedKey);
            }
            
            return new DirectoryFactory(MakeTransport(id, keys, currentKeyId), Guid.Parse(directoryId));
        }

        /// <summary>
        /// Creates a factory with multiple directory credentials allowing for use of single purpose keys. Allows interacting with any directories or services within the organization.
        /// </summary>
        /// <param name="directoryId">The unique directory ID of the directory</param>
        /// <param name="privateKeys">A dictionary where the key is the MD5 hash of the private key and the value is a System.Security.Cryptography.RSA object</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param>
        /// <returns></returns>
        public DirectoryFactory MakeDirectoryFactory(string directoryId, Dictionary<string, RSA> privateKeys, string currentKeyId)
        {
            if (privateKeys.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(privateKeys));
            var id = new EntityIdentifier(EntityType.Directory, Guid.Parse(directoryId));
            return new DirectoryFactory(MakeTransport(id, privateKeys, currentKeyId), Guid.Parse(directoryId));            
        }

        /// <summary>
        /// Creates a factory using organization-level credentials. Allows interacting with any directories or services within the organization.
        /// </summary>
        /// <param name="organizationId">The unique organization ID</param>
        /// <param name="privateKeyPem">The private key to use. Should be the key itself -- not a path.</param>
        /// <returns></returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use MakeOrganizationFactory(string organizationId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public OrganizationFactory MakeOrganizationFactory(string organizationId, string privateKeyPem)
        {
            var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(_crypto.LoadRsaPrivateKey(privateKeyPem));
            return MakeOrganizationFactory(organizationId, new List<string> {privateKeyPem}, fingerprint);            
        }

        /// <summary>
        /// Creates a factory with multiple organization credentials allowing for use of single purpose keys. Allows interacting with any directories or services within the organization.
        /// </summary>
        /// <param name="organizationId">The unique organization ID</param>
        /// <param name="privateKeyPems">A list of private keys to use for the entity. Should be the key itself -- not a path.</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param>
        /// <returns></returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use MakeOrganizationFactory(string organizationId, Dictionary<string, RSA> privateKeys, string currentKeyId) instead")]
        public OrganizationFactory MakeOrganizationFactory(string organizationId, List<string> privateKeyPems, string currentKeyId)
        {
            var keys = new Dictionary<string, RSA>();
            var id = new EntityIdentifier(EntityType.Organization, Guid.Parse(organizationId));
            foreach(string key in privateKeyPems)
            {
                var parsedKey = _crypto.LoadRsaPrivateKey(key); 
                var fingerprint = _crypto.GeneratePublicKeyFingerprintFromPrivateKey(parsedKey);
                keys.Add(fingerprint, parsedKey);
            }
            
            return new OrganizationFactory(MakeTransport(id, keys, currentKeyId), Guid.Parse(organizationId));            
        }

        /// <summary>
        /// Creates a factory with multiple organization credentials allowing for use of single purpose keys. Allows interacting with any directories or services within the organization.
        /// </summary>
        /// <param name="organizationId">The unique organization ID</param>
        /// <param name="privateKeys">A dictionary where the key is the MD5 hash of the private key and the value is a System.Security.Cryptography.RSA object</param>
        /// <param name="currentKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key to be used to sign requests.</param>
        /// <returns></returns>
        public OrganizationFactory MakeOrganizationFactory(string organizationId, Dictionary<string, RSA> privateKeys, string currentKeyId)
        {
            if (privateKeys.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(privateKeys));
            var id = new EntityIdentifier(EntityType.Organization, Guid.Parse(organizationId));
            return new OrganizationFactory(MakeTransport(id, privateKeys, currentKeyId), Guid.Parse(organizationId));            
        }

        private ITransport MakeTransport(
            EntityIdentifier issuer,
            Dictionary<string, RSA> privateKeys,
            string currentPrivateKey)
        {
            foreach (var key in privateKeys)
            {
                _entityKeyMap.AddKey(issuer, key.Key, key.Value);
            }

            return new WebClientTransport(
                _httpClient,
                _crypto,
                _keyCache,
                _apiBaseUrl,
                issuer,
                new JwtService(new UnixTimeConverter(), _apiIdentifier, privateKeys, currentPrivateKey, _requestExpireSeconds),
                new JweService(privateKeys),
                _offsetTtl,
                _currentPublicKeyTtl,
                _entityKeyMap,
                new JsonNetJsonEncoder()
            );
        }
    }
}