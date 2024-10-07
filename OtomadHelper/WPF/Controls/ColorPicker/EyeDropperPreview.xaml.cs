using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// EyeDropperPreview.xaml 的交互逻辑
/// </summary>
[DependencyProperty<Color>("PointColor", DefaultValueExpression = "System.Windows.Media.Colors.White")]
public partial class EyeDropperPreview : Window {
	public EyeDropperPreview() {
		InitializeComponent();
	}

	internal void MoveToMouse(Point point, (double X, double Y) dpi) {
		//Point point = PointToScreen(Mouse.GetPosition(this));
		Left = point.X / dpi.X - ActualWidth / 2;
		Top = point.Y / dpi.Y - ActualHeight / 2;
	}

	public new void Hide() {
		Storyboard hideAnimation = (Storyboard)Resources["Hide"];
		hideAnimation.Completed += (sender, e) => base.Hide();
		BeginStoryboard(hideAnimation);
	}
}
