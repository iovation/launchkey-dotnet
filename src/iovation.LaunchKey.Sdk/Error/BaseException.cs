using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Base class for all LaunchKey-related exceptions. Useful for quick and dirty error handling when making requests
	/// </summary>
	public class BaseException : Exception
	{
		public string ErrorCode { get; }

		public BaseException(string message) : base(message)
		{
		}

		public BaseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public BaseException(string message, Exception innerException, string errorCode) : base(message, innerException)
		{
			ErrorCode = errorCode;
		}

		public override string ToString()
		{
			return $"{GetType().Name}{{message='{Message}', ErrorCode='{ErrorCode}'}}";
		}
	}
}
