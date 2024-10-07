using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Button.xaml 的交互逻辑
/// </summary>
[DependencyProperty<object>("DialogResult", DefaultValue = "")]
[DependencyProperty<Color?>("Accent")]
[DependencyProperty<double>("BackgroundOpacity", DefaultValue = 1)]
public partial class Button : System.Windows.Controls.Button {

}
