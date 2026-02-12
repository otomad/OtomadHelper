namespace OtomadHelper.WPF.Controls;

public partial class PitchPickerViewModel : ObservableObject<PitchPickerFlyout> {
	public static string[] NoteNames { get; } = ["C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"];
	public static int[] Octaves { get; } = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

	public string NoteName {
		get => field;
		set {
			value = value
				.ToUpperInvariant()
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
			SetProperty(ref field, value, NoteNames.Contains(value));
		}
	} = "C";

	public int Octave {
		get => field;
		set {
			SetProperty(ref field, value, Octaves.Contains(value));
			OctaveSpinCommand.NotifyCanExecuteChanged();
		}
	} = 5;

	internal string originalPitch = "C5";
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
	private void NoteNameSpin(FocusMoveDirection direction) {
		if (ToDelta(direction) is { } delta)
			NoteName = NoteNames[Math.FloorMod(NoteNames.IndexOf(NoteName) + delta, NoteNames.Length)];
		else if (direction is FocusMoveDirection.First)
			NoteName = NoteNames.First();
		else if (direction is FocusMoveDirection.Last)
			NoteName = NoteNames.Last();
	}

	[RelayCommand(CanExecute = nameof(CanExecuteOctaveSpin))]
	private void OctaveSpin(FocusMoveDirection direction) {
		if (ToDelta(direction) is { } delta)
			Octave = Math.Clamp(Octaves.IndexOf(Octave) + delta, 0, Octaves.Length);
		else if (direction is FocusMoveDirection.First)
			Octave = Octaves.First();
		else if (direction is FocusMoveDirection.Last)
			Octave = Octaves.Last();
	}

	private bool CanExecuteOctaveSpin(FocusMoveDirection direction) {
		int index = Octaves.IndexOf(Octave);
		return direction switch {
			< 0 => index != 0,
			> 0 => index != Octaves.Length - 1,
			_ => false,
		};
	}

	private static int? ToDelta(FocusMoveDirection direction) => direction switch {
		FocusMoveDirection.Previous or FocusMoveDirection.Next => direction.Delta,
		FocusMoveDirection.PageBackward or FocusMoveDirection.PageForward => direction.Delta * ((PitchPickerFlyout.DisplayItemCount - 1) / 2),
		_ => null,
	};

	[RelayCommand]
	private void CloseKeyDown() => View?.Close();

	[RelayCommand]
	public void CloseWithoutSaving() {
		Pitch = originalPitch;
		View?.Close();
	}
}
