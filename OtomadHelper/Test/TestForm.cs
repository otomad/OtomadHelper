using System.Windows.Forms;
using System.Windows.Forms.Integration;

using OtomadHelper.Module;

namespace OtomadHelper.Test;

public partial class TestForm : Form {
	private readonly MainDock mainDock;
	//private readonly MainWPFDock mainWpfDock;
	//private readonly ElementHost elementHost;

	public TestForm() {
		InitializeComponent();
		mainDock = new MainDock();
		Controls.Add(mainDock);
		mainDock.DocumentTitleChanged += title => Text = title;
		//mainWpfDock = new();
		//elementHost = new() { Dock = DockStyle.Fill, Child = mainWpfDock };
		//Controls.Add(elementHost);
		//mainWpfDock.DocumentTitleChanged += title => Text = title;

		Icon = Properties.Resources.OtomadHelper;

		//var window = new MainWindow();
		//window.Show();
	}
}
