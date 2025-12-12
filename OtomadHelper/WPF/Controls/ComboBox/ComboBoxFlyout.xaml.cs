using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ComboBoxFlyout.xaml 的交互逻辑
/// </summary>
[DependencyProperty<double>("ItemHeight", DefaultValue = 20d)]
[DependencyProperty<bool>("IsPressingSpace", DefaultValue = false)]
public partial class ComboBoxFlyout : BaseFlyout {
	public ComboBoxFlyout() {
		InitializeComponent();

		KeyDown += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = true);
		KeyUp += (sender, e) => _ = e.Key == Key.Space && (IsPressingSpace = false);
	}

	public ComboBoxFlyout(bool isContent = false) : this() {
		IsContent = isContent;
		if (IsContent) {
			CloseWhenDeactived = false;
			SystemBackdropType = SystemBackdropType.None;
			TitleBarType = TitleBarType.Borderless;
			BorderThickness = new(0);
			Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
		}
	}

	public new ComboBoxViewModel DataContext => (ComboBoxViewModel)base.DataContext;

	public static ComboBoxFlyout Initial<T>(IEnumerable<T> ids, IEnumerable<string> options, IEnumerable<string>? icons, T selected, Rect targetRect, out Task<T> dialogResult) {
		ComboBoxFlyout comboBox = new(), comboBoxContent = new(true);
		foreach (ComboBoxFlyout combo in new[] { comboBox, comboBoxContent }) {
			combo.DataContext.Selected = combo.DataContext.originalSelected = selected!;
			foreach (T id in ids)
				combo.DataContext.Ids.Add(id!);
			combo.DataContext.Options.AddRange(options);
			if (icons is not null)
				combo.DataContext.Icons.AddRange(icons.Select(svg => IconTemplate.FromSvg(svg)));
			combo.SetTargetRect(targetRect);
		}
		//comboBoxContent.Deactivated += (_, _) => {
		//	if (comboBoxContent.Related is not null && !comboBoxContent.Related.IsActive)
		//		comboBoxContent.Vanish();
		//};
		comboBox.Related = comboBoxContent;
		comboBoxContent.Related = comboBox;
		comboBox.Showing += (sender, e) => {
			comboBoxContent.OwnerHandle = comboBox.Handle;
			comboBoxContent.Show();
		};
		//comboBox.Closing += (sender, e) => comboBoxContent.Vanish();
		comboBoxContent.Closing += (sender, e) => comboBox.Vanish();
		dialogResult = comboBoxContent.GetDialogResultTask(() => (T)comboBoxContent.DataContext.Selected);
		return comboBox;
	}

	private double ResourcePadding => (double)Resources["Padding"];

	public static readonly DependencyProperty IsContentProperty = DependencyProperty.Register(
		nameof(IsContent), typeof(bool), typeof(ComboBoxFlyout), new PropertyMetadata(false));
	private bool IsContent { get => (bool)GetValue(IsContentProperty); set => SetValue(IsContentProperty, value); }
	internal ComboBoxFlyout? Related { get; set; }

	private void SetTargetRect(Rect rect) {
		SetLocation(rect);
		MinWidth = rect.Width + ResourcePadding * 4;
		ItemHeight = rect.Height + ResourcePadding * 2;
		Height = DataContext.Ids.Count * ItemHeight + ResourcePadding * 3;
	}

	private struct StoryboardProperty {
		public double collapsedTop;
		public double expandedTop;
		public double collapsedHeight;
		public double expandedHeight;
	}
	private StoryboardProperty storyboardProperty;

	protected override void OnSourceInitialized(EventArgs e) {
		base.OnSourceInitialized(e);
		if (IsContent) {
			HwndSource? source = PresentationSource.FromVisual(this) as HwndSource;
			source?.AddHook((IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled) => {
				if (msg == 0x0021) {
					handled = true;
					return new IntPtr(0x0003);
				} else
					return IntPtr.Zero;
			});
		}
	}

	private void Window_Loaded(object sender, RoutedEventArgs e) {
		closeStoryboardCompleted = false;
		//if (IsContent) {
		//	Activate();
		//	//CloseWhenDeactived = true;
		//}
		//Left -= ResourcePadding * 2;
		//Top -= ResourcePadding * 3;
		//Top -= Math.Max(DataContext.SelectedIndex, 0) * ItemHeight;
		Left -= ResourcePadding * 2;
		Top -= ResourcePadding * 3;
		if (IsContent) Top -= Math.Max(DataContext.SelectedIndex, 0) * ItemHeight;
		MoveIntoScreen();
		RadioButton? checkedRadio = ItemsControlWrapper.GetChildrenOfType<RadioButton>().FirstOrDefault(radio => radio.IsChecked == true);
		checkedRadio?.BringIntoView();
		storyboardProperty = new() {
			collapsedTop = Top,
			expandedTop = Top - Math.Max(DataContext.SelectedIndex, 0) * ItemHeight,
			collapsedHeight = ItemHeight + ResourcePadding * 2,
			expandedHeight = DataContext.Options.Count * ItemHeight + ResourcePadding * 3,
		};
		BeginStoryboard(
			storyboardProperty.collapsedHeight,
			storyboardProperty.expandedHeight,
			storyboardProperty.collapsedTop,
			storyboardProperty.expandedTop,
			storyboardProperty.collapsedTop - storyboardProperty.expandedTop,
			isExit: false
		);

		//if (!IsContent)
		//	AddExtendedWindowStyles(Handle, ExtendedWindowStyles.NoActivate);
		//if (IsContent) {
			//CloseWhenDeactived = true;
			Deactivated += (_, _) => {
				s = (IsContent ? "content" : "base") + " deactivated";
				//IntPtr activeWindow = GetActiveWindow();
				//s = !IsContent ? (activeWindow, Handle, Related?.Handle, IsActive, Related?.IsActive) : (activeWindow, Related?.Handle, Handle, Related?.IsActive, IsActive);
				//if (Related is not null && Handle != activeWindow && Related.Handle != activeWindow)
				//	this.Vanish();
			};
		//}
	}

	private bool closeStoryboardCompleted = false, isClosing = false;
	private void Window_Closing(object sender, CancelEventArgs e) {
		if (!closeStoryboardCompleted && !isClosing) {
			isClosing = true;
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
		ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

		if (!IsContent) {
			DoubleAnimation heightAnimation = new() {
				From = fromHeight,
				To = toHeight,
				Duration = duration,
				EasingFunction = easing,
			};
			Storyboard.SetTargetProperty(heightAnimation, new("Height"));
			DoubleAnimation topAnimation = new() {
				From = fromTop,
				To = toTop,
				Duration = duration,
				EasingFunction = easing,
			};
			Storyboard.SetTargetProperty(topAnimation, new("Top"));
			Storyboard parentStoryboard = new();
			parentStoryboard.Children.Add(heightAnimation);
			parentStoryboard.Children.Add(topAnimation);
			parentStoryboard.Completed += (_, _) => {
				ScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
				Completed?.Invoke();
			};
			parentStoryboard.Begin(this);
		} else {
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
			};
			Storyboard.SetTargetProperty(rectAnimation, new("OpacityMask.Drawing.Children[1].Geometry.Rect"));
			Storyboard childStoryboard = new();
			childStoryboard.Children.Add(rectAnimation);
			if (Completed != null)
				childStoryboard.Completed += (_, _) => Completed();
			childStoryboard.Begin(ItemsControlWrapper);
		}
	}
}
