using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace OtomadHelper.Core.Helpers {
	internal class WindowHelper {
		private Process pro;
		private IntPtr ClientHandle { get { return pro.MainWindowHandle; } }
		private readonly Window window;

		internal WindowHelper(Window window) {
			this.window = window;
		}

		private bool isOpenedClient = false;
		public void OpenClient() {
			if (isOpenedClient && pro != null) {
				SwitchToThisWindow(ClientHandle, true);
				return;
			}
			isOpenedClient = true;
			pro = new Process();
			Debug.WriteLine(Handle);
			string arguments = string.Format("{0} {1}", "-OtomadHelper", Handle); // 可不传，也可传入多个参数，每个参数之间用空格隔开
			ProcessStartInfo proStartInfo = new ProcessStartInfo(@"D:\Documents\source\repos\OtomadHelper\OtomadHelper.UI\bin\x64\Debug\net6.0-windows10.0.19041.0\win10-x64\OtomadHelper.UI.exe", arguments); // exe 的路径
			pro.StartInfo = proStartInfo;
			pro.Start(); // 运行 exe
			new Thread(() => {
				pro.WaitForExit();
				isOpenedClient = false;
				pro.Close();
				pro.Dispose();
				pro = null;
			}).Start();
		}

		public void Close() {
			if (pro != null && !pro.HasExited) pro?.Kill(); // 结束进程
		}

		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
		[DllImport("user32.dll", EntryPoint = "FindWindow")]
		private static extern int FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll ", SetLastError = true)]
		static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);
		const int WM_COPYDATA = 0x004A; // 当一个应用程序传递数据给另一个应用程序时发送此消息

		private struct COPYDATASTRUCT {
			public IntPtr dwData; // 用户定义数据
			public int cbData; // 数据大小
			[MarshalAs(UnmanagedType.LPStr)]
			public string lpData; // 指向数据的指针
		}

		public void SendMessage(string data) {
			if (pro == null || pro.HasExited) {
				Debug.WriteLine("客户端已关闭！");
				return;
			}
			data = Shared.Encodings.EncodeNonAscii(data);
			byte[] sarr = Encoding.Default.GetBytes(data);
			int len = sarr.Length;
			COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)Convert.ToInt16(1); // 可以是任意值
			cds.cbData = len + 1; // 指定 lpData 内存区域的字节数
			cds.lpData = data; // 发送给目标窗口所在进程的数据
			SendMessage((int)ClientHandle, WM_COPYDATA, 0, ref cds); // 发送消息
		}

		internal IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
			handled = false;
			// if message is coming from simconnect and the connection is not null;
			// continue and receive message
			if (msg == WM_COPYDATA) {
				COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT)); // 接收封装的消息
				// 以下为逻辑处理
				string strResult = Shared.Encodings.DecodeEncodedNonAscii(cds.lpData);
				string strType = cds.dwData.ToString();
				if (strType == "1") Received(strResult);
				handled = true;
			}
			return (IntPtr)0;
		}

		public IntPtr Handle { get { return new WindowInteropHelper(window).Handle; } }
		public delegate void ReceivedType(string text);
		public event ReceivedType Received;
	}
}
