using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Bridges;

public class Bridge {
	public string ShowMessageBox(string title, string body, ContentDialogButtonItem<string>[] buttons, string iconName = "") =>
		ContentDialog.ShowDialog<string>(title, body, buttons, iconName) ?? "";
		// Test:
		// await bridges.bridge.showMessageBox("幸福倒计时", "Windows 11 即将更新！", [{ text: "OK", dialogResult: "ok", isDefault: true }, { text: "Cancel", dialogResult: "cancel" }], "info");
}
