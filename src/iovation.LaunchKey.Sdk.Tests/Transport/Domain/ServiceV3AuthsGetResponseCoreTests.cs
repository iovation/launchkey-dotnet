using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Tests.Transport.Domain
{
	[TestClass]
	public class ServiceV3AuthsGetResponseCoreTests
	{
		[TestMethod]
		public void ShouldDeserialize()
		{
			var json = "{\"public_key_id\": \"f7:93:a2:9e:45:ac:61:40:ef:33:22:58:52:d1:51:db\", \"service_user_hash\": \"B1R3RD7WFKjlr0g4RACl25c2phPqcunajeQeZdTkLkM\", \"user_push_id\": \"54631005-5987-5b88-b59e-62bac03cc155\", \"org_user_hash\": \"x3gBfvijAPJfKKL6arFDandXLvTx7ItFVgCGzEzWyBt\", \"auth\": \"i0uyoZ1XTlV3Qar9p1pcsITibCRNgpt252hT5kl0WXX9RMYwV44L10H3fV+6SYHm\\r\\nMp5LXqkozce+Cyxl7prR3nm+2ft/7wBCrvr+X6MN46v8HyUPFs+RK9uuic7Rwklz\\r\\nKS6WaL7yK7/ybN0SrqIb2wofj+WYvIxjVg6+/wZQbQLFkCzY9EZXKzHOC0dD/PnF\\r\\nZLNRbGuf8HoLbp4OOGxCmnH6Oj6svHfgJg7NFrciffRp/uWiW8yh5Xit3wWkmmOA\\r\\njF6DBpQiHizzVxLoDCO08l6gG2awkrddQRIKfDeLz5c397oMck1zMXzmDc03370R\\r\\nL0XrEWQWmsu1m9OR93CcsQ==\"}";
			var o = JsonConvert.DeserializeObject<ServiceV3AuthsGetResponseCore>(json);

			Assert.AreEqual("i0uyoZ1XTlV3Qar9p1pcsITibCRNgpt252hT5kl0WXX9RMYwV44L10H3fV+6SYHm\r\nMp5LXqkozce+Cyxl7prR3nm+2ft/7wBCrvr+X6MN46v8HyUPFs+RK9uuic7Rwklz\r\nKS6WaL7yK7/ybN0SrqIb2wofj+WYvIxjVg6+/wZQbQLFkCzY9EZXKzHOC0dD/PnF\r\nZLNRbGuf8HoLbp4OOGxCmnH6Oj6svHfgJg7NFrciffRp/uWiW8yh5Xit3wWkmmOA\r\njF6DBpQiHizzVxLoDCO08l6gG2awkrddQRIKfDeLz5c397oMck1zMXzmDc03370R\r\nL0XrEWQWmsu1m9OR93CcsQ==", o.EncryptedDeviceResponse);
			Assert.AreEqual("x3gBfvijAPJfKKL6arFDandXLvTx7ItFVgCGzEzWyBt", o.OrgUserHash);
			Assert.AreEqual("f7:93:a2:9e:45:ac:61:40:ef:33:22:58:52:d1:51:db", o.PublicKeyId);
			Assert.AreEqual("B1R3RD7WFKjlr0g4RACl25c2phPqcunajeQeZdTkLkM", o.ServiceUserHash);
			Assert.AreEqual("54631005-5987-5b88-b59e-62bac03cc155", o.UserPushId);
		}
	}
}
