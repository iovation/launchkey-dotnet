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

		public JweService(RSA privateKey)
		{
			_privateKey = privateKey;
		}

		public string Decrypt(string data)
		{
			if (string.IsNullOrWhiteSpace(data)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(data));
			return Decrypt(data, _privateKey);
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
	}
}