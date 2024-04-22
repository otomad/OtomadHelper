namespace OtomadHelper.WPF.Controls;

public class ContentDialogButtonItem<TDialogResult> {
	public string Text { get; set; } = "";
	public TDialogResult DialogResult { get; set; }
	public bool IsDefault { get; set; } = false;

	public ContentDialogButtonItem(string text, TDialogResult dialogResult, bool isDefault = false) {
		Text = text;
		DialogResult = dialogResult;
		IsDefault = isDefault;
	}
}
