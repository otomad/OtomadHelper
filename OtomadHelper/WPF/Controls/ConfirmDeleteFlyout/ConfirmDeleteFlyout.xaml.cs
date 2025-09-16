namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ConfirmDeleteFlyout.xaml 的交互逻辑
/// </summary>
public partial class ConfirmDeleteFlyout : BaseFlyout {
	public ConfirmDeleteFlyout() {
		InitializeComponent();
	}

	public new ConfirmDeleteFlyoutViewModel DataContext => (ConfirmDeleteFlyoutViewModel)base.DataContext;

	public static ConfirmDeleteFlyout Initial(Rect targetRect, string message, out Task<bool> dialogResult) {
		ConfirmDeleteFlyout flyout = new();
		ConfirmDeleteFlyoutViewModel viewModel = flyout.DataContext;
		viewModel.Message = message;
		dialogResult = flyout.GetDialogResultTask(() => flyout.DataContext.DialogResult);
		flyout.Loaded += (_, _) => flyout.Flyout(targetRect);
		return flyout;
	}

	private void ConfirmDeleteFlyout_Showing(object sender, System.Windows.RoutedEventArgs e) =>
		Controls.Icon.PlaySound("Question");
}
