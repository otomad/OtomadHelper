using System.Windows.Forms;
namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// 将拖放文件事件参数转换为拖放的文件名称数组。如果拖放的不是文件，返回空数组。
	/// </summary>
	/// <param name="e">拖放事件参数。</param>
	/// <returns>拖放的文件名称数组。</returns>
	public static string[] GetFileNames(this DragEventArgs e) {
		return e.Data.GetDataPresent(DataFormats.FileDrop) ? e.Data.GetData(DataFormats.FileDrop) as string[] ?? new string[0] : new string[0];
	}
}
