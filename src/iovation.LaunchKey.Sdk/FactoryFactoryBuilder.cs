using System;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Transport.Domain;
using iovation.LaunchKey.Sdk.Transport.WebClient;

namespace iovation.LaunchKey.Sdk
{
    /// <summary>
    /// The main entry point for most SDK consumers. Provides sensible defaults for configurable elements, as well as default wiring of all injectable dependencies.
    /// Allows for customizing of individual dependent services.
    /// Provides a handy fluent API for quick initialization.
    /// </summary>
    public class FactoryFactoryBuilder
    {
        public const string API_BASE_URL = "https://api.launchkey.com";
        private IHttpClient _httpClient;
        private ICrypto _crypto;
        private ICache _cache;
        private EntityKeyMap _entityKeyMap = new EntityKeyMap();

        private int _requestExpireSeconds = 5;
        private int _offsetTtl = 3600;
        private int _currentPublicKeyTtl = 300;
        private string _apiBaseUrl = API_BASE_URL;
        private string _apiIdentifier = "lka";

        /// <summary>
        /// Sets the TTL of requests in-flight. Should allow for some time in-flight, as well as processing time.
        /// The SDK detects time drift between the server and the client, so this value should not have to compensate for that.
        /// Default is 5 seconds.
        /// </summary>
        /// <param name="requestExpireSeconds">The amount of time, in seconds, that requests should remain valid</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetRequestExpireSeconds(int requestExpireSeconds)
        {
            _requestExpireSeconds = requestExpireSeconds;
            return this;
        }

        /// <summary>
        /// Sets the duration that the SDK will wait before synchronizing clocks with the server. Default is 1 hour. 
        /// If time drift is severe (i.e. local clock is unreliable), try setting lower.
        /// </summary>
        /// <param name="offsetTtl">The amount of time, in seconds, between clock syncs</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetOffsetTtl(int offsetTtl)
        {
            _offsetTtl = offsetTtl;
            return this;
        }

        /// <summary>
        /// Sets the duration that the SDK will cache the server's public key. This saves on network traffic, server load and client load.
        /// Default is 5 minutes.
        /// </summary>
        /// <param name="currentPublicKeyTtl">The duration, in seconds, between public key refetches</param>
        /// <returns>The builder</returns>
        [Obsolete]
        public FactoryFactoryBuilder SetCurrentPublicKeyTttl(int currentPublicKeyTtl)
        {
            return SetCurrentPublicKeyTtl(currentPublicKeyTtl);
        }

        /// <summary>
        /// Sets the duration that the SDK will cache the server's public key. This saves on network traffic, server load and client load.
        /// Default is 5 minutes.
        /// </summary>
        /// <param name="currentPublicKeyTtl">The duration, in seconds, between public key refetches</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetCurrentPublicKeyTtl(int currentPublicKeyTtl)
        {
            _currentPublicKeyTtl = currentPublicKeyTtl;
            return this;
        }

        /// <summary>
        /// Sets the IHttpClient used by the SDK. Need to control thread pools or outbound connection limits? Inject here.
        /// Defaults to the SDK's internal, HttpWebRequest based implementation which is compatible with .NET 4.0 and beyond.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetHttpClient(IHttpClient httpClient)
        {
            _httpClient = httpClient;
            return this;
        }

        /// <summary>
        /// Sets the ICrypto provider used by the SDK.
        /// Defaults to SDK's internal, BouncyCastle-based crypto provider.
        /// </summary>
        /// <param name="crypto">The Crypto provider to use</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetCrypto(ICrypto crypto)
        {
            _crypto = crypto;
            return this;
        }

        /// <summary>
        /// Sets the ICache provider used by the SDK. The SDK uses this service to store public keys retrieved from the server.
        /// Defaults to SDK's internal, basic Dictionary(T) implementation.
        /// </summary>
        /// <param name="cache">The cache provider to use</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetCache(ICache cache)
        {
            _cache = cache;
            return this;
        }

        /// <summary>
        /// Sets the API base URL used by the SDK. The SDK uses this for building outbound requests.
        /// Defaults to https://api.launchkey.com
        /// </summary>
        /// <param name="url">The base URL to use</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetApiBaseUrl(string url)
        {
            _apiBaseUrl = url;
            return this;
        }

        /// <summary>
        /// Sets the identifier used to represent the LaunchKey application. The SDK uses this to verify responses and set audiences on outbound requests.
        /// Defaults to "lka".
        /// </summary>
        /// <param name="apiIdentifier">The identifier to use.</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder SetApiIdentifier(string apiIdentifier)
        {
            _apiIdentifier = apiIdentifier;
            return this;
        }

