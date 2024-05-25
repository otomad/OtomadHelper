using System.Resources;
using System.Windows.Baml2006;
using System.Xaml;

namespace OtomadHelper.Helpers;

/// <summary>
/// Read xaml assembly resource in runtime.
/// </summary>
public sealed class BamlAssemblyResource : IDisposable {
	private readonly Stream manifestResourceStream;
	private readonly ResourceReader resourceReader;

	/// <inheritdoc cref="BamlAssemblyResource"/>
	/// <remarks>
	/// <example>
	/// <code>
	/// using BamlAssemblyResource baml = new();
	/// </code>
	/// </example>
	/// </remarks>
	public BamlAssemblyResource() {
		Assembly assembly = Assembly.GetExecutingAssembly();
		string resName = ResourceHelper.AssemblyName + ".g.resources";
		manifestResourceStream = assembly.GetManifestResourceStream(resName);
		resourceReader = new(manifestResourceStream);
	}

	/// <summary>
	/// Get all project assembly resource entries.
	/// </summary>
	public IEnumerable<DictionaryEntry> Entries => resourceReader.Cast<DictionaryEntry>();

	/// <summary>
	/// Get all project assembly resource names.
	/// </summary>
	public string[] ResourceNames => Entries.Select(entry => (string)entry.Key).ToArray();

	/// <summary>
	/// Get the xaml object from specified path.
	/// </summary>
	/// <remarks>
	/// <example>
	/// If the embedded resource path is <c>OtomadHelper/WPF/Styles/Icons.xaml</c>,<br />
	/// pass <c>WPF/Styles/Icons</c> to <paramref name="resourcePath"/>. Case insensitive.
	/// </example>
	/// </remarks>
	/// <param name="path">
	/// The path to the assembly resource xaml/baml file relative to the project root directory,
	/// using "/" (slash) to separate directories.
	/// </param>
	/// <returns>The actual xaml object of the file.</returns>
	/// <exception cref="FileNotFoundException">Throw if the resource path is not exist.</exception>
	public object GetXaml(string path) {
		path = path.ToLowerInvariant() + ".baml";
		DictionaryEntry? entry = Entries.FirstOrDefault(entry => (string)entry.Key == path);
		if (entry is null) throw new FileNotFoundException("Cannot find the assembly xaml/baml resource: " + path);
		using Baml2006Reader reader = new((Stream)entry.Value.Value);
		using XamlObjectWriter writer = new(reader.SchemaContext);
		while (reader.Read())
			writer.WriteNode(reader);
		return writer.Result;
	}

	public void Dispose() {
		manifestResourceStream?.Dispose();
		resourceReader?.Dispose();
	}
}
