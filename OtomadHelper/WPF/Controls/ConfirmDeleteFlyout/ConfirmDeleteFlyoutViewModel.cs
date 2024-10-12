namespace OtomadHelper.WPF.Controls;

public partial class ConfirmDeleteFlyoutViewModel : ObservableObject<ConfirmDeleteFlyout> {
	[ObservableProperty]
	private string message = "";

	[ObservableProperty]
	private bool dialogResult = false;

	[RelayCommand]
	private void SetDialogResult(bool? dialogResult) {
		DialogResult = dialogResult == true;
		View?.Close();
	}
}
