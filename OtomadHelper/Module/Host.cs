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

using ScriptPortal.Vegas;

using BackdropWindow = OtomadHelper.WPF.Controls.BackdropWindow;
using ContextMenu = OtomadHelper.Models.ContextMenu;

namespace OtomadHelper.Module;

public sealed partial class Host : UserControl {
#if VEGAS_ENV
	internal Dockable Dockable { get; }
	private Keybindings Keybindings => Dockable.Module.Keybindings;

	public Host(Dockable dockable) {
		Dockable = dockable;
		Dockable.Closed += Dockable_Closed;
		Dockable.VisibleChanged += Dockable_VisibleChanged;
#else
	public Host(object @null) {
#endif
		InitializeComponent();
		Dock = DockStyle.Fill;
		CheckForIllegalCrossThreadCalls = false;

		DragDrop += (_, _) => Host_DragLeave(isDrop: true);
		DragLeave += (_, _) => Host_DragLeave(isDrop: false);

		SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
		SystemCursorConfig.CursorChanged += (_, _) => PostSystemConfigToTheWeb();

		BackColor = SkinColors.Current.Background;
		ForeColor = SkinColors.Current.Foreground;
		SplashOverflowMenuButton.ForeColor = SkinColors.Current.Foreground;
		SplashOverflowMenuButton.FlatAppearance.BorderColor = SkinColors.Current.Background;
		SplashOverflowMenuButton.FlatAppearance.MouseOverBackColor = SkinColors.Current.ButtonNormal;
		SplashOverflowMenuButton.FlatAppearance.MouseDownBackColor = SkinColors.Current.ButtonPressed;

		InitLoadingAnimation();
		//CoreWebView2_LoadEnvironment();
	}

	private async void CoreWebView2_LoadEnvironment() {
		CoreWebView2EnvironmentOptions options = new("--enable-features=OverlayScrollbar,msEdgeFluentOverlayScrollbar,msOverlayScrollbarWinStyleAnimation --disable-web-security"); // msWebView2BrowserHitTransparent
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

	private async void Browser_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e) {
		CoreWebView2 webView = Browser.CoreWebView2;
		await webView.AddScriptToExecuteOnDocumentCreatedAsync("initialSystemConfig = " + SerializeWebMessageJson(GetRefreshedSystemConfig()));
		ManagedStream.Handler(Browser);
		Browser.Source = new(ManagedStream.RESOURCE_HOST + "index.html"); // "http://www.sunchateau.com/free/ua.htm"
		webView.NewWindowRequested += CoreWebView2_NewWindowRequested;
		webView.DocumentTitleChanged += (_, _) => DocumentTitleChanged?.Invoke(Browser.CoreWebView2.DocumentTitle);
		webView.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
		webView.ScriptDialogOpening += CoreWebView2_ScriptDialogOpening;
		webView.AddHostObjectToScript("bridge", new BetterBridge(new Bridge()));
		WebMessageAcknowledgement webMessageAcknowledgement = new();
		webView.AddHostObjectToScript("webMessageAcknowledgement", webMessageAcknowledgement);
		//Browser.ZoomFactor // TODO: Set UI zoom factor.
		MessageSender.Host = this;
		webMessageAcknowledgement.Received += OnReceiveAcknowledgement;
//#if DEBUG
		webView.OpenDevToolsWindow(); // WARN: Comment this line when released!
//#endif
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
	}

	private void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e) {
		try {
			string message = e.TryGetWebMessageAsString();
			switch (message) {
				case "initialized":
					//PostSystemConfigToTheWeb();
					//await Task.Delay(500);
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
		string[] files = e.FileNames;
		if (files.Length < 1) return;
		string fullPath = files[0];
		Path path = new(fullPath);
		e.Effect = e.AllowedEffect & DragDropEffects.Copy;
		DropTargetHelper.DragEnter(this, e.Data, new Point(e.X, e.Y), e.Effect, t.Host.ToolTip.ImportToHere, path.FileName);
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
		DropTargetHelper.DragOver(new(e.X, e.Y), e.Effect);
	}

	internal bool isDevMode = false;
	/// <summary>
	/// Is the page focused?
	/// </summary>
	internal new bool Focused { get; set { field = value; NotifyBgTrackingChanged(); } }
	/// <summary>
	/// Is the dockable host not focused but visible?
	/// </summary>
	/// <remarks>
	/// At this moment, it would enable background tracking.
	/// </remarks>
	public bool BgTracking => !Focused &&
#if VEGAS_ENV
		Dockable.Visible;
#else
		true;
#endif
	private void NotifyBgTrackingChanged() {
		BgTrackingChanged?.Invoke(this, BgTracking); // TODO: Optional debounce.
	}
	public event EventHandler<bool>? BgTrackingChanged;

	private async void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e) {
		CoreWebView2 webView = Browser.CoreWebView2;
		IList<CoreWebView2ContextMenuItem> menuList = e.MenuItems;
		CoreWebView2Deferral deferral = e.GetDeferral();
		// `e.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText` won't work
		// if doesn't select any text, so we check if the menu list includes the "paste" option, and if so,
		// it indicates that the target is a text box.
		bool isTextBox = menuList.FirstOrDefault(item => item.Name == "paste") is { };
		if (isTextBox) {
			if (!isDevMode) RemoveMenuItems(
				names: ["emoji", "inspectElement", "share", "webCapture"],
				commandIds: [50221, 41120]
			);
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
				if (menuItem is { }) menuList.Remove(menuItem);
			}
			foreach (int commandId in commandIds) { // Some new menu items in Microsft Edge don't have a name, so we can use command id only.
				CoreWebView2ContextMenuItem? menuItem = menuList.FirstOrDefault(item => item.CommandId == commandId);
				if (menuItem is { }) menuList.Remove(menuItem);
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
				// TODO: svg icon won't work, but png will work. See: https://github.com/MicrosoftEdge/WebView2Feedback/issues/4827
				switch (item.kind) {
					case CoreWebView2ContextMenuItemKind.Separator:
						break;
					case CoreWebView2ContextMenuItemKind.Submenu:
						menuItem.Children.AddRange(CreateContextMenuItems(item.items ?? [], menuUuid));
						break;
					default:
						menuItem.CustomItemSelected += (_, _) =>
							PostWebMessage(new ContextMenuItemClickEventArgs(menuUuid, item.uuid));
						break;
				}
				menuItem.IsEnabled = item.enabled;
				menuItem.IsChecked = item.@checked;
				return menuItem;
			}).ToList();
	}

