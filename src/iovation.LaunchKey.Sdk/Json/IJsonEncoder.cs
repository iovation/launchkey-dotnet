using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Json
{
	public interface IJsonEncoder
	{
		TResult DecodeObject<TResult>(string data);
		string EncodeObject(object obj);
	}
}