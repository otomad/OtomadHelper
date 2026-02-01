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
[DependencyProperty<DeclaredIcon>("IconName")]
public partial class Icon : Viewbox {
	public Icon() {
		InitializeComponent();
	}

	internal static readonly SolidColorBrush defaultForeground = Brushes.Transparent;

	partial void OnIconNameChanged(DeclaredIcon icon) {
		if (Enum.GetName(icon) is not string iconName) return;
		const string iconNamePrefix = "Icon:";
		if (iconName.StartsWith(iconNamePrefix))
			throw new ArgumentException($"""
				Do not manually add "Icon:" prefix before the icon name, it will automatically add it!
				Unexpected icon name: {iconName}
				""");
		SetResourceReference(SourceProperty, "Icon:" + iconName);
	}

	public static bool IsKnownIcon(string iconName) => Enum.IsDefined<KnownIcon>(iconName);

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
