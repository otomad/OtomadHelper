using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(int), typeof(int))]
public class AddOneConverter : ValueConverter<int, int> {
	public override int Convert(int zeroBased, Type targetType, object parameter, CultureInfo culture) => zeroBased + 1;

	public override int ConvertBack(int oneBased, Type targetType, object parameter, CultureInfo culture) => oneBased - 1;
}

[ValueConversion(typeof(int), typeof(Rect))]
public class CountToInlineViewportConverter : ValueConverter<int, Rect, bool> {
	public override Rect Convert(int count, Type targetType, bool isYAxis = false, CultureInfo? culture = null) {
		double width = GetToggleButtonsSize(count), height = QuickSelectInterval1DEditor.ToggleButtonSize + QuickSelectInterval1DEditor.ToggleButtonSpacing * 2;
		return !isYAxis ? new(0, 0, width, height) : new(0, 0, height, width);
	}

	internal static double GetToggleButtonsSize(int count) => (QuickSelectInterval1DEditor.ToggleButtonSize + QuickSelectInterval1DEditor.ToggleButtonSpacing) * count;
}

public class CountToVisualBrushSizeConverter : MultiValueConverter<Tuple<int, double>, double> {
	public override double Convert(Tuple<int, double> value, Type targetType, object parameter, CultureInfo culture) {
		(int count, double viewportSize) = value;
		return Math.Max(0, viewportSize - CountToInlineViewportConverter.GetToggleButtonsSize(count));
	}
}

public class AlternationIndexToRowCellConverter : MultiValueConverter<int[], (int row, int cell)> {
	public override (int row, int cell) Convert(int[] value, Type targetType, object parameter, CultureInfo culture) =>
		(value[0] - 1, value[1] - 1);
}

public class CountToBlockViewportConverter : MultiValueConverter<Tuple<int, int>, Rect> {
	public override Rect Convert(Tuple<int, int> size, Type targetType, object parameter, CultureInfo culture) {
		(int columns, int rows) = size;
		return new(0, 0, CountToInlineViewportConverter.GetToggleButtonsSize(columns), CountToInlineViewportConverter.GetToggleButtonsSize(rows));
	}
}

[ValueConversion(typeof(int), typeof(int[]))]
public class CountToCollectionConverter : ValueConverter<int, int[]> {
	public override int[] Convert(int count, Type targetType, object parameter, CultureInfo culture) =>
		Enumerable.Range(1, count).ToArray();
}

[ValueConversion(typeof(double), typeof(bool))]
public class ContentWidthToIsWideConverter : ValueConverter<double, bool> {
	public override bool Convert(double width, Type targetType, object parameter, CultureInfo culture) => width >= QuickSelectInterval2DEditor.WideThreshold;
}
