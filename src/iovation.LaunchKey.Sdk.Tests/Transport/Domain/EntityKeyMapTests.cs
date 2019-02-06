using System;
using System.Security.Cryptography;
using iovation.LaunchKey.Sdk.Error;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class EntityKeyMapTests
    {
        [TestMethod]
        public void AddKey_ShouldOverwrite()
        {
            var km = new EntityKeyMap();
            var rsa = new RSACryptoServiceProvider();
            var identifier = new EntityIdentifier(EntityType.Directory, Guid.NewGuid());

            km.AddKey(identifier, "key", rsa);
            var rsa2 = km.GetKey(identifier, "key");

            Assert.AreSame(rsa, rsa2);

            var rsa3 = new RSACryptoServiceProvider();
            km.AddKey(identifier, "key", rsa3);

            var rsa4 = km.GetKey(identifier, "key");

            Assert.AreSame(rsa3, rsa4);
        }

        [TestMethod]
        [ExpectedException(typeof(NoKeyFoundException))]
        public void GetKey_ShouldThrowNoKeyFoundException()
        {
            var km = new EntityKeyMap();

            km.GetKey(new EntityIdentifier(EntityType.Organization, Guid.NewGuid()), "key");
        }
    }
}
