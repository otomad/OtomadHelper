
namespace VegasScript {
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
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.CancelBtn, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 174);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 42);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(386, 9);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 24);
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
			this.tableLayoutPanel2.Controls.Add(this.RealTimeUpdateCheck, 0, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(471, 174);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// InfoLabel
			// 
			this.InfoLabel.AutoSize = true;
			this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLabel.Location = new System.Drawing.Point(11, 65);
			this.InfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.InfoLabel.Size = new System.Drawing.Size(449, 33);
			this.InfoLabel.TabIndex = 0;
			this.InfoLabel.Text = "正在生成音 MAD / YTPMV⋯⋯";
			this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PercentLabel
			// 
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PercentLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
			this.PercentLabel.Location = new System.Drawing.Point(6, 6);
			this.PercentLabel.Margin = new System.Windows.Forms.Padding(0);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(459, 59);
			this.PercentLabel.TabIndex = 1;
			this.PercentLabel.Text = "0%";
			this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.ProgressBar.Location = new System.Drawing.Point(14, 101);
			this.ProgressBar.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(443, 23);
			this.ProgressBar.Step = 1;
			this.ProgressBar.TabIndex = 2;
			this.ProgressBar.UseWaitCursor = true;
			// 
			// RealTimeUpdateCheck
			// 
			this.RealTimeUpdateCheck.AutoSize = true;
			this.RealTimeUpdateCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RealTimeUpdateCheck.Location = new System.Drawing.Point(9, 140);
			this.RealTimeUpdateCheck.Name = "RealTimeUpdateCheck";
			this.RealTimeUpdateCheck.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.RealTimeUpdateCheck.Size = new System.Drawing.Size(453, 25);
			this.RealTimeUpdateCheck.TabIndex = 3;
			this.RealTimeUpdateCheck.Text = "实时更新轨道 UI（会减慢生成速度）";
			this.RealTimeUpdateCheck.UseVisualStyleBackColor = true;
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(471, 216);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
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