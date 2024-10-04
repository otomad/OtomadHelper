using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using Wacton.Unicolour;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(ColorPickerModelAxis), typeof(bool))]
public class ColorPickerModelAxisToCheckedConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		ColorPickerModelAxis modelAxis = (ColorPickerModelAxis)value;
		return modelAxis == ColorPickerModelAxis.FromName((string)parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		return ColorPickerModelAxis.FromName((string)parameter);
	}
}

[ValueConversion(typeof(Dictionary<ColorPickerModelAxis, double>), typeof(string))]
public class ColorPickerValuesToTextConverter : IValueConverter {
	[SuppressMessage("Style", "IDE0008")]
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		var dictionary = (Dictionary<ColorPickerModelAxis, double>)value;
		return dictionary.TryGetValue(ColorPickerModelAxis.FromName((string)parameter), out double result) ? Math.Round(result).ToString() : string.Empty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(TextChangedEventArgs), typeof(ValueTuple<string, string>))]
public class ColorPickerTextChangedEventArgsToTextAndNameConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		TextChangedEventArgs e = (TextChangedEventArgs)value;
		TextBox textBox = (TextBox)e.Source;
		return (textBox.Text, (string)textBox.Tag);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(Unicolour), typeof(Color))]
public class UnicolourToMediaColorConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		Unicolour color = (Unicolour)value;
		return color.ToMediaColor();
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		Color color = (Color)value;
		return color.ToUnicolour();
	}
}

public class TrackThumbInnerBaseMultiplySizeConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
		values.Cast<double>().ToArray().Aggregate(1d, (a, b) => a * b);

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}

[ValueConversion(typeof(ColorPickerModelAxis), typeof(Range))]
public class ColorPickerModelAxisToRangeConverter : IMultiValueConverter {
	[SuppressMessage("Style", "IDE0008")]
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		(ColourSpace model, int axis) = (ColorPickerModelAxis)values[0];
		var range = ColorPickerViewModel.GetOutputRange(model);
		int xyzIndex = (int)parameter;
		int pointXyz = GetPointXyz(xyzIndex, axis);
		return range.Get<Range>(pointXyz);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();

	public static int GetPointXyz(int xyzIndex, int axis) {
		List<int> xyzMap = [0, 1, 2];
		xyzMap.Remove(axis);
		xyzMap.Add(axis);
		return xyzMap[xyzIndex];
	}
}

[ValueConversion(typeof(Unicolour), typeof(double))]
public class UnicolourToPointXyzConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		Unicolour color = (Unicolour)values[0];
		(ColourSpace model, int axis) = (ColorPickerModelAxis)values[1];
		int xyzIndex = (int)parameter;
		int pointXyz = ColorPickerModelAxisToRangeConverter.GetPointXyz(xyzIndex, axis);
		return ColorPickerViewModel.ToTriplet(color, model).Get<double>(pointXyz);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
