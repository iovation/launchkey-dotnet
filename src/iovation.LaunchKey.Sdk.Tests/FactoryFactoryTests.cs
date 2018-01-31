using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Cache;
using iovation.LaunchKey.Sdk.Client;
using iovation.LaunchKey.Sdk.Crypto;
using iovation.LaunchKey.Sdk.Transport.WebClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace iovation.LaunchKey.Sdk.Tests
{
	[TestClass]
	public class FactoryFactoryTests
	{
		FactoryFactory MakeFactoryFactory()
		{
			var crypto = new Mock<ICrypto>();
			crypto.Setup(p => p.LoadRsaPublicKey(It.IsAny<string>()))
				.Returns(new RSACryptoServiceProvider());
			crypto.Setup(p => p.GeneratePublicKeyFingerprintFromPrivateKey(It.IsAny<RSA>()))
				.Returns("ff:ff");

			var httpClient = new Mock<IHttpClient>();
			var cache = new HashCache();

			var factoryFactory = new FactoryFactory(
				crypto.Object,
				httpClient.Object,
				cache,
				"https://api.launchkey.com",
				"lka",
				5,
				3600,
				500,
				new Sdk.Transport.Domain.EntityKeyMap()
			);

			return factoryFactory;
		}

		[TestMethod]
		public void MakeServiceFactory_ShouldReturnServiceFactory()
		{
			var factoryFactory = MakeFactoryFactory();
			var serviceClient = factoryFactory.MakeServiceFactory(TestConsts.DefaultServiceId.ToString("D"), "key");

			Assert.IsTrue(serviceClient != null);
		}

		[TestMethod]
		public void MakeDirectoryFactory_ShouldReturnDirectoryFactory()
		{
			var factoryFactory = MakeFactoryFactory();
			var directoryFactory = factoryFactory.MakeDirectoryFactory(TestConsts.DefaultDirectoryId.ToString("D"), "key");

			Assert.IsTrue(directoryFactory != null);
		}

		[TestMethod]
		public void MakeOrganizationFactory_ShouldReturnOrganizationFactory()
		{
			var factoryFactory = MakeFactoryFactory();
			var organizationFactory = factoryFactory.MakeOrganizationFactory(TestConsts.DefaultOrgId.ToString("D"), "key");

			Assert.IsTrue(organizationFactory != null);			
		}
	}
}
