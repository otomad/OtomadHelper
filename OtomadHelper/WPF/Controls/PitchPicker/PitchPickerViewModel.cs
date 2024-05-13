using System.Collections.ObjectModel;

namespace OtomadHelper.WPF.Controls;

public class PitchPickerViewModel : ObservableObject<PitchPickerFlyout> {
	public static readonly ObservableCollection<string> noteNames = new() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
	public ObservableCollection<string> NoteNames => noteNames;

	public static readonly ObservableCollection<int> octaves = new() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
	public ObservableCollection<int> Octaves => octaves;
}
