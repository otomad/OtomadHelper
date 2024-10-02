using Microsoft.Xaml.Behaviors;
using System.Windows.Input;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<RoutedEvent>("RoutedEvent")]
[DependencyProperty<ICommand>("Command")]
[DependencyProperty<string>("EventArgsParameterPath")]
[DependencyProperty<IValueConverter>("EventArgsConverter")]
[DependencyProperty<object>("EventArgsConverterParameter")]
[DependencyProperty<object>("CommandParameter")]
public partial class EventToCommandBehavior : Behavior<FrameworkElement> {
	private readonly RoutedEventHandler handler;

	public EventToCommandBehavior() {
		handler = (sender, e) => {
			object? args = CommandParameter;

			if (args is null && !string.IsNullOrWhiteSpace(EventArgsParameterPath))
				args = e.GetPath(EventArgsParameterPath!);

			if (args is null && EventArgsConverter is not null)
				args = EventArgsConverter.Convert(e, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentCulture);

			args ??= e;

			if (Command?.CanExecute(args) == true)
				Command.Execute(args);
		};
	}

	protected override void OnAttached() {
		base.OnAttached();
		AssociatedObject.AddHandler(RoutedEvent, handler);
	}

	protected override void OnDetaching() {
		base.OnDetaching();
		AssociatedObject.RemoveHandler(RoutedEvent, handler);
	}
}
