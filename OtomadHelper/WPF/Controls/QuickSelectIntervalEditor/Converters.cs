using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(int), typeof(int))]
public class AddOneConverter : ValueConverter<int, int> {
	public override int Convert(int zeroBased, Type targetType, object parameter, CultureInfo culture) => zeroBased + 1;

	public override int ConvertBack(int oneBased, Type targetType, object parameter, CultureInfo culture) => oneBased - 1;
}

[ValueConversion(typeof(int), typeof(Rect))]
public class CountToViewportConverter : ValueConverter<int, Rect> {
	public override Rect Convert(int count, Type targetType, object parameter, CultureInfo culture) =>
		new(0, 0, GetToggleButtonsWidth(count), QuickSelectInterval1DEditor.ToggleButtonSize);

	internal static double GetToggleButtonsWidth(int count) => (QuickSelectInterval1DEditor.ToggleButtonSize + QuickSelectInterval1DEditor.ToggleButtonSpacing) * count;
}

public class CountToVisualBrushWidthConverter : MultiValueConverter<Tuple<int, double>, double> {
	public override double Convert(Tuple<int, double> value, Type targetType, object parameter, CultureInfo culture) {
		(int count, double actualWidth) = value;
		return Math.Max(0, actualWidth - CountToViewportConverter.GetToggleButtonsWidth(count) /*+ QuickSelectInterval1DEditor.ToggleButtonSpacing / 2*/);
	}
}
