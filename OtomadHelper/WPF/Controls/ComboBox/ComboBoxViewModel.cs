using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel<T> : ObservableObject<ComboBoxFlyout> {
	public ObservableCollection<string> Options { get; } = [];

	public ObservableCollection<T> Ids { get; } = [];

	[ObservableProperty]
	private T selected = typeof(T).Extends(typeof(string)) ? (T)(object)string.Empty : default!;

	public int SelectedIndex => Ids.ToList().IndexOf(Selected);

	public void Close() => View?.Close();

	[RelayCommand]
	private void CheckRadioButton(T id) {
		Selected = id;
		Close();
	}

	[RelayCommand]
	private void ArrowMove(int direction) {
		if (Ids.Count == 0) return;
		Selected = Ids[MathEx.PNMod(SelectedIndex + direction, Ids.Count)];
	}
}

public class ComboBoxViewModel : ComboBoxViewModel<object> { }
