namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// When using <c>foreach</c> to traverse an <see cref="IEnumerable"/> object, it is allowed to provide an additional index
	/// value of the current item for use.
	/// </summary>
	/// <typeparam name="T">The item type of the <see cref="IEnumerable"/> object.</typeparam>
	/// <param name="collection"><see cref="IEnumerable"/> object.</param>
	/// <returns>A conversion function can be called containing the current item and index values.</returns>
	public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection) =>
		collection.Select((item, index) => (item, index));

	/// <summary>
	/// Get the value associated with the specified string key, ignoring its case.
	/// </summary>
	/// <typeparam name="TValue">The value type of <paramref name="dictionary"/>.</typeparam>
	/// <param name="dictionary"><see cref="Dictionary"/></param>
	/// <param name="key">The key to get the value from.</param>
	/// <param name="value">If the specified key is found, returns the value containing that key.</param>
	/// <returns><paramref name="dictionary"/> Does it contain the key? Case insensitive.</returns>
	public static bool TryGetValueIgnoreCase<TValue>(this Dictionary<string, TValue> dictionary, string key, out TValue value) {
		IEnumerable<KeyValuePair<string, TValue>> result = dictionary.Where(x => x.Key.ToUpperInvariant() == key.ToUpperInvariant());
		value = result.FirstOrDefault().Value;
		return result.Count() > 0;
	}

	/// <summary>
	/// Adds the elements of the specified collection to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
	/// </summary>
	/// <param name="collection">
	/// The collection whose elements should be added to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
	/// The collection itself cannot be <c>null</c>, but it can contain elements that are <c>null</c>, if type
	/// <typeparamref name="T"/> is a reference type.
	/// </param>
	public static void AddRange<T>(this IList<T> list, IEnumerable<T> collection) {
		foreach (T item in collection)
			list.Add(item);
	}
}
