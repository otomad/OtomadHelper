using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<double>("Step", DefaultValue = 1)]
public partial class NumberTextBoxUpDownKeyBehavior : Behavior<TextBox> {
	protected override void OnAttached() {
		AssociatedObject.PreviewKeyDown += TextBox_KeyDown;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewKeyDown -= TextBox_KeyDown;
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e) {
		if (Step == 0) return;
		if (NumberTextBoxBehavior.GetNumberInputMode(AssociatedObject) == NumberTextBoxInputMode.Text) return;
		string text = AssociatedObject.Text;
		if (!double.TryParse(string.IsNullOrEmpty(text) ? "0" : text, out double value)) return;
		if (e.Key is Key.Up or Key.Down) {
			value += (e.Key == Key.Up ? 1 : -1) * Step;
			AssociatedObject.SetCurrentValue(TextBox.TextProperty, value.ToString());
			AssociatedObject.CaretIndex = int.MaxValue;
			e.Handled = true;
		}
	}
}
