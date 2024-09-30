using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

public partial class ColorPickerViewModel : ObservableObject<ColorPicker> {
	[ObservableProperty]
	private ColourSpace model = ColourSpace.Hsl;

	[ObservableProperty]
	private int axis = 2;

	[ObservableProperty]
	private Unicolour color = new(ColourSpace.Rgb255, 0, 0, 0);
}
