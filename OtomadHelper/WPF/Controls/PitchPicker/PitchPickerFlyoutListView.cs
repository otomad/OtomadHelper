using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

	public event MouseButtonEventHandler? ItemClick;
	internal void InvokeItemClick(object sender, MouseButtonEventArgs e) => ItemClick?.Invoke(sender, e);
}

public class PitchPickerFlyoutListViewItem : ListViewItem {
	public PitchPickerFlyoutListView? ParentListView { get; internal set; }

	public event MouseButtonEventHandler? Click;

	public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(nameof(IsPressed), typeof(bool), typeof(PitchPickerFlyoutListViewItem), new(false));
	public bool IsPressed { get => (bool)GetValue(IsPressedProperty); internal set => SetValue(IsPressedProperty, value); }

	public PitchPickerFlyoutListViewItem() {
		// If user press the item and then drag it out, do not trigger the click event when mouse up.
		PreviewMouseLeftButtonDown += (sender, e) => IsPressed = true;
		MouseLeave += (sender, e) => IsPressed = false;
		PreviewMouseLeftButtonUp += (sender, e) => {
			if (IsPressed == true) {
				Click?.Invoke(sender, e);
				ParentListView?.InvokeItemClick(sender, e);
			}
		};
	}

	public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(PitchPickerFlyoutListViewItem), new(false));
	public bool IsActive { get => (bool)GetValue(IsActiveProperty); internal set => SetValue(IsActiveProperty, value); }
}
