using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Crypto.Jwt
{
    [TestClass]
    public class JwtServiceTests
    {
        private BouncyCastleCrypto _crypto;
        private RSA _privateKey;
        private RSA _privateKeyPublicKey;
        private RSA _publicKey;
        private RSA _publicKeyOther;
        private Dictionary<string, RSA> _keyList;
        private JwtService _jwtService;

        private static readonly string PUBLIC_KEY_PEM = "-----BEGIN PUBLIC KEY-----\r\nMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAinATCdbqz0oDfcUtjzrx\r\nvF9JNJOrZzBNCmTUpOz/VptDWpraj040eoywD3VRklmMVFt0e77Hs34BsrhchCav\r\nmzlQmYYjL4zIzRX4B0l+U/PhC6p6RIL8D/TSk11u11sHtBycSOThYDeoPRuBo/Zq\r\ng3rVvsYdjQ56RLEgI9JkXM5xJWEPgRE2NcCMCBjEQu3icWKUsu5boo4vT33ZhOMU\r\nCDrajXshXvCxrp6JSb3jvoWC/lIpcDomtDnj/u9GXivsGv3Vk8YjmFlTEnr5Kb/o\r\n3uSlCFO9bLfEGEhlBULyOeN7m2NKFvFXqfbd4hdtVbEQWBc+te9hLfAF6n13wURk\r\nqF23lpEZCLcvql4mq/38u+MlgHshaOfYuGN5lPLZn4pRLUPPGS+Q1dYEVirLzWJx\r\n1Ztn7Ti8qe3ePbXHF2W/+9T+udhROQNv3pJsGp7dxG3WxZB2l16v2cir0nv+jZti\r\nJaXPf+seoEup2RckvCWhalpnUeXSJE339CkFAN1uTkvXgMWr5XRNuxBsRhz8pnLT\r\nTxrmsAS6Onkyjhl/+ihxJasCTpN69jmwqxSFNmStzXFz6LjqUtiPIeMdiCn9dFrD\r\nGb2x+XCOpvFR9q+9RPP/bZxnJPmSPbQEcrjwhLerDL9qbwgHnGYXdlM9JaYYkG5y\r\n2ZzlVAZOwr81Y9KxOGFq+w8CAwEAAQ==\r\n-----END PUBLIC KEY-----";
        //private static readonly string PUBLIC_KEY_FINGERPRINT = "d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de";

        private static readonly string PUBLIC_KEY_OTHER = "-----BEGIN PUBLIC KEY-----\n" +
                                                        "\n" +
                                                        "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA8zQos4iDSjmUVrFUAg5G\n" +
                                                        "uhU6GehNKb8MCXFadRWiyLGjtbGZAk8fusQU0Uj9E3o0mne0SYESACkhyK+3M1Er\n" +
                                                        "bHlwYJHN0PZHtpaPWqsRmNzui8PvPmhm9QduF4KBFsWu1sBw0ibBYsLrua67F/wK\n" +
                                                        "PaagZRnUgrbRUhQuYt+53kQNH9nLkwG2aMVPxhxcLJYPzQCat6VjhHOX0bgiNt1i\n" +
                                                        "HRHU2phxBcquOW2HpGSWcpzlYgFEhPPQFAxoDUBYZI3lfRj49gBhGQi32qQ1YiWp\n" +
                                                        "aFxOB8GA0Ny5SfI67u6w9Nz9Z9cBhcZBfJKdq5uRWjZWslHjBN3emTAKBpAUPNET\n" +
                                                        "nwIDAQAB\n" +
                                                        "-----END PUBLIC KEY-----\n";


        [TestInitialize]
        public void Initialize()
        {
            _crypto = new BouncyCastleCrypto();
            _privateKey = _crypto.LoadRsaPrivateKey(File.ReadAllText("test-private.key"));
            _privateKeyPublicKey = _crypto.LoadRsaPublicKey(File.ReadAllText("test-public.key"));
            _publicKey = _crypto.LoadRsaPublicKey(PUBLIC_KEY_PEM);
            _publicKeyOther = _crypto.LoadRsaPublicKey(PUBLIC_KEY_OTHER);
            _keyList = new Dictionary<string, RSA>();
            _keyList.Add("main", _privateKey);
            _jwtService = new JwtService(new UnixTimeConverter(), "lka", _keyList, "main", 5);
        }

        [TestMethod]
        public void GetJWTData_HeadersAreCorrect()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            var jwtData = _jwtService.GetJWTData(jwt);

            Assert.AreEqual(jwtData.Audience, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a");
            Assert.AreEqual(jwtData.Issuer, "lka");
            Assert.AreEqual(jwtData.KeyId, "d2:8e:16:91:39:5b:9d:24:73:0e:36:0a:9a:ef:7e:de");
            Assert.AreEqual(jwtData.Subject, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a");
        }

        [TestMethod]
        [ExpectedException(typeof(JwtError))]
        public void Decode_ExpiredTokensShouldBeRejected()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            _jwtService.Decode(_publicKey, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a", "22dae4253165495a8ed56c34c4d050ce", DateTime.Parse("1/1/2001"), jwt);
        }

        [TestMethod]
        [ExpectedException(typeof(JwtError))]
        public void Decode_SignatureShouldBeRejectedIfKeyIsWrong()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            var jwtIssue = DateTime.Parse(
                "1/4/2018 7:41:41 AM",
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );
            _jwtService.Decode(_publicKeyOther, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a", "22dae4253165495a8ed56c34c4d050ce", jwtIssue, jwt);
        }

        [TestMethod]
        public void Decode_ValidTokenShouldDecodeOK()
        {
            // a token we got from the API
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";

            // pretend our date is right
            var jwtIssue = DateTime.Parse(
                "1/4/2018 7:41:41 AM",
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );

            // decode it, expect no exception
            _jwtService.Decode(_publicKey, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a", "22dae4253165495a8ed56c34c4d050ce", jwtIssue, jwt);
        }

        [TestMethod]
        [ExpectedException(typeof(JwtError))]
        public void Decode_WrongExpectedTokenThrowsException()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            var jwtIssue = DateTime.Parse(
                "1/4/2018 7:41:41 AM",
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );
            _jwtService.Decode(_publicKey, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a", "wrong token id", jwtIssue, jwt);
        }

        [TestMethod]
        public void Decode_NullTokenIdShouldAvoidCheck()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            var jwtIssue = DateTime.Parse(
                "1/4/2018 7:41:41 AM",
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );
            _jwtService.Decode(_publicKey, "svc:f8329e0e-dd7a-11e7-98b4-469158467b1a", null, jwtIssue, jwt);
        }

        [TestMethod]
        [ExpectedException(typeof(JwtError))]
        public void Decode_WrongAudienceThrowsException()
        {
            var jwt = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImQyOjhlOjE2OjkxOjM5OjViOjlkOjI0OjczOjBlOjM2OjBhOjlhOmVmOjdlOmRlIn0.eyJhdWQiOiJzdmM6ZjgzMjllMGUtZGQ3YS0xMWU3LTk4YjQtNDY5MTU4NDY3YjFhIiwiaXNzIjoibGthIiwiY3R5IjoiYXBwbGljYXRpb24vanNvbiIsIm5iZiI6MTUxNTA1MTcwMSwianRpIjoiMjJkYWU0MjUzMTY1NDk1YThlZDU2YzM0YzRkMDUwY2UiLCJleHAiOjE1MTUwNTE3MDYsImlhdCI6MTUxNTA1MTcwMSwicmVzcG9uc2UiOnsic3RhdHVzIjoyMDAsImhhc2giOiI0YzViODBlZDhlNTRhZWIwNDkwODFiYTBjMWE0ZDViMjkwNzhkMjIzM2NhZTkxZDg5ODUyMjUwYTFkMWQ1OWEyIiwiZnVuYyI6IlMyNTYifSwic3ViIjoic3ZjOmY4MzI5ZTBlLWRkN2EtMTFlNy05OGI0LTQ2OTE1ODQ2N2IxYSJ9.NFhztQSep7f0QT8j7KIfK4yFwdHvL24r8qdYKZz3M3Y2kyQYT0ECBPXnXgr2KbU5gntWdjVUOySUQbDnF2aL8QDyWPQHGy6jX7n4rO2VdeYuROz-3dbx7taSJEUUlDSvoDAerL26P02xU6LbZno1TCl4pYMQn4k41xwBvYdf8vvN1ixLhCJE_S2V18bT4YKTqwxE-XnbubdAdvhkPdl-7Khja4r1MpktRTZAMWJECLXI3JJILm-3HudO9KS5uAAHkxGbW1ovZfxOcKv9fcwoZ4mSn1pXA7k1aAt7DJdk4EuOtLmxb5rIKTDvQcBZIPyO_N3repWeD8ghCIFgWOSHqUE7cM1koUvUZzDiTvPA9o2C46zD26QaljSWfsxYPKtyzZsyglPYc1bcmzJZWzfHFCOoKA8uxe7HLiqNyzry3Cyg0_kPiwwtJ_FBRAnN3MVhAczaXbEPHtrm3vLafw14_M2svtL6f6RsObFroBJE_VDmgdiKtiJiYQ1UEpx_LcwK_OB-BagOnXUFGtveKQIJvABZoRwrxS6TebhVMAU1cGurF2sJmlSO3oi1CC-GSgtVDMmh_a2oA9E5UrG3W5nmB-ThCGBpKfTIYB19rwsOd6RYNUaOEj65_KN_EFHH_7dLAeDDlPWqFCUfML9pjeRUK289c_ByDDOX3N3iTrnCt1o";
            var jwtIssue = DateTime.Parse(
                "1/4/2018 7:41:41 AM",
                CultureInfo.CurrentCulture.DateTimeFormat,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
            );
            _jwtService.Decode(_publicKey, "wrong audience", "22dae4253165495a8ed56c34c4d050ce", jwtIssue, jwt);
        }

        [TestMethod]
        public void Encode_ShouldBeDecodable()
        {
            var now = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var encoded = _jwtService.Encode("token id", "issuer", "subject", now, "GET", "/out/of/here", null, null);
            var decoded = Jose.JWT.Decode(encoded, _privateKeyPublicKey);
            Assert.AreEqual("{\"jti\":\"token id\",\"iss\":\"issuer\",\"sub\":\"subject\",\"aud\":\"lka\",\"iat\":978307200,\"nbf\":978307200,\"exp\":978307205,\"request\":{\"meth\":\"GET\",\"path\":\"/out/of/here\"}}", decoded);
        }
    }
}
