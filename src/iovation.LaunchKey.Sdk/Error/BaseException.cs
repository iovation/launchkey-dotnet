using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Error
{
	/// <summary>
	/// Base class for all LaunchKey-related exceptions. Useful for quick and dirty error handling when making requests
	/// </summary>
	[Serializable]
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

		public BaseException(SerializationInfo info, StreamingContext context) 
			: base(info, context)
		{
			ErrorCode = info.GetString("ErrorCode");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("ErrorCode", ErrorCode);
			base.GetObjectData(info, context);
		}

		public override string ToString()
		{
			return $"{GetType().Name}{{message='{Message}', ErrorCode='{ErrorCode}'}}";
		}

		public override bool Equals(object obj)
		{
			var exception = obj as BaseException;
			return exception != null &&
				   ErrorCode == exception.ErrorCode;
		}
	}
}
