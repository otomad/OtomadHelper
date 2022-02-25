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
	public partial class AutoLayoutTracksGridForm : Form {
		public AutoLayoutTracksGridForm() {
			InitializeComponent();
			CustomRadio_CheckedChanged(null, null);
			ColumnCountBox.MouseWheel += NumericUpDown_MouseWheel;
		}

		public static void NumericUpDown_MouseWheel(object sender, MouseEventArgs e) {
			//throw new NotImplementedException();
			//HandledMouseEventArgs hme = e as HandledMouseEventArgs;
			//if (hme != null) hme.Handled = true;
			NumericUpDown numeric = sender as NumericUpDown;
			if (e is HandledMouseEventArgs hme) hme.Handled = true;
			decimal increment = numeric.Increment;
			if (e.Delta > 0) {
				if (numeric.Value + increment > numeric.Maximum) numeric.Value = numeric.Maximum;
				else numeric.Value += increment;
			} else if (e.Delta < 0) {
				if (numeric.Value - increment < numeric.Minimum) numeric.Value = numeric.Minimum;
				else numeric.Value -= increment;
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ColumnCountBox_ValueChanged(object sender, EventArgs e) {
			RowCountBox.Value = ColumnCountBox.Value;
		}

		private void CustomRadio_CheckedChanged(object sender, EventArgs e) {
			bool isCustom = CustomRadio.Checked;
			CustomGroup.Enabled = isCustom;
		}
	}
}
