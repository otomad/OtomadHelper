using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OtomadHelper.UI.Communication; 
internal class Client {
	internal Client() {

	}

	[DllImport("User32.dll", EntryPoint = "SendMessage")]
	private static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
	[DllImport("User32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Ansi)]
	private static extern int FindWindow(string lpClassName, string lpWindowName);
	const int WM_COPYDATA = 0x004A; // 当一个应用程序传递数据给另一个应用程序时发送此消息
	public struct COPYDATASTRUCT {
		public IntPtr dwData; // 用户定义数据
		public int cbData; // 数据大小
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpData; //指向数据的指针
	}

	/*protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);
		if (PresentationSource.FromVisual(this) is HwndSource hwndSource) {
			IntPtr handle = hwndSource.Handle;
			hwndSource.AddHook(new HwndSourceHook(WndProc));
		}
	}*/

	private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
		handled = false;
		// if message is coming from simconnect and the connection is not null;
		// continue and receive message
		if (msg == WM_COPYDATA) {
			COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam,  typeof(COPYDATASTRUCT)); // 接收封装的消息
			//以下为逻辑处理
			string strResult = cds.lpData;
			string strType = cds.dwData.ToString();
			if (strType == "1") {
				Info.Text = strResult;
			}
			handled = true;
		}
		return (IntPtr)0;
	}

	public void SendMessage(string data) {
		byte[] sarr = Encoding.Default.GetBytes(data);
		int len = sarr.Length;
		COPYDATASTRUCT cds;
		cds.dwData = (IntPtr)Convert.ToInt16(1);//可以是任意值
		cds.cbData = len + 1;//指定lpData内存区域的字节数
		cds.lpData = data;//发送给目标窗口所在进程的数据

		/*if (App.ServerHwnd == IntPtr.Zero) {
			var msgbox = new Wpf.Ui.Controls.MessageBox {
				Content = "找不到服务端！",
				ResizeMode = ResizeMode.NoResize,
			};
			msgbox.ShowDialog();
		}*/
		SendMessage(App.ServerHwnd.ToInt32(), WM_COPYDATA, 0, ref cds);
	}
}
