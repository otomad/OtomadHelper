using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public partial class ContentDialogViewModel : ObservableObject<ContentDialog> {
	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string body = "";

	private string iconName = "Info";
	public string IconName {
		get => iconName;
		set {
			value = Icon.NormalizeIconName(value);
			if (Icon.IsValidIconName(value))
				SetProperty(ref iconName, value);
		}
	}

	public ObservableCollection<ContentDialogButtonItem> Buttons { get; } = [];

	[ObservableProperty]
	private object? dialogResult = null;

	[RelayCommand]
	public void ClickButton(object dialogResult) {
		DialogResult = dialogResult;
		View?.Close();
	}

	private bool expandable = false;
	public bool Expandable {
		get => expandable && !string.IsNullOrEmpty(Body);
		set => SetProperty(ref expandable, value);
	}

	[ObservableProperty]
	private bool canCopyBody = false;

	[ObservableProperty]
	private string subtitle = "";

	[ObservableProperty]
	private string footer = "";
}
