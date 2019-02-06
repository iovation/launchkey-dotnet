using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Crypto
{
    [TestClass]
    public class CryptoTests
    {
        private ICrypto _crypto;
        private RSA _privateKey;
        private RSA _publicKey;

        [TestInitialize]
        public void Setup()
        {
            _crypto = new BouncyCastleCrypto();
            _publicKey = _crypto.LoadRsaPublicKey(File.ReadAllText("test-public.key"));
            _privateKey = _crypto.LoadRsaPrivateKey(File.ReadAllText("test-private.key"));
        }

        [TestMethod]
        public void TestLoadPrivateKey()
        {
            var pkey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAkWY1n5mCrzLjys1zijQwhTeZwXMajWg7kE4KD6m7KBCtkqoO
nL6rnlRELsWpKsKujFjWIqE4V2rJEV+ctZK/xThFgEURYQ8Tl5QLJN8anp+9eot8
E9s5A8zjfBf3lq+BmzDJDOaCg+ZgmoPbx6t0t6Y76kSMuP1UoRp4B/OFB1WETDQM
rzc8vwgJb+LC9fDHYIMk3aKbYVI1FHYPC0QGw1EgfmpqYnWkhDb8y9YZMMaVAYQF
YCrELy77DBEhkGFoyIlbHJA8ZMSJgMNeY3Soq2dQRcwseKCSy0h7ugB/10+Hst2N
oDLMjXw3IevbUisQoeE32kJm0jTg2CgjfNXE4wIDAQABAoIBAHrilH2QA3hJHWbA
n1713vAoXsW1n4JVzsmWe0Bjpi8lcV1cZr8pEujctUeGfQQLx/QL/OcFtRWExezu
DwcSwQtRFMRlG1wZnuhkEcsW2Gup/D0++B3cEOLaXoT5yKJNMM/VuoYxp6sMoSbW
PIETsrCoSUkkqH6MdOM3+KxXr13TLb5uwdW+7XCuJZhqtpW1BfXyLsODfTJ9dTeK
HCbsaAc8aYb3T++I2VZ1CnjY65u5Ve7EEmKhmdAwPAkv3nAV2M/WejnhvM5WxEIF
e4XLrl/BGGHrO23iJfsh4U0JiqiRKK+pZ5/OS0IjClHOcAqXPpXlAOAl60EB8sDi
prLht2kCgYEAv8FR0IGl8+wXt+y8enj+wRw77keX60amZI0BP/6T2DG4Iifbeutz
FEjZjr9lQRucbguYQZQvNCUD3bcJhLycKjnWlLx1rFYiKr+InVGjdxDgTXytmfXv
5cwjtuPutjLmtSB6G7eyjT541nAY5eLgUsQmG6GJGjuXFbk68E/hul8CgYEAwhz7
oFX++WBG6rcJppd8ZSh+22qxM2ujb74rFkSA+ElkmLvuJWcUWCKP9NKLrSzNFC56
hPf6ahTV32W3D1yV2jH8uD13vaRl81Ibf6joyZ1NjlLzF8f80kxhTv9sBYXCFoE0
N65bv4pBOH1j5W8JU+VENG4e/yPmphwYmw3Ri/0CgYEAqzlcbOrX3O88XqetxU61
bIphmWukZBo6Ch8+tn4EVlBPM3CTszb7E2nZmnWdXH3jOQBxfF1tqQpEYX5HqZch
ezMaQMXn7XMcHz/YJWH4rSEMqSRjf87z4CZg3ba6OUdKawINByMI9MaL8C84jE4C
MOWlgZbEbgA38NXtoxgxX9ECgYBoAnehlfKlVL0rDyCQryLsYSJ4F1cTol1UhqU3
rUUdxNWuMSSNzgnMn9ha6mYFSLgqVC06ClWXecqPhUTtakKxQ4+SeP0sFGa8VgZ+
BNeXED56QGAZIgOJ+3s4hQwweVdiD6EXJMnse/wudKGOUkzBM1u0bD0XoPj5kJxu
mJzrUQKBgQCkUZ3gzA90Pf3dogYn0mGfTj4hY/+qn3tGtc0CFrzGHdIEFnJi5WlO
r+ppoKXL4tSFNDsShVSQBgEAZU9F/F7wUgCXqBMnHu574fpKakiNpeTmLnQ4JNn4
PmRoieUCtxxvmnckMGk4ub+/X4AJHb0ErqavEbIrrBNLW4ahtrJC5g==
-----END RSA PRIVATE KEY-----";
            var key = _crypto.LoadRsaPrivateKey(pkey);
            var csp = key as RSACryptoServiceProvider;

            Assert.IsTrue(csp != null);
            Assert.AreEqual(2048, csp.KeySize);
            Assert.AreEqual(false, csp.PublicOnly);
        }

        [TestMethod]
        public void TestLoadPublicKey()
        {
            var pemkey = @"-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAinATCdbqz0oDfcUtjzrx
vF9JNJOrZzBNCmTUpOz/VptDWpraj040eoywD3VRklmMVFt0e77Hs34BsrhchCav
mzlQmYYjL4zIzRX4B0l+U/PhC6p6RIL8D/TSk11u11sHtBycSOThYDeoPRuBo/Zq
g3rVvsYdjQ56RLEgI9JkXM5xJWEPgRE2NcCMCBjEQu3icWKUsu5boo4vT33ZhOMU
CDrajXshXvCxrp6JSb3jvoWC/lIpcDomtDnj/u9GXivsGv3Vk8YjmFlTEnr5Kb/o
3uSlCFO9bLfEGEhlBULyOeN7m2NKFvFXqfbd4hdtVbEQWBc+te9hLfAF6n13wURk
qF23lpEZCLcvql4mq/38u+MlgHshaOfYuGN5lPLZn4pRLUPPGS+Q1dYEVirLzWJx
1Ztn7Ti8qe3ePbXHF2W/+9T+udhROQNv3pJsGp7dxG3WxZB2l16v2cir0nv+jZti
JaXPf+seoEup2RckvCWhalpnUeXSJE339CkFAN1uTkvXgMWr5XRNuxBsRhz8pnLT
TxrmsAS6Onkyjhl/+ihxJasCTpN69jmwqxSFNmStzXFz6LjqUtiPIeMdiCn9dFrD
Gb2x+XCOpvFR9q+9RPP/bZxnJPmSPbQEcrjwhLerDL9qbwgHnGYXdlM9JaYYkG5y
2ZzlVAZOwr81Y9KxOGFq+w8CAwEAAQ==
-----END PUBLIC KEY-----";
            var loader = new BouncyCastleCrypto();
            var key = loader.LoadRsaPublicKey(pemkey);
            var csp = key as RSACryptoServiceProvider;

            Assert.IsTrue(csp != null);
            Assert.AreEqual(4096, csp.KeySize);
            Assert.AreEqual(true, csp.PublicOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographyError))]
        public void LoadRsaPrivateKey_ShouldThrowOnBadData()
        {
            var pemKey = @"total gibbberish, i mean this doesn't look like a key AT ALL";
            _crypto.LoadRsaPrivateKey(pemKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadRsaPrivateKey_ShouldThrowOnNull()
        {
            _crypto.LoadRsaPrivateKey(null);
        }

        [TestMethod]
        [ExpectedException(typeof(CryptographyError))]
        public void LoadRsaPublicKey_ShouldThrowOnBadData()
        {
            var pemKey = @"total gibbberish, i mean this doesn't look like a key AT ALL";
            _crypto.LoadRsaPublicKey(pemKey);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadRsaPublicKey_ShouldThrowOnNull()
        {
            _crypto.LoadRsaPublicKey(null);
        }

        [TestMethod]
        public void Sha256_ShouldProduceValidHash()
        {
            var sourceData = "Hello crazy world";
            var sourceDataBinary = Encoding.UTF8.GetBytes(sourceData);

            var resultHash = _crypto.Sha256(sourceDataBinary);
            var resultHashString = ByteArrayUtils.ByteArrayToHexString(resultHash);

            Assert.AreEqual(resultHashString, "2907d7ba4e806c96a8ab89b03be1745b5beb5568a109af9287acd71707e79f2d");
        }

        [TestMethod]
        public void Sha384_ShouldProduceValidHash()
        {
            var sourceData = "Hello crazy world";
            var sourceDataBinary = Encoding.UTF8.GetBytes(sourceData);

            var resultHash = _crypto.Sha384(sourceDataBinary);
            var resultHashString = ByteArrayUtils.ByteArrayToHexString(resultHash);

            Assert.AreEqual(resultHashString, "00d025cd5118e8b21b7b01142e59d37cbeac7db6e75a3ae41795f07bbb4f8dfd0bce48f81a0b0b3df50c8b6e05c37eea");
        }

        [TestMethod]
        public void Sha512_ShouldProduceValidHash()
        {
            var sourceData = "Hello crazy world";
            var sourceDataBinary = Encoding.UTF8.GetBytes(sourceData);

            var resultHash = _crypto.Sha512(sourceDataBinary);
            var resultHashString = ByteArrayUtils.ByteArrayToHexString(resultHash);

            Assert.AreEqual(resultHashString, "9f9ddccd74997c8fe237d5fb20b92b57503f17a7d26c5a7dac50fe51c4f78f85a83dc9db9f0ddc02e46434ff2c645feae646102e01dbb65833189a8ba1e3e5df");
        }

        [TestMethod]
        public void RSAFunctions_ShouldEncryptAndDecryptBackToSame()
        {
            var sourceData = "Hello crazy world";
            var sourceDataBinary = Encoding.UTF8.GetBytes(sourceData);
            var encryptedData = _crypto.EncryptRSA(sourceDataBinary, _publicKey);
            var decryptedData = _crypto.DecryptRSA(encryptedData, _privateKey);
            var decryptedDataString = Encoding.UTF8.GetString(decryptedData);

            Assert.AreEqual(sourceData, decryptedDataString);
        }
    }
}
