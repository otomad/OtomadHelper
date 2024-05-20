using System.Collections.ObjectModel;
using System.Windows;

using OtomadHelper.Module;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <summary>
	/// Get <see cref="Uri"/> from a <paramref name="path"/> to this project.
	/// </summary>
	/// <param name="path">A <paramref name="path"/> to this project.</param>
	/// <returns>A valid <see cref="Uri"/> which won't let Vegas to crash.</returns>
	public static Uri ProjectUri(string path) =>
		new($"pack://application:,,,/{OtomadHelperModule.ASSEMBLY_NAME};component/{path}", UriKind.Absolute);

	/*/// <summary>
	/// Get the <see cref="ResourceDictionary"/> name if declared, or <see langword="null"/>.
	/// </summary>
	/// <returns>The name of the <see cref="ResourceDictionary"/> if declared, or <see langword="null"/>.</returns>
	public static string? GetName(this ResourceDictionary resource) => resource["ResourceDictionaryName"] as string;

	/// <summary>
	/// Find the first <see cref="ResourceDictionary"/> from <see cref="ResourceDictionary.MergedDictionaries"/> by name.
	/// </summary>
	/// <param name="merged"><see cref="ResourceDictionary.MergedDictionaries"/>.</param>
	/// <param name="name">The name of the <see cref="ResourceDictionary"/>.</param>
	/// <returns>The first matched <see cref="ResourceDictionary"/>.</returns>
	public static ResourceDictionary? FindByName(this Collection<ResourceDictionary> merged, string name) =>
		merged.FirstOrDefault(dict => dict.GetName() == name);

	/// <summary>
	/// Find all matched <see cref="ResourceDictionary"/> from <see cref="ResourceDictionary.MergedDictionaries"/> by name.
	/// </summary>
	/// <param name="merged"><see cref="ResourceDictionary.MergedDictionaries"/>.</param>
	/// <param name="name">The name of the <see cref="ResourceDictionary"/>.</param>
	/// <returns>All matched <see cref="ResourceDictionary"/> <see cref="IEnumerable{ResourceDictionary}"/>.</returns>
	public static IEnumerable<ResourceDictionary> FindAllByName(this Collection<ResourceDictionary> merged, string name) =>
		merged.Where(dict => dict.GetName() == name);*/
}
