using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

public class ClickLabelToFocusTextBoxBehavior : Behavior<FrameworkElement> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += TextBlock_MouseDown;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= TextBlock_MouseDown;
	}

	private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
		int currentIndex;
		if ((currentIndex = AssociatedObject.Index) == -1) return;
		UIElement? nextSibling = (AssociatedObject.Parent as Panel)?.Children.Cast<UIElement>().ElementAtOrDefault(currentIndex + 1);
		if (nextSibling is TextBox textBox)
			textBox.Focus();
	}
}
