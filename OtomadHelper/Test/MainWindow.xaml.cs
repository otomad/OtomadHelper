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

namespace OtomadHelper.Test {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		private IntPtr Handle => new WindowInteropHelper(this).Handle;

		public MainWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			RefreshFrame();
			RefreshDarkMode();
			//ThemeManager.Current.ActualApplicationThemeChanged += (_, _) => RefreshDarkMode();

			SetWindowAttribute(Handle,
				DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
				3);
		}

		private void RefreshFrame() {
			IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
			HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
			mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

			MARGINS margins = new MARGINS();
			margins.cxLeftWidth = -1;
			margins.cxRightWidth = -1;
			margins.cyTopHeight = -1;
			margins.cyBottomHeight = -1;

			ExtendFrame(mainWindowSrc.Handle, margins);
		}

		private void RefreshDarkMode() {
			var isDark = true; // ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark;
			int flag = isDark ? 1 : 0;
			SetWindowAttribute(Handle,
				DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE,
				flag);
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e) {
			int flag = int.Parse((string)((RadioButton)sender).Tag);
			SetWindowAttribute(Handle,
				DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
				flag);
		}
	}
}
