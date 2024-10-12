using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Bridges;

public class Bridge {
	/// <summary>
	/// Sets the development mode flag for the application.
	/// </summary>
	/// <param name="isDevMode">A boolean value indicating whether the application is in development mode.</param>
	public void SetIsDevMode(bool isDevMode) => MainDock.isDevMode = isDevMode;

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
	/// <param name="selected">The current selected item ID in the combo box.</param>
	/// <param name="ids">An array of <see cref="string"/> representing the ID of items in the combo box.</param>
	/// <param name="options">An array of <see cref="string"/> representing the items to be displayed in the combo box.</param>
	/// <returns>The selected item from the combo box flyout.</returns>
	public async Task<T> ShowComboBox<T>(Rect rect, T selected, T[] ids, string[] options) {
		Rect screenRect = MainDock.ClientToScreenRect(rect);
		ComboBoxFlyout flyout = ComboBoxFlyout.Initial(ids, options, selected, screenRect, out Task<T> resultTask);
		MainDock.ShowFlyout(flyout);
		return await resultTask;
	}

	/// <summary>
	/// Displays a pitch picker flyout with the specified initial pitch.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the flyout's position.</param>
	/// <param name="pitch">The initial pitch (note name and octave) selected in the pitch picker.</param>
	/// <returns>The selected pitch from the pitch picker flyout.</returns>
	public async Task<string> ShowPitchPicker(Rect rect, string pitch) {
		Rect screenRect = MainDock.ClientToScreenRect(rect);
		PitchPickerFlyout flyout = PitchPickerFlyout.Initial(screenRect, pitch, out Task<string> resultTask);
		MainDock.ShowFlyout(flyout);
		return await resultTask;
	}

	/// <summary>
	/// Displays a color picker window with the old hex color.
	/// </summary>
	/// <param name="hex">Old hex color.</param>
	/// <returns>New hex color.</returns>
	public async Task<string> ShowColorPicker(string hex) {
		string newHex = await ColorPicker.ShowDialog(hex);
		return newHex;
	}

	/// <summary>
	/// Displays a comfirm delete flyout with the hint text.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the target's position.</param>
	/// <param name="text">Text like <b>"Are you sure you want to delete it?"</b>.</param>
	/// <returns>Does user click the OK button?</returns>
	public async Task<bool> ShowConfirmDeleteFlyout(Rect rect, string message) {
		Rect screenRect = MainDock.ClientToScreenRect(rect);
		ConfirmDeleteFlyout flyout = ConfirmDeleteFlyout.Initial(screenRect, message, out Task<bool> resultTask);
		MainDock.ShowFlyout(flyout);
		return await resultTask;
	}
}
