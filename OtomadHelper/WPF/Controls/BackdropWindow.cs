using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

using OtomadHelper.Models;

using ContextMenu = System.Windows.Controls.ContextMenu;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// BackdropWindow.xaml 的交互逻辑
/// </summary>
[DependencyProperty<SystemBackdropType>("SystemBackdropType", DefaultValueExpression = nameof(DefaultSystemBackdropType))]
[DependencyProperty<bool>("IsLightTheme", DefaultValue = true)]
[DependencyProperty<Color?>("CustomAccentColor")]
[DependencyProperty<Color>("WindowGlassColor", DefaultValueExpression = nameof(WindowsDefaultGlassColor), IsReadOnly = true)]
[DependencyProperty<Brush>("WindowGlassBrush", DefaultValueExpression = nameof(WindowsDefaultGlassBrush), IsReadOnly = true)]
[DependencyProperty<TitleBarType>("TitleBarType", DefaultValueExpression = "TitleBarType.System")]
[DependencyProperty<FontFamily>("MonoFont")]
[DependencyProperty<FontFamily>("DefaultFont")]
[DependencyProperty<bool>("IsNonClientActive")]
[DependencyProperty<bool?>("MinimizeBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.MinimizeBox" />""")]
[DependencyProperty<bool?>("MaximizeBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.MaximizeBox" />""")]
[DependencyProperty<bool?>("ControlBox", OnChanged = nameof(UpdateControlBoxesVisibility), PropertyXmlDocumentation = """<inheritdoc cref="System.Windows.Forms.Form.ControlBox" />""")]
[RoutedEvent("Showing", RoutedEventStrategy.Bubble)]
public partial class BackdropWindow : Window {
	protected readonly WindowInteropHelper helper;
	protected IntPtr Handle => helper.Handle;

	private bool IsRtl => false;

	public BackdropWindow() : base() {
		InitializeComponent();
		helper = new(this);
		if (IsRtl) FlowDirection = FlowDirection.RightToLeft;
	}

	public IntPtr OwnerHandle {
		get => helper.Owner;
		set => helper.Owner = value;
	}

	private void InitializeComponent() {
		RefreshCulture();
		CommandBindings.AddRange(Commands.CommandBindings);
		AddResource("WPF/Themes/Generic.xaml");
		AddResource("WPF/Themes/Controls.xaml");
		if (Background == DefaultBackground) base.Background = Background;
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
		if (ResizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize)
			ReserveSystemMenuItems(Handle, SystemMenuItemType.Move | SystemMenuItemType.Close);
		BindViewToViewModel();
		RefreshFrame();
		RefreshDarkMode();
		RefreshAccentColor();
		SetSystemBackdropType(SystemBackdropType);
		if (TitleBarType == TitleBarType.WindowChromeNoTitleBar)
			AddExtendedWindowStyles(Handle, ExtendedWindowStyles.ToolWindow);
		//SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, 0xfffffffe);
		OnWindowAttributeSetting();
		// reference: https://www.cnblogs.com/code1992/p/11699416.html
		/*if (RegisterShellHookWindow(Handle))
			WM_ShellHook = RegisterWindowMessage("SHELLHOOK");*/
		if (WindowsVersion.Current < WindowsNT.Windows8)
			SystemEvents.UserPreferenceChanged += OnSystemThemeChanged;
	}

