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

	public BackdropWindow() {
		InitializeComponent();
		helper = new(this);
	}

	public IntPtr OwnerHandle {
		get => helper.Owner;
		set => helper.Owner = value;
	}

	private void InitializeComponent() {
		Background = Brushes.Transparent;
		Loaded += Window_Loaded;
		IsVisibleChanged += (sender, e) => {
			if ((bool)e.NewValue) RaiseEvent(new RoutedEventArgs(ShowingEvent));
		};

		// Default styles
		SetResourceReference(BorderBrushProperty, "CardStroke");
		BorderThickness = new(1);
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		BindViewToViewModel();
		RefreshFrame();
		RefreshDarkMode(isOnLoad: true);
		SetSystemBackdropType(SystemBackdropType);
		if (TitleBarType == TitleBarType.WindowChromeNoTitleBar)
			AddExtendedWindowStyles(Handle, ExtendedWindowStyles.ToolWindow);
	}

	protected void SetCurrentThemeResource(bool isDarkTheme, bool isOnLoad) {
		foreach (ResourceDictionary resource in Resources.MergedDictionaries.ToArray())
			if (IsThemeColorResource(resource))
				Resources.MergedDictionaries.Remove(resource);

		if (isOnLoad)
			AddDictionary("Wpf/Styles/Controls.xaml");

		AddDictionary($"Wpf/Styles/{(isDarkTheme ? "Dark" : "Light")}Theme.xaml");

		bool IsThemeColorResource(ResourceDictionary resource) =>
			resource["ResourceDictionaryName"] is "ThemeColor";

		void AddDictionary(string path) =>
			Resources.MergedDictionaries.Add(new() { Source = ProjectUri(path) });
	}

	private void BindViewToViewModel() {
		if (DataContext is IViewAccessibleViewModel viewModel)
			viewModel.SetView(this);
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
		double maxLeft = workingArea.Right / dpiX - Width;
		double maxTop = workingArea.Bottom / dpiY - Height;
		if (Left > maxLeft) Left = maxLeft;
		if (Top > maxTop) Top = maxTop;
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

	#region Set backdrop type
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

	protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);

		// Fix the issue of incorrect window size when use WindowChrome with SizeToContent.WidthAndHeight.
		// See: https://www.cnblogs.com/dino623/p/problems_of_WindowChrome.html#720121120
		if (SizeToContent == SizeToContent.WidthAndHeight && WindowChrome.GetWindowChrome(this) != null)
			InvalidateMeasure();

		// Detect when the theme changed
		HwndSource source = (HwndSource)PresentationSource.FromVisual(this);
		source.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) => {
			const int WM_SETTINGCHANGE = 0x001A;
			if (msg == WM_SETTINGCHANGE)
				if (wParam == IntPtr.Zero && Marshal.PtrToStringUni(lParam) == "ImmersiveColorSet") {
					RefreshDarkMode(isOnLoad: false);
					RaiseEvent(new RoutedEventArgs(ThemeChangeEvent, this));
				}
			return IntPtr.Zero;
		});
	}

	protected void RefreshDarkMode(bool isOnLoad) {
		bool isDarkTheme = ShouldAppsUseDarkMode();
		int flag = isDarkTheme ? 1 : 0;
		SetWindowAttribute(Handle, DwmWindowAttribute.UseImmersiveDarkMode, flag);
		SetCurrentThemeResource(isDarkTheme, isOnLoad);
	}

	private const SystemBackdropType DEFAULT_SYSTEM_BACKDROP_TYPE = SystemBackdropType.TransientWindow;
	public static readonly DependencyProperty SystemBackdropTypeProperty = DependencyProperty.Register(
		nameof(SystemBackdropType), typeof(SystemBackdropType), typeof(BackdropWindow), new PropertyMetadata(DEFAULT_SYSTEM_BACKDROP_TYPE, SystemBackdropTypeChangedCallback));
	public SystemBackdropType SystemBackdropType { get => (SystemBackdropType)GetValue(SystemBackdropTypeProperty); set => SetValue(SystemBackdropTypeProperty, value); }

	protected void SetSystemBackdropType(SystemBackdropType systemBackdropType) {
		SetWindowAttribute(Handle, DwmWindowAttribute.SystemBackdropType, (int)systemBackdropType);
	}

	private static void SystemBackdropTypeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not BackdropWindow window) return;
		window.SetSystemBackdropType((SystemBackdropType)e.NewValue);
	}

	protected static readonly RoutedEvent ThemeChangeEvent =
		EventManager.RegisterRoutedEvent("ThemeChange", RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(BackdropWindow));

	public event RoutedEventHandler ThemeChange {
		add => AddHandler(ThemeChangeEvent, value);
		remove => RemoveHandler(ThemeChangeEvent, value);
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
	#endregion

	#region Extends content into title bar
	public static readonly DependencyProperty TitleBarTypeProperty = DependencyProperty.Register(
		nameof(TitleBarType), typeof(TitleBarType), typeof(BackdropWindow), new PropertyMetadata(TitleBarType.System, EnableWindowChromeChangedCallback));
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
					ResizeBorderThickness = new(8, 0, 8, 8),
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
