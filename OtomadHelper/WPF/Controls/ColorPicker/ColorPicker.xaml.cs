using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ColorPicker.xaml 的交互逻辑
/// </summary>
public partial class ColorPicker : UserControl {
	public ColorPicker() {
		InitializeComponent();
	}

	private void OnLoaded(object sender, RoutedEventArgs e) {
		DataContext.View = this;
	}

	public new ColorPickerViewModel DataContext => (ColorPickerViewModel)base.DataContext;

	public static async Task<Color> ShowDialog(Color color) { // TODO: 预计传入的颜色参数为 Hex。
		ColorPicker panel = new();
		ColorPickerViewModel viewModel = panel.DataContext;
		viewModel.Color = color.ToUnicolour();
		bool dialogResult = await ContentDialog.ShowDialog<bool?>("Select a Color", panel, [
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Ok, true, true),
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Cancel, false),
		], "Color") ?? false;
		Unicolour newColor = viewModel.Color;
		return newColor.ToMediaColor();
	}
}
