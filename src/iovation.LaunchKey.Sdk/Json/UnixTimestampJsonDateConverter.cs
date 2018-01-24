using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iovation.LaunchKey.Sdk.Time;
using Newtonsoft.Json;

namespace iovation.LaunchKey.Sdk.Json
{
	class UnixTimestampJsonDateConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
	{
		private static readonly UnixTimeConverter _converter = new UnixTimeConverter();
			
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null) return;
			if (!(value is DateTime)) throw new InvalidOperationException("This must be used on DateTime objects");

			writer.WriteRawValue(_converter.GetUnixTimestamp((DateTime)value).ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.Value == null) return null;
			return _converter.GetDateTime((long)reader.Value);
		}
	}
}
