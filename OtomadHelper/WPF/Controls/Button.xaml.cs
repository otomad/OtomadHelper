using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DialogResult = System.Windows.Forms.DialogResult;

namespace OtomadHelper.WPF.Controls;
/// <summary>
/// Button.xaml 的交互逻辑
/// </summary>
public partial class Button : UserControl {
	public Button() {
		InitializeComponent();
		Btn.Click += (sender, e) => RaiseEvent(new RoutedEventArgs(ClickRoutedEvent, this));
	}

	public string Text {
		get => BtnText.Text;
		set => BtnText.Text = value;
	}

	public DialogResult DialogResult { get; set; }

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
