using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel : ObservableObject {
	public ComboBoxFlyout? View { get; set; }

	public ObservableCollection<ComboBoxViewModelItem> Items { get; } = new();

	[ObservableProperty]
	public string selected = "";

	public int SelectedIndex => Items.ToList().FindIndex(item => item.Text == Selected);

	[RelayCommand]
	private void CheckRadioButton(string value) {
		View?.Close();
	}
}

public class ComboBoxViewModelItem {
	public ComboBoxViewModel ViewModel { get; }

	public string Text { get; set; }

	public bool IsChecked {
		get => ViewModel.Selected == Text;
		set { if (value) ViewModel.Selected = Text; }
	}

	public ComboBoxViewModelItem(string text, ComboBoxViewModel viewModel) {
		ViewModel = viewModel;
		Text = text;
	}
}
