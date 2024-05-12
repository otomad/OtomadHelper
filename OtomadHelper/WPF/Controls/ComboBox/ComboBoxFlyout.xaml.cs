using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using OtomadHelper.Interop;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxFlyout : BackdropWindow {
	public ComboBoxFlyout() {
		InitializeComponent();

		KeyDown += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = true);
		KeyUp += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = false);
	}

	public new ComboBoxViewModel DataContext => (ComboBoxViewModel)base.DataContext;

	public static ComboBoxFlyout Initial(IEnumerable<string> list, string selected, Rect targetRect) {
		ComboBoxFlyout comboBox = new();
		SetDataContext(comboBox.DataContext);
		comboBox.SetTargetRect(targetRect);
		return comboBox;

		void SetDataContext(ComboBoxViewModel viewModel) {
			viewModel.Items.AddRange(list.Select(text => new ComboBoxViewModelItem(text, viewModel)));
			viewModel.Selected = selected;
		}
	}

	public new double Width {
		get => base.Width - ResourcePadding * 4;
		set => base.Width = value + ResourcePadding * 4;
	}

	public new double MinWidth {
		get => base.MinWidth - ResourcePadding * 4;
		set => base.MinWidth = value + ResourcePadding * 4;
	}

	private double ResourcePadding => (double)Resources["Padding"];

	private void Window_Deactivated(object sender, EventArgs e) =>
		this.Vanish();

	private void SetTargetRect(Rect rect) {
		Left = rect.Left;
		Top = rect.Top;
		MinWidth = rect.Width;
		ItemHeight = rect.Height;
	}

	public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
		nameof(ItemHeight), typeof(double), typeof(ComboBoxFlyout), new PropertyMetadata(20.0));
	public double ItemHeight {
		get => (double)GetValue(ItemHeightProperty) - ResourcePadding * 2;
		set => SetValue(ItemHeightProperty, value + ResourcePadding * 2);
	}

	protected static readonly DependencyProperty IsPressingSpaceProperty = DependencyProperty.Register(
		nameof(IsPressingSpace), typeof(bool), typeof(BackdropWindow), new PropertyMetadata(false));
	protected bool IsPressingSpace {
		get => (bool)GetValue(IsPressingSpaceProperty);
		set => SetValue(IsPressingSpaceProperty, value);
	}

	/*private struct StoryboardProperty {
		public double collapsedTop;
		public double expandedTop;
		public double collapsedHeight;
		public double expandedHeight;
	}
	private StoryboardProperty storyboardProperty;*/

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		closeStoryboardCompleted = false;

		Left -= ResourcePadding * 2;
		Top -= ResourcePadding * 2;
		Top -= Math.Max(DataContext.SelectedIndex, 0) * (ItemHeight + ResourcePadding * 2);
		/*storyboardProperty = new() {
			collapsedTop = Top,
			expandedTop = Top - Math.Max(DataContext.SelectedIndex, 0) * (ItemHeight + ResourcePadding * 2),
			collapsedHeight = ItemHeight + ResourcePadding * 4,
			expandedHeight = DataContext.Items.Count * (ItemHeight + ResourcePadding * 2) + ResourcePadding * 2,
		};*/
	}

	private bool closeStoryboardCompleted = false;
	private void Window_Closing(object sender, CancelEventArgs e) {
		/*if (!closeStoryboardCompleted) {
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
		}*/
	}

	/*private void BeginStoryboard(
		double fromHeight,
		double toHeight,
		double fromTop,
		double toTop,
		double fromChildTranslate,
		bool isExit = false,
		Action? Completed = null
	) {
		Duration duration = new(TimeSpan.FromMilliseconds(250));
		IEasingFunction easing = (IEasingFunction)Resources["IndicatorAnimationEasingFunction"];

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
