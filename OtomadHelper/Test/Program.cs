using System.Windows.Forms;

namespace OtomadHelper.Test;

internal static class Program {
	/// <summary>
	/// 应用程序的主入口点。
	/// </summary>
	[STAThread]
	static void Main() {
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		//Application.Run(new TestForm());
		//Application.Run(new TestControls());
		//new WPF.Controls.ContentDialog<string>("Title", "Body", new WPF.Controls.ContentDialogButtonItem<string>[] { new WPF.Controls.ContentDialogButtonItem<string>("OK", "ok", true) }).ShowDialog();
		WPF.Controls.ContentDialog.ShowDialog("幸福倒计时", "Windows 11 即将更新！", new WPF.Controls.ContentDialogButtonItem<string>[] { new WPF.Controls.ContentDialogButtonItem<string>("OK", "ok", true), new WPF.Controls.ContentDialogButtonItem<string>("Cancel", "cancel") });
	}
}
