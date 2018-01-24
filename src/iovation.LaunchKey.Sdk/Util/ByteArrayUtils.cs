using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iovation.LaunchKey.Sdk.Util
{
	static class ByteArrayUtils
	{
		public static string ByteArrayToHexString(byte[] data, string separator = "")
		{
			var buffer = new StringBuilder();
			var count = 0;
			var useSeparator = !string.IsNullOrWhiteSpace(separator);
			foreach (var b in data)
			{
				if (count++ > 0 && useSeparator)
					buffer.Append(separator);
				buffer.Append(b.ToString("x2"));
			}
			return buffer.ToString();
		}

		public static byte[] HexStringToByteArray(string data)
		{
			if ((data.Length % 2) != 0)
				throw new ArgumentException("data should be of length of a multiple of 2");

			var buffer = new byte[data.Length / 2];
			
			for (var i = 0; i < data.Length / 2; i++)
			{
				var c = data.Substring(i*2, 2);
				buffer[i] = Convert.ToByte(c, 16);
			}

			return buffer;
		}

		public static bool AreEqual(byte[] a, byte[] b)
		{
			if (Object.ReferenceEquals(a, b)) return true;
			if (a == b) return true;
			if (a == null || b == null) return false;
			if (a.Length != b.Length) return false;
			for (var i = 0; i < a.Length; i++)
				if (a[i] != b[i]) return false;
			return true;
		}
	}
}
