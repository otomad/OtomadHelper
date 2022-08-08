
namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class GradientTracksForm {
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
			this.InfoLbl = new System.Windows.Forms.Label();
			this.EffectsCombo = new System.Windows.Forms.ComboBox();
			this.ReverseCheck = new System.Windows.Forms.CheckBox();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table.SuspendLayout();
			this.dock.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(397, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(499, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.EffectsCombo, 0, 1);
			this.table.Controls.Add(this.ReverseCheck, 0, 2);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(5);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(12, 14, 12, 14);
			this.table.RowCount = 3;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.table.Size = new System.Drawing.Size(605, 132);
			this.table.TabIndex = 14;
			// 
			// InfoLbl
			// 
			this.InfoLbl.AutoSize = true;
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Location = new System.Drawing.Point(12, 22);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(581, 20);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "选择一种渐变效果应用到所选的视频轨道：";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// EffectsCombo
			// 
			this.EffectsCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EffectsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EffectsCombo.FormattingEnabled = true;
			this.EffectsCombo.Items.AddRange(new object[] {
            "彩虹色",
            "逐渐饱和",
            "逐渐对比",
            "阈值",
            "彩灰交替",
            "正负交替"});
			this.EffectsCombo.Location = new System.Drawing.Point(16, 50);
			this.EffectsCombo.Margin = new System.Windows.Forms.Padding(4);
			this.EffectsCombo.Name = "EffectsCombo";
			this.EffectsCombo.Size = new System.Drawing.Size(573, 28);
			this.EffectsCombo.TabIndex = 1;
			// 
			// ReverseCheck
			// 
			this.ReverseCheck.AutoSize = true;
			this.ReverseCheck.Checked = true;
			this.ReverseCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ReverseCheck.Location = new System.Drawing.Point(16, 90);
			this.ReverseCheck.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
			this.ReverseCheck.Name = "ReverseCheck";
			this.ReverseCheck.Size = new System.Drawing.Size(91, 24);
			this.ReverseCheck.TabIndex = 2;
			this.ReverseCheck.Text = "降序排序";
			this.ReverseCheck.UseVisualStyleBackColor = true;
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
			this.dock.Location = new System.Drawing.Point(0, 137);
			this.dock.Margin = new System.Windows.Forms.Padding(4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(605, 52);
			this.dock.TabIndex = 13;
			// 
			// GradientTracksForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(605, 189);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GradientTracksForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "渐变轨道";
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.dock.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel table;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.ComboBox EffectsCombo;
		private System.Windows.Forms.CheckBox ReverseCheck;
	}
}