using System;
using iovation.LaunchKey.Sdk.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Time
{
    [TestClass]
    public class UnixTimeConverterTests
    {
        private IUnixTimeConverter _converter = new UnixTimeConverter();

        [TestMethod]
        public void TestUtcTimeToUnix()
        {
            var time = DateTime.Parse("2017-12-13T18:17:53Z");

            var output = _converter.GetUnixTimestamp(time);

            Assert.AreEqual(1513189073L, output);
        }

        [TestMethod]
        public void TestLocalTimeToUnix()
        {
            var time = DateTime.Parse("2017-12-13T18:17:52Z").ToLocalTime();
            var output = _converter.GetUnixTimestamp(time);

            Assert.AreEqual(1513189072L, output);
        }

        [TestMethod]
        public void TestUtcTimestampToDateTime()
        {
            var time = DateTime.Parse("2017-12-13T18:17:53Z");
            var input = 1513189073L;
            var output = _converter.GetDateTime(input);

            Assert.AreEqual(time, output.ToLocalTime());
        }
    }
}
