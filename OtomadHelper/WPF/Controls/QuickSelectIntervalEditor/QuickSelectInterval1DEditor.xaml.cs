using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ParityEditor.xaml 的交互逻辑
/// </summary>
[AttachedDependencyProperty<Rect, VisualBrush>("TileShadowViewport")]
public partial class QuickSelectInterval1DEditor : UserControl {
	public QuickSelectInterval1DEditor() {
		InitializeComponent();
	}

	public new QuickSelectInterval1DEditorViewModel DataContext => (QuickSelectInterval1DEditorViewModel)base.DataContext;

	public static async Task<(bool ok, bool[] bits, int interval, string name)> ShowDialog(bool[] bits, string name = "") {
		QuickSelectInterval1DEditor panel = new();
		QuickSelectInterval1DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = new(bits);
		bool dialogResult = await ShowDialog(panel);
		if (!dialogResult) return (false, bits, bits.Length, name);
		return (true, viewModel.Bits.ToArray(), viewModel.Bits.Count, viewModel.Name);
	}

	public static async Task<(bool ok, string base64, string name)> ShowDialog(string base64, string name = "") {
		QuickSelectInterval1DEditor panel = new();
		QuickSelectInterval1DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = ObservableQuickSelectIntervalCollection<bool>.FromBase64(base64);
		bool dialogResult = await ShowDialog(panel);
		if (!dialogResult) return (false, base64, name);
		return (true, viewModel.Bits.ToBase64(), viewModel.Name);
	}

	internal const string SingletonId = "Quick Select Interval Editor";

	internal static async Task<bool> ShowDialog(FrameworkElement panel) {
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: "Quick Select Interval Editor",
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			icon: KnownIcon.SkipForwardInterval,
			singletonId: SingletonId,
			customize: CustomizeDialog
		) ?? false;
		return dialogResult;
	}

	internal static void CustomizeDialog(ContentDialog dialog) {
		dialog.Topmost = false;
		dialog.ResizeMode = ResizeMode.CanResize;
		dialog.SizeToContent = SizeToContent.Manual;
		dialog.MinimizeBox = false;
		dialog.Width = 800;
		dialog.Height = 510;
		dialog.MinWidth = 400;
		dialog.MinHeight = 398.5;
	}

	static partial void OnTileShadowViewportChanged(VisualBrush brush, Rect viewport) {
		brush.Opacity = 0.35;
		brush.Stretch = Stretch.None;
		brush.TileMode = TileMode.Tile;
		brush.Viewport = viewport;
		brush.ViewportUnits = BrushMappingMode.Absolute;
		brush.AlignmentX = AlignmentX.Left;
		brush.AlignmentY = AlignmentY.Top;
	}

	public const double ToggleButtonSize = 36;
	public const double ToggleButtonSpacing = 4;
}
