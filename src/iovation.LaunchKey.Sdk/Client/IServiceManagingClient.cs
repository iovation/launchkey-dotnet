using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Client
{
    public interface IServiceManagingClient
	{
		Guid CreateService(string name, string description, Uri icon, Uri callbackURL, bool active);
	}
}
