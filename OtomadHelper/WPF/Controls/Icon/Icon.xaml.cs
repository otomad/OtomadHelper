using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Icon.xaml 的交互逻辑
/// </summary>
[DependencyProperty<double>("Size", DefaultValue = 16.0)]
[DependencyProperty<Brush>("Foreground", DefaultValueExpression = "defaultForeground")]
[DependencyProperty<IconTemplate>("Source")]
[DependencyProperty<string>("IconName")]
public partial class Icon : Viewbox {
	public Icon() {
		InitializeComponent();
	}

	private void Icon_Loaded(object sender, RoutedEventArgs e) {
		if (Foreground == defaultForeground)
			SetResourceReference(ForegroundProperty, "ForegroundBrush");
	}

	private static readonly SolidColorBrush defaultForeground = new(Colors.Black);

	partial void OnIconNameChanged(string? prevIconName, string? iconName) {
		this.SetResourceReference(SourceProperty, "Icon:" + iconName);
	}

	private static string[]? validIconNames;
	public static string[] ValidIconNames {
		get {
			if (validIconNames is null) {
				using BamlAssemblyResource baml = new();
				ResourceDictionary xaml = (ResourceDictionary)baml.GetXaml("WPF/Styles/Icons");
				validIconNames = xaml.Keys.Cast<string>()
					.Where(key => key.StartsWith("Icon:", StringComparison.InvariantCultureIgnoreCase))
					.Select(key => key.Replace(new Regex(@"^Icon:", RegexOptions.IgnoreCase), ""))
					.ToArray();
			}
			return validIconNames;
		}
	}
	public static bool IsValidIconName(string iconName) =>
		ValidIconNames.Contains(iconName, StringComparer.InvariantCultureIgnoreCase);

	public static string NormalizeIconName(string iconName) => new VariableName(iconName).Pascal;
	public static string NormalizeIconName(object iconName) => NormalizeIconName(iconName.ToString());
}
