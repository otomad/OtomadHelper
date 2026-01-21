using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Common;

public class ClickLabelToFocusTextBoxBehavior : Behavior<TextBlock> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += TextBlock_MouseDown;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= TextBlock_MouseDown;
	}

	private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) {
		if (AssociatedObject.Parent is Panel parent) {
			UIElementCollection children = parent.Children;
			int currentIndex = children.IndexOf(AssociatedObject);
			if (currentIndex > -1 && currentIndex < children.Count - 1) {
				UIElement nextSibling = children[currentIndex + 1];
				if (nextSibling is TextBox textBox)
					textBox.Focus();
			}
		}
	}
}
