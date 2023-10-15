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
		[DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);

		/// <summary>
		/// Enable Mica on the given HWND.
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="darkThemeEnabled"></param>
		public static void EnableMica(IntPtr hWnd) {
			AccentPolicy accent = new AccentPolicy();
			accent.AccentState = AccentState.ACCENT_ENABLE_ACRYLICBLURBEHIND;
			accent.GradientColor = 0 << 0 | 0 << 8 | 0 << 16 | 10 << 24;

			// 将托管结构转换为非托管对象。
			int accentPolicySize = Marshal.SizeOf(accent);
			IntPtr accentPtr = Marshal.AllocHGlobal(accentPolicySize);
			Marshal.StructureToPtr(accent, accentPtr, false);

			// 设置窗口组合特性。
			try {
				// 设置模糊特效。
				WindowCompositionAttributeData data = new WindowCompositionAttributeData {
					Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
					SizeOfData = accentPolicySize,
					Data = accentPtr,
				};
				SetWindowCompositionAttribute(hWnd, ref data);
			} finally {
				// 释放非托管对象。
				Marshal.FreeHGlobal(accentPtr);
			}
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

	[StructLayout(LayoutKind.Sequential)]
	internal struct AccentPolicy {
		public AccentState AccentState;
		public int AccentFlags;
		public int GradientColor;
		public int AnimationId;
	}

	internal enum AccentState {
		/// <summary>
		/// 完全禁用 DWM 的叠加特效。
		/// </summary>
		ACCENT_DISABLED = 0,
		ACCENT_ENABLE_GRADIENT = 1,
		ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
		ACCENT_ENABLE_BLURBEHIND = 3,
		ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
		ACCENT_INVALID_STATE = 5,
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct WindowCompositionAttributeData {
		public WindowCompositionAttribute Attribute;
		public IntPtr Data;
		public int SizeOfData;
	}

	internal enum WindowCompositionAttribute {
		// 省略其他未使用的字段
		WCA_ACCENT_POLICY = 19,
		// 省略其他未使用的字段
	}
}
