using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport;

namespace iovation.LaunchKey.Sdk.Client
{
	/// <summary>
	/// Creates API clients for a given Directory. This class allows the programmer to interact with a directory and its child services via 'Clients', which this class creates
	/// </summary>
	public class DirectoryFactory
	{
		private ITransport _transport;
		private Guid _directoryId;
		
		/// <summary>
		/// Creates a new instance
		/// </summary>
		/// <param name="transport">The preconfigured ITransport interface to use when communicating to the API.</param>
		/// <param name="directoryId">The directory ID</param>
		public DirectoryFactory(ITransport transport, Guid directoryId)
		{
			_transport = transport;
			_directoryId = directoryId;
		}

		/// <summary>
		/// Create a service client for a child service within this directory.
		/// </summary>
		/// <param name="serviceId">The ID of the service you wish to interact with</param>
		/// <returns>The service client</returns>
		public IServiceClient MakeServiceClient(string serviceId)
		{
			return new BasicServiceClient(Guid.Parse(serviceId), _transport);
		}

		/// <summary>
		/// Creates a directory client for interacting with this directory.
		/// </summary>
		/// <returns>A directory client</returns>
		public IDirectoryClient MakeDirectoryClient()
		{
			return new BasicDirectoryClient(_directoryId, _transport);
		}
	}
}
