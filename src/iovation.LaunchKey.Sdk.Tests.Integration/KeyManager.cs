using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Crypto;

namespace iovation.LaunchKey.Sdk.Tests.Integration
{
	public class KeyManager
	{
		private string ReadTextFile(string keyName)
		{
			return File.ReadAllText(Path.Combine("Keys", keyName));
		}

		private byte[] ReadBinaryFile(string keyName)
		{
			return File.ReadAllBytes(Path.Combine("Keys", keyName));
		}

		private string ReadBinaryFileAsB64(string keyName)
		{
			return Convert.ToBase64String(ReadBinaryFile(keyName));
		}

		public string GetBase64EncodedAlphaP12()
		{
			return ReadBinaryFileAsB64("alpha-cert.p12");
		}

		public string GetBase64EncodedBetaP12()
		{
			return ReadBinaryFileAsB64("beta-cert.p12");
		}

		public string GetAlphaCertificateFingerprint()
		{
			return ReadTextFile("alpha-cert-sha256-fingerprint.txt");
		}

		public string GetBetaCertificateFingerprint()
		{
			return ReadTextFile("beta-cert-sha256-fingerprint.txt");
		}

		public RSA GetAlphaPublicKey()
		{
			var crypto = new BouncyCastleCrypto();
			return crypto.LoadRsaPublicKey(ReadTextFile("alpha-public-key.pem"));
		}

		public RSA GetBetaPublicKey()
		{
			var crypto = new BouncyCastleCrypto();
			return crypto.LoadRsaPublicKey(ReadTextFile("beta-public-key.pem"));
		}
	}
}
