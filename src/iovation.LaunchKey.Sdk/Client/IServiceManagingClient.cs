using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain;
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

		/// <summary>
		/// Get a list of Public Keys for a Service
		/// </summary>
		/// <param name="serviceId">The service to get the public keys for</param>
		/// <returns>A list of public keys</returns>
		List<PublicKey> GetServicePublicKeys(Guid serviceId);

		/// <summary>
		/// Add a public key to a service
		/// </summary>
		/// <param name="serviceId">The service to add the key to</param>
		/// <param name="publicKeyPem">The public key (in PEM format) to add</param>
		/// <param name="active">Whether or not the key should be active</param>
		/// <param name="expires">The time at which the key should no longer be active</param>
		/// <returns></returns>
		string AddServicePublicKey(Guid serviceId, string publicKeyPem, bool active, DateTime? expires);

		/// <summary>
		/// Update a public key for a service
		/// </summary>
		/// <param name="serviceId">The service to update the key for</param>
		/// <param name="keyId">The public key to update</param>
		/// <param name="active">Whether or not the key should be active</param>
		/// <param name="expires">The time at which the key should no longer be active</param>
		void UpdateServicePublicKey(Guid serviceId, string keyId, bool active, DateTime? expires);

		/// <summary>
		/// Remove a Public Key from a Service. You may not remove the only Public Key from a Service.
		/// To deactivate a key, rather than remove, see <see cref="UpdateServicePublicKey(Guid, string, bool, DateTime?)"/>
		/// </summary>
		/// <param name="serviceId">The service to remove a public key from</param>
		/// <param name="keyId">The key to remove</param>
		void RemoveServicePublicKey(Guid serviceId, string keyId);

		/// <summary>
		/// Get the Default authorization policy for the Service
		/// </summary>
		/// <param name="serviceId">The service to retrieve the authorization policy for</param>
		/// <returns></returns>
		ServicePolicy GetServicePolicy(Guid serviceId);

		/// <summary>
		/// Update the default authorization policy for a Service
		/// </summary>
		/// <param name="serviceId">The service to update the authorization policy for</param>
		/// <param name="policy">The new authorization policy</param>
		void SetServicePolicy(Guid serviceId, ServicePolicy policy);

		/// <summary>
		/// Remove the default authorization policy for a Service
		/// </summary>
		/// <param name="serviceId">The service to remove the policy for</param>
		void RemoveServicePolicy(Guid serviceId);
	}
}