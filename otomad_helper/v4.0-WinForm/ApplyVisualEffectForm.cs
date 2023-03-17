using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class ApplyVisualEffectForm : Form {
		public ApplyVisualEffectForm() {
			InitializeComponent();
		}

		private void VideoEffectCombo_SelectedIndexChanged(object sender, EventArgs e) {
			_ = VideoEffectCombo.SelectedIndex;
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			// apply
		}
	}
}
