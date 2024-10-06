using System.Globalization;
using System.Runtime.CompilerServices;

namespace OtomadHelper.WPF.Common;

public class CollectionConverter<T> : TypeConverter where T : IEnumerable {
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
		sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
		destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

	private static readonly Regex removeParenthesisRegex = new(@"^(\((?<content>.*)\)|\[(?<content>.*)\]|\{(?<content>.*)\})$");
	public static string RemoveParenthesis(ref string source) {
		source = source.Trim();
		Match match = source.Match(removeParenthesisRegex);
		if (match.Success) source = match.Groups["content"].Value;
		return source;
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
		if (value is null) throw GetConvertFromException(value);
		if (value is not string source) return base.ConvertFrom(context, culture, value);
		RemoveParenthesis(ref source);
		if (!typeof(T).TryGetIEnumerableType(out Type enumerable, out Type itemType)) goto UnknownType;
		string[] items = source.Split(',', ';');
		int length = items.Length;
		if (typeof(T).Extends(typeof(Array))) {
			IList array = Array.CreateInstance(itemType, length);
			for (int i = 0; i < length; i++)
				array[i] = Convert.ChangeType(items[i], itemType);
			return (T)array;
		} else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>)) {
			IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType), [length]);
			foreach (object item in items)
				list.Add(Convert.ChangeType(items, itemType));
			return (T)list;
		} else // Lazy to implement Dictionary and HashSet
			goto UnknownType;
	UnknownType:
		throw new ArgumentException($"The argument type `{typeof(T)}` is not supported");
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
		destinationType == typeof(string) ? value.ToString() : base.ConvertTo(context, culture, value, destinationType);
}

public class TupleConverter<T> : TypeConverter where T : ITuple {
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
		sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) =>
		destinationType == typeof(string) || base.CanConvertTo(context, destinationType);

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
		if (value is null) throw GetConvertFromException(value);
		if (value is not string source) return base.ConvertFrom(context, culture, value);
		CollectionConverter<IEnumerable>.RemoveParenthesis(ref source);
		if (!typeof(T).Extends(typeof(ITuple)) || !typeof(T).IsGenericType) goto UnknownType;
		IEnumerable<object> items = source.Split(',', ';').Select((item, index) => Convert.ChangeType(item, typeof(T).GenericTypeArguments[index]));
		return items.ToTuple(typeof(T));
	UnknownType:
		throw new ArgumentException($"The argument type `{typeof(T)}` is not supported");
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
		destinationType == typeof(string) ? value.ToString() : base.ConvertTo(context, culture, value, destinationType);
}
