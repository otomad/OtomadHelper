using OtomadHelper.Module;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace OtomadHelper.Test {
	public partial class TestForm : Form {
		private readonly MainDock mainDock;

		public TestForm() {
			InitializeComponent();
			mainDock = new MainDock();
			Controls.Add(mainDock);
			Icon = Properties.Resources.OtomadHelper;
		}
	}
}
