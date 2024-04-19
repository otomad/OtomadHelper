using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Xml.Linq;

namespace OtomadHelper.WPF.Controls.Flyouts;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxFlyout : BackdropWindow {
	public ComboBoxFlyout() {
		InitializeComponent();
	}

	public ComboBoxFlyout(IEnumerable<string> list, string selected) : this() {
		Selected = selected;
		SelectedIndex = list.ToList().IndexOf(selected);
		foreach (string item in list) {
			ComboBoxItem comboBoxItem = new() {
				Text = item,
				Selected = selected == item,
			};
			comboBoxItem.CurrentChanged += current => Selected = current; // TODO: set selected index.
			comboBoxItem.Click += (sender, e) => this.Vanish();
			Container.Children.Add(comboBoxItem);
		}
	}

	public ComboBoxFlyout(IEnumerable<string> list, int selectedIndex) : this() {
		SelectedIndex = selectedIndex;
		Selected = list.ElementAtOrDefault(selectedIndex);
		foreach ((string item, int index) in list.WithIndex()) {
			ComboBoxItem comboBoxItem = new() {
				Text = item,
				Selected = selectedIndex == index,
			};
			comboBoxItem.CurrentChanged += current => Selected = current; // TODO: set selected index.
			comboBoxItem.Click += (sender, e) => this.Vanish();
			Container.Children.Add(comboBoxItem);
		}
	}

	public string Selected { get; private set; } = "";
	public int SelectedIndex { get; private set; } = -1;
	public double ItemHeight {
		set {
			foreach (ComboBoxItem comboBoxItem in ComboBoxItems)
				comboBoxItem.Height = value;
		}
	}

	public new double Width {
		get => base.Width - ResourceMarginX;
		set => base.Width = value + ResourceMarginX;
	}

	private Thickness ResourceMargin => (Thickness)Resources["Margin"];
	private double ResourceMarginX => ResourceMargin.Left + ResourceMargin.Right;

	private void Window_Deactivated(object sender, EventArgs e) {
		this.Vanish();
	}

	private IEnumerable<ComboBoxItem> ComboBoxItems => Container.Children.OfType<ComboBoxItem>();

	private void ComboBoxFlyout_Loaded(object sender, RoutedEventArgs e) {
		Left -= ResourceMargin.Left;
		Top -= ResourceMargin.Top;
		foreach (UIElement element in Container.Children)
			if (element is ComboBoxItem comboBoxItem && comboBoxItem.Selected) {
				Point coordinates = comboBoxItem.TransformToAncestor(this).Transform(new Point(0, 0));
				Top -= coordinates.Y;
				break;
			}
	}
}