	private void Window_Closed(object sender, EventArgs e) {
		//DeregisterShellHookWindow(Handle);
		if (WindowsVersion.Current < WindowsNT.Windows8)
			SystemEvents.UserPreferenceChanged -= OnSystemThemeChanged;
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
			AddResource(menu, "WPF/Themes/Generic.xaml");
			AddResource(menu, "WPF/Themes/Controls.xaml");
			AddResource(menu, $"WPF/Themes/{(isDark ? "Dark" : "Light")}Theme.xaml", true);
			themedSuccessfully = true;
			return menu;
		} catch (Exception) {
			themedSuccessfully = false;
			return new();
		}
	}

	private static readonly Brush DefaultBackground = Brushes.Transparent;
	public new Brush Background { get; set { field = value; base.Background = value; } } = DefaultBackground;

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

	protected void SetCurrentThemeResource(bool isDarkTheme) {
		foreach (ResourceDictionary resource in Resources.MergedDictionaries.ToList())
			if (resource is NamedResourceDictionary named && named.Name == "ThemeColor")
				Resources.MergedDictionaries.Remove(resource);

		AddResource($"WPF/Themes/{(isDarkTheme ? "Dark" : "Light")}Theme.xaml", true);
	}

	public static void AddResource(FrameworkElement element, string path, bool isNamedResourceDictionary = false) {
		ResourceDictionary resource = isNamedResourceDictionary ? new NamedResourceDictionary() : new ResourceDictionary();
		resource.Source = ProjectUri(path);
		element.Resources.MergedDictionaries.Add(resource);
	}

	public void AddResource(string path, bool isNamedResourceDictionary = false) => AddResource(this, path, isNamedResourceDictionary);

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
		if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) is not null)
			InvalidateMeasure();

		// Detect when the theme changed
		HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
		source.AddHook(WndProc);
	}

	/// <inheritdoc cref="System.Windows.Forms.Form.WndProc(ref System.Windows.Forms.Message)"/>
	protected IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
		const int SettingChange = 0x001A;
		const int DwmColorizationColorChanged = 0x0320;
		const int NCActivate = 0x0086;
		const int DwmCompositionChanged = 0x31E;
		const int ThemeChanged = 0x31A;
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值

		switch (msg) {
			//case SettingChange:
			//	if (wParam == IntPtr.Zero && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet") {
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
				OnSystemThemeChanged(null, new(UserPreferenceCategory.General));
				break;
			case NCActivate:
				// reference: https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html#29282701
				IsNonClientActive = wParam == trueValue;
				break;
			//case DwmCompositionChanged:
			//case ThemeChanged:
			//	// Respond to DWM being enabled/disabled or system theme being changed
			//	OnTitleBarTypeChanged(TitleBarType);
			//	goto case DwmColorizationColorChanged;
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
		return IntPtr.Zero;
	}
	private static readonly IntPtr trueValue = new(1);
	//private uint WM_ShellHook;

	protected override void OnActivated(EventArgs e) {
		base.OnActivated(e);
		IsNonClientActive = true;
	}

	protected override void OnDeactivated(EventArgs e) {
		base.OnDeactivated(e);
		IsNonClientActive = false;
	}

	internal static bool IsGlassEnabled => SystemParameters.IsGlassEnabled;
	protected void OnSystemThemeChanged(object? sender, UserPreferenceChangedEventArgs e) {
		//if (e.Category is not (UserPreferenceCategory.Color or UserPreferenceCategory.General or UserPreferenceCategory.Window or UserPreferenceCategory.VisualStyle)) return;
		if (WindowsVersion.Current < WindowsNT.Windows8) // Since Windows 8, the DWM will not be turned off.
			OnTitleBarTypeChanged(TitleBarType);
		RefreshDarkMode();
		RefreshAccentColor();
	}

	protected void RefreshDarkMode() {
		bool isDarkTheme = ShouldAppsUseDarkMode();
		IsLightTheme = !isDarkTheme;
		uint flag = isDarkTheme ? 1u : 0u;
		SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, flag);
		EnableDarkSystemMenu(isDarkTheme);
		SetCurrentThemeResource(isDarkTheme);
		//Color borderColor = isDarkTheme ? Color.FromRgb(20, 20, 20) : Color.FromRgb(219, 219, 219);
		//SetWindowAttribute(Handle, DwmWindowAttribute.BorderColor, borderColor.ToAbgr(false));

		SetSolidBackgroundColorAsNeeded();
		// TODO: Change background color will cover the three window buttons.
		//Color solidBackgroundColor = isDarkTheme ? Color.FromRgb(32, 32, 32) : Color.FromRgb(243, 243, 243);
		//if (Background == DefaultBackground)
		//	base.Background = (SystemBackdropType == SystemBackdropType.None || !SupportSystemBackdropType) && TitleBarType != TitleBarType.Borderless ?
		//		new SolidColorBrush(solidBackgroundColor) : Brushes.Transparent;
	}

	protected void SetSolidBackgroundColorAsNeeded() {
		SolidColorBrush solidBackgroundBrush = IsLightTheme ? SolidLightThemeBackgroundBrush : SolidDarkThemeBackgroundBrush;
		if (Background == DefaultBackground)
			base.Background = TitleBarType == TitleBarType.System || !IsGlassEnabled ? solidBackgroundBrush : DefaultBackground;
	}
	public static readonly SolidColorBrush SolidLightThemeBackgroundBrush = new(Color.FromRgb(243, 243, 243));
	public static readonly SolidColorBrush SolidDarkThemeBackgroundBrush = new(Color.FromRgb(32, 32, 32));

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
		SetWindowAttribute(Handle, DwmWindowAttribute.SystemBackdropType, (uint)systemBackdropType);
	}

	partial void OnSystemBackdropTypeChanged(SystemBackdropType newValue) {
		SetSystemBackdropType(newValue);
		RefreshDarkMode();
		OnWindowAttributeSetting();
	}

	protected virtual void OnWindowAttributeSetting() { }

	private static readonly Color WindowsDefaultGlassColor = Color.FromRgb(0, 95, 184);
	private static Brush WindowsDefaultGlassBrush => new SolidColorBrush(WindowsDefaultGlassColor);
	#endregion

	#region Extends content into title bar
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
				RelativeSource backdropWindowRelativeSource = new(RelativeSourceMode.FindAncestor, typeof(BackdropWindow), 1);
				Binding resizeBorderThicknessBinding = new("ResizeMode") {
					RelativeSource = backdropWindowRelativeSource,
					Converter = new WindowChromeTitleBarTypeResizeModeToResizeBorderThicknessConverter(),
				};
				BindingOperations.SetBinding(WindowChrome.GetWindowChrome(this), WindowChrome.ResizeBorderThicknessProperty, resizeBorderThicknessBinding);
				MultiBinding nonClientFrameEdgesBinding = new() { Converter = new WindowChromeTitleBarTypeResizeModeAndFlowDirectionToNonClientFrameEdgesConverter() };
				nonClientFrameEdgesBinding.AddBinding([
					new("ResizeMode") { RelativeSource = backdropWindowRelativeSource },
					new("FlowDirection") { RelativeSource = backdropWindowRelativeSource },
				]);
				BindingOperations.SetBinding(WindowChrome.GetWindowChrome(this), WindowChrome.NonClientFrameEdgesProperty, nonClientFrameEdgesBinding);
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

		void RemoveWindowChrome() => WindowChrome.SetWindowChrome(this, null);
	}

	[ValueConversion(typeof(ResizeMode), typeof(Thickness))]
	public class WindowChromeTitleBarTypeResizeModeToResizeBorderThicknessConverter : ValueConverter<ResizeMode, Thickness> {
		public override Thickness Convert(ResizeMode resizeMode, Type targetType, object parameter, CultureInfo culture) =>
			resizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize ? new(0) : new(8, 0, 8, 8);
	}

	[ValueConversion(typeof(ResizeMode), typeof(NonClientFrameEdges))]
	public class WindowChromeTitleBarTypeResizeModeAndFlowDirectionToNonClientFrameEdgesConverter : MultiValueConverter<ValueTuple<ResizeMode, FlowDirection>, NonClientFrameEdges> {
		// Decided by whenever ILRepark is enabled.
		public override NonClientFrameEdges Convert(ValueTuple<ResizeMode, FlowDirection> value, Type targetType, object parameter, CultureInfo culture) {
			(ResizeMode resizeMode, FlowDirection flowDirection) = value;
			return resizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize ? NonClientFrameEdges.None :
				WindowsVersion.Current < WindowsNT.Windows10 ? NonClientFrameEdges.None :
				flowDirection == FlowDirection.LeftToRight ? NonClientFrameEdges.Right :
				NonClientFrameEdges.Right | NonClientFrameEdges.Left | NonClientFrameEdges.Bottom;
		}
	}

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
