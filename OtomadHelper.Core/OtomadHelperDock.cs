using ScriptPortal.Vegas;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace OtomadHelper.Core {
	public class OtomadHelperDock : DockableControl {
		private MainWindow myForm = null;
		private readonly ElementHost elementHost = new ElementHost { Dock = DockStyle.Fill };
		private Vegas vegas { get { return myVegas; } }

		public OtomadHelperDock() : base(OtomadHelperModule.INTERNAL_NAME) {
			DisplayName = OtomadHelperModule.DISPLAY_NAME;
		}

		public override DockWindowStyle DefaultDockWindowStyle {
			get { return DockWindowStyle.Docked; }
		}

		public override Size DefaultFloatingSize {
			get { return new Size(800, 480); }
		}

		protected override void OnLoad(EventArgs args) {
			myForm = new MainWindow(myVegas);
			elementHost.Child = myForm;
			Controls.Add(elementHost);

			vegas.TrackEventStateChanged += HandleTrackEventChanged;
			vegas.TrackEventCountChanged += HandleTrackEventChanged;
		}

		private void HandleTrackEventChanged(object sender, EventArgs e) {
			myForm.SelectBtn_Click(null, null);
		}

		protected override void OnClosed(EventArgs args) {
			base.OnClosed(args);
		}
	}
}
