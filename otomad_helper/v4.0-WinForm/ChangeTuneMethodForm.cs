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
	public partial class ChangeTuneMethodForm : Form {
		public ChangeTuneMethodForm() {
			InitializeComponent();
			elastiqueAttrArray = new string[] {
				"专业", "高效", "独奏（单声道）", "独奏（语音）"
			};
			classicAttrArray = new string[] {
				"A01. 音乐 1（最小变调，可能有回音）",
				"A02. 音乐 2",
				"A03. 音乐 3（回音较小）",
				"A04. 音乐 4（快速，适合低音）",
				"A05. 音乐 5",
				"A06. 音乐 6",
				"A07. 语音 1",
				"A08. 语音 2",
				"A09. 语音 3（快速）",
				"A10. 独奏乐器 1",
				"A11. 独奏乐器 2",
				"A12. 独奏乐器 3",
				"A13. 独奏乐器 4（回音较小）",
				"A14. 独奏乐器 5",
				"A15. 独奏乐器 6",
				"A16. 独奏乐器 7（快速）",
				"A17. 鼓，无音高（最小回音）",
				"A18. 鼓（更适用于通鼓）",
				"A19. 鼓（微弱回音）",
			};
			MethodCombo.SelectedIndex = 1;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void MethodCombo_SelectedIndexChanged(object sender, EventArgs e) {
			TimeStretchPitchShift method =
				MethodCombo.SelectedIndex == 1 ? TimeStretchPitchShift.Elastique :
				MethodCombo.SelectedIndex == 2 ? TimeStretchPitchShift.Classic :
				TimeStretchPitchShift.None;
			StretchAttrCombo.Enabled = PitchLockCheck.Enabled = method != TimeStretchPitchShift.None;
			FormantLockCheck.Enabled = method == TimeStretchPitchShift.Elastique && StretchAttrCombo.SelectedIndex == 0;
			if (method != lastMethod) {
				lastMethod = method;
				MethodCombo.Items.Clear();
				if (method == TimeStretchPitchShift.Elastique) {
					MethodCombo.Items.AddRange(elastiqueAttrArray);
					MethodCombo.SelectedIndex = 1;
				} else if (method == TimeStretchPitchShift.Classic) {
					MethodCombo.Items.AddRange(classicAttrArray);
					MethodCombo.SelectedIndex = 0;
				}
			}
			if (method == TimeStretchPitchShift.Elastique) {
				ElastiqueStretchAttributes attr = (ElastiqueStretchAttributes)StretchAttrCombo.SelectedIndex;
				if (attr == ElastiqueStretchAttributes.Efficient || attr == ElastiqueStretchAttributes.Soloist_Speech) FormantLockCheck.Enabled = false;
				else if (attr == ElastiqueStretchAttributes.Soloist_Monophonic) FormantLockCheck.Enabled = true;
			}
		}

		private static string[] elastiqueAttrArray;
		private static string[] classicAttrArray;
		private TimeStretchPitchShift lastMethod = TimeStretchPitchShift.Elastique;
	}
}
