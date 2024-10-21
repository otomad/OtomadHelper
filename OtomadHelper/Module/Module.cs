/// <remarks>
/// `const` is a "nice" way to shoot yourself in the foot.
/// </remarks>
/// <see href="https://stackoverflow.com/a/756010/19553213" />
/// <see href="https://www.stum.de/2009/01/14/const-strings-a-very-convenient-way-to-shoot-yourself-in-the-foot/" />

using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

/// <summary>
/// Defines initialization routines for hosting a set of custom commands.
/// </summary>
public class Module : ICustomCommandModule {
	public Vegas vegas = null!;
	internal static readonly CommandCategory COMMAND_CATEGORY = CommandCategory.View; // NOTE: DO NOT USE `const`! Or ILRepack won't bundle it.
	private readonly CustomCommand customCommandModule = new(COMMAND_CATEGORY, DisplayName); // This will show in menu: View → Extensions
	internal const string InternalName = "OtomadHelperInternal";
	internal const string DisplayName = "Otomad Helper";
	private static string AssemblyName => ResourceHelper.AssemblyName; // Only available in Vegas environment, so private.
	internal Keybindings Keybindings { get; } = new();

	internal static string CustomModulePath => Assembly.GetExecutingAssembly().Location;
	internal string VegasAppDataPath => vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData);

	/// <summary>
	/// Initialize the module which hosts a set of custom commands.
	/// </summary>
	/// <param name="myVegas">The Vegas application object.</param>
	public void InitializeModule(Vegas myVegas) {
		vegas = myVegas;
		customCommandModule.MenuItemName = DisplayName;
		customCommandModule.IconFile = SaveAndGetIconPath();
		Keybindings.Initialize();
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
	}

	/// <summary>
	/// Get the collection of custom command objects hosted by this module.
	/// </summary>
	/// <returns>The collection of custom command objects hosted by this module.</returns>
	public ICollection GetCustomCommands() {
		customCommandModule.MenuPopup += HandlePICmdMenuPopup;
		customCommandModule.Invoked += HandlePICmdInvoked;
		CustomCommand[] commands = [customCommandModule, Keybindings.Parent, .. Keybindings.Commands];
		return commands;
	}

	/// <summary>
	/// Occurs just before the command's menu item appears.
	/// </summary>
	private void HandlePICmdMenuPopup(object sender, EventArgs args) {
		customCommandModule.Checked = vegas.FindDockView(InternalName);
	}

	/// <summary>
	/// Occurs when the command is invoked.
	/// </summary>
	private void HandlePICmdInvoked(object sender, EventArgs args) {
		if (!vegas.ActivateDockView(InternalName)) {
			Dockable dock = new(this) {
				AutoLoadCommand = customCommandModule,
				PersistDockWindowState = true,
			};
			vegas.LoadDockView(dock);
		}
	}

	/// <summary>
	/// Extracts a small icon file stream from the project's embedded resources
	/// to Vegas application data directory, then returns the extracted file path.
	/// </summary>
	/// <remarks>
	/// Since the icon of a custom command must be specified as a local file path and not a stream,
	/// this method is used as a workaround.
	/// </remarks>
	/// <returns>The exteacted small icon file path.</returns>
	private string SaveAndGetIconPath() {
		string localIconPath = new Path(VegasAppDataPath, "Otomad Helper.png");
		ResourceHelper.WriteResourceToFile("Assets.ToolbarIcon.png", localIconPath);
		return localIconPath;
	}

	/// <summary>
	/// Applies when resolving an external DLL fails.
	/// </summary>
	private Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
		lock (this) {
			AssemblyName askedAssembly = new(args.Name);
			string name = askedAssembly.Name;
			// If a resource file is requested in the form "dllname.resources.dll" always return null;
			// https://stackoverflow.com/questions/4368201
			// if (name.EndsWith(".resources")) return null;
			return name == AssemblyName ? Assembly.GetExecutingAssembly() : null;
		}
	}
}
