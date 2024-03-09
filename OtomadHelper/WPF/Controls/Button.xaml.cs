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

	public string Text {
		get => BtnText.Text;
		set => BtnText.Text = value;
	}

	public string DialogResult { get; set; } = "";

	private static readonly RoutedEvent ClickRoutedEvent =
		EventManager.RegisterRoutedEvent(nameof(Click), RoutingStrategy.Bubble, typeof(EventHandler<RoutedEventArgs>), typeof(Button));

	public event RoutedEventHandler Click {
		add => AddHandler(ClickRoutedEvent, value);
		remove => RemoveHandler(ClickRoutedEvent, value);
	}

	public bool IsDefault {
		get => Btn.IsDefault;
		set => Btn.IsDefault = value;
	}
}
