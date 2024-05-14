using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public class PitchPickerFlyoutListView : ListView {
	public PitchPickerFlyoutListView() {
		Loaded += OnLoaded;
	}

	private void OnLoaded(object sender, RoutedEventArgs e) {
		foreach (ListViewItem item in this.GetChildrenOfType<ListViewItem>()) {
			// TODO: MouseDown...
			item.PreviewMouseLeftButtonUp += (sender, e) => ItemClick?.Invoke(sender, e);
		}
	}

	public event MouseButtonEventHandler? ItemClick;
}
