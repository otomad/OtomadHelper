using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

public class BaseFlyout : BackdropWindow {
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
		Deactivated += (sender, e) => this.Vanish();

		// Loaded animation
		DoubleAnimation fadeInAnimation = new() {
			From = 0,
			To = 1,
			Duration = new(TimeSpan.FromMilliseconds(250)),
			FillBehavior = FillBehavior.Stop,
		};
		Storyboard.SetTargetProperty(fadeInAnimation, new("Opacity"));
		Storyboard fadeInStoryboard = new();
		fadeInStoryboard.Children.Add(fadeInAnimation);
		Loaded += (sender, e) => fadeInStoryboard.Begin(this);
	}

	public void Center(Rect rect, SetWidthType widthType = SetWidthType.Nothing) {
		SetLocation(rect, widthType);
		Top -= (ActualHeight - rect.Height) / 2;
		Left -= (ActualWidth - rect.Width) / 2;
		MoveIntoScreen();
	}
}
