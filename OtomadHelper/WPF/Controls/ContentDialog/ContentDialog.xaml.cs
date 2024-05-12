namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public ContentDialog() {
		InitializeComponent();
		Loaded += (sender, e) => UpdateShadingIcon();
	}

	public new ContentDialogViewModel DataContext => (ContentDialogViewModel)base.DataContext;

	public static TDialogResult? ShowDialog<TDialogResult>(
		string title,
		string body,
		IEnumerable<ContentDialogButtonItem> buttons,
		string iconName = ""
	) {
		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = title;
		viewModel.Body = body;
		viewModel.IconName = iconName;
		viewModel.Buttons.AddRange(buttons);
		dialog.ShowDialog();
		return (TDialogResult?)viewModel.DialogResult;
}

	internal void UpdateShadingIcon() =>
		IconEl.SetResourceReference(Controls.Icon.SourceProperty, "Icon:" + DataContext.IconName);
}
