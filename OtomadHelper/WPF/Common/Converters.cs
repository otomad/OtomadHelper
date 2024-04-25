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

[ValueConversion(typeof(double), typeof(CornerRadius))]
public class DoubleToCornerRadiusConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		double uniformRadius = (double)value;
		return new CornerRadius(uniformRadius);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		string side = (parameter ?? "top left").ToString().ToLower();
		CornerRadius cornerRadius = (CornerRadius)value;
		return side switch {
			string s when s.Contains("top") && s.Contains("right") => cornerRadius.TopRight,
			string s when s.Contains("bottom") && s.Contains("right") => cornerRadius.BottomRight,
			string s when s.Contains("bottom") && s.Contains("left") => cornerRadius.BottomLeft,
			_ => cornerRadius.TopLeft,
		};
	}
}

[ValueConversion(typeof(FrameworkElement), typeof(Rect))]
public class ActualSizeToRectConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		FrameworkElement element = (FrameworkElement)value;
		return new Rect(0, 0, element.ActualWidth, element.ActualHeight);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
