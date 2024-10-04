using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<double>("X", DefaultValue = 0, OnChanged = "OnChange")]
[DependencyProperty<double>("Y", DefaultValue = 0, OnChanged = "OnChange")]
[DependencyProperty<ColorTrackThumbDragAxis>("DragAxis", DefaultValue = ColorTrackThumbDragAxis.XY)]
[DependencyProperty<ValueTuple<double, double>>("XRange", DefaultValueExpression = "(0, 1)", OnChanged = "OnChange")]
[DependencyProperty<ValueTuple<double, double>>("YRange", DefaultValueExpression = "(0, 1)", OnChanged = "OnChange")]
[DependencyProperty<bool>("ReverseX", DefaultValue = false)]
[DependencyProperty<bool>("ReverseY", DefaultValue = false)]
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
		SetPosition(position.X - offset.X, position.Y - offset.Y);
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
	}
	protected void Canvas_MouseMove(object sender, MouseEventArgs e) {
		if (e.Source is not Canvas Canvas) return;
		if (!Canvas.IsMouseCaptured) return;
		Point position = e.GetPosition(Canvas);
		SetPosition(position.X - ActualWidth / 2, position.Y - ActualHeight / 2);
	}
	protected void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
		if (e.Source is not Canvas Canvas) return;
		Canvas.ReleaseMouseCapture();
	}

	private void SetPosition(double x, double y, bool initial = false) {
		if (Canvas is null) return;
		isOnChanging = true;
		x = MathEx.Clamp(x, -ActualWidth / 2, Canvas.ActualWidth - ActualWidth / 2);
		y = MathEx.Clamp(y, -ActualHeight / 2, Canvas.ActualHeight - ActualHeight / 2);
		(Range X, Range Y) range = CheckReverse();
		if (initial || (DragAxis & ColorTrackThumbDragAxis.X) != 0) {
			Canvas.SetLeft(this, x);
			if (IsXBound) X = MathEx.Map(x, 0, Canvas.ActualWidth, range.X.Min, range.X.Max);
		}
		if (initial || (DragAxis & ColorTrackThumbDragAxis.Y) != 0) {
			Canvas.SetTop(this, y);
			if (IsYBound) Y = MathEx.Map(y, 0, Canvas.ActualHeight, range.Y.Min, range.Y.Max);
		}
		isOnChanging = false;
	}

	private bool IsXBound => GetBindingExpression(XProperty) != null;
	private bool IsYBound => GetBindingExpression(YProperty) != null;

	private bool isOnChanging = false;
	private void OnChange() => OnChange(false);
	private void OnChange(bool initial) {
		if (Canvas is null || isOnChanging) return;
		(Range X, Range Y) range = CheckReverse();
		SetPosition(
			MathEx.Map(X, range.X.Min, range.X.Max, 0, Canvas.ActualWidth) - ActualWidth / 2,
			MathEx.Map(Y, range.Y.Min, range.Y.Max, 0, Canvas.ActualHeight) - ActualHeight / 2,
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
