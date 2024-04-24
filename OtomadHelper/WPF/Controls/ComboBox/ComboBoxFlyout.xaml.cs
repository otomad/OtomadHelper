using System.Windows;
using System.Windows.Media.Animation;

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
		get => base.Width - ResourcePadding * 4;
		set => base.Width = value + ResourcePadding * 4;
	}

	private double ResourcePadding => (double)Resources["Padding"];

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

	private struct StoryboardProperty {
		public double collapsedTop;
		public double expandedTop;
		public double collapsedHeight;
		public double expandedHeight;
	}

	private StoryboardProperty storyboardProperty;

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		closeStoryboardCompleted = false;
		Left -= ResourcePadding * 2;
		Top -= ResourcePadding * 2;
		storyboardProperty = new() {
			collapsedTop = Top,
			expandedTop = Top - DataContext.SelectedIndex * ItemHeight,
			collapsedHeight = ItemHeight + ResourcePadding * 4,
			expandedHeight = DataContext.Items.Count * (ItemHeight + ResourcePadding * 2),
		};

		//SizeToContent sizeToContent = SizeToContent;
		SizeToContent = SizeToContent.Manual;
		Height = storyboardProperty.collapsedHeight;
		BeginStoryboard(
			storyboardProperty.collapsedHeight,
			storyboardProperty.expandedHeight,
			storyboardProperty.collapsedTop,
			storyboardProperty.expandedTop,
			storyboardProperty.expandedTop - storyboardProperty.collapsedTop,
			0,
			isExit: false
		);
	}

	private bool closeStoryboardCompleted = false;
	private void Window_Closing(object sender, CancelEventArgs e) {
		if (!closeStoryboardCompleted) {
			BeginStoryboard(
				storyboardProperty.expandedHeight,
				storyboardProperty.collapsedHeight,
				storyboardProperty.expandedTop,
				storyboardProperty.collapsedTop,
				0,
				storyboardProperty.expandedTop - storyboardProperty.collapsedTop,
				isExit: true,
				Completed: () => {
					closeStoryboardCompleted = true;
					this.Vanish();
				}
			);
			e.Cancel = true;
		}
	}

	private void BeginStoryboard(
		double fromHeight,
		double toHeight,
		double fromTop,
		double toTop,
		double fromChildShift,
		double toChildShift,
		bool isExit = false,
		Action? Completed = null
	) {
		Duration duration = new(TimeSpan.FromMilliseconds(250));
		IEasingFunction easing = (IEasingFunction)Resources["IndicatorAnimationEasingFunction"];
		TimeSpan beginTime = isExit ? default : TimeSpan.FromMilliseconds(200);

		DoubleAnimation heightAnimation = new() {
			From = fromHeight,
			To = toHeight,
			Duration = duration,
			EasingFunction = easing,
			BeginTime = beginTime,
		};
		Storyboard.SetTargetName(heightAnimation, "View");
		Storyboard.SetTargetProperty(heightAnimation, new("Height"));
		DoubleAnimation topAnimation = new() {
			From = fromTop,
			To = toTop,
			Duration = duration,
			EasingFunction = easing,
			BeginTime = beginTime,
		};
		Storyboard.SetTargetName(topAnimation, "View");
		Storyboard.SetTargetProperty(topAnimation, new("Top"));
		Storyboard parentStoryboard = new();
		parentStoryboard.Children.Add(heightAnimation);
		parentStoryboard.Children.Add(topAnimation);
		if (Completed != null) parentStoryboard.Completed += (sender, e) => Completed();

		DoubleAnimation childShiftAnimation = new() {
			From = fromChildShift,
			To = toChildShift,
			Duration = duration,
			EasingFunction = easing,
			BeginTime = beginTime,
		};
		Storyboard.SetTargetName(childShiftAnimation, "ItemsControlWrapper");
		Storyboard.SetTargetProperty(childShiftAnimation, new("(Canvas.Top)"));
		Storyboard childStoryboard = new();
		childStoryboard.Children.Add(childShiftAnimation);

		parentStoryboard.Begin(this);
		childStoryboard.Begin(ItemsControlWrapper);
	}
}
