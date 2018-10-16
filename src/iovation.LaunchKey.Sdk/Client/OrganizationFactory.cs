using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport;

namespace iovation.LaunchKey.Sdk.Client
{
	/// <summary>
	/// Creates API clients for a given Organization. This class allows the programmer to interact with an organization and its child services and directories via 'Clients', which this class creates
	/// </summary>
	public class OrganizationFactory
	{
		private ITransport _transport;
		private Guid _organizationId;

		public OrganizationFactory(ITransport transport, Guid organizationId)
		{
			_transport = transport;
			_organizationId = organizationId;
		}

		/// <summary>
		/// Create a service client for a child service within this organization or one of its child directories.
		/// </summary>
		/// <param name="serviceId">The ID of the service you wish to interact with</param>
		/// <returns>The service client</returns>
		public IServiceClient MakeServiceClient(string serviceId)
		{
			return new BasicServiceClient(Guid.Parse(serviceId), _transport);
		}

		/// <summary>
		/// Creates a directory client for interacting with a directory within this organization
		/// </summary>
		/// <param name="directoryId">The ID of the directory you wish to interact with</param>
		/// <returns>A directory client</returns>
		public IDirectoryClient MakeDirectoryClient(string directoryId)
		{
			return new BasicDirectoryClient(Guid.Parse(directoryId), _transport);
		}

		/// <summary>
		/// Creates an organization client for managing the organization, its services and directories
		/// </summary>
		/// <returns></returns>
		public IOrganizationClient MakeOrganizationClient()
		{
			return new BasicOrganizationClient(_organizationId, _transport);
		}
	}
}