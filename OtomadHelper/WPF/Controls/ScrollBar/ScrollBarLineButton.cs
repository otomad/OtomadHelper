using System.Windows.Controls.Primitives;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<ScrollBarLineButtonArrowPoint>("ArrowPoint", DefaultValue = ScrollBarLineButtonArrowPoint.Up)]
[DependencyProperty<double>("ArrowRotation", DefaultValue = 0, IsReadOnly = true)]
[DependencyProperty<double>("ArrowScale", DefaultValue = 1)]
public partial class ScrollBarLineButton : RepeatButton {
	partial void OnArrowPointChanged(ScrollBarLineButtonArrowPoint arrowPoint) => ArrowRotation = (double)arrowPoint;
}

public enum ScrollBarLineButtonArrowPoint {
	Up = 0,
	Right = 90,
	Down = 180,
	Left = 270,
}
