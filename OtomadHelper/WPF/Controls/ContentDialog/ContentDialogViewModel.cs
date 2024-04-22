using System.Collections.ObjectModel;

using OtomadHelper.WPF.Converters;

namespace OtomadHelper.WPF.Controls;

public class ContentDialogViewModel<TDialogResult> : NotifyPropertyChanged {
	private string title = "";
	public string Title { get => title; set => SetField(ref title, value); }

	private string body = "";
	public string Body { get => body; set => SetField(ref body, value); }

	private static string DefaultIconName => ContentDialogIconNameToSymbolConverter.DefaultIconName;
	private string iconName = DefaultIconName;
	public string IconName {
		get => iconName;
		set => SetField(ref iconName, string.IsNullOrWhiteSpace(value) ? DefaultIconName : value);
	}

	public ObservableCollection<ContentDialogButtonItem<TDialogResult>> Buttons { get; } = new();
}

internal class ContentDialogViewModel : ContentDialogViewModel<object> { }
