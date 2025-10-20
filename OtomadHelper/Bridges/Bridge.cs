using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Bridges;

public class Bridge {
	/// <summary>
	/// Set the development mode flag for the application.
	/// </summary>
	/// <param name="isDevMode">A boolean value indicating whether the application is in development mode.</param>
	public void SetIsDevMode(bool isDevMode) => Host.isDevMode = isDevMode;

	/// <summary>
	/// Set the culture for the application.
	/// </summary>
	/// <param name="culture">A <see cref="string"/> representing the culture to be set.<br />
	/// The culture should be in the format "xx-XX" where xx is the language code and XX is the country/region code.</param>
	public void SetCulture(string culture) => Localize.SetCulture = culture;

	/// <summary>
	/// Set whether the page is focused or not.
	/// </summary>
	/// <param name="focused">Got focused?</param>
	public void SetPageFocus(bool focused) => Host.Focused = focused;

	/// <summary>
	/// Display a message box with the specified title, body, buttons, and optional icon name.
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
	/// Display a combo box flyout with the specified options and initial selected item.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the flyout's position.</param>
	/// <param name="selected">The current selected item ID in the combo box.</param>
	/// <param name="ids">An array of <see cref="string"/> representing the ID of items in the combo box.</param>
	/// <param name="options">An array of <see cref="string"/> representing the items to be displayed in the combo box.</param>
	/// <returns>The selected item from the combo box flyout.</returns>
	public async Task<T> ShowComboBox<T>(Rect rect, T selected, T[] ids, string[] options, string[]? icons = null) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		ComboBoxFlyout flyout = ComboBoxFlyout.Initial(ids, options, icons, selected, screenRect, out Task<T> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}

	/// <summary>
	/// Display a pitch picker flyout with the specified initial pitch.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the flyout's position.</param>
	/// <param name="pitch">The initial pitch (note name and octave) selected in the pitch picker.</param>
	/// <returns>The selected pitch from the pitch picker flyout.</returns>
	public async Task<string> ShowPitchPicker(Rect rect, string pitch) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		PitchPickerFlyout flyout = PitchPickerFlyout.Initial(screenRect, pitch, out Task<string> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}

	/// <summary>
	/// Display a color picker window with the old hex color.
	/// </summary>
	/// <param name="hex">Old hex color.</param>
	/// <returns>A value tuple indicate if user click the ok and the new hex color.</returns>
	public async Task<ValueTuple<bool, string>> ShowColorPicker(string hex) {
		(bool, string) result = await ColorPicker.ShowDialog(hex);
		return result;
	}

	/// <summary>
	/// Display a comfirm delete flyout with the hint text.
	/// </summary>
	/// <param name="rect">A tuple representing the screen coordinates (x, y, width, height) of the target's position.</param>
	/// <param name="message">Text like <b>"Are you sure you want to delete it?"</b>.</param>
	/// <returns>Does user click the OK button?</returns>
	public async Task<bool> ShowConfirmDeleteFlyout(Rect rect, string message) {
		Rect screenRect = Host.ClientToScreenRect(rect);
		ConfirmDeleteFlyout flyout = ConfirmDeleteFlyout.Initial(screenRect, message, out Task<bool> resultTask);
		Host.ShowFlyout(flyout);
		return await resultTask;
	}
}
