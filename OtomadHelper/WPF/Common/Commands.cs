using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using OtomadHelper.WPF.Controls;

namespace OtomadHelper.WPF.Common;

public static class Commands {
	public static readonly RoutedUICommand ClearAll = new(
		name: "ClearAll",
		text: t.TextBox.Menu.ClearAll, // TODO: Possible to dynamic change i18n?
		inputGestures: [new KeyGesture(Key.None, ModifierKeys.None, "Ctrl+A, Del")],
		ownerType: typeof(Commands)
	);
	public static readonly RoutedUICommand Increase = new(
		name: "Increase",
		text: t.TextBox.NumericUpDown.Increase,
		inputGestures: [new KeyGesture(Key.Up)],
		ownerType: typeof(Commands)
	);
	public static readonly RoutedUICommand Decrease = new(
		name: "Decrease",
		text: t.TextBox.NumericUpDown.Decrease,
		inputGestures: [new KeyGesture(Key.Up)],
		ownerType: typeof(Commands)
	);

	internal static readonly CommandBinding[] CommandBindings = [
		Create(ApplicationCommands.Delete, static (RoutedEventArgs e, ref bool canExecute) => {
			if (e.OriginalSource is not TextBox textBox) return null;
			canExecute = textBox.IsEditable && textBox.SelectionLength > 0;
			return () => textBox.SelectedText = string.Empty;
		}),
		Create(ClearAll, static (RoutedEventArgs e, ref bool canExecute) => {
			if (e.OriginalSource is not TextBox textBox) return null;
			canExecute = textBox.IsEditable && textBox.Text.Length > 0;
			return () => textBox.Clear();
		}),
		Create(Increase, static (RoutedEventArgs e, ref bool canExecute) => {
			if (NumericUpDown.GetTextBoxFromSpinnerRepeatButton(e.OriginalSource) is not TextBox textBox) return null;
			canExecute = NumberTextBoxBehavior.GetNumberInputMode(textBox) != NumberTextBoxInputMode.Text;
			return () => {
				textBox.Focus();
				NumberTextBoxUpDownKeyBehavior.Spin(textBox, 1);
			};
		}),
		Create(Decrease, static (RoutedEventArgs e, ref bool canExecute) => {
			if (NumericUpDown.GetTextBoxFromSpinnerRepeatButton(e.OriginalSource) is not TextBox textBox) return null;
			canExecute = NumberTextBoxBehavior.GetNumberInputMode(textBox) != NumberTextBoxInputMode.Text;
			return () => {
				textBox.Focus();
				NumberTextBoxUpDownKeyBehavior.Spin(textBox, -1);
			};
		}),
	];

	private delegate Action? CommandBindingHandler(RoutedEventArgs e, ref bool canExecute);
	private static CommandBinding Create(ICommand command, CommandBindingHandler Handler) =>
		new(
			command: command,
			executed: (_, e) => {
				bool canExecute = false;
				Action? execute = Handler(e, ref canExecute);
				if (canExecute) execute?.Invoke();
			},
			canExecute: (_, e) => {
				bool canExecute = e.CanExecute;
				Handler(e, ref canExecute);
				e.CanExecute = canExecute;
			}
		);
}
