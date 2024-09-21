using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<double>("Spacing", DefaultValue = 8.0)]
internal partial class ContentDialogButtonPanel : Grid {
	public ContentDialogButtonPanel() : base() {
		LayoutUpdated += (sender, e) => UpdateChildren();
	}

	partial void OnSpacingChanged() {
		UpdateChildren();
	}

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
