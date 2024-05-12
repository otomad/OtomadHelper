using System.Windows;

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
	}

	public void CenterVertically(Rect rect, SetWidthType widthType = SetWidthType.Nothing) {
		SetLocation(rect, widthType);
		Top -= (ActualHeight - rect.Height) / 2;
	}
}
