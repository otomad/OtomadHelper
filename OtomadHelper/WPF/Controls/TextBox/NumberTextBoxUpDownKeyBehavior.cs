using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

[AttachedDependencyProperty<double>("Step", DefaultValue = 1)]
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
		if (e.Key is Key.Up or Key.Down)
			e.Handled = Spin(AssociatedObject, e.Key == Key.Up ? 1 : -1);
	}

	public static bool Spin(TextBox textBox, int direction) {
		double step = GetStep(textBox);
		if (step == 0) return false;
		if (NumberTextBoxBehavior.GetNumberInputMode(textBox) == NumberTextBoxInputMode.Text) return false;
		string text = textBox.Text;
		if (!double.TryParse(string.IsNullOrEmpty(text) ? "0" : text, out double value)) return false;
		if (direction == 0) return false;

		value += direction * step;
		textBox.SetCurrentValue(TextBox.TextProperty, value.ToString());
		textBox.CaretIndex = int.MaxValue;
		return true;
	}
}
