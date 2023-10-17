using CefSharp;
using CefSharp.WinForms;
//using OtomadHelper.Telemetry;
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
		public ChromiumWebBrowser Browser;

		public MainDock() {
			InitializeComponent();
			Dock = DockStyle.Fill;
		}

		public void CreateBrowser() {
			//参数设置
			CefSettings settings = new CefSettings {
				Locale = "zh-CN",
			};
			//settings.CefCommandLineArgs.Add("disable-gpu", "1");//去掉gpu，否则chrome显示有问题
			//settings.RegisterScheme(new CefCustomScheme() {
			//	SchemeName = ResourceSchemeHandlerFactory.SchemeName,
			//	SchemeHandlerFactory = new ResourceSchemeHandlerFactory()
			//});
			Cef.Initialize(settings/*, performDependencyCheck: true, browserProcessHandler: null*/);
			//创建实例
			Browser = new ChromiumWebBrowser("https://www.baidu.com") { // "resource://web/dist/index.html"
				// 无论出于何种原因，请求总是以小写形式返回（无论您的地址大小写如何），因此您需要确保所有文件和文件夹都是小写形式。
				Dock = DockStyle.Fill,
			};
			// 将浏览器放入容器中
			Controls.Add(Browser);
		}

		private void MainDock_Load(object sender, EventArgs e) {
			CreateBrowser();
		}
	}
}
