using System.Windows;
using System.Windows.Controls;

namespace OtomadHelper.WPF.Controls;
/// <summary>
/// ContentDialog.xaml 的交互逻辑
/// </summary>
public partial class ContentDialog : BackdropWindow {
	public new DialogResult DialogResult { get; set; } = DialogResult.Cancel;

	public ContentDialog(string title, string body, IEnumerable<ContentDialogButtonItem> buttons) {
		InitializeComponent();
		Title = title;
		Body = body;
		Buttons = buttons;
	}

	public new DialogResult ShowDialog() {
		base.ShowDialog();
		return DialogResult;
	}

	public new string Title {
		get => TitleLbl.Text;
		set => TitleLbl.Text = value;
	}

	public string Body {
		get => BodyLbl.Text;
		set => BodyLbl.Text = value;
	}

	private IEnumerable<ContentDialogButtonItem> Buttons {
		set {
			ButtonGrid.Children.Clear();
			ButtonGrid.ColumnDefinitions.Clear();
			int count = value.Count();
			foreach ((ContentDialogButtonItem button, int index) in value.WithIndex()) {
				ButtonGrid.ColumnDefinitions.Add(new() { Width = new(1, GridUnitType.Star) });
				if (index != count - 1)
					ButtonGrid.ColumnDefinitions.Add(new() { Width = new(8) });
				Button btn = new() {
					Text = button.Text,
					DialogResult = button.DialogResult,
					IsDefault = button.IsDefault,
				};
				btn.Click += Button_Click;
				ButtonGrid.Children.Add(btn);
				Grid.SetColumn(btn, index * 2);
			}
		}
	}

	private void Button_Click(object sender, RoutedEventArgs e) {
		if (sender is not Button button) return;
		DialogResult = button.DialogResult;
		Close();
	}
}

public struct ContentDialogButtonItem {
	public string Text;
	public DialogResult DialogResult;
	public bool IsDefault;

	public ContentDialogButtonItem(string text, DialogResult dialogResult = DialogResult.Cancel, bool isDefault = false) {
		Text = text;
		DialogResult = dialogResult;
		IsDefault = isDefault;
	}
}
