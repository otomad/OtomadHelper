using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<NumberTextBoxInputMode>("NumberInputMode", DefaultValueExpression = "NumberTextBoxInputMode.Text")]
public partial class NumberTextBoxBehavior : Behavior<TextBox> {
	protected override void OnAttached() {
		AssociatedObject.PreviewTextInput += TextBox_TextInput;
		DataObject.AddPastingHandler(AssociatedObject, TextBox_Pasting);

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewTextInput -= TextBox_TextInput;
		DataObject.RemovePastingHandler(AssociatedObject, TextBox_Pasting);
	}

	private void TextBox_TextInput(object sender, TextCompositionEventArgs e) {
		if (NumberInputMode == NumberTextBoxInputMode.Text) return;
		string input = e.Text, original = AssociatedObject.Text;
		int caret = AssociatedObject.CaretIndex;
		bool allowed = IsTextAllowed(input, true);
		if (input == "." && original.Contains(".")) allowed = false;
		if (input == "-" && caret != 0) allowed = false;
		e.Handled = !allowed;
	}

	private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e) {
		if (NumberInputMode == NumberTextBoxInputMode.Text) return;
		if (e.DataObject.GetDataPresent(typeof(string))) {
			string text = (string)e.DataObject.GetData(typeof(string));
			bool allowed = IsTextAllowed(text, false);
			if (!allowed) e.CancelCommand();
		} else e.CancelCommand();
	}

	/// <summary>
	/// Check if the text is allowed to input to the text box.
	/// </summary>
	/// <param name="text">The text or character to be checked.</param>
	/// <param name="isInput">Is the text input from the keyboard instead of paste from clipboard?</param>
	public bool IsTextAllowed(string text, bool isInput) {
		Regex? regex = GetRegex(isInput);
		if (regex is null) return true;
		bool allowed = regex.IsMatch(text);
		return allowed;
	}

	private Regex? GetRegex(bool isInput) {
		return isInput ? NumberInputMode switch {
			NumberTextBoxInputMode.Integer => new(@"[\d]+"),
			NumberTextBoxInputMode.Decimal => new(@"[\d\.]+"),
			NumberTextBoxInputMode.SignedInteger => new(@"[\d-]+"),
			NumberTextBoxInputMode.SignedDecimal => new(@"[\d\.-]+"),
			_ => null,
		} : NumberInputMode switch {
			NumberTextBoxInputMode.Integer => new(@"^\d+$"),
			NumberTextBoxInputMode.Decimal => new(@"^\d+(\.\d+)?$"),
			NumberTextBoxInputMode.SignedInteger => new(@"^-?\d+$"),
			NumberTextBoxInputMode.SignedDecimal => new(@"^-?\d+(\.\d+)?$"),
			_ => null,
		};
	}
}

[Flags]
public enum NumberTextBoxInputMode {
	/// <remarks>Disabled</remarks>
	Text = -1,

	Digit = 0,
	Point = 1,
	Negative = 1 << 1,

	Integer = Digit,
	Decimal = Digit | Point,
	SignedInteger = Digit | Negative,
	SignedDecimal = Digit | Point | Negative,
}
