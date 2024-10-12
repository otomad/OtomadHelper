using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<BaseFlyoutStartupAnimation>("StartupAnimation", DefaultValue = BaseFlyoutStartupAnimation.None)]
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

		LoadedAnimation();
	}

	public void Center(Rect rect, SetWidthType widthType = SetWidthType.Nothing) {
		SetLocation(rect, widthType);
		Top -= (ActualHeight - rect.Height) / 2;
		Left -= (ActualWidth - rect.Width) / 2;
		MoveIntoScreen();
	}

	private void LoadedAnimation() {
		if (StartupAnimation == BaseFlyoutStartupAnimation.FadeIn) {
			DoubleAnimation fadeInAnimation = new() {
				From = 0,
				To = 1,
				Duration = (Duration)Resources["BaseAnimationDuration"],
				FillBehavior = FillBehavior.Stop,
			};
			Storyboard.SetTargetProperty(fadeInAnimation, new("Opacity"));
			Storyboard fadeInStoryboard = new();
			fadeInStoryboard.Children.Add(fadeInAnimation);
			Loaded += (sender, e) => fadeInStoryboard.Begin(this);
		}
	}

	private bool waitToClose = false;
	private void BaseFlyout_Closing(object sender, CancelEventArgs e) {
		// If the window is closed too quickly, the text of the background element has not had time to switch,
		// so the old value will suddenly flash.
		if (waitToClose) return;
		e.Cancel = true;
		waitToClose = true;
		Task.Delay(50).Then(Close);
	}

	public void Flyout(Rect rect) {
		Screen screen = Screen.FromHandle(Handle);
		System.Drawing.Rectangle workingArea = screen.WorkingArea;
		(_, double dpiY) = this.GetDpi();
		double screenHeight = workingArea.Height / dpiY;
		bool placeTop = rect.Top >= screenHeight - rect.Bottom;
		Top = placeTop ? rect.Top - ActualHeight : rect.Bottom;
	}
}

public enum BaseFlyoutStartupAnimation {
	None,
	FadeIn,
	FloatIn,
}
