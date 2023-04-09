using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace OtomadHelper.Shared {
	public class Encodings {
		public static string EncodeNonAscii(string value) {
			StringBuilder sb = new StringBuilder();
			foreach (char c in value) {
				if (c == '\\') {
					sb.Append("\\\\");
				} else if (c > 127) {
					// This character is too big for ASCII
					string encodedValue = "\\u" + ((int)c).ToString("x4");
					sb.Append(encodedValue);
				} else {
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		public static string DecodeEncodedNonAscii(string value) {
			value = Regex.Replace(value, @"(?<!\\)\\u(?<Value>[a-zA-Z0-9]{4})", m =>
				((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString());
			value = value.Replace("\\\\", "\\");
			return value;
		}
	}
}
