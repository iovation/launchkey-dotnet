using System;
using iovation.LaunchKey.Sdk.Domain.Directory;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class DirectoryUserTotpTests
    {
        private string _testSecret = "ABCDEFG";
        private string _testAlgorithm = "SHA1";
        private int _testPeriod = 60;
        private int _testDigits = 6;

        [TestMethod]
        public void TestAttributesAreSet()
        {
            var totp = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            Assert.AreEqual(_testSecret, totp.Secret);
            Assert.AreEqual(_testAlgorithm, totp.Algorithm);
            Assert.AreEqual(_testPeriod, totp.Period);
            Assert.AreEqual(_testDigits, totp.Digits);
            
        }

        [TestMethod]
        public void TestEquals()
        {
            var totp = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            var totp2 = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            Assert.AreEqual(totp, totp2);
        }
        
        [TestMethod]
        public void TestEqualsSelf()
        {
            var totp = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            Assert.AreEqual(totp, totp);
        }
        
        [TestMethod]
        public void TestNotEqualsOther()
        {
            var totp = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            var totp2 = new DirectoryUserTotp("OtherSecret", _testAlgorithm, _testPeriod, _testDigits);
            Assert.AreNotEqual(totp, totp2);
        }

        [TestMethod]
        public void TestToString()
        {
            var totp = new DirectoryUserTotp(_testSecret, _testAlgorithm, _testPeriod, _testDigits);
            Assert.AreEqual("iovation.LaunchKey.Sdk.Domain.Directory.DirectoryUserTotp" + "{" +
                            "secret = '" + _testSecret + "'" +
                            "algorithm = '" + _testAlgorithm + "'" +
                            "period = '" + _testPeriod + "'" +
                            "digits = '" + _testDigits + "'" +
                            "}", totp.ToString());
        }
    }
}
