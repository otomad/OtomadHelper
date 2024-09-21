using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

public partial class PitchPickerViewModel : ObservableObject<PitchPickerFlyout> {
	public static string[] NoteNames { get; } = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
	public static int[] Octaves { get; } = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

	private string noteName = "C";
	public string NoteName {
		get => noteName;
		set {
			value = value.ToUpperInvariant()
				.Replace("♯", "#")
				.Replace(new Regex(@"(?<=[A-G])[b♭]", RegexOptions.IgnoreCase), "b");
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
	public int Octave {
		get => octave;
		set => SetProperty(ref octave, value, Octaves.Contains(value));
	}

	public string Pitch {
		get => NoteName + Octave;
		set {
			Match pitch = value.Match(new(@"(?<NoteName>[A-G][#♯b♭]?)(?<Octave>\d+)", RegexOptions.IgnoreCase));
			if (pitch.Captures.Count == 0) return;
			NoteName = pitch.Groups["NoteName"].Value;
			if (int.TryParse(pitch.Groups["Octave"].Value, out int octave))
				Octave = octave;
		}
	}

	[RelayCommand]
	private void NoteNameChange(string value) => NoteName = value;

	[RelayCommand]
	private void OctaveChange(int value) => Octave = value;

	[RelayCommand]
	private void NoteNameSpin(int delta) {
		SignDelta(ref delta);
		NoteName = NoteNames[MathEx.PNMod(NoteNames.IndexOf(NoteName) + delta, NoteNames.Length)];
	}

	[RelayCommand]
	private void OctaveSpin(int delta) {
		SignDelta(ref delta);
		Octave = MathEx.Clamp(Octaves.IndexOf(Octave) + delta, 0, Octaves.Length);
	}

	/// <summary>
	/// Invert the polarity, and change the absolute value from 120 to 1.
	/// </summary>
	private static int SignDelta(ref int delta) => delta = -Math.Sign(delta);

	[RelayCommand]
	private void CloseKeyDown() => View?.Close();
}
