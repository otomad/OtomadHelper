namespace OtomadHelper.WPF.Controls;

public partial class QuickSelectInterval1DEditorViewModel : ObservableObject<QuickSelectInterval1DEditor> {
	[ObservableProperty]
	private uint column = 0;
	[ObservableProperty]
	private uint row = 0;
	[ObservableProperty]
	private bool[] bits = [];
}
