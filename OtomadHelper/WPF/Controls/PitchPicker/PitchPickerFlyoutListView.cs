using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

public class PitchPickerFlyoutListView : ListView {
	protected override DependencyObject GetContainerForItemOverride() =>
		new PitchPickerFlyoutListViewItem();

	protected override bool IsItemItsOwnContainerOverride(object item) =>
		item is PitchPickerFlyoutListViewItem;

	protected override void PrepareContainerForItemOverride(DependencyObject element, object item) {
		base.PrepareContainerForItemOverride(element, item);
		if (element is PitchPickerFlyoutListViewItem listViewItem)
			listViewItem.ParentListView = this;
	}
}

[DependencyProperty<bool>("IsPressed", DefaultValue = false)]
[RoutedEvent("Click", RoutedEventStrategy.Bubble)]
public partial class PitchPickerFlyoutListViewItem : ListViewItem {
	public PitchPickerFlyoutListView? ParentListView { get; internal set; }

	public PitchPickerFlyoutListViewItem() {
		// If user press the item and then drag it out, do not trigger the click event when mouse up.
		PreviewMouseLeftButtonDown += (sender, e) => IsPressed = true;
		MouseLeave += (sender, e) => IsPressed = false;
		PreviewMouseLeftButtonUp += (sender, e) => {
			if (IsPressed) {
				RaiseEvent(new RoutedEventArgs(ClickEvent));
			}
		};
	}
}
