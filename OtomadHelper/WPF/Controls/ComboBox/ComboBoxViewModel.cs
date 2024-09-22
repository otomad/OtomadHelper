using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel : ObservableObject<ComboBoxFlyout> {
	public ObservableCollection<string> Items { get; } = [];

	[ObservableProperty]
	private string selected = "";

	public int SelectedIndex => Items.ToList().IndexOf(Selected);

	[RelayCommand]
	private void CheckRadioButton(string item) {
		Selected = item;
		View?.Close();
	}

	[RelayCommand]
	private void KeyDown(KeyEventArgs e) {
		if (e.Key == Key.Enter) KeyUp();
		else if (e.Key is Key.Up or Key.Down) {
			int direction = e.Key == Key.Up ? -1 : 1;
			if (Items.Count == 0) return;
			Selected = Items[MathEx.PNMod(SelectedIndex + direction, Items.Count)];
		}
	}

	[RelayCommand]
	public void KeyUp(KeyEventArgs? e = null) {
		if (e is null || e.Key is Key.Space)
			View?.Close();
	}
}
