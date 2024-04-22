using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxFlyout : BackdropWindow {
	public ComboBoxFlyout() {
		InitializeComponent();
	}
	/*public ComboBoxFlyout(IEnumerable<string> list, string selected) : this() {
		Selected = selected;
		foreach (string item in list) {
			ComboBoxItem comboBoxItem = new() {
				Text = item,
				Current = selected,
			};
			comboBoxItem.CurrentChanged += current => Selected = current;
			comboBoxItem.Click += (sender, e) => this.Vanish();
			Container.Children.Add(comboBoxItem);
		}
	}

	public ComboBoxFlyout(IEnumerable<string> list, int selectedIndex) : this(list, list.ElementAtOrDefault(selectedIndex)) { }

	public IEnumerable<string> ItemList { get; private set; } = new string[0];
	public string Selected { get; private set; } = "";
	public int SelectedIndex => ItemList.ToList().IndexOf(Selected);

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
	}*/
}
