using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Common;

/// <summary>
/// Automatic horizontal or vertical spacing in StackPanel and other list like panels.
/// </summary>
/// <remarks>
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
/// <StackPanel Orientation="Vertical" m:MarginSetter.Margin="5" m:MarginSetter.LastItemMargin="5 0">
///     <Button>Button 1</Button>
///     <Button>Button 2</Button>
/// </StackPanel>
/// ]]>
/// </code>
/// </example>
/// </remarks>
[AttachedDependencyProperty<Thickness, Panel>("Margin")]
[AttachedDependencyProperty<Thickness, Panel>("LastItemMargin")]
[AttachedDependencyProperty<bool>("Exclude", DefaultValue = false)]
public partial class MarginSetter {
	static partial void OnMarginChanged(Panel panel) {
		// Avoid duplicate registrations.
		panel.Loaded -= OnPanelLoaded;
		panel.Loaded += OnPanelLoaded;

		if (panel.IsLoaded) OnPanelLoaded(panel, null);
	}

	static partial void OnLastItemMarginChanged(Panel panel) => OnMarginChanged(panel);

	private static void OnPanelLoaded(object sender, RoutedEventArgs? e) {
		Panel panel = (Panel)sender;

		// Go over the children and set margin for them:
		for (int i = 0; i < panel.Children.Count; i++) {
			UIElement child = panel.Children[i];
			if (child is not FrameworkElement element || GetExclude(element)) continue;

			bool isLastItem = i == panel.Children.Count - 1;
			element.Margin = isLastItem ? GetLastItemMargin(panel) : GetMargin(panel);
		}
	}
}

/// <summary>
/// Automatic horizontal or vertical spacing in StackPanel and other list like panels.
/// </summary>
/// <remarks>
/// <see href="https://gist.github.com/angularsen/90040fb174f71c5ab3ad" />
/// </remarks>
[AttachedDependencyProperty<double>("Horizontal", DefaultValue = 0d)]
[AttachedDependencyProperty<double>("Vertical", DefaultValue = 0d)]
public partial class Spacing {
	static partial void OnHorizontalChanged(DependencyObject panel, double space) {
		MarginSetter.SetMargin(panel, new(0, 0, space, 0));
		MarginSetter.SetLastItemMargin(panel, new(0));
	}

	static partial void OnVerticalChanged(DependencyObject panel, double space) {
		MarginSetter.SetMargin(panel, new(0, 0, 0, space));
		MarginSetter.SetLastItemMargin(panel, new(0));
	}
}
