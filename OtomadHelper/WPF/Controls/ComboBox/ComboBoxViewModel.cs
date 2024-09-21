using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel : ObservableObject<ComboBoxFlyout> {
	public ObservableCollection<ComboBoxViewModelItem> Items { get; } = [];

	[ObservableProperty]
	private string selected = "";

	public int SelectedIndex => Items.ToList().FindIndex(item => item.Text == Selected);

	[RelayCommand]
	private void CheckRadioButton() => View?.Close();

	//public RelayCommand<int> ArrowKeyDownCommand => DefineCommand<int>(direction => {
	//	if (Items.Count == 0) return;
	//	Items[MathEx.PNMod(SelectedIndex + direction, Items.Count)].IsChecked = true;
	//});

	//public RelayCommand EnterKeyDownCommand => DefineCommand(() => KeyUp());

	[RelayCommand]
	private void KeyDown(KeyEventArgs e) {
		if (e.Key == Key.Enter) KeyUp();
		else if (e.Key is Key.Up or Key.Down) {
			int direction = e.Key == Key.Up ? -1 : 1;
			if (Items.Count == 0) return;
			Items[MathEx.PNMod(SelectedIndex + direction, Items.Count)].IsChecked = true;
		}
	}

	[RelayCommand]
	public void KeyUp(KeyEventArgs? e = null) {
		if (e is null || e.Key is Key.Space)
			View?.Close();
	}
}

public class ComboBoxViewModelItem(string text, ComboBoxViewModel viewModel) : ObservableObject {
	public ComboBoxViewModel ViewModel { get; } = viewModel;
	public string Text { get; set; } = text;

	public bool IsChecked {
		get => ViewModel.Selected == Text;
		set {
			if (value) {
				ViewModel.Selected = Text;
				OnPropertyChanged(value, !value); // BUG: SetProperty
			}
		}
	}
}
