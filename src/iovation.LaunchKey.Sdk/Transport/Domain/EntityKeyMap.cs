using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
	public class EntityKeyMap
	{
		private Dictionary<EntityIdentifier, Dictionary<string, RSA>> _store = new Dictionary<EntityIdentifier, Dictionary<string, RSA>>();
		private object _locker = new object();

		public void AddKey(EntityIdentifier entityIdentifier, string publicKeyFingerprint, RSA privateKey)
		{
			lock (_locker)
			{
				if (!_store.ContainsKey(entityIdentifier))
				{
					_store[entityIdentifier] = new Dictionary<string, RSA>();
				}
				_store[entityIdentifier][publicKeyFingerprint] = privateKey;
			}
		}

		public RSA GetKey(EntityIdentifier entityIdentifier, string publicKeyFingerprint)
		{
			lock (_locker)
			{
				if (!_store.ContainsKey(entityIdentifier))
					throw new NoKeyFoundException($"No keys found for entity {entityIdentifier}.");
				if (!_store[entityIdentifier].ContainsKey(publicKeyFingerprint))
					throw new NoKeyFoundException($"No key found for entity {entityIdentifier} with key {publicKeyFingerprint}");
				return _store[entityIdentifier][publicKeyFingerprint];
			}
		}
	}
}
