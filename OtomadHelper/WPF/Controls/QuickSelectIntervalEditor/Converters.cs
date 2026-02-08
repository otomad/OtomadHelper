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

public class AlternationIndexToRowCellConverter : MultiValueConverter<int[], (int row, int cell)> {
	public override (int row, int cell) Convert(int[] value, Type targetType, object parameter, CultureInfo culture) =>
		(value[0] - 1, value[1] - 1);
}
