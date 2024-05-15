using System.Resources;
using System.Windows.Baml2006;
using System.Xaml;

namespace OtomadHelper.Helpers;

public sealed class BamlAssemblyResource : IDisposable {
	private readonly Stream manifestResourceStream;
	private readonly ResourceReader resourceReader;

	public BamlAssemblyResource() {
		Assembly assembly = Assembly.GetExecutingAssembly();
		string resName = assembly.GetName().Name + ".g.resources";
		manifestResourceStream = assembly.GetManifestResourceStream(resName);
		resourceReader = new(manifestResourceStream);
	}

	public IEnumerable<DictionaryEntry> Entries => resourceReader.Cast<DictionaryEntry>();

	public string[] ResourceNames => Entries.Select(entry => (string)entry.Key).ToArray();

	public object GetXaml(string path) {
		path = path.ToLowerInvariant() + ".baml";
		DictionaryEntry entry = Entries.FirstOrDefault(entry => (string)entry.Key == path);
		using Baml2006Reader reader = new((Stream)entry.Value);
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
