using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class CustomFadeGainForm : Form {
		public CustomFadeGainForm() {
			InitializeComponent();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void PreviewBtn_Paint(object sender, PaintEventArgs e) {
			Button button = PreviewBtn;
			decimal min = FromBox.Value, max = ToBox.Value;
			Debug.WriteLine($"${min}, ${max}");
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			const int MARGIN = 0;
			decimal width = button.Width - MARGIN * 2, height = button.Height - MARGIN * 2;
			Rectangle r = new Rectangle(MARGIN, MARGIN, (int)width, (int)height);
			e.Graphics.DrawLines(new Pen(Color.FromArgb(0, 120, 212), 2), new Point[] {
				new Point(r.Left, (int)(r.Bottom - min / 100 * height)),
				new Point(r.Right, (int)(r.Bottom - max / 100 * height)),
			});
		}

		private void FadeBox_ValueChanged(object sender, EventArgs e) {
			PreviewBtn.Invalidate();
		}
	}
}
