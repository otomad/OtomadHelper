
namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class ProgressForm {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLabel = new System.Windows.Forms.Label();
			this.PercentLabel = new System.Windows.Forms.Label();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.RealTimeUpdateCheck = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel1.Controls.Add(this.CancelBtn, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 191);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(18, 4, 18, 4);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(589, 50);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(472, 9);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(5);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.InfoLabel, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.PercentLabel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.ProgressBar, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(18, 8, 18, 8);
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(589, 191);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// InfoLabel
			// 
			this.InfoLabel.AutoSize = true;
			this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLabel.Location = new System.Drawing.Point(24, 86);
			this.InfoLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this.InfoLabel.Size = new System.Drawing.Size(541, 43);
			this.InfoLabel.TabIndex = 0;
			this.InfoLabel.Text = "正在生成音 MAD / YTPMV⋯⋯";
			this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PercentLabel
			// 
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PercentLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
			this.PercentLabel.Location = new System.Drawing.Point(18, 8);
			this.PercentLabel.Margin = new System.Windows.Forms.Padding(0);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(553, 78);
			this.PercentLabel.TabIndex = 1;
			this.PercentLabel.Text = "0%";
			this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.ProgressBar.Location = new System.Drawing.Point(28, 133);
			this.ProgressBar.Margin = new System.Windows.Forms.Padding(10, 4, 10, 4);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(533, 29);
			this.ProgressBar.Step = 1;
			this.ProgressBar.TabIndex = 2;
			this.ProgressBar.UseWaitCursor = true;
			// 
			// RealTimeUpdateCheck
			// 
			this.RealTimeUpdateCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RealTimeUpdateCheck.AutoSize = true;
			this.RealTimeUpdateCheck.BackColor = System.Drawing.SystemColors.Control;
			this.RealTimeUpdateCheck.Location = new System.Drawing.Point(22, 205);
			this.RealTimeUpdateCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 7);
			this.RealTimeUpdateCheck.Name = "RealTimeUpdateCheck";
			this.RealTimeUpdateCheck.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.RealTimeUpdateCheck.Size = new System.Drawing.Size(298, 24);
			this.RealTimeUpdateCheck.TabIndex = 6;
			this.RealTimeUpdateCheck.Text = "实时更新当前进度（会减慢生成速度）";
			this.RealTimeUpdateCheck.UseVisualStyleBackColor = true;
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(589, 241);
			this.ControlBox = false;
			this.Controls.Add(this.RealTimeUpdateCheck);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "正在处理它";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		public System.Windows.Forms.Label InfoLabel;
		public System.Windows.Forms.Label PercentLabel;
		public System.Windows.Forms.ProgressBar ProgressBar;
		public System.Windows.Forms.CheckBox RealTimeUpdateCheck;
	}
}