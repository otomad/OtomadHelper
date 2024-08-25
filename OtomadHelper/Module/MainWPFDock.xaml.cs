using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using OtomadHelper.Bridges;
using OtomadHelper.Helpers.WebView2BetterBridge;

namespace OtomadHelper.Module;

/// <summary>
/// MainWPFDock.xaml 的交互逻辑
/// </summary>
public partial class MainWPFDock : UserControl {
	public MainWPFDock() {
		InitializeComponent();

		//Stream fileStream = ResourceHelper.GetEmbeddedResource("Assets.LoadingAnimation.apng");
		//LoadingAnimationPicture.Source = new BitmapImage() { StreamSource = fileStream };

		//PostWebMessage_SetWebView2 = Browser;
		CoreWebView2_LoadEnvironment();
	}

	private async void CoreWebView2_LoadEnvironment() {
		CoreWebView2EnvironmentOptions options = new("--enable-features=OverlayScrollbar,msEdgeFluentOverlayScrollbar,msOverlayScrollbarWinStyleAnimation,msWebView2BrowserHitTransparent");
		CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
		await Browser.EnsureCoreWebView2Async(environment);
		CoreWebView2Settings settings = Browser.CoreWebView2.Settings;
		settings.AreBrowserAcceleratorKeysEnabled = false;
		//settings.AreDefaultContextMenusEnabled = false;
		//settings.AreDefaultScriptDialogsEnabled = false;
		//settings.AreDevToolsEnabled = false;
		settings.IsBuiltInErrorPageEnabled = false;
		settings.IsGeneralAutofillEnabled = false;
		settings.IsPasswordAutosaveEnabled = false;
		settings.IsPinchZoomEnabled = false;
		settings.IsReputationCheckingRequired = false;
		settings.IsStatusBarEnabled = false;
		settings.IsSwipeNavigationEnabled = false;
		//settings.IsZoomControlEnabled = false;
	}

	private void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
		//ManagedStream.Handler(Browser);sssssssssssssssssssssssssssssssssssssss
		Browser.Source = new Uri(ManagedStream.RESOURCE_HOST + "index.html"); // "http://www.sunchateau.com/free/ua.htm"
		Browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
		Browser.CoreWebView2.DocumentTitleChanged += (sender, e) => DocumentTitleChanged?.Invoke(Browser.CoreWebView2.DocumentTitle);
		Browser.CoreWebView2.AddHostObjectToScript("bridge", new BetterBridge(new Bridge()));
#if DEBUG
		//Browser.CoreWebView2.OpenDevToolsWindow();
#endif
	}

	private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e) {
		e.Handled = true;
		Process.Start(e.Uri);
	}

	private async void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) {
		string message = e.TryGetWebMessageAsString();
		if (message == "initialized") {
			await Task.Delay(500);
			LoadingAnimationPicture.Visibility = Visibility.Collapsed;
			//LoadingAnimationPicture.Stop();
			//SplashContainer.Visible = false;
			Browser.Visibility = Visibility.Visible;
		}
	}

	public delegate void DocumentTitleChangedEventHandler(string title);
	public event DocumentTitleChangedEventHandler? DocumentTitleChanged;
}