	public Rect ClientToScreenRect(Rect clientRect) {
		Point location = PointToScreen(Point.Empty);
		(double dpiX, double dpiY) = this.Dpi;
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
		PostSystemConfigToTheWeb();
	}

	private SystemConfig GetRefreshedSystemConfig() {
		SystemConfig config = new();
		BackdropWindow.GetWindowsAccentPalette(config);
		SystemCursorConfig.SetSystemConfig(config);
		return config;
	}

	private void PostSystemConfigToTheWeb() => PostWebMessage(GetRefreshedSystemConfig());

	private System.Windows.Controls.ContextMenu SplashOverflowMenu {
		get {
			if (field is null) {
				field = BackdropWindow.CreateContextMenu(out bool themedSuccessfully);
				field.Placement = System.Windows.Controls.Primitives.PlacementMode.AbsolutePoint;
				AddMenuItem("Otomad Helper " + OtomadHelperVersionTag, icon: field.FindResource("Icon:OtomadHelper_24"), disabled: true);
				AddMenuItem("VEGAS Pro " + VegasProVersionTag, icon: "VegasPro", disabled: true);
				field.Items.Add(new System.Windows.Controls.Separator());
				AddMenuItem(t.SplashOverflowMenu.RepositoryLink, icon: "GitHub", link: "https://github.com/otomad/OtomadHelper");
				AddMenuItem(t.SplashOverflowMenu.GetLatestVersion, icon: "ArrowDownload", link: "https://github.com/otomad/OtomadHelper/releases/latest");
				AddMenuItem(t.SplashOverflowMenu.Troubleshooting, icon: "WrenchScrewdriver");
				AddMenuItem(t.SplashOverflowMenu.Feedback, icon: "TextEdit", link: "https://github.com/otomad/OtomadHelper/issues");
				AddMenuItem(t.SplashOverflowMenu.Reset, icon: "ArrowReset", action: ResetAll);
				field.Opened += (_, _) => SplashOverflowMenuButton.BackColor = SkinColors.Current.ButtonNormal;
				field.Closed += (_, _) => SplashOverflowMenuButton.BackColor = Color.Transparent;

				WPF.Controls.Icon? GetIcon(string name) => themedSuccessfully ? field.FindResource(name) as WPF.Controls.IconTemplate : null;

				void AddMenuItem(string header, object? icon = null, string link = "", bool disabled = false, Action? action = null) {
					System.Windows.Controls.MenuItem menuItem = new() {
						Header = header,
						Icon = icon is null ? null : icon is string iconName ? GetIcon("Icon:" + iconName) : icon,
						IsEnabled = !disabled,
					};
					if (action is not null)
						menuItem.Click += (_, _) => action();
					else if (!string.IsNullOrEmpty(link))
						menuItem.Click += (_, _) => OpenLink(link);
					field.Items.Add(menuItem);
				}
			}
			return field;
		}
	}

