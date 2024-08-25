using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// Icon.xaml 的交互逻辑
/// </summary>
public partial class Icon : Viewbox {
	public Icon() {
		InitializeComponent();
		Loaded += Icon_Loaded;
	}

	private void Icon_Loaded(object sender, RoutedEventArgs e) {
		if (Foreground == defaultForeground)
			SetResourceReference(ForegroundProperty, "ForegroundBrush");
	}

	public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
		nameof(Size), typeof(double), typeof(Icon), new(16.0));
	public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

	private static readonly SolidColorBrush defaultForeground = new(Colors.Black);
	public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
		nameof(Foreground), typeof(Brush), typeof(Icon), new(defaultForeground));
	public Brush Foreground { get => (Brush)GetValue(ForegroundProperty); set => SetValue(SizeProperty, value); }

	public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
		nameof(Source), typeof(IconTemplate), typeof(Icon), new(null));
	public IconTemplate? Source { get => (IconTemplate?)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

	public static readonly DependencyProperty IconNameProperty = DependencyProperty.Register(
		nameof(IconName), typeof(string), typeof(Icon), new(null, IconNamePropertyChanged));
	public string? IconName { get => (string?)GetValue(IconNameProperty); set => SetValue(IconNameProperty, value); }
	private static void IconNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		string iconName = (string)e.NewValue;
		Icon iconEl = (Icon)sender;
		iconEl.SetResourceReference(SourceProperty, "Icon:" + iconName);
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
	public static string NormalizeIconName(object iconName) => new VariableName(iconName.ToString()).Pascal;
}
