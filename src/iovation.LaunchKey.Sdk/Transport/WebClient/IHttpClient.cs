using System.Collections.Generic;

namespace iovation.LaunchKey.Sdk.Transport.WebClient
{
    public interface IHttpClient
    {
        HttpResponse ExecuteRequest(HttpMethod method, string url, string body, Dictionary<string, string> headers);
    }
}