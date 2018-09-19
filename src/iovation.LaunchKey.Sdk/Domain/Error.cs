using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Domain
{
	public class Error
	{
		[JsonProperty("error_code")]
		public string ErrorCode { get; set; }

		[JsonProperty("error_detail")]
		public object ErrorDetail { get; set; }
	}
}