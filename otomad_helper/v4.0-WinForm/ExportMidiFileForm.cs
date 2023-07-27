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
	public partial class ExportMidiFileForm : Form {
		public ExportMidiFileForm() {
			InitializeComponent();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void List_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
			ListView list = sender as ListView;
		}

		private void AddToTrackBtn_Click(object sender, EventArgs e) {
			ToString();
		}

		private void ActionsClick(object sender, EventArgs e) {
			ToString();
		}

		private void List_SelectedIndexChanged(object sender, EventArgs e) {
			ToString();
		}

		private void MidiTrackNameTxt_TextChanged(object sender, EventArgs e) {
			ToString();
		}

		private void ChannelValueCombo_SelectedIndexChanged(object sender, EventArgs e) {
			ToString();
		}

		private void PreviewTrackBtn_Click(object sender, EventArgs e) {
			ToString();
		}

		private void ExportMidiFileForm_FormClosing(object sender, FormClosingEventArgs e) {
			ToString();
		}

		private void MidiInstrumentList_SelectedIndexChanged(object sender, EventArgs e) {
			ToString();
		}

		private void InstrumentCombo_SelectedIndexChanged(object sender, EventArgs e) {
			ToString();
		}
	}
}
