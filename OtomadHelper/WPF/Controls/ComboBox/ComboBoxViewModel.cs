using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public class ComboBoxViewModel : ObservableObject<ComboBoxFlyout> {
	public ObservableCollection<string> Items { get; } = new();

	private string selected = "";
	public string Selected { get => selected; set => SetProperty(ref selected, value); }

	public int SelectedIndex => Items.ToList().FindIndex(item => item == Selected);

	public RelayCommand<string> CheckRadioButtonCommand => DefineCommand<string>(value => View?.Close());

	public RelayCommand<int> ArrowKeyDownCommand => DefineCommand<int>(direction => {
		if (Items.Count == 0) return;
		Selected = Items[MathEx.PNMod(SelectedIndex + direction, Items.Count)];
	});

	public RelayCommand EnterKeyDownCommand => DefineCommand(() => KeyUp());

	public IRelayCommand<KeyEventArgs?> KeyUpCommand => DefineCommand<KeyEventArgs?>(KeyUp);
	private void KeyUp(KeyEventArgs? e = null) {
		if (e is null || e.Key is Key.Space)
			View?.Close();
	}
}

/*public class ComboBoxViewModelItem : ObservableObject {
	public ComboBoxViewModel ViewModel { get; }

	public string Text { get; set; }

	public bool IsChecked {
		get => ViewModel.Selected == Text;
		set {
			if (value) {
				ViewModel.Selected = Text;
				OnPropertyChanged();
			}
		}
	}

	public ComboBoxViewModelItem(string text, ComboBoxViewModel viewModel) {
		ViewModel = viewModel;
		Text = text;
	}
}*/
