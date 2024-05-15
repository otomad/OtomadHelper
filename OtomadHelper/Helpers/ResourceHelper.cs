using System.IO;
using System.Resources;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Baml2006;
using System.Xaml;

using Microsoft.WindowsAPICodePack.Shell;

using OtomadHelper.Module;

namespace OtomadHelper.Helpers;

internal static class ResourceHelper {
	/// <summary>
	/// Get the embedded resource.
	/// </summary>
	/// <param name="resourcePath">
	/// The path to the embedded resource file relative to the project root directory,
	/// using "." (dot) to separate directories.
	/// </param>
	/// <returns>The byte sequence of the resource.</returns>
	/// <exception cref="FileNotFoundException">Throw if the resource path is not exist.</exception>
	public static Stream GetEmbeddedResource(string resourcePath) {
		Assembly assembly = Assembly.GetExecutingAssembly();
		string assetsFilePath = assembly.GetName().Name + "." + resourcePath; // You can set a breakpoint to see the value here.
		if (!IsResourceExist(assetsFilePath)) // Check for gzip file
			assetsFilePath += ".gz";
		if (!IsResourceExist(assetsFilePath))
			throw new FileNotFoundException("Cannot find embedded resource: " + assetsFilePath);
		return assembly.GetManifestResourceStream(assetsFilePath);

		bool IsResourceExist(string filePath) => assembly.GetManifestResourceInfo(filePath) != null;
	}

	/// <summary>
	/// Get the thumbnail of a local file.
	/// </summary>
	/// <param name="filePath">Local file path.</param>
	/// <param name="allowIcon">Allow file icon to be returned when there is no thumbnail?</param>
	/// <exception cref="InvalidOperationException">
	/// If the specified file has no thumbnail but only an icon, an error will be raised when <paramref name="allowIcon"/> is false.
	/// </exception>
	/// <returns>A large thumbnail image of the file.</returns>
	public static BitmapSource GetFileThumbnail(string filePath, bool allowIcon = false) {
		ShellFile shellFile = ShellFile.FromFilePath(filePath);
		shellFile.Thumbnail.AllowBiggerSize = true;
		shellFile.Thumbnail.FormatOption = allowIcon ? ShellThumbnailFormatOption.Default : ShellThumbnailFormatOption.ThumbnailOnly;
		BitmapSource thumb = shellFile.Thumbnail.ExtraLargeBitmapSource;
		return thumb;
	}

	/// <summary>
	/// Write the embedded resource file to the specified path on the local disk.
	/// </summary>
	/// <param name="resourcePath">The path to the embedded resource file.</param>
	/// <param name="filePath">Local file path.</param>
	public static void WriteResourceToFile(string resourcePath, string filePath) {
		using Stream resource = GetEmbeddedResource(resourcePath);
		using FileStream file = new(filePath, FileMode.Create, FileAccess.Write);
		resource.CopyTo(file);
	}
}
