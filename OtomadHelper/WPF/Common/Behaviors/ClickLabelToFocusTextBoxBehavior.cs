using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Xaml.Behaviors;

using OtomadHelper.Services;

namespace OtomadHelper.WPF.Common;

[DependencyProperty<ClickLabelToFocusTextBoxBehaviorModeType>("Mode", DefaultValue = ClickLabelToFocusTextBoxBehaviorModeType.Logical)]
public partial class ClickLabelToFocusTextBoxBehavior : Behavior<FrameworkElement> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += TextBlock_MouseDown;
		AssociatedObject.PreviewMouseUp += TextBlock_MouseUp;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= TextBlock_MouseDown;
		AssociatedObject.PreviewMouseUp -= TextBlock_MouseUp;
	}

	private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) => OnLabelPressed(true);
	private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e) => OnLabelPressed(false);

	private void OnLabelPressed(bool isMouseDown) {
		TextBox? textBox = null;
		RadioButton? radio = null;
		if ((radio = AssociatedObject as RadioButton) is not null && (radio.IsChecked != true || isMouseDown)) return;
		if (Mode == ClickLabelToFocusTextBoxBehaviorModeType.GridVisual && AssociatedObject.Parent is Grid grid) {
			(int column, int row) = grid.GetCellPosition(AssociatedObject);
			if ((textBox = grid.FindChildrenByCellPosition(column + 1, row).FirstOrDefault() as TextBox) is null)
				textBox = grid.FindChildrenByCellPosition(column + 2, row).FirstOrDefault() as TextBox;
		} else {
			int currentIndex;
			if ((currentIndex = AssociatedObject.Index) == -1) return;
			UIElement? nextSibling = (AssociatedObject.Parent as Panel)?.Children.Cast<UIElement>().ElementAtOrDefault(currentIndex + 1);
			textBox = nextSibling as TextBox;
		}
		if (radio is not null && textBox is not null) {
			ITimer.WPF.Timeout(() => textBox?.Focus(), 0);
		} else
			textBox?.Focus();
	}
}

public enum ClickLabelToFocusTextBoxBehaviorModeType {
	/// <summary>
	/// Find the next sibling directly.
	/// </summary>
	Logical,
	/// <summary>
	/// Find by iterate through all children of the grid that match the correct cell position (right).
	/// </summary>
	/// <remarks>
	/// It has a worse performance than <see cref="Logical" />.
	/// </remarks>
	GridVisual,
}
