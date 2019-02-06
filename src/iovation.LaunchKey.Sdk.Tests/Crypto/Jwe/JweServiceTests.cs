using System;
using System.IO;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwe;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Crypto.Jwe
{
    [TestClass]
    public class JweServiceTests
    {
        private JweService _jwe;
        private RSA _rsaPrivate;
        private RSA _rsaPublic;

        [TestInitialize]
        public void Init()
        {
            var crypto = new BouncyCastleCrypto();

            _rsaPrivate = crypto.LoadRsaPrivateKey(File.ReadAllText("test-private.key"));
            _rsaPublic = crypto.LoadRsaPublicKey(File.ReadAllText("test-public.key"));

            _jwe = new JweService(_rsaPrivate);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullDataOnEncrypt()
        {
            _jwe.Encrypt(null, new RSACryptoServiceProvider(), "key", "application/dontmatter");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestNullKeyOnEncrypt()
        {
            _jwe.Encrypt("data", null, "key", "application/dontmatter");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullKeyIdOnEncrypt()
        {
            _jwe.Encrypt("data", new RSACryptoServiceProvider(), null, "application/dontmatter");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNullContentTypeOnEncrypt()
        {
            _jwe.Encrypt("data", new RSACryptoServiceProvider(), "key id", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestInvalidDataToEncrypt()
        {
            _jwe.Encrypt(string.Empty, new RSACryptoServiceProvider(), "key id", "application/dontmatter");
        }

        [TestMethod]
        public void TestEncryptionRoundtrip()
        {
            var rsa = new RSACryptoServiceProvider();
            var encrypted = _jwe.Encrypt("secret", rsa, "my key id", "ct");

            var decrypted = _jwe.Decrypt(encrypted, rsa);

            Assert.AreEqual("secret", decrypted);
        }

        [TestMethod]
        public void TestEncryptionRoundtripWithDefaultKey()
        {
            var encrypted = _jwe.Encrypt("secretz", _rsaPublic, "key id", "ctype");
            var decrypted = _jwe.Decrypt(encrypted);
            Assert.AreEqual("secretz", decrypted);
        }

        [TestMethod]
        public void GetHeaders_ShouldReturnHeaders()
        {
            var encrypted = _jwe.Encrypt("secretz", _rsaPublic, "key id", "ctype");
            var headers = _jwe.GetHeaders(encrypted);

            Assert.IsTrue(headers["alg"] == "RSA-OAEP");
            Assert.IsTrue(headers["enc"] == "A256CBC-HS512");
            Assert.IsTrue(headers["kid"] == "key id");
            Assert.IsTrue(headers["cty"] == "ctype");
        }
    }
}
