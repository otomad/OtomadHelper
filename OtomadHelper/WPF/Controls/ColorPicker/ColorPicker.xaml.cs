using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ColorPicker.xaml 的交互逻辑
/// </summary>
public partial class ColorPicker : UserControl {
	static ColorPicker() {
		_ = Configuration.Default;
	}

	public ColorPicker() {
		InitializeComponent();
	}

	private void OnLoaded(object sender, RoutedEventArgs e) {
		DataContext.View = this;
		DataContext.InitialColor();
		DataContext.UpdateThumbsBinding();
		ContentDialog?.SetNonDefaultButtonAccent(DataContext.Color.ToMediaColor());
	}

	public new ColorPickerViewModel DataContext => (ColorPickerViewModel)base.DataContext;

	public static async Task<string> ShowDialog(string hex) {
		bool startsWithHash = hex.StartsWith("#");
		Unicolour? color = ColorPickerViewModel.FromHex(hex);
		if (color is null) return hex;
		ColorPicker panel = new();
		ColorPickerViewModel viewModel = panel.DataContext;
		viewModel.Color = color;
		bool dialogResult = await ContentDialog.ShowDialog<bool?>((string)t.ColorPicker.Title, panel, [
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Ok, true, true),
			new ContentDialogButtonItem<bool>(t.ContentDialog.Button.Cancel, false),
		], "Color") ?? false;
		Unicolour newColor = viewModel.Color;
		if (!dialogResult) return hex;
		return (startsWithHash ? "#" : "") + ColorPickerViewModel.ToHex(newColor)[0];
	}

	public TElement? FindForm<TElement>(string tag) where TElement : FrameworkElement {
		foreach (UIElement element in Form.Children)
			if (element is TElement el && el.Tag is string Tag && Tag == tag)
				return el;
		return null;
	}

	internal void SetAccentColor(Color? color = null) {
		color ??= DataContext.Color.ToMediaColor();
		if (ContentDialog is not null)
			ContentDialog.CustomAccentColor = color;
	}

	private ContentDialog? contentDialog;
	private ContentDialog ContentDialog => contentDialog ??= (Window.GetWindow(this) as ContentDialog)!;
}
