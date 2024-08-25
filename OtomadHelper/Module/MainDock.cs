using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

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
		CheckForIllegalCrossThreadCalls = false;

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

#if VEGAS_ENV
		BackColor = Skins.Colors.ButtonFace;
		ForeColor = Skins.Colors.ButtonText;
#endif
		InitLoadingAnimation();
		CoreWebView2_LoadEnvironment();
	}

	private async void CoreWebView2_LoadEnvironment() {
		CoreWebView2EnvironmentOptions options = new("--enable-features=OverlayScrollbar,msEdgeFluentOverlayScrollbar,msOverlayScrollbarWinStyleAnimation"); // msWebView2BrowserHitTransparent
		CoreWebView2Environment environment = await CoreWebView2Environment.CreateAsync(null, null, options);
		await Browser.EnsureCoreWebView2Async(environment);
		CoreWebView2Settings settings = Browser.CoreWebView2.Settings;
		settings.AreBrowserAcceleratorKeysEnabled = false;
		settings.AreDefaultScriptDialogsEnabled = false;
		//settings.AreDefaultContextMenusEnabled = false;
		//settings.AreDevToolsEnabled = false;
		//settings.IsZoomControlEnabled = false;
		settings.IsBuiltInErrorPageEnabled = false;
		settings.IsGeneralAutofillEnabled = false;
		settings.IsPasswordAutosaveEnabled = false;
		settings.IsPinchZoomEnabled = false;
		settings.IsReputationCheckingRequired = false;
		settings.IsStatusBarEnabled = false;
		settings.IsSwipeNavigationEnabled = false;
	}

	private void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
		ManagedStream.Handler(Browser);
		Browser.Source = new Uri(ManagedStream.RESOURCE_HOST + "index.html"); // "http://www.sunchateau.com/free/ua.htm"
		CoreWebView2 webView = Browser.CoreWebView2;
		webView.NewWindowRequested += CoreWebView2_NewWindowRequested;
		webView.DocumentTitleChanged += (sender, e) => DocumentTitleChanged?.Invoke(Browser.CoreWebView2.DocumentTitle);
		webView.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
		webView.ScriptDialogOpening += CoreWebView2_ScriptDialogOpening;
		webView.AddHostObjectToScript("bridge", new BetterBridge(new Bridge()));
		WebMessageAcknowledgement webMessageAcknowledgement = new();
		webView.AddHostObjectToScript("webMessageAcknowledgement", webMessageAcknowledgement);
		MessageSender.MainDock = this;
		webMessageAcknowledgement.Received += OnReceiveAcknowledgement;
#if DEBUG
		webView.OpenDevToolsWindow();
