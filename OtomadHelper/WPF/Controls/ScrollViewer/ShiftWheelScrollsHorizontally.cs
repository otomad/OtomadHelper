using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Controls;

public class ShiftWheelScrollsHorizontally : Behavior<ScrollViewer> {
	protected override void OnAttached() {
		base.OnAttached();
		AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
	}

	protected override void OnDetaching() {
		base.OnDetaching();
		AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
	}

	private void AssociatedObject_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e) {
		if (Keyboard.Modifiers == ModifierKeys.Shift ||
			AssociatedObject.ComputedHorizontalScrollBarVisibility == Visibility.Visible &&
			AssociatedObject.ComputedVerticalScrollBarVisibility != Visibility.Visible) {
			int hScrollLines = MouseScrollSettings.GetHorizontalScrollChars();
			while (hScrollLines-- > 0)
				if (e.Delta < 0) AssociatedObject.LineRight();
				else AssociatedObject.LineLeft();
			e.Handled = true;
		}
	}
}
