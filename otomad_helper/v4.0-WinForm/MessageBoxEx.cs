using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public class MessageBoxEx {
		//测试样例
		protected static void Test() {
			Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[] { "按钮一(&O)", "按钮二(&T)", "按钮三(&H)" });
		}
		/// <summary>
		/// 测试样例<br />
		/// <code>Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[] { "按钮一(＆O)", "按钮二(＆T)", "按钮三(＆H)" });</code>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <param name="buttonTitles"></param>
		/// <returns></returns>
		public static DialogResult Show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, string[] buttonTitles = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
			if (buttonTitles == null) return MessageBox.Show(text, caption, buttons, icon, defaultButton);
			DummyForm frm = new DummyForm(buttons, buttonTitles);
			frm.Show();
			frm.WatchForActivate = true;
			DialogResult result = MessageBox.Show(frm, text, caption, buttons, icon, defaultButton);
			frm.Close();
			return result;
		}

		class DummyForm : Form {
			IntPtr _handle;
			MessageBoxButtons _buttons;
			string[] _buttonTitles = null;

			bool _watchForActivate = false;

			public bool WatchForActivate {
				get { return _watchForActivate; }
				set { _watchForActivate = value; }
			}

			public DummyForm(MessageBoxButtons buttons, string[] buttonTitles) {
				_buttons = buttons;
				_buttonTitles = buttonTitles;

				//让自己在界面上看不到
				this.Text = "";
				this.StartPosition = FormStartPosition.Manual;
				this.Location = new Point(-32000, -32000);
				this.ShowInTaskbar = false;
			}

			protected override void OnShown(EventArgs e) {
				base.OnShown(e);
				//把自己藏起来，在任务列表里也看不到
				NativeWin32API.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, 659);
			}

			protected override void WndProc(ref System.Windows.Forms.Message m) {
				if (_watchForActivate && m.Msg == 0x0006) {
					_watchForActivate = false;
					_handle = m.LParam;
					CheckMsgbox();
				}
				base.WndProc(ref m);
			}

			private void CheckMsgbox() {
				if (_buttonTitles == null || _buttonTitles.Length == 0)
					return;

				//按钮标题的索引
				int buttonTitleIndex = 0;
				//获取子控件的句柄
				IntPtr h = NativeWin32API.GetWindow(_handle, OCCommon.Message.GW_CHILD);
				while (h != IntPtr.Zero) {
					//按顺序把按钮标题赋上
					if (NativeWin32API.GetWindowClassName(h).Equals("Button")) {
						if (_buttonTitles.Length > buttonTitleIndex) {
							NativeWin32API.SetWindowText(h, _buttonTitles[buttonTitleIndex]);
							buttonTitleIndex++;
						}
					}
					h = NativeWin32API.GetWindow(h, OCCommon.Message.GW_HWNDNEXT);
				}
			}


			private static class OCCommon {
				/// <summary>
				/// Messages
				/// </summary>
				public static class Message {
					public const int GW_CHILD = 5;
					public const int GW_HWNDNEXT = 2;
				}
			}

			/// <summary>
			/// Win32 API
			/// </summary>
			private static class NativeWin32API {
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);
				[DllImport("user32.dll")]
				public static extern IntPtr GetWindow(IntPtr hWnd, long wCmd);
				[DllImport("user32.dll")]
				public static extern bool SetWindowText(IntPtr hWnd, string lpString);
				[DllImport("user32.dll")]
				public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

				public static string GetWindowClassName(IntPtr handle) {
					StringBuilder sb = new StringBuilder(256);

					GetClassNameW(handle, sb, sb.Capacity); //得到窗口类名并保存在strClass中
					return sb.ToString();
				}
			}
		}
	}
}
