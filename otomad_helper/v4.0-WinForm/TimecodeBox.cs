using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ScriptPortal.Vegas;

namespace Otomad.VegasScript.OtomadHelper.V4 {

	[ToolboxBitmap(typeof(NumericUpDown))]
	public class TimecodeBox : NumericUpDown {
		public TimecodeBox() : base() { }

		#region | Fields |
		private bool useTimecodeDeal = false;
		private const string DEFAULT_TIME = "0:00.000";
		private const RulerFormat format = RulerFormat.Time;
		private Selection? lastSelection;
		#endregion

		#region | Properties |
		[Description("与控件关联的文本。"), Category("Appearance"), DefaultValue(DEFAULT_TIME)]
		public override string Text {
			get { return base.Text; }
			set {
				TimecodeText.TimeUnit unit = TimecodeText.GetUnit(TextBox.SelectionStart, base.Text);
				if (!useTimecodeDeal) base.Text = TimecodeText.DealLegal(value);
				else Timecode = Timecode.FromPositionString(value, format);
				Select(TimecodeText.GetPosition(unit, base.Text), 0);
			}
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public new int Value {
			get { return new TimecodeText(Text).Ms; }
			set {
				if (!useTimecodeDeal) base.Text = new TimecodeText(value).ToString();
				else DoubleValue = value;
			}
		}

		[Description("与控件关联的文本对应的毫秒双精度浮点值。"), Category("Behavior"), DefaultValue(0)]
		public double DoubleValue {
			get { return Value; }
			set {
				if (!useTimecodeDeal) Value = (int)value;
				else Timecode = Timecode.FromMilliseconds(value);
			}
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Milliseconds {
			get { return Value; }
			set { Value = value; }
		}

		[Description("与控件关联的文本对应的时间码值。"), Category("Behavior"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Timecode Timecode {
			get {
				return Timecode.FromMilliseconds(DoubleValue);
			}
			set {
				base.Text = value.ToPositionString();
			}
		}

		[Description("是否使用 Vegas 时间码处理。"), Category("Behavior"), DefaultValue(false)]
		public bool UseTimecodeDeal {
			get { return useTimecodeDeal; }
			set { useTimecodeDeal = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("TimecodeBox 控件不支持此属性。"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue(3)]
		public new int DecimalPlaces {
			get {
				return base.DecimalPlaces;
			}
			set {
				base.DecimalPlaces = value;
			}
		}

		/// <summary>
		/// 获取或设置 <see cref="NumberFormatInfo"/>，它定义适合区域性的、显示数字、货币和百分比的格式。
		/// </summary>
		private static NumberFormatInfo NumberFormat { get { return CultureInfo.CurrentCulture.NumberFormat; } }
		#endregion

		#region | Classes |
		/// <summary>
		/// 时间码文本处理辅助类。
		/// </summary>
		private class TimecodeText {
			#region 逻辑方法部分
			/// <summary>
			/// 处理输入文本的合法性。
			/// </summary>
			/// <param name="raw">源输入文本。</param>
			/// <returns>合法的文本。</returns>
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
				return Format(raw);
			}

			/// <summary>
			/// 将输入的时间格式的字符串转换为毫秒。
			/// </summary>
			/// <param name="clipTrimTime">时间格式，如 “0:00.000”。</param>
			/// <returns>毫秒值。</returns>
			private static int ClipTrimTime2Ms(string clipTrimTime) {
				if (clipTrimTime == "") clipTrimTime = DEFAULT_TIME;
				int h = 0, m = 0, s = 0, ms = 0;
				string[] timeSplitDot = clipTrimTime.Split(NumberFormat.NumberDecimalSeparator[0]);
				if (timeSplitDot.Length >= 2) {
					string ms_str = timeSplitDot[1];
					while (ms_str.Length < 3) ms_str += '0';
					ms = int.Parse(ms_str);
				}
				string[] timeSplitColon = timeSplitDot[0].Split(':');
				var tryParseInt = new Func<string, int, int>((str, def) => {
					int result;
					bool ok = int.TryParse(str, out result);
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
			/// <param name="value">毫秒值。</param>
			/// <returns>时间格式，如 “0:00.000”。</returns>
			private static string FormatClipTrimTime(int value) {
				int ms = value % 1000; value /= 1000;
				int s = value % 60; value /= 60;
				int m = value % 60; value /= 60;
				int h = value;
				return h == 0 ?
					string.Format("{0:D}:{1:D2}.{2:D3}", m, s, ms) :
					string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", h, m, s, ms);
			}

			/// <summary>
			/// 格式化时间值。
			/// </summary>
			/// <param name="clipTrimTime">不规范的时间格式。</param>
			/// <returns>时间格式，如 “0:00.000”。</returns>
			public static string Format(string clipTrimTime) {
				return new TimecodeText(clipTrimTime).ToString();
			}
			#endregion

			private int ms = 0;
			/// <summary>毫秒值。</summary>
			public int Ms { get { return ms; } }

			/// <summary>
			/// 格式化时间值。
			/// </summary>
			/// <param name="clipTrimTime">不规范的时间格式。</param>
			public TimecodeText(string clipTrimTime) {
				ms = ClipTrimTime2Ms(clipTrimTime);
			}

			/// <summary>
			/// 从毫秒值初始化。
			/// </summary>
			/// <param name="ms">毫秒值。</param>
			public TimecodeText(int ms) {
				this.ms = ms;
			}

			/// <summary>
			/// 格式化时间值。
			/// </summary>
			/// <returns>时间格式，如 “0:00.000”。</returns>
			public override string ToString() {
				return FormatClipTrimTime(ms);
			}

			/// <summary>
			/// 对时间码加减。
			/// </summary>
			/// <param name="unit">单位。</param>
			/// <param name="value">数值。默认为加一。</param>
			/// <returns>返回自己，用于链式。</returns>
			public TimecodeText AddOrSub(TimeUnit unit, int value = 1) {
				int mul = unit == TimeUnit.SECONDS ? 1000 :
					unit == TimeUnit.MINUTES ? 60000 :
					unit == TimeUnit.HOURS ? 3600000 : 1;
				int _ms = ms;
				ms += value * mul;
				if (ms < 0) ms = _ms;
				return this;
			}

			/// <summary>
			/// 时间单位。
			/// </summary>
			public enum TimeUnit {
				MILLISECONDS, SECONDS, MINUTES, HOURS,
			}

			internal static TimeUnit GetUnit(int cursor, string text) {
				int dotPosition = text.IndexOf(NumberFormat.NumberDecimalSeparator[0]);
				if (dotPosition != -1 && cursor > dotPosition)
					return TimeUnit.MILLISECONDS;
				dotPosition = text.Length - 1;
				int colonCount = 0;
				char lastChar = '\0';
				for (int i = dotPosition - 1; i >= 0 && i >= cursor; i--) {
					char ch = text[i];
					if (ch == ':' && lastChar != ch)
						colonCount++;
					lastChar = ch;
				}
				return colonCount == 0 ? TimeUnit.SECONDS :
					colonCount == 1 ? TimeUnit.MINUTES : TimeUnit.HOURS;
			}

			internal static int GetPosition(TimeUnit unit, string text) {
				int newCursor = unit == TimeUnit.MILLISECONDS ? 0 :
					unit == TimeUnit.SECONDS ? 4 :
					unit == TimeUnit.MINUTES ? 7 : 10;
				newCursor = text.Length - newCursor;
				if (newCursor < 0) newCursor = 0;
				return newCursor;
			}
		}

		/// <summary>
		/// 选择区域。
		/// </summary>
		private struct Selection {
			public int start;
			public int end;
			public int length { get { return end - start; } }
			public Selection(int start, int end) {
				this.start = start;
				this.end = end;
			}
			public bool legal { get { return start < end; } }
		}
		#endregion

		#region | Methods |
		protected override void UpdateEditText() {
			Text = base.Text;
		}

		protected override void OnKeyDown(KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) return;
			base.OnKeyDown(e);
		}

		protected override void OnTextBoxKeyPress(object source, KeyPressEventArgs e) {
			string key = e.KeyChar.ToString();
			if (key == NumberFormat.NegativeSign) { // 禁止输入负号。
				e.Handled = true;
				System.Media.SystemSounds.Beep.Play();
			} else if (key == ":") { } // 允许输入冒号。
			else base.OnTextBoxKeyPress(source, e);
		}

		protected override void OnMouseClick(MouseEventArgs e) {
			base.OnMouseClick(e);
			string text = base.Text;
			if (TextBox.SelectionLength != 0) return;
			int cursor = TextBox.SelectionStart;
			if (e != null && lastSelection.HasValue && cursor >= lastSelection.Value.start && cursor <= lastSelection.Value.end) {
				lastSelection = null;
				return;
			}
			lastSelection = null;
			Selection selection = new Selection(0, text.Length);
			for (int i = cursor - 1; i >= 0; i--) {
				char ch = text[i];
				if (!char.IsDigit(ch)) {
					selection.start = i + 1;
					break;
				}
			}
			for (int i = cursor; i < text.Length; i++) {
				char ch = text[i];
				if (!char.IsDigit(ch)) {
					selection.end = i;
					break;
				}
			}
			if (selection.legal)
				Select(selection.start, selection.length);
			lastSelection = selection;
		}

		public override void UpButton() {
			OnUpDown(1);
		}
		public override void DownButton() {
			OnUpDown(-1);
		}
		protected void OnUpDown(int value) {
			string text = base.Text;
			TimecodeText.TimeUnit unit = TimecodeText.GetUnit(TextBox.SelectionStart, text);
			Value = new TimecodeText(text).AddOrSub(unit, value).Ms;
			int newCursor = TimecodeText.GetPosition(unit, base.Text);
			Select(newCursor, 0);
			OnMouseClick(null);
		}

		/// <summary>
		/// 获取控件中的文本框部分。
		/// </summary>
		protected TextBox TextBox {
			get {
				PropertyInfo property = typeof(NumericUpDown).GetProperty("TextBox", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic); // 使用 GetType 差点栈溢出。
				if (property == null)
					throw new MissingFieldException(new StackTrace().GetFrame(0).ToString());
				return (TextBox)property.GetValue(this);
			}
		}
		#endregion
	}
}
