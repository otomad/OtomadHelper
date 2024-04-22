using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public partial class ComboBoxViewModel : ObservableObject {
	public ObservableCollection<string> Items { get; } = new();

	[ObservableProperty]
	public string current = "";
}
