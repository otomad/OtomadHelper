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
		//Application.Run(new TestForm());
		//Application.Run(new TestControls());
		s = WPF.Controls.ContentDialog.ShowDialog<string>("幸福倒计时", "Windows 11 即将更新！", new WPF.Controls.ContentDialogButtonItem[] { new("OK", "ok", true), new("Cancel", "cancel") });
		WPF.Controls.ContentDialog.ShowError("Aira is not defined", "uihewny8eyfh4ehf8734yf8\ngfdgf876y\nhf8ywefew7\nfg3476rt34tryrty6874g8");
	}
}
