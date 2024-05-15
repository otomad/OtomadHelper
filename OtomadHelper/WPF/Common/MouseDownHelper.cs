using System.Windows;

namespace OtomadHelper.WPF.Common;

/// <remarks>
/// <example>
/// <code>
/// <![CDATA[
/// <Setter Property="m:MouseDownHelper.Enabled" Value="True" />
/// <Style.Triggers>
///     <Trigger Property="m:MouseDownHelper.IsLeftPressed" Value="True">
///         <Setter Property="..." Value="..." />
///     </Trigger>
/// </Style.Triggers>
/// ]]>
/// </code>
/// </example>
/// </remarks>
public static class MouseDownHelper {
	public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached("Enabled",
		typeof(bool), typeof(MouseDownHelper), new(false, OnEnabledChanged));

	public static void SetEnabled(UIElement element, bool value) => element.SetValue(EnabledProperty, value);
	public static bool GetEnabled(UIElement element) => (bool)element.GetValue(EnabledProperty);

	private static void OnEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		if (sender is not UIElement element || e.NewValue is not bool enabled) return;
		if (enabled) Register(element);
		else UnRegister(element);
	}

	private static void Register(UIElement element) {
		element.PreviewMouseDown += Element_MouseDown;
		element.PreviewMouseLeftButtonDown += Element_MouseLeftButtonDown;
		element.MouseLeave += Element_MouseLeave;
		element.PreviewMouseUp += Element_MouseUp;
	}

	private static void UnRegister(UIElement element) {
		element.PreviewMouseDown -= Element_MouseDown;
		element.PreviewMouseLeftButtonDown -= Element_MouseLeftButtonDown;
		element.MouseLeave -= Element_MouseLeave;
		element.PreviewMouseUp -= Element_MouseUp;
	}

	private static void Element_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
		if (sender is UIElement element) SetIsPressed(element, true);
	}

	private static void Element_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {
		if (sender is UIElement element) SetIsLeftPressed(element, true);
	}

	private static void Element_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
		if (sender is UIElement element) {
			SetIsPressed(element, false);
			SetIsLeftPressed(element, false);
		}
	}

	private static void Element_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
		if (sender is UIElement element) {
			SetIsPressed(element, false);
			SetIsLeftPressed(element, false);
		}
	}

	internal static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterAttachedReadOnly
		("IsPressed", typeof(bool), typeof(MouseDownHelper), new(false));
	public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

	internal static void SetIsPressed(UIElement element, bool value) => element.SetValue(IsPressedPropertyKey, value);
	public static bool GetIsPressed(UIElement element) => (bool)element.GetValue(IsPressedProperty);

	internal static readonly DependencyPropertyKey IsLeftPressedPropertyKey = DependencyProperty.RegisterAttachedReadOnly
		("IsLeftPressed", typeof(bool), typeof(MouseDownHelper), new(false));
	public static readonly DependencyProperty IsLeftPressedProperty = IsLeftPressedPropertyKey.DependencyProperty;

	internal static void SetIsLeftPressed(UIElement element, bool value) => element.SetValue(IsLeftPressedPropertyKey, value);
	public static bool GetIsLeftPressed(UIElement element) => (bool)element.GetValue(IsLeftPressedProperty);
}
