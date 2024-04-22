using System.Drawing;
using System.Windows;

using Grid = System.Windows.Controls.Grid;

namespace OtomadHelper.WPF.Controls;

/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	//protected new dynamic? DialogResult { get; set; }

	public ContentDialog() {
		InitializeComponent();
		//Title = title;
		//Body = body;
		/*Buttons = buttons;
		IconName = "Info";

		Type? dialogResultType = buttons.Cast<dynamic>().FirstOrDefault(button => button?.DialogResult is not null)?.DialogResult?.GetType();
		if (dialogResultType is not null) {
			if (dialogResultType == typeof(string)) DialogResult = "cancel";
			else if (dialogResultType == typeof(DialogResult)) DialogResult = System.Windows.Forms.DialogResult.Cancel;
			else if (dialogResultType == typeof(bool)) DialogResult = false;
		}*/
	}

	//protected ContentDialog(string title, string body, string iconName, IEnumerable buttons) : this(title, body, buttons) {
	//	//IconName = iconName;
	//}

	/*protected new dynamic? ShowDialog() {
		base.ShowDialog();
		return DialogResult;
	}*/

	/*public new string Title {
		get => TitleLbl.Text;
		set {
			TitleLbl.Text = value;
			base.Title = value;
		}
	}

	public string Body {
		get => BodyLbl.Text;
		set => BodyLbl.Text = value;
	}

	public new string Icon {
		get => IconLbl.Text;
		set => IconLbl.Text = value;
	}

	public string IconName {
		set {
			value = new VariableName(value).Pascal;

			if (!SegoeIconNames.TryGetValue(value, out string iconChar))
				iconChar = SegoeIconNames["Info"];
			Icon = iconChar;

			if (Properties.Resources.ResourceManager.GetObject(value) is not Icon iconImage)
				iconImage = Properties.Resources.Info;
			base.Icon = iconImage.ToImageSource();
		}
	}

	public static readonly Dictionary<string, string> SegoeIconNames = new() {
		{ "Info", "\ue946" },
		{ "Warning", "\ue7ba" },
		{ "Error", "\uea39" },
		{ "Question", "\ue9ce" },
		{ "Locale", "\ue774" },
	};

	private IEnumerable Buttons {
		set {
			ButtonGrid.Children.Clear();
			ButtonGrid.ColumnDefinitions.Clear();
			IEnumerable<dynamic> buttons = value.Cast<dynamic>();
			int count = buttons.Count();
			foreach ((dynamic button, int index) in buttons.WithIndex()) {
				ButtonGrid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
				if (index != count - 1)
					ButtonGrid.ColumnDefinitions.Add(new() { Width = new(8) });
				Button btn = new() {
					Content = button.Text,
					DialogResult = button.DialogResult,
					IsDefault = button.IsDefault,
				};
				btn.Click += Button_Click;
				ButtonGrid.Children.Add(btn);
				Grid.SetColumn(btn, index * 2);
			}
		}
	}*/

	private void Button_Click(object sender, RoutedEventArgs e) {
		//if (sender is not Button button) return;
		//DialogResult = button.DialogResult;
		//Close();
	}

	public static void ShowDialog<TDialogResult>(
		string title,
		string body,
		IEnumerable<ContentDialogButtonItem<TDialogResult>> buttons,
		string iconName = ""
	) {
		ContentDialogViewModel<TDialogResult> viewModel = new() {
			Title = title,
			Body = body,
			IconName = iconName,
		};
		viewModel.Buttons.AddRange(buttons);
		ContentDialog dialog = new() { DataContext = viewModel };
		dialog.ShowDialog();
	}
}

/*public class ContentDialog<R> : ContentDialog {
	public ContentDialog(string title, string body, IEnumerable<ContentDialogButtonItem<R>> buttons) : base(title, body, buttons) { }

	public ContentDialog(string title, string body, string iconName, IEnumerable<ContentDialogButtonItem<R>> buttons) : base(title, body, iconName, buttons) { }

	public new R DialogResult {
		get => (R)base.DialogResult!;
		set => base.DialogResult = value;
	}

	public new R ShowDialog() => (R)base.ShowDialog()!;
}

public class ContentDialogButtonItem<R> {
	public string Text;
	public R DialogResult;
	public bool IsDefault = false;

	public ContentDialogButtonItem(string text, R dialogResult, bool isDefault = false) {
		Text = text;
		DialogResult = dialogResult;
		IsDefault = isDefault;
	}
}*/
