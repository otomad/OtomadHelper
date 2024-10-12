using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<BaseFlyoutLoadedAnimation>("LoadedAnimation", DefaultValue = BaseFlyoutLoadedAnimation.None)]
public partial class BaseFlyout : BackdropWindow {
	public BaseFlyout() {
		InitializeComponent();
	}

	private void InitializeComponent() {
		// Default styles
		ResizeMode = ResizeMode.NoResize;
		ShowInTaskbar = false;
		SystemBackdropType = SystemBackdropType.TransientWindow;
		TitleBarType = TitleBarType.WindowChromeNoTitleBar;
		WindowStartupLocation = WindowStartupLocation.Manual;
		Topmost = true;
		Deactivated += (sender, e) => this.Vanish();
		Closing += BaseFlyout_Closing;
	}

	public void Center(Rect rect, SetWidthType widthType = SetWidthType.Nothing) {
		SetLocation(rect, widthType);
		Top -= (ActualHeight - rect.Height) / 2;
		Left -= (ActualWidth - rect.Width) / 2;
		MoveIntoScreen();
	}

	private Storyboard LoadedStoryboard { get; } = new();
	private void BeginLoadedStoryboard(object? sender, RoutedEventArgs? e) => LoadedStoryboard.Begin(this);
	public Duration BaseAnimationDuration => (Duration)Resources["BaseAnimationDuration"];
	public ExponentialEase EaseOutExpo => (ExponentialEase)Resources["EaseOutExpo"];
	private bool FadeInOutOnLoadedUnloaded => LoadedAnimation is BaseFlyoutLoadedAnimation.FadeIn or BaseFlyoutLoadedAnimation.FloatUp or BaseFlyoutLoadedAnimation.FloatDown;
	partial void OnLoadedAnimationChanged(BaseFlyoutLoadedAnimation loadedAnimation) {
		LoadedStoryboard.Children.Clear();
		Loaded -= BeginLoadedStoryboard;
		if (FadeInOutOnLoadedUnloaded) {
			DoubleAnimation fadeInAnimation = new() {
				From = 0,
				To = 1,
				Duration = BaseAnimationDuration,
				FillBehavior = FillBehavior.Stop,
			};
			Storyboard.SetTargetProperty(fadeInAnimation, new("Opacity"));
			LoadedStoryboard.Children.Add(fadeInAnimation);
			if (loadedAnimation is BaseFlyoutLoadedAnimation.FloatUp or BaseFlyoutLoadedAnimation.FloatDown) {
				DoubleAnimation floatInAnimation = new() {
					From = Top,
					To = Top + FLYOUT_OFFSET * (loadedAnimation == BaseFlyoutLoadedAnimation.FloatUp ? -1 : 1),
					Duration = new(TimeSpan.FromMilliseconds(500)),
					EasingFunction = EaseOutExpo,
					FillBehavior = FillBehavior.HoldEnd,
				};
				Storyboard.SetTargetProperty(floatInAnimation, new("Top"));
				LoadedStoryboard.Children.Add(floatInAnimation);
			}
			Loaded += BeginLoadedStoryboard;
		}
	}

	private void BeginUnloadedAnimation() {
		DoubleAnimation fadeInAnimation = new() {
			From = 1,
			To = 0,
			Duration = new(TimeSpan.FromMilliseconds(150)),
			FillBehavior = FillBehavior.HoldEnd,
		};
		Storyboard.SetTargetProperty(fadeInAnimation, new("Opacity"));
		Storyboard storyboard = new();
		storyboard.Children.Add(fadeInAnimation);
		storyboard.Completed += (sender, e) => Close();
		storyboard.Begin(this);
	}

	private bool waitToClose = false;
	private void BaseFlyout_Closing(object sender, CancelEventArgs e) {
		// If the window is closed too quickly, the text of the background element has not had time to switch,
		// so the old value will suddenly flash.
		if (waitToClose) return;
		e.Cancel = true;
		waitToClose = true;
		if (FadeInOutOnLoadedUnloaded)
			BeginUnloadedAnimation();
		else
			Task.Delay(50).Then(Close);
	}

	private const double FLYOUT_OFFSET = 15;
	public void Flyout(Rect rect) {
		Screen screen = Screen.FromHandle(Handle);
		System.Drawing.Rectangle workingArea = screen.WorkingArea;
		(_, double dpiY) = this.GetDpi();
		double screenHeight = workingArea.Height / dpiY;
		bool placeTop = rect.Top >= screenHeight - rect.Bottom;
		double clientHeight = Content is FrameworkElement element ? element.ActualHeight : ActualHeight;
		Top = placeTop ? rect.Top - clientHeight : rect.Bottom;
		Left = rect.Left - (ActualWidth - rect.Width) / 2;
		MoveIntoScreen();
		LoadedAnimation = placeTop ? BaseFlyoutLoadedAnimation.FloatUp : BaseFlyoutLoadedAnimation.FloatDown;
		BeginLoadedStoryboard(null, null);
	}
}

public enum BaseFlyoutLoadedAnimation {
	None,
	FadeIn,
	FloatUp,
	FloatDown,
}
