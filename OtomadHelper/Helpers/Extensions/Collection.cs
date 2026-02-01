using System.Runtime.CompilerServices;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	extension<T>(IEnumerable<T> collection) {
		/// <summary>
		/// When using <c>foreach</c> to traverse an <see cref="IEnumerable"/> object,
		/// it is allowed to provide an additional index value of the current item for use.
		/// </summary>
		/// <typeparam name="T">The item type of the <see cref="IEnumerable"/> object.</typeparam>
		/// <param name="collection"><see cref="IEnumerable"/> object.</param>
		/// <returns>A conversion function can be called containing the current item and index values.</returns>
		public IEnumerable<(T item, int index)> WithIndex() =>
			collection.Select((item, index) => (item, index));

		/// <summary>
		/// Get the index <see cref="IEnumerable{T}" /> of the <paramref name="collection" />.
		/// </summary>
		/// <remarks>
		/// For example, if the length of the <paramref name="collection" /> is <see langword="4" />,
		/// this will return <c>IEnumerable&lt;int&gt; { 0, 1, 2, 3 }</c>.
		/// </remarks>
		public IEnumerable<int> Keys() => Enumerable.Range(0, collection.Count());
	}

	extension<T>(IEnumerable<T?> collection) {
		/// <summary>
		/// Remove all null from a nullable collection, and return a new <typeparamref name="T" /> collection which have no null in it.
		/// </summary>
		/// <returns>A new <typeparamref name="T" /> collection which have no null in it.</returns>
		public IEnumerable<T> NonNull() => collection.OfType<T>();
	}

	extension<TValue>(Dictionary<string, TValue> dict) {
		/// <summary>
		/// Get the value associated with the specified string key, ignoring its case.
		/// </summary>
		/// <typeparam name="TValue">The value type of the <paramref name="dict"/>.</typeparam>
		/// <param name="dict"><see cref="Dictionary{string, TValue}"/></param>
		/// <param name="key">The key to get the value from.</param>
		/// <param name="value">If the specified key is found, returns the value containing that key.</param>
		/// <returns>Does the <paramref name="dict"/> contain the key? Case insensitive.</returns>
		public bool TryGetValueIgnoreCase(string key, out TValue value) {
			IEnumerable<KeyValuePair<string, TValue>> result = dict.Where(x => x.Key.ToUpperInvariant() == key.ToUpperInvariant());
			value = result.FirstOrDefault().Value;
			return result.Any();
		}
	}

	extension<T>(IList<T> list) {
		/// <summary>
		/// Adds the elements of the specified collection to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
		/// </summary>
		/// <param name="list"><see cref="IList{T}" />.</param>
		/// <param name="collection">
		/// The collection whose elements should be added to the end of the <see cref="IList"/>&lt;<typeparamref name="T"/>&gt;.
		/// The collection itself cannot be <see langword="null"/> but it can contain elements that are <see langword="null"/>,
		/// if type <typeparamref name="T"/> is a reference type.
		/// </param>
		public void AddRange(IEnumerable<T> collection) {
			foreach (T item in collection)
				list.Add(item);
		}
		/// <inheritdoc cref="Deconstruct{T}(IList{T}, out T, out T, out T, out IList{T})"/>
		public void Deconstruct(out T first, out IList<T> rest) {
			first = list.Count > 0 ? list[0] : throw outOfRangeException;
			rest = list.Skip(1).ToList();
		}
		/// <inheritdoc cref="Deconstruct{T}(IList{T}, out T, out T, out T, out IList{T})"/>
		public void Deconstruct(out T first, out T second, out IList<T> rest) {
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
		public void Deconstruct(out T first, out T second, out T third, out IList<T> rest) {
			first = list.Count > 0 ? list[0] : throw outOfRangeException;
			second = list.Count > 1 ? list[1] : throw outOfRangeException;
			third = list.Count > 2 ? list[2] : throw outOfRangeException;
			rest = list.Skip(3).ToList();
		}
		/// <inheritdoc cref="Deconstruct{T}(IList{T}, out T, out T, out T, out IList{T})"/>
		public void Deconstruct(out T first, out T second, out T third, out T fourth, out IList<T> rest) {
			first = list.Count > 0 ? list[0] : throw outOfRangeException;
			second = list.Count > 1 ? list[1] : throw outOfRangeException;
			third = list.Count > 2 ? list[2] : throw outOfRangeException;
			fourth = list.Count > 3 ? list[3] : throw outOfRangeException;
			rest = list.Skip(4).ToList();
		}

		/// <inheritdoc cref="Dictionary{int, T}.TryGetValue(int, out T)"/>
		public bool TryGetValue(int index, out T value) {
			if (index >= 0 && index < list.Count) {
				value = list[index];
				return true;
			} else {
				value = default!;
				return false;
			}
		}

		/// <summary>
		/// Run null-conditional operator (<c>??</c>) for each items in an <see cref="IList{T}" />.
		/// </summary>
		/// <remarks>
		/// <para>
		/// For example, if you pass 0 to <paramref name="def" />, every <see langword="null" />
		/// in the <see cref="IList{T}" /> will be replaced to 0.
		/// </para>
		/// <para>
		/// Note: This will modify the original <paramref name="list" />.
		/// </para>
		/// </remarks>
		/// <param name="def">The value to replace nulls with.</param>
		public void FillNullsWith(T def) {
			for (int i = 0; i < list.Count; i++)
				list[i] ??= def;
		}
	}

	extension<TKey, TValue>(Dictionary<TKey, TValue> dict) {
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
		public TValue GetOrInit(TKey key, TValue initial) {
			if (!dict.TryGetValue(key, out TValue? value)) {
				value = initial;
				dict.Add(key, value);
			}
			return value!;
		}

		/// <inheritdoc cref="GetOrInit{TKey,TValue}(System.Collections.Generic.Dictionary{TKey,TValue},TKey,TValue)"/>
		/// <param name="CreateNew">If the dictionary does't contains that <paramref name="key"/>,
		/// the initial value will get from this function, and also add to the dictionary.</param>
		public TValue GetOrInit(TKey key, Func<TValue> CreateNew) {
			if (!dict.TryGetValue(key, out TValue? value)) {
				value = CreateNew();
				dict.Add(key, value);
			}
			return value!;
		}
	}

	private static readonly ArgumentOutOfRangeException outOfRangeException = new();

	extension(IEnumerable<object> list) {
		/// <summary>
		/// Converts a list of objects into a tuple.
		/// </summary>
		/// <param name="list">The list of objects to convert into a tuple.</param>
		/// <param name="tupleType">The type of the tuple to create. If not provided, it will get the real type of the object item.</param>
		/// <returns>An instance of the specified tuple type containing the elements from the input list.</returns>
		/// <exception cref="Exception">Thrown when the input list contains more than 8 items, as tuples can only contain up to 8 items.</exception>
		public ITuple ToTuple(Type? tupleType = null) {
			tupleType ??= typeof(ITuple);
			int length = list.Count();
			Type tupleBaseType = tupleType?.FullName?.StartsWith(typeof(ValueTuple).FullName!) == true ? typeof(ValueTuple) : typeof(Tuple);
			MethodInfo[] createTupleMethods = tupleBaseType.GetMethods(BindingFlags.Public | BindingFlags.Static)!;
			MethodInfo? method = createTupleMethods.FirstOrDefault(method => method.GetParameters().Length == length) ??
				throw new($"You can only create a tuple containing up to 8 items, currently providing {length} items");
			Type[] genericArgs = tupleType!.GenericTypeArguments;
			if (genericArgs.Length == 0)
				genericArgs = list.Select(item => item.GetType()).ToArray();
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
		public TTuple ToTuple<TTuple>() where TTuple : ITuple =>
			(TTuple)list.ToTuple(typeof(TTuple));
	}

	extension<T>(List<T> list) where T : struct {
		/// <inheritdoc cref="List{T}.IndexOf(T)"/>
		public int IndexOf(T? item) =>
			item is null ? -1 : list.IndexOf(item.Value);
	}

	extension<T>(IEnumerable<T> list) {
		/// <inheritdoc cref="List{T}.IndexOf(T)"/>
		public int IndexOf(T? item) =>
			item is null ? -1 : list.ToList().IndexOf(item);

		/// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
		public void ForEach(Action<T> action) {
			foreach (T item in list)
				action(item);
		}

		/// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
		public void ForEach(Action<T, int> action) {
			int i = 0;
			foreach (T item in list)
				action(item, i++);
		}
	}

	extension(JsonElement jsonElement) {
		/// <summary>
		/// Converts a <see cref="JsonElement"/> to a JSON string.
		/// </summary>
		/// <param name="jsonElement">The <see cref="JsonElement"/> to convert.</param>
		/// <returns>A JSON string representation of the <paramref name="jsonElement"/>.</returns>
		/// <remarks>
		/// This method uses <see cref="Utf8JsonWriter"/> to write the <paramref name="jsonElement"/> to a memory stream,
		/// and then converts the stream to a UTF-8 string.
		/// </remarks>
		public string ToJsonString() {
			using MemoryStream stream = new();
			Utf8JsonWriter writer = new(stream, new() { Indented = true });
			jsonElement.WriteTo(writer);
			writer.Flush();
			return Encoding.UTF8.GetString(stream.ToArray());
		}
	}

	extension(ITuple tuple) {
		/// <summary>
		/// Convert a <see cref="Tuple" /> or <see cref="ValueTuple" /> to <see cref="Array" />.
		/// </summary>
		/// <param name="tuple"><see cref="Tuple" /> or <see cref="ValueTuple" />.</param>
		public T[] ToArray<T>() {
			T[] array = new T[tuple.Length];
			for (int i = 0; i < tuple.Length; i++)
				array[i] = (T)tuple[i];
			return array;
		}

		/// <summary>
		/// Convert a <see cref="Tuple" /> or <see cref="ValueTuple" /> to <see cref="List{T}" />.
		/// </summary>
		/// <param name="tuple"><see cref="Tuple" /> or <see cref="ValueTuple" />.</param>
		public List<T> ToList<T>() =>
			tuple.ToArray<T>().ToList();

		/// <summary>
		/// Get <see cref="Tuple" /> or <see cref="ValueTuple" /> item value by its index.
		/// </summary>
		/// <param name="tuple"><see cref="Tuple" /> or <see cref="ValueTuple" />.</param>
		/// <param name="index">The index of the item.</param>
		public T Get<T>(int index) => (T)tuple[index];
	}

	extension<T>(IEnumerable<T> source) {
		/// <summary>
		/// Check if the <see cref="IEnumerable{T}"/> has any item with the specified index.
		/// </summary>
		public bool HasIndex(int index) =>
			index >= 0 && source.Count() > index;

		/// <inheritdoc cref="Enumerable.FirstOrDefault{TSource}(IEnumerable{TSource})" />
		/// <param name="def">Default value.</param>
		public T FirstOrDefault(T def) =>
			source.Any() ? source.First() : def;

		/// <inheritdoc cref="Enumerable.LastOrDefault{TSource}(IEnumerable{TSource})" />
		/// <param name="def">Default value.</param>
		public T LastOrDefault(T def) =>
			source.Any() ? source.Last() : def;

		/// <inheritdoc cref="Enumerable.ElementAtOrDefault{TSource}(IEnumerable{TSource}, int)" />
		/// <param name="def">Default value.</param>
		public T ElementAtOrDefault(int index, T def) =>
			source.HasIndex(index) ? source.ElementAt(index) : def;
	}

	extension<TKey, TValue>(Dictionary<TKey, TValue> dictionary) {
		/// <summary>
		/// Get dictionary key by value.
		/// </summary>
		/// <remarks>
		/// Throw if the value doesn't match any <see cref="KeyValuePairs" />.
		/// </remarks>
		/// <typeparam name="TValue">The value type must override <see cref="Object.Equals(object)" /> method.</typeparam>
		/// <param name="value">The value type must override <see cref="Object.Equals(object)" /> method.</param>
		/// <returns>The first key that match the value.</returns>
		/// <exception cref="ArgumentNullException" />
		public TKey GetKeyByValue(TValue value) =>
			dictionary.First(pair => EqualityComparer<TValue>.Default.Equals(pair.Value, value)).Key;
	}

	extension<TKey, TValue>(KeyValuePair<TKey, TValue> pair) {
		public void Deconstruct(out TKey key, out TValue value) {
			key = pair.Key;
			value = pair.Value;
		}
	}

	extension(IEnumerable source) {
		/// <inheritdoc cref="Enumerable.Count{TSource}(IEnumerable{TSource})" />
		public int Count() {
			int count = 0;
			foreach (object? _ in source)
				count++;
			return count;
		}
	}

	extension(NameValueCollection collection) {
		/// <summary>
		/// Check if the specified key is in the <see cref="NameValueCollection" />?
		/// </summary>
		/// <remarks>
		/// Note: You cannot use <c>collection[key] == null</c>, this is because the value belong with the key may be null.
		/// </remarks>
		/// <param name="collection"><see cref="NameValueCollection" /></param>
		/// <param name="key">The key to check.</param>
		public bool HasKey(string key) =>
			collection.AllKeys.Contains(key);
	}

	extension(byte[] bytes) {
		/// <summary>
		/// Writes the specified byte array <paramref name="data"/> into the current <see cref="byte"/> array starting at the given <paramref name="offset"/>.
		/// </summary>
		/// <param name="offset">The zero-based index in the current <see cref="byte"/> array at which to begin writing.</param>
		/// <param name="data">The byte array to write. If <see langword="null"/>, the method returns <see langword="false"/>.</param>
		/// <param name="littleEndian">If <see langword="true"/>, reverses the <paramref name="data"/> array before writing to ensure little-endian order.</param>
		/// <returns><see langword="true"/> if the write operation was successful; otherwise, <see langword="false"/>.</returns>
		public bool Write(int offset, byte[]? data, bool littleEndian = false) {
			if (data is null) return false;
			if (littleEndian) Array.Reverse(data);
			for (int i = offset, j = 0; i < bytes.Length && j < data.Length; i++, j++)
				bytes[i] = data[j];
			return true;
		}

		/// <summary>
		/// Writes the specified hexadecimal string <paramref name="hexString"/> into the current <see cref="byte"/> array starting at the given <paramref name="offset"/>.
		/// </summary>
		/// <param name="offset">The zero-based index in the current <see cref="byte"/> array at which to begin writing.</param>
		/// <param name="hexString">The hexadecimal string to write. Spaces will be removed before processing. The string must contain only valid hexadecimal characters and have an even length.</param>
		/// <param name="littleEndian">If <see langword="true"/>, reverses the byte array before writing to ensure little-endian order.</param>
		/// <returns><see langword="true"/> if the write operation was successful; otherwise, <see langword="false"/>.</returns>
		public bool Write(int offset, string hexString, bool littleEndian = false) {
			hexString = hexString.Replace(" ", "");
			if (hexString.Length % 2 == 1) return false;
			if (Regex.Matches(hexString, @"[^0-9A-Fa-f]").Count != 0) return false;
			byte[] data = new byte[hexString.Length / 2];
			for (int i = 0; i < data.Length; i++)
				data[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

			return bytes.Write(offset, data, littleEndian);
		}

		/// <summary>
		/// Writes the specified number <paramref name="value"/> into the current <see cref="byte"/> array starting at the given <paramref name="offset"/>.
		/// </summary>
		/// <param name="offset">The zero-based index in the current <see cref="byte"/> array at which to begin writing.</param>
		/// <param name="value">The number value. Must be a numeric (including <see cref="int"/>, <see cref="double"/>, etc.) type.</param>
		/// <param name="bigEndian">If <see langword="true"/>, reverses the <paramref name="data"/> array before writing to ensure big-endian order.</param>
		/// <returns><see langword="true"/> if the write operation was successful; otherwise, <see langword="false"/>.</returns>
		/// <exception cref="NotImplementedException">Throws if the provided value is not a number type.</exception>
		public bool Write<TNumber>(int offset, TNumber value, bool bigEndian = false) where TNumber : IComparable<TNumber> {
			NotImplementedException NaNException = new($"{typeof(TNumber).Name} is not a number type");
			if (!typeof(TNumber).IsNumber) throw NaNException;

			byte[] data = BitConverter.GetBytes((dynamic)value);
			return bytes.Write(offset, data, bigEndian);
		}

		/// <summary>
		/// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with uppercase hex characters.
		/// </summary>
		/// <returns>The string representation in hex of the elements in the array.</returns>
		public string ToHexString() => BitConverter.ToString(bytes).Replace("-", "");
	}

	extension(Enum) {
		/// <inheritdoc cref="Enum.GetName(Type, object)" />
		public static string GetName<TEnum>(TEnum value) where TEnum : struct, Enum =>
			Enum.GetName(typeof(TEnum), value);

		/// <inheritdoc cref="Enum.GetNames(Type)" />
		public static string[] GetNames<TEnum>() where TEnum : struct, Enum =>
			Enum.GetNames(typeof(TEnum));

		/// <inheritdoc cref="Enum.GetValues(Type)" />
		public static TEnum[] GetValues<TEnum>() where TEnum : struct, Enum =>
			(TEnum[])Enum.GetValues(typeof(TEnum));

		/// <inheritdoc cref="Enum.IsDefined(Type, object)" />
		public static bool IsDefined<TEnum>(string keyName) where TEnum : struct, Enum =>
			Enum.IsDefined(typeof(TEnum), keyName);

		/// <inheritdoc cref="Enum.IsDefined(Type, object)" />
		public static bool IsDefined<TEnum>(int intValue) where TEnum : struct, Enum =>
			Enum.IsDefined(typeof(TEnum), intValue);
	}
}
