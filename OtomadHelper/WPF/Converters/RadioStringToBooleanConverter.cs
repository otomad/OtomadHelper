using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Converters;

[ValueConversion(typeof(string), typeof(bool))]
public class RadioStringToBooleanConverter : IValueConverter {
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		string text = value.ToString();
		string current = parameter.ToString();
		return text == current;
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		return parameter;
	}
}
