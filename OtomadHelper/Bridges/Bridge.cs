using OtomadHelper.WPF.Controls;

using RectTuple = System.Tuple<double, double, double, double>;

namespace OtomadHelper.Bridges;

public class Bridge {
	/// <summary>
	/// Sets the development mode flag for the application.
	/// </summary>
	/// <param name="isDevMode">A boolean value indicating whether the application is in development mode.</param>
	public void SetIsDevMode(bool isDevMode) => Host.isDevMode = isDevMode;

	/// <summary>
	/// Sets the culture for the application.
	/// </summary>
	/// <param name="culture">A <see cref="string"/> representing the culture to be set.<br />
	/// The culture should be in the format "xx-XX" where xx is the language code and XX is the country/region code.</param>
	public void SetCulture(string culture) => I18n.SetCulture = culture;

	/// <summary>
	/// Displays a message box with the specified title, body, buttons, and optional icon name.
	/// </summary>
	/// <param name="title">The title of the message box.</param>
	/// <param name="body">The content of the message box.</param>
	/// <param name="buttons">An array of button items to be displayed in the message box.</param>
	/// <param name="iconName">An optional <see cref="string"/> representing the icon name to be displayed
	/// in the message box. Defaults to "Info".</param>
	/// <returns>The selected button <see cref="ContentDialogButtonItem.DialogResult"/> from the message box,
	/// or an empty <see cref="string"/> if user click the close button in the title bar.</returns>
	public async Task<string> ShowMessageBox(string title, string body, ContentDialogButtonItem<string>[] buttons, string iconName = "") =>
		await ContentDialog.ShowDialog<string>(title, body, buttons, iconName) ?? "";
		// Test:
		// await bridges.bridge.showMessageBox("幸福倒计时", "Windows 11 即将更新！", [{ text: "OK", dialogResult: "ok", isDefault: true }, { text: "Cancel", dialogResult: "cancel" }], "info");

	/// <summary>
	/// Displays a combo box flyout with the specified options and initial selected item.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the flyout's position.</param>
	/// <param name="selected">The current selected item in the combo box.</param>
	/// <param name="options">An array of <see cref="string"/> representing the items to be displayed in the combo box.</param>
	/// <returns>The selected item from the combo box flyout.</returns>
	public async Task<string> ShowComboBox(RectTuple rect, string selected, string[] options) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		ComboBoxFlyout flyout = ComboBoxFlyout.Initial(options, selected, screenRect, out Task<string> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}

	/// <summary>
	/// Displays a pitch picker flyout with the specified initial pitch.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the flyout's position.</param>
	/// <param name="pitch">The initial pitch (note name and octave) selected in the pitch picker.</param>
	/// <returns>The selected pitch from the pitch picker flyout.</returns>
	public async Task<string> ShowPitchPicker(RectTuple rect, string pitch) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		PitchPickerFlyout flyout = PitchPickerFlyout.Initial(screenRect, pitch, out Task<string> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}
}
