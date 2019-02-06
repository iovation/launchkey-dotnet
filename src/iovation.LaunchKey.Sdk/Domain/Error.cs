using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Domain
{
    public class Error
    {
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("error_detail")]
        public object ErrorDetail { get; set; }

        [JsonProperty("error_data")]
        public IDictionary<String, Object> ErrorData { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            Error error = (Error)obj;
            return ErrorCode == error.ErrorCode && ErrorDetail == error.ErrorDetail && ErrorData == error.ErrorData;
        }

        public override int GetHashCode()
        {
            int hashCode = this.GetType().GetHashCode();
            if (ErrorCode != null) hashCode = hashCode ^ ErrorCode.GetHashCode();
            if (ErrorDetail != null) hashCode = hashCode ^ ErrorDetail.GetHashCode();
            if (ErrorData != null) hashCode = hashCode ^ ErrorData.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{GetType().Name}{{ErrorCode='{ErrorCode}', ErrorDetail= '{ErrorDetail}', ErrorData='{ErrorData}'}}";
        }
    }
}