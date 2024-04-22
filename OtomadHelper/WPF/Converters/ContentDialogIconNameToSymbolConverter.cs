using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Converters;

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
}
