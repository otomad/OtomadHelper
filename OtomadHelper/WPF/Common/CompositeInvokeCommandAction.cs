using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

// Source - https://stackoverflow.com/a/70876872
// Posted by thatguy
// Retrieved 2026-01-25, License - CC BY-SA 4.0

[DependencyProperty<ICommand>("Command")]
[DependencyProperty<object>("CommandParameter")]
[DependencyProperty<IValueConverter>("EventArgsConverter")]
[DependencyProperty<object>("EventArgsConverterParameter")]
[DependencyProperty<string>("EventArgsParameterPath")]
public sealed partial class CompositeInvokeCommandAction : TriggerAction<DependencyObject> {
	private string commandName = null!;

	/// <summary>
	/// Gets or sets the name of the command this action should invoke.
	/// </summary>
	/// <value>The name of the command this action should invoke.</value>
	/// <remarks>This property will be superseded by the Command property if both are set.</remarks>
	public string CommandName {
		get {
			ReadPreamble();
			return commandName;
		}
		set {
			if (CommandName != value) {
				WritePreamble();
				commandName = value;
				WritePostscript();
			}
		}
	}

	/// <summary>
	/// Specifies whether the EventArgs of the event that triggered this action should be passed to the Command as a parameter.
	/// </summary>
	/// <remarks>If the <see cref="Command"/>, <see cref="EventArgsParameterPath"/>, or <see cref="EventArgsConverter"/> properties are set, this property is ignored.</remarks>
	public bool PassEventArgsToCommand { get; set; } = true;

	/// <summary>
	/// Invokes the action.
	/// </summary>
	/// <param name="parameter">The parameter to the action. If the action does not require a parameter, the parameter may be set to a null reference.</param>
	protected override void Invoke(object parameter) {
		if (AssociatedObject is not null) {
			ICommand? command = ResolveCommand();

			if (command is not null) {
				object? eventArgs = null;

				if (!string.IsNullOrWhiteSpace(EventArgsParameterPath))
					eventArgs = GetEventArgsPropertyPathValue(parameter);

				if (eventArgs is null && EventArgsConverter is not null)
					eventArgs = EventArgsConverter.Convert(parameter, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentCulture);

				if (eventArgs is null && PassEventArgsToCommand)
					eventArgs = parameter;

				CompositeCommandParameter compositeCommandParameter = new(CommandParameter, eventArgs);
				if (command.CanExecute(compositeCommandParameter))
					command.Execute(compositeCommandParameter);
			}
		}
	}

	private object GetEventArgsPropertyPathValue(object parameter) {
		object commandParameter;
		object propertyValue = parameter;
		string[] propertyPathParts = EventArgsParameterPath!.Split('.');
		foreach (string propertyPathPart in propertyPathParts) {
			PropertyInfo propInfo = propertyValue.GetType().GetProperty(propertyPathPart);
			propertyValue = propInfo.GetValue(propertyValue, null);
		}

		commandParameter = propertyValue;
		return commandParameter;
	}

	private ICommand? ResolveCommand() {
		ICommand? command = null;

		if (Command is not null) command = Command;
		else if (AssociatedObject is not null) {
			// todo jekelly 06/09/08: we could potentially cache some or all of this information if needed, updating when AssociatedObject changes
			Type associatedObjectType = AssociatedObject.GetType();
			PropertyInfo[] typeProperties = associatedObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (PropertyInfo propertyInfo in typeProperties)
				if (typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType))
					if (string.Equals(propertyInfo.Name, CommandName, StringComparison.Ordinal))
						command = (ICommand)propertyInfo.GetValue(AssociatedObject, null);
		}

		return command;
	}

	//private CompositeCommandParameter CreateCompositeCommandParameter(ICommand command) {
	//	if (command is CommunityToolkit.Mvvm.Input.RelayCommand<int>) { }
	//}
}

public class CompositeCommandParameter(object? parameter, object? eventArgs) {
	public dynamic Parameter { get; } = parameter!;
	public dynamic EventArgs { get; } = eventArgs!;
	public void Deconstruct(out object parameter, out object eventArgs) {
		parameter = Parameter;
		eventArgs = EventArgs;
	}
}
