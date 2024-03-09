using System.Drawing;
using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

public class OtomadHelperDock : DockableControl {
	private MainDock? mainDock;
	internal Vegas vegas => myVegas;

	public OtomadHelperDock() : base(OtomadHelperModule.INTERNAL_NAME) => DisplayName = OtomadHelperModule.DISPLAY_NAME;

	public override DockWindowStyle DefaultDockWindowStyle => DockWindowStyle.Docked;

	public override Size DefaultFloatingSize => new(800, 480);

	protected override void OnLoad(EventArgs args) {
		if (mainDock != null) Controls.Remove(mainDock);
		mainDock = new();
		Controls.Add(mainDock);

		//vegas.TrackEventStateChanged += HandleTrackEventChanged;
		//vegas.TrackEventCountChanged += HandleTrackEventChanged;
	}

	private void HandleTrackEventChanged(object sender, EventArgs e) {
		//myForm.SelectBtn_Click(null, null);
	}

	/*protected override void OnClosed(EventArgs args) {
		base.OnClosed(args);
	}

	protected override void WndProc(ref Message m) {
		bool handled = false;
		windowHelper.WndProc(m.HWnd, m.Msg, m.WParam, m.LParam, ref handled);
		if (!handled) base.WndProc(ref m);
	}*/
}
