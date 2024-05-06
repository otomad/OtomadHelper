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
	public static void Swap<T>(ref T left, ref T right) {
		T temp = left;
		left = right;
		right = temp;
	}
}
