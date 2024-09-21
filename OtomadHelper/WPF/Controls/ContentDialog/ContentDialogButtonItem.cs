namespace OtomadHelper.WPF.Controls;

public class ContentDialogButtonItem(string text, object? dialogResult, bool isDefault = false) {
	public string Text { get; set; } = text;
	public object? DialogResult { get; set; } = dialogResult;
	public bool IsDefault { get; set; } = isDefault;
}

public class ContentDialogButtonItem<TDialogResult>(string text, TDialogResult? dialogResult, bool isDefault = false) :
	ContentDialogButtonItem(text, dialogResult, isDefault) {

	public new TDialogResult? DialogResult {
		get => (TDialogResult?)base.DialogResult;
		set => base.DialogResult = value;
	}
}
