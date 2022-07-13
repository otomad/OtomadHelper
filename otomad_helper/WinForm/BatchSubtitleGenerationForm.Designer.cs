namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class BatchSubtitleGenerationForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.PresetsLbl = new System.Windows.Forms.Label();
			this.PresetsCombo = new System.Windows.Forms.ComboBox();
			this.SubtitlesLbl = new System.Windows.Forms.Label();
			this.SubtitlesTxt = new System.Windows.Forms.TextBox();
			this.SuggestionInfo = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.SingleDurationLbl = new System.Windows.Forms.Label();
			this.SingleDurationTxt = new System.Windows.Forms.TextBox();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.dock.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(479, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(560, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.BackColor = System.Drawing.SystemColors.Window;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.PresetsLbl, 0, 0);
			this.table.Controls.Add(this.PresetsCombo, 0, 1);
			this.table.Controls.Add(this.SubtitlesLbl, 0, 2);
			this.table.Controls.Add(this.SubtitlesTxt, 0, 3);
			this.table.Controls.Add(this.SuggestionInfo, 0, 5);
			this.table.Controls.Add(this.tableLayoutPanel1, 0, 4);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
			this.table.RowCount = 6;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(644, 459);
			this.table.TabIndex = 19;
			// 
			// PresetsLbl
			// 
			this.PresetsLbl.AutoSize = true;
			this.PresetsLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PresetsLbl.Location = new System.Drawing.Point(13, 11);
			this.PresetsLbl.Name = "PresetsLbl";
			this.PresetsLbl.Size = new System.Drawing.Size(618, 15);
			this.PresetsLbl.TabIndex = 0;
			this.PresetsLbl.Text = "选择一个预先设定好的“字幕和文字”媒体发生器的预设：";
			this.PresetsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PresetsCombo
			// 
			this.PresetsCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PresetsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PresetsCombo.FormattingEnabled = true;
			this.PresetsCombo.Location = new System.Drawing.Point(13, 29);
			this.PresetsCombo.Name = "PresetsCombo";
			this.PresetsCombo.Size = new System.Drawing.Size(618, 23);
			this.PresetsCombo.TabIndex = 1;
			// 
			// SubtitlesLbl
			// 
			this.SubtitlesLbl.AutoSize = true;
			this.SubtitlesLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SubtitlesLbl.Location = new System.Drawing.Point(13, 61);
			this.SubtitlesLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
			this.SubtitlesLbl.Name = "SubtitlesLbl";
			this.SubtitlesLbl.Size = new System.Drawing.Size(618, 15);
			this.SubtitlesLbl.TabIndex = 2;
			this.SubtitlesLbl.Text = "输入要插入的字幕文本（一行一个，忽略空行）：";
			this.SubtitlesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SubtitlesTxt
			// 
			this.SubtitlesTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SubtitlesTxt.Location = new System.Drawing.Point(13, 79);
			this.SubtitlesTxt.MaxLength = 65535;
			this.SubtitlesTxt.Multiline = true;
			this.SubtitlesTxt.Name = "SubtitlesTxt";
			this.SubtitlesTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.SubtitlesTxt.Size = new System.Drawing.Size(618, 316);
			this.SubtitlesTxt.TabIndex = 3;
			this.SubtitlesTxt.WordWrap = false;
			// 
			// SuggestionInfo
			// 
			this.SuggestionInfo.AutoSize = true;
			this.SuggestionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SuggestionInfo.Location = new System.Drawing.Point(13, 433);
			this.SuggestionInfo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
			this.SuggestionInfo.Name = "SuggestionInfo";
			this.SuggestionInfo.Size = new System.Drawing.Size(618, 15);
			this.SuggestionInfo.TabIndex = 5;
			this.SuggestionInfo.Text = "稍后可开启“自动跟进”功能以便后续调整时间。";
			this.SuggestionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.SingleDurationLbl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.SingleDurationTxt, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 398);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 29);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// SingleDurationLbl
			// 
			this.SingleDurationLbl.AutoSize = true;
			this.SingleDurationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SingleDurationLbl.Location = new System.Drawing.Point(3, 0);
			this.SingleDurationLbl.Name = "SingleDurationLbl";
			this.SingleDurationLbl.Size = new System.Drawing.Size(103, 29);
			this.SingleDurationLbl.TabIndex = 0;
			this.SingleDurationLbl.Text = "每个字幕持续时间";
			this.SingleDurationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SingleDurationTxt
			// 
			this.SingleDurationTxt.Dock = System.Windows.Forms.DockStyle.Left;
			this.SingleDurationTxt.Location = new System.Drawing.Point(112, 3);
			this.SingleDurationTxt.Name = "SingleDurationTxt";
			this.SingleDurationTxt.Size = new System.Drawing.Size(150, 23);
			this.SingleDurationTxt.TabIndex = 1;
			this.SingleDurationTxt.Leave += new System.EventHandler(this.SingleDurationTxt_Leave);
			// 
			// dock
			// 
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 3;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.Controls.Add(this.OkBtn, 1, 0);
			this.dock.Controls.Add(this.CancelBtn, 2, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 459);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(644, 42);
			this.dock.TabIndex = 18;
			// 
			// BatchSubtitleGenerationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(644, 501);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(660, 540);
			this.Name = "BatchSubtitleGenerationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "批量生成字幕";
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.dock.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label PresetsLbl;
		private System.Windows.Forms.ComboBox PresetsCombo;
		private System.Windows.Forms.Label SubtitlesLbl;
		private System.Windows.Forms.TextBox SubtitlesTxt;
		private System.Windows.Forms.Label SuggestionInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label SingleDurationLbl;
		private System.Windows.Forms.TextBox SingleDurationTxt;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel dock;
	}
}