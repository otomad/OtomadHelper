using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

public class MultiBindingTuple : MultiBinding {
	public MultiBindingTuple() : base() {
		Converter = new MultiValueToTupleConverter();
	}
}

internal class MultiValueToTupleConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) =>
		values.ToTuple<ITuple>();

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
