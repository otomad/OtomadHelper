using System.Drawing;
using System.Text.Json.Nodes;
using System.Windows.Forms;

using APNGLib;

using Microsoft.Web.WebView2.Core;

using OtomadHelper.Bridges;
using OtomadHelper.Helpers.WebView2BetterBridge;
using OtomadHelper.Models;
using OtomadHelper.Test;

using ScriptPortal.MediaSoftware.Skins;

namespace OtomadHelper.Module;

public partial class MainDock : UserControl {
	public MainDock() {
		InitializeComponent();
		Dock = DockStyle.Fill;

		DragDrop += (sender, e) => MainDock_DragLeave();
		DragLeave += (sender, e) => MainDock_DragLeave();

		//MainWindow window = new();
		//new TestControls().Show();
		//window.Show();
		/*new ContentDialog("幸福倒计时", "即将升级 Windows 到最新版本！", new ContentDialogButtonItem[] {
			new("草", DialogResult.Abort),
			new("走", DialogResult.Retry),
			new("忽略", DialogResult.Ignore, true),
		}).ShowDialog();*/
		Browser.Capture = true;

#if VEGAS_ENV
		BackColor = Skins.Colors.ButtonFace;
		ForeColor = Skins.Colors.ButtonText;
#endif
		InitLoadingAnimation();
		Host = this;
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
		Browser.Source = new Uri(ManagedStream.RESOURCE_HOST + "index.html"); // "http://www.sunchateau.com/free/ua.htm"
		Browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
		Browser.CoreWebView2.DocumentTitleChanged += (sender, e) => DocumentTitleChanged?.Invoke(Browser.CoreWebView2.DocumentTitle);
		Browser.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
		Browser.CoreWebView2.AddHostObjectToScript("bridge", new BetterBridge(new Bridge()));
		Browser.CoreWebView2.AddHostObjectToScript("postMessageToHost", postMessageToHost);
#if DEBUG
		Browser.CoreWebView2.OpenDevToolsWindow();
#endif
	}
	private readonly PostMessageToHost postMessageToHost = new();

	private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e) {
		e.Handled = true;
		Process.Start(e.Uri);
	}

	private async void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) {
		string message = e.TryGetWebMessageAsString();
		if (message == "initialized") {
			await Task.Delay(500);
			LoadingAnimationPicture.Visible = false;
			LoadingAnimationPicture.Stop();
			SplashContainer.Visible = false;
			Browser.Visible = true;
		}
	}

	private void InitLoadingAnimation() {
		try {
			Stream fileStream = ResourceHelper.GetEmbeddedResource("Assets.LoadingAnimation.apng");
			APNG apng = new(fileStream);
			LoadingAnimationPicture.APNGFile = apng;
			LoadingAnimationPicture.CenterToParent(this);
			SplashContainer.Controls.Add(LoadingAnimationPicture);
			LoadingAnimationPicture.Start();
		} catch (Exception) { }
	}

	public delegate void DocumentTitleChangedEventHandler(string title);
	public event DocumentTitleChangedEventHandler? DocumentTitleChanged;

	private void MainDock_DragEnter(object sender, DragEventArgs e) {
		if (LoadingAnimationPicture.Visible) return; // The animation should not respond to drag events when initializing it.
		string[] files = e.GetFileNames();
		if (files.Length < 1) return;
		string fullPath = files[0];
		Path path = new(fullPath);
		e.Effect = e.AllowedEffect & DragDropEffects.Copy;
		DropTargetHelper.DragEnter(this, e.Data, new Point(e.X, e.Y), e.Effect, t.ImportToHere, path.FullFileName);
		bool isDirectory = path.IsDirectory;
		string extension = path.DotExtension;
		using RegistryKey? registryKey = Registry.ClassesRoot.OpenSubKey(extension);
		string contentType = (string)(registryKey?.GetValue("Content Type") ?? "");
		PostWebMessage(new DragOver() {
			extension = extension,
			contentType = contentType,
			isDirectory = isDirectory,
			isDragging = true,
		});
	}

	private void MainDock_DragLeave() {
		PostWebMessage(new DragOver() {
			isDragging = false,
		});
		DropTargetHelper.DragLeave(this);
	}

	private void MainDock_DragOver(object sender, DragEventArgs e) {
		e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? e.AllowedEffect & DragDropEffects.Copy : DragDropEffects.None;
		DropTargetHelper.DragOver(new Point(e.X, e.Y), e.Effect);
	}

	internal bool isDevMode = false;
	private void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e) {
		CoreWebView2 webView = Browser.CoreWebView2;
		IList<CoreWebView2ContextMenuItem> menuList = e.MenuItems;
		// `e.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText` won't work
		// if doesn't select any text, so we check if the menu list includes the "paste" option, and if so,
		// it indicates that the target is a text box.
		bool isTextBox = menuList.FirstOrDefault(item => item.Name == "paste") is not null;
		if (isTextBox) {
			if (!isDevMode) RemoveMenuItems("emoji", "inspectElement", "share", "webCapture");
		} else {
			if (!isDevMode) RemoveAllMenuItems();
			CoreWebView2ContextMenuItem deleteItem = webView.Environment.CreateContextMenuItem(
			"Delete", null, CoreWebView2ContextMenuItemKind.Command);
			CoreWebView2ContextMenuItem separator = webView.Environment.CreateContextMenuItem(
				"", null, CoreWebView2ContextMenuItemKind.Separator);
			deleteItem.CustomItemSelected += (sender, ex) => {
				Point location = e.Location;
				Debug.WriteLine(location);
			};
			menuList.Insert(0, deleteItem);
			if (isDevMode)
				menuList.Insert(1, separator);
		}
		RemoveIrrationalSeparators();

		void RemoveMenuItems(params string[] names) {
			foreach (string name in names) {
				CoreWebView2ContextMenuItem? menuItem = menuList.FirstOrDefault(item => item.Name == name);
				if (menuItem is not null)
					menuList.Remove(menuItem);
			}
		}

		void RemoveAllMenuItems() {
			foreach (CoreWebView2ContextMenuItem menuItem in menuList.ToList())
				menuList.Remove(menuItem);
		}

		void RemoveIrrationalSeparators() {
			for (int i = menuList.Count - 1; i >= 0; i--) {
				CoreWebView2ContextMenuItem menuItem = menuList[i];
				if (menuItem.Kind == CoreWebView2ContextMenuItemKind.Separator)
					if (i == menuList.Count - 1 || i == 0 ||
						menuList[i + 1].Kind == CoreWebView2ContextMenuItemKind.Separator)
						menuList.RemoveAt(i);
			}
		}
	}
}
