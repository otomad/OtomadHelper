using System;
using ScriptPortal.Vegas;
using OtomadHelper.Module;
using System.Collections;

//namespace OtomadHelper.Module {
	public class OtomadHelperModule : ICustomCommandModule {
		public Vegas vegas = null;
		private readonly CustomCommand customCommandModule =
			new CustomCommand(CommandCategory.View, DISPLAY_NAME); // 这将显示在“查看”菜单的扩展下。
		internal const string INTERNAL_NAME = "OtomadHelperInternal";
		internal const string DISPLAY_NAME = "Otomad Helper";

		public void InitializeModule(Vegas myVegas) {
			vegas = myVegas;
			customCommandModule.MenuItemName = DISPLAY_NAME;
			customCommandModule.IconFile = @"C:\ProgramData\VEGAS Pro\20.0\Application Extensions\Otomad Helper.png";
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
					PersistDockWindowState = true
				};
				vegas.LoadDockView(dock);
			}
		}
	}
//}
