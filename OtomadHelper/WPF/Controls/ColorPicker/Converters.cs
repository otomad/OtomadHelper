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
