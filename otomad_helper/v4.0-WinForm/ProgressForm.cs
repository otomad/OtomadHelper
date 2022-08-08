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
	public partial class ProgressForm : Form {
		public ProgressForm() {
			InitializeComponent();
			ControlBox = false;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		public int Minimum { get { return ProgressBar.Minimum; } }
		public int Maximum { get { return ProgressBar.Maximum; } }

		public bool ReportProgress(int value) {
			if (value < Minimum) value = Minimum;
			if (value > Maximum) value = Maximum;
			ProgressBar.Value = value;
			PercentLabel.Text = value + "%";
			Application.DoEvents();
			ProgressBar.Update();
			ProgressBar.Refresh();
			PercentLabel.Update();
			PercentLabel.Refresh();
			if (value < Maximum) return true;
			Close();
			return false;
		}

		public bool ReportProgress(double value) {
			return ReportProgress((int)value);
		}

		public int Progress {
			get { return ProgressBar.Value; }
			set { ReportProgress(value); }
		}

		public string Info {
			get { return InfoLabel.Text; }
			set {
				InfoLabel.Text = value;
				InfoLabel.Update();
				InfoLabel.Refresh();
			}
		}
	}
}
