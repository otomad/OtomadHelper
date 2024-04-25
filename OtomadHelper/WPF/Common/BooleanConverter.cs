using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

public class BooleanConverter<T> : IValueConverter
	where T : notnull {
	public BooleanConverter(T trueValue, T falseValue) {
		True = trueValue;
		False = falseValue;
	}

	public T True { get; set; }
	public T False { get; set; }

	public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		value is bool boolean && boolean ? True : False;

	public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
		value is T t && EqualityComparer<T>.Default.Equals(t, True);
}

[ValueConversion(typeof(bool), typeof(Visibility))]
public sealed class BooleanToVisibilityConverter : BooleanConverter<Visibility> {
	public BooleanToVisibilityConverter() :
		base(Visibility.Visible, Visibility.Collapsed) { }
}
