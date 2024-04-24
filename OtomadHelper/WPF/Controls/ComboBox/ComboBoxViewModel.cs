using System.Collections.ObjectModel;
using System.Windows.Input;

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

	[RelayCommand]
	private void ArrowKeyDown(int direction) {
		if (Items.Count == 0) return;
		Selected = Items[MathEx.PNMod(SelectedIndex + direction, Items.Count)].Text;
		View?.RefreshBindings();
	}

	[RelayCommand]
	private void EnterKeyDown() {
		KeyUp();
	}

	[RelayCommand]
	private void KeyUp(KeyEventArgs? e = null) {
		if (e is null || e.Key is Key.Space or Key.Escape)
			View?.Close();
	}
}

public class ComboBoxViewModelItem : ObservableObject {
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
