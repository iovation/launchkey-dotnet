using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;
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
			var request = new ServicesPostRequest(name, description, icon, callbackUrl, active);
			var response = _transport.OrganizationV3ServicesPost(request, _organizationId);
			return response.Id;
		}

		public void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active)
		{
			var request = new ServicesPatchRequest(serviceId, name, description, icon, callbackUrl, active);
			_transport.OrganizationV3ServicesPatch(request, _organizationId);
		}

		public Service GetService(Guid serviceId)
		{
			return GetServices(new List<Guid> {serviceId}).First();
		}

		public List<Service> GetServices(List<Guid> serviceIds)
		{
			var request = new ServicesListPostRequest(serviceIds);
			var response = _transport.OrganizationV3ServicesListPost(request, _organizationId);
			var services = new List<Service>();

			foreach (var serviceItem in response.Services)
			{
				services.Add(
					new Service(
						serviceItem.Id,
						serviceItem.Name,
						serviceItem.Description,
						serviceItem.Icon,
						serviceItem.CallbackUrl,
						serviceItem.Active
				));
			}

			return services;
		}

		public List<Service> GetAllServices()
		{
			var response = _transport.OrganizationV3ServicesGet(_organizationId);
			var services = new List<Service>();
			foreach (var serviceItem in response.Services)
			{
				services.Add(
					new Service(
						serviceItem.Id,
						serviceItem.Name,
						serviceItem.Description,
						serviceItem.Icon,
						serviceItem.CallbackUrl,
						serviceItem.Active
					)
				);
			}
			return services;
		}
	}
}
