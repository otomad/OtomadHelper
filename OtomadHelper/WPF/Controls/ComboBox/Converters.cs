using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

[ValueConversion(typeof(string), typeof(bool))]
public class ComboBoxIdToIsCheckedConverter : IValueConverter {
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		// Both value and parameter are object types, if they are actually string types, it will return false,
		// because two same strings are probably two different objects. So we convert them to dynamic types,
		// then they will apply the override equal operator correctly.
		return (dynamic)value == (dynamic)parameter;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		return (bool)value ? parameter : DependencyProperty.UnsetValue;
	}
}

[ValueConversion(typeof(string), typeof(string))]
public class ComboBoxIdToOptionConverter : IValueConverter {
	private static (object[] ids, string[] options) GetParameter(object parameter) {
		object[] @params = (object[])parameter;
		object[] ids = [.. (ObservableCollection<object>)@params[0]];
		string[] options = [.. (ObservableCollection<string>)@params[1]];
		return (ids, options);
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		(object[] ids, string[] options) = GetParameter(parameter);
		int index = ids.IndexOf(value);
		if (!options.TryGetValue(index, out string option))
			option = value is string id ? $"<{id}>" : string.Empty;
		return option;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		(object[] ids, string[] options) = GetParameter(parameter);
		int index = options.IndexOf(value);
		if (!ids.TryGetValue(index, out object id))
			id = value is string option ? $"<{option}>" : string.Empty;
		return id;
	}
}
