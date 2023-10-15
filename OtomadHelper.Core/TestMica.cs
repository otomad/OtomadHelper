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
using System.Windows.Interop;

namespace OtomadHelper.Core {
	public partial class TestMica : UserControl {
		private IntPtr dockHandle;

		public TestMica(IntPtr dockHandle) {
			this.dockHandle = dockHandle;
			InitializeComponent();
		}

		[DllImport("uxtheme.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttribute, IntPtr pvAttribute, int cbAttribute);
		[Flags]
		public enum DwmWindowAttribute : uint {
			DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
			DWMWA_MICA_EFFECT = 1029,
		}

		protected override void OnHandleCreated(EventArgs e) {
			WindowUtils.EnableAcrylic(this, Color.Transparent);
			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.Clear(Color.Transparent);
		}

		/// <summary>
		/// Enable Mica on the given HWND.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="darkThemeEnabled"></param>
		public static void EnableMica(IntPtr hWnd) {
			
		}

		private void Button1_Click(object sender, EventArgs e) {
			//EnableMica(dockHandle);
			//EnableMica(Handle);
			Form1 form = new Form1();
			//form.Size = new Size(300, 400);
			//form.Load += (_sender, _e) => EnableMica(form.Handle);
			form.Show();
		}
	}

	internal class Form1 : Form {
		public Form1() {
			Size = new Size(300, 400);
			TestMica testMica = new TestMica(Handle);
			Controls.Add(testMica);
		}

		protected override void OnHandleCreated(EventArgs e) {
			// Use e.g. Color.FromArgb(128, Color.Lime) for a 50% opacity green tint.
			WindowUtils.EnableAcrylic(this, Color.Transparent);

			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.Clear(Color.Transparent);
		}
	}
}
