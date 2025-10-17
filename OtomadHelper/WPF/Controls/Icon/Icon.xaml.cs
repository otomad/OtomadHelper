using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Icon.xaml 的交互逻辑
/// </summary>
[DependencyProperty<double>("Size", DefaultValue = 16d)]
[DependencyProperty<Brush>("Foreground", DefaultValueExpression = "defaultForeground")]
[DependencyProperty<IconTemplate>("Source")]
[DependencyProperty<string>("IconName")]
public partial class Icon : Viewbox {
	public Icon() {
		InitializeComponent();
	}

	private void Icon_Loaded(object sender, RoutedEventArgs e) {
		_ = Source;
		if (Foreground == defaultForeground)
			SetResourceReference(ForegroundProperty, "ForegroundBrush");
	}

	private static readonly SolidColorBrush defaultForeground = Brushes.Black;

	partial void OnIconNameChanged(string? iconName) {
		this.SetResourceReference(SourceProperty, "Icon:" + iconName);
	}

	private static string[]? validIconNames;
	public static string[] ValidIconNames {
		get {
			if (validIconNames is null) {
				using BamlAssemblyResource baml = new();
				ResourceDictionary xaml = (ResourceDictionary)baml.GetXaml("WPF/Themes/Icons");
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

	/// <summary>
	/// Play system sound with presupposed icon names. Other icon names will not play any sound.
	/// </summary>
	/// <param name="iconName">All known icon names, including: Info, Warning, Error, Question.</param>
	public static void PlaySound(string iconName) {
		SystemSound? sound = iconName switch {
			"Info" => SystemSounds.Beep,
			"Warning" => SystemSounds.Asterisk,
			"Error" => SystemSounds.Hand,
			"Question" => SystemSounds.Exclamation,
			_ => null,
		};
		sound?.Play();
	}
}
