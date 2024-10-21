namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// Get <see cref="Uri"/> from a <paramref name="path"/> to this project.
	/// </summary>
	/// <param name="path">A <paramref name="path"/> to this project.</param>
	/// <returns>A valid <see cref="Uri"/> which won't let Vegas to crash.</returns>
	public static Uri ProjectUri(string path) =>
		new($"pack://application:,,,/{ResourceHelper.AssemblyName};component/{path}", UriKind.Absolute);
}
