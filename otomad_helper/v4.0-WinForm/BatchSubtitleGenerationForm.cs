using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class BatchSubtitleGenerationForm : Form {
		public BatchSubtitleGenerationForm() {
			InitializeComponent();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void SingleDurationTxt_Leave(object sender, EventArgs e) {
			SingleDurationTxt.Text = "0";
		}

		private void ImportFromFileBtn_Click(object sender, EventArgs e) {
			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = GetOpenFileDialogFilter("文本文档", "*.txt;*.text;*.log;*.md;*.lrc;*.srt", "所有文件", "*.*"),
				RestoreDirectory = false,
				FilterIndex = 1,
				Title = "打开",
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK) return;
			long fileSize = new System.IO.FileInfo(openFileDialog.FileName).Length;
			const long LARGE_FILE_SIZE = 1024L * 1024L; // 设 1 MB 以上文件为大文件。
			if (fileSize > LARGE_FILE_SIZE)
				if (MessageBox.Show("文件过大，是否仍要打开？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No) return;
			if (!string.IsNullOrWhiteSpace(SubtitlesTxt.Text))
				if (MessageBox.Show("将会清除现有的文本内容，系统可能不会保留您的更改。", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Cancel) return;
			string text = System.IO.File.ReadAllText(openFileDialog.FileName);
			text = Regex.Replace(text, @"\r\n|\n\r|\r|\n", "\r\n");
			SubtitlesTxt.Text = text;
		}

		private static string GetOpenFileDialogFilter(params string[] exts) {
			return string.Join("|", exts);
		}
	}
}
