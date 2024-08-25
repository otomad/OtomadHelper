using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

internal class ContentDialogButtonPanel : Grid {
	public ContentDialogButtonPanel() : base() {
		LayoutUpdated += (sender, e) => UpdateChildren();
	}

	public static readonly DependencyProperty SpacingProperty = DependencyProperty.Register(
		nameof(Spacing), typeof(double), typeof(ContentDialogButtonPanel),
		new(8.0, (sender, e) => (sender as ContentDialogButtonPanel)?.UpdateChildren()));
	public double Spacing { get => (double)GetValue(SpacingProperty); set => SetValue(SpacingProperty, value); }

	private void UpdateChildren() {
		ColumnDefinitions.Clear();
		foreach ((UIElement button, int index) in Children.Cast<UIElement>().WithIndex()) {
			ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
			if (index != Children.Count - 1)
				ColumnDefinitions.Add(new() { Width = new(Spacing) });
			SetColumn(button, index * 2);
		}
	}
}
