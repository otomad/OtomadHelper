using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OtomadHelper.WPF.Controls;
/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public ContentDialog() {
		InitializeComponent();
	}

	public new static bool? Show() {
		ContentDialog dialog = new();
		return dialog.ShowDialog();
	}

	private void Button_Click(object sender, RoutedEventArgs e) {
		Close();
	}
}
