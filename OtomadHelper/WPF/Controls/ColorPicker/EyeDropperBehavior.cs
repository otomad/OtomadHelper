using System.Drawing;
using System.Windows;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using DrawingColor = System.Drawing.Color;
using DrawingPoint = System.Drawing.Point;

namespace OtomadHelper.WPF.Controls;

public class EyeDropperBehavior : Behavior<Button> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += Button_MouseDown;
		AssociatedObject.PreviewMouseMove += Button_MouseMove;
		AssociatedObject.PreviewMouseUp += Button_MouseUp;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= Button_MouseDown;
		AssociatedObject.PreviewMouseMove -= Button_MouseMove;
		AssociatedObject.PreviewMouseUp -= Button_MouseUp;
	}

	private Window? window;
	private Window Window => window ??= Window.GetWindow(AssociatedObject);
	private double CurrentWindowLeft { get; set; }
	private const double InvisibleWindowLeft = 65536;

	private void Button_MouseDown(object sender, MouseButtonEventArgs e) {
		AssociatedObject.CaptureMouse();
		CurrentWindowLeft = Window.Left;
		Window.Left = InvisibleWindowLeft;
		Mouse.OverrideCursor = Cursors.Cross;
	}

	private void Button_MouseMove(object sender, MouseEventArgs e) {
		if (!AssociatedObject.IsMouseCaptured) return;
		//System.Windows.Point position = GetPoint(e);
		//position.Offset(65536, 0);
		//s = position;
	}

	private void Button_MouseUp(object sender, MouseButtonEventArgs e) {
		if (AssociatedObject.IsMouseCaptured) {
			Point position = GetPoint(e);
			Color color = GetColorAt(position);
			AssociatedObject.RaiseEvent(new GetColorRoutedEventArgs(color, GetColorEvent, AssociatedObject));
		}
		Window.Left = CurrentWindowLeft;
		Mouse.OverrideCursor = null;
		AssociatedObject.ReleaseMouseCapture();
	}

	private Point GetPoint(MouseEventArgs e) =>
		Window.PointToScreen(e.GetPosition(Window));

	private static Color GetColorAt(Point point) =>
		GetColorAt((int)point.X, (int)point.Y).ToMediaColor();

	private static DrawingColor GetColorAt(int x, int y) {
		Bitmap bmp = new(1, 1);
		Rectangle bounds = new(x, y, 1, 1);
		using (Graphics g = Graphics.FromImage(bmp))
			g.CopyFromScreen(bounds.Location, DrawingPoint.Empty, bounds.Size);
		return bmp.GetPixel(0, 0);
	}

	public static readonly RoutedEvent GetColorEvent = EventManager.RegisterRoutedEvent(nameof(GetColorEvent),
		RoutingStrategy.Bubble, typeof(GetColorRoutedEventHandler), typeof(EyeDropperBehavior));
	public static void AddGetColorHandler(DependencyObject d, RoutedEventHandler handler) =>
		(d as UIElement)?.AddHandler(GetColorEvent, handler);
	public static void RemoveGetColorHandler(DependencyObject d, RoutedEventHandler handler) =>
		(d as UIElement)?.RemoveHandler(GetColorEvent, handler);
	public delegate void GetColorRoutedEventHandler(object sender, GetColorRoutedEventArgs e);
	public class GetColorRoutedEventArgs(Color color, RoutedEvent @event, object source) : RoutedEventArgs(@event, source) {
		public Color Color { get; } = color;
	}
}
