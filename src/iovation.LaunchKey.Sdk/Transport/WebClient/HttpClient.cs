using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace iovation.LaunchKey.Sdk.Transport.WebClient
{
	/// <summary>
	/// a lightweight wrapper around WebRequest. Handles well-known corner cases.
	/// Treats status codes as non-exceptional
	/// </summary>
	public class WebRequestHttpClient : IHttpClient
	{
		private int _timeout;

		public WebRequestHttpClient(TimeSpan timeout)
		{
			_timeout = (int) timeout.TotalMilliseconds;
		}

		public HttpResponse ExecuteRequest(
			HttpMethod method,
			string url,
			string body,
			Dictionary<string, string> headers)
		{
			var request = CreateRequest(method, url, headers);
			return ExecuteRequest(request, body);
		}

		private HttpWebRequest CreateRequest(HttpMethod method, string url, Dictionary<string, string> headers)
		{
			var request = (HttpWebRequest) WebRequest.Create(url);

			request.Method = method.ToString();
			request.Timeout = _timeout;
			request.ReadWriteTimeout = _timeout;
			if (headers != null)
			{
				foreach (var header in headers)
				{
					request.Headers.Add(header.Key, header.Value);
				}
			}

			return request;
		}

		private HttpResponse ResponseFromFrameworkResponse(HttpWebResponse httpWebResponse)
		{
			using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
			{
				var response = new HttpResponse();
				response.Headers = httpWebResponse.Headers;
				response.StatusCode = httpWebResponse.StatusCode;
				response.ResponseBody = streamReader.ReadToEnd();
				response.StatusDescription = httpWebResponse.StatusDescription;
				return response;
			}
		}

		private HttpResponse ExecuteRequest(HttpWebRequest request, string requestBody)
		{
			try
			{
				if (!string.IsNullOrEmpty(requestBody))
				{
					using (var reqStream = new StreamWriter(request.GetRequestStream()))
					{
						reqStream.Write(requestBody);
					}
				}

				using (var httpWebResponse = (HttpWebResponse) request.GetResponse())
				{
					return ResponseFromFrameworkResponse(httpWebResponse);
				}
			}
			catch (WebException ex)
			{
				if (ex.Status == WebExceptionStatus.ProtocolError)
				{
					var response = ex.Response as HttpWebResponse;
					if (response != null)
					{
						return ResponseFromFrameworkResponse(response);
					}
				}

				throw;
			}
			finally
			{
				// this fixes a problem that I've seen on some systems with some
				// network drivers. effectively the request lingers natively and results in exhaustion
				// of threadpool resources
				if (request != null)
					request.Abort();
			}
		}
	}
}