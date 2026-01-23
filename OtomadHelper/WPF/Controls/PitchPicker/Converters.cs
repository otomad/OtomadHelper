using System.Globalization;

namespace OtomadHelper.WPF.Controls;

public class OctaveSpinnerToIsEnabledConverter : ValueConverter<int, bool, FocusMoveDirection> {
	public override bool Convert(int octave, Type targetType, FocusMoveDirection direction, CultureInfo culture) {
		int index = PitchPickerViewModel.Octaves.IndexOf(octave);
		return direction switch {
			< 0 => index != 0,
			> 0 => index != PitchPickerViewModel.Octaves.Length - 1,
			_ => false,
		};
	}
}
