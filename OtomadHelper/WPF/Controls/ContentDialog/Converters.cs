using System.Globalization;
using System.Windows;

namespace OtomadHelper.WPF.Controls;

public class WindowActualSizeToWindowCaptionButtonsRectConverter : MultiValueConverter<Tuple<double, double>, Rect> {
	public override Rect Convert(Tuple<double, double> size, Type targetType, object parameter, CultureInfo culture) {
		(double width, double height) = size;
		const double captionBtnsWidth = 48, captionBtnsHeight = 30;
		return new((width - captionBtnsWidth * 3) / width * 100, captionBtnsHeight / 2 / height * 100, captionBtnsWidth * 3 / width * 100, captionBtnsHeight / height * 100);
	}
}
