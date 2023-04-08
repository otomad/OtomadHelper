using Microsoft.UI.Xaml;
using System;

namespace OtomadHelper.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
	internal static MainWindow Instance { get; private set; }
	internal string Received {
		set {
			Pages.HomePage homePage = Pages.HomePage.Instance;
			if (homePage != null)
				homePage.Received = value;
		}
	}

	public MainWindow() {
		InitializeComponent();
		Instance = this;
	}

	private void Window_Closed(object sender, WindowEventArgs args) {
		
	}
}
