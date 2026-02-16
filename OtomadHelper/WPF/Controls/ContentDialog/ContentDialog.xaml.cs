using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public ContentDialog() : base() => InitializeComponent();

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
		string iconName = "None"
	) {
		ValidateDialogResultType<TDialogResult>();
		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = title;
		viewModel.Body = body;
		viewModel.IconName = iconName;
		viewModel.Buttons.AddRange(buttons);
		return (TDialogResult?)await dialog.ShowDialogAsync();
	}

	protected static string GetIconName(KnownIcon icon) => Enum.GetName<KnownIcon>(icon);

	public static async Task<TDialogResult?> ShowDialog<TDialogResult>(
		string title,
		string body,
		IEnumerable<ContentDialogButtonItem> buttons,
		KnownIcon icon /*= KnownIcon.Info */
	) => await ShowDialog<TDialogResult>(title, body, buttons, GetIconName(icon));

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
		viewModel.Expandable = true;
		viewModel.CanCopyBody = true;
		viewModel.Footer = errorFooter;
		Services.ITimer.WPF? timer = null;
		viewModel.Buttons.AddRange([
			// new("Report", "report"), // I'm worried that users encounter any bug, they immediately click to report it directly.
			new(t.ContentDialog.Button.CopyMessage, "copy", click: (sender, e) => {
				if (sender is not Button button) return;
				dialog.CopyErrorMessage(message, stackTrace);
				button.Content = t.ContentDialog.Button.Copied;
				timer?.Stop();
				timer = new(() => {
					timer = null;
					button.Content = t.ContentDialog.Button.CopyMessage;
				}, 1000);
				timer.SingleShot();
			}),
			new(t.ContentDialog.Button.Close, "close"),
		]);
		dialog.ShowDialogAsync();
	}

	public static async Task<TDialogResult?> ShowDialog<TDialogResult>(
		string title,
		FrameworkElement content,
		IEnumerable<ContentDialogButtonItem> buttons,
		KnownIcon icon = KnownIcon.None,
		bool? topmost = null,
		string? singletonId = null,
		ShowDialogEventHandler? customize = null
	) {
		ValidateDialogResultType<TDialogResult>();
		ContentDialog dialog = new();
		ContentDialogViewModel viewModel = dialog.DataContext;
		viewModel.Title = title;
		viewModel.Content = content;
		viewModel.IconName = GetIconName(icon);
		viewModel.Buttons.AddRange(buttons);
		dialog.Width = content.Width;
		dialog.SizeToContent = SizeToContent.WidthAndHeight;
		if (topmost is not null) dialog.Topmost = topmost.Value;
		if (!string.IsNullOrEmpty(singletonId)) {
			if (singletons.TryGetValue(singletonId!, out ContentDialog openedDialog)) {
				//openedDialog.Vanish();
				return default;
			}
			singletons.Add(singletonId!, dialog);
			dialog.Closed += (_, _) => singletons.Remove(singletonId!);
		}
		customize?.Invoke(dialog);
		return (TDialogResult?)await dialog.ShowDialogAsync();
	}

	private static readonly Dictionary<string, ContentDialog> singletons = [];

	public delegate void ShowDialogEventHandler(ContentDialog contentDialog);

	private static void ValidateDialogResultType<TDialogResult>() {
		if (!typeof(TDialogResult).IsNullable) {
			string typeName = typeof(TDialogResult).Name;
			throw new TypeLoadException($"""The generic type "{typeName}" in method "{nameof(ContentDialog)}.{nameof(ShowDialog)}" is a value type, and it is not a nullable type. You have to replace the generic type from "{typeName}" to "{typeName}?".""");
		}
	}

	public static void ShowError(Exception exception) =>
		ShowError(exception.Message, exception.StackTrace);

	private void CopyErrorMessage(string message, string stackTrace) {
		StringBuilder text = new();
		text.Append(t.Shared.Exceptions.ErrorHeader);
		text.AppendLine(message);
		text.AppendLine(stackTrace);
		if (!string.IsNullOrEmpty(errorFooter)) {
			text.AppendLine('-'.Repeat(50));
			text.AppendLine(errorFooter);
		}
		Clipboard.SetText(text.ToString().TrimEnd());
	}

	internal void SetNonDefaultButtonAccent(Color color) {
		for (int i = 0; i < ButtonsContainer.Items.Count; i++) {
			ContentPresenter presenter = (ContentPresenter)ButtonsContainer.ItemContainerGenerator.ContainerFromIndex(i);
			if (presenter.ContentTemplate.FindName("Button", presenter) is not Button button) continue;
			button.MixCheckerBoard = true;
			if (!button.IsDefault) button.Accent = color;
		}
	}

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
			storyboard.Completed += (_, _) => {
				SizeToContent = sizeToContent;
				isExpansionRunning = false;
				SetIsVerticalScrollBarShown(true);
			};
			storyboard.Begin(this);
		}

		void SetIsVerticalScrollBarShown(bool shown) =>
			ScrollViewer.SetVerticalScrollBarVisibility(CopyableBody, shown ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);
	}

	protected virtual void View_Showing(object sender, RoutedEventArgs e) =>
		Controls.Icon.PlaySound(ViewModel.IconName);
}
