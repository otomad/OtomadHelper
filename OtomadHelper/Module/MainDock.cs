using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtomadHelper.Module {
	public partial class MainDock : UserControl {
		public MainDock() {
			InitializeComponent();
			Dock = DockStyle.Fill;
			Browser.EnsureCoreWebView2Async();
		}

		private void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
			ManagedStream.Handler(Browser);
			Browser.Source = new Uri("http://app/index.html"); // "http://www.sunchateau.com/free/ua.htm"
		}
	}
}
