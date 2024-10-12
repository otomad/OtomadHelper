namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ConfirmDeleteFlyout.xaml 的交互逻辑
/// </summary>
public partial class ConfirmDeleteFlyout : BaseFlyout {
	public ConfirmDeleteFlyout() {
		InitializeComponent();
	}

	public new ConfirmDeleteFlyoutViewModel DataContext => (ConfirmDeleteFlyoutViewModel)base.DataContext;

	public static bool ShowFlyout(Rect rect, string body) {
		ConfirmDeleteFlyout flyout = new();
		ConfirmDeleteFlyoutViewModel viewModel = flyout.DataContext;
		viewModel.Body = body;
		flyout.ShowDialog();
		return viewModel.DialogResult;
	}
}
