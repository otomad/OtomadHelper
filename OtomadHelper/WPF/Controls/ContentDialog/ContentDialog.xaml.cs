using System.Windows;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public ContentDialog() {
		InitializeComponent();
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
			// new("Report", "report"), // I'm worried that users encounter any bug, they immediately click to report it directly.
			new("Close", "close"),
		});
		viewModel.Expandable = true;
		viewModel.CanCopyBody = true;
		dialog.ShowDialog();
	}

	public static void ShowError(Exception exception) =>
		ShowError(exception.Message, exception.StackTrace);

	private void ExpandButton_Click(object sender, RoutedEventArgs e) {
		// When the user focuses on the expand button and then holds down the Enter key,
		// this will trigger the button at a high frequency, causing animation abnormalities.
		if (DataContext.IsExpansionRunning) return;
		DataContext.IsExpansionRunning = true;

		bool isExpanded = ExpandButton.IsChecked == true;
		double expandedHeight, windowFromHeight = ActualHeight;
		{
			double fromHeight, toHeight;
			if (!isExpanded) {
				fromHeight = expandedHeight = BodyWrapper.ActualHeight;
				toHeight = 0;
				BodyWrapper.Height = 0;
			} else {
				BodyWrapper.Height = double.NaN;
				BodyWrapper.UpdateLayout();
				toHeight = expandedHeight = BodyWrapper.ActualHeight;
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
			double fromHeight = windowFromHeight;
			double toHeight = fromHeight + expandedHeight * (isExpanded ? 1 : -1);
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
			storyboard.Completed += (sender, e) => {
				SizeToContent = sizeToContent;
				//ITimer.Thread.Timeout(() => Dispatcher.Invoke(() => DataContext.IsExpansionRunning = false), 200);
			};
			storyboard.Begin(this);
		}
	}
}
