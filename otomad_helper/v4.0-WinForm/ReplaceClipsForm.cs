using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class ReplaceClipsForm : Form {
		public ReplaceClipsForm() {
			InitializeComponent();
			Icon = ConfigForm.icon;
			ReplacedIcon.Image = GetIcon(IconType.CROSS);
			ReplacerIcon.Image = GetIcon(IconType.CHECK);
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ReplacedBtn_Click(object sender, EventArgs e) {
			ClassicReplacedLbl.Text = "";
		}

		private void ReplacerBtn_Click(object sender, EventArgs e) {
			ClassicReplacerLbl.Text = "";
		}

		private void ReplacerCombo_SelectedIndexChanged(object sender, EventArgs e) {
			ClassicReplacerLbl.Text = "";
		}

		private void BackToSelect_Click(object sender, EventArgs e) {
			Close();
		}

		private void SetReplacedBtn_Click(object sender, EventArgs e) {
			(sender as Button).Enabled = false;
		}

		private void SetReplacerBtn_Click(object sender, EventArgs e) {
			(sender as Button).Enabled = false;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool MessageBeep(uint type);

		[DllImport("Shell32.dll")]
		public static extern int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);

		public static Icon LoadImageresIcon(int index) {
			IntPtr[] largeIcon = new IntPtr[1], smallIcon = new IntPtr[1];
			ExtractIconEx("imageres.dll", index, largeIcon, smallIcon, 1);
			Icon ic = Icon.FromHandle(largeIcon[0]);
			return ic;
		}

		private enum IconType {
			CROSS, CHECK
		}

		private Image GetIcon(IconType iconType) {
			return iconType == IconType.CROSS ? LoadImageresIcon(100).ToBitmap() :
				iconType == IconType.CHECK ? LoadImageresIcon(101).ToBitmap() :
				null;
		}

		private void ReplaceClipsForm_FormClosing(object sender, FormClosingEventArgs e) {
			Console.WriteLine("hh");
		}
	}
}
