using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OtomadHelper.UI.Pages;
using System;
using System.Collections.ObjectModel;
using WinUICommunity.Common.ViewModel;

namespace OtomadHelper.UI;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ShellPage : Page {
	public static ShellPage Instance { get; private set; }
	public ShellViewModel ViewModel { get; } = new ShellViewModel();

	public ShellPage() {
		InitializeComponent();
		Instance = this;
		ViewModel.InitializeNavigation(NavigationFrame, NavigationView)
			.WithKeyboardAccelerator(KeyboardAccelerators)
			.WithDefaultPage(typeof(HomePage))
			.WithSettingsPage(typeof(SettingsPage));
		ViewModel.PropertyChanged += (sender, e) => SetHeader();
	}

	public string AppTitle { get { return "Otomad Helper - Config"; } } // Windows.ApplicationModel.Package.Current.DisplayName

	public void Navigate(string uniqeId) {
		Type pageType = Type.GetType(uniqeId);
		NavigationFrame.Navigate(pageType);
	}

	private void Page_Loaded(object sender, RoutedEventArgs e) {
		ViewModel.OnLoaded();
	}

	private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args) {
		ViewModel.OnItemInvoked(args);
	}

	private void OkBtn_Click(object sender, RoutedEventArgs e) {
		App.MainWindow.Close();
	}

	private void CancelBtn_Click(object sender, RoutedEventArgs e) {
		App.MainWindow.Close();
	}

	private readonly ObservableCollection<object> Breadcrumbs = new();
	private void SetHeader(params string[] header) {
		Breadcrumbs.Clear();
		foreach (string item in header)
			Breadcrumbs.Add(new Crumb(item, null));
	}

	private void SetHeader() {
		SetHeader(ViewModel.Selected.Content as string);
	}

	public readonly struct Crumb {
		public Crumb(string label, object data) {
			Label = label;
			Data = data;
		}
		public string Label { get; }
		public object Data { get; }
		public override string ToString() => Label;
	}
}
