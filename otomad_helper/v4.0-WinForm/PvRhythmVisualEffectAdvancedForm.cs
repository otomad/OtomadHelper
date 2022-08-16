using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class PvRhythmVisualEffectAdvancedForm : Form {
		private readonly ConfigForm configForm;
		private readonly Dictionary<PvVisualEffectType, RadioButton> type2radio = new Dictionary<PvVisualEffectType, RadioButton>();
		public PvRhythmVisualEffectAdvancedForm(ConfigForm configForm) {
			CheckForIllegalCrossThreadCalls = false;
			SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
			InitializeComponent();
			this.configForm = configForm;
			int groupIndex = 0;
			foreach (KeyValuePair<string, PvVisualEffectType[]> klass in PvVisualEffect.Classes) { // 避开关键字 class
				string className = klass.Key;
				GroupBox group = new GroupBox {
					Text = className,
					Dock = DockStyle.Fill,
				};
				EffectsTable.Controls.Add(group);
				EffectsTable.SetCellPosition(group,
					new TableLayoutPanelCellPosition(0, groupIndex++));
				TableLayoutPanel effectTable = new TableLayoutPanel {
					Dock = DockStyle.Fill,
					AutoSize = true,
					GrowStyle = TableLayoutPanelGrowStyle.AddColumns,
					ColumnCount = 2,
					RowCount = 2,
				};
				group.Controls.Add(effectTable);
				effectTable.ColumnStyles.Add(new ColumnStyle());
				effectTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
				effectTable.Controls.Add(new Label {
					Text = "初始值",
					Dock = DockStyle.Top,
					TextAlign = ContentAlignment.MiddleLeft,
					MinimumSize = new Size(0, LABEL_MIN_HEIGHT),
					AutoSize = true,
				}, 0, 1);
				ComboBox initialCombo = new ComboBox {
					DropDownStyle = ComboBoxStyle.DropDownList,
					Dock = DockStyle.Left,
					Width = 90,
				};
				effectTable.Controls.Add(initialCombo, 1, 1);
				effectTable.RowStyles.Add(new RowStyle());
				FlowLayoutPanel effectRadioFlow = new FlowLayoutPanel {
					Dock = DockStyle.Fill,
					AutoSize = true,
					WrapContents = true,
					FlowDirection = FlowDirection.LeftToRight,
				};
				effectTable.Controls.Add(effectRadioFlow, 0, 0);
				effectTable.SetColumnSpan(effectRadioFlow, 2);
				RadioButton noneRadio = new RadioButton {
					Text = "无效果",
					TextAlign = ContentAlignment.MiddleLeft,
					AutoSize = true,
					Checked = true,
				};
				effectRadioFlow.Controls.Add(noneRadio);
				foreach (PvVisualEffectType prveType in klass.Value) {
					if (prveType == PvVisualEffectType.NONE) continue;
					string effectName = configForm.VideoEffectCombo.Items[(int)prveType].ToString();
					RadioButton effectRadio = new RadioButton {
						Text = effectName,
						TextAlign = ContentAlignment.MiddleLeft,
						AutoSize = true,
					};
					if ((PvVisualEffectType)configForm.VideoEffectCombo.SelectedIndex == prveType) {
						effectRadio.Checked = true;
						noneRadio.Checked = false;
					}
					effectRadioFlow.Controls.Add(effectRadio);
					type2radio.Add(prveType, effectRadio);
					effectRadio.CheckedChanged += EffectRadio_CheckedChanged;
				}
			}
			Load += (sender, e) => PvRhythmVisualEffectAdvancedForm_Resize(null, null);
		}

		private void EffectRadio_CheckedChanged(object sender, EventArgs e) {
			Console.WriteLine("");
		}

		private const int LABEL_MIN_HEIGHT = 34;

		private void PvRhythmVisualEffectAdvancedForm_Resize(object sender, EventArgs e) {
			foreach (Control control_i in EffectsTable.Controls)
				if (control_i is GroupBox) {
					GroupBox group = control_i as GroupBox;
					foreach (Control control_j in control_i.Controls)
						if (control_j is TableLayoutPanel) {
							TableLayoutPanel effectTable = control_j as TableLayoutPanel;
							Label initialLbl = null;
							foreach (Control control_k in control_j.Controls)
								if (control_k is Label)
									initialLbl = control_k as Label;
							int height = group.PointToClient(initialLbl.PointToScreen(new Point(0, 0))).Y + LABEL_MIN_HEIGHT + effectTable.Margin.Top + effectTable.Margin.Bottom;
							group.Height = height;
						}
				}
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}
	}
}
