using System.Windows;
using System.Windows.Controls;
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
		nameof(Size), typeof(double), typeof(Icon), new PropertyMetadata(16.0));
	public double Size { get => (double)GetValue(SizeProperty); set => SetValue(SizeProperty, value); }

	private static readonly SolidColorBrush defaultForeground = new(Colors.Black);
	public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
		nameof(Foreground), typeof(Brush), typeof(Icon), new PropertyMetadata(defaultForeground));
	public Brush Foreground { get => (Brush)GetValue(ForegroundProperty); set => SetValue(SizeProperty, value); }

	public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
		nameof(Source), typeof(IconTemplate), typeof(Icon), new PropertyMetadata(null));
	public IconTemplate? Source { get => (IconTemplate)GetValue(SourceProperty); set => SetValue(SizeProperty, value); }
}
