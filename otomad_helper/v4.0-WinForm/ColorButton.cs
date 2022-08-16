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
		private readonly ColorDialog dialog = new ColorDialog {
			AnyColor = true,
			FullOpen = true,
			Color = Color.White,
		};

		public ColorButton() {
			base.UseVisualStyleBackColor = false;
			base.BackColor = Color.White;
			base.ForeColor = Color.Black;
			base.Text = "#FFFFFF";
			Click += ColorButton_Click;
		}

		private void ColorButton_Click(object sender, EventArgs e) {
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

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("该控件不支持此属性。"), DefaultValue("#FFFFFF")]
		public new string Text {
			get { return base.Text; }
			set { base.Text = value; }
		}

		[Category("Appearance"), DefaultValue(typeof(Color), "White"), Description("用户选定的颜色。")]
		public Color Color {
			get { return dialog.Color; }
			set {
				base.BackColor = dialog.Color = value;
				base.ForeColor = GetForeColor(value);
				base.Text = '#' + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2");
			}
		}

		[Category("Appearance"), DefaultValue("#FFFFFF"), Description("颜色的十六进制代码。")]
		public string Hex {
			get { return base.Text; }
			set {
				MatchCollection matches = Regex.Matches(value.ToUpper(), @"#[0-9A-F]{6}");
				string color = matches.Count != 0 ? matches[0].ToString() : "#000000";
				int r = Convert.ToInt16(color.Substring(1, 2), 16),
					g = Convert.ToInt16(color.Substring(3, 2), 16),
					b = Convert.ToInt16(color.Substring(5, 2), 16);
				Color = Color.FromArgb(r, g, b);
			}
		}

		/// <summary>
		/// 根据背景颜色获取前景（文字）颜色。
		/// </summary>
		/// <param name="backColor">背景颜色。</param>
		/// <returns>黑色或白色。</returns>
		public static Color GetForeColor(Color backColor) {
			double grey = backColor.R * 0.3 + backColor.G * 0.59 + backColor.B * 0.11;
			return grey < 128 ? Color.White : Color.Black;
		}
	}
}
