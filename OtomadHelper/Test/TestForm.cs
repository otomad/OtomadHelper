using System.Windows.Forms;
using OtomadHelper.Module;

namespace OtomadHelper.Test;

public partial class TestForm : Form {
	private readonly MainDock mainDock;

	public TestForm() {
		InitializeComponent();
		mainDock = new MainDock();
		Controls.Add(mainDock);
		Icon = Properties.Resources.OtomadHelper;
		mainDock.DocumentTitleChanged += title => Text = title;

		//var window = new MainWindow();
		//window.Show();
	}
}
