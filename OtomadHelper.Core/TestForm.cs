using OtomadHelper.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtomadHelper.Core {
	public partial class TestForm : Form {
		private IntPtr appWin;

		public TestForm() {
			InitializeComponent();

			Process p = null;
			try {
				p = Process.Start(@"D:\Documents\source\repos\OtomadHelper\OtomadHelper.UI\bin\x64\Debug\net6.0-windows10.0.19041.0\win10-x64\OtomadHelper.UI.exe");
				p.WaitForInputIdle();
				System.Threading.Thread.Sleep(2000);
				appWin = p.MainWindowHandle;
				Debug.WriteLine(p.MainWindowHandle);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, "Error");
			}

			Win32API.SetParent(appWin, panel.Handle);
			Win32API.SetWindowLong(new HandleRef(panel, appWin), Win32API.GWL_STYLE, Win32API.WS_VISIBLE);
			Win32API.MoveWindow(appWin, 0, 0, panel.Width, panel.Height, true);
		}

		protected override void OnResize(EventArgs e) {
			if (appWin != IntPtr.Zero) {
				Win32API.MoveWindow(appWin, 0, 0, panel.Width, panel.Height, true);
			}
			base.OnResize(e);
		}

		protected override void OnHandleDestroyed(EventArgs e) {
			if (appWin != IntPtr.Zero) {
				Win32API.PostMessage(appWin, Win32API.WM_CLOSE, 0, 0);
				System.Threading.Thread.Sleep(1000);
				appWin = IntPtr.Zero;
			}
			base.OnHandleDestroyed(e);
		}
	}
}
