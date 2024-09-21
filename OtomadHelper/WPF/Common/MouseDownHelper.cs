/// <summary>
/// <see href="https://stackoverflow.com/a/10667643/19553213" />
/// </summary>

using System.Windows;
using System.Windows.Input;

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
[AttachedDependencyProperty<bool, UIElement>("IsPressed", DefaultValue = false, IsReadOnly = true)]
[AttachedDependencyProperty<bool, UIElement>("IsLeftPressed", DefaultValue = false, IsReadOnly = true)]
public static partial class MouseDownHelper {
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

	private static void Element_MouseDown(object sender, MouseButtonEventArgs e) {
		if (sender is UIElement element) SetIsPressed(element, true);
	}

	private static void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
		if (sender is UIElement element) SetIsLeftPressed(element, true);
	}

	private static void Element_MouseLeave(object sender, MouseEventArgs e) {
		if (sender is UIElement element) {
			SetIsPressed(element, false);
			SetIsLeftPressed(element, false);
		}
	}

	private static void Element_MouseUp(object sender, MouseButtonEventArgs e) {
		if (sender is UIElement element) {
			SetIsPressed(element, false);
			SetIsLeftPressed(element, false);
		}
	}
}
