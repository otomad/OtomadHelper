using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

using IconImage = System.Drawing.Icon;

namespace OtomadHelper.WPF.Controls;

[Obsolete()]
[ValueConversion(typeof(string), typeof(string))]
public class IconNameToSymbolConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		string iconName = Icon.NormalizeIconName(value);

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
public class IconNameToImageSourceConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		string iconName = Icon.NormalizeIconName(value);

		if (Properties.Resources.ResourceManager.GetObject(iconName) is not IconImage iconImage)
			iconImage = Properties.Resources.Info;
		return iconImage.ToImageSource();
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
