using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ParityEditor.xaml 的交互逻辑
/// </summary>
public partial class QuickSelectInterval2DEditor : UserControl {
	public QuickSelectInterval2DEditor() {
		InitializeComponent();
	}

	public new QuickSelectInterval2DEditorViewModel DataContext => (QuickSelectInterval2DEditorViewModel)base.DataContext;

	public static async Task<(bool ok, bool[,] bits, int rows, int columns, string name)> ShowDialog(bool[,] bits, string name = "") {
		QuickSelectInterval2DEditor panel = new();
		QuickSelectInterval2DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = new(bits);
		bool dialogResult = await ShowDialog(panel);
		if (!dialogResult) return (false, bits, bits.GetLength(0), bits.GetLength(1), name);
		return (true, viewModel.Bits.ToArray(), viewModel.Bits.Count, viewModel.Bits[0].Count, viewModel.Name);
	}

	public static async Task<(bool ok, string base64, string name)> ShowDialog(string base64, string name = "") {
		QuickSelectInterval2DEditor panel = new();
		QuickSelectInterval2DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = ObservableQuickSelectInterval2DCollection<bool>.FromBase64(base64);
		bool dialogResult = await ShowDialog(panel);
		if (!dialogResult) return (false, base64, name);
		return (true, viewModel.Bits.ToBase64(), viewModel.Name);
	}

	internal static async Task<bool> ShowDialog(FrameworkElement panel) {
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: "Quick Select Interval 2D Editor",
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			icon: KnownIcon.TableSimpleInclude,
			topmost: false,
			singletonId: QuickSelectInterval1DEditor.SingletonId,
			customize: dialog => {
				dialog.ResizeMode = ResizeMode.CanResize;
				dialog.SizeToContent = SizeToContent.Manual;
				dialog.Width = QuickSelectInterval1DEditor.DialogWidth;
				dialog.Height = QuickSelectInterval1DEditor.DialogHeight;
				dialog.MinWidth = QuickSelectInterval1DEditor.DialogMinWidth;
				dialog.MinHeight = QuickSelectInterval1DEditor.DialogMinHeight;
				dialog.MinimizeBox = false;
			}
		) ?? false;
		return dialogResult;
	}
}
