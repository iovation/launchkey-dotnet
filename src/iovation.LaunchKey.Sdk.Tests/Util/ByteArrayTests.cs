using System;
using iovation.LaunchKey.Sdk.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Util
{
	[TestClass]
	public class ByteArrayTests
	{
		[TestMethod]
		public void ByteArrayToHexString_ShouldProduceExpectedString()
		{
			byte[] src = {255, 200, 220};
			var str = ByteArrayUtils.ByteArrayToHexString(src);

			Assert.AreEqual(str, "ffc8dc");
		}

		[TestMethod]
		public void ByteArrayToHexString_ShouldContainSeparator()
		{
			byte[] src = {255, 200, 220};
			var str = ByteArrayUtils.ByteArrayToHexString(src, ":");

			Assert.AreEqual(str, "ff:c8:dc");
		}

		[TestMethod]
		public void HexStringToByteArray_ShouldProduceExpectedBytes()
		{
			var str = "fffe00";
			var bytes = ByteArrayUtils.HexStringToByteArray(str);

			Assert.IsTrue(ByteArrayUtils.AreEqual(bytes, new byte[] {0xff, 0xfe, 0x00}));
		}

		[TestMethod]
		public void AreEqual_ShouldHandleNulls()
		{
			Assert.IsTrue(ByteArrayUtils.AreEqual(null, null));
		}

		[TestMethod]
		public void AreEqual_ShouldHandleInequalLength()
		{
			Assert.IsFalse(ByteArrayUtils.AreEqual(new byte[] {0xff, 0xfe, 0xfd}, new byte[] {0xff, 0xfe, 0xfd, 0xfc}));
		}

		[TestMethod]
		public void AreEqual_ShouldReturnTrueForIdentical()
		{
			Assert.IsTrue(ByteArrayUtils.AreEqual(new byte[] {0xff, 0xfe, 0xfd}, new byte[] {0xff, 0xfe, 0xfd }));
		}
	}
}
