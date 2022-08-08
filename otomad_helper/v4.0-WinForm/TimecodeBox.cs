using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {

	//[ToolboxBitmap(typeof(TextBox))]
	public class TimecodeBox : TextBox {
		public TimecodeBox() : base() {
			Leave += (sender, e) => {
				base.Text = DealLegal(Text);
			};
		}

		const string DEFAULT_TIME = "0:00.000";

		[Description("与控件关联的文本。"), Category("Appearance"), DefaultValue(DEFAULT_TIME)]
		public override string Text {
			get { return base.Text; }
			set { base.Text = DealLegal(value); }
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Value {
			get { return ClipTrimTime2Ms(Text); }
			set { Text = FormatClipTrimTime(value); }
		}

		[Description("与控件关联的文本对应的毫秒双精度浮点值。"), Category("Behavior"), DefaultValue(0)]
		public double DoubleValue {
			get { return Value; }
			set { Value = (int)value; }
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Millisecond {
			get { return Value; }
			set { Value = value; }
		}

		public static string DealLegal(string raw) {
			raw = new Regex(@"[:：;；]+").Replace(raw, ":");
			raw = new Regex(@"[\.。．,，、]+").Replace(raw, ".");
			raw = new Regex(@"\s").Replace(raw, "");
			raw = new Regex(@"^\.").Replace(raw, "0.");
			MatchCollection matches = Regex.Matches(raw, @"(\d+:){0,2}\d+(\.\d{1,3})?");
			if (matches.Count == 0) return DEFAULT_TIME;
			else {
				string bestMatch = "";
				foreach (Match match in matches)
					if (match.ToString().Length > bestMatch.Length)
						bestMatch = match.ToString();
				raw = bestMatch;
			}
			return FormatClipTrimTime(raw);
		}

		/// <summary>
		/// 将输入的时间格式的字符串转换为毫秒。
		/// </summary>
		/// <param name="clipTrimTime">时间格式，如 “0:00.000”</param>
		/// <returns>毫秒值</returns>
		public static int ClipTrimTime2Ms(string clipTrimTime) {
			if (clipTrimTime == "") clipTrimTime = DEFAULT_TIME;
			int h = 0, m = 0, s = 0, ms = 0;
			string[] timeSplitDot = clipTrimTime.Split('.');
			if (timeSplitDot.Length >= 2) {
				string ms_str = timeSplitDot[1];
				while (ms_str.Length < 3) ms_str += '0';
				ms = int.Parse(ms_str);
			}
			string[] timeSplitColon = timeSplitDot[0].Split(':');
			var tryParseInt = new Func<string, int, int>((str, def) => {
				bool ok = int.TryParse(str, out int result);
				if (!ok) result = def;
				return result;
			});
			s = tryParseInt(timeSplitColon[timeSplitColon.Length - 1], 0); // timeSplitColon[^1]
			if (timeSplitColon.Length >= 2) m = tryParseInt(timeSplitColon[timeSplitColon.Length - 2], 0); // timeSplitColon[^2]
			if (timeSplitColon.Length >= 3) h = tryParseInt(timeSplitColon[timeSplitColon.Length - 3], 0); // timeSplitColon[^3]
			int value = ((h * 60 + m) * 60 + s) * 1000 + ms;
			return value;
		}

		/// <summary>
		/// 格式化时间值。
		/// </summary>
		/// <param name="value">毫秒值</param>
		/// <returns>时间格式，如 “0:00.000”</returns>
		public static string FormatClipTrimTime(int value) {
			int ms = value % 1000; value /= 1000;
			int s = value % 60; value /= 60;
			int m = value % 60; value /= 60;
			int h = value;
			return h == 0 ?
				string.Format("{0:D}:{1:D2}.{2:D3}", m, s, ms) :
				string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", h, m, s, ms);
		}

		public static string FormatClipTrimTime(string clipTrimTime) {
			return FormatClipTrimTime(ClipTrimTime2Ms(clipTrimTime));
		}
	}
}
