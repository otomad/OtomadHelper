using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

using OtomadHelper.Interop;
using OtomadHelper.Models;

using ContextMenu = System.Windows.Controls.ContextMenu;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// BackdropWindow.xaml 的交互逻辑
/// </summary>
[DependencyProperty<SystemBackdropType>("SystemBackdropType", DefaultValueExpression = nameof(DefaultSystemBackdropType))]
[DependencyProperty<bool>("IsLightTheme", DefaultValue = true, IsReadOnly = true)]
[DependencyProperty<bool>("IsHighContrast", DefaultValue = false)]
[DependencyProperty<string>("CurrentThemeName", DefaultValue = "Aero", IsReadOnly = true)]
[DependencyProperty<Color?>("CustomAccentColor")]
[DependencyProperty<Color>("WindowGlassColor", DefaultValueExpression = nameof(WindowsDefaultGlassColor), IsReadOnly = true)]
[DependencyProperty<Brush>("WindowGlassBrush", DefaultValueExpression = nameof(WindowsDefaultGlassBrush), IsReadOnly = true)]
[DependencyProperty<TitleBarType>("TitleBarType", DefaultValueExpression = "TitleBarType.System")]
[DependencyProperty<FontFamily>("MonoFont")]
[DependencyProperty<FontFamily>("DefaultFont")]
//[DependencyProperty<bool>("IsNonClientActive")]
[DependencyProperty<bool?>("MinimizeBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.MinimizeBox" />""")]
[DependencyProperty<bool?>("MaximizeBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.MaximizeBox" />""")]
[DependencyProperty<bool?>("ControlBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.ControlBox" />""")]
[DependencyProperty<bool>("UseUniversalControlBox", DefaultValue = false)]
[RoutedEvent("Showing", RoutedEventStrategy.Bubble)]
public partial class BackdropWindow : Window {
	protected readonly WindowInteropHelper helper;
	protected nint Handle => helper.Handle;

	private bool IsRtl => false;

	public BackdropWindow() : base() {
		InitializeComponent();
		helper = new(this);
		if (IsRtl) FlowDirection = FlowDirection.RightToLeft;
	}

	public nint OwnerHandle {
		get => helper.Owner;
		set => helper.Owner = value;
	}

	private void InitializeComponent() {
		Background = DefaultBackground;
		RefreshCulture();
		CommandBindings.AddRange(Commands.CommandBindings);
		AddThemeResource(AddThemeResourceType.Common);
		SetResourceReference(IsHighContrastProperty, SystemParameters.HighContrastKey);
		UpdateCurrentThemeName();
		Loaded += Window_Loaded;
		Closed += Window_Closed;
		IsVisibleChanged += (_, e) => {
			if ((bool)e.NewValue) RaiseEvent(new RoutedEventArgs(ShowingEvent));
		};
		OnCultureChanged(Culture);
		CultureChanged += OnCultureChanged;

		// Border color (useless when system border color set)
		SetResourceReference(BorderBrushProperty, "CardStroke");
		// Looks weird in Windows Basic and Classic themes, so set the border thickness to 0.
		BorderThickness = new(0);

		// Debug focused element
		// Services.ITimer.WPF.Interval(() => s = System.Windows.Input.FocusManager.GetFocusedElement(this), 1000);
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		FixNonClientFrameEdgesMargin();
		if (ResizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize)
			ReserveSystemMenuItems(Handle, SystemMenuItemType.Move | SystemMenuItemType.Close);
		BindViewToViewModel();
		RefreshFrame();
		RefreshDarkMode();
		RefreshAccentColor();
		SetSystemBackdropType(SystemBackdropType);
		if (TitleBarType == TitleBarType.WindowChromeNoTitleBar)
			AddExtendedWindowStyles(Handle, ExtendedWindowStyles.ToolWindow);
		OnWindowAttributeSetting();
		// reference: https://www.cnblogs.com/code1992/p/11699416.html
		/*if (RegisterShellHookWindow(Handle))
			WM_ShellHook = RegisterWindowMessage("SHELLHOOK");*/
		if (WindowsVersion.Current < WindowsNT.Windows8) {
			SystemEvents.UserPreferenceChanged += OnSystemThemeChanged;
			//SystemParameters.StaticPropertyChanged += OnSystemThemeChanged;
		}
	}

	private void Window_Closed(object sender, EventArgs e) {
		//DeregisterShellHookWindow(Handle);
		if (WindowsVersion.Current < WindowsNT.Windows8) {
			SystemEvents.UserPreferenceChanged -= OnSystemThemeChanged;
			//SystemParameters.StaticPropertyChanged -= OnSystemThemeChanged;
		}
	}

	private void BindViewToViewModel() {
		if (DataContext is IViewAccessibleViewModel viewModel)
			viewModel.View = this;
	}

	public virtual void RefreshBindings() {
		object? viewModel = DataContext;
		DataContext = null;
		DataContext = viewModel;
	}

	public void MoveIntoScreen() {
		Screen screen = Screen.FromHandle(Handle);
		System.Drawing.Rectangle workingArea = screen.WorkingArea;
		(double dpiX, double dpiY) = this.Dpi;
		double maxLeft = workingArea.Right / dpiX - Width,
			maxTop = workingArea.Bottom / dpiY - Height,
			minLeft = workingArea.Left / dpiX,
			minTop = workingArea.Top / dpiY,
			screenWidth = workingArea.Width / dpiX,
			screenHeight = workingArea.Height / dpiY;
		if (Left > maxLeft) Left = maxLeft;
		if (Top > maxTop) Top = maxTop;
		if (Left < minLeft) Left = minLeft;
		if (Top < minTop) Top = minTop;
		MaxWidth = Math.Min(MaxWidth, screenWidth);
		MaxHeight = Math.Min(MaxHeight, screenHeight);
	}

	protected virtual void SetLocation(double left, double top) {
		Left = left;
		Top = top;
	}
	protected virtual void SetLocation(double left, double top, double width, SetWidthType widthType) {
		SetLocation(left, top);
		if ((widthType & SetWidthType.Width) != 0) Width = width;
		if ((widthType & SetWidthType.MinWidth) != 0) MinWidth = width;
		if ((widthType & SetWidthType.MaxWidth) != 0) MaxWidth = width;
	}
	protected virtual void SetLocation(Rect rect, SetWidthType widthType = SetWidthType.Nothing) =>
		SetLocation(rect.Left, rect.Top, rect.Width, widthType);

	protected internal Task<T> GetDialogResultTask<T>(Func<T> GetResult) {
		bool isGottenDialogResultTask = false;
		TaskCompletionSource<T> taskCompletionSource = new();
		Closing += (_, _) => {
			if (isGottenDialogResultTask) return;
			isGottenDialogResultTask = true;
			taskCompletionSource.SetResult(GetResult());
		};
		return taskCompletionSource.Task;
	}

	/// <summary>
	/// Enable a better looking and easier to see WPF text selection visuals,
	/// and make <see cref="TextBoxBase.SelectionTextBrush" /> effective.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If you've ever selected text in a WPF UI, such as through a <see cref="TextBox" />, you've probably noticed
	/// it looks slightly strange compared to WinForm and WinUI text selection for example - it looks more as though
	/// the text selection color is overlayed on top of the text rather than underneath it.
	/// </para>
	/// <para>
	/// So let's fix this. In .Net Framework 4.7.2,
	/// <a href="https://learn.microsoft.com/dotnet/framework/configure-apps/file-schema/runtime/appcontextswitchoverrides-element#:~:text=Switch.System.Windows.Controls.Text.%0AUseAdornerForTextboxSelectionRendering">Microsoft introduced a fix for this</a>,
	/// which results in the text selection visual in WPF looking much more like it does in other UI frameworks.
	/// </para>
	/// <para><a href="https://codingguides.quinnscomputing.com/2023/03/how-to-enable-better-looking-and-easier.html">Reference</a></para>
	/// </remarks>
	/// <param name="enabled">
	/// <list type="bullet">
	/// <item>If true (by default), it will display a solid selection color, and the text color will be also inverted to white as required.
	/// Just like classic Win32, WinForm, UWP, WinUI, etc. do.</item>
	/// <item>If false, it will use adorner for textbox section rendering to display a translucent selection color overlay the text,
	/// and the text will not change color. Just like WPF does before .NET Framework 4.7.1 and earlier versions.</item>
	/// </list>
	/// </param>
	public static void EnableTextSelectionVisuals(bool enabled = true) =>
		AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", !enabled);

	public static ContextMenu CreateContextMenu(out bool themedSuccessfully) {
		try {
			bool isDark = ShouldAppsUseDarkMode();
			ContextMenu menu = new();
			ContextMenuAcrylicBehavior.SetAutoIcon(menu, false);
			AddThemeResource(menu, AddThemeResourceType.Common);
			AddThemeResource(menu, isDark ? AddThemeResourceType.Dark : AddThemeResourceType.Light);
			themedSuccessfully = true;
			return menu;
		} catch (Exception) {
			themedSuccessfully = false;
			return new();
		}
	}

	private static readonly Brush DefaultBackground = Brushes.Transparent;

	public void UpdateControlBoxesVisibility() {
		if (MinimizeBox is null && MaximizeBox is null && ControlBox is null) return;
		long currentStyle = GetWindowLongPtr(Handle, WindowLongFlags.Style);
		ToggleExtendedWindowStyle(WindowStyles.MinimizeBox, MinimizeBox);
		ToggleExtendedWindowStyle(WindowStyles.MaximizeBox, MaximizeBox);
		ToggleExtendedWindowStyle(WindowStyles.SysMenu, ControlBox);
		SetWindowLongPtr(Handle, WindowLongFlags.Style, currentStyle);

		if (MinimizeBox is not null) DisableOrEnableSystemMenuItems(Handle, SystemMenuItemType.Minimize, MinimizeBox.Value);
		if (MaximizeBox is not null) DisableOrEnableSystemMenuItems(Handle, SystemMenuItemType.Maximize, MaximizeBox.Value);

		void ToggleExtendedWindowStyle(WindowStyles style, bool? enabled) {
			if (enabled == true) currentStyle |= (long)style;
			else if (enabled == false) currentStyle &= ~(long)style;
		}
	}

	#region Set backdrop type
	/// <inheritdoc cref="FrameworkElement.Resources" />
	/// <remarks>
	/// If I don't create a new property with the same name to override it like this,
	/// the style declaration in the implemented window will overwrite the global style.
	/// </remarks>
	public new ResourceDictionary Resources {
		get => base.Resources;
		set {
			// Why not `base.Resources.MergedDictionaries.Add(value);` ?
			// This will make the XAML hot reload feature become invalid.
			ResourceDictionaries originalResources = base.Resources.MergedDictionaries;
			base.Resources = value;
			base.Resources.MergedDictionaries.AddRange(originalResources);
		}
	}

	protected void SetCurrentThemeResource(AddThemeResourceType theme) {
		foreach (ResourceDictionary resource in Resources.MergedDictionaries.ToList())
			if (resource is NamedResourceDictionary named && named.Name == "ThemeColor")
				Resources.MergedDictionaries.Remove(resource);

		AddThemeResource(theme);
	}

	public static void AddResource(FrameworkElement element, string path, bool isNamedResourceDictionary = false) {
		ResourceDictionary resource = isNamedResourceDictionary ? new NamedResourceDictionary() : new ResourceDictionary();
		resource.Source = ProjectUri(path);
		element.Resources.MergedDictionaries.Add(resource);
	}

	public void AddResource(string path, bool isNamedResourceDictionary = false) => AddResource(this, path, isNamedResourceDictionary);

	protected enum AddThemeResourceType {
		Common,
		Light,
		Dark,
		HighContrast,
	}

	protected static void AddThemeResource(FrameworkElement element, AddThemeResourceType theme) {
		if (theme == AddThemeResourceType.Common) {
			AddResource(element, "WPF/Themes/Generic.xaml");
			AddResource(element, "WPF/Themes/Controls.xaml");
		} else
			AddResource(element, $"WPF/Themes/{theme}Theme.xaml", true);
	}

	protected void AddThemeResource(AddThemeResourceType theme) => AddThemeResource(this, theme);

	protected void RefreshFrame() {
		HwndSource mainWindowSrc = HwndSource.FromHwnd(Handle);
		mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

		Margins margins = new(-1);

		ExtendFrame(mainWindowSrc.Handle, margins);
	}

	//[DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#132")] // Not available after Windows 1903.
	protected internal static bool ShouldAppsUseDarkMode() {
		using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
		object? value = key?.GetValue("AppsUseLightTheme");
		return value is 0;
	}

	//[DllImport("dwmapi.dll", EntryPoint = "#127")] // Equivalent
	//internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS dp);
	protected internal static Color? GetDwmColorizationColor() {
		AccentPalette? palette = GetWindowsAccentPalette();
		if (palette is null) return null;
		bool isDark = ShouldAppsUseDarkMode();
		return isDark ? palette.DarkAccentColor : palette.LightAccentColor;
	}

	protected internal static bool GetWindowsAccentPalette(AccentPalette palette) {
		if (SystemParameters.HighContrast) {
			palette.LightAccentColor = palette.DarkAccentColor = palette.Colorization = SystemColors.HighlightColor;
			return true;
		}

		if (WindowsVersion.Current < WindowsNT.Windows10 && IsGlassEnabled) {
			// In Windows 7, change color in control panel directly without saving will not update the settings in the registry, so it looks like the color is delayed.
			palette.LightAccentColor = palette.DarkAccentColor = palette.Colorization = SystemParameters.WindowGlassColor;
			return true;
		}

		using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM")) {
			// CAUTION: It is confusing that "AccentColor" is ABGR, however "ColorizationColor" is ARGB.
			if (key?.GetValue("AccentColor") is int value) // Windows 10 ~ 11
				palette.Colorization = Color.FromAbgr(value);
			else if (key?.GetValue("ColorizationColor") is int value2) // Windows Vista ~ 8.1
				palette.Colorization = Color.FromArgb(value2);
			else // Versions lower than Windows Vista.
				return false;
		}

		using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\Accent")) {
			if (key?.GetValue("AccentPalette") is byte[] value) { // Windows 10 ~ 11
				palette.DarkAccentColor = Color.FromRgb(value[4], value[5], value[6]);
				palette.LightAccentColor = Color.FromRgb(value[16], value[17], value[18]);
			} else
				palette.LightAccentColor = palette.DarkAccentColor = palette.Colorization;
		}

		return true;
	}

	protected internal static AccentPalette GetWindowsAccentPalette() {
		AccentPalette palette = new();
		GetWindowsAccentPalette(palette);
		return palette;
	}

	protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);
		UpdateControlBoxesVisibility();

		// Fix the issue of incorrect window size when use WindowChrome with SizeToContent.WidthAndHeight.
		// See: https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html#720121120
		if (SizeToContent == SizeToContent.WidthAndHeight && Chrome is not null)
			InvalidateMeasure();

		// Detect when the theme changed
		HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
		source.AddHook(WndProc);
	}

	/// <inheritdoc cref="System.Windows.Forms.Form.WndProc(ref System.Windows.Forms.Message)"/>
	protected nint WndProc(nint hWnd, int msg, nint wParam, nint lParam, ref bool handled) {
#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
		const int SettingChange = 0x001A;
		const int DwmColorizationColorChanged = 0x0320;
		const int NCActivate = 0x0086;
		const int DwmCompositionChanged = 0x31E;
		const int ThemeChanged = 0x31A;
		const int NCHitTest = 0x0084;
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值

		switch (msg) {
			// https://learn.microsoft.com/zh-cn/windows/win32/inputdev/wm-nchittest
			// NCHitTest must be listened at first, or it will be ignored in other HwndSource.AddHook.
			case NCHitTest:
				foreach (NCHitTestHookHandler Hook in NCHitTestHooks)
					if (Hook(lParam, ref handled) is nint hitResult && hitResult != 0)
						return hitResult;
				break;
			//case SettingChange:
			//	if (wParam == 0 && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet") {
			//		RefreshDarkMode();
			//		RaiseEvent(new(ThemeChangeEvent, this));
			//		goto case DwmColorizationColorChanged;
			//	}
			//	break;
			//case DwmColorizationColorChanged:
			//	RefreshAccentColor();
			//	RaiseEvent(new(AccentChangeEvent, this));
			//	break;
			case SettingChange:
			case DwmColorizationColorChanged:
				OnSystemThemeChanged();
				break;
			//case NCActivate:
			//	// reference: https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html#29282701
			//	IsNonClientActive = wParam == trueValue;
			//	break;
			case DwmCompositionChanged:
			case ThemeChanged:
				UpdateCurrentThemeName();
				// // Respond to DWM being enabled/disabled or system theme being changed
				// OnTitleBarTypeChanged(TitleBarType);
				// goto case DwmColorizationColorChanged;
				break;
			default:
				break;
		}
		/*if (msg == WM_ShellHook)
			switch ((ShellEvents)wParam) {
				case ShellEvents.Flash:
					//s = "Flash!";
					break;
				default:
					break;
			}*/
		return 0;
	}
	private static readonly nint trueValue = 1;
	//private uint WM_ShellHook;

	//protected override void OnActivated(EventArgs e) {
	//	base.OnActivated(e);
	//	IsNonClientActive = true;
	//}

	//protected override void OnDeactivated(EventArgs e) {
	//	base.OnDeactivated(e);
	//	IsNonClientActive = false;
	//}

	internal delegate nint NCHitTestHookHandler(nint lParam, ref bool handled);
	private readonly List<NCHitTestHookHandler> NCHitTestHooks = [];
	internal event NCHitTestHookHandler OnNCHitTest {
		add => NCHitTestHooks.Add(value);
		remove => NCHitTestHooks.Remove(value);
	}

	private void UpdateCurrentThemeName() => CurrentThemeName = ThemeInfo.Current.ThemeName;

	internal static bool IsGlassEnabled => SystemParameters.IsGlassEnabled;
	protected void OnSystemThemeChanged() {
		//if (e.Category is not (UserPreferenceCategory.Color or UserPreferenceCategory.General or UserPreferenceCategory.Window or UserPreferenceCategory.VisualStyle)) return;
		if (WindowsVersion.Current < WindowsNT.Windows8) // Since Windows 8, the DWM cannot be turned off.
			OnTitleBarTypeChanged(TitleBarType);
		RefreshDarkMode();
		RefreshAccentColor();
	}

	protected void OnSystemThemeChanged(object sender, UserPreferenceChangedEventArgs e) => OnSystemThemeChanged();
	protected void OnSystemThemeChanged(object sender, PropertyChangedEventArgs e) => OnSystemThemeChanged();
	partial void OnIsHighContrastChanged() {
		bool isLoaded = helper is not null;
		if (isLoaded) RefreshDarkMode();
		if (isLoaded) SetSystemBackdropType(SystemBackdropType);
		else Loaded += (_, _) => SetSystemBackdropType(SystemBackdropType);
	}

	protected void RefreshDarkMode() {
		bool isDarkTheme = ShouldAppsUseDarkMode();
		IsLightTheme = !isDarkTheme;
		uint flag = isDarkTheme ? 1u : 0u;
		SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, flag);
		EnableDarkSystemMenu(isDarkTheme);
		SetCurrentThemeResource(IsHighContrast ? AddThemeResourceType.HighContrast : isDarkTheme ? AddThemeResourceType.Dark : AddThemeResourceType.Light);

		SetSolidBackgroundColorAsNeeded();
	}

	protected void SetSolidBackgroundColorAsNeeded() {
		if (Background == DefaultBackground || this.GetDynamicResourceKey(BackgroundProperty) == BackgroundBrushKeyName)
			if (TitleBarType == TitleBarType.System || !IsGlassEnabled || SystemBackdropType == SystemBackdropType.None || IsHighContrast)
				SetResourceReference(BackgroundProperty, BackgroundBrushKeyName);
			else
				Background = DefaultBackground;
	}
	public static readonly SolidColorBrush LightThemeBackgroundBrush = new(Color.FromArgb(0xFFF3F3F3u));
	public static readonly SolidColorBrush DarkThemeBackgroundBrush = new(Color.FromArgb(0xFF202020u));
	public static readonly SolidColorBrush LightThemeAcrylicBackgroundBrush = new(Color.FromArgb(0xB2FCFCFCu));
	public static readonly SolidColorBrush DarkThemeAcrylicBackgroundBrush = new(Color.FromArgb(0xCE2C2C2Cu));
	private const string BackgroundBrushKeyName = "BackgroundBrush";
	private const string AcrylicBackgroundBrushKeyName = "AcrylicBackground";

	private static readonly Color WindowsDefaultGlassColor = Color.FromArgb(0xFF005FB8u);
	private static Brush WindowsDefaultGlassBrush => new SolidColorBrush(WindowsDefaultGlassColor);

	partial void OnCustomAccentColorChanged() => RefreshAccentColor();
	protected void RefreshAccentColor() {
		Color? accentColor = CustomAccentColor;
		accentColor ??= GetDwmColorizationColor();
		if (accentColor is Color color) {
			WindowGlassColor = color;
			WindowGlassBrush = new SolidColorBrush(color);
		}
	}

	private const SystemBackdropType DefaultSystemBackdropType = SystemBackdropType.TransientWindow;

	protected void SetSystemBackdropType(SystemBackdropType systemBackdropType) {
		if (IsHighContrast) systemBackdropType = SystemBackdropType.None;
		if (SupportSystemBackdropType >= SupportSystemBackdropTypeLevel.AcrylicMicaMicaAlt)
			SetWindowAttribute(Handle, DwmWindowAttribute.SystemBackdropType, (uint)systemBackdropType);
		else if (SupportSystemBackdropType >= SupportSystemBackdropTypeLevel.Blur && systemBackdropType is not SystemBackdropType.None) {
			SetAcrylicByComposition(Handle, this, AccentState.EnableBlurBehind);
			// AccentState.EnableAcrylicBlurBehind is stuck when moving window in Windows 10 ~ Windows 11 21H2, so use the early blur effect instead of acrylic.
			//WindowChrome.GetWindowChrome(this).GlassFrameThickness = new(0, 30, 0, 0);
		}
	}

	public static bool SetAcrylicByComposition(nint hWnd, Control control, AccentState backdrop) {
		bool isLight = !ShouldAppsUseDarkMode();
		if (!EnableAcrylicBlurBehind(hWnd, (isLight ? LightThemeAcrylicBackgroundBrush : DarkThemeAcrylicBackgroundBrush).Color.ToAbgr(), backdrop)) return false;
		if (backdrop == AccentState.EnableBlurBehind && control is { })
			if (!(control is BackdropWindow window && window.Background != DefaultBackground))
				control.SetResourceReference(BackgroundProperty, AcrylicBackgroundBrushKeyName);
		return true;
	}

	partial void OnSystemBackdropTypeChanged(SystemBackdropType newValue) {
		SetSystemBackdropType(newValue);
		RefreshDarkMode();
		OnWindowAttributeSetting();
	}

	protected virtual void OnWindowAttributeSetting() { }
	#endregion

	#region Extends content into title bar
	/// <inheritdoc cref="WindowChrome.GetWindowChrome" />
	public WindowChrome? Chrome => WindowChrome.GetWindowChrome(this);

	private static readonly RelativeSource backdropWindowRelativeSource = new(RelativeSourceMode.FindAncestor, typeof(BackdropWindow), 1);

	partial void OnTitleBarTypeChanged(TitleBarType value) {
		if (!IsGlassEnabled) value = TitleBarType.System;
		SetSolidBackgroundColorAsNeeded();
		switch (value) {
			case TitleBarType.WindowChrome:
				WindowChrome.SetWindowChrome(this, new() {
					CaptionHeight = 54, // Default: 20
					CornerRadius = new(6),
					GlassFrameThickness = new(-1),
					UseAeroCaptionButtons = true,
				});
				#region WindowChrome.ResizeBorderThickness
				Binding resizeBorderThicknessBinding = new(nameof(ResizeMode)) {
					RelativeSource = backdropWindowRelativeSource,
					Converter = new WindowChromeTitleBarTypeResizeModeToResizeBorderThicknessConverter(),
				};
				BindingOperations.SetBinding(Chrome, WindowChrome.ResizeBorderThicknessProperty, resizeBorderThicknessBinding);
				#endregion
				#region WindowChrome.NonClientFrameEdges
				MultiBinding nonClientFrameEdgesBinding = new() { Converter = new WindowChromeTitleBarTypeResizeModeAndFlowDirectionAndWindowStateAndCurrentThemeNameToNonClientFrameEdgesConverter() };
				nonClientFrameEdgesBinding.AddBinding(
					new(nameof(ResizeMode)) { RelativeSource = backdropWindowRelativeSource },
					new(nameof(FlowDirection)) { RelativeSource = backdropWindowRelativeSource },
					new(nameof(WindowState)) { RelativeSource = backdropWindowRelativeSource },
					new(nameof(CurrentThemeName)) { RelativeSource = backdropWindowRelativeSource }
				);
				BindingOperations.SetBinding(Chrome, WindowChrome.NonClientFrameEdgesProperty, nonClientFrameEdgesBinding);
				#endregion
				#region WindowChrome.GlassFrameThickness
				if (WindowsVersion.Current is >= WindowsNT.Windows10 and < WindowsNT.Windows11_Dev) { // Windows 10 only.
					Binding glassFrameThicknessBinding = new(nameof(UseUniversalControlBox)) {
						RelativeSource = backdropWindowRelativeSource,
						Converter = new UseUniversalControlBoxToWindowChromeGlassFrameThicknessConverter(),
					};
					BindingOperations.SetBinding(Chrome, WindowChrome.GlassFrameThicknessProperty, glassFrameThicknessBinding);
				}
				#endregion
				#region BackdropWindow.UseUniversalControlBox
				if (WindowsVersion.Current is >= WindowsNT.Windows10 and <= WindowsNT.Windows11) // From Windows 10 RTM to Windows 11 21H2 (before Windows 11 22H2).
					UseUniversalControlBox = true;
				else {
					MultiBinding useUniversalControlBoxBinding = new() { Converter = new SystemBackdropTypeOrIsHighContrastToUseUniversalControlBoxConverter() };
					useUniversalControlBoxBinding.AddBinding(
						new(nameof(SystemBackdropType)) { RelativeSource = new(RelativeSourceMode.Self) },
						new(nameof(IsHighContrast)) { RelativeSource = new(RelativeSourceMode.Self) }
					);
					SetBinding(UseUniversalControlBoxProperty, useUniversalControlBoxBinding);
				}
				#endregion
				break;
			case TitleBarType.WindowChromeNoTitleBar:
				WindowChrome.SetWindowChrome(this, new() {
					CaptionHeight = 0,
					CornerRadius = new(6),
					GlassFrameThickness = new(-1),
					ResizeBorderThickness = new(0),
				});
				WindowStyle = WindowStyle.None;
				ResizeMode = ResizeMode.CanResize;
				break;
			case TitleBarType.Borderless:
				RemoveWindowChrome();
				AllowsTransparency = true;
				WindowStyle = WindowStyle.None;
				break;
			case TitleBarType.System:
			default:
				RemoveWindowChrome();
				break;
		}

		void RemoveWindowChrome() {
			WindowChrome.SetWindowChrome(this, null);
			UseUniversalControlBox = false;
		}
	}

	private static void FixNonClientFrameEdgesMargin(Border templateRootBorder) {
		templateRootBorder.Margin = new(0);
		//I don't know why does WindowChrome set a bad margin (0, 0, -6.5, 0).
		MultiBinding marginBinding = new() { Converter = new WindowChromeNonClientFrameEdgesMarginAndMaximizedWindowStateToTemplateRootBorderMarginConverter() };
		marginBinding.AddBinding(
			new("Chrome.NonClientFrameEdges") { RelativeSource = backdropWindowRelativeSource },
			new("WindowState") { RelativeSource = backdropWindowRelativeSource }
		);
		templateRootBorder.SetBinding(MarginProperty, marginBinding);
	}

	private void FixNonClientFrameEdgesMargin() {
		if (VisualChildrenCount > 0 && GetVisualChild(0) is Border templateRootBorder) {
			FixNonClientFrameEdgesMargin(templateRootBorder);
			templateRootBorder.LayoutUpdated += (_, _) => {
				if (templateRootBorder.GetBindingExpression(MarginProperty) is null)
					FixNonClientFrameEdgesMargin(templateRootBorder);
			};
			templateRootBorder.Unloaded += (_, _) => FixNonClientFrameEdgesMargin();
		}
	}

	#region Converters
	[ValueConversion(typeof(ResizeMode), typeof(Thickness))]
	private class WindowChromeTitleBarTypeResizeModeToResizeBorderThicknessConverter : ValueConverter<ResizeMode, Thickness> {
		public override Thickness Convert(ResizeMode resizeMode, Type targetType, object parameter, CultureInfo culture) =>
			resizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize ? new(0) : new(8, 0, 8, 8);
	}

	private class WindowChromeTitleBarTypeResizeModeAndFlowDirectionAndWindowStateAndCurrentThemeNameToNonClientFrameEdgesConverter : MultiValueConverter<(ResizeMode, FlowDirection, WindowState, string), NonClientFrameEdges> {
		// Decided by whenever ILRepark is enabled.
		public override NonClientFrameEdges Convert((ResizeMode, FlowDirection, WindowState, string) value, Type targetType, object parameter, CultureInfo culture) {
			(ResizeMode resizeMode, FlowDirection flowDirection, WindowState windowState, string themeName) = value;
			return resizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize ? NonClientFrameEdges.None :
				windowState == WindowState.Maximized ? NonClientFrameEdges.None :
				WindowsVersion.Current < WindowsNT.Windows10_TP || themeName == "AeroLite" ? NonClientFrameEdges.None :
				flowDirection == FlowDirection.LeftToRight ? NonClientFrameEdges.Right :
				NonClientFrameEdges.Right | NonClientFrameEdges.Left | NonClientFrameEdges.Bottom;
		}
	}

	private class WindowChromeNonClientFrameEdgesMarginAndMaximizedWindowStateToTemplateRootBorderMarginConverter : MultiValueConverter<(NonClientFrameEdges, WindowState), Thickness> {
		public override Thickness Convert((NonClientFrameEdges, WindowState) value, Type targetType, object parameter, CultureInfo culture) {
			(NonClientFrameEdges ncEdge, WindowState state) = value;
			const double BASE_MARGIN = -6.5;
			return state != WindowState.Maximized ? new(0) : new(
				(ncEdge & NonClientFrameEdges.Left) != 0 ? BASE_MARGIN : 0,
				(ncEdge & NonClientFrameEdges.Top) != 0 ? BASE_MARGIN : 0,
				(ncEdge & NonClientFrameEdges.Right) != 0 ? BASE_MARGIN : 0,
				(ncEdge & NonClientFrameEdges.Bottom) != 0 ? BASE_MARGIN : 0
			);
		}
	}

	[ValueConversion(typeof(bool), typeof(Thickness))]
	private class UseUniversalControlBoxToWindowChromeGlassFrameThicknessConverter : ValueConverter<bool, Thickness> {
		public override Thickness Convert(bool use, Type targetType, object parameter, CultureInfo culture) =>
			!use ? new(-1) : new(0, 1, 0, 0);
	}

	[ValueConversion(typeof(SystemBackdropType), typeof(bool))]
	private class SystemBackdropTypeOrIsHighContrastToUseUniversalControlBoxConverter : MultiValueConverter<(SystemBackdropType, bool), bool> {
		public override bool Convert((SystemBackdropType, bool) value, Type targetType, object parameter, CultureInfo culture) {
			(SystemBackdropType backdrop, bool highContrast) = value;
			return backdrop == SystemBackdropType.None || highContrast;
		}
	}
	#endregion

	protected override void OnKeyDown(KeyEventArgs e) {
		if (TitleBarType == TitleBarType.WindowChromeNoTitleBar) {
			if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space) {
				e.Handled = true;
				return;
			}
		}
		if (e.Key == Key.Escape)
			this.Vanish();
		base.OnKeyDown(e);
	}
	#endregion

	#region Default fonts
	private void OnCultureChanged(CultureInfo culture) {
		FontFamily defaultI10nFont = new Control().FontFamily,
			englishMonoFont = (FontFamily)Resources["EnglishMonoFont"],
			englishPropFont = (FontFamily)Resources["EnglishPropFont"];
		FontFamily? defaultGuessedFont = culture.GetDefaultUIFont();
		DefaultFont = FontFamily.Concat([englishPropFont, defaultGuessedFont, defaultI10nFont]);
		MonoFont = FontFamily.Concat([englishMonoFont, defaultGuessedFont, defaultI10nFont]);
		FontFamily = DefaultFont;
	}
	#endregion
}

public enum TitleBarType {
	System,
	Borderless,
	WindowChrome,
	WindowChromeNoTitleBar,
}

[Flags]
public enum SetWidthType {
	Nothing = 0,
	Width = 1 << 0,
	MinWidth = 1 << 1,
	MaxWidth = 1 << 2,
}
