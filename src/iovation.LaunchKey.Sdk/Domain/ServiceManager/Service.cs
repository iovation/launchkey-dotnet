using System;

namespace iovation.LaunchKey.Sdk.Domain.ServiceManager
{
    public class Service
    {
        /// <summary>
        /// The unique ID of this service
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of this service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The description of this service
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The URL of the icon image associated with this service
        /// </summary>
        public Uri Icon { get; set; }

        /// <summary>
        /// The URL for Webhook callbacks associated with this service
        /// </summary>
        public Uri CallbackUrl { get; set; }

        /// <summary>
        /// Whether or not this service is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Create a Service entity
        /// </summary>
        /// <param name="id">unique ID of the service</param>
        /// <param name="name">The service name</param>
        /// <param name="description">The service description</param>
        /// <param name="icon">Icon to display for the service</param>
        /// <param name="callbackUrl">Callback URL for Webhook events</param>
        /// <param name="active">Whether service is active or not</param>
        public Service(Guid id, string name, string description, Uri icon, Uri callbackUrl, bool active)
        {
            Id = id;
            Name = name;
            Description = description;
            Icon = icon;
            CallbackUrl = callbackUrl;
            Active = active;
        }
    }
}