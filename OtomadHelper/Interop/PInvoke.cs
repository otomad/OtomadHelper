using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.Interop {
	public class PInvoke {
		[Flags]
		public enum DWM_SYSTEMBACKDROP_TYPE {
			/// <summary>Mica</summary>
			DWMSBT_MAINWINDOW = 2,
			/// <summary>Acrylic</summary>
			DWMSBT_TRANSIENTWINDOW = 3,
			/// <summary>MicaAlt</summary>
			DWMSBT_TABBEDWINDOW = 4
		}

		[Flags]
		public enum DWMWINDOWATTRIBUTE {
			DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
			DWMWA_SYSTEMBACKDROP_TYPE = 38
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MARGINS {
			/// <summary>
			/// width of left border that retains its size
			/// </summary>
			public int cxLeftWidth;
			/// <summary>
			/// width of right border that retains its size
			/// </summary>
			public int cxRightWidth;
			/// <summary>
			/// height of top border that retains its size
			/// </summary>
			public int cyTopHeight;
			/// <summary>
			/// height of bottom border that retains its size
			/// </summary>
			public int cyBottomHeight;
		};

		[DllImport("dwmapi.dll")]
		internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, ref int pvAttribute, int cbAttribute);

		public static int ExtendFrame(IntPtr hwnd, MARGINS margins)
			=> DwmExtendFrameIntoClientArea(hwnd, ref margins);

		public static int SetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE attribute, int parameter)
			=> DwmSetWindowAttribute(hwnd, attribute, ref parameter, Marshal.SizeOf<int>());
	}
}
