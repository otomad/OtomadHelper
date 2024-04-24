using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

[ValueConversion(typeof(double), typeof(Thickness))]
public class DoubleToThicknessConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		double uniformLength = (double)value;
		return new Thickness(uniformLength);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		string side = (parameter ?? "left").ToString().ToLower();
		Thickness thickness = (Thickness)value;
		return side switch {
			"top" => thickness.Top,
			"right" => thickness.Right,
			"bottom" => thickness.Bottom,
			_ => thickness.Left,
		};
	}
}
