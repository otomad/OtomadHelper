using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

[AttachedDependencyProperty<bool, TextBox>("ShowSpinner", DefaultValue = false)]
public partial class NumericUpDown : TextBox {
	public NumericUpDown() {
		InitializeComponent();
	}

	internal static TextBox? GetTextBoxFromSpinnerRepeatButton(object source) {
		TextBox? textBox;
		if ((textBox = source as TextBox) is null)
			if ((textBox = (source as FrameworkElement)?.TemplatedParent as TextBox) is null)
				return null;
		return textBox;
	}
}
