using Microsoft.UI.Xaml;

namespace OtomadHelper.UI;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window {
	internal static MainWindow Instance { get; private set; }

	public MainWindow() {
		InitializeComponent();
		Instance = this;
	}
}
