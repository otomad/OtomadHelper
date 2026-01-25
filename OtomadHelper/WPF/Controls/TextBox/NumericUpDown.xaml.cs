using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

[AttachedDependencyProperty<bool, TextBox>("ShowSpinner", DefaultValue = false)]
public partial class NumericUpDown : TextBox {
	public NumericUpDown() {
		InitializeComponent();
	}
}
