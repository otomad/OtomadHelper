namespace OtomadHelper.WPF.Controls;

public class ContentDialogButtonItem {
	public string Text { get; set; } = "";
	public object DialogResult { get; set; }
	public bool IsDefault { get; set; } = false;

	public ContentDialogButtonItem(string text, object dialogResult, bool isDefault = false) {
		Text = text;
		DialogResult = dialogResult;
		IsDefault = isDefault;
	}
}
