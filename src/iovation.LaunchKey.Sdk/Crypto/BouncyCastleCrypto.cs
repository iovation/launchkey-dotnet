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
using Org.BouncyCastle.Math;
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
				return DotNetUtilities2.ToRSA((RsaKeyParameters)pemObject);
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
			cipher.Init(false, DotNetUtilities2.GetRsaKeyPair(privateKey).Private);
			cipher.ProcessBytes(data);
			return cipher.DoFinal();
		}

		public byte[] EncryptRSA(byte[] data, RSA publicKey)
		{
			var cipher = CipherUtilities.GetCipher(RSA_CRYPTO_CIPHER);
			cipher.Init(true, DotNetUtilities2.GetRsaPublicKey(publicKey));
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
					var cipherPair = (AsymmetricCipherKeyPair)pemObject;
					if (cipherPair.Private == null) throw new CryptographyError("No private key found in PEM object");
					if (!(cipherPair.Private is RsaPrivateCrtKeyParameters)) throw new CryptographyError("Private key is not RSA");
					return DotNetUtilities2.ToRSA((RsaPrivateCrtKeyParameters)cipherPair.Private);
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
			var keyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(DotNetUtilities2.GetRsaPublicKey(privateKey));
			var keyBytes = keyInfo.ToAsn1Object().GetDerEncoded();
			var hash = DoHash(keyBytes, new MD5Digest());
			var hashString = ByteArrayUtils.ByteArrayToHexString(hash, ":");
			return hashString;
		}
	}

	internal static class DotNetUtilities2
	{
		public static AsymmetricCipherKeyPair GetRsaKeyPair(RSA rsa)
		{
			return GetRsaKeyPair(rsa.ExportParameters(true));
		}

		public static AsymmetricCipherKeyPair GetRsaKeyPair(RSAParameters rp)
		{
			BigInteger modulus = new BigInteger(1, rp.Modulus);
			BigInteger pubExp = new BigInteger(1, rp.Exponent);

			RsaKeyParameters pubKey = new RsaKeyParameters(
				false,
				modulus,
				pubExp);

			RsaPrivateCrtKeyParameters privKey = new RsaPrivateCrtKeyParameters(
				modulus,
				pubExp,
				new BigInteger(1, rp.D),
				new BigInteger(1, rp.P),
				new BigInteger(1, rp.Q),
				new BigInteger(1, rp.DP),
				new BigInteger(1, rp.DQ),
				new BigInteger(1, rp.InverseQ));

			return new AsymmetricCipherKeyPair(pubKey, privKey);
		}

		public static RsaKeyParameters GetRsaPublicKey(RSA rsa)
		{
			return GetRsaPublicKey(rsa.ExportParameters(false));
		}

		public static RsaKeyParameters GetRsaPublicKey(
			RSAParameters rp)
		{
			return new RsaKeyParameters(
				false,
				new BigInteger(1, rp.Modulus),
				new BigInteger(1, rp.Exponent));
		}
		

		public static RSA ToRSA(RsaKeyParameters rsaKey)
		{
			// TODO This appears to not work for private keys (when no CRT info)
			return CreateRSAProvider(ToRSAParameters(rsaKey));
		}

		public static RSA ToRSA(RsaPrivateCrtKeyParameters privKey)
		{
			return CreateRSAProvider(ToRSAParameters(privKey));
		}

		public static RSAParameters ToRSAParameters(RsaKeyParameters rsaKey)
		{
			RSAParameters rp = new RSAParameters();
			rp.Modulus = rsaKey.Modulus.ToByteArrayUnsigned();
			if (rsaKey.IsPrivate)
				rp.D = ConvertRSAParametersField(rsaKey.Exponent, rp.Modulus.Length);
			else
				rp.Exponent = rsaKey.Exponent.ToByteArrayUnsigned();
			return rp;
		}

		public static RSAParameters ToRSAParameters(RsaPrivateCrtKeyParameters privKey)
		{
			RSAParameters rp = new RSAParameters();
			rp.Modulus = privKey.Modulus.ToByteArrayUnsigned();
			rp.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
			rp.P = privKey.P.ToByteArrayUnsigned();
			rp.Q = privKey.Q.ToByteArrayUnsigned();
			rp.D = ConvertRSAParametersField(privKey.Exponent, rp.Modulus.Length);
			rp.DP = ConvertRSAParametersField(privKey.DP, rp.P.Length);
			rp.DQ = ConvertRSAParametersField(privKey.DQ, rp.Q.Length);
			rp.InverseQ = ConvertRSAParametersField(privKey.QInv, rp.Q.Length);
			return rp;
		}


		// TODO Move functionality to more general class
		private static byte[] ConvertRSAParametersField(BigInteger n, int size)
		{
			byte[] bs = n.ToByteArrayUnsigned();

			if (bs.Length == size)
				return bs;

			if (bs.Length > size)
				throw new ArgumentException("Specified size too small", "size");

			byte[] padded = new byte[size];
			Array.Copy(bs, 0, padded, size - bs.Length, bs.Length);
			return padded;
		}

		private static RSA CreateRSAProvider(RSAParameters rp)
		{
			var rsaCsp = new RSACryptoServiceProvider();
			rsaCsp.ImportParameters(rp);
			return rsaCsp;
		}
	}
}