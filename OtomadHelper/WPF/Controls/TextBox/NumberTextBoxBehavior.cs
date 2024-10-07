using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

[AttachedDependencyProperty<NumberTextBoxInputMode, TextBox>("NumberInputMode", DefaultValueExpression = "NumberTextBoxInputMode.Text")]
[AttachedDependencyProperty<ValueTuple<double, double>?, TextBox>("Range", TypeConverter = typeof(TupleConverter<ValueTuple<double, double>>), Validate = true)]
[AttachedDependencyProperty<int, TextBox>("MaxLength", DefaultValue = int.MaxValue)]
public partial class NumberTextBoxBehavior : Behavior<TextBox> {
	protected override void OnAttached() {
		AssociatedObject.PreviewTextInput += TextBox_TextInput;
		DataObject.AddPastingHandler(AssociatedObject, TextBox_Pasting);
		AssociatedObject.TextChanged += TextBox_TextChanged;
		AssociatedObject.PreviewKeyDown += TextBox_KeyDown;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewTextInput -= TextBox_TextInput;
		DataObject.RemovePastingHandler(AssociatedObject, TextBox_Pasting);
		AssociatedObject.TextChanged -= TextBox_TextChanged;
		AssociatedObject.PreviewKeyDown -= TextBox_KeyDown;
	}

	private NumberTextBoxInputMode NumberInputMode => GetNumberInputMode(AssociatedObject);
	private Range? Range => GetRange(AssociatedObject);
	private int MaxLength => GetMaxLength(AssociatedObject);

	private int caretIndex = int.MaxValue;
	private int CaretIndex {
		get => caretIndex;
		set => caretIndex = Math.Max(0, value);
	}

	private void TextBox_TextInput(object sender, TextCompositionEventArgs e) {
		if (NumberInputMode == NumberTextBoxInputMode.Text) return;
		string input = e.Text, original = AssociatedObject.Text;
		int caret = AssociatedObject.CaretIndex;
		bool allowed = IsTextAllowed(input, true);
		if (input == "." && original.Contains(".")) allowed = false;
		if (input == "-" && caret != 0) allowed = false;
		if (ShouldConstrainTextLength(input.Length)) allowed = false;
		e.Handled = !allowed;
		CaretIndex = caret + input.Length;
	}

	private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e) {
		if (NumberInputMode == NumberTextBoxInputMode.Text) return;
		if (e.DataObject.GetDataPresent(typeof(string))) {
			string text = (string)e.DataObject.GetData(typeof(string));
			bool allowed = IsTextAllowed(text, false);
			if (ShouldConstrainTextLength(text.Length)) allowed = false;
			if (!allowed) e.CancelCommand();
			else CaretIndex = AssociatedObject.CaretIndex + text.Length;
		} else e.CancelCommand();
	}

	private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {
		AssociatedObject.CaretIndex = CaretIndex;
		if (NumberInputMode == NumberTextBoxInputMode.Text) return;
		string text = AssociatedObject.Text;
		if (string.IsNullOrEmpty(text)) return;
		if (NumberInputMode == NumberTextBoxInputMode.Hex) {
			KeepCaretAndSetText(text.ToUpperInvariant());
			return;
		}
		if (!double.TryParse(text, out double value) || Range is not Range range) return;
		if (value < range.Min || value > range.Max) {
			value = value < range.Min ? range.Min : range.Max;
			KeepCaretAndSetText(value);
		}
	}

	private void KeepCaretAndSetText(object value) {
		CaretIndex = AssociatedObject.CaretIndex;
		AssociatedObject.SetCurrentValue(TextBox.TextProperty, value.ToString());
		AssociatedObject.CaretIndex = CaretIndex;
	}

	private bool ShouldConstrainTextLength(int textLengthToAdd) {
		if (MaxLength > 0 && MaxLength != int.MaxValue)
			if (AssociatedObject.Text.Length + textLengthToAdd > MaxLength)
				return true;
		return false;
	}

	private void TextBox_KeyDown(object sender, KeyEventArgs e) {
		if (e.Key is Key.Back or Key.Delete || e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			CaretIndex = AssociatedObject.SelectionLength > 0 ? AssociatedObject.CaretIndex : AssociatedObject.CaretIndex - 1;
	}

	private static partial bool IsRangeValid((double, double)? value) => value is not Range range || range.Min <= range.Max;

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
			NumberTextBoxInputMode.Hex => new(@"[\dA-F]+", RegexOptions.IgnoreCase),
			_ => null,
		} : NumberInputMode switch {
			NumberTextBoxInputMode.Integer => new(@"^\d+$"),
			NumberTextBoxInputMode.Decimal => new(@"^\d+(\.\d+)?$"),
			NumberTextBoxInputMode.SignedInteger => new(@"^-?\d+$"),
			NumberTextBoxInputMode.SignedDecimal => new(@"^-?\d+(\.\d+)?$"),
			NumberTextBoxInputMode.Hex => new(@"^[\dA-F]+$", RegexOptions.IgnoreCase),
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
	Hex = 1 << 2,

	Integer = Digit,
	Decimal = Digit | Point,
	SignedInteger = Digit | Negative,
	SignedDecimal = Digit | Point | Negative,
}