	private void SplashOverflowMenuButton_Click(object sender, EventArgs e) {
		(double dpiX, double dpiY) = this.Dpi;
		Screen screen = Screen.FromControl(SplashOverflowMenuButton);
		Point location = SplashOverflowMenuButton.PointToScreen(new(0, 0));
		System.Windows.Controls.ContextMenu menu = SplashOverflowMenu;
		menu.IsOpen = true;
		menu.HorizontalOffset = (location.X + SplashOverflowMenuButton.Width) / dpiX;
		if (menu.HorizontalOffset <= screen.WorkingArea.Width / dpiX) menu.HorizontalOffset -= menu.ActualWidth;
		menu.VerticalOffset = (location.Y + SplashOverflowMenuButton.Height) / dpiY;
		if (menu.VerticalOffset + menu.ActualHeight > screen.WorkingArea.Height / dpiY) menu.VerticalOffset -= menu.ActualHeight + SplashOverflowMenuButton.Height / dpiY;
	}

	private async void ResetAll() {
		bool sure;
		try {
			sure = await WPF.Controls.ContentDialog.ShowDialog<bool?>(
				t.ResetConfig.Title,
				t.ResetConfig.Content,
				(IEnumerable<WPF.Controls.ContentDialogButtonItem>)[
					new(t.ContentDialog.Button.Ok, true),
					new(t.ContentDialog.Button.Cancel, false, true),
				],
				"Warning"
			) == true;
		} catch {
			sure = MessageBox.Show(
				t.ResetConfig.Content,
				t.ResetConfig.Title,
				MessageBoxButtons.OKCancel,
				MessageBoxIcon.Warning,
				MessageBoxDefaultButton.Button2
			) == DialogResult.OK;
		}
		if (!sure) return;
		await Browser.CoreWebView2.Profile.ClearBrowsingDataAsync();
#if VEGAS_ENV
		Dockable.Module.RestartDockView();
#else
		ParentForm?.Close();
#endif
	}

#if VEGAS_ENV
	private void AddModuleKeybindings() {
		Keybindings.TriggerKeybinding += Module_TriggerKeybinding;
		Keybindings.Enabled = true;
	}

	private async void Module_TriggerKeybinding(object sender, VegasKeybindingEventArgs e) {
		if (!Dockable.Shown) return;
		PostWebMessage(new VegasCommandEvent { @event = e.Type });
		switch (e.Type) {
			case VegasCommandType.Reset:
				ResetAll();
				break;
			default:
				break;
		}
	}

	private void Dockable_Closed(object sender, EventArgs e) {
		Keybindings.TriggerKeybinding -= Module_TriggerKeybinding;
		Keybindings.Enabled = false;
	}

	private void Dockable_VisibleChanged(object sender, bool e) => NotifyBgTrackingChanged();
#endif
}
