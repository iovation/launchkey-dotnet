using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Crypto.Jwt
{
	public interface IJwtService
	{
		/// <summary>
		/// Creates a LaunchKey custom JWT token given the inputs
		/// </summary>
		/// <param name="jti">JWT jti field</param>
		/// <param name="issuer">JWT iss field</param>
		/// <param name="subject">JWT sub field</param>
		/// <param name="currentTime">The current time</param>
		/// <param name="method">The HTTP request method associated with this JWT token</param>
		/// <param name="path">The HTTP request path associated with this JWT token</param>
		/// <param name="contentHashAlgorithm">The hash function used when hashing the body of the request</param>
		/// <param name="contentHash">The computed hash, using the function specified by <paramref name="contentHashAlgorithm" />.</param>
		/// <returns></returns>
		string Encode(
			string jti,
			string issuer,
			string subject,
			DateTime currentTime,
			string method,
			string path,
			string contentHashAlgorithm,
			string contentHash
		);

		/// <summary>
		/// Decodes a LaunchKey custom JWT token from an encoded payload
		/// </summary>
		/// <param name="publicKey">the RSA public key to use when verifying the signature of the JWT token</param>
		/// <param name="expectedAudience">The expected audience (aud)</param>
		/// <param name="expectedTokenId">The expected token id (jti)</param>
		/// <param name="currentTime">The current time, used for verifying the token</param>
		/// <param name="jwt">The encoded JWT token</param>
		/// <returns>A decoded JWT token</returns>
		JwtClaims Decode(
			RSA publicKey,
			string expectedAudience,
			string expectedTokenId,
			DateTime currentTime,
			string jwt
		);

		/// <summary>
		/// Decodes header information associated with the JWT token, bypassing some validity checks
		/// </summary>
		/// <param name="jwt">The encoded JWT token</param>
		/// <returns>The header data decoded</returns>
		JwtData GetJWTData(string jwt);
	}
}