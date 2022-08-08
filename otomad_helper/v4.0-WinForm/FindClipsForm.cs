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
	public partial class FindClipsForm : Form {
		public FindClipsForm() {
			InitializeComponent();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void SetEnabled(object sender, EventArgs e) {
			bool isMatchName = MatchNameRadio.Checked;
			ClipNameTxt.Enabled = ClipNameList.Enabled = isMatchName;
		}

		private void AutoSelectMatchName(object sender, EventArgs e) {
			MatchNameRadio.Checked = true;
		}

		private void ClipNameTxt_TextChanged(object sender, EventArgs e) {
			AutoSelectMatchName(null, null);
		}
	}
}
