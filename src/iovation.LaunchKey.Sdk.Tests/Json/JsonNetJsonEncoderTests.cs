using iovation.LaunchKey.Sdk.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iovation.LaunchKey.Sdk.Tests.Json
{
    [TestClass]
    public class JsonNetJsonEncoderTests
    {
        [TestMethod]
        public void Encode_ShouldReturnNullForNull()
        {
            var encoder = new JsonNetJsonEncoder();

            Assert.IsNull(encoder.EncodeObject(null));
        }

        [TestMethod]
        public void Encode_BasicEncoding()
        {
            var o = new
            {
                name = "hi",
                age = 87,
                happy = 1.01,
                child = new
                {
                    name = "kid",
                    age = 12,
                    happy = 1.02
                }
            };

            var encoder = new JsonNetJsonEncoder();
            var result = encoder.EncodeObject(o);
            Assert.AreEqual("{\"name\":\"hi\",\"age\":87,\"happy\":1.01,\"child\":{\"name\":\"kid\",\"age\":12,\"happy\":1.02}}", result);
        }

        class TestPoco
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        [TestMethod]
        public void ShouldDecodeSameObjectAsEncoded()
        {
            var person = new TestPoco();
            person.Age = 5;
            person.Name = "Bill";

            var encoder = new JsonNetJsonEncoder();

            var encoded = encoder.EncodeObject(person);
            var decodedObject = encoder.DecodeObject<TestPoco>(encoded);

            Assert.AreEqual(person.Age, decodedObject.Age);
            Assert.AreEqual(person.Name, decodedObject.Name);
        }
    }
}
