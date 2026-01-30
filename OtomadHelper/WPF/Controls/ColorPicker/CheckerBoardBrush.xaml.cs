using System.Windows;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

[AttachedDependencyProperty<SolidColorBrush, FrameworkElement>("Mix", DefaultValueExpression = nameof(TransparentBrush))]
[AttachedDependencyProperty<Brush, FrameworkElement>("Brush", DefaultValueExpression = nameof(TransparentBrush))]
public partial class CheckerBoardBrush {
	private static readonly SolidColorBrush TransparentBrush = new(Colors.Transparent);
	private static readonly RectangleGeometry GeometryFill = new(new(0, 0, 100, 100));

	static partial void OnMixChanged(FrameworkElement target, SolidColorBrush? mix) {
		mix ??= TransparentBrush;
		DrawingBrush source = (DrawingBrush)target.FindResource("CheckerBoard");
		DrawingGroup drawings = new();
		drawings.Children.AddRange([
			new GeometryDrawing(source, null, GeometryFill),
			new GeometryDrawing(mix, null, GeometryFill),
		]);
		DrawingBrush brush = new() {
			Stretch = source.Stretch,
			TileMode = source.TileMode,
			ViewportUnits = source.ViewportUnits,
			Viewport = source.Viewport,
			Drawing = drawings,
		};
		SetBrush(target, brush);
	}
}
