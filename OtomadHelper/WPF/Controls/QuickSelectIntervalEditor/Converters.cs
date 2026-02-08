using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(int), typeof(int))]
public class AddOneConverter : ValueConverter<int, int> {
	public override int Convert(int zeroBased, Type targetType, object parameter, CultureInfo culture) => zeroBased + 1;

	public override int ConvertBack(int oneBased, Type targetType, object parameter, CultureInfo culture) => oneBased - 1;
}

[ValueConversion(typeof(int), typeof(Rect))]
public class CountToXViewportConverter : ValueConverter<int, Rect> {
	public override Rect Convert(int count, Type targetType, object parameter, CultureInfo culture) =>
		new(0, 0, GetToggleButtonsSize(count), QuickSelectInterval1DEditor.ToggleButtonSize + QuickSelectInterval1DEditor.ToggleButtonSpacing * 2);

	internal static double GetToggleButtonsSize(int count) => (QuickSelectInterval1DEditor.ToggleButtonSize + QuickSelectInterval1DEditor.ToggleButtonSpacing) * count;
}

public class CountToVisualBrushSizeConverter : MultiValueConverter<Tuple<int, double>, double> {
	public override double Convert(Tuple<int, double> value, Type targetType, object parameter, CultureInfo culture) {
		(int count, double viewportSize) = value;
		return Math.Max(0, viewportSize - CountToXViewportConverter.GetToggleButtonsSize(count));
	}
}

public class AlternationIndexToRowCellConverter : MultiValueConverter<int[], (int row, int cell)> {
	public override (int row, int cell) Convert(int[] value, Type targetType, object parameter, CultureInfo culture) =>
		(value[0] - 1, value[1] - 1);
}

public class CountToYViewportConverter : MultiValueConverter<Tuple<int, int>, Rect> {
	public override Rect Convert(Tuple<int, int> size, Type targetType, object parameter, CultureInfo culture) {
		(int columns, int rows) = size;
		return new(0, 0, CountToXViewportConverter.GetToggleButtonsSize(columns), CountToXViewportConverter.GetToggleButtonsSize(rows));
	}
}
