using System.Globalization;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

public class MultiBindingTuple : MultiBinding {
	public MultiBindingTuple() : base() {
		Converter = new MultiValueToTupleConverter();
	}
}

internal class MultiValueToTupleConverter : IMultiValueConverter {
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		int length = values.Length;
		MethodInfo[] createTupleMethods = typeof(Tuple).GetMethods(BindingFlags.Public | BindingFlags.Static)!;
		MethodInfo? method = createTupleMethods.FirstOrDefault(method => method.GetParameters().Length == length);
		if (method is null)
			throw new Exception($"You can only create a tuple containing up to 8 items, currently providing {length} items");
		MethodInfo genericMethod = method.MakeGenericMethod(values.Select(item => item.GetType()).ToArray())!;
		return genericMethod.Invoke(null, values);
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
		throw new NotImplementedException();
}
