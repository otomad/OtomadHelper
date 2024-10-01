using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

public class TextBoxSelectionBehavior : Behavior<TextBox> {// BUG:
	public const double MAX_DISTANCE = 10;
	private Point position;
	private int start;
	private int cur;
	private double right;
	private bool drag;
	private const double MAX_DRAG = 12;

	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += V_PreviewMouseDown;
		AssociatedObject.PreviewMouseUp += V_PreviewMouseUp;
		AssociatedObject.PreviewMouseMove += V_PreviewMouseMove;
		AssociatedObject.LostFocus += V_LostFocus;
		AssociatedObject.IsKeyboardFocusedChanged += V_IsKeyboardFocusedChanged;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= V_PreviewMouseDown;
		AssociatedObject.PreviewMouseUp -= V_PreviewMouseUp;
		AssociatedObject.PreviewMouseMove -= V_PreviewMouseMove;
		AssociatedObject.LostFocus -= V_LostFocus;
		AssociatedObject.IsKeyboardFocusedChanged -= V_IsKeyboardFocusedChanged;
	}

	public static readonly DependencyProperty ClickSelectsProperty = DependencyProperty.RegisterAttached("ClickSelects", typeof(bool), typeof(TextBoxSelectionBehavior), new UIPropertyMetadata(true));
	public static bool GetClickSelects(FrameworkElement obj) => (bool)obj.GetValue(ClickSelectsProperty);
	public static void SetClickSelects(FrameworkElement obj, bool value) => obj.SetValue(ClickSelectsProperty, value);

	private void V_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
		if (e.ChangedButton != MouseButton.Left) return;
		AssociatedObject.ReleaseMouseCapture();
		if (e.OriginalSource is Grid g && VisualTreeHelper.GetParent(g) is FrameworkElement { Name: "PART_ContentHost" }) {
			position = e.GetPosition(AssociatedObject);
			FrameworkElement r = (FrameworkElement)AssociatedObject.GetChildOfType<ScrollContentPresenter>()!.Content;
			right = r.TranslatePoint(new Point(r.ActualWidth, 0), AssociatedObject).X;
			cur = start = AssociatedObject.GetCharacterIndexFromPoint(position, true) + (position.X >= right ? 1 : 0);
			AssociatedObject.SelectionStart = start;
			AssociatedObject.SelectionLength = 0;
			AssociatedObject.Focus();
			AssociatedObject.CaptureMouse();
			drag = false;
			e.Handled = true;
		}
	}
	private void V_PreviewMouseMove(object sender, MouseEventArgs e) {
		Point p = e.GetPosition(AssociatedObject);
		cur = AssociatedObject.GetCharacterIndexFromPoint(p, true);
		AssociatedObject.SelectionStart = Math.Min(start, cur);
		AssociatedObject.SelectionLength = Math.Abs(cur - start) + (p.X >= right ? position.X >= right ? -1 : 1 : 0);
		if (!drag) drag = Math.Max(Math.Abs(position.X - p.X), Math.Abs(position.Y - p.Y)) > MAX_DRAG;
		e.Handled = true;
	}
	private void V_PreviewMouseUp(object sender, MouseButtonEventArgs e) {
		AssociatedObject.ReleaseMouseCapture();
		if (!drag && GetClickSelects(AssociatedObject)) AssociatedObject.SelectAll();
		e.Handled = true;
	}
	private void V_LostFocus(object sender, RoutedEventArgs e) {
		AssociatedObject.ReleaseMouseCapture();
	}
	private void V_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e) {
		if ((bool)e.NewValue) return;
		AssociatedObject.ReleaseMouseCapture();
	}
}
