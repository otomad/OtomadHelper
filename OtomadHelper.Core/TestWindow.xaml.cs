using OtomadHelper.Core.Helpers;
using System;
using System.Windows;
using System.Windows.Interop;

namespace OtomadHelper.Core {
	/// <summary>
	/// TestWindow.xaml 的交互逻辑
	/// </summary>
	public partial class TestWindow : Window {
		internal readonly WindowHelper windowHelper;

		public TestWindow() {
			InitializeComponent();
			windowHelper = new WindowHelper(this);
			windowHelper.Received += text => {
				MainDock dock = MainDock.Instance;
				if (dock != null) dock.Received = text;
			};

			TestForm form = new TestForm();
			form.ShowDialog();
			Close();
		}

		protected override void OnSourceInitialized(EventArgs e) {
			base.OnSourceInitialized(e);
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			if (hwndSource != null) {
				IntPtr handle = hwndSource.Handle;
				hwndSource.AddHook(new HwndSourceHook(windowHelper.WndProc));
			}
		}

		internal void Send(string text) {
			windowHelper.SendMessage(text);
		}

		private void Window_Closed(object sender, EventArgs e) {
			windowHelper.Close();
		}
	}
}
