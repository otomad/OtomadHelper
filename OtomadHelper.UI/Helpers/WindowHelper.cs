using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using WinRT;

namespace OtomadHelper.UI;
internal class WindowHelper {
	public WindowHelper(Window window) {
		this.window = window;
		SubClassing();
	}

	private Window window;
	private delegate IntPtr WinProc(IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);
	private WinProc newWndProc = null;
	private IntPtr oldWndProc = IntPtr.Zero;
	[DllImport("user32")]
	private static extern IntPtr SetWindowLong(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);
	[DllImport("user32.dll")]
	static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

	private void SubClassing() {
		// Get the Window's HWND
		IntPtr hwnd = window.As<IWindowNative>().WindowHandle;
		newWndProc = new WinProc(NewWindowProc);
		oldWndProc = SetWindowLong(hwnd, PInvoke.User32.WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
	}

	public int MinWidth = 800;
	public int MinHeight = 600;

	[StructLayout(LayoutKind.Sequential)]
	struct MINMAXINFO {
		public PInvoke.POINT ptReserved;
		public PInvoke.POINT ptMaxSize;
		public PInvoke.POINT ptMaxPosition;
		public PInvoke.POINT ptMinTrackSize;
		public PInvoke.POINT ptMaxTrackSize;
	}

	private IntPtr NewWindowProc(IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam) {
		switch (Msg) {
			case PInvoke.User32.WindowMessage.WM_GETMINMAXINFO:
				var dpi = PInvoke.User32.GetDpiForWindow(hWnd);
				float scalingFactor = (float)dpi / 96;

				MINMAXINFO minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
				minMaxInfo.ptMinTrackSize.x = (int)(MinWidth * scalingFactor);
				minMaxInfo.ptMinTrackSize.y = (int)(MinHeight * scalingFactor);
				Marshal.StructureToPtr(minMaxInfo, lParam, true);
				break;

		}
		return CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
	internal interface IWindowNative {
		IntPtr WindowHandle { get; }
	}
}
