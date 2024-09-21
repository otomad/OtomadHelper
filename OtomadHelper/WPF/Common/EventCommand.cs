using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Event command.
/// </summary>
[DependencyProperty<ICommand>("Command", Description = "Event.")]
[DependencyProperty<object>("CommandParameter", Description = "Event argument. If empty, the native event argument will be automatically passed in.")]
public partial class EventCommand : TriggerAction<DependencyObject> {
	protected override void Invoke(object parameter) {
		if (CommandParameter != null) parameter = CommandParameter;
		Command?.Execute(parameter);
	}
}
