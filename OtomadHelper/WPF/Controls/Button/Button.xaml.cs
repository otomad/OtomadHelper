using System.Windows;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Button.xaml 的交互逻辑
/// </summary>
public class Button : System.Windows.Controls.Button {
	public static readonly DependencyProperty DialogResultProperty = DependencyProperty.Register(
		nameof(DialogResult), typeof(object), typeof(Button), new(""));
	public object DialogResult { get => GetValue(DialogResultProperty); set => SetValue(DialogResultProperty, value); }
}
