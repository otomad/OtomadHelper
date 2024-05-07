using System.Collections.ObjectModel;
using System.Windows;

using IconNameToSymbol = OtomadHelper.WPF.Controls.ContentDialogIconNameToSymbolConverter;

namespace OtomadHelper.WPF.Controls;

public partial class ContentDialogViewModel : ObservableObject, IViewAccessibleViewModel<ContentDialog> {
	public ContentDialog? View { get; set; }

	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string body = "";

	[ObservableProperty]
	private string iconName = IconNameToSymbol.DefaultIconName;
	partial void OnIconNameChanged(string? oldValue, string newValue) {
		if (!IconNameToSymbol.IsValidIconName(newValue)) IconName = oldValue!;
	}

	public ObservableCollection<ContentDialogButtonItem> Buttons { get; } = new();

	[ObservableProperty]
	private object? dialogResult = null;

	[RelayCommand]
	private void ClickButton(object dialogResult) {
		DialogResult = dialogResult;
		View?.Close();
	}
}
