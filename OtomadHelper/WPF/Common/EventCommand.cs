using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Event command.
/// </summary>
public class EventCommand : TriggerAction<DependencyObject> { // BUG: Vegas doesn't support.
	protected override void Invoke(object parameter) {
		if (CommandParameter != null)
			parameter = CommandParameter;
		if (Command != null)
			Command.Execute(parameter);
	}

	/// <summary>
	/// Event.
	/// </summary>
	public ICommand Command {
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}
	public static readonly DependencyProperty CommandProperty =
		DependencyProperty.Register("Command", typeof(ICommand), typeof(EventCommand), new PropertyMetadata(null));

	/// <summary>
	/// Event argument. If empty, the native event argument will be automatically passed in.
	/// </summary>
	public object CommandParameter {
		get => GetValue(CommandParameterProperty);
		set => SetValue(CommandParameterProperty, value);
	}
	public static readonly DependencyProperty CommandParameterProperty =
		DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventCommand), new PropertyMetadata(null));
}
