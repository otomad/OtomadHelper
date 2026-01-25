using System.Windows;

namespace OtomadHelper.WPF.Controls;

public class ContentDialogButtonItem(string text, object? dialogResult, bool isDefault = false, RoutedEventHandler? click = null) {
	public string Text { get; set; } = text;
	public object? DialogResult { get; set; } = dialogResult;
	public bool IsDefault { get; set; } = isDefault;
	public RoutedEventHandler? Click { get; set; } = click;
}

public class ContentDialogButtonItem<TDialogResult>(string text, TDialogResult? dialogResult, bool isDefault = false, RoutedEventHandler? click = null) :
	ContentDialogButtonItem(text, dialogResult, isDefault, click) {

	public new TDialogResult? DialogResult {
		get => (TDialogResult?)base.DialogResult;
		set => base.DialogResult = value;
	}
}
