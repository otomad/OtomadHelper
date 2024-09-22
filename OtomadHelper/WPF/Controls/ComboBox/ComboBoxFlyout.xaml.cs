using System.Windows;
using System.Windows.Input;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
[DependencyProperty<double>("ItemHeight", DefaultValue = 20.0)]
[DependencyProperty<bool>("IsPressingSpace", DefaultValue = false)]
public partial class ComboBoxFlyout : BaseFlyout {
	public ComboBoxFlyout() {
		InitializeComponent();

		KeyDown += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = true);
		KeyUp += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = false);
	}

	public new ComboBoxViewModel DataContext => (ComboBoxViewModel)base.DataContext;

	public static ComboBoxFlyout Initial(IEnumerable<string> list, string selected, Rect targetRect, out Task<string> dialogResult) {
		ComboBoxFlyout comboBox = new();
		comboBox.DataContext.Selected = selected;
		comboBox.DataContext.Items.AddRange(list);
		comboBox.SetTargetRect(targetRect);
		dialogResult = comboBox.GetDialogResultTask(() => comboBox.DataContext.Selected);
		return comboBox;
	}

	private double ResourcePadding => (double)Resources["Padding"];

	private void SetTargetRect(Rect rect) {
		SetLocation(rect);
		MinWidth = rect.Width + ResourcePadding * 4;
		ItemHeight = rect.Height + ResourcePadding * 2;
		Height = DataContext.Items.Count * ItemHeight + ResourcePadding * 3;
	}

	/*private struct StoryboardProperty {
		public double collapsedTop;
		public double expandedTop;
		public double collapsedHeight;
		public double expandedHeight;
	}
	private StoryboardProperty storyboardProperty;*/

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		Left -= ResourcePadding * 2;
		Top -= ResourcePadding * 3;
		Top -= Math.Max(DataContext.SelectedIndex, 0) * ItemHeight;
		MoveIntoScreen();
		/*storyboardProperty = new() {
			collapsedTop = Top,
			expandedTop = Top - Math.Max(DataContext.SelectedIndex, 0) * ItemHeight,
			collapsedHeight = ItemHeight + ResourcePadding * 2,
			expandedHeight = DataContext.Items.Count * ItemHeight + ResourcePadding * 2,
		};*/
	}

	/*private void Window_Closing(object sender, CancelEventArgs e) {
		if (!closeStoryboardCompleted) {
			BeginStoryboard(
				storyboardProperty.expandedHeight,
				storyboardProperty.collapsedHeight,
				storyboardProperty.expandedTop,
				storyboardProperty.collapsedTop,
				storyboardProperty.collapsedTop - storyboardProperty.expandedTop,
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
		double fromChildTranslate,
		bool isExit = false,
		Action? Completed = null
	) {
		Duration duration = (Duration)Resources["BaseAnimationDuration"];
		IEasingFunction easing = (IEasingFunction)Resources["EaseOutExpo"];

		if (!IsContent) {
			TimeSpan beginTime = isExit ? default : TimeSpan.FromMilliseconds(200);
			DoubleAnimation heightAnimation = new() {
				From = fromHeight,
				To = toHeight,
				Duration = duration,
				EasingFunction = easing,
				BeginTime = beginTime,
			};
			Storyboard.SetTargetProperty(heightAnimation, new("Height"));
			DoubleAnimation topAnimation = new() {
				From = fromTop,
				To = toTop,
				Duration = duration,
				EasingFunction = easing,
				BeginTime = beginTime,
			};
			Storyboard.SetTargetProperty(topAnimation, new("Top"));
			Storyboard parentStoryboard = new();
			parentStoryboard.Children.Add(heightAnimation);
			parentStoryboard.Children.Add(topAnimation);
			if (Completed != null)
				parentStoryboard.Completed += (sender, e) => Completed();
			parentStoryboard.Begin(this);
		} else {
			TimeSpan beginTime = isExit ? default : TimeSpan.FromMilliseconds(300);
			Rect from = new(0, fromChildTranslate, ActualWidth, ItemHeight);
			Rect to = new(0, 0, ActualWidth, ActualHeight);
			RectangleGeometry geometry = (RectangleGeometry)((GeometryDrawing)((DrawingGroup)((DrawingBrush)ItemsControlWrapper.OpacityMask).Drawing).Children[1]).Geometry;
			geometry.Rect = from;
			if (isExit) (from, to) = (to, from);
			RectAnimation rectAnimation = new() {
				From = from,
				To = to,
				Duration = duration,
				EasingFunction = easing,
				BeginTime = beginTime,
			};
			Storyboard.SetTargetProperty(rectAnimation, new("OpacityMask.Drawing.Children[1].Geometry.Rect"));
			Storyboard childStoryboard = new();
			childStoryboard.Children.Add(rectAnimation);
			if (Completed != null)
				childStoryboard.Completed += (sender, e) => Completed();
			childStoryboard.Begin(ItemsControlWrapper);
		}
	}*/
}
