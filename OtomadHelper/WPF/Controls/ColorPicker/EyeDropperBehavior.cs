using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

using Color = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;
using Point = System.Windows.Point;
using DrawingPoint = System.Drawing.Point;

namespace OtomadHelper.WPF.Controls;

public class EyeDropperBehavior : Behavior<Button> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += Button_MouseDown;
		AssociatedObject.PreviewMouseMove += Button_MouseMove;
		AssociatedObject.PreviewMouseUp += Button_MouseUp;
		AssociatedObject.LostMouseCapture += Button_LostMouseCapture;
		Preview.MouseMove += Button_MouseMove;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= Button_MouseDown;
		AssociatedObject.PreviewMouseMove -= Button_MouseMove;
		AssociatedObject.PreviewMouseUp -= Button_MouseUp;
		AssociatedObject.LostMouseCapture -= Button_LostMouseCapture;
		Preview.MouseMove -= Button_MouseMove;
	}

	private Window Window => field ??= Window.GetWindow(AssociatedObject);
	private double CurrentWindowLeft { get; set; }
	private const double InvisibleWindowLeft = 65536; // TODO: -32000 (minimize to taskbar position)

	private EyeDropperPreview Preview { get; } = new();

	private void Button_MouseDown(object sender, MouseButtonEventArgs e) {
		if (AssociatedObject.IsMouseCaptured) {
			Button_LostMouseCapture();
			return;
		}
		AssociatedObject.CaptureMouse();
		CurrentWindowLeft = Window.Left;
		Window.Left += InvisibleWindowLeft;
		Mouse.OverrideCursor = Cursors.Cross;
		Preview.Show();
		Button_MouseMove(sender, e);
	}

	private void Button_MouseMove(object sender, MouseEventArgs e) {
		if (!AssociatedObject.IsMouseCaptured) return;
		DrawingPoint position = CursorPosition;
		Visual source = PresentationSource.FromVisual(Preview) is not null ? Preview : Window;
		Preview.MoveToMouse(position, source.Dpi);
		Color color = GetColorAt(position);
		Preview.PointColor = color;
	}

	private void Button_MouseUp(object sender, MouseButtonEventArgs e) {
		if (AssociatedObject.IsMouseCaptured) {
			DrawingPoint position = CursorPosition;
			Color color = GetColorAt(position, true);
			AssociatedObject.RaiseEvent(new GetColorRoutedEventArgs(color, GetColorEvent, AssociatedObject));
		}
		Button_LostMouseCapture();
	}

	private void Button_LostMouseCapture() => Button_LostMouseCapture(null!, null!);
	[SuppressMessage("ReSharper", "AsyncVoidMethod")]
	[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
	private async void Button_LostMouseCapture(object sender, MouseEventArgs e) {
		if (sender is not null) {
			await Task.Delay(5);
			if (AssociatedObject.IsMouseCaptured) return;
		}
		Window.Left = CurrentWindowLeft;
		Mouse.OverrideCursor = null;
		Preview.Hide();
		AssociatedObject.ReleaseMouseCapture();
	}

	private static DrawingPoint CursorPosition => System.Windows.Forms.Cursor.Position;
	// Do not use MouseEventArgs to get mouse position like below, or cannot get negative mouse position.
	// private Point GetPoint(MouseEventArgs e) => Window.PointToScreen(e.GetPosition(Window));

	// ReSharper disable once InconsistentNaming
	private static Color GetColorAt(DrawingPoint point, bool debug_isMouseUp = false) {
		point = ScalePointByDpiAware(point);
		//if (debug_isMouseUp) Debug_ScreenShot(point);
		return GetColorAt(point.X, point.Y).ToMediaColor();
	}

	private static DrawingColor GetColorAt(int x, int y) {
		using Bitmap bmp = new(1, 1);
		Rectangle bounds = new(x, y, 1, 1);
		using (Graphics g = Graphics.FromImage(bmp))
			g.CopyFromScreen(bounds.Location, DrawingPoint.Empty, bounds.Size);
		return bmp.GetPixel(0, 0);
	}

	private static DrawingPoint ScalePointByDpiAware(DrawingPoint point) {
		Screen screen = Screen.FromPoint(point);
		Rectangle bounds = screen.Bounds;
		int width = bounds.Width, height = bounds.Height, left = bounds.Left, top = bounds.Top;
		Rectangle realBounds = screen.GetPhysicalBounds();
		int realWidth = realBounds.Width, realHeight = realBounds.Height, realLeft = realBounds.Left, realTop = realBounds.Top;
		return new(
			(int)(((double)point.X - left) / width * realWidth + realLeft),
			(int)(((double)point.Y - top) / height * realHeight + realTop)
		);
	}

	private static void Debug_ScreenShot(DrawingPoint point) {
		using Bitmap bmp = new(3840, 3240);
		Rectangle bounds = new(point.X, point.Y, bmp.Width, bmp.Height);
		using (Graphics g = Graphics.FromImage(bmp))
			g.CopyFromScreen(bounds.Location, DrawingPoint.Empty, bounds.Size);
		bmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\screenshot.png");
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
