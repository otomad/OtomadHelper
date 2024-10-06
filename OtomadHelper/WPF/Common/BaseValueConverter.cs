using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace OtomadHelper.WPF.Common;

//[ValueConversion(typeof(TSource), typeof(TTarget))]
public abstract class ValueConverter<TSource, TTarget> : ValueConverter<TSource, TTarget, object> { }
public abstract class ValueConverter<TSource, TTarget, TParameter> : IValueConverter {
	public abstract TTarget Convert(TSource value, Type targetType, TParameter parameter, CultureInfo culture);
	public virtual TSource ConvertBack(TTarget value, Type targetType, TParameter parameter, CultureInfo culture) =>
		throw new NotImplementedException();

	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
		TParameter param = MultiValueConverter<TSource, TTarget, TParameter>.ToCollectionType<TParameter>(parameter);
		return Convert((TSource)value, targetType, param, culture)!;
	}
	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
		TParameter param = MultiValueConverter<TSource, TTarget, TParameter>.ToCollectionType<TParameter>(parameter);
		return ConvertBack((TTarget)value, targetType, param, culture)!;
	}
}

public abstract class MultiValueConverter<TSource, TTarget> : MultiValueConverter<TSource, TTarget, object> { }
public abstract class MultiValueConverter<TSource, TTarget, TParameter> : IMultiValueConverter {
	public abstract TTarget Convert(TSource value, Type targetType, TParameter parameter, CultureInfo culture);
	public virtual TSource ConvertBack(TTarget value, Type[] targetType, TParameter parameter, CultureInfo culture) =>
		throw new NotImplementedException();

	object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
		TParameter param = ToCollectionType<TParameter>(parameter);
		TSource source = ToCollectionType<TSource>(values);
		return Convert(source, targetType, param, culture)!;
	}

	object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
		TParameter param = ToCollectionType<TParameter>(parameter);
		TSource sources = ConvertBack((TTarget)value, targetTypes, param, culture);
		return ToCollectionType<object[]>(sources!);
	}

	public static T ToCollectionType<T>(object source, bool throwIfNotCollection = false) {
		if (source is not object[] sources)
			goto UnknownType;
		if (typeof(T).Extends(typeof(ITuple)))
			return (T)sources.ToTuple(typeof(T));
		if (typeof(T).TryGetIEnumerableType(out Type enumerable, out Type itemType)) {
			int length = sources.Length;
			if (typeof(T).Extends(typeof(Array))) {
				IList array = Array.CreateInstance(itemType, length);
				for (int i = 0; i < length; i++)
					array[i] = sources[i];
				return (T)array;
			} else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>)) {
				IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType), [length]);
				foreach (object item in sources)
					list.Add(item);
				return (T)list;
			} else // Lazy to implement Dictionary and HashSet
				goto UnknownType;
		}
	UnknownType:
		return throwIfNotCollection ? throw new ArgumentException($"The argument {source} is not an object array type") : (T)source;
	}
}
