using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.ServiceManager;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IServiceManagingClient
	{
		/// <summary>
		/// Create a new service
		/// </summary>
		/// <param name="name">The name of the service</param>
		/// <param name="description">A readable description of the service</param>
		/// <param name="icon">The URI of the icon to show for the service</param>
		/// <param name="callbackUrl">The URI for Webhook callbacks</param>
		/// <param name="active">Whether or not the service is active</param>
		/// <returns>the unique ID of the newly created service</returns>
		Guid CreateService(string name, string description, Uri icon, Uri callbackUrl, bool active);

		/// <summary>
		/// Update an existing service
		/// </summary>
		/// <param name="serviceId">The unique ID of the service to update</param>
		/// <param name="name">The name of the service</param>
		/// <param name="description">A readable description of the service</param>
		/// <param name="icon">The URI of the icon to show for the service</param>
		/// <param name="callbackUrl">The URI for Webhook callbacks</param>
		/// <param name="active">Whether or not the service is active</param>
		void UpdateService(Guid serviceId, string name, string description, Uri icon, Uri callbackUrl, bool active);

		/// <summary>
		/// Retrieve a service
		/// </summary>
		/// <param name="serviceId">The unique ID of the service to fetch</param>
		/// <returns>A service object containing details about the service</returns>
		Service GetService(Guid serviceId);

		/// <summary>
		/// Retrieve multiple services
		/// </summary>
		/// <param name="serviceIds">A list of service IDs to retrieve</param>
		/// <returns>A list of services</returns>
		List<Service> GetServices(List<Guid> serviceIds);

		/// <summary>
		/// Retrieve all services for this organization or directory
		/// </summary>
		/// <returns>A list of services</returns>
		List<Service> GetAllServices();
	}
}
