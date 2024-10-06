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
		Type enumerable = typeof(T).GetInterface("IEnumerable`1");
		if (enumerable is null) throw new ArgumentException($"The target type `{typeof(T)}` is not supported");
		Type itemType = enumerable.GenericTypeArguments[0];
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) =>
		destinationType == typeof(string) ? value.ToString() : base.ConvertTo(context, culture, value, destinationType);
}

public class TupleConverter<T> : TypeConverter where T : ITuple {

}
