using System.Windows.Forms;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// 将拖放文件事件参数转换为拖放的文件名称数组。如果拖放的不是文件，返回空数组。
	/// </summary>
	/// <param name="e">拖放事件参数。</param>
	/// <returns>拖放的文件名称数组。</returns>
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
	/// 判断一个类型是否是数字（包括 <see cref="int"/>、<see cref="double"/> 等）类型。
	/// </summary>
	/// <param name="type">类型。</param>
	/// <returns>该类型是数字类型。</returns>
	public static bool IsNumber(this Type type) =>
		NumericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
}
