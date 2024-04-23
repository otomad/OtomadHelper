using System.Windows;
using System.Windows.Controls;

using OtomadHelper.Interop;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxFlyout : BackdropWindow {
	public ComboBoxFlyout() {
		InitializeComponent();
	}

	public new ComboBoxViewModel DataContext => (ComboBoxViewModel)base.DataContext;

	public static ComboBoxFlyout Initial(IEnumerable<string> list, string selected, Bounding targetBounding) {
		ComboBoxFlyout comboBox = new();
		ComboBoxViewModel viewModel = comboBox.DataContext;
		viewModel.Items.AddRange(list.Select(text => new ComboBoxViewModelItem(text, viewModel)));
		viewModel.Selected = selected;
		comboBox.SetTargetBounding(targetBounding);
		return comboBox;
	}

	public new double Width {
		get => base.Width - ResourceMarginX;
		set => base.Width = value + ResourceMarginX;
	}

	private Thickness ResourceMargin => (Thickness)Resources["Padding"];
	private double ResourceMarginX => ResourceMargin.Left + ResourceMargin.Right;

	private void Window_Deactivated(object sender, EventArgs e) => this.Vanish();

	private void SetTargetBounding(Bounding bounding) {
		Left = bounding.Left;
		Top = bounding.Top;
		Width = bounding.Width;
		ItemHeight = bounding.Height;
	}

	public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
		nameof(ItemHeight), typeof(double), typeof(ComboBoxFlyout), new PropertyMetadata(20.0));
	public double ItemHeight { get => (double)GetValue(ItemHeightProperty); set => SetValue(ItemHeightProperty, value); }

	private void ComboBoxFlyout_Loaded(object sender, RoutedEventArgs e) {
		Left -= ResourceMargin.Left;
		Top -= ResourceMargin.Top;
		Top -= DataContext.SelectedIndex * ItemHeight;
	}
}
