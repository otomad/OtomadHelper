using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

public class OtomadHelperModule : ICustomCommandModule {
	public Vegas vegas = null!;
	private readonly CustomCommand customCommandModule =
		new(CommandCategory.View, DisplayName); // This will show in menu: View → Extensions
	internal const string InternalName = "OtomadHelperInternal";
	internal const string DisplayName = "Otomad Helper";
	private static string AssemblyName => ResourceHelper.AssemblyName; // Only available in Vegas environment, so private.

	internal static string CustomModulePath => Assembly.GetExecutingAssembly().Location;
	internal string VegasAppDataPath => vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData);

	public void InitializeModule(Vegas myVegas) {
		vegas = myVegas;
		customCommandModule.MenuItemName = DisplayName;
		customCommandModule.IconFile = SaveAndGetIconPath();
		ReleaseProbing();
	}

	public ICollection GetCustomCommands() {
		customCommandModule.MenuPopup += HandlePICmdMenuPopup;
		customCommandModule.Invoked += HandlePICmdInvoked;
		CustomCommand[] commands = new CustomCommand[] { customCommandModule };
		return commands;
	}

	private void HandlePICmdMenuPopup(object sender, EventArgs args) {
		customCommandModule.Checked = vegas.FindDockView(InternalName);
	}

	private void HandlePICmdInvoked(object sender, EventArgs args) {
		if (!vegas.ActivateDockView(InternalName)) {
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

	private void ReleaseProbing() {
		string assemblyDllName = AssemblyName + ".dll";
		string vegasDir = vegas.InstallationDirectory;
		Path probingFile = Path.r(vegasDir, assemblyDllName);
		File.Delete(probingFile);
		RunCmdBackground($"mklink \"{probingFile}\" \"{CustomModulePath}\"");
	}
}
