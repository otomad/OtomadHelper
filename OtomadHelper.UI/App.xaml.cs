using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using WinUICommunity.Common.Helpers;
using WinUICommunity.Common.Tools;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OtomadHelper.UI;
/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application {
	internal MainWindow mainWindow;
	internal AppWindow appWindow;
	internal ThemeManager themeManager;
	internal static new App Current { get { return Application.Current as App; } }
	internal static MainWindow MainWindow { get { return App.Current.mainWindow; } }
	internal WindowHelper windowHelper;
	public static List<string> Args { get; private set; }
	public static AppStartMode StartMode { get; private set; }
	public static IntPtr ServerHwnd { get; private set; }

	/// <summary>
	/// Initializes the singleton application object.  This is the first line of authored code
	/// executed, and as such is the logical equivalent of main() or WinMain().
	/// </summary>
	public App() {
		InitializeComponent();
	}

	/// <summary>
	/// Invoked when the application is launched normally by the end user.
	/// Other entry points will be used such as when the application is launched to open a specific file.
	/// </summary>
	/// <param name="args">Details about the launch request and process.</param>
	protected override void OnLaunched(LaunchActivatedEventArgs args) {
		Args = Environment.GetCommandLineArgs().ToList();
		if (Args.Count <= 1) {
			StartMode = AppStartMode.Manual;
			//return;
			goto StartApp; // 生产环境换成上面的 return。
		}
		int paramIndex =  Args.IndexOf("-OtomadHelper");
		if (paramIndex == -1 || paramIndex >= Args.Count - 1) {
			StartMode = AppStartMode.Abnormal;
			return;
		}
		string serverHwnd_Str = Args[paramIndex + 1];
		bool ok = int.TryParse(serverHwnd_Str, out int serverHwnd_Int);
		if (!ok) {
			StartMode = AppStartMode.Abnormal;
			return;
		}
		ServerHwnd = (IntPtr)serverHwnd_Int;
		StartMode = AppStartMode.Awake;

	StartApp:
		mainWindow = new MainWindow();
		mainWindow.SizeChanged += SizeChanged;
		appWindow = GetAppWindow(mainWindow); // Set ExtendsContentIntoTitleBar for the AppWindow not the window
		appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
		appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
		appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
		themeManager = ThemeManager.Initialize(mainWindow, WinUICommunity.Common.Helpers.BackdropType.DesktopAcrylic);
		mainWindow.Activate();
		windowHelper = new WindowHelper(mainWindow);
		windowHelper.Received += text => mainWindow.Received = text;
	}

	private void SizeChanged(object sender, WindowSizeChangedEventArgs args) {
		// Update the title bar draggable region. We need to indent from the left both for the nav back button and to avoid the system menu
		double dpi = 2; // TODO: 暂时不知道 WinUI 3 怎么找 DPI。
		Windows.Graphics.RectInt32[] rects = new Windows.Graphics.RectInt32[] { new Windows.Graphics.RectInt32(48, 0, (int)((args.Size.Width - 48) * dpi), 48) };
		appWindow.TitleBar.SetDragRectangles(rects);
	}

	private static AppWindow GetAppWindow(Window window) {
		IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
		WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
		return AppWindow.GetFromWindowId(windowId);
	}
}
