using System.Windows.Controls;

using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Test;

public partial class TestControlsWPF : BackdropWindow {
	public TestControlsWPF() {
		InitializeComponent();
	}

	private void SystemBackdropTypeChange(object sender, System.Windows.RoutedEventArgs e) {
		string text = (string)((RadioButton)sender).Content;
		SystemBackdropType backdrop = text switch {
			"Mica" => SystemBackdropType.MainWindow,
			"MicaAlt" => SystemBackdropType.TabbedWindow,
			"Acrylic" => SystemBackdropType.TransientWindow,
			"Solid" => SystemBackdropType.None,
			_ => SystemBackdropType.Auto,
		};
		if (backdrop == SystemBackdropType.Auto) return;
		SystemBackdropType = backdrop;
	}
}
