using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Bridges;

public class Bridge {
	public string ShowMessageBox(string title, string body, ContentDialogButtonItem[] buttons, string iconName = "") {
		return ContentDialog.ShowDialog<string>(title, body, buttons, iconName) ?? "";
	}
}
