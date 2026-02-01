using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using OtomadHelper.Interop;

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
		DataContext.OriginalColor = DataContext.Color;
		DataContext.InitialColor();
		DataContext.UpdateThumbsBinding();
		ContentDialog?.SetNonDefaultButtonAccent(DataContext.Color.ToMediaColor());
		ContentDialog?.DataContext.Header = Header;
	}

	public new ColorPickerViewModel DataContext => (ColorPickerViewModel)base.DataContext;

	// TODO: Try to convert `ValueTuple<bool, string>` to `[bool ok, string color]`.
	// CAUTION: Async method cannot use `out` parameter. So return a value tuple type instead.
	public static async Task<ValueTuple<bool, string>> ShowDialog(string hex, ColorPickerModelAxis? initialModelAxis = null) {
		bool startsWithHash = hex.StartsWith("#");
		Unicolour? color = Unicolour.FromHex(hex);
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
			icon: KnownIcon.Color,
			singletonId: "Color Picker"
		) ?? false;
		Unicolour newColor = viewModel.Color;
		if (!dialogResult) return (false, hex);
		return (true, (startsWithHash ? "#" : "") + ColorPickerViewModel.ToHexes(newColor)[0]);
	}

	public TElement? FindForm<TElement>(ColorPickerModelAxis modelAxis) where TElement : FrameworkElement {
		foreach (UIElement element in Form.Children)
			if (element is TElement el && GetModelAxis(el) == modelAxis)
				return el;
		return null;
	}

	internal Color? DialogAccentColor {
		set {
			Color? color = DataContext.Color.ToMediaColor();
			ContentDialog?.CustomAccentColor = color;
		}
	}

	internal ColorPickerModelAxis ColorSpaceDisplayName {
		set => Header.ModelAxis = value.DisplayName;
	}

	internal Unicolour ColorDisplayName {
		set => Header.Color = ColorDisplayNameHelper.ToDisplayName(value.ToMediaColor());
	}

	private ContentDialog ContentDialog => field ??= (Window.GetWindow(this) as ContentDialog)!;
	private ColorDisplayNameHeader Header { get; } = new();
}
