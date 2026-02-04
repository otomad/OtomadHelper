using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public partial class QuickSelectInterval1DEditorViewModel : ObservableObject<QuickSelectInterval1DEditor> {
	//[ObservableProperty]
	//private uint interval = 0;
	[ObservableProperty]
	private ObservableQuickSelectIntervalCollection<bool> bits = [];
	[ObservableProperty]
	private string name = "";

	[RelayCommand]
	private void ToggleBoolean(int index) => Bits[index] = !Bits[index];
}
