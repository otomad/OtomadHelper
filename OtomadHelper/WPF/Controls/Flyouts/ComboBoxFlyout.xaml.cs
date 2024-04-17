namespace OtomadHelper.WPF.Controls.Flyouts;

/// <summary>
/// ComboBox.xaml 的交互逻辑
/// </summary>
public partial class ComboBoxFlyout : BackdropWindow {
	public ComboBoxFlyout() {
		InitializeComponent();
	}

	private void Window_Deactivated(object sender, EventArgs e) {
		this.Vanish();
	}
}
