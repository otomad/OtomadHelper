using System.Collections.ObjectModel;

using IconNameToSymbol = OtomadHelper.WPF.Controls.ContentDialogIconNameToSymbolConverter;

namespace OtomadHelper.WPF.Controls;

public partial class ContentDialogViewModel : ObservableObject {
	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string body = "";

	private string iconName = IconNameToSymbol.DefaultIconName;
	public string IconName {
		get => iconName;
		set => SetProperty(ref iconName, IconNameToSymbol.IsValidIconName(value) ? value : IconNameToSymbol.DefaultIconName);
	}

	public ObservableCollection<ContentDialogButtonItem> Buttons { get; } = new();

	[ObservableProperty]
	private object? dialogResult = null;

	[RelayCommand]
	private void ClickButton(dynamic parameters) { // parameters actually type: Tuple<TDialogResult, ContentDialog>
		dynamic dialogResult = parameters.Item1;
		ContentDialog dialog = parameters.Item2;
		DialogResult = dialogResult;
		dialog.Close();
	}
}
