using System.Windows.Forms;

namespace OtomadHelper.Test;

internal static class Program {
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new TestForm());
		//Application.Run(new TestControls());
		/*s = WPF.Controls.ContentDialog.ShowDialog<string>("幸福倒计时", "Windows 11 即将更新！", new WPF.Controls.ContentDialogButtonItem[] { new("OK", "ok", true), new("Cancel", "cancel") });
		try {
			TestError();
		} catch (Exception e) {
			WPF.Controls.ContentDialog.ShowError(e);
		}
		static int TestError() => 0 / Math.Abs(0);*/
	}
}
