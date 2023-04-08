using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;

namespace OtomadHelper.UI.Pages; 
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class HomePage : Page {
	public HomePage() {
		InitializeComponent();
	}

	private string Received { get => App.MainWindow.recieved; }

	private void SendBtn_Click(object sender, RoutedEventArgs e) {
		string text = SendTxt.Text;
		/*await new ContentDialog {
			Title = "Message",
			Content = text,
			CloseButtonText = "OK",
			XamlRoot = XamlRoot,
		}.ShowAsync();*/
		App.Current.client.send = text;
	}
}
