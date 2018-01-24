using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace iovation.LaunchKey.Sdk.Crypto
{
	/// <summary>
	/// Provides general cryptographic functions
	/// </summary>
	public interface ICrypto
	{
		/// <summary>
		/// Decrypts RSA encrypted binary data
		/// </summary>
		/// <param name="data">the data to decrypt</param>
		/// <param name="privateKey">the RSA private key to use when decrypting</param>
		/// <returns>decrypted data</returns>
		byte[] DecryptRSA(byte[] data, RSA privateKey);

		/// <summary>
		/// Encrypts data using RSA
		/// </summary>
		/// <param name="data">the data to encrypt</param>
		/// <param name="publicKey">the RSA public key to use when encrypting</param>
		/// <returns>encrypted data</returns>
		byte[] EncryptRSA(byte[] data, RSA publicKey);

		/// <summary>
		/// computes a SHA-256 hash
		/// </summary>
		/// <param name="data">the data to hash</param>
		/// <returns>the computed hash</returns>
		byte[] Sha256(byte[] data);


		/// <summary>
		/// computes a SHA-384 hash
		/// </summary>
		/// <param name="data">the data to hash</param>
		/// <returns>the computed hash</returns>
		byte[] Sha384(byte[] data);
		
		/// <summary>
		/// computes a SHA-512 hash
		/// </summary>
		/// <param name="data">the data to hash</param>
		/// <returns>the computed hash</returns>
		byte[] Sha512(byte[] data);

		/// <summary>
		/// Loads private key from PEM file contents
		/// </summary>
		/// <param name="keyContents">The PEM-format key</param>
		/// <returns>an RSACryptoServiceProvider configured with the key</returns>
		RSA LoadRsaPrivateKey(string keyContents);

		/// <summary>
		/// Loads public key from PEM file contents
		/// </summary>
		/// <param name="keyContents">The PEM-format key</param>
		/// <returns>an RSACryptoServiceProvider configured with the key</returns>
		RSA LoadRsaPublicKey(string keyContents);

		/// <summary>
		/// Extracts a public key fingerprint from an RSA private key using MD5.
		/// </summary>
		/// <param name="privateKey">The RSA private key to use when generating the public key and its fingerprint</param>
		/// <returns>an MD5 hash in the format of (aa:bb:cc:dd...)</returns>
		string GeneratePublicKeyFingerprintFromPrivateKey(RSA privateKey);
	}
}
