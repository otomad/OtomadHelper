using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml;

using DataFormats = System.Windows.Forms.DataFormats;
using DragEventArgs = System.Windows.Forms.DragEventArgs;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	extension(DragEventArgs e) {
		/// <summary>
		/// Convert the drag-and-drop file event arguments to an array containing the file names of the drag-and-drop files.
		/// If the drag-and-drop file is not a file, an empty array is returned.
		/// </summary>
		/// <param name="e">Drag-and-drop event arguments.</param>
		/// <returns>Array of the file names for drag-and-drop.</returns>
		public string[] FileNames {
			get {
				string[] NOTHING = [];
				return e.Data.GetDataPresent(DataFormats.FileDrop) ? e.Data.GetData(DataFormats.FileDrop) as string[] ?? NOTHING : NOTHING;
			}
		}
	}

	private static readonly HashSet<Type> NumericTypes = [
		typeof(int), typeof(double), typeof(decimal),
		typeof(long), typeof(short), typeof(sbyte),
		typeof(byte), typeof(ulong), typeof(ushort),
		typeof(uint), typeof(float),
	];
	private static T GetDefaultGeneric<T>() => default!;

	extension(Type type) {
		/// <summary>
		/// Determine whether a type is a numeric (including <see cref="int"/>, <see cref="double"/>, etc.) type.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <returns>The type is a numeric type.</returns>
		public bool IsNumber => NumericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);

		/// <summary>
		/// Determines whether the current type extends (inherits from) the specified base type.
		/// </summary>
		/// <param name="type">The current type to check.</param>
		/// <param name="baseType">The base type to check if the current type extends.</param>
		/// <returns>
		/// <see langword="true"/> if the current type extends the specified base type; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// This method uses the <see cref="Type.IsAssignableFrom(Type)"/> method to check if the current type is
		/// assignable from the specified base type.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// If either <paramref name="type"/> or <paramref name="baseType"/> is <see langword="null"/>.
		/// </exception>
		public bool Extends(Type baseType) {
			// BaseType<>, Interface<>
			if (baseType.IsGenericTypeDefinition)
				return baseType.IsInterface ?
					// Type<T> implement Interface<>
					type.GetInterfaces().Any(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == baseType) :
					// Type<T> extends BaseType<>
					baseType.IsAssignableFrom(type.GetGenericTypeDefinition());

			// Type extends BaseType implement Interface
			// Type<T> extends BaseType<T> implement Interface<T>
			return baseType.IsAssignableFrom(type);
		}

		/// <summary>
		/// Determines whether the given type can be assigned to <see langword="null"/>.
		/// </summary>
		/// <param name="type">The type to check.</param>
		/// <returns>
		/// <see langword="true"/> if the given type can be assigned to <see langword="null"/>;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// This method checks if the given <paramref name="type"/> is a <b>reference type</b>
		/// or a value type that has a <see cref="Nullable"/> type as its underlying type.<br />
		/// This indicates that the <paramref name="type"/> can be assigned to <see langword="null"/>.<br />
		/// If the <paramref name="type"/> is a normal value type, it returns <see langword="false"/>.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		/// If the given <paramref name="type"/> is <see langword="null"/>.
		/// </exception>
		public bool IsNullable =>
			!type.IsValueType || Nullable.GetUnderlyingType(type) != null;

		/// <summary>
		/// Check if the type is <see cref="IEnumerable{T}" /> type, and return the <see cref="IEnumerable{T}" /> type and its item type.
		/// </summary>
		/// <param name="enumerableType"><see cref="IEnumerable{T}" /> (or <see cref="IEnumerable" /> if don't know the <see langword="T" />) type.</param>
		/// <param name="itemType">Type of <see langword="T" /> (or <see langword="null" /> if don't know).</param>
		/// <returns>Is the type extends the <see cref="IEnumerable" /> type?</returns>
		public bool TryGetIEnumerableType(out Type enumerableType, out Type itemType) {
			enumerableType = null!;
			itemType = null!;
			if (!type.Extends(typeof(IEnumerable)))
				return false;
			enumerableType = type.GetInterface("IEnumerable`1");
			itemType = enumerableType?.GenericTypeArguments[0]!;
			enumerableType ??= type.GetInterface("IEnumerable");
			return true;
		}

		/// <summary>
		/// Get the default value of a <see cref="Type" /> <b>variable</b> like <c><see langword="default" />(Type)</c>.
		/// </summary>
		public object GetDefault() =>
			typeof(Extensions).GetMethod(nameof(GetDefaultGeneric), BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(type).Invoke(null, null);
	}

	/// <summary>
	/// Converts the string representation of a number to its <typeparamref name="TNumber"/> number equivalent.
	/// A return value indicates whether the operation succeeded.<br />
	/// The difference from <see cref="int.TryParse(string, out int)"/> and others is that if the conversion fails,
	/// the original value of <paramref name="result"/> will be reserved instead of being assigned to the default
	/// <typeparamref name="TNumber"/> value (that is, <see langword="0"/>).
	/// </summary>
	/// <typeparam name="TNumber">The number type that you want to convert from string <paramref name="s"/>.</typeparam>
	/// <param name="s">A string containing a number to convert.</param>
	/// <param name="result">
	/// When this method returns, contains the <typeparamref name="TNumber"/> number value equivalent of the number
	/// contained in <paramref name="s"/>, if the conversion succeeded, or reserves <paramref name="result"/> value
	/// originally if the conversion failed. The conversion fails if the <paramref name="s"/> parameter is
	/// <see langword="null"/> or <see cref="string.Empty"/>, is not of the correct format, or represents a number less
	/// than <see cref="int.MinValue"/> or greater than <see cref="int.MaxValue"/>.
	/// </param>
	/// <returns><see langword="true"/> if <paramref name="s"/> was converted successfully; otherwise,
	/// <see langword="false"/>.</returns>
	/// <exception cref="NotImplementedException">If type <typeparamref name="TNumber"/> is not a number type,
	/// it will raise an error.</exception>
	public static bool TryParseNumber<TNumber>(string s, ref TNumber result) where TNumber : IComparable<TNumber> {
		NotImplementedException NaNException = new($"{typeof(TNumber).Name} is not a number type");
		if (!typeof(TNumber).IsNumber) throw NaNException;
		MethodInfo tryParseMethod = typeof(TNumber).GetMethod("TryParse",
			[typeof(string), typeof(TNumber).MakeByRefType()]) ?? throw NaNException;
		object?[] parameters = [s, null];
		if ((bool)tryParseMethod.Invoke(null, parameters)) {
			result = (TNumber)parameters[1]!;
			return true;
		} else
			return false; // Leave `result` as it was.
	}

	#region Task Then
	// NOTE: These task extention methods cannot convert to C# 14 extension members.

	/// <summary>
	/// Asynchronously executes a function after the completion of the given task.
	/// </summary>
	/// <typeparam name="TSource">The type of the result of the task.</typeparam>
	/// <typeparam name="TTarget">The type of the result of the function.</typeparam>
	/// <param name="task">The task to await.</param>
	/// <param name="then">The function to execute after the task completes.</param>
	/// <returns>The result of the function.</returns>
	/// <remarks>
	/// This method is useful for chaining asynchronous operations.
	/// Or you want to get the result of an asynchronous method in a synchronous method.
	/// <example>
	/// <code>
	/// public void Foo() {
	///     AsyncFunc().Then(result => {
	///         Debug.WriteLine(result);
	///     });
	/// }
	/// // Equivalent to
	/// public async void Foo() {
	///     var result = AsyncFunc();
	///     Debug.WriteLine(result);
	/// }
	/// </code>
	/// </example>
	/// </remarks>
	public static async Task<TTarget> Then<TSource, TTarget>(this Task<TSource> task, Func<TSource, TTarget> then) => then(await task);
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async Task Then<TSource>(this Task<TSource> task, Action<TSource> then) => then(await task);
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async Task<TTarget> Then<TTarget>(this Task task, Func<TTarget> then) { await task; return then(); }
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async Task Then(this Task task, Action then) { await task; then(); }
	#endregion

	extension(ResourceDictionary parent) {
		/// <summary>
		/// If this <see cref="ResourceDictionary"/> is inside a <see cref="ResourceDictionary.MergedDictionaries"/> and it has
		/// <see cref="ResourceDictionary.Source"/> property.
		/// Get the <c>rootElement</c> which is private by reflection.
		/// </summary>
		/// <param name="parent"></param>
		/// <returns>The root element of a <see cref="ResourceDictionary"/> if it has.</returns>
		public ResourceDictionary? RootElement =>
			typeof(ResourceDictionary).GetField("_rootElement", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(parent) as ResourceDictionary;
	}

	extension(BindingBase? bindingBase) {
		/// <inheritdoc cref="Binding.Mode"/>
		/// <exception cref="ArgumentException">The <see cref="BindingBase"/> type is not implemented and supported.</exception>
		public void SetMode(BindingMode mode) {
			switch (bindingBase) {
				case null:
					break;
				case Binding binding:
					binding.Mode = mode;
					break;
				case MultiBinding binding:
					binding.Mode = mode;
					binding.Bindings.ForEach(bind => bind.SetMode(mode));
					break;
				case PriorityBinding binding:
					binding.Bindings.ForEach(bind => bind.SetMode(mode));
					break;
				default:
					throw new NotSupportedException($"The binding type `{bindingBase.GetType().Name}` is not implemented and supported yet");
			}
		}
	}

	extension(MultiBinding multiBinding) {
		/// <summary>
		/// We've known that <see cref="MultiBinding.Bindings"/> supports <see cref="Binding"/> only,
		/// not supports <see cref="MultiBinding"/> and <see cref="PriorityBinding"/>. So we support them.
		/// </summary>
		/// <param name="multiBinding">The bindings where to be added.</param>
		/// <param name="bindingBase">The binding bases which will be added.</param>
		/// <returns>How many bindings were added.</returns>
		/// <exception cref="ArgumentException">The <see cref="BindingBase"/> type is not supported.</exception>
		public int AddBinding(BindingBase? bindingBase) {
			int added = 0;
			switch (bindingBase) {
				case null:
					break;
				case Binding binding:
					multiBinding.Bindings.Add(binding);
					added += 1;
					break;
				case MultiBinding binding:
					foreach (BindingBase bind in binding.Bindings)
						added += multiBinding.AddBinding(bind);
					break;
				case PriorityBinding binding:
					foreach (BindingBase bind in binding.Bindings)
						added += multiBinding.AddBinding(bind);
					break;
				default:
					throw new NotSupportedException($"The binding type `{bindingBase.GetType().Name}` is not implemented and supported yet");
			}
			return added;
		}
	}

	extension(object) {
		/// <summary>
		/// Get the property from a object with the path string.
		/// </summary>
		/// <remarks>
		/// <example>
		/// <code>
		/// object.GetPath(foo, "bar.baz");
		/// </code>
		/// Equals to
		/// <code>
		/// foo.bar.baz;
		/// </code>
		/// </example>
		/// </remarks>
		public static object GetPath(object obj, string path) {
			string[] array = path.Split('.');
			foreach (string name in array)
				obj = obj.GetType().GetProperty(name)!.GetValue(obj, null);
			return obj;
		}
	}

	extension(Thread thread) {
		/// <summary>
		/// Join and start the <see cref="Thread" />.
		/// </summary>
		public void JoinStart() {
			thread.Start();
			thread.Join();
		}
	}

	extension(double value) {
		/// <summary>
		/// Check the value if it is not <see cref="double.NaN" />, <see cref="double.PositiveInfinity" />, or <see cref="double.NegativeInfinity" />.
		/// </summary>
		public bool IsFinite => !(double.IsNaN(value) || double.IsInfinity(value));

		/// <summary>
		/// If the <paramref name="value" /> is not <see cref="double.NaN" />, <see cref="double.PositiveInfinity" />, or <see cref="double.NegativeInfinity" />,
		/// return it, or return the default value.
		/// </summary>
		/// <param name="def">Default value.</param>
		public double FiniteOrDefault(double def = default) => value.IsFinite ? value : def;
	}

	/// <inheritdoc cref="XmlElement.GetAttribute(string)" />
	extension(XmlElement element) {
		/// <inheritdoc cref="XmlElement.GetAttribute(string)" />
		public string? GetAttributeCaseInsensitive(string name) {
			name = name.ToLowerInvariant();
			foreach (XmlAttribute attribute in element.Attributes)
				if (attribute.Name.ToLowerInvariant() == name)
					return attribute.Value;
			return null;
		}

		/// <inheritdoc cref="XmlElement.GetAttribute(string, string)" />
		public string? GetAttributeCaseInsensitive(string localName, string namespaceURI) {
			localName = localName.ToLowerInvariant();
			namespaceURI = namespaceURI.ToLowerInvariant();
			foreach (XmlAttribute attribute in element.Attributes)
				if (attribute.LocalName.ToLowerInvariant() == localName && attribute.NamespaceURI.ToLowerInvariant() == namespaceURI)
					return attribute.Value;
			return null;
		}

		/// <inheritdoc cref="XmlElement.HasAttribute(string)" />
		public bool HasAttributeCaseInsensitive(string name) {
			name = name.ToLowerInvariant();
			foreach (XmlAttribute attribute in element.Attributes)
				if (attribute.Name.ToLowerInvariant() == name)
					return true;
			return false;
		}

		/// <inheritdoc cref="XmlElement.HasAttribute(string, string)" />
		public bool HasAttributeCaseInsensitive(string localName, string namespaceURI) {
			localName = localName.ToLowerInvariant();
			namespaceURI = namespaceURI.ToLowerInvariant();
			foreach (XmlAttribute attribute in element.Attributes)
				if (attribute.LocalName.ToLowerInvariant() == localName && attribute.NamespaceURI.ToLowerInvariant() == namespaceURI)
					return true;
			return false;
		}
	}

	extension(Stream stream) {
		/// <summary>
		/// Read the text from a <see cref="Stream" />.
		/// </summary>
		/// <param name="stream">The stream to read the text.</param>
		/// <param name="encoding">The encoding to read the text. Defaults to UTF-8.</param>
		/// <returns>String text.</returns>
		public string ReadText(Encoding? encoding = null) {
			encoding ??= Encoding.UTF8;
			using StreamReader reader = new(stream, encoding);
			return reader.ReadToEnd();
		}
	}

	extension(FontFamily) {
		/// <summary>
		/// Creates a new FontFamily by joining the sources of the specified font families into a single, comma-separated
		/// source string.
		/// </summary>
		/// <param name="fontFamilies">A parameter array of collections of FontFamily objects whose sources will be combined. Each collection can contain
		/// one or more FontFamily instances.</param>
		/// <returns>A FontFamily representing the combined sources of all specified font families, joined as a comma-separated list.</returns>
		public static FontFamily Join(params IEnumerable<FontFamily?> fontFamilies) =>
			new(fontFamilies.NonNull().Select(font => font.Source).Join(", "));
	}

	extension(CultureInfo culture) {
		/// <summary>
		/// Get the default UI font of Windows in the specific culture info, but not fully covered.
		/// </summary>
		/// <returns>The default UI font in the specific culture info.</returns>
		public FontFamily? GetDefaultUIFont() {
			while (!(culture is null || culture == CultureInfo.InvariantCulture)) {
				string? font = culture.Name switch {
					"zh-CN" or "zh-Hans" or "zh-CHS" or "zh" => "Microsoft YaHei UI",
					"zh-TW" or "zh-HK" or "zh-MO" or "zh-Hant" or "zh-CHT" => "Microsoft JhengHei UI",
					"ja-JP" or "ja" => "Yu Gothic UI",
					"ko-KR" or "ko-KP" or "ko" => "Malgun Gothic",
					"en-US" or "en-GB" or "en-UK" => "Segoe UI",
					_ => null,
				};
				if (font is not null) return new(font);
				culture = culture.Parent;
			}
			return null;
		}
	}
}