        /// <summary>
        /// Adds an additional RSA private key for a known service. Useful if you plan to process Webhook requests for a service.
        /// </summary>
        /// <param name="serviceId">The unique service ID</param>
        /// <param name="pemKey">The private key contents</param>
        /// <returns>The builder</returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use AddServicePrivateKey(string serviceId, RSA serviceKey, string serviceKeyId)")]
        public FactoryFactoryBuilder AddServicePrivateKey(string serviceId, string pemKey)
        {
            var crypto = GetCrypto();
            var key = crypto.LoadRsaPrivateKey(pemKey);
            var fingerprint = crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Service, Guid.Parse(serviceId)),
                fingerprint,
                key
            );
            return this;
        }

        /// <summary>
        /// Adds an additional RSA private key for a known service. Useful if you plan to process Webhook requests for a service.
        /// </summary>
        /// <param name="serviceId">The unique service ID</param>
        /// <param name="serviceKey">The System.Security.Cryptography.RSA key object</param>
        /// <param name="serviceKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder AddServicePrivateKey(string serviceId, RSA serviceKey, string serviceKeyId)
        {
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Service, Guid.Parse(serviceId)),
                serviceKeyId,
                serviceKey
            );
            return this;
        }
        
        /// <summary>
        /// Adds an additional RSA private key for a known directory. Useful if you plan to process Webhook requests for a directory.
        /// </summary>
        /// <param name="directoryId">The unique directory ID</param>
        /// <param name="pemKey">The private key contents</param>
        /// <returns>The builder</returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use AddDirectoryPrivateKey(string directoryId, RSA directoryKey, string directoryKeyId)")]
        public FactoryFactoryBuilder AddDirectoryPrivateKey(string directoryId, string pemKey)
        {
            var crypto = GetCrypto();
            var key = crypto.LoadRsaPrivateKey(pemKey);
            var fingerprint = crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Directory, Guid.Parse(directoryId)),
                fingerprint,
                key
            );
            return this;
        }

        /// <summary>
        /// Adds an additional RSA private key for a known directory. Useful if you plan to process Webhook requests for a directory.
        /// </summary>
        /// <param name="directoryId">The unique directory ID</param>
        /// <param name="directoryKey">The System.Security.Cryptography.RSA key object</param>
        /// <param name="directoryKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder AddDirectoryPrivateKey(string directoryId, RSA directoryKey, string directoryKeyId)
        {
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Service, Guid.Parse(directoryId)),
                directoryKeyId,
                directoryKey
            );
            return this;
        }
        
        /// <summary>
        /// Adds an additional RSA private key for a known organization. Useful if you plan to process Webhook requests for an organization.
        /// </summary>
        /// <param name="organizationId">The unique organization ID</param>
        /// <param name="pemKey">The private key contents</param>
        /// <returns>The builder</returns>
        [Obsolete("Reading in PEM files directly is deprecated and will be removed in a future version. Please use AddOrganizationPrivateKey(string organizationId, RSA organizationKey, string organizationKeyId)")]
        public FactoryFactoryBuilder AddOrganizationPrivateKey(string organizationId, string pemKey)
        {
            var crypto = GetCrypto();
            var key = crypto.LoadRsaPrivateKey(pemKey);
            var fingerprint = crypto.GeneratePublicKeyFingerprintFromPrivateKey(key);
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Organization, Guid.Parse(organizationId)),
                fingerprint,
                key
            );
            return this;
        }

        /// <summary>
        /// Adds an additional RSA private key for a known organization. Useful if you plan to process Webhook requests for an organization.
        /// </summary>
        /// <param name="organizationId">The unique organization ID</param>
        /// <param name="organizationKey">The System.Security.Cryptography.RSA key object</param>
        /// <param name="organizationKeyId">A MD5 hash in format of aa:bb:cc:dd... representing the key ID of the key</param>
        /// <returns>The builder</returns>
        public FactoryFactoryBuilder AddOrganizationPrivateKey(string organizationId, RSA organizationKey, string organizationKeyId)
        {
            _entityKeyMap.AddKey(
                new EntityIdentifier(EntityType.Service, Guid.Parse(organizationId)),
                organizationKeyId,
                organizationKey
            );
            return this;
        }
        
        private ICrypto GetCrypto()
        {
            return _crypto ?? new BouncyCastleCrypto();
        }

        /// <summary>
        /// Creates a FactoryFactory. Call this after you have set any custom settings or dependencies using the fluent API of this class.
        /// </summary>
        /// <returns>A configured and ready-to-use FactoryFactory</returns>
        public FactoryFactory Build()
        {
            return new FactoryFactory(
                GetCrypto(),
                _httpClient ?? new WebRequestHttpClient(TimeSpan.FromSeconds(30)),
                _cache ?? new HashCache(),
                _apiBaseUrl,
                _apiIdentifier,
                _requestExpireSeconds,
                _offsetTtl,
                _currentPublicKeyTtl,
                _entityKeyMap
            );
        }
    }
}