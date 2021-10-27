using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Domain
{
    /// <summary>
    /// Represents a public key on a service or directory
    /// </summary>
    public class PublicKey
    {
        /// <summary>
        /// The unique ID for this public key. Use this when updating/removing the key via API calls.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Whether or not the key is currently active
        /// </summary>
        public bool Active { get; }

        /// <summary>
        /// The date and time this key was created
        /// </summary>
        public DateTime Created { get; }

        /// <summary>
        /// The date and time this key will expire
        /// </summary>
        public DateTime? Expires { get; }

        /// <summary>
        /// The type of key that represents what it will be used for. IE: encryption, signature, or both.
        /// </summary>
        public KeyType KeyType { get; }

        public PublicKey(string id, bool active, DateTime created, DateTime? expires, KeyType keyType = KeyType.BOTH)
        {
            Id = id;
            Active = active;
            Created = created;
            Expires = expires;
            KeyType = keyType;
        }

        public override bool Equals(object obj)
        {
            return obj is PublicKey key &&
                   Id == key.Id &&
                   Active == key.Active &&
                   Created == key.Created &&
                   Expires == key.Expires &&
                   KeyType == key.KeyType;
        }

        public override int GetHashCode()
        {
            int hashCode = -683061471;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + Active.GetHashCode();
            hashCode = hashCode * -1521134295 + Created.GetHashCode();
            hashCode = hashCode * -1521134295 + Expires.GetHashCode();
            hashCode = hashCode * -1521134295 + KeyType.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// The type of key that is being utilized
    /// </summary>
    public enum KeyType
    {
        /// <summary>
        /// Other exists only to allow for forward compatibility to future key types
        /// </summary>
        OTHER = -1,

        /// <summary>
        /// This key can be used to both decrypt responses as well as sign requests
        /// </summary>
        BOTH = 0,

        /// <summary>
        /// This key can only be used to decrypt requests
        /// </summary>
        ENCRYPTION = 1,

        /// <summary>
        /// This key can only be used to sign requests
        /// </summary>
        SIGNATURE = 2
    }
}