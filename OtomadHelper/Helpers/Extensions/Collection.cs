using System.Runtime.CompilerServices;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// When using <c>foreach</c> to traverse an <see cref="IEnumerable"/> object,
	/// it is allowed to provide an additional index value of the current item for use.
	/// </summary>
	/// <typeparam name="T">The item type of the <see cref="IEnumerable"/> object.</typeparam>
	/// <param name="collection"><see cref="IEnumerable"/> object.</param>
	/// <returns>A conversion function can be called containing the current item and index values.</returns>
	public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection) =>
		collection.Select((item, index) => (item, index));

	/// <summary>
	/// Get the value associated with the specified string key, ignoring its case.
	/// </summary>
	/// <typeparam name="TValue">The value type of the <paramref name="dict"/>.</typeparam>
	/// <param name="dict"><see cref="Dictionary{string, TValue}"/></param>
	/// <param name="key">The key to get the value from.</param>
	/// <param name="value">If the specified key is found, returns the value containing that key.</param>
	/// <returns>Does the <paramref name="dict"/> contain the key? Case insensitive.</returns>
	public static bool TryGetValueIgnoreCase<TValue>(this Dictionary<string, TValue> dict, string key, out TValue value) {
		IEnumerable<KeyValuePair<string, TValue>> result = dict.Where(x => x.Key.ToUpperInvariant() == key.ToUpperInvariant());
		value = result.FirstOrDefault().Value;
		return result.Count() > 0;
	}

	/// <summary>
	/// Adds the elements of the specified collection to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
	/// </summary>
	/// <param name="collection">
	/// The collection whose elements should be added to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
	/// The collection itself cannot be <see langword="null"/> but it can contain elements that are <see langword="null"/>,
	/// if type <typeparamref name="T"/> is a reference type.
	/// </param>
	public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection) {
		foreach (T item in collection)
			list.Add(item);
	}

	/// <summary>
	/// Get existing value or create and add new value in a <see cref="Dictionary{TKey, TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">Dictionary key type.</typeparam>
	/// <typeparam name="TValue">Dictionary value type.</typeparam>
	/// <param name="dict"><see cref="Dictionary{TKey, TValue}"/>.</param>
	/// <param name="key">Specify the key of the dictionary.</param>
	/// <param name="initial">
	/// <para>If the dictionary does't contains that <paramref name="key"/>, the initial value will be added in.</para>
	/// <para>Note that if it is got by calling complex methods, it will have unnecessary side effects.</para>
	/// </param>
	/// <returns>Existing value or initial value.</returns>
	public static TValue GetOrInit<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue initial) {
		if (!dict.TryGetValue(key, out TValue? value)) {
			value = initial;
			dict.Add(key, value);
		}
		return value!;
	}

	/// <inheritdoc cref="GetOrInit"/>
	/// <param name="CreateNew">If the dictionary does't contains that <paramref name="key"/>,
	/// the initial value will get from this function, and also add to the dictionary.</param>
	public static TValue GetOrInit<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, Func<TValue> CreateNew) {
		if (!dict.TryGetValue(key, out TValue? value)) {
			value = CreateNew();
			dict.Add(key, value);
		}
		return value!;
	}

	private static readonly ArgumentOutOfRangeException outOfRangeException = new();
	public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		rest = list.Skip(1).ToList();
	}
	public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		second = list.Count > 1 ? list[1] : throw outOfRangeException;
		rest = list.Skip(2).ToList();
	}
	public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		second = list.Count > 1 ? list[1] : throw outOfRangeException;
		third = list.Count > 2 ? list[2] : throw outOfRangeException;
		rest = list.Skip(3).ToList();
	}

	public static ITuple ToTuple(this IList<object> list, Type? tupleType = null) {
		tupleType ??= typeof(ITuple);
		int length = list.Count;
		MethodInfo[] createTupleMethods = typeof(Tuple).GetMethods(BindingFlags.Public | BindingFlags.Static)!;
		MethodInfo? method = createTupleMethods.FirstOrDefault(method => method.GetParameters().Length == length);
		if (method is null)
			throw new Exception($"You can only create a tuple containing up to 8 items, currently providing {length} items");
		Type[] genericArgs = tupleType.GenericTypeArguments;
		if (genericArgs.Length == 0) genericArgs = list.Select(item => item.GetType()).ToArray();
		MethodInfo genericMethod = method.MakeGenericMethod(genericArgs)!;
		return (ITuple)genericMethod.Invoke(null, list.ToArray());
	}

	public static TTuple ToTuple<TTuple>(this IList<object> list) where TTuple : ITuple =>
		(TTuple)list.ToTuple(typeof(TTuple));

	/// <inheritdoc cref="List{T}.IndexOf(T)"/>
	public static int IndexOf<T>(this List<T> list, T? item) where T : struct =>
		item is null ? -1 : list.IndexOf(item.Value);

	/// <inheritdoc cref="List{T}.IndexOf(T)"/>
	public static int IndexOf<T>(this IEnumerable<T> list, T? item) =>
		item is null ? -1 : list.ToList().IndexOf(item);

	/// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
	public static void ForEach<T>(this IEnumerable<T> list, Action<T> action) {
		foreach (T item in list)
			action(item);
	}

	/// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
	public static void ForEach<T>(this IEnumerable<T> list, Action<T, int> action) {
		int i = 0;
		foreach (T item in list)
			action(item, i++);
	}
}
