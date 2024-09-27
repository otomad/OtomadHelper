using System.Windows;

namespace OtomadHelper.WPF.Common;

/// <remarks>
/// Usage:
/// <example>
/// <code>
/// <![CDATA[
/// <Style TargetType="Grid">
///     <Setter Property="m:AttachedEvents.RaiseIsVisibleChanged" Value="True" />
///     <EventSetter Event="m:AttachedEvents.IsVisibleChanged" Handler="OnIsVisibleChanged" />
/// </Style>
/// ]]>
/// </code>
/// </example>
/// </remarks>
[AttachedDependencyProperty<bool, UIElement>("RaiseIsVisibleChanged", DefaultValue = false, OnChanged = "OnRaiseIsVisibleChanged")]
public static partial class AttachedEvents {
	public static readonly RoutedEvent IsVisibleChangedEvent = EventManager.RegisterRoutedEvent(
		"IsVisibleChanged",
		RoutingStrategy.Bubble,
		typeof(RoutedPropertyChangedEventHandler<bool>),
		typeof(AttachedEvents)
	);

	public static void AddIsVisibleChangedHandler(DependencyObject dependencyObject, RoutedPropertyChangedEventHandler<bool> handler) {
		if (dependencyObject is not UIElement uiElement) return;
		uiElement.AddHandler(IsVisibleChangedEvent, handler);
	}

	private static void RaiseIsVisibleChangedEvent(object sender, DependencyPropertyChangedEventArgs e) {
		((UIElement)sender).RaiseEvent(new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue, IsVisibleChangedEvent));
	}

	public static void RemoveIsVisibleChangedHandler(DependencyObject dependencyObject, RoutedPropertyChangedEventHandler<bool> handler) {
		if (dependencyObject is not UIElement uiElement) return;
		uiElement.RemoveHandler(IsVisibleChangedEvent, handler);
	}

	private static void OnRaiseIsVisibleChanged(UIElement uiElement, bool newValue) {
		if (newValue)
			uiElement.IsVisibleChanged += RaiseIsVisibleChangedEvent;
		else
			uiElement.IsVisibleChanged -= RaiseIsVisibleChangedEvent;
	}
}
