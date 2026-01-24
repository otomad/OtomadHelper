using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Automatically adding horizontal or vertical spacing in <see cref="StackPanel" /> and other list like <see cref="Panel" />s.
/// </summary>
/// <remarks>
/// <para>
/// Currently, only <see cref="StackPanel" /> and <see cref="UniformGrid" /> are supported,
/// but <see cref="Grid" /> and other type of <see cref="Panel" />s are not supported.
/// </para>
/// <see href="https://gist.github.com/angularsen/90040fb174f71c5ab3ad" />
/// <example>
/// <code>
/// <![CDATA[
/// <StackPanel Orientation="Horizontal" m:Spacing.Horizontal="5">
///     <Button>Button 1</Button>
///     <Button>Button 2</Button>
/// </StackPanel>
///
/// <StackPanel Orientation="Vertical" m:Spacing.Vertical="5">
///     <Button>Button 1</Button>
///     <Button>Button 2</Button>
/// </StackPanel>
///
/// <!-- MarginSetter is deprecated. -->
/// <StackPanel Orientation="Vertical" m:MarginSetter.Margin="5" m:MarginSetter.LastItemMargin="5 0">
///     <Button>Button 1</Button>
///     <Button>Button 2</Button>
/// </StackPanel>
/// ]]>
/// </code>
/// </example>
/// </remarks>
[AttachedDependencyProperty<double, Panel>("Horizontal", DefaultValue = 0d, OnChanged = "OnSpacingChanged")]
[AttachedDependencyProperty<double, Panel>("Vertical", DefaultValue = 0d, OnChanged = "OnSpacingChanged")]
[AttachedDependencyProperty<bool>("Exclude", DefaultValue = false)]
public partial class Spacing {
	protected static void OnSpacingChanged(Panel panel) {
		// Avoid duplicate registrations.
		panel.Loaded -= OnPanelLoaded;
		panel.Loaded += OnPanelLoaded;

		if (panel.IsLoaded) OnPanelLoaded(panel);
	}

	protected static void OnPanelLoaded(object sender, RoutedEventArgs? e) => OnPanelLoaded((Panel)sender);
	protected static void OnPanelLoaded(Panel panel) {
		double horizontal = GetHorizontal(panel), vertical = GetVertical(panel);
		int childCount = panel.Children.Count;

		// Go over the children and set margin for them:
		for (int i = 0; i < childCount; i++) {
			UIElement child = panel.Children[i];
			if (child is not FrameworkElement element || GetExclude(element)) continue;

			//if (panel is StackPanel) {
				bool isLastItem = i == childCount - 1;
				element.Margin = isLastItem ? new(0) : new(0, 0, horizontal, vertical);
			//} else if (panel is UniformGrid uniformGrid) {
			//	int columns = Math.Min(uniformGrid.Columns, childCount),
			//		rows = Math.
			//}
		}
	}
}
