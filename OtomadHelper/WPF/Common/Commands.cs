using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtomadHelper.WPF.Common;

public static class Commands {
	public static readonly RoutedUICommand ClearAll = new(
		name: "ClearAll",
		text: "Clear All", // TODO: i18n
		inputGestures: [new KeyGesture(Key.None, ModifierKeys.None, "Ctrl+A, Del")],
		ownerType: typeof(Commands)
	);

	internal static readonly CommandBinding[] CommandBindings = [
		Create(ApplicationCommands.Delete, static (RoutedEventArgs e, ref bool canExecute) => {
			if (e.Source is not TextBox textBox) return null;
			canExecute = textBox.IsEditable() && textBox.SelectionLength > 0;
			return () => textBox.SelectedText = string.Empty;
		}),
		Create(ClearAll, static (RoutedEventArgs e, ref bool canExecute) => {
			if (e.Source is not TextBox textBox) return null;
			canExecute = textBox.IsEditable() && textBox.Text.Length > 0;
			return () => textBox.Clear();
		}),
	];

	private delegate Action? CommandBindingHandler(RoutedEventArgs e, ref bool canExecute);
	private static CommandBinding Create(ICommand command, CommandBindingHandler Handler) =>
		new(
			command: command,
			executed: (sender, e) => {
				bool canExecute = false;
				Action? execute = Handler(e, ref canExecute);
				if (canExecute) execute?.Invoke();
			},
			canExecute: (sender, e) => {
				bool canExecute = e.CanExecute;
				Handler(e, ref canExecute);
				e.CanExecute = canExecute;
			}
		);
}
