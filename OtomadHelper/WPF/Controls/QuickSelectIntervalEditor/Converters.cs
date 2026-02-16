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

public class AlternationIndexToRowColumnConverter : MultiValueConverter<int[], (int row, int column)> {
	public override (int row, int column) Convert(int[] value, Type targetType, object parameter, CultureInfo culture) =>
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

#pragma warning disable IDE0008 // 使用显式类型
[ValueConversion(typeof(int), typeof(string))]
public class QuickSelectInterval1DIndexToAutomationPropertiesConverter : ValueConverter<int, string, AutomationPropertiesProperty> {
	public override string Convert(int index, Type targetType, AutomationPropertiesProperty property, CultureInfo culture) {
		var tA = t.Descriptions.QuickSelectIntervalEditor.Aria.OneD;
		bool isHelpText = property == AutomationPropertiesProperty.HelpText;
		if (!isHelpText) index++;
		return string.Format(isHelpText ? tA.HelpText : tA.Name, index);
	}
}

[ValueConversion(typeof((int row, int column)), typeof(string))]
public class QuickSelectInterval2DRowColumnToAutomationPropertiesConverter : MultiValueConverter<(int row, int column), string, AutomationPropertiesProperty> {
	public override string Convert((int row, int column) location, Type targetType, AutomationPropertiesProperty property, CultureInfo culture) {
		(int row, int column) = location;
		var tA = t.Descriptions.QuickSelectIntervalEditor.Aria.TwoD;
		bool isHelpText = property == AutomationPropertiesProperty.HelpText;
		return string.Format(isHelpText ? tA.HelpText : tA.Name, column, row);
	}
}
#pragma warning restore IDE0008 // 使用显式类型
