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
	public partial class BatchSubtitleGenerationForm : Form {
		public BatchSubtitleGenerationForm() {
			InitializeComponent();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void SingleDurationTxt_Leave(object sender, EventArgs e) {
			SingleDurationTxt.Text = "0";
		}
	}
}
