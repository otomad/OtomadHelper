using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

public class SnapLayoutButtonBehavior : Behavior<Button> {
	// WINAPI:
	private const int WM_NCHITTEST = 0x0084; // InteropValues
	private const int WM_NCLBUTTONDOWN = 0x00A1;
	private const int WM_NCLBUTTONUP = 0x00A2;
	private const int HTMAXBUTTON = 9;

	private HwndSource? hwndSource;

	/// <summary>
	/// This needs to be called in this.Loaded event.
	/// </summary>
	protected override void OnAttached() {
		base.OnAttached();
		Window? window = Window.GetWindow(AssociatedObject);
		if (WindowsVersion.Current < WindowsNT.Windows11_Dev || window is null) return;

		hwndSource = PresentationSource.FromVisual(AssociatedObject) as HwndSource;
		hwndSource?.AddHook(HwndSourceHook);
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		hwndSource?.RemoveHook(HwndSourceHook);
		hwndSource = null;
	}

	private bool IsCursorOnButton(IntPtr lparam, FrameworkElement button) {
		// Extract mouse coordinates from lparam
		int mouseX = (short)(lparam.ToInt32() & 0xFFFF);
		int mouseY = (short)((lparam.ToInt32() >> 16) & 0xFFFF);

		// Get button's actual dimensions and position
		Point buttonPosition = button.PointToScreen(new(0, 0));

		(double dpiX, double dpiY) = AssociatedObject.Dpi;

		// Check if mouse coordinates are within the button bounds using a single return statement
		return
			mouseX >= buttonPosition.X && mouseX <= buttonPosition.X + button.ActualWidth * dpiX &&
			mouseY >= buttonPosition.Y && mouseY <= buttonPosition.Y + button.ActualHeight * dpiY;
	}

	private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled) {
		// https://learn.microsoft.com/en-us/windows/apps/desktop/modernize/apply-snap-layout-menu
		// https://github.com/dotnet/wpf/issues/4825
		switch (msg) {
			case WM_NCHITTEST:
				if (IsCursorOnButton(lparam, AssociatedObject)) {
					SetButtonState(AssociatedObject, isMouseOver: true);
					handled = true;
					return new IntPtr(HTMAXBUTTON);
				} else
					SetButtonState(AssociatedObject, isMouseOver: false, isPressed: false);
				break;

			case WM_NCLBUTTONDOWN:
				if (IsCursorOnButton(lparam, AssociatedObject)) {
					SetButtonState(AssociatedObject, isPressed: true);
					handled = true;
				}
				break;

			case WM_NCLBUTTONUP:
				if (IsCursorOnButton(lparam, AssociatedObject)) {
					GetButtonState(AssociatedObject, out _, out bool? wasPressed);
					SetButtonState(AssociatedObject, isPressed: false);
					handled = true;
					if (wasPressed == true) // Fire click
						AssociatedObject.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
				}
				break;
			default:
				break;
		}

		return IntPtr.Zero;
	}

	#region Helpers
	// internal dependency properties
	private static readonly DependencyPropertyKey uiElementIsMouseOverPropertyKey =
		(DependencyPropertyKey)typeof(UIElement).GetField("IsMouseOverPropertyKey", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

	private static readonly DependencyPropertyKey buttonIsPressedPropertyKey =
		(DependencyPropertyKey)typeof(ButtonBase).GetField("IsPressedPropertyKey", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

	private static void GetButtonState(UIElement button, out bool isMouseOver, out bool? isPressed) {
		isMouseOver = (bool)button.GetValue(uiElementIsMouseOverPropertyKey.DependencyProperty);
		isPressed = null;
		if (button is ButtonBase)
			isPressed = (bool)button.GetValue(buttonIsPressedPropertyKey.DependencyProperty);
	}

	private static void SetButtonState(UIElement button, bool? isMouseOver = null, bool? isPressed = null) {
		if (isMouseOver.HasValue)
			button.SetValue(uiElementIsMouseOverPropertyKey, isMouseOver.Value);

		if (isPressed.HasValue && button is ButtonBase)
			button.SetValue(buttonIsPressedPropertyKey, isPressed.Value);

		if (button is FrameworkElement element) {
			// refresh actual states
			GetButtonState(button, out bool mouseOver, out isPressed);
			isMouseOver = mouseOver;

			string state = isPressed == true ? "Pressed" : isMouseOver == true ? "MouseOver" : "Normal";

			// apply visual state (for styling to work)
			VisualStateManager.GoToState(element, state, true);
		}
	}
	#endregion
}
