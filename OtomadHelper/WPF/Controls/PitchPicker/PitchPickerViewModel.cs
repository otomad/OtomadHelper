using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public class PitchPickerViewModel : ObservableObject<PitchPickerFlyout> {
	public static string[] NoteNames { get; } = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
	public static int[] Octaves { get; } = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

	private string noteName = "C";
	public string NoteName {
		get => noteName;
		set {
			value = value.ToUpper().Replace("♯", "#").Replace(new Regex(@"(?<=[A-G])[b♭]", RegexOptions.IgnoreCase), "b");
			if (value.EndsWith("b"))
				value = value switch {
					"Db" => "C#",
					"Eb" => "D#",
					"Gb" => "F#",
					"Ab" => "G#",
					"Bb" => "A#",
					_ => value,
				};
			SetProperty(ref noteName, value, NoteNames.Contains(value));
		}
	}

	private int octave = 5;
	public int Octave { get => octave; set => SetProperty(ref octave, value, Octaves.Contains(value)); }

	public string Pitch {
		get => NoteName + Octave;
		set {
			Match pitch = value.Match(new(@"(?<NoteName>[A-G][#♯b♭]?)(?<Octave>1?\d)", RegexOptions.IgnoreCase));
			if (pitch.Captures.Count == 0) return;
			NoteName = pitch.Groups["NoteName"].Value;
			if (int.TryParse(pitch.Groups["Octave"].Value, out int octave))
				Octave = octave;
		}
	}

	public RelayCommand<string> NoteNameChangeCommand => DefineCommand<string>(value => NoteName = value);

	public RelayCommand<int> OctaveChangeCommand => DefineCommand<int>(value => Octave = value);
}
