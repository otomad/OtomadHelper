using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static OtomadHelper.Interop.PInvoke.ParameterTypes;
using static OtomadHelper.Interop.PInvoke.Methods;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Shell;
using OtomadHelper.Properties;
using System.Diagnostics;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// BackdropWindow.xaml 的交互逻辑
/// </summary>
public partial class BackdropWindow : Window, INotifyPropertyChanged {
	protected IntPtr Handle => new WindowInteropHelper(this).Handle;

	public BackdropWindow() {
		InitializeComponent();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		RefreshFrame();
		RefreshDarkMode();
		SystemBackdropType = DWM_SYSTEMBACKDROP_TYPE.DWMSBT_TRANSIENTWINDOW;
	}

	protected void RefreshFrame() {
		IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
		HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
		mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

		MARGINS margins = new() {
			cxLeftWidth = -1,
			cxRightWidth = -1,
			cyTopHeight = -1,
			cyBottomHeight = -1
		};

		ExtendFrame(mainWindowSrc.Handle, margins);
	}

	//[DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#132")] // Windows 1903 之后用不了。
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
					RefreshDarkMode();
					RaiseEvent(new RoutedEventArgs(ThemeChangeEvent, this));
				}
			return IntPtr.Zero;
		});
	}

	protected void RefreshDarkMode() {
		bool isDarkTheme = ShouldAppsUseDarkMode();
		int flag = isDarkTheme ? 1 : 0;
		SetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE, flag);
		SetCurrentThemeResource(isDarkTheme);
	}

	public DWM_SYSTEMBACKDROP_TYPE SystemBackdropType {
		set => SetSystemBackdropType(value);
	}

	protected void SetSystemBackdropType(DWM_SYSTEMBACKDROP_TYPE systemBackdropType) {
		SetWindowAttribute(Handle, DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE, (int)systemBackdropType);
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

	protected void SetCurrentThemeResource(bool isDarkTheme) {
		Resources.MergedDictionaries.Clear();
		Resources.MergedDictionaries.Add(new() { Source = new($"pack://application:,,,/OtomadHelper;component/Wpf/Styles/{(isDarkTheme ? "Dark" : "Light")}Theme.xaml", UriKind.Absolute) });
	}

	private void InitializeComponent() {
		Background = Brushes.Transparent;
		Loaded += Window_Loaded;
		WindowChrome.SetWindowChrome(this, new() {
			CaptionHeight = 20,
			CornerRadius = new(0),
			GlassFrameThickness = new(-1),
			ResizeBorderThickness = new(8),
			NonClientFrameEdges = NonClientFrameEdges.Right,
			UseAeroCaptionButtons = true,
		});
	}
}
