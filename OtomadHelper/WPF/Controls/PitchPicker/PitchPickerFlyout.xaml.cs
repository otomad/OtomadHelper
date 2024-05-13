using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// PitchPicker.xaml 的交互逻辑
/// </summary>
public partial class PitchPickerFlyout : BaseFlyout {
	public PitchPickerFlyout() {
		InitializeComponent();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		//this.GetChildrenOfType<ScrollViewer>().ForEach(scrollViewer =>
		//	scrollViewer.PreviewMouseWheel += (sender, e) => e.Handled = true);
	}

	public static PitchPickerFlyout Initial(Rect targetRect) {
		PitchPickerFlyout picker = new();
		picker.Loaded += (sender, e) => picker.CenterVertically(targetRect, SetWidthType.Width);
		return picker;
	}
}
