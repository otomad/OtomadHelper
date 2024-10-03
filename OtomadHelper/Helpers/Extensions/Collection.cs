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
	/// <inheritdoc cref="Deconstruct{T}(IList{T}, out T, out T, out T, out IList{T})"/>
	public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		rest = list.Skip(1).ToList();
	}
	/// <inheritdoc cref="Deconstruct{T}(IList{T}, out T, out T, out T, out IList{T})"/>
	public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		second = list.Count > 1 ? list[1] : throw outOfRangeException;
		rest = list.Skip(2).ToList();
	}
	/// <summary>
	/// Ability to deconstruct a <paramref name="list"/> into each elements and the rest of the list.
	/// </summary>
	/// <remarks>
	/// <example>
	/// <code>
	/// var (foo, bar, baz, rest) = list;
	///
	/// // Don't care about the rest
	/// var (foo, bar, baz, _) = list;
	///
	/// // Any count of list items
	/// var (foo, (bar, (baz, rest))) = list;
	/// </code>
	/// </example>
	/// </remarks>
	/// <typeparam name="T">The type of elements in the <paramref name="list"/>.</typeparam>
	/// <param name="list">The list to deconstruct.</param>
	/// <param name="first">The first element of the <paramref name="list"/>.</param>
	/// <param name="second">The second element of the <paramref name="list"/>.</param>
	/// <param name="third">The third element of the <paramref name="list"/>.</param>
	/// <param name="rest">The rest of the list after the first three elements.</param>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when the list contains less than the specified elements.</exception>
	public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out IList<T> rest) {
		first = list.Count > 0 ? list[0] : throw outOfRangeException;
		second = list.Count > 1 ? list[1] : throw outOfRangeException;
		third = list.Count > 2 ? list[2] : throw outOfRangeException;
		rest = list.Skip(3).ToList();
	}

	/// <summary>
	/// Converts a list of objects into a tuple.
	/// </summary>
	/// <param name="list">The list of objects to convert into a tuple.</param>
	/// <param name="tupleType">The type of the tuple to create. If not provided, it will get the real type of the object item.</param>
	/// <returns>An instance of the specified tuple type containing the elements from the input list.</returns>
	/// <exception cref="Exception">Thrown when the input list contains more than 8 items, as tuples can only contain up to 8 items.</exception>
	public static ITuple ToTuple(this IList<object> list, Type? tupleType = null) {
		tupleType ??= typeof(ITuple);
		int length = list.Count;
		MethodInfo[] createTupleMethods = typeof(Tuple).GetMethods(BindingFlags.Public | BindingFlags.Static)!;
		MethodInfo? method = createTupleMethods.FirstOrDefault(method => method.GetParameters().Length == length) ??
			throw new Exception($"You can only create a tuple containing up to 8 items, currently providing {length} items");
		Type[] genericArgs = tupleType.GenericTypeArguments;
		if (genericArgs.Length == 0) genericArgs = list.Select(item => item.GetType()).ToArray();
		MethodInfo genericMethod = method.MakeGenericMethod(genericArgs)!;
		return (ITuple)genericMethod.Invoke(null, [.. list]);
	}

	/// <summary>
	/// Converts a list of objects into a tuple.
	/// </summary>
	/// <typeparam name="TTuple">The type of the tuple to create. If not provided, it will get the real type of the object item.</typeparam>
	/// <param name="list">The list of objects to convert into a tuple.</param>
	/// <returns>An instance of the specified tuple type containing the elements from the input list.</returns>
	/// <exception cref="Exception">Thrown when the input list contains more than 8 items, as tuples can only contain up to 8 items.</exception>
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

	/// <summary>
	/// Converts a <see cref="JsonElement"/> to a JSON string.
	/// </summary>
	/// <param name="jsonElement">The <see cref="JsonElement"/> to convert.</param>
	/// <returns>A JSON string representation of the <paramref name="jsonElement"/>.</returns>
	/// <remarks>
	/// This method uses <see cref="Utf8JsonWriter"/> to write the <paramref name="jsonElement"/> to a memory stream,
	/// and then converts the stream to a UTF-8 string.
	/// </remarks>
	public static string ToJsonString(this JsonElement jsonElement) {
		using MemoryStream stream = new();
		Utf8JsonWriter writer = new(stream, new JsonWriterOptions { Indented = true });
		jsonElement.WriteTo(writer);
		writer.Flush();
		return Encoding.UTF8.GetString(stream.ToArray());
	}

	/// <inheritdoc cref="Dictionary{int, T}.TryGetValue(int, out T)"/>
	public static bool TryGetValue<T>(this IList<T> list, int index, out T value) {
		try {
			T result = list[index];
			value = result;
			return true;
		} catch (IndexOutOfRangeException) {
			value = default!;
			return false;
		}
	}

	/// <summary>
	/// Convert a <see cref="Tuple" /> or <see cref="ValueTuple" /> to <see cref="Array" />.
	/// </summary>
	/// <param name="tuple"><see cref="Tuple" /> or <see cref="ValueTuple" />.</param>
	public static T[] ToArray<T>(this ITuple tuple) {
		T[] array = new T[tuple.Length];
		for (int i = 0; i < tuple.Length; i++)
			array[i] = (T)tuple[i];
		return array;
	}

	/// <summary>
	/// Get <see cref="Tuple" /> or <see cref="ValueTuple" /> item value by its index.
	/// </summary>
	/// <param name="tuple"><see cref="Tuple" /> or <see cref="ValueTuple" />.</param>
	/// <param name="index">The index of the item.</param>
	public static T Get<T>(this ITuple tuple, int index) => (T)tuple[index];
}
