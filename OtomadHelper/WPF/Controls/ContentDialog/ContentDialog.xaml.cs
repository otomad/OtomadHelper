using System.Windows;
using System.Windows.Controls;
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

	public Task<object?> ShowDialogAsync() {
		TaskCompletionSource<object?> taskCompletionSource = new();
		// Show a modal dialog after the current event handler is completed,
		// to avoid potential reentrancy caused by running a nested message loop in the WebView2 event handler.
		if (SynchronizationContext.Current is not null)
			SynchronizationContext.Current.Post(state => ShowDialogAndGetResult(), null);
		else
			ShowDialogAndGetResult();
		return taskCompletionSource.Task;

		void ShowDialogAndGetResult() {
			ShowDialog();
			taskCompletionSource.SetResult(DataContext.DialogResult);
		}
}

	public static async Task<TDialogResult?> ShowDialog<TDialogResult>(
		string title,
		string body,
		IEnumerable<ContentDialogButtonItem> buttons,
		string iconName = ""
	) {
		if (!typeof(TDialogResult).IsNullable()) {
			string typeName = typeof(TDialogResult).Name;
			throw new TypeLoadException($"""The generic type "{typeName}" in method "{nameof(ContentDialog)}.{nameof(ShowDialog)}" is a value type, and it is not a nullable type. You have to replace the generic type from "{typeName}" to "{typeName}?".""");
		}

		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = title;
		viewModel.Body = body;
		viewModel.IconName = iconName;
		viewModel.Buttons.AddRange(buttons);
		return (TDialogResult?)await dialog.ShowDialogAsync();
	}

	internal static string errorFooter = "";
	public static void ShowError(
		string message,
		string stackTrace
	) {
		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = message;
		viewModel.Subtitle = t.ContentDialog.ShowError.Title;
		viewModel.Body = stackTrace;
		viewModel.IconName = "Error";
		viewModel.Buttons.AddRange([
			// new("Report", "report"), // I'm worried that users encounter any bug, they immediately click to report it directly.
			new(t.ContentDialog.Button.Close, "close"),
		]);
		viewModel.Expandable = true;
		viewModel.CanCopyBody = true;
		viewModel.Footer = errorFooter;
		dialog.ShowDialogAsync();
	}

	public static void ShowError(Exception exception) =>
		ShowError(exception.Message, exception.StackTrace);

	private bool isExpansionRunning = false;
	private void ExpandButton_Click(object sender, RoutedEventArgs e) {
		// When the user focuses on the expand button and then holds down the Enter key,
		// this will trigger the button at a high frequency, causing animation abnormalities.
		if (isExpansionRunning) return;
		isExpansionRunning = true;
		SetIsVerticalScrollBarShown(false);

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
				isExpansionRunning = false;
				SetIsVerticalScrollBarShown(true);
			};
			storyboard.Begin(this);
		}

		void SetIsVerticalScrollBarShown(bool shown) =>
			ScrollViewer.SetVerticalScrollBarVisibility(CopyableBody, shown ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);
	}
}
