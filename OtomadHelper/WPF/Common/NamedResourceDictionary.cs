using System.Windows;

namespace OtomadHelper.WPF.Common;

public class NamedResourceDictionary : ResourceDictionary {
	/// <summary>
	/// Add your custom name to the resource dictionary.
	/// </summary>
	public string Name { get; set; } = "";
}
