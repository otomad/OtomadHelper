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
	public partial class SelectIntervalForm : Form {
		public SelectIntervalForm() {
			InitializeComponent();
			foreach (Control control in table.Controls)
				if (control is NumericUpDown)
					control.MouseWheel += AutoLayoutTracksGridForm.NumericUpDown_MouseWheel;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ApplyBtn_Click(object sender, EventArgs e) {
			Show();
		}

		private void SelectOneEveryFewBox_ValueChanged(object sender, EventArgs e) {
			SelectWhichEachGroupBox.Maximum = SelectOneEveryFewBox.Value;
		}

		private void SubmitSelectBtn_Click(object sender, EventArgs e) {
			SelectInfo.Text = "";
		}

		private void ResetBtn_Click(object sender, EventArgs e) {
			SelectInfo.Text = "";
		}
	}
}
