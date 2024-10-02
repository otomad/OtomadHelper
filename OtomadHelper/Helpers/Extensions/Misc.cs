using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Data;

using DataFormats = System.Windows.Forms.DataFormats;
using DragEventArgs = System.Windows.Forms.DragEventArgs;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// Convert the drag-and-drop file event arguments to an array containing the file names of the drag-and-drop files.
	/// If the drag-and-drop file is not a file, an empty array is returned.
	/// </summary>
	/// <param name="e">Drag-and-drop event arguments.</param>
	/// <returns>Array of the file names for drag-and-drop.</returns>
	public static string[] GetFileNames(this DragEventArgs e) {
		string[] NOTHING = [];
		return e.Data.GetDataPresent(DataFormats.FileDrop) ? e.Data.GetData(DataFormats.FileDrop) as string[] ?? NOTHING : NOTHING;
	}

	private static readonly HashSet<Type> NumericTypes = [
		typeof(int), typeof(double), typeof(decimal),
		typeof(long), typeof(short), typeof(sbyte),
		typeof(byte), typeof(ulong), typeof(ushort),
		typeof(uint), typeof(float),
	];

	/// <summary>
	/// Determine whether a type is a numeric (including <see cref="int"/>, <see cref="double"/>, etc.) type.
	/// </summary>
	/// <param name="type">Type.</param>
	/// <returns>The type is a numeric type.</returns>
	public static bool IsNumber(this Type type) =>
		NumericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);

	/// <summary>
	/// Swaps the values of two variables.
	/// </summary>
	/// <typeparam name="T">The type of the variables to be swapped.</typeparam>
	/// <param name="left">The first variable.</param>
	/// <param name="right">The second variable.</param>
	[Obsolete("Using tuple to swap values.")]
	public static void Swap<T>(ref T left, ref T right) {
		T temp = left;
		left = right;
		right = temp;
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
		if (!typeof(TNumber).IsNumber()) throw NaNException;
		MethodInfo tryParseMethod = typeof(TNumber).GetMethod("TryParse",
			[typeof(string), typeof(TNumber).MakeByRefType()]) ?? throw NaNException;
		object?[] parameters = [s, null];
		if ((bool)tryParseMethod.Invoke(null, parameters)) {
			result = (TNumber)parameters[1]!;
			return true;
		} else
			return false; // Leave `result` as it was.
	}

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
	public static async Task<TTarget> Then<TSource, TTarget>(this Task<TSource> task, Func<TSource, TTarget> then) =>
		then(await task);
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async void Then<TSource>(this Task<TSource> task, Action<TSource> then) =>
		then(await task);
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async Task<TTarget> Then<TTarget>(this Task task, Func<TTarget> then) {
		await task; return then(); }
	/// <inheritdoc cref="Then{TSource, TTarget}(Task{TSource}, Func{TSource, TTarget})"/>
	public static async void Then(this Task task, Action then) {
		await task; then(); }

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
	public static bool Extends(this Type type, Type baseType) =>
		baseType.IsAssignableFrom(type);

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
	public static bool IsNullable(this Type type) =>
		!type.IsValueType || Nullable.GetUnderlyingType(type) != null;

	/// <summary>
	/// If this <see cref="ResourceDictionary"/> is inside a <see cref="ResourceDictionary.MergedDictionaries"/> and it has
	/// <see cref="ResourceDictionary.Source"/> property.
	/// Get the <c>rootElement</c> which is private by reflection.
	/// </summary>
	/// <param name="parent"></param>
	/// <returns>The root element of a <see cref="ResourceDictionary"/> if it has.</returns>
	public static ResourceDictionary? GetRootElement(this ResourceDictionary parent) =>
		typeof(ResourceDictionary).GetField("_rootElement", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(parent) as ResourceDictionary;

	/// <inheritdoc cref="Binding.Mode"/>
	/// <exception cref="ArgumentException">The <see cref="BindingBase"/> type is not implemented and supported.</exception>
	public static void SetMode(this BindingBase? bindingBase, BindingMode mode) {
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

	/// <summary>
	/// We've known that <see cref="MultiBinding.Bindings"/> supports <see cref="Binding"/> only,
	/// not supports <see cref="MultiBinding"/> and <see cref="PriorityBinding"/>. So we support them.
	/// </summary>
	/// <param name="multiBinding">The bindings where to be added.</param>
	/// <param name="bindingBase">The binding bases which will be added.</param>
	/// <returns>How many bindings were added.</returns>
	/// <exception cref="ArgumentException">The <see cref="BindingBase"/> type is not supported.</exception>
	public static int AddBinding(this MultiBinding multiBinding, BindingBase? bindingBase) {
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

	/// <summary>
	/// Get the property from a object with the path string.
	/// </summary>
	/// <remarks>
	/// <example>
	/// <code>
	/// foo.GetPath("bar.baz");
	/// </code>
	/// Equals to
	/// <code>
	/// foo.bar.baz;
	/// </code>
	/// </example>
	/// </remarks>
	public static object GetPath(this object obj, string path) {
		string[] array = path.Split('.');
		foreach (string name in array)
			obj = obj.GetType().GetProperty(name).GetValue(obj, null);
		return obj;
	}
}
