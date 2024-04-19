using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OtomadHelper.WPF.Controls;
/// <summary>
/// Button.xaml 的交互逻辑
/// </summary>
[ContentProperty(nameof(Text))]
public partial class Button : UserControl {
	public Button() {
		InitializeComponent();
		Btn.Click += (sender, e) => RaiseEvent(new RoutedEventArgs(ClickRoutedEvent, this));
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		nameof(Text), typeof(string), typeof(Button), new PropertyMetadata(""));
	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

	public object DialogResult { get; set; } = "";

	private static readonly RoutedEvent ClickRoutedEvent =
		EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(Button));

	public event RoutedEventHandler Click {
		add => AddHandler(ClickRoutedEvent, value);
		remove => RemoveHandler(ClickRoutedEvent, value);
	}

	public static readonly DependencyProperty IsDefaultProperty = DependencyProperty.Register(
		nameof(IsDefault), typeof(bool), typeof(Button), new PropertyMetadata(false));
	public bool IsDefault { get => (bool)GetValue(IsDefaultProperty); set => SetValue(IsDefaultProperty, value); }
}
