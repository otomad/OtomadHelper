using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace OtomadHelper.WPF.Controls;

public partial class ContentDialogViewModel<TDialogResult> : ObservableValidator {
	[ObservableProperty]
	private string title = "";

	[ObservableProperty]
	private string body = "";

	private static string DefaultIconName => ContentDialogIconNameToSymbolConverter.DefaultIconName;
	[ObservableProperty]
	[ContentDialogIconNameValidator]
	private string iconName = DefaultIconName;

	public ObservableCollection<ContentDialogButtonItem<TDialogResult>> Buttons { get; } = new();
}

internal class ContentDialogViewModel : ContentDialogViewModel<object> { }
