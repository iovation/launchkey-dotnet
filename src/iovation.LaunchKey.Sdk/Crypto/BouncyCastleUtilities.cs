using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
// ReSharper disable SuggestVarOrType_SimpleTypes

namespace iovation.LaunchKey.Sdk.Crypto
{
    /// <summary>
    /// These functions were pulled from the main BouncyCastle crypto library, and updated to
    /// work correctly cross-platform. Specifically, I have removed all usages of RSACryptoServiceProvider that
    /// rely on the CspParameters constructor, as this constructor involves accessing the Windows crypto API in
    /// a non-portable way. The functionality was not required for our code, so I removed it.
    ///
    /// I have tried to keep this as close to the original as possible. As such there may be stylistic inconsistencies
    /// between this file and other files in the project.
    ///
    /// GitHub issue referencing this problem within BouncyCastle: https://github.com/bcgit/bc-csharp/issues/160
    /// 
    /// </summary>
    internal static class BouncyCastleUtilities
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