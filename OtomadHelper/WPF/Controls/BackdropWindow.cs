using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// BackdropWindow.xaml 的交互逻辑
/// </summary>
public class BackdropWindow : Window, INotifyPropertyChanged {
	protected readonly WindowInteropHelper helper;
	protected IntPtr Handle => helper.Handle;

	public BackdropWindow() : base() {
		InitializeComponent();
		helper = new(this);
	}

	public IntPtr OwnerHandle {
		get => helper.Owner;
		set => helper.Owner = value;
	}

	private void InitializeComponent() {
		RefreshCulture();
		AddResource("Wpf/Styles/Generic.xaml");
		Background = Brushes.Transparent;
		Loaded += Window_Loaded;
		IsVisibleChanged += (sender, e) => {
			if ((bool)e.NewValue) RaiseEvent(new RoutedEventArgs(ShowingEvent));
		};
		OnCultureChanged(Culture);
		CultureChanged += OnCultureChanged;

		// Default styles
		SetResourceReference(BorderBrushProperty, "CardStroke");
		BorderThickness = new(1);
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		ReserveSystemMenuItems(Handle, SystemMenuItemType.MOVE | SystemMenuItemType.CLOSE);
		BindViewToViewModel();
		RefreshFrame();
		RefreshDarkMode();
		RefreshAccentColor();
		SetSystemBackdropType(SystemBackdropType);
		if (TitleBarType == TitleBarType.WindowChromeNoTitleBar)
			AddExtendedWindowStyles(Handle, ExtendedWindowStyles.ToolWindow);
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
		(double dpiX, double dpiY) = this.GetDpi();
		double maxLeft = workingArea.Right / dpiX - Width,
			maxTop = workingArea.Bottom / dpiY - Height,
			minLeft = workingArea.Left / dpiX,
			minTop = workingArea.Top / dpiY,
			screenWidth = workingArea.Width / dpiX;
		if (Left > maxLeft) Left = maxLeft;
		if (Top > maxTop) Top = maxTop;
		if (Left < minLeft) Left = minLeft;
		if (Top < minTop) Top = minTop;
		MaxWidth = Math.Min(MaxWidth, screenWidth);
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
		Closing += (sender, e) => {
			if (isGottenDialogResultTask) return;
			isGottenDialogResultTask = true;
			taskCompletionSource.SetResult(GetResult());
		};
		return taskCompletionSource.Task;
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

		AddResource($"Wpf/Styles/{(isDarkTheme ? "Dark" : "Light")}Theme.xaml", true);
	}

	public void AddResource(string path, bool isNamedResourceDictionary = false) {
		ResourceDictionary resource = isNamedResourceDictionary ? new NamedResourceDictionary() : new ResourceDictionary();
		resource.Source = ProjectUri(path);
		Resources.MergedDictionaries.Add(resource);
	}

	protected void RefreshFrame() {
		HwndSource mainWindowSrc = HwndSource.FromHwnd(Handle);
		mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

		MARGINS margins = new() {
			cxLeftWidth = -1,
			cxRightWidth = -1,
			cyTopHeight = -1,
			cyBottomHeight = -1,
		};

		ExtendFrame(mainWindowSrc.Handle, margins);
	}

	//[DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#132")] // Not available after Windows 1903.
	protected static bool ShouldAppsUseDarkMode() {
		using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");
		object? value = key?.GetValue("AppsUseLightTheme");
		return value is 0;
	}

	//[DllImport("dwmapi.dll", EntryPoint = "#127")] // Equivalent
	//internal static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS dp);
	protected static Color? GetDwmColorizationColor() {
		using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
		object? value = key?.GetValue("AccentColor");
		if (value is not int accentColorValue) return null;
		Color color = FromAbgr(accentColorValue);
		return color;

		static Color FromAbgr(int value) => Color.FromArgb(
			(byte)(value >> 24),
			(byte)value,
			(byte)(value >> 8),
			(byte)(value >> 16)
		);
	}

	protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);

		// Fix the issue of incorrect window size when use WindowChrome with SizeToContent.WidthAndHeight.
		// See: https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html#720121120
		if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
			InvalidateMeasure();

		// Detect when the theme changed
		HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
		source.AddHook(WndProc);
	}

	/// <inheritdoc cref="System.Windows.Forms.Form.WndProc(ref System.Windows.Forms.Message)"/>
	protected IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) {
		const int WM_SETTINGCHANGE = 0x001A;
		const int WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320;

		switch (msg) {
			case WM_SETTINGCHANGE:
				if (wParam == IntPtr.Zero && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet") {
					RefreshDarkMode();
					RaiseEvent(new RoutedEventArgs(ThemeChangeEvent, this));
				}
				break;
			case WM_DWMCOLORIZATIONCOLORCHANGED:
				RefreshAccentColor();
				RaiseEvent(new RoutedEventArgs(AccentChangeEvent, this));
				break;
			default:
				break;
		}
		return IntPtr.Zero;
	}

	protected void RefreshDarkMode() {
		bool isDarkTheme = ShouldAppsUseDarkMode();
		IsLightTheme = !isDarkTheme;
		int flag = isDarkTheme ? 1 : 0;
		SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, flag);
		SetCurrentThemeResource(isDarkTheme);
	}

	protected void RefreshAccentColor() {
		if (GetDwmColorizationColor() is Color accentColor) {
			WindowGlassColor = accentColor;
			WindowGlassBrush = new SolidColorBrush(accentColor);
		}
	}

	private const SystemBackdropType DEFAULT_SYSTEM_BACKDROP_TYPE = SystemBackdropType.TransientWindow;
	public static readonly DependencyProperty SystemBackdropTypeProperty = DependencyProperty.Register(
		nameof(SystemBackdropType), typeof(SystemBackdropType), typeof(BackdropWindow), new(DEFAULT_SYSTEM_BACKDROP_TYPE, SystemBackdropTypeChangedCallback));
	public SystemBackdropType SystemBackdropType { get => (SystemBackdropType)GetValue(SystemBackdropTypeProperty); set => SetValue(SystemBackdropTypeProperty, value); }

	protected void SetSystemBackdropType(SystemBackdropType systemBackdropType) {
		SetWindowAttribute(Handle, DwmWindowAttribute.SystemBackdropType, (int)systemBackdropType);
	}

	private static void SystemBackdropTypeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not BackdropWindow window) return;
		window.SetSystemBackdropType((SystemBackdropType)e.NewValue);
	}

	protected static readonly RoutedEvent ThemeChangeEvent =
		EventManager.RegisterRoutedEvent(nameof(ThemeChange), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(BackdropWindow));
	public event RoutedEventHandler ThemeChange {
		add => AddHandler(ThemeChangeEvent, value);
		remove => RemoveHandler(ThemeChangeEvent, value);
	}
	protected static readonly RoutedEvent AccentChangeEvent =
		EventManager.RegisterRoutedEvent(nameof(AccentChange), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(BackdropWindow));
	public event RoutedEventHandler AccentChange {
		add => AddHandler(AccentChangeEvent, value);
		remove => RemoveHandler(AccentChangeEvent, value);
	}

	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public static readonly RoutedEvent ShowingEvent = EventManager.RegisterRoutedEvent("Showing", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BackdropWindow));

	public event RoutedEventHandler Showing {
		add => AddHandler(ShowingEvent, value);
		remove => RemoveHandler(ShowingEvent, value);
	}

	public static readonly DependencyProperty IsLightThemeProperty = DependencyProperty.Register(
		nameof(IsLightTheme), typeof(bool), typeof(BackdropWindow), new(true));
	public bool IsLightTheme { get => (bool)GetValue(IsLightThemeProperty); private set => SetValue(IsLightThemeProperty, value); }

	private static readonly Color WindowsDefaultGlassColor = Color.FromRgb(0, 95, 184);

	public static readonly DependencyProperty WindowGlassColorProperty = DependencyProperty.Register(
		nameof(WindowGlassColor), typeof(Color), typeof(BackdropWindow), new(WindowsDefaultGlassColor));
	public Color WindowGlassColor { get => (Color)GetValue(WindowGlassColorProperty); private set => SetValue(WindowGlassColorProperty, value); }

	public static readonly DependencyProperty WindowGlassBrushProperty = DependencyProperty.Register(
		nameof(WindowGlassBrush), typeof(Brush), typeof(BackdropWindow), new(new SolidColorBrush(WindowsDefaultGlassColor)));
	public Brush WindowGlassBrush { get => (Brush)GetValue(WindowGlassBrushProperty); private set => SetValue(WindowGlassBrushProperty, value); }
	#endregion

	#region Extends content into title bar
	public static readonly DependencyProperty TitleBarTypeProperty = DependencyProperty.Register(
		nameof(TitleBarType), typeof(TitleBarType), typeof(BackdropWindow), new(TitleBarType.System, EnableWindowChromeChangedCallback));
	public TitleBarType TitleBarType {
		get => (TitleBarType)GetValue(TitleBarTypeProperty);
		set => SetValue(TitleBarTypeProperty, value);
	}

	private static void EnableWindowChromeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not BackdropWindow window) return;
		if (e.NewValue is not TitleBarType value) return;
		switch (value) {
			case TitleBarType.WindowChrome:
				WindowChrome.SetWindowChrome(window, new() {
					CaptionHeight = 54, // Default: 20
					CornerRadius = new(0),
					GlassFrameThickness = new(-1),
					ResizeBorderThickness = window.ResizeMode is ResizeMode.NoResize or ResizeMode.CanMinimize ?
						new(0) : new(8, 0, 8, 8),
					NonClientFrameEdges = NonClientFrameEdges.Right,
					UseAeroCaptionButtons = true,
				});
				break;
			case TitleBarType.WindowChromeNoTitleBar:
				WindowChrome.SetWindowChrome(window, new() {
					CaptionHeight = 0,
					CornerRadius = new(0),
					GlassFrameThickness = new(-1),
					ResizeBorderThickness = new(0),
				});
				window.WindowStyle = WindowStyle.None;
				window.ResizeMode = ResizeMode.CanResize;
				break;
			case TitleBarType.Borderless:
				RemoveWindowChrome();
				window.AllowsTransparency = true;
				window.WindowStyle = WindowStyle.None;
				break;
			case TitleBarType.System:
			default:
				RemoveWindowChrome();
				break;
		}

		void RemoveWindowChrome() => WindowChrome.SetWindowChrome(window, null);
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
	public static readonly DependencyProperty MonoFontProperty = DependencyProperty.Register(
		nameof(MonoFont), typeof(FontFamily), typeof(BackdropWindow));
	public FontFamily MonoFont { get => (FontFamily)GetValue(MonoFontProperty); private set => SetValue(MonoFontProperty, value); }

	private void OnCultureChanged(CultureInfo culture) {
		FontFamily defaultFont = FontFamily, englishMonoFont = (FontFamily)Resources["EnglishMonoFont"];
		MonoFont = new(new[] { englishMonoFont, defaultFont }.Select(font => font.Source).Join(", "));
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
