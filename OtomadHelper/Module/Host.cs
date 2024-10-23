using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;

using APNGLib;

using Microsoft.Web.WebView2.Core;

using OtomadHelper.Bridges;
using OtomadHelper.Helpers.WebView2BetterBridge;
using OtomadHelper.Models;
using OtomadHelper.Services;

using ScriptPortal.MediaSoftware.Skins;
using ScriptPortal.Vegas;

using BackdropWindow = OtomadHelper.WPF.Controls.BackdropWindow;
using ContextMenu = OtomadHelper.Models.ContextMenu;
using Timer = System.Windows.Forms.Timer;

namespace OtomadHelper.Module;

public partial class Host : UserControl {
#if VEGAS_ENV
	internal Dockable Dockable { get; }
	private Keybindings Keybindings => Dockable.Module.Keybindings;

	public Host(Dockable dockable) {
		Dockable = dockable;
		Dockable.Closed += Dockable_Closed;
#else
	public Host(object @null) {
#endif
		InitializeComponent();
		Dock = DockStyle.Fill;
		CheckForIllegalCrossThreadCalls = false;

		DragDrop += (sender, e) => Host_DragLeave(isDrop: true);
		DragLeave += (sender, e) => Host_DragLeave(isDrop: false);

		SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

		//MainWindow window = new();
		//new TestControls().Show();
		//window.Show();
		/*_ = WPF.Controls.ContentDialog.ShowDialog<string>("幸福倒计时", "即将升级 Windows 到最新版本！", [
			new("草", DialogResult.Abort),
			new("走", DialogResult.Retry),
			new("忽略", DialogResult.Ignore, true),
		]);*/

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
		MessageSender.Host = this;
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

	private void WebInitialized() {
		LoadingAnimationPicture.Visible = false;
		LoadingAnimationPicture.Stop();
		SplashContainer.Visible = false;
		Browser.Visible = true;
#if VEGAS_ENV
		AddModuleKeybindings();
#endif
		StartUpdateFps();
	}

	private async void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) {
		try {
			string message = e.TryGetWebMessageAsString();
			switch (message) {
				case "initialized":
					PostAccentColorToTheWeb();
					await Task.Delay(500);
					WebInitialized();
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

	private void Host_DragEnter(object sender, DragEventArgs e) {
		if (LoadingAnimationPicture.Visible) return; // The animation should not respond to drag events when initializing it.
		string[] files = e.GetFileNames();
		if (files.Length < 1) return;
		string fullPath = files[0];
		Path path = new(fullPath);
		e.Effect = e.AllowedEffect & DragDropEffects.Copy;
		DropTargetHelper.DragEnter(this, e.Data, new Point(e.X, e.Y), e.Effect, t.Host.ToolTip.ImportToHere, path.FullFileName);
		bool isDirectory = path.IsDirectory;
		string extension = path.DotExtension;
		using RegistryKey? registryKey = Registry.ClassesRoot.OpenSubKey(extension);
		string contentType = (string)(registryKey?.GetValue("Content Type") ?? "");
		PostWebMessage(new DragOver {
			extension = extension,
			contentType = contentType,
			isDirectory = isDirectory,
			isDragging = true,
		});
	}

	private void Host_DragLeave(bool isDrop) {
		if (LoadingAnimationPicture.Visible) return;
		PostWebMessage(new DragOver { isDragging = isDrop ? false : null });
		DropTargetHelper.DragLeave(this);
	}

	private void Host_DragOver(object sender, DragEventArgs e) {
		if (LoadingAnimationPicture.Visible) return;
		e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? e.AllowedEffect & DragDropEffects.Copy : DragDropEffects.None;
		DropTargetHelper.DragOver(new Point(e.X, e.Y), e.Effect);
	}

	internal bool isDevMode = false;

	private async void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e) {
		CoreWebView2 webView = Browser.CoreWebView2;
		IList<CoreWebView2ContextMenuItem> menuList = e.MenuItems;
		CoreWebView2Deferral deferral = e.GetDeferral();
		// `e.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText` won't work
		// if doesn't select any text, so we check if the menu list includes the "paste" option, and if so,
		// it indicates that the target is a text box.
		bool isTextBox = menuList.FirstOrDefault(item => item.Name == "paste") is not null;
		if (isTextBox) {
			if (!isDevMode) RemoveMenuItems(["emoji", "inspectElement", "share", "webCapture"], [50221, 41120]);
		} else {
			if (!isDevMode) RemoveAllMenuItems();
			string contextMenuJson = await webView.ExecuteScriptAsync("window.contextMenu");
			if (contextMenuJson == "null") goto Complete;
			ContextMenu contextMenu;
			try {
				contextMenu = ParseJson<ContextMenu>(contextMenuJson);
			} catch (JsonException) {
				goto Complete;
			}
			List<CoreWebView2ContextMenuItem> menuItems = CreateContextMenuItems(contextMenu.items, contextMenu.uuid);
			if (isDevMode) menuItems.Add(webView.Environment.CreateContextMenuItem("", null, CoreWebView2ContextMenuItemKind.Separator));
			foreach ((CoreWebView2ContextMenuItem item, int index) in menuItems.WithIndex())
				menuList.Insert(index, item);
		}
		RemoveIrrationalSeparators();

	Complete:
		deferral.Complete();

		void RemoveMenuItems(string[] names, int[] commandIds) {
			foreach (string name in names) {
				CoreWebView2ContextMenuItem? menuItem = menuList.FirstOrDefault(item => item.Name == name);
				if (menuItem is not null) menuList.Remove(menuItem);
			}
			foreach (int commandId in commandIds) { // Some new menu items in Microsft Edge don't have a name, so we can use command id only.
				CoreWebView2ContextMenuItem? menuItem = menuList.FirstOrDefault(item => item.CommandId == commandId);
				if (menuItem is not null) menuList.Remove(menuItem);
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

		List<CoreWebView2ContextMenuItem> CreateContextMenuItems(ContextMenuItem[] items, string menuUuid) =>
			items.Select(item => {
				CoreWebView2ContextMenuItem menuItem = webView.Environment.CreateContextMenuItem(item.label, null, item.kind);
				// TODO: svg icon won't work, but png will work.
				switch (item.kind) {
					case CoreWebView2ContextMenuItemKind.Separator:
						break;
					case CoreWebView2ContextMenuItemKind.Submenu:
						menuItem.Children.AddRange(CreateContextMenuItems(item.items ?? [], menuUuid));
						break;
					default:
						menuItem.CustomItemSelected += (sender, e) =>
							PostWebMessage(new ContextMenuItemClickEventArgs(menuUuid, item.uuid));
						break;
				}
				return menuItem;
			}).ToList();
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
			[okBtn] :
			[okBtn, cancelBtn];
		CoreWebView2Deferral deferral = e.GetDeferral();
		bool? dialogResult = await WPF.Controls.ContentDialog.ShowDialog<bool?>(e.Message, "", buttons, iconName);
		if (dialogResult == true) e.Accept();
		deferral.Complete();
	}

	private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e) {
		if (e.Category != UserPreferenceCategory.General) return;
		PostAccentColorToTheWeb();
	}

	private void PostAccentColorToTheWeb() {
		AccentPalette? palette = BackdropWindow.GetAccentPalette();
		if (palette is not null) PostWebMessage(palette);
	}

#if VEGAS_ENV
	protected void AddModuleKeybindings() {
		Keybindings.TriggerKeybinding += Module_TriggerKeybinding;
		Keybindings.Enabled = true;
	}

	private void Module_TriggerKeybinding(object sender, VegasKeybindingEventArgs e) {
		if (!Dockable.Shown) return;
		PostWebMessage(new TriggerKeybinding { @event = e.Type });
	}

	protected void Dockable_Closed(object sender, EventArgs e) {
		Keybindings.TriggerKeybinding -= Module_TriggerKeybinding;
		Keybindings.Enabled = false;
		fpsTimer.Tick -= FpsTimer_Tick;
		fpsTimer.Stop();
	}
#endif

	private readonly Timer fpsTimer = new() { Interval = 100 };
	internal uint Fps { get; private set; } = 60;

	private void StartUpdateFps() {
		fpsTimer.Tick += FpsTimer_Tick;
		fpsTimer.Start();
	}

	private void FpsTimer_Tick(object sender, EventArgs e) {
		MonitorInfo? monitorInfo = GetMonitorInfo(Handle);
		if (monitorInfo.HasValue) {
			uint newFps = monitorInfo.Value.Frequency;
			if (newFps > 0 && newFps != Fps) {
				Fps = newFps;
				PostWebMessage(new FpsUpdated { fps = newFps });
			}
		}
	}
}