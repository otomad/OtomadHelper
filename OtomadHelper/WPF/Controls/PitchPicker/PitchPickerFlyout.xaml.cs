using System.Windows;
using System.Windows.Data;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// PitchPicker.xaml 的交互逻辑
/// </summary>
public partial class PitchPickerFlyout : BaseFlyout {
	public PitchPickerFlyout() {
		InitializeComponent();

		BindingOperations.SetBinding(this, NoteNameProperty, new Binding("NoteName"));
		BindingOperations.SetBinding(this, OctaveProperty, new Binding("Octave"));
	}

	public static PitchPickerFlyout Initial(Rect targetRect) {
		PitchPickerFlyout picker = new();
		//picker.Width = targetRect.Width + picker.ItemPadding * 2;
		picker.Loaded += (sender, e) => picker.Center(targetRect, SetWidthType.Width);
		return picker;
	}

	private double ItemHeight => (double)Resources["ItemHeight"];
	private double ItemPadding => ((Thickness)Resources["ItemPadding"]).Left;
	private const int DISPLAY_ITEM_COUNT = 7;

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		Height = ItemHeight * DISPLAY_ITEM_COUNT + ItemPadding * 2;
		//this.GetChildrenOfType<ScrollViewer>().ForEach(scrollViewer =>
		//	scrollViewer.PreviewMouseWheel += (sender, e) => e.Handled = true);
	}

	public static readonly DependencyProperty NoteNameProperty = DependencyProperty.Register("NoteName", typeof(string), typeof(PitchPickerFlyout), new(NoteNamePropertyChanged));
	public static readonly DependencyProperty OctaveProperty = DependencyProperty.Register("Octave", typeof(int), typeof(PitchPickerFlyout), new(OctavePropertyChanged));

	public static void NoteNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}

	public static void OctavePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
		s = e.NewValue;
	}
}
