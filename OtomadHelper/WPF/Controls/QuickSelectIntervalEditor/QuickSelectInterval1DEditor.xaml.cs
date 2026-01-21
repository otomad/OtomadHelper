using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ParityEditor.xaml 的交互逻辑
/// </summary>
public partial class QuickSelectInterval1DEditor : UserControl {
	public QuickSelectInterval1DEditor() {
		InitializeComponent();
	}

	public new QuickSelectInterval1DEditorViewModel DataContext => (QuickSelectInterval1DEditorViewModel)base.DataContext;

	public static async Task<ValueTuple<bool, bool[]>> ShowDialog(bool[] bits) {
		QuickSelectInterval1DEditor panel = new();
		QuickSelectInterval1DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = bits;
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: "Quick Select Interval Editor",
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			singletonId: "Quick Select Interval Editor"
		) ?? false;
		if (!dialogResult) return (false, bits);
		return (true, (bool[])viewModel.Bits);
	}
}
