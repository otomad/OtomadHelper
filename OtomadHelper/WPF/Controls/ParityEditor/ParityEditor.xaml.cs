using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ParityEditor.xaml 的交互逻辑
/// </summary>
public partial class ParityEditor : UserControl {
	public ParityEditor() {
		InitializeComponent();
	}

	public new ParityEditorViewModel DataContext => (ParityEditorViewModel)base.DataContext;

	public static async Task<ValueTuple<bool, bool[]>> ShowDialog(bool[] bits) {
		ParityEditor panel = new();
		ParityEditorViewModel<bool[]> viewModel = panel.DataContext;
		viewModel.Bits = bits;
		bool dialogResult = await ContentDialog.ShowDialog<bool?>(
			title: "Parity Editor",
			content: panel,
			buttons: (ContentDialogButtonItem<bool>[])[
				new(t.ContentDialog.Button.Ok, true, true),
				new(t.ContentDialog.Button.Cancel, false),
			],
			singletonId: "Parity Editor"
		) ?? false;
		if (!dialogResult) return (false, bits);
		return (true, (bool[])viewModel.Bits);
	}
}
