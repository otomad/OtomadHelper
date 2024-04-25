using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(string), typeof(string))]
public class ContentDialogIconNameToSymbolConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		string iconName = new VariableName(value.ToString()).Pascal;

		if (!SegoeIconNames.TryGetValue(iconName, out string symbol))
			symbol = DefaultIcon;
		return symbol;
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();

	public static readonly Dictionary<string, string> SegoeIconNames = new() {
		{ "Info", "\ue946" },
		{ "Warning", "\ue7ba" },
		{ "Error", "\uea39" },
		{ "Question", "\ue9ce" },
		{ "Locale", "\ue774" },
	};

	public const string DefaultIconName = "Info";
	public static string DefaultIcon => SegoeIconNames[DefaultIconName];

	public static bool IsValidIconName(string iconName) => SegoeIconNames.ContainsKey(iconName);
}

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
