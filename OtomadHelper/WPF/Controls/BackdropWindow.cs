using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shell;

using OtomadHelper.Module;
using OtomadHelper.WPF.Common;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// BackdropWindow.xaml 的交互逻辑
/// </summary>
public partial class BackdropWindow : Window, INotifyPropertyChanged {
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
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		BindViewToViewModel();
		RefreshFrame();
		RefreshDarkMode(isOnLoad: true);
		SetSystemBackdropType(SystemBackdropType);
	}

	protected void SetCurrentThemeResource(bool isDarkTheme, bool isOnLoad) {
		const string THEME_COLOR = "ThemeColor";
		foreach (ResourceDictionary resource in Resources.MergedDictionaries)
			if (resource is NamedResourceDictionary named && named.Name == THEME_COLOR)
				Resources.MergedDictionaries.Remove(resource);

		if (isOnLoad)
			AddDictionary("Wpf/Styles/Controls.xaml");

		AddDictionary($"Wpf/Styles/{(isDarkTheme ? "Dark" : "Light")}Theme.xaml");

		void AddDictionary(string path) =>
			Resources.MergedDictionaries.Add(new() { Source = new($"pack://application:,,,/{OtomadHelperModule.ASSEMBLY_NAME};component/{path}", UriKind.Absolute) });
	}

	private void BindViewToViewModel() {
		if (DataContext is null) return;
		PropertyInfo? viewProperty = DataContext.GetType().GetProperty("View");
		if (viewProperty is not null &&
			typeof(FrameworkElement).IsAssignableFrom(viewProperty.PropertyType) &&
			viewProperty.SetMethod is not null)
			viewProperty.SetMethod.Invoke(DataContext, new object[] { this });
	}

	#region Set backdrop type
	protected void RefreshFrame() {
		IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
		HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
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
		return value is int i && i == 0;
	}

	protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);

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
	#endregion

	#region Extends content into title bar
	public static readonly DependencyProperty TitleBarTypeProperty = DependencyProperty.Register(
		nameof(TitleBarType), typeof(TitleBarType), typeof(BackdropWindow), new PropertyMetadata(TitleBarType.System, EnableWindowChromeChangedCallback));
	public TitleBarType TitleBarType { get => (TitleBarType)GetValue(TitleBarTypeProperty); set => SetValue(TitleBarTypeProperty, value); }

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
