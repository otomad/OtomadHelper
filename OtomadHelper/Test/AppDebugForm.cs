using System.Windows.Forms;

using OtomadHelper.Module;

namespace OtomadHelper.Test;

public partial class AppDebugForm : Form {
	private readonly Host host;

	public AppDebugForm() {
		InitializeComponent();
		host = new Host(null!);
		Controls.Add(host);
		host.DocumentTitleChanged += title => Text = title;

		Icon = Properties.Resources.OtomadHelper;
	}
}
