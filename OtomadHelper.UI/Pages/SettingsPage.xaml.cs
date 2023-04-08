using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;

namespace OtomadHelper.UI.Pages;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page {
	public SettingsPage() {
		InitializeComponent();
		App.Current.themeManager.ActualThemeChanged += (sender, e) => {
			ThemeRadioButtons_SelectionChanged(null, null);
			BackdropRadioButtons_SelectionChanged(null, null);
		};
	}

	private static BackdropType selectedBackdrop = BackdropType.Acrylic;
	private int SelectedBackdropIndex { get => (int)selectedBackdrop; }
	private void BackdropRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		BackdropType selected = (BackdropType)BackdropRadioButtons.SelectedIndex;
		if (selectedBackdrop == selected) return;
		selectedBackdrop = selected;
		App.Current.themeManager.SetSystemBackdrop(selected.ToWinUICommunityType());
		SetSolidBackground();
	}

	private static ElementTheme selectedTheme = ElementTheme.Default;
	private int SelectedThemeIndex { get => (int)selectedTheme; }
	private void ThemeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e) {
		ElementTheme selected = (ElementTheme)ThemeRadioButtons.SelectedIndex;
		if (selectedTheme == selected) return;
		selectedTheme = selected;
		App.Current.themeManager.ChangeTheme(selected);
		SetSolidBackground();
	}

	private void SetSolidBackground() {
		if (App.MainWindow.Content is ShellPage page) {
			bool isPictureBg = selectedBackdrop is BackdropType.Rem or BackdropType.Custom;
			bool isDark = App.Current.themeManager.IsDarkTheme();
			page.Background = new SolidColorBrush(
				!(selectedBackdrop == BackdropType.None || isPictureBg) ? Colors.Transparent :
				isDark ? Colors.Black : Colors.White
			);
			page.PageGrid.Background = isPictureBg ? new ImageBrush {
				ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Rem.png", UriKind.RelativeOrAbsolute)),
				Stretch = Stretch.UniformToFill,
				Opacity = isDark ? 1 : 0.5,
			} : new SolidColorBrush(Colors.Transparent);
		}
	}
}
