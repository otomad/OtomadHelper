using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScriptPortal.Vegas;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class QuickConfigPropertiesForm : Form {
		public QuickConfigPropertiesForm() {
			InitializeComponent();
			VideoResampleModeCombo.SelectedIndex = 0;
			RefreshDisabled(null, null);
			foreach (CheckBox check in this.GetChildrenOfType<CheckBox>()) {
				check.AutoCheck = false;
				check.Click += ChangeThreeStateCheckBoxStateChangeOrder;
			}
			bool supportsOpticalFlow = Enum.IsDefined(typeof(VideoResampleMode), VideoResampleMode_OpticalFlow);
			if (!supportsOpticalFlow)
				VideoResampleModeCombo.Items.RemoveAt(VideoResampleMode_OpticalFlow + 2);
		}

		private const int VideoResampleMode_OpticalFlow = 3; //= VideoResampleMode.OpticalFlow

		private void RefreshDisabled(object sender, EventArgs e) {
			VideoRateBox.Enabled = VideoSetRateRadio.Checked || VideoMultiplyRateRadio.Checked;
			AudioRateBox.Enabled = AudioSetRateRadio.Checked || AudioMultiplyRateRadio.Checked;
			VideoGainBox.Enabled = VideoSetGainRadio.Checked || VideoMultiplyGainRadio.Checked;
			AudioGainBox.Enabled = AudioSetGainRadio.Checked || AudioMultiplyGainRadio.Checked;
			VideoUnderSampleRateBox.Enabled = VideoUnderSampleRateCheck.Checked;
			AudioRecalcNormGainPanel.Enabled = AudioNormalizeCheck.Checked;
			if (VideoRateBox.Enabled) VideoRateBox.Maximum = VideoMultiplyRateRadio.Checked ? 16 : 4;
			if (AudioRateBox.Enabled) AudioRateBox.Maximum = AudioMultiplyRateRadio.Checked ? 16 : 4;
			if (VideoGainBox.Enabled) VideoGainBox.Maximum = VideoMultiplyGainRadio.Checked ? 100 : 1;
			if (AudioGainBox.Enabled) AudioGainBox.Maximum = AudioMultiplyGainRadio.Checked ? 100 : 1;
		}

		private void ChangeThreeStateCheckBoxStateChangeOrder(object sender, EventArgs e) {
			CheckBox check = sender as CheckBox;
			if (check == null) return;
			switch (check.CheckState) {
				case CheckState.Unchecked:
					check.CheckState = CheckState.Indeterminate;
					break;
				default:
				case CheckState.Indeterminate:
					check.CheckState = CheckState.Checked;
					break;
				case CheckState.Checked:
					check.CheckState = CheckState.Unchecked;
					break;
			}
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
