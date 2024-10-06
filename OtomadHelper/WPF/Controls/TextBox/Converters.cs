using System.Globalization;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

public class TextBoxStripeSizeToRectConverter : MultiValueConverter<double[], RectangleGeometry> {
	public double StripeHeight { get; set; }

	public override RectangleGeometry Convert(double[] values, Type targetType, object parameter, CultureInfo culture) {
		double width = values[0], height = values[1], stripeHeight = values.Length > 2 ? values[2] : StripeHeight;
		Rect rect = new(0, height - stripeHeight, width, height);
		return new(rect);
	}
}
