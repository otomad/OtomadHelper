using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Xaml.Behaviors;

namespace OtomadHelper.WPF.Controls;

/// <remarks>
/// <see href="https://stackoverflow.com/a/64393305/19553213"/>
/// <para>Currently only fix the click padding issue, and drag to select text is not supported yet.</para>
/// </remarks>
public class TextBoxSelectionBehavior : Behavior<Border> {
	protected override void OnAttached() {
		AssociatedObject.PreviewMouseDown += PreviewMouseDown;

		base.OnAttached();
	}

	protected override void OnDetaching() {
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= PreviewMouseDown;
	}

	private void PreviewMouseDown(object sender, MouseButtonEventArgs e) {
		if (e.Source is not TextBox textBox || IsTextBoxView(e.OriginalSource)) return;
		if (e.MiddleButton == MouseButtonState.Pressed) return;
		if (e.RightButton == MouseButtonState.Pressed && textBox.SelectionLength > 0) return;
		Border border = AssociatedObject;
		Thickness padding = border.Padding;
		Point position = e.GetPosition(border);
		if (position.X >= border.ActualWidth - padding.Right - TEXT_BOX_INNER_PADDING_X)
			textBox.CaretIndex = int.MaxValue;
		else if (position.X <= padding.Left + TEXT_BOX_INNER_PADDING_X)
			textBox.CaretIndex = 0;
	}

	private const int TEXT_BOX_INNER_PADDING_X = 4;

	/// <summary>
	/// Test a object is a <see cref="TextBoxView" /> instance.
	/// </summary>
	/// <remarks>
	/// The class <see cref="TextBoxView" /> is internal so we cannot get it directly.
	/// </remarks>
	/// <param name="test">The object to be tested.</param>
	private static bool IsTextBoxView(object? test) =>
		test is not null && test.GetType().FullName == "System.Windows.Controls.TextBoxView";
}
