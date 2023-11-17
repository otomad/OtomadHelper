using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class TestDarkModeForm : Form {
		public TestDarkModeForm() {
			InitializeComponent();
			for (int i = 0; i < 6; i++) {
				int index = dataGridView1.Rows.Add();
				dataGridView1.Rows[index].Cells[0].Value = "Cell " + index;
			}
			ThemeAllControls();
		}

		private void ThemeAllControls(Control parent = null) {
			parent = parent ?? this;
			Action<Control> Theme = control => {
				int trueValue = 0x01;
				SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
				DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
				DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
				control.BackColor = Color.FromArgb(32, 32, 32);
				control.ForeColor = Color.White;
			};
			if (parent == this) Theme(this);
			foreach (Control control in parent.Controls) {
				Theme(control);
				if (control.Controls.Count != 0)
					ThemeAllControls(control);
			}
		}

		[DllImport("uxtheme.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
		[Flags]
		public enum DwmWindowAttribute : uint {
			DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
			DWMWA_MICA_EFFECT = 1029
		}
	}
}
