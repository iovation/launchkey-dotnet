using System;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class ServicesPostRequestTests
	{
		[TestMethod]
		public void Constructor_ShouldSetProperties()
		{
			var req = new ServicesPostRequest("n", "d", new Uri("http://a.com"), new Uri("http://b.com"), true);
			Assert.AreEqual("n", req.Name);
			Assert.AreEqual("d", req.Description);
			Assert.AreEqual(new Uri("http://a.com"), req.Icon);
			Assert.AreEqual(new Uri("http://b.com"), req.CallbackUrl);
			Assert.AreEqual(true, req.Active);
		}
	}
}