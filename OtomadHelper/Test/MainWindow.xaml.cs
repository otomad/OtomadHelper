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
using OtomadHelper.Controls;

namespace OtomadHelper.Test {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : BackdropWindow {
		public MainWindow() {
			InitializeComponent();
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e) {
			DWM_SYSTEMBACKDROP_TYPE flag = (DWM_SYSTEMBACKDROP_TYPE)int.Parse((string)((RadioButton)sender).Tag);
			SystemBackdropType = flag;
		}
	}
}
