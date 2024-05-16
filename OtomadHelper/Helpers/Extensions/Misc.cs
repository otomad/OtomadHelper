using System.Windows.Forms;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// Convert the drag-and-drop file event arguments to an array containing the file names of the drag-and-drop files.
	/// If the drag-and-drop file is not a file, an empty array is returned.
	/// </summary>
	/// <param name="e">Drag-and-drop event arguments.</param>
	/// <returns>Array of the file names for drag-and-drop.</returns>
	public static string[] GetFileNames(this DragEventArgs e) {
		string[] NOTHING = new string[0];
		return e.Data.GetDataPresent(DataFormats.FileDrop) ? e.Data.GetData(DataFormats.FileDrop) as string[] ?? NOTHING : NOTHING;
	}

	private static readonly HashSet<Type> NumericTypes = new() {
		typeof(int), typeof(double), typeof(decimal),
		typeof(long), typeof(short), typeof(sbyte),
		typeof(byte), typeof(ulong), typeof(ushort),
		typeof(uint), typeof(float),
	};

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
		MethodInfo? tryParseMethod = typeof(TNumber).GetMethod("TryParse",
			new[] { typeof(string), typeof(TNumber).MakeByRefType() });
		if (tryParseMethod is null) throw NaNException;
		object?[] parameters = new object?[] { s, null };
		if ((bool)tryParseMethod.Invoke(null, parameters)) {
			result = (TNumber)parameters[1]!;
			return true;
		} else
			return false; // Leave `result` as it was.
	}
}
