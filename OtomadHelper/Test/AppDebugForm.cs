using System.Windows.Forms;

using OtomadHelper.Module;

namespace OtomadHelper.Test;

public partial class AppDebugForm : Form {
	private readonly Host host;

	public AppDebugForm() {
		InitializeComponent();
#if !VEGAS_ENV
		host = new Host();
#else
		host = new Host(null!);
#endif
		Controls.Add(host);
		host.DocumentTitleChanged += title => Text = title;

		Icon = Properties.Resources.OtomadHelper;
	}
}
