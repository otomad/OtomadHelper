using OtomadHelper.Core.Helpers;
using ScriptPortal.Vegas;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace OtomadHelper.Core {
	public class OtomadHelperDock : DockableControl {
		private MainDock myForm = null;
		//private readonly ElementHost elementHost = new ElementHost { Dock = DockStyle.Fill };
		private TestMica testMica;
		internal Vegas vegas { get { return myVegas; } }
		internal readonly WindowHelper windowHelper;

		public OtomadHelperDock() : base(OtomadHelperModule.INTERNAL_NAME) {
			DisplayName = OtomadHelperModule.DISPLAY_NAME;
			windowHelper = new WindowHelper(this);
			windowHelper.Received += text => {
				if (myForm != null) myForm.Received = text;
			};
		}

		protected override void OnHandleCreated(EventArgs e) {
			WindowUtils.EnableAcrylic(this, Color.Transparent);
			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.Clear(Color.Transparent);
		}

		public override DockWindowStyle DefaultDockWindowStyle {
			get { return DockWindowStyle.Docked; }
		}

		public override Size DefaultFloatingSize {
			get { return new Size(800, 480); }
		}

		protected override void OnLoad(EventArgs args) {
			//myForm = new MainDock(this);
			//elementHost.Child = myForm;
			//Controls.Add(elementHost);
			testMica = new TestMica(Handle) { Dock = DockStyle.Fill };
			Controls.Add(testMica);


			//vegas.TrackEventStateChanged += HandleTrackEventChanged;
			//vegas.TrackEventCountChanged += HandleTrackEventChanged;
		}

		private void HandleTrackEventChanged(object sender, EventArgs e) {
			//myForm.SelectBtn_Click(null, null);
		}

		protected override void OnClosed(EventArgs args) {
			base.OnClosed(args);
		}

		protected override void WndProc(ref Message m) {
			bool handled = false;
			windowHelper.WndProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
			if (!handled) base.WndProc(ref m);
		}
	}
}
