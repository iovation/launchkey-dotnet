using System;
using System.IO;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Util;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace iovation.LaunchKey.Sdk.Crypto
{
	public class BouncyCastleCrypto : ICrypto
	{
		private static readonly string RSA_CRYPTO_CIPHER = "RSA/ECB/OAEPWithSHA1AndMGF1Padding";

		public RSA LoadRsaPublicKey(string keyContents)
		{
			if (keyContents == null) throw new ArgumentNullException(nameof(keyContents));
			var stringReader = new StringReader(keyContents);
			var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(stringReader);
			var pemObject = pemReader.ReadObject();
			if (pemObject is RsaKeyParameters)
			{
				return DotNetUtilities.ToRSA((RsaKeyParameters) pemObject);
			}

			throw new CryptographyError($"Failed to load public key from PEM file.");
		}

		private byte[] DoHash(byte[] data, IDigest digest)
		{
			var result = new byte[digest.GetDigestSize()];
			digest.BlockUpdate(data, 0, data.Length);
			digest.DoFinal(result, 0);
			return result;
		}

		public byte[] DecryptRSA(byte[] data, RSA privateKey)
		{
			var cipher = CipherUtilities.GetCipher(RSA_CRYPTO_CIPHER);
			cipher.Init(false, DotNetUtilities.GetRsaKeyPair(privateKey).Private);
			cipher.ProcessBytes(data);
			return cipher.DoFinal();
		}

		public byte[] EncryptRSA(byte[] data, RSA publicKey)
		{
			var cipher = CipherUtilities.GetCipher(RSA_CRYPTO_CIPHER);
			cipher.Init(true, DotNetUtilities.GetRsaPublicKey(publicKey));
			cipher.ProcessBytes(data);
			return cipher.DoFinal();
		}

		public byte[] Sha256(byte[] data)
		{
			return DoHash(data, new Sha256Digest());
		}

		public byte[] Sha384(byte[] data)
		{
			return DoHash(data, new Sha384Digest());
		}

		public byte[] Sha512(byte[] data)
		{
			return DoHash(data, new Sha512Digest());
		}

		public RSA LoadRsaPrivateKey(string keyContents)
		{
			if (keyContents == null) throw new ArgumentNullException(nameof(keyContents));
			try
			{
				var stringReader = new StringReader(keyContents);
				var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(stringReader);
				var pemObject = pemReader.ReadObject();
				if (pemObject is AsymmetricCipherKeyPair)
				{
					var cipherPair = (AsymmetricCipherKeyPair) pemObject;
					if (cipherPair.Private == null) throw new CryptographyError("No private key found in PEM object");
					if (!(cipherPair.Private is RsaPrivateCrtKeyParameters)) throw new CryptographyError("Private key is not RSA");
					return DotNetUtilities.ToRSA((RsaPrivateCrtKeyParameters) cipherPair.Private);
				}

				throw new CryptographyError($"Failed to load public key from PEM file. Object was not of type expected. ({pemObject})");
			}
			catch (CryptographyError)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new CryptographyError("Unknown error occurred while parsing PEM data, see inner exception", ex);
			}
		}

		public string GeneratePublicKeyFingerprintFromPrivateKey(RSA privateKey)
		{
			if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
			var keyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(DotNetUtilities.GetRsaPublicKey(privateKey));
			var keyBytes = keyInfo.ToAsn1Object().GetDerEncoded();
			var hash = DoHash(keyBytes, new MD5Digest());
			var hashString = ByteArrayUtils.ByteArrayToHexString(hash, ":");
			return hashString;
		}
	}
}