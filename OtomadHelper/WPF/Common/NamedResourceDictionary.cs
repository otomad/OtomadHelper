using System.Windows;

namespace OtomadHelper.WPF.Common;

public class NamedResourceDictionary : ResourceDictionary {
	private string name = "";

	/// <summary>
	/// Add your custom name to the resource dictionary.
	/// </summary>
	public string Name {
		get => !string.IsNullOrEmpty(name) ? name :
			RootElement is NamedResourceDictionary resource ? resource.Name : name;
		set => name = value;
	}

	public ResourceDictionary? RootElement =>
		typeof(ResourceDictionary).GetField("_rootElement", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(this) as ResourceDictionary;
}
