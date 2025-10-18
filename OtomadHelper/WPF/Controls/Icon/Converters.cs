using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

using IconImage = System.Drawing.Icon;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class IconNameToImageSourceConverter : ValueConverter<string, ImageSource> {
	public override ImageSource Convert(string value, Type targetType, object parameter, CultureInfo culture) {
		string iconName = Icon.NormalizeIconName(value);

		if (Properties.Resources.ResourceManager.GetObject(iconName) is not IconImage iconImage)
			iconImage = Properties.Resources.Info;
		return iconImage.ToImageSource();
	}
}

public class IconAutoFilledConverter : IMultiValueConverter {
	public object Convert(object[] brushes, Type targetType, object parameter, CultureInfo culture) {
		return brushes.FirstOrDefault(brush => brush != DependencyProperty.UnsetValue && brush != Icon.defaultForeground && brush is Brush) ?? DependencyProperty.UnsetValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
