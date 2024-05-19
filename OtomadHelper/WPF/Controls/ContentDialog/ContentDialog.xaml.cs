using System.Windows;
using System.Windows.Media.Animation;

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

	private double expandedHeight = double.NaN;
	private void ExpandButton_Click(object sender, RoutedEventArgs e) {
		bool isExpanded = ExpandButton.IsChecked == true;
		{
			double fromHeight, toHeight;
			if (!isExpanded) {
				if (expandedHeight is double.NaN)
					expandedHeight = BodyWrapper.ActualHeight;
				fromHeight = expandedHeight;
				toHeight = 0;
				BodyWrapper.Height = 0;
			} else {
				BodyWrapper.Height = double.NaN;
				toHeight = expandedHeight;
				fromHeight = 0;
			}

			DoubleAnimation animation = new() {
				From = fromHeight,
				To = toHeight,
				Duration = (Duration)Resources["BaseAnimationDuration"],
				FillBehavior = FillBehavior.Stop,
				EasingFunction = (IEasingFunction)Resources["EaseOutExpo"],
			};
			Storyboard.SetTargetProperty(animation, new("Height"));
			Storyboard storyboard = new();
			storyboard.Children.Add(animation);
			storyboard.Begin(BodyWrapper);
		}
		{
			double fromHeight = ActualHeight;
			double toHeight = isExpanded ? fromHeight + expandedHeight : fromHeight - expandedHeight;
			SizeToContent sizeToContent = SizeToContent;
			SizeToContent = SizeToContent.Manual;

			DoubleAnimation animation = new() {
				From = fromHeight,
				To = toHeight,
				Duration = (Duration)Resources["BaseAnimationDuration"],
				FillBehavior = FillBehavior.Stop,
				EasingFunction = (IEasingFunction)Resources["EaseOutExpo"],
			};
			Storyboard.SetTargetProperty(animation, new("Height"));
			Storyboard storyboard = new();
			storyboard.Children.Add(animation);
			storyboard.Completed += (sender, e) => SizeToContent = sizeToContent;
			storyboard.Begin(this);
		}
	}
}
