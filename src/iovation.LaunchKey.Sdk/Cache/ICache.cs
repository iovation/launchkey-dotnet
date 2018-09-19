using System;
using System.Text;

namespace iovation.LaunchKey.Sdk.Cache
{
	/// <summary>
	/// A simple key-value caching interface
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// retrieve a value given a key
		/// </summary>
		/// <param name="key">the key to retrieve</param>
		/// <returns>the value</returns>
		string Get(string key);

		/// <summary>
		/// store a value given a key. Overwrite if needed.
		/// </summary>
		/// <param name="key">the key to store</param>
		/// <param name="value">the value to store</param>
		void Put(string key, string value);
	}
}