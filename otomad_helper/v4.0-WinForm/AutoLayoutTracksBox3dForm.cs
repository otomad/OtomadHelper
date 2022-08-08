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
	public partial class AutoLayoutTracksBox3dForm : Form {
		public AutoLayoutTracksBox3dForm() {
			InitializeComponent();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void Combo_SelectedIndexChanged(object sender, EventArgs e) {
			if (!(sender is ComboBox)) return;
			ComboBox combo = sender as ComboBox;
			int selected = combo.SelectedIndex;
			if (selected == 0 || selected == -1) return;
			foreach (Control control in table.Controls) {
				if (control is ComboBox) {
					ComboBox comboBox = control as ComboBox;
					if (comboBox == combo) continue;
					if (comboBox.SelectedIndex == selected) comboBox.SelectedIndex = 0;
				}
			}
		}
	}
}
