using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using OtomadHelper.WPF.Controls.Flyouts;

namespace OtomadHelper.Test;
public partial class TestControls : Form {
	private readonly List<System.Windows.Window> flyouts = new();

	public TestControls() {
		InitializeComponent();
	}

	private void ComboBoxBtn_Click(object sender, EventArgs e) {
		if (sender is not Control control) return;
		Point location = control.PointToScreen(Point.Empty);
		(double dpiX, double dpiY) = this.GetDpi();

		ComboBoxFlyout flyout = new() {
			Left = location.X / dpiX,
			Top = location.Y / dpiY,
			Width = control.Width / dpiX,
		};
		flyouts.Add(flyout);
		flyout.ShowDialog();
	}

	protected override void WndProc(ref Message m) {
		// 0x0201FFFE is for : 0201 (left button down) and FFFE (HTERROR).
		// 0201 - left button down, 0204 - right button down, 0207 - middle button down
		if (m.Msg == 0x20 && m.LParam.ToInt32() is 0x0201FFFE or 0x0204FFFE or 0x0207FFFE) {
			foreach (System.Windows.Window flyout in flyouts.ToList()) {
				flyout.Vanish();
				flyouts.Remove(flyout);
			}
		} else {
			base.WndProc(ref m);
		}
	}
}