#endif
		RevokeWebView2DragDropSwallow(this);
	}

	private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e) {
		e.Handled = true;
		Process.Start(e.Uri);
	}

	private async void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) {
		try {
			string message = e.TryGetWebMessageAsString();
			switch (message) {
				case "initialized":
					await Task.Delay(500);
					LoadingAnimationPicture.Visible = false;
					LoadingAnimationPicture.Stop();
					SplashContainer.Visible = false;
					Browser.Visible = true;
					break;
				default:
					break;
			}
		} catch (ArgumentException) {
			string json = e.WebMessageAsJson;
			JsonNode? node = JsonNode.Parse(json);
			if (node?.GetValueKind() == JsonValueKind.Number) {
				
			}
			Debug.WriteLine(node);
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
		DropTargetHelper.DragEnter(this, e.Data, new Point(e.X, e.Y), e.Effect, t.MainDock.ToolTip.ImportToHere, path.FullFileName);
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
		if (LoadingAnimationPicture.Visible) return;
		PostWebMessage(new DragOver() {
			isDragging = false,
		});
		DropTargetHelper.DragLeave(this);
	}

	private void MainDock_DragOver(object sender, DragEventArgs e) {
		if (LoadingAnimationPicture.Visible) return;
		e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? e.AllowedEffect & DragDropEffects.Copy : DragDropEffects.None;
		DropTargetHelper.DragOver(new Point(e.X, e.Y), e.Effect);
	}

	internal bool isDevMode = false;

	private enum ContextMenuTarget {
		Ignore,
		TextBox,
		DeletableItem,
	}

	private async void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e) {
		CoreWebView2 webView = Browser.CoreWebView2;
		IList<CoreWebView2ContextMenuItem> menuList = e.MenuItems;
		// `e.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText` won't work if doesn't select any text.
		ContextMenuTarget target = ContextMenuTarget.Ignore;
		string deletedItem = "";
		CoreWebView2Deferral deferral = e.GetDeferral();
		HTMLElementAttributes[] attrs = await PostWebMessageAndGetResult<HTMLElementAttributes[]>(e.Location, "elementFromPoint");
		foreach (HTMLElementAttributes attr in attrs) {
			if (attr.tag == "input" && attr.type == "text" || attr.tag == "textarea") {
				target = ContextMenuTarget.TextBox;
				break;
			} else if (attr.data.ContainsKey("delete")) {
				target = ContextMenuTarget.DeletableItem;
				deletedItem = attr.data["delete"];
				break;
			}
		}
		switch (target) {
			case ContextMenuTarget.TextBox:
				if (!isDevMode) RemoveMenuItems("emoji", "inspectElement", "share", "webCapture");
				break;
			case ContextMenuTarget.DeletableItem:
				if (!isDevMode) RemoveAllMenuItems();
				CoreWebView2ContextMenuItem deleteItem = webView.Environment.CreateContextMenuItem(
				t.CoreWebView.MenuItem.Delete, null, CoreWebView2ContextMenuItemKind.Command);
				CoreWebView2ContextMenuItem separator = webView.Environment.CreateContextMenuItem(
					"", null, CoreWebView2ContextMenuItemKind.Separator);
				deleteItem.CustomItemSelected += (sender, ex) => {
					Debug.WriteLine(deletedItem);
				};
				menuList.Insert(0, deleteItem);
				if (isDevMode) menuList.Insert(1, separator);
				break;
			case ContextMenuTarget.Ignore:
			default:
				goto Complete;
		}
		RemoveIrrationalSeparators();

	Complete:
		deferral.Complete();

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

	public Rect ClientToScreenRect(Rect clientRect) {
		Point location = PointToScreen(Point.Empty);
		(double dpiX, double dpiY) = this.GetDpi();
		return new(
			x: location.X / dpiX + clientRect.X,
			y: location.Y / dpiY + clientRect.Y,
			width: clientRect.Width,
			height: clientRect.Height
		);
	}

	public Rect ClientToScreenRect(Tuple<double, double, double, double> clientRect) {
		(double x, double y, double width, double height) = clientRect;
		return ClientToScreenRect(new Rect(x, y, width, height));
	}

	public void ShowFlyout(System.Windows.Window flyout) {
		try {
			new WindowInteropHelper(flyout).Owner = Handle;
			ElementHost.EnableModelessKeyboardInterop(flyout);
			flyout.Show(); // ShowDialog will prevent WndProc in the dock.
		} catch (Exception) { }
	}

	private async void CoreWebView2_ScriptDialogOpening(object sender, CoreWebView2ScriptDialogOpeningEventArgs e) {
		if (e.Kind == CoreWebView2ScriptDialogKind.Prompt) return;
		string iconName = e.Kind switch {
			CoreWebView2ScriptDialogKind.Confirm => "Question",
			CoreWebView2ScriptDialogKind.Beforeunload => "Warning",
			_ => "Info",
		};
		WPF.Controls.ContentDialogButtonItem<bool>
			okBtn = new(t.ContentDialog.Button.Ok, true, true),
			cancelBtn = new(t.ContentDialog.Button.Cancel, false);
		WPF.Controls.ContentDialogButtonItem<bool>[] buttons = e.Kind == CoreWebView2ScriptDialogKind.Alert ?
			new[] { okBtn } :
			new[] { okBtn, cancelBtn };
		CoreWebView2Deferral deferral = e.GetDeferral();
		bool? dialogResult = await WPF.Controls.ContentDialog.ShowDialog<bool?>(e.Message, "", buttons, iconName);
		if (dialogResult == true) e.Accept();
		deferral.Complete();
	}
}
