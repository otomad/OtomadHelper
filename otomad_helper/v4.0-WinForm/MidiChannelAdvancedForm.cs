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
	public partial class MidiChannelAdvancedForm : Form {
		public MidiChannelAdvancedForm(ConfigForm configForm) {
			InitializeComponent();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CommingSoon() {
			MessageBox.Show("Comming soon in v5.x!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void EditNotesBtn_Click(object sender, EventArgs e) {
			CommingSoon();
		}

		private void AutoLayoutTracksBox3dBtn_Click(object sender, EventArgs e) {
			CommingSoon();
		}

		private void AutoLayoutTracksGridBtn_Click(object sender, EventArgs e) {
			new AutoLayoutTracksGridForm().ShowDialog();
		}

		private void GradientTracksBtn_Click(object sender, EventArgs e) {
			new GradientTracksForm().ShowDialog();
		}

		private void ResetAutoLayoutTracksBtn_Click(object sender, EventArgs e) {
			Refresh();
		}

		private void ChannelListView_ItemChecked(object sender, ItemCheckedEventArgs e) {
			Console.WriteLine("王手");
		}

		private void ChannelListView_SelectedIndexChanged(object sender, EventArgs e) {
			EditNotesBtn.Enabled = ChannelListView.SelectedItems.Count == 1;
		}

		private void SelectAllBtn_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in ChannelListView.Items)
				item.Selected = true;
		}

		private void InvertSelectionButton_Click(object sender, EventArgs e) {
			foreach (ListViewItem item in ChannelListView.Items)
				item.Selected = !item.Selected;
		}
	}
}
