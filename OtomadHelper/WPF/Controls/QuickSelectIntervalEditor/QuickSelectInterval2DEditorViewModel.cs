namespace OtomadHelper.WPF.Controls;

public partial class QuickSelectInterval2DEditorViewModel : ObservableObject<QuickSelectInterval2DEditor> {
	[ObservableProperty]
	private ObservableQuickSelectInterval2DCollection<bool> bits = [];
	[ObservableProperty]
	private string name = "";

	public ObservableQuickSelectInterval2DCollection<bool>? OriginalBits { get; private set; }
	partial void OnBitsChanged(ObservableQuickSelectInterval2DCollection<bool> bits) {
		if (OriginalBits is null) {
			OriginalBits = bits.Clone();
			RevertOriginalCommand.NotifyCanExecuteChanged();
		}
	}

	[RelayCommand]
	private void ToggleBoolean((int row, int column)? cell) { // CAUTION: Cannot remove the `?` mark after the value tuple.
		if (cell is null) return;
		(int row, int column) = cell.Value;
		Bits[row, column] = !Bits[row, column];
	}

	[RelayCommand(CanExecute = nameof(CanRevertOriginal))]
	private void RevertOriginal() { if (OriginalBits is not null) Bits = OriginalBits.Clone(); }
	private bool CanRevertOriginal() => OriginalBits is not null;

	[RelayCommand]
	private void InvertSelection() {
		foreach ((bool selected, int row, int column) in Bits.Cells)
			Bits[row, column] = !selected;
	}

	[RelayCommand]
	private void SelectNone() {
		foreach ((bool selected, int row, int column) in Bits.Cells)
			Bits[row, column] = false;
	}
}
