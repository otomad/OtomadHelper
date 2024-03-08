using Microsoft.Web.WebView2.Core;
using OtomadHelper.Helpers;
using OtomadHelper.Test;
using OtomadHelper.WPF.Controls;
using APNGLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScriptPortal.MediaSoftware.Skins;
using System.IO.Packaging;
using OtomadHelper.Bridges;
using Microsoft.Win32;
using OtomadHelper.Models;
using System.Net.Mime;

namespace OtomadHelper.Module;
public partial class MainDock : UserControl {
	public MainDock() {
		InitializeComponent();
		Dock = DockStyle.Fill;

		DragDrop += (sender, e) => Browser_DragLeave();
		DragLeave += (sender, e) => Browser_DragLeave();

		//MainWindow window = new();
		//window.Show();
		/*new ContentDialog("幸福倒计时", "即将升级 Windows 到最新版本！", new ContentDialogButtonItem[] {
			new("草", DialogResult.Abort),
			new("走", DialogResult.Retry),
			new("忽略", DialogResult.Ignore, true),
		}).ShowDialog();*/

#if VEGAS_ENV
		BackColor = Skins.Colors.ButtonFace;
		ForeColor = Skins.Colors.ButtonText;
#endif
		InitLoadingAnimation();
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
		ManagedStream.Handler(Browser);
		Browser.Source = new Uri("http://app/index.html"); // "http://www.sunchateau.com/free/ua.htm"
		Browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
		Browser.CoreWebView2.DocumentTitleChanged += (sender, e) => DocumentTitleChanged?.Invoke(Browser.CoreWebView2.DocumentTitle);
		Browser.CoreWebView2.AddHostObjectToScript("bridge", new Bridge());
	}

	private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e) {
		e.Handled = true;
		Process.Start(e.Uri);
	}

	private void Browser_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
		LoadingAnimationPicture.Visible = false;
		LoadingAnimationPicture.Stop();
		Browser.Visible = true;
	}

	private APNGBox LoadingAnimationPicture = null!;
	private void InitLoadingAnimation() {
		try {
			APNG apng = new();
			Stream fileStream = ResourceHelper.GetEmbeddedResource("Assets.LoadingAnimation.apng");
			apng.Load(fileStream);
			LoadingAnimationPicture = new APNGBox(apng) {
				Location = new Point((Width - (int)apng.Width) / 2, (Height - (int)apng.Height) / 2),
				Anchor = AnchorStyles.None,
			};
			Controls.Add(LoadingAnimationPicture);
			LoadingAnimationPicture.Start();
		} catch (Exception) { }
	}

	public delegate void DocumentTitleChangedEventHandler(string title);
	public event DocumentTitleChangedEventHandler? DocumentTitleChanged;

	private void Browser_DragEnter(object sender, DragEventArgs e) {
		if (LoadingAnimationPicture.Visible) return; // 初始化动画时不应响应拖拽事件。
		string[] filenames = e.GetFileNames();
		if (filenames.Length < 1) return;
		e.Effect = DragDropEffects.All;
		string filename = filenames[0];
		bool isDirectory = File.GetAttributes(filename).HasFlag(FileAttributes.Directory);
		string extension = Path.GetExtension(filename).ToLower();
		using RegistryKey? registryKey = Registry.ClassesRoot.OpenSubKey(extension);
		string contentType = (string)(registryKey?.GetValue("Content Type") ?? "");
		PostWebMessage(new DragOver() {
			Extension = extension,
			ContentType = contentType,
			IsDirectory = isDirectory,
			IsDragging = true,
		});
	}

	private void Browser_DragLeave() {
		PostWebMessage(new DragOver() {
			IsDragging = false,
		});
	}

	private readonly JsonSerializerOptions jsonOptions = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	public void PostWebMessage<T>(T message) where T : BaseWebMessageEvent {
		Browser.CoreWebView2.PostWebMessageAsJson(JsonSerializer.Serialize(message, jsonOptions));
	}
}
