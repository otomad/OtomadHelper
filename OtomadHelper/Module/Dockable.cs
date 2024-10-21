using System.Drawing;
using ScriptPortal.Vegas;

namespace OtomadHelper.Module;

[DesignerCategory("Code")]
public class Dockable : DockableControl {
	private Host? host;
	internal Vegas Vegas => myVegas;
	internal Module Module { get; }

	public Dockable(Module module) : base(Module.InternalName) {
		DisplayName = Module.DisplayName;
		Module = module;
	}

	public override DockWindowStyle DefaultDockWindowStyle => DockWindowStyle.Docked;
	public override Size DefaultFloatingSize => new(800, 480);
	public bool Shown { get; private set; } = false;

	public void Reload() {
		DisposeHost();
		host = new(this);
		Controls.Add(host);
	}

	protected override void OnLoad(EventArgs args) {
		Reload();
		Shown = true;

		//Vegas.TrackEventStateChanged += HandleTrackEventChanged;
		//Vegas.TrackEventCountChanged += HandleTrackEventChanged;
	}

	protected void DisposeHost() {
		if (host is not null) {
			Controls.Remove(host);
			host.Dispose();
			host = null;
		}
	}

	protected override void OnClosed(EventArgs args) {
		DisposeHost();
		Shown = false;
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
