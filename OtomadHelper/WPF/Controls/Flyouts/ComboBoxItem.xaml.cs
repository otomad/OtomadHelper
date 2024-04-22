using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace OtomadHelper.WPF.Controls.Flyouts;

/// <summary>
/// ComboBoxItem.xaml 的交互逻辑
/// </summary>
[ContentProperty(nameof(Text))]
public partial class ComboBoxItem : UserControl {
	public ComboBoxItem() {
		InitializeComponent();
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		nameof(Text), typeof(string), typeof(BackdropWindow), new PropertyMetadata(""));
	public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }

	public bool Selected => RadioButton.IsChecked == true;

	public static readonly DependencyProperty CurrentProperty = DependencyProperty.Register(
		nameof(Current), typeof(string), typeof(BackdropWindow), new PropertyMetadata("", DebugPropertyChanged));
	public string Current { get => (string)GetValue(CurrentProperty); set => SetValue(CurrentProperty, value); }

	private void RadioButton_Checked(object sender, RoutedEventArgs e) {
		//Current = Text;
		//CurrentChanged?.Invoke(Text);
	}

	public delegate void CurrentChangedHandler(string current);
	public event CurrentChangedHandler CurrentChanged = null!;

	private static readonly RoutedEvent ClickRoutedEvent =
		EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(Button));
	public event RoutedEventHandler Click {
		add => AddHandler(ClickRoutedEvent, value);
		remove => RemoveHandler(ClickRoutedEvent, value);
	}
	private void RadioButton_Click(object sender, RoutedEventArgs e) {
		//RaiseEvent(new RoutedEventArgs(ClickRoutedEvent, this));
	}
}
