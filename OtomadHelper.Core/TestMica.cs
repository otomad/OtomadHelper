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

		/// <summary>
		/// Type of system backdrop to be rendered by DWM
		/// </summary>
		public enum DWM_SYSTEMBACKDROP_TYPE : uint {
			DWMSBT_AUTO = 0,
			/// <summary>
			/// no backdrop
			/// </summary>
			DWMSBT_NONE = 1,
			/// <summary>
			/// Use tinted blurred wallpaper backdrop (Mica)
			/// </summary>
			DWMSBT_MAINWINDOW = 2,
			/// <summary>
			/// Use Acrylic backdrop
			/// </summary>
			DWMSBT_TRANSIENTWINDOW = 3,
			/// <summary>
			/// Use blurred wallpaper backdrop
			/// </summary>
			DWMSBT_TABBEDWINDOW = 4,
		}

		private const uint DWMWA_SYSTEMBACKDROP_TYPE = 38;

		/// <summary>
		/// Enable Mica on the given HWND.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="darkThemeEnabled"></param>
		public static void EnableMica(IntPtr hWnd) {
			DWM_SYSTEMBACKDROP_TYPE backdropType = DWM_SYSTEMBACKDROP_TYPE.DWMSBT_MAINWINDOW;
			GCHandle value = GCHandle.Alloc((uint)backdropType, GCHandleType.Pinned);
			int result = DwmSetWindowAttribute(hWnd, DWMWA_SYSTEMBACKDROP_TYPE, value.AddrOfPinnedObject(), sizeof(uint));
			value.Free();
			if (result != 0)
				throw Marshal.GetExceptionForHR(result);
		}

		private void Button1_Click(object sender, EventArgs e) {
			EnableMica(dockHandle);
			EnableMica(Handle);
			Form form = new Form();
			form.Size = new Size(300, 400);
			form.Load += (_sender, _e) => EnableMica(form.Handle);
			form.Show();
		}
	}
}
