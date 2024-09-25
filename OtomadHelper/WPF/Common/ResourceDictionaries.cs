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

	public ResourceDictionary? RootElement => this.GetRootElement();
}

public class CustomControlResourceDictionary : ResourceDictionary {
	public CustomControlResourceDictionary() {
		AddResource("Wpf/Styles/Generic.xaml");
	}

	// The auto added resource doesn't includes Control.xaml.
	// If your CustomControlResourceDictionary requires other CustomControl style (like DefaultXxxStyle),
	// you have to add them manually, and make sure that if control A includes control B, the control B cannot
	// include control A, or will cause a stack overflow exception.
	public void AddResource(string path) {
		// We need to lock the thread and use a static field to judge before and after adding the
		// resource dictionary code to avoid recursing to the control being created when adding the
		// "Control.xaml" dictionary, causing a stack overflow exception.
		MergedDictionaries.Add(new() { Source = ProjectUri(path) });
	}
}

/// <summary>
/// The shared resource dictionary is a specialized resource dictionary that loads it content only once.
/// If a second instance with the same source is created, it only merges the resources from the cache.
/// </summary>
public class SharedResourceDictionary : ResourceDictionary {
	/// <summary>
	/// A value indicating whether the application is in designer mode.
	/// </summary>
	private static readonly bool isInDesignerMode;

	/// <summary>
	/// Local member of the source uri
	/// </summary>
	private Uri? sourceUri = null;

	/// <summary>
	/// Initializes static members of the <see cref="SharedResourceDictionary"/> class.
	/// </summary>
	static SharedResourceDictionary() {
		isInDesignerMode = (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
	}

	/// <summary>
	/// Gets the internal cache of loaded dictionaries.
	/// </summary>
	public static Dictionary<Uri, ResourceDictionary> SharedDictionaries { get; } = [];

	/// <inheritdoc cref="ResourceDictionary.Source"/>
	public new Uri? Source {
		get => sourceUri;
		set {
			sourceUri = value;
			if (value == null) return;
			// Always load the dictionary by default in designer mode.
			if (!SharedDictionaries.ContainsKey(value) || isInDesignerMode) {
				// Add it to the cache if we're not in designer mode.
				if (!isInDesignerMode) SharedDictionaries.Add(value, this);
				// If the dictionary is not yet loaded, load it by setting the source of the base class.
				base.Source = value;
			} else {
				// If the dictionary is already loaded, get it from the cache.
				MergedDictionaries.Add(SharedDictionaries[value]);
			}
		}
	}

	/// <summary>
	/// We don't required to write the whole long URI again.
	/// </summary>
	public string ControlSource {
		set => Source = new Path(ProjectUri("Wpf/Controls/")) { IsWindows = false } + (value + ".xaml");
	}
}
