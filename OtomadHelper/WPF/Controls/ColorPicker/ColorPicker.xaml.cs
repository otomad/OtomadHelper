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

	public new ColorPickerViewModel DataContext => (ColorPickerViewModel)base.DataContext;

	public static async Task<Color> ShowDialog(Color color) { // TODO: 预计传入的颜色参数为 Hex。
		ColorPicker panel = new();
		ColorPickerViewModel viewModel = panel.DataContext;
		viewModel.Color = new(ColourSpace.Rgb255, color.R, color.G, color.B, color.A);
		bool dialogResult = await ContentDialog.ShowDialog<bool?>("Select a Color", panel, [
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Ok, true, true),
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Cancel, false),
		], "Color") ?? false;
		Unicolour newColor = viewModel.Color;
		return Color.FromArgb((byte)newColor.Alpha.A255, (byte)newColor.Rgb.Byte255.R, (byte)newColor.Rgb.Byte255.G, (byte)newColor.Rgb.Byte255.B);
	}

	private void TextBox_SourceUpdated(object sender, System.Windows.Data.DataTransferEventArgs e) {
		s = 123;
	}
}
