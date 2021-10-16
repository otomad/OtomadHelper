using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VegasScript {
	public partial class ReplaceClipsForm : Form {
		public ReplaceClipsForm() {
			InitializeComponent();
			Icon = ConfigForm.icon;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ReplacedBtn_Click(object sender, EventArgs e) {
			ReplacedLbl.Text = "";
		}

		private void ReplacerBtn_Click(object sender, EventArgs e) {
			ReplacerLbl.Text = "";
		}

		private void ReplacerCombo_SelectedIndexChanged(object sender, EventArgs e) {
			ReplacerLbl.Text = "";
		}
	}
}
