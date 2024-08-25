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

public class CustomControlResourceDictionary : ResourceDictionary {
	public CustomControlResourceDictionary() {
		AddResource("Wpf/Styles/Generic.xaml");
	}

	private static readonly List<string> dictionaryBeingAdded = new();
	public void AddResource(string path) {
		lock (dictionaryBeingAdded) {
			// We need to lock the thread and use a static field to judge before and after adding the
			// resource dictionary code to avoid recursing to the control being created when adding the
			// "Control.xaml" dictionary, causing a stack overflow exception.
			if (dictionaryBeingAdded.Contains(path)) return;
			dictionaryBeingAdded.Add(path);
			MergedDictionaries.Add(new() { Source = ProjectUri(path) });
			dictionaryBeingAdded.Remove(path);
		}
	}
}
