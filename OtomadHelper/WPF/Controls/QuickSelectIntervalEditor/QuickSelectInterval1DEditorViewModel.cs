namespace OtomadHelper.WPF.Controls;

public partial class QuickSelectInterval1DEditorViewModel : ObservableObject<QuickSelectInterval1DEditor> {
	[ObservableProperty]
	private uint interval = 0;
	[ObservableProperty]
	private bool[] bits = [];
	[ObservableProperty]
	private string name = "";
}
