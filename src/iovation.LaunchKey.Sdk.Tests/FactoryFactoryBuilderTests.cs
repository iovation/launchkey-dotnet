using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Transport.WebClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests
{
	[TestClass]
	public class FactoryFactoryBuilderTests
	{
		[TestMethod]
		public void Build_ShouldReturnFactoryFactory()
		{
			var ffb = new FactoryFactoryBuilder();
			var factoryFactory = ffb.Build();
			Assert.IsNotNull(factoryFactory);
		}

		[TestMethod]
		public void AddDirectoryPrivateKey_ShouldAddKey()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.AddDirectoryPrivateKey(TestConsts.DefaultDirectoryId.ToString("N"), TestConsts.DefaultPrivateKey);

			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void AddServicePrivateKey_ShouldAddKey()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.AddServicePrivateKey(TestConsts.DefaultServiceId.ToString("N"), TestConsts.DefaultPrivateKey);

			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void AddOrgPrivateKey_ShouldAddKey()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.AddOrganizationPrivateKey(TestConsts.DefaultOrgId.ToString("N"), TestConsts.DefaultPrivateKey);

			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetApiBaseUrl_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.SetApiBaseUrl("test");
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetApiIdentifier_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.SetApiIdentifier("test");
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetCache_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var cache = new Mock<ICache>().Object;
			var result = ffb.SetCache(cache);
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetCrypto_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var crypto = new Mock<ICrypto>().Object;
			var result = ffb.SetCrypto(crypto);
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetCurrentPublicKeyTtl_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.SetCurrentPublicKeyTttl(10);
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetHttpClient_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var http = new Mock<IHttpClient>().Object;
			var result = ffb.SetHttpClient(http);
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetOffsetTtl_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.SetOffsetTtl(10);
			Assert.AreSame(ffb, result);
		}

		[TestMethod]
		public void SetRequestExpireSeconds_ShouldReturnSelf()
		{
			var ffb = new FactoryFactoryBuilder();
			var result = ffb.SetRequestExpireSeconds(10);
			Assert.AreSame(ffb, result);
		}
	}
}
