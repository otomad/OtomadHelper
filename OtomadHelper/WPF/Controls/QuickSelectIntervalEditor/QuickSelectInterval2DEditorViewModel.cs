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
			RestoreOriginalCommand.NotifyCanExecuteChanged();
		}
	}

	[RelayCommand]
	private void ToggleBoolean((int row, int column) cell) => Bits[cell.row, cell.column] = !Bits[cell.row, cell.column];

	[RelayCommand(CanExecute = nameof(CanRestoreOriginal))]
	private void RestoreOriginal() { if (OriginalBits is not null) Bits = OriginalBits.Clone(); }
	private bool CanRestoreOriginal() => OriginalBits is not null;

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
