using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Crypto.Jwt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Crypto.Jwt
{
	[TestClass]
	public class JwtDataTests
	{
		[TestMethod]
		public void TestMutability()
		{
			var data = new JwtData("iss", "sub", "aud", "kid");

			Assert.AreEqual(data.Issuer, "iss");
			Assert.AreEqual(data.Subject, "sub");
			Assert.AreEqual(data.Audience, "aud");
			Assert.AreEqual(data.KeyId, "kid");
		}
	}
}
