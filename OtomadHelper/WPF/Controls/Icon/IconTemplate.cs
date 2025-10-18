using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace OtomadHelper.WPF.Controls;

[ContentProperty("Path")]
public class IconTemplate {
	[TypeConverter(typeof(IconPathConverter))]
	public List<Geometry> Path { get; set; } = [];

	public double Size { get; set; } = 16;

	public Brush Fill { get; set; } = Icon.defaultForeground;

	/// <remarks>
	/// This is a simple implement, only available for known used SVG.
	/// For more complex SVG, you may need to use a more powerful library.
	/// </remarks>
	public static IconTemplate FromSvg(string svg) {
		IconTemplate icon = new();
		XmlDocument doc = new();
		doc.LoadXml(svg);
		XmlElement root = doc.DocumentElement;
		double size = -1;
		if (!double.TryParse(root.GetAttributeCaseInsensitive("width"), out size))
			double.TryParse(root.GetAttributeCaseInsensitive("height"), out size);
		if (size != -1) icon.Size = size;
		foreach (XmlNode node in root.ChildNodes)
			if (node is XmlElement child && child.Name.ToLowerInvariant() == "path" && child.HasAttributeCaseInsensitive("d"))
				icon.Path.Add(Geometry.Parse(child.GetAttributeCaseInsensitive("d")!));
		return icon;
	}

	public Icon ToIcon() => new() { Source = this };
}
