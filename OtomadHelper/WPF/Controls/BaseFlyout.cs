using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

[DependencyProperty<bool>("FadeIn", DefaultValue = false)]
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

		// Loaded animation
		if (FadeIn) {
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

	public void Center(Rect rect, SetWidthType widthType = SetWidthType.Nothing) {
		SetLocation(rect, widthType);
		Top -= (ActualHeight - rect.Height) / 2;
		Left -= (ActualWidth - rect.Width) / 2;
		MoveIntoScreen();
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
}
