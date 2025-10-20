using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ColorPicker.xaml 的交互逻辑
/// </summary>
[AttachedDependencyProperty<ColorPickerModelAxis>("ModelAxis", DefaultValueExpression = "null")]
public partial class ColorPicker : UserControl {
	[MethodImpl(MethodImplOptions.NoOptimization)]
	static ColorPicker() {
		// Only needed once, makes first Unicolour initialize faster.
		// https://github.com/waacton/Unicolour/issues/4#issuecomment-2396548329
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

	public static async Task<ValueTuple<bool, string>> ShowDialog(string hex, ColorPickerModelAxis? initialModelAxis = null) {
		bool startsWithHash = hex.StartsWith("#");
		Unicolour? color = ColorPickerViewModel.FromHex(hex);
		if (color is null) return (false, hex);
		ColorPicker panel = new();
		ColorPickerViewModel viewModel = panel.DataContext;
		viewModel.Color = color;
		if (ColorPickerModelAxis.Valid(initialModelAxis)) viewModel.ModelAxis = initialModelAxis;
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: t.ColorPicker.Title,
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			iconName: "Color",
			singletonId: "Color Picker"
		) ?? false;
		Unicolour newColor = viewModel.Color;
		if (!dialogResult) return (false, hex);
		return (true, (startsWithHash ? "#" : "") + ColorPickerViewModel.ToHex(newColor)[0]);
	}

	public TElement? FindForm<TElement>(ColorPickerModelAxis modelAxis) where TElement : FrameworkElement {
		foreach (UIElement element in Form.Children)
			if (element is TElement el && GetModelAxis(el) == modelAxis)
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
