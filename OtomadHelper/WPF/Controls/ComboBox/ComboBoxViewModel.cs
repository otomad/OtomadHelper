using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel<T> : ObservableObject<ComboBoxFlyout> {
	public ObservableCollection<string> Options { get; } = [];

	public ObservableCollection<T> Ids { get; } = [];

	[ObservableProperty]
	private T selected = typeof(T).Extends(typeof(string)) ? (T)(object)string.Empty : default!;

	public int SelectedIndex => Ids.ToList().IndexOf(Selected);

	[RelayCommand]
	private void CheckRadioButton(T id) {
		Selected = id;
		View?.Close();
	}

	[RelayCommand]
	private void KeyDown(KeyEventArgs e) {
		if (e.Key == Key.Enter) KeyUp();
		else if (e.Key is Key.Up or Key.Down) {
			int direction = e.Key == Key.Up ? -1 : 1;
			if (Ids.Count == 0) return;
			Selected = Ids[MathEx.PNMod(SelectedIndex + direction, Ids.Count)];
		}
	}

	[RelayCommand]
	public void KeyUp(KeyEventArgs? e = null) {
		if (e is null || e.Key is Key.Space)
			View?.Close();
	}
}

public class ComboBoxViewModel : ComboBoxViewModel<object> { }
