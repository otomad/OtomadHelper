using System.Windows;
using System.Windows.Controls;
using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Test;

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
