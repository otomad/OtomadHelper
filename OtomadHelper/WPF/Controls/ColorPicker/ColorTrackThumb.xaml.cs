using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

using XY = (double X, double Y);

[DependencyProperty<double>("X", DefaultValue = 0, OnChanged = "OnChange")]
[DependencyProperty<double>("Y", DefaultValue = 0, OnChanged = "OnChange")]
[DependencyProperty<ColorTrackThumbDragAxis>("DragAxis", DefaultValue = ColorTrackThumbDragAxis.XY)]
[DependencyProperty<ValueTuple<double, double>>("XRange", DefaultValueExpression = "(0, 1)", OnChanged = "OnChange")]
[DependencyProperty<ValueTuple<double, double>>("YRange", DefaultValueExpression = "(0, 1)", OnChanged = "OnChange")]
[DependencyProperty<bool>("ReverseX", DefaultValue = false)]
[DependencyProperty<bool>("ReverseY", DefaultValue = false)]
[DependencyProperty<bool>("Round", DefaultValue = false, Description = "Set the value to the nearest integer.")]
[RoutedEvent("Dragging", RoutedEventStrategy.Bubble)]
public partial class ColorTrackThumb : Thumb {
	protected Canvas? Canvas { get; private set; }

	public ColorTrackThumb() {
		Loaded += (sender, e) => {
			DependencyObject? parent;
			do
				parent = this.GetParentObject();
			while (!(parent is Canvas || parent is null));
			Canvas = parent as Canvas;

			if (Canvas is not null) {
				OnChange(true);

				if (Canvas.Background is null) Canvas.Background = new SolidColorBrush(Colors.Transparent);
				Canvas.PreviewMouseDown += Canvas_MouseDown;
				Canvas.PreviewMouseMove += Canvas_MouseMove;
				Canvas.PreviewMouseUp += Canvas_MouseUp;
				Canvas.SizeChanged += (sender, e) => OnChange();
			}
		};
	}

	private Point offset;
	protected override void OnMouseDown(MouseButtonEventArgs e) {
		base.OnMouseDown(e);
		if (Canvas is null) return;
		offset = e.GetPosition(Canvas);
		offset.Y -= Canvas.GetTop(this).FiniteOrDefault(0);
		offset.X -= Canvas.GetLeft(this).FiniteOrDefault(0);
		CaptureMouse();
	}
	protected override void OnMouseMove(MouseEventArgs e) {
		base.OnMouseMove(e);
		if (Canvas is null || !IsMouseCaptured) return;
		Point position = e.GetPosition(Canvas);
		XY xy = SetPosition(position.X - offset.X + ActualWidth / 2, position.Y - offset.Y + ActualHeight / 2);
		RaiseDragging(xy);
	}
	protected override void OnMouseUp(MouseButtonEventArgs e) {
		ReleaseMouseCapture();
		base.OnMouseMove(e);
	}

	protected void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
		if (e.Source is not Canvas Canvas) return;
		offset = e.GetPosition(Canvas);
		Canvas.CaptureMouse();
		Canvas_MouseMove(sender, e);
		VisualStateManager.GoToState(this, "Pressed", true);
	}
	protected void Canvas_MouseMove(object sender, MouseEventArgs e) {
		if (e.Source is not Canvas Canvas) return;
		if (!Canvas.IsMouseCaptured) return;
		Point position = e.GetPosition(Canvas);
		XY xy = SetPosition(position.X, position.Y);
		RaiseDragging(xy);
	}
	protected void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
		if (e.Source is not Canvas Canvas) return;
		Canvas.ReleaseMouseCapture();
		VisualStateManager.GoToState(this, IsMouseOver ? "MouseOver" : "Normal", true);
	}

	private XY SetPosition(double x, double y, bool initial = false) {
		XY result = default;
		if (Canvas is null) return result;
		isOnChanging = true;
		x = MathEx.Clamp(x, 0, Canvas.ActualWidth);
		y = MathEx.Clamp(y, 0, Canvas.ActualHeight);
		(Range X, Range Y) range = CheckReverse();
		if (initial || (DragAxis & ColorTrackThumbDragAxis.X) != 0) {
			Canvas.SetLeft(this, x - ActualWidth / 2);
			result.X = MathEx.Map(x, 0, Canvas.ActualWidth, range.X.Min, range.X.Max);
			if (IsXBound) X = RoundIfNeed(result.X);
		}
		if (initial || (DragAxis & ColorTrackThumbDragAxis.Y) != 0) {
			Canvas.SetTop(this, y - ActualHeight / 2);
			result.Y = MathEx.Map(y, 0, Canvas.ActualHeight, range.Y.Min, range.Y.Max);
			if (IsYBound) Y = RoundIfNeed(result.Y);
		}
		isOnChanging = false;
		return result;
	}

	private double RoundIfNeed(double value) => Round ? Math.Round(value) : value;

	private bool IsXBound => GetBindingExpression(XProperty) != null;
	private bool IsYBound => GetBindingExpression(YProperty) != null;

	private void RaiseDragging(XY xy) =>
		RaiseEvent(new ColorTrackThumbDraggingRoutedEventArgs(DraggingEvent, this, Tag as string ?? "", xy.X, xy.Y));

	private bool isOnChanging = false;
	private void OnChange() => OnChange(false);
	private void OnChange(bool initial) {
		if (Canvas is null || isOnChanging) return;
		(Range X, Range Y) range = CheckReverse();
		SetPosition(
			MathEx.Map(X, range.X.Min, range.X.Max, 0, Canvas.ActualWidth),
			MathEx.Map(Y, range.Y.Min, range.Y.Max, 0, Canvas.ActualHeight),
			initial
		);
	}

	private (Range X, Range Y) CheckReverse() {
		Range x = XRange, y = YRange;
		if (ReverseX) (x.Min, x.Max) = (x.Max, x.Min);
		if (ReverseY) (y.Min, y.Max) = (y.Max, y.Min);
		return (x, y);
	}
}

[Flags]
public enum ColorTrackThumbDragAxis {
	None = 0,
	X = 1,
	Y = 2,
	XY = X | Y,
}

public class ColorTrackThumbDraggingRoutedEventArgs(RoutedEvent routedEvent, object source, string axis, double x, double y) :
	RoutedEventArgs(routedEvent, source) {
	public string Axis { get; } = axis;
	public double X { get; } = x;
	public double Y { get; } = y;
}
