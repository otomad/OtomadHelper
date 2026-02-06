#define TEST6
using System.Windows.Forms;

using OtomadHelper.WPF.Controls;

namespace OtomadHelper.Test;

internal static class Program {
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	public static void Main() {
		//CosturaUtility.Initialize();
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Prior.Initialize();
		//AppContext.SetSwitch("Switch.System.Windows.Controls.Text.UseAdornerForTextboxSelectionRendering", false);
		//SetCulture = "en-US";
#if TEST0
		Application.Run(new AppDebugForm());
#endif

#if TEST1
		Application.Run(new TestControlsWinForm());
#endif
#if TEST2
		s = ContentDialog.ShowDialog<DialogResult?>("幸福倒计时", "即将更新 Windows 11 到最新版本！", [
			new("草", DialogResult.Abort),
			new("走", DialogResult.Retry),
			new("忽略", DialogResult.Ignore, true),
		]);
#endif
#if TEST3
		try {
			ContentDialog.errorFooter = "VEGAS Pro: v21.0\nOtomad Helper: v8.0.0";
			TestError();
		} catch (Exception e) {
			ContentDialog.ShowError(e);
		}
		static int TestError() => 0 / Math.Abs(0);
#endif
#if TEST4
		new TestControlsWPF().ShowDialog();
#endif
#if TEST5
		_ = ColorPicker.ShowDialog("f00", new(Wacton.Unicolour.ColourSpace.Hsb, 2)).Then(color => s = color);
#endif
#if TEST6
		_ = QuickSelectInterval1DEditor.ShowDialog([true, false, true, false]).Then(r => s = (r.bits.Select(i => i.ToString()).Join(","), QsiCodec.EncodeQsiProtocol(r.bits)));
		//_ = QuickSelectInterval1DEditor.ShowDialog("QSI1:ABo").Then(r => s = r.base64);
#endif
	}
}
