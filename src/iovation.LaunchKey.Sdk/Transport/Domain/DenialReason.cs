using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.Domain
{
    public class DenialReason : IEquatable<DenialReason>
    {
        public DenialReason(string id, string reason, bool fraud)
        {
            Id = id;
            Reason = reason;
            Fraud = fraud;
        }

        [JsonProperty("id")]
        public string Id { get; }

        [JsonProperty("reason")]
        public string Reason { get; }

        [JsonProperty("fraud")]
        public bool Fraud { get; }

        public bool Equals(DenialReason other)
        {
            return other != null &&
                   Id == other.Id &&
                   Reason == other.Reason &&
                   Fraud == other.Fraud;
        }

        public override bool Equals(object obj)
        {
            var item = obj as DenialReason;
            return Equals(item);
        }

        public override int GetHashCode()
        {
            var hashCode = -1513616286;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Reason);
            hashCode = hashCode * -1521134295 + Fraud.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"Denialreason{{Id:\"{Id}\",Reason:\"{Reason}\",Fraud:{Fraud}}}";
        }
    }
}
