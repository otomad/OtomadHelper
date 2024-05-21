using OtomadHelper.WPF.Controls;

using RectTuple = System.Tuple<double, double, double, double>;

namespace OtomadHelper.Bridges;

public class Bridge {
	public void SetIsDevMode(bool isDevMode) => Host.isDevMode = isDevMode;

	public string ShowMessageBox(string title, string body, ContentDialogButtonItem<string>[] buttons, string iconName = "") =>
		ContentDialog.ShowDialog<string>(title, body, buttons, iconName) ?? "";
		// Test:
		// await bridges.bridge.showMessageBox("幸福倒计时", "Windows 11 即将更新！", [{ text: "OK", dialogResult: "ok", isDefault: true }, { text: "Cancel", dialogResult: "cancel" }], "info");

	public async Task<string> ShowComboBox(RectTuple rect, string selected, string[] options) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		ComboBoxFlyout flyout = ComboBoxFlyout.Initial(options, selected, screenRect, out Task<string> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}

	public async Task<string> ShowPitchPicker(RectTuple rect, string pitch) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		PitchPickerFlyout flyout = PitchPickerFlyout.Initial(screenRect, pitch, out Task<string> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}
}
