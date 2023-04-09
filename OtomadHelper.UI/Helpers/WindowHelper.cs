using Microsoft.UI.Xaml;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace OtomadHelper.UI;
internal class WindowHelper {
	private readonly Window window;
	private WinProc newWndProc = null;
	private IntPtr oldWndProc = IntPtr.Zero;
	private delegate IntPtr WinProc(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("User32.dll")]
	internal static extern int GetDpiForWindow(IntPtr hwnd);

	[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
	private static extern int SetWindowLong32(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc);

	[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
	private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc);

	[DllImport("user32.dll")]
	private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", EntryPoint = "SendMessage")]
	private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

	public static int MinWindowWidth { get; set; } = 500;
	public static int MinWindowHeight { get; set; } = 400;

	public WindowHelper(Window window) {
		this.window = window;
		RegisterWindowMinMax(window);
	}

	private void RegisterWindowMinMax(Window window) {
		IntPtr hwnd = GetWindowHandleForCurrentWindow(window);

		newWndProc = new WinProc(WndProc);
		oldWndProc = SetWindowLongPtr(hwnd, WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
	}

	private static IntPtr GetWindowHandleForCurrentWindow(object target) =>
		WinRT.Interop.WindowNative.GetWindowHandle(target);

	private IntPtr WndProc(IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam) {
		switch (Msg) {
			case WindowMessage.WM_GETMINMAXINFO:
				int dpi = GetDpiForWindow(hWnd);
				float scalingFactor = (float)dpi / 96;

				MINMAXINFO minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
				minMaxInfo.ptMinTrackSize.x = (int)(MinWindowWidth * scalingFactor);
				minMaxInfo.ptMinTrackSize.y = (int)(MinWindowHeight * scalingFactor);

				Marshal.StructureToPtr(minMaxInfo, lParam, true);
				break;
			case WindowMessage.WM_COPYDATA:
				COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT)); // 接收封装的消息
				// 以下为逻辑处理
				string strResult = Shared.Encodings.DecodeEncodedNonAscii(cds.lpData);
				string strType = cds.dwData.ToString();
				if (strType == "1") Received(strResult);
				break;
		}
		return CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
	}

	public void SendMessage(string data) {
		data = Shared.Encodings.EncodeNonAscii(data);
		byte[] sarr = Encoding.Default.GetBytes(data);
		int len = sarr.Length;
		COPYDATASTRUCT cds;
		cds.dwData = (IntPtr)Convert.ToInt16(1); // 可以是任意值
		cds.cbData = len + 1; // 指定 lpData 内存区域的字节数
		cds.lpData = data; // 发送给目标窗口所在进程的数据
		if (App.ServerHwnd != IntPtr.Zero)
			SendMessage(App.ServerHwnd.ToInt32(), (int)WindowMessage.WM_COPYDATA, 0, ref cds);
	}

	private static IntPtr SetWindowLongPtr(IntPtr hWnd, WindowLongIndexFlags nIndex, WinProc newProc) {
		return IntPtr.Size == 8 ? SetWindowLongPtr64(hWnd, nIndex, newProc) : new IntPtr(SetWindowLong32(hWnd, nIndex, newProc));
	}

	private struct POINT {
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	private struct MINMAXINFO {
		public POINT ptReserved;
		public POINT ptMaxSize;
		public POINT ptMaxPosition;
		public POINT ptMinTrackSize;
		public POINT ptMaxTrackSize;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct COPYDATASTRUCT {
		public IntPtr dwData; // 用户定义数据
		public int cbData; // 数据大小
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpData; // 指向数据的指针
	}

	[Flags]
	private enum WindowLongIndexFlags : int {
		GWL_WNDPROC = -4,
	}

	private enum WindowMessage : int {
		WM_GETMINMAXINFO = 0x0024,
		WM_COPYDATA = 0x004A, // 当一个应用程序传递数据给另一个应用程序时发送此消息
	}

	public delegate void ReceivedType(string text);
	public event ReceivedType Received;
}
