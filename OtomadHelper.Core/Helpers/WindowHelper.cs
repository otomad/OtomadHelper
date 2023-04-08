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
		private readonly Window window;

		internal WindowHelper(Window window) {
			this.window = window;
		}

		private bool isOpenedClient = false;
		public void OpenClient() {
			if (isOpenedClient) throw new Exception("isOpenedClient");
			isOpenedClient = true;
			pro = new Process();
			Debug.WriteLine(Handle);
			string arguments = string.Format("{0} {1}", "-OtomadHelper", Handle); // 可不传，也可传入多个参数，每个参数之间用空格隔开
			ProcessStartInfo proStartInfo = new ProcessStartInfo(@"D:\Documents\source\repos\OtomadHelper\OtomadHelper.UI\bin\x64\Debug\net6.0-windows10.0.19041.0\win10-x64\OtomadHelper.UI.exe", arguments); // exe 的路径
			pro.StartInfo = proStartInfo;
			pro.Start(); // 运行 exe
			/*new Thread(() => {
				pro.WaitForExit();
				Dispatcher.Invoke(Close);
			}).Start();*/
		}

		private void ServerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (!pro.HasExited) pro?.Kill(); // 结束进程
		}

		private void OnSourceInitialized() {
			HwndSource hwndSource = PresentationSource.FromVisual(window) as HwndSource;
			if (hwndSource != null) {
				IntPtr handle = hwndSource.Handle;
				hwndSource.AddHook(new HwndSourceHook(WndProc));
			}
		}

		[DllImport("User32.dll", EntryPoint = "SendMessage")]
		private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
		[DllImport("User32.dll", EntryPoint = "FindWindow")]
		private static extern int FindWindow(string lpClassName, string lpWindowName);
		const int WM_COPYDATA = 0x004A; // 当一个应用程序传递数据给另一个应用程序时发送此消息
		private struct COPYDATASTRUCT {
			public IntPtr dwData; // 用户定义数据
			public int cbData; // 数据大小
			[MarshalAs(UnmanagedType.LPStr)]
			public string lpData; //指向数据的指针
		}

		public void SendMessage(string data) {
			/*if (pro == null || pro.HasExited) {
				MessageBox.Show("客户端已关闭！");
				return;
			}*/
			byte[] sarr = Encoding.UTF8.GetBytes(data);
			int len = sarr.Length;
			COPYDATASTRUCT cds;
			cds.dwData = (IntPtr)Convert.ToInt16(1); // 可以是任意值
			cds.cbData = len + 1; // 指定lpData内存区域的字节数
			cds.lpData = data; // 发送给目标窗口所在进程的数据
			SendMessage((int)pro.MainWindowHandle, WM_COPYDATA, 0, ref cds); // 发送消息
		}

		internal IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
			handled = false;
			// if message is coming from simconnect and the connection is not null;
			// continue and receive message
			if (msg == WM_COPYDATA) {
				COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT)); // 接收封装的消息
				//以下为逻辑处理
				string strResult = cds.lpData;
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
