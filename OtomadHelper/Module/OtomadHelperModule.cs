using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

public class OtomadHelperModule : ICustomCommandModule {
	public Vegas vegas = null!;
	private readonly CustomCommand customCommandModule =
		new(CommandCategory.View, DISPLAY_NAME); // 这将显示在“查看”菜单的“扩展”下。
	internal const string INTERNAL_NAME = "OtomadHelperInternal";
	internal const string DISPLAY_NAME = "Otomad Helper";
	internal const string ASSEMBLY_NAME = "OtomadHelper";
	// 注意：不能用 Assembly.GetEntryAssembly().GetName().Name，一用 Vegas 就会闪退。

	internal static string CustomModulePath =>
		Assembly.GetExecutingAssembly().Location;

	internal string VegasAppDataPath =>
		vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData);

	public void InitializeModule(Vegas myVegas) {
		vegas = myVegas;
		customCommandModule.MenuItemName = DISPLAY_NAME;
		customCommandModule.IconFile = SaveAndGetIconPath();
	}

	public ICollection GetCustomCommands() {
		customCommandModule.MenuPopup += HandlePICmdMenuPopup;
		customCommandModule.Invoked += HandlePICmdInvoked;
		CustomCommand[] cmds = new CustomCommand[] { customCommandModule };
		return cmds;
	}

	private void HandlePICmdMenuPopup(object sender, EventArgs args) {
		customCommandModule.Checked = vegas.FindDockView(INTERNAL_NAME);
	}

	private void HandlePICmdInvoked(object sender, EventArgs args) {
		if (!vegas.ActivateDockView(INTERNAL_NAME)) {
			OtomadHelperDock dock = new() {
				AutoLoadCommand = customCommandModule,
				PersistDockWindowState = true,
			};
			vegas.LoadDockView(dock);
		}
	}

	private string SaveAndGetIconPath() {
		string localIconPath = Path.r(VegasAppDataPath, "Otomad Helper.png");
		ResourceHelper.WriteResourceToFile("Assets.ToolbarIcon.png", localIconPath);
		return localIconPath;
	}
}
