using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using iovation.LaunchKey.Sdk.Json;
using iovation.LaunchKey.Sdk.Transport.Domain;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Domain;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
    [TestClass]
    public class KeysListPostResponseTests
    {
        [TestMethod]
        public void FromTransportShouldReturnPublicKeyList()
        {
            KeysListPostResponse response = new KeysListPostResponse(
                new List<KeysListPostResponse.Key>{
                    new KeysListPostResponse.Key
                    {
                        Active = true,
                        Created = new DateTime(2020, 1, 1),
                        Expires = new DateTime(2021, 1, 1),
                        Id = "other-key",
                        PublicKey = "k",
                        KeyType = -1
                    },
                    new KeysListPostResponse.Key {
                        Active = false,
                        Created = new DateTime(2020, 2, 2),
                        Expires = new DateTime(2021, 2, 2),
                        Id = "both-key",
                        PublicKey = "k",
                        KeyType = 0
                    },
                    new KeysListPostResponse.Key {
                        Active = true,
                        Created = new DateTime(2020, 3, 3),
                        Expires = new DateTime(2021, 3, 3),
                        Id = "encryption-key",
                        PublicKey = "k",
                        KeyType = 1
                    },
                    new KeysListPostResponse.Key {
                        Active = false,
                        Created = new DateTime(2020, 4, 4),
                        Expires = new DateTime(2021, 4, 4),
                        Id = "signature-key",
                        PublicKey = "k",
                        KeyType = 2
                    }
                }
            );

            List<PublicKey> keys = response.FromTransport();
            List<PublicKey> expected = new List<PublicKey>(){
                new PublicKey("other-key", true, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), KeyType.OTHER),
                new PublicKey("both-key", false, new DateTime(2020, 2, 2), new DateTime(2021, 2, 2), KeyType.BOTH),
                new PublicKey("encryption-key", true, new DateTime(2020, 3, 3), new DateTime(2021, 3, 3), KeyType.ENCRYPTION),
                new PublicKey("signature-key", false, new DateTime(2020, 4, 4), new DateTime(2021, 4, 4), KeyType.SIGNATURE),
            };
            CollectionAssert.AreEqual(
                expected,
                keys
            );
        }
    }
}
