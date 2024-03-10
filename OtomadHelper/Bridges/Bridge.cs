using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Bridges;

public class Bridge {
	public string ShowMessageBox(string title, string body, ContentDialogButtonItem[] buttons) {
		return new ContentDialog(title, body, buttons) { WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen }.ShowDialog();
	}
}
