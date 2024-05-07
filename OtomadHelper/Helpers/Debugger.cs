using System.Windows;

using OtomadHelper.Module;

namespace OtomadHelper.Helpers;

internal static class Debugger {
	/// <summary>
	/// Show me the fucking content!
	/// </summary>
	/// <remarks>
	/// 给老娘展示内容！
	/// </remarks>
	[Obsolete("This method can only be used for debugging and should not be used in release!")]
#pragma warning disable IDE1006 // 命名样式
	public static object? s {
#pragma warning restore IDE1006 // 命名样式
		set => Debug.WriteLine(value is null ? "null" : value);
	}

	[Obsolete("This method can only be used for debugging and should not be used in release!")]
	public static void DebugPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}

	/// <summary>
	/// Get <see cref="Uri"/> from a <paramref name="path"/> to this project.
	/// </summary>
	/// <param name="path">A <paramref name="path"/> to this project.</param>
	/// <returns>A valid <see cref="Uri"/> which won't let Vegas to crash.</returns>
	public static Uri ProjectUri(string path) =>
		new($"pack://application:,,,/{OtomadHelperModule.ASSEMBLY_NAME};component/{path}", UriKind.Absolute);
}
