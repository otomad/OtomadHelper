namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// 在使用 <c>foreach</c> 遍历可迭代对象时，允许额外提供一个当前项的索引值以便使用。
	/// </summary>
	/// <typeparam name="T">可迭代对象的项目类型。</typeparam>
	/// <param name="collection">可迭代对象。</param>
	/// <returns>可调用包含当前项目和索引值的转换函数。</returns>
	public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> collection) =>
		collection.Select((item, index) => (item, index));

	/// <summary>
	/// 获取与指定字符串键关联的值，但忽略键的大小写。
	/// </summary>
	/// <typeparam name="TValue"><paramref name="dictionary"/> 的值类型。</typeparam>
	/// <param name="dictionary"><see cref="Dictionary"/></param>
	/// <param name="key">要获取值的键。</param>
	/// <param name="value">如果找到指定键，则包含该键的值。</param>
	/// <returns><paramref name="dictionary"/> 是否包含该键？不区分大小写。</returns>
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
