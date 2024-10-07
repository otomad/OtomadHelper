using Microsoft.Xaml.Behaviors;

using System.Windows;

namespace OtomadHelper.WPF.Common;

/// <remarks>
/// <example>
/// <code>
/// <![CDATA[
/// <i:Interaction.Triggers>
///     <local:RoutedEventTrigger RoutedEvent="{x:Static ext:ClickExtensions.ClickEvent}">
///         <i:InvokeCommandAction Command="{Binding ClickCommand}" />
///     </local:RoutedEventTrigger>
/// </i:Interaction.Triggers>
/// ]]>
/// </code>
/// </example>
/// <see href="https://github.com/microsoft/XamlBehaviorsWpf/issues/110" />
/// </remarks>
public class RoutedEventTrigger : EventTriggerBase<FrameworkElement> {
	public RoutedEvent? RoutedEvent { get; set; }

	protected override void OnAttached() {
		if (RoutedEvent is not null)
			Source.AddHandler(RoutedEvent, new RoutedEventHandler(OnRoutedEvent));
	}

	protected void OnRoutedEvent(object sender, RoutedEventArgs args) => base.OnEvent(args);

	protected override string GetEventName() => RoutedEvent?.Name ?? string.Empty;
}
