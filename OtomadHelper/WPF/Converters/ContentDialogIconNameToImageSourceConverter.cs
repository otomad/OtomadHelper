using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Converters;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class ContentDialogIconNameToImageSourceConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		string iconName = new VariableName(value.ToString()).Pascal;

		if (Properties.Resources.ResourceManager.GetObject(iconName) is not Icon iconImage)
			iconImage = Properties.Resources.Info;
		return iconImage.ToImageSource();
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
