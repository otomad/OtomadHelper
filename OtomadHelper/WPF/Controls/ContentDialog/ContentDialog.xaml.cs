using System.Windows.Input;
using System.Windows.Threading;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public ContentDialog() {
		InitializeComponent();
		System.Windows.Controls.TextBox textbox;
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

	public static void ShowError(
		string message,
		string stackTrace
	) {
		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = message;
		viewModel.Subtitle = "错误"; // TODO: localization.
		viewModel.Body = stackTrace;
		viewModel.IconName = "Error";
		viewModel.Buttons.AddRange(new ContentDialogButtonItem[] {
			new("Report", "report"),
			new("Close", "close", true),
		});
		viewModel.Expandable = true;
		viewModel.CanCopyBody = true;
		dialog.ShowDialog();
	}

	public static void ShowError(Exception exception) =>
		ShowError(exception.Message, exception.StackTrace);
}
