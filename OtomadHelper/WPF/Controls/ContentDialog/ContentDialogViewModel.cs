using System.Collections.ObjectModel;
using System.Windows;

using BaseButton = System.Windows.Controls.Button;

namespace OtomadHelper.WPF.Controls;

public partial class ContentDialogViewModel : ObservableObject<ContentDialog> {
	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string body = "";

	[ObservableProperty]
	private UIElement? content;

	public string IconName {
		get => field;
		set {
			value = Icon.NormalizeIconName(value);
			if (Icon.IsKnownIcon(value))
				SetProperty(ref field, value);
		}
	} = "Info";

	public ObservableCollection<ContentDialogButtonItem> Buttons { get; } = [];

	[ObservableProperty]
	private object? dialogResult = null;

	[RelayCommand]
	public void ClickButtonToClose(object dialogResult) {
		DialogResult = dialogResult;
		View?.Close();
	}

	[RelayCommand]
	public void CustomClickButton(CompositeCommandParameter<RoutedEventHandler?, RoutedEventArgs?> param) {
		(RoutedEventHandler? click, RoutedEventArgs? e) = param;
		click?.Invoke(e?.OriginalSource, e);
	}

	public bool Expandable {
		get => field && !string.IsNullOrEmpty(Body);
		set => SetProperty(ref field, value);
	} = false;

	[ObservableProperty]
	private bool canCopyBody = false;

	[ObservableProperty]
	private string subtitle = "";

	[ObservableProperty]
	private string footer = "";

	[ObservableProperty]
	private UIElement? header;
}
