using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

/// <summary>
/// This class is only called if the user accidentally puts this extension into the scripts directory.
/// </summary>
public class EntryPoint {
	internal Vegas vegas = null!;

	public void FromVegas(Vegas myVegas) {
		vegas = myVegas;

		WPF.Controls.ContentDialog.ShowDialog<string>(
			"嘿，你把该扩展程序放错地方了！",
			"新版 Otomad Helper 是一个扩展程序，与旧版不同，它不是一个脚本。\n\n请将该扩展程序移动到 VEGAS 的扩展程序目录，而不是脚本目录。\n\n位置：\nC:\\ProgramData\\VEGAS Pro\\Application Extensions",
			new WPF.Controls.ContentDialogButtonItem[] {
				new(t.ContentDialog.Button.Ok, "ok"),
				new("了解更多", "learnMore", true),
			}
		);
	}
}
