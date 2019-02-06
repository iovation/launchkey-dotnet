using System.Collections.Generic;
using System.Security.Cryptography;

namespace iovation.LaunchKey.Sdk.Crypto.Jwe
{
    public interface IJweService
    {
        /// <summary>
        /// Given a JWE-encrypted payload, decrypt it, and return the unecnrypted result.
        /// This function also performs validity checks, including signature checks
        /// </summary>
        /// <param name="data">The JWE payload</param>
        /// <returns>The decrytped data</returns>
        string Decrypt(string data);

        /// <summary>
        /// Given a JWE-encrypted payload, decrypt it, and return the unecnrypted result.
        /// This function also performs validity checks, including signature checks.
        /// </summary>
        /// <param name="data">The JWE payload</param>
        /// <param name="privateKey">The RSA private key to use when decrypting it</param>
        /// <returns>The decrytped data</returns>
        string Decrypt(string data, RSA privateKey);

        /// <summary>
        /// Encrypts <paramref name="data"/> using <paramref name="publicKey"/> as JWE
        /// </summary>
        /// <param name="data">the plaintext to encrypt</param>
        /// <param name="publicKey">the rsa public key to use. Usually this is an RSACryptoServiceProvider from an X509Certificate2 or a PEM file loaded via BouncyCastle.</param>
        /// <param name="keyId">the key id (kid) header to use</param>
        /// <param name="contentType">the content type (cty) header to use</param>
        /// <returns>encrypted payload</returns>
        string Encrypt(string data, RSA publicKey, string keyId, string contentType);

        /// <summary>
        /// Returns just the JWE headers of a given JWE payload.
        /// </summary>
        /// <param name="data">The encrypted payload</param>
        /// <returns>the JWE headers decoded from the payload</returns>
        Dictionary<string, string> GetHeaders(string data);
    }
}