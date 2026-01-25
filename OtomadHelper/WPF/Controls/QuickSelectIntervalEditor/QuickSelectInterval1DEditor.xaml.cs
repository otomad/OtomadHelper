using System.Collections.ObjectModel;
using System.Windows;
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

	public static async Task<ValueTuple<bool, bool[], uint, string>> ShowDialog(bool[] bits, uint interval, string name = "") {
		QuickSelectInterval1DEditor panel = new();
		QuickSelectInterval1DEditorViewModel viewModel = panel.DataContext;
		viewModel.Bits = new(bits);
		bool dialogResult = await ShowDialog(panel);
		if (!dialogResult) return (false, bits, interval, name);
		return (true, viewModel.Bits.ToArray(), viewModel.Interval, viewModel.Name);
	}

	internal static async Task<bool> ShowDialog(FrameworkElement panel) {
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: "Quick Select Interval Editor",
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			icon: KnownIcon.None,
			topmost: false,
			singletonId: "Quick Select Interval Editor",
			customize: dialog => {
				dialog.ResizeMode = ResizeMode.CanResize;
				dialog.SizeToContent = SizeToContent.Manual;
				dialog.Width = 800;
				dialog.Height = 480;
				dialog.MinWidth = 400;
				dialog.MinHeight = 320;
				dialog.MinimizeBox = false;
			}
		) ?? false;
		return dialogResult;
	}

	public const double ToggleButtonSize = 36;
	public const double ToggleButtonSpacing = 4;
}
