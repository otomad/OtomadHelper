using System;
using ScriptPortal.Vegas;
using OtomadHelper.Module;
using System.Collections;
using System.IO;
using System.Reflection;
using OtomadHelper.Helpers;

namespace OtomadHelper.Module {
	public class OtomadHelperModule : ICustomCommandModule {
		public Vegas vegas = null;
		private readonly CustomCommand customCommandModule =
			new CustomCommand(CommandCategory.View, DISPLAY_NAME); // 这将显示在“查看”菜单的扩展下。
		internal const string INTERNAL_NAME = "OtomadHelperInternal";
		internal const string DISPLAY_NAME = "Otomad Helper";

		internal static string CustomModulePath { get {
			return Assembly.GetExecutingAssembly().Location;
		} }
		internal string VegasAppDataPath { get {
			return vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData);
		} }

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
				OtomadHelperDock dock = new OtomadHelperDock {
					AutoLoadCommand = customCommandModule,
					PersistDockWindowState = true,
				};
				vegas.LoadDockView(dock);
			}
		}

		private string SaveAndGetIconPath() {
			string localIconPath = Path.Combine(VegasAppDataPath, "Otomad Helper.png");
			ResourceHelper.WriteResourceToFile("Assets.ToolbarIcon.png", localIconPath);
			return localIconPath;
		}
	}
}
