using System;
using System.IO;

namespace iovation.LaunchKey.Sdk.Tests.Integration.SpecFlow
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

        public string GetP12ForFingerprint(string fingerprint)
        {
            if (GetAlphaCertificateFingerprint() == fingerprint) return GetBase64EncodedAlphaP12();
            if (GetBetaCertificateFingerprint() == fingerprint) return GetBase64EncodedBetaP12();
            if (fingerprint == null) return null;
            throw new Exception("Unrecognized fingerprint");
        }

        public string GetAlphaPublicKey()
        {
            return (ReadTextFile("alpha-public-key.pem"));
        }

        public string GetBetaPublicKey()
        {
            return (ReadTextFile("beta-public-key.pem"));
        }
    }
}
