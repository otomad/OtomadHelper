using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	public partial class VisualTruncateOptionsForm : Form {
		public VisualTruncateOptionsForm() {
			InitializeComponent();
		}

		private void IdleEffect_CheckedChanged(object sender, EventArgs e) {
			MonochromeTrack.Enabled = MonochromeCheck.Checked;
			FadeTrack.Enabled = FadeCheck.Checked;
			NegativeCombo.Enabled = NegativeCheck.Checked;
		}

		private void Option_CheckedChanged(object sender, EventArgs e) {
			LoopRegionTable.Enabled = PingPongWithoutSplitRadio.Checked || PingPongWithSplitRadio.Checked || LoopWithSplitRadio.Checked;
			IdleEffectGroup.Enabled = SplitThenFreezeRadio.Checked || SplitThenResumeRadio.Checked || PingPongWithSplitRadio.Checked || LoopWithSplitRadio.Checked;
		}

		private void VisualTruncateOptionsForm_Shown(object sender, EventArgs e) {
			this.ReserveSystemMenuItems(SystemMenuItemType.MOVE | SystemMenuItemType.CLOSE);
		}
	}
}
