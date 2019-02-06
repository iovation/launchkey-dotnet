using System;
using System.Collections.Generic;
using iovation.LaunchKey.Sdk.Error;

namespace iovation.LaunchKey.Sdk.Cache
{
    public class HashCache : ICache
    {
        private readonly Dictionary<string, string> _store = new Dictionary<string, string>();
        private readonly object _locker = new object();

        public string Get(string key)
        {
            lock (_locker)
            {
                try
                {
                    return _store[key];
                }
                catch (Exception ex)
                {
                    throw new CacheException("Cache error on get", ex);
                }
            }
        }

        public void Put(string key, string value)
        {
            lock (_locker)
            {
                try
                {
                    _store[key] = value;
                }
                catch (Exception ex)
                {
                    throw new CacheException("Cache error on set", ex);
                }
            }
        }
    }
}