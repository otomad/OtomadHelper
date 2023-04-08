using OtomadHelper.Core.Communication;
using System.Windows;

namespace OtomadHelper.Core {
	/// <summary>
	/// TestWindow.xaml 的交互逻辑
	/// </summary>
	public partial class TestWindow : Window {
		internal PipeServer server;

		public TestWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			server = new PipeServer();
			server.ServerReceived += text => Dispatcher.Invoke(() => (Content as MainDock).ReceivedLbl.Text = text);
		}

		private void Window_Closed(object sender, System.EventArgs e) {
			if (server != null)
				server.Close();
		}
	}
}
