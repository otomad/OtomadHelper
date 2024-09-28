using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

public class TextBoxStripeSizeToRectConverter : IMultiValueConverter {
	public double StripeHeight { get; set; }

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		double width = (double)values[0], height = (double)values[1], stripeHeight = values.Length > 2 ? (double)values[2] : StripeHeight;
		Rect rect = new(0, height - stripeHeight, width, height);
		return new RectangleGeometry(rect);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotSupportedException();
}
