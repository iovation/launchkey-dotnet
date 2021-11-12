using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Crypto.Jwe
{
    public class JweService : IJweService
    {
        private readonly RSA _privateKey;
        private readonly Dictionary<string, RSA> _privateKeys;

        public JweService(RSA privateKey)
        {
            _privateKey = privateKey;
            _privateKeys = null;
        }

        public JweService(Dictionary<string, RSA> privateKeys)
        {
            _privateKey = null;
            _privateKeys = privateKeys;
        }
        
        public string Decrypt(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            return Decrypt(data, GetCurrentEncryptionKey(data));
        }

        public string Decrypt(string data, RSA privateKey)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            try
            {
                return Jose.JWT.Decode(data, privateKey);
            }
            catch (Exception ex)
            {
                throw new JweException("Error performing JWE decryption, see inner exception", ex);
            }
        }

        public string Encrypt(string data, RSA publicKey, string keyId, string contentType)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
            if (string.IsNullOrWhiteSpace(keyId)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(keyId));
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(contentType));

            try
            {
                return Jose.JWT.Encode(
                    data,
                    publicKey,
                    Jose.JweAlgorithm.RSA_OAEP,
                    Jose.JweEncryption.A256CBC_HS512,
                    extraHeaders: new Dictionary<string, object>
                    {
						// JWE Key ID
						{"kid", keyId},

						// JWE Content-Type
						{"cty", contentType}
                    }
                );
            }
            catch (Exception ex)
            {
                throw new JweException("Error performing JWE encryption, see inner exception", ex);
            }
        }

        public Dictionary<string, string> GetHeaders(string data)
        {
            try
            {
                return Jose.JWT.Headers(data).ToDictionary(p => p.Key, p => p.Value.ToString());
            }
            catch (Exception ex)
            {
                throw new JweException("Error reading JWE headers, see inner exception", ex);
            }
        }

        private RSA GetCurrentEncryptionKey(string data)
        {
            // Provides backwards compatibility for dual purpose keys 
            if (_privateKeys is null || _privateKeys.Count == 0)
            {
                if (_privateKey is null)
                {
                    throw new NoKeyFoundException("No keys were passed to the JWE service");
                }

                return _privateKey;
            }

            // Obtain KID from headers and select the appropriate key from the list of the entities keys
            Dictionary<string, string> headers = GetHeaders(data);
            if (!headers.ContainsKey("kid"))
                throw new InvalidRequestException("JWE headers did not include a key id");
            
            try
            {
                return _privateKeys[headers["kid"]];
            }
            catch (Exception)
            {
                throw new NoKeyFoundException(
                    $"The key id: {headers["kid"]} could not be found in the entities available keys");
            }
        }
    }
}