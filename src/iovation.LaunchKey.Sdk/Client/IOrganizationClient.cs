using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Domain.Organization;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IOrganizationClient : IServiceManagingClient
	{
		/// <summary>
		/// Creates a directory within the organization
		/// </summary>
		/// <param name="name">The name of the directory</param>
		/// <returns>The unique ID of the directory created</returns>
		Guid CreateDirectory(string name);

		/// <summary>
		/// Updates an existing directory within the organization
		/// </summary>
		/// <param name="directoryId">The unique ID of the directory to update</param>
		/// <param name="active">Whether or not the directory should be active</param>
		/// <param name="androidKey">GCM push key</param>
		/// <param name="iosP12">APNS push certificate in .P12 format that has been Base64 Encoded</param>
		void UpdateDirectory(Guid directoryId, bool active, string androidKey, string iosP12);

		/// <summary>
		/// Get a directory from this organization
		/// </summary>
		/// <param name="directoryId">The unique ID of the directory to fetch</param>
		/// <returns>The directory object</returns>
		Directory GetDirectory(Guid directoryId);

		/// <summary>
		/// Get multiple directories from this organization in a single API call
		/// </summary>
		/// <param name="directoryIds">A list of IDs representing the directories to fetch</param>
		/// <returns>A list of directory objects</returns>
		List<Directory> GetDirectories(List<Guid> directoryIds);

		/// <summary>
		/// Get all directories within this organization
		/// </summary>
		/// <returns>The directories for this organization</returns>
		List<Directory> GetAllDirectories();
	}
}
