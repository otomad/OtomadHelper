namespace OtomadHelper.WPF.Controls;

public class ContentDialogButtonItem {
	public string Text { get; set; } = "";
	public object? DialogResult { get; set; }
	public bool IsDefault { get; set; } = false;

	public ContentDialogButtonItem(string text, object? dialogResult, bool isDefault = false) {
		Text = text;
		DialogResult = dialogResult;
		IsDefault = isDefault;
	}
}

public class ContentDialogButtonItem<TDialogResult> : ContentDialogButtonItem {
	public new TDialogResult? DialogResult { get => (TDialogResult?)base.DialogResult; set => base.DialogResult = value; }

	public ContentDialogButtonItem(string text, TDialogResult? dialogResult, bool isDefault = false) :
		base(text, dialogResult, isDefault) { }
}
