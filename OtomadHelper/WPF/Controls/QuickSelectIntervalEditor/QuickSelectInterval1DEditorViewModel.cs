namespace OtomadHelper.WPF.Controls;

public partial class QuickSelectInterval1DEditorViewModel : ObservableObject<QuickSelectInterval1DEditor> {
	[ObservableProperty]
	private ObservableQuickSelectIntervalCollection<bool> bits = [];
	[ObservableProperty]
	private string name = "";

	public ObservableQuickSelectIntervalCollection<bool>? OriginalBits { get; private set; }
	partial void OnBitsChanged(ObservableQuickSelectIntervalCollection<bool> bits) {
		if (OriginalBits is null) {
			OriginalBits = bits.Clone();
			RevertOriginalCommand.NotifyCanExecuteChanged();
		}
	}

	[RelayCommand]
	private void ToggleBoolean(int index) => Bits[index] = !Bits[index];

	[RelayCommand(CanExecute = nameof(CanRevertOriginal))]
	private void RevertOriginal() { if (OriginalBits is not null) Bits = OriginalBits.Clone(); }
	private bool CanRevertOriginal() => OriginalBits is not null;

	[RelayCommand]
	private void InvertSelection() {
		for (int i = 0; i < Bits.Count; i++)
			Bits[i] = !Bits[i];
	}

	[RelayCommand]
	private void SelectNone() {
		for (int i = 0; i < Bits.Count; i++)
			Bits[i] = false;
	}
}
