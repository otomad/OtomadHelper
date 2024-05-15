using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public class ContentDialogViewModel : ObservableObject<ContentDialog> {
	private string title = "";
	public string Title { get => title; set => SetProperty(ref title, value); }

	private string body = "";
	public string Body { get => body; set => SetProperty(ref body, value); }

	private string iconName = "Info";
	public string IconName {
		get => iconName;
		set {
			value = Icon.NormalizeIconName(value);
			if (Icon.IsValidIconName(value))
				SetProperty(ref iconName, value);
		}
	}

	public ObservableCollection<ContentDialogButtonItem> Buttons { get; } = new();

	private object? dialogResult = null;
	public object? DialogResult { get => dialogResult; set => SetProperty(ref dialogResult, value); }

	public RelayCommand<object> ClickButtonCommand => DefineCommand<object>(dialogResult => {
		DialogResult = dialogResult;
		View?.Close();
	});
}
