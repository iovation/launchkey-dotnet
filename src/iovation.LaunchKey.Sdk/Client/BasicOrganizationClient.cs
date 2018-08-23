using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Transport;
using iovation.LaunchKey.Sdk.Transport.Domain;

namespace iovation.LaunchKey.Sdk.Client
{
	public class BasicOrganizationClient : IOrganizationClient
	{
		private EntityIdentifier _organizationId;
		private ITransport _transport;

		public BasicOrganizationClient(Guid organizationId, ITransport transport)
		{
			_organizationId = new EntityIdentifier(EntityType.Organization, organizationId);
			_transport = transport;
		}

		public Guid CreateService(string name, string description, Uri icon, Uri callbackUrl, bool active)
		{
			var request = new ServicesPostRequest(name, icon, description, callbackUrl, active);
			var response = _transport.OrganizationV3ServicesPost(request, _organizationId);
			return response.Id;
		}
	}
}
