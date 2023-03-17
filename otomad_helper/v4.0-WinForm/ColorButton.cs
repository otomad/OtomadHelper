using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public class ColorButton : Button {
		private readonly AlphaColorDialog dialog = new AlphaColorDialog {
			AnyColor = true,
			FullOpen = true,
			Color = Color.White,
		};

		public ColorButton() {
			base.UseVisualStyleBackColor = false;
			base.BackColor = Color.White;
			base.ForeColor = Color.Black;
			base.Text = "#FFFFFFFF";
			Click += ColorButton_Click;
		}

		private void ColorButton_Click(object sender, EventArgs e) {
			dialog.Color = Color;
			DialogResult dr = dialog.ShowDialog();
			if (dr == DialogResult.OK) Color = dialog.Color;
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("该控件不支持此属性。"), DefaultValue(false)]
		public new bool UseVisualStyleBackColor {
			get { return base.UseVisualStyleBackColor; }
			set { base.UseVisualStyleBackColor = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("该控件不支持此属性。"), DefaultValue(typeof(Color), "White")]
		public new Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("该控件不支持此属性。"), DefaultValue(typeof(Color), "Black")]
		public new Color ForeColor {
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		private string _text = string.Empty;
		[Category("Appearance"), DefaultValue(""), Description("与控件关联的文本。")]
		public new string Text {
			get { return _text; }
			set {
				_text = value;
				if (string.IsNullOrEmpty(value)) base.Text = Hex;
				else base.Text = _text;
			}
		}

		private Color color = Color.White;
		[Category("Appearance"), DefaultValue(typeof(Color), "White"), Description("用户选定的颜色。")]
		public Color Color {
			get { return color; }
			set {
				base.BackColor = color = value;
				base.ForeColor = GetForeColor(value);
				if (string.IsNullOrEmpty(_text)) Text = string.Empty;
			}
		}

		[Category("Appearance"), DefaultValue("#FFFFFFFF"), Description("颜色的十六进制代码。")]
		public string Hex {
			get {
				return '#' + Color.R.ToString("X2") + Color.G.ToString("X2") + Color.B.ToString("X2") + Color.A.ToString("X2");
			}
			set {
				MatchCollection matches = Regex.Matches(value.ToUpper(), @"#[0-9A-F]{8}");
				string color;
				if (matches.Count != 0) color = matches[0].ToString();
				else {
					matches = Regex.Matches(value.ToUpper(), @"#[0-9A-F]{6}");
					if (matches.Count != 0) color = matches[0].ToString() + "FF";
					else color = "#00000000";
				}
				int r = Convert.ToInt16(color.Substring(1, 2), 16),
					g = Convert.ToInt16(color.Substring(3, 2), 16),
					b = Convert.ToInt16(color.Substring(5, 2), 16),
					a = Convert.ToInt16(color.Substring(7, 2), 16);
				Color = Color.FromArgb(a, r, g, b);
			}
		}

		/// <summary>
		/// 根据背景颜色获取前景（文字）颜色。
		/// </summary>
		/// <param name="backColor">背景颜色。</param>
		/// <returns>黑色或白色。</returns>
		public static Color GetForeColor(Color backColor) {
			int r = backColor.R, g = backColor.G, b = backColor.B, a = backColor.A;
			Func<int, int> MixAlpha = new Func<int, int>(c => (255 - c) * (255 - a) / 255 + c);
			r = MixAlpha(r); g = MixAlpha(g); b = MixAlpha(b);
			double grey = r * 0.3 + g * 0.59 + b * 0.11;
			return grey < 128 ? Color.White : Color.Black;
		}
	}
}
