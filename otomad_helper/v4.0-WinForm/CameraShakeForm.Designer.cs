namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class CameraShakeForm {
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
			this.components = new System.ComponentModel.Container();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.XToYBox = new System.Windows.Forms.NumericUpDown();
			this.SkewOutBox = new System.Windows.Forms.NumericUpDown();
			this.SkewXYBox = new System.Windows.Forms.NumericUpDown();
			this.ShouldClearFramesCheck = new System.Windows.Forms.CheckBox();
			this.XToYLbl = new System.Windows.Forms.Label();
			this.SkewOutLbl = new System.Windows.Forms.Label();
			this.SkewXYLbl = new System.Windows.Forms.Label();
			this.SkewInLbl = new System.Windows.Forms.Label();
			this.ShouldResetPanCheck = new System.Windows.Forms.CheckBox();
			this.SkewInBox = new System.Windows.Forms.NumericUpDown();
			this.Balloon = new System.Windows.Forms.ToolTip(this.components);
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.XToYBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewOutBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewXYBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewInBox)).BeginInit();
			this.SuspendLayout();
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
			this.dock.Location = new System.Drawing.Point(0, 215);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(366, 52);
			this.dock.TabIndex = 9;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(158, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(260, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.XToYBox, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.SkewOutBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.SkewXYBox, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.ShouldClearFramesCheck, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.XToYLbl, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.SkewOutLbl, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.SkewXYLbl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.SkewInLbl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.ShouldResetPanCheck, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.SkewInBox, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(9);
			this.tableLayoutPanel1.RowCount = 6;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(366, 214);
			this.tableLayoutPanel1.TabIndex = 10;
			// 
			// XToYBox
			// 
			this.XToYBox.DecimalPlaces = 3;
			this.XToYBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.XToYBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.XToYBox.Location = new System.Drawing.Point(147, 114);
			this.XToYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.XToYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.XToYBox.Name = "XToYBox";
			this.XToYBox.Size = new System.Drawing.Size(207, 27);
			this.XToYBox.TabIndex = 9;
			this.XToYBox.Value = new decimal(new int[] {
            25,
            0,
            0,
            65536});
			// 
			// SkewOutBox
			// 
			this.SkewOutBox.DecimalPlaces = 3;
			this.SkewOutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewOutBox.Location = new System.Drawing.Point(147, 80);
			this.SkewOutBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.SkewOutBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.SkewOutBox.Name = "SkewOutBox";
			this.SkewOutBox.Size = new System.Drawing.Size(207, 27);
			this.SkewOutBox.TabIndex = 8;
			this.SkewOutBox.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
			// 
			// SkewXYBox
			// 
			this.SkewXYBox.DecimalPlaces = 3;
			this.SkewXYBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewXYBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.SkewXYBox.Location = new System.Drawing.Point(147, 46);
			this.SkewXYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.SkewXYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.SkewXYBox.Name = "SkewXYBox";
			this.SkewXYBox.Size = new System.Drawing.Size(207, 27);
			this.SkewXYBox.TabIndex = 7;
			this.SkewXYBox.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
			// 
			// ShouldClearFramesCheck
			// 
			this.ShouldClearFramesCheck.AutoSize = true;
			this.ShouldClearFramesCheck.Checked = true;
			this.ShouldClearFramesCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel1.SetColumnSpan(this.ShouldClearFramesCheck, 2);
			this.ShouldClearFramesCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ShouldClearFramesCheck.Location = new System.Drawing.Point(12, 178);
			this.ShouldClearFramesCheck.Name = "ShouldClearFramesCheck";
			this.ShouldClearFramesCheck.Size = new System.Drawing.Size(342, 24);
			this.ShouldClearFramesCheck.TabIndex = 5;
			this.ShouldClearFramesCheck.Text = "在摇晃前重置所有帧";
			this.ShouldClearFramesCheck.UseVisualStyleBackColor = true;
			// 
			// XToYLbl
			// 
			this.XToYLbl.AutoSize = true;
			this.XToYLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.XToYLbl.Location = new System.Drawing.Point(9, 111);
			this.XToYLbl.Margin = new System.Windows.Forms.Padding(0);
			this.XToYLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.XToYLbl.Name = "XToYLbl";
			this.XToYLbl.Size = new System.Drawing.Size(135, 34);
			this.XToYLbl.TabIndex = 3;
			this.XToYLbl.Text = "水平/垂直位移比";
			this.XToYLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SkewOutLbl
			// 
			this.SkewOutLbl.AutoSize = true;
			this.SkewOutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewOutLbl.Location = new System.Drawing.Point(9, 77);
			this.SkewOutLbl.Margin = new System.Windows.Forms.Padding(0);
			this.SkewOutLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.SkewOutLbl.Name = "SkewOutLbl";
			this.SkewOutLbl.Size = new System.Drawing.Size(135, 34);
			this.SkewOutLbl.TabIndex = 2;
			this.SkewOutLbl.Text = "数量";
			this.SkewOutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SkewXYLbl
			// 
			this.SkewXYLbl.AutoSize = true;
			this.SkewXYLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewXYLbl.Location = new System.Drawing.Point(9, 43);
			this.SkewXYLbl.Margin = new System.Windows.Forms.Padding(0);
			this.SkewXYLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.SkewXYLbl.Name = "SkewXYLbl";
			this.SkewXYLbl.Size = new System.Drawing.Size(135, 34);
			this.SkewXYLbl.TabIndex = 1;
			this.SkewXYLbl.Text = "水平/垂直同步系数";
			this.SkewXYLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SkewInLbl
			// 
			this.SkewInLbl.AutoSize = true;
			this.SkewInLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewInLbl.Location = new System.Drawing.Point(9, 9);
			this.SkewInLbl.Margin = new System.Windows.Forms.Padding(0);
			this.SkewInLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.SkewInLbl.Name = "SkewInLbl";
			this.SkewInLbl.Size = new System.Drawing.Size(135, 34);
			this.SkewInLbl.TabIndex = 0;
			this.SkewInLbl.Text = "速度";
			this.SkewInLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ShouldResetPanCheck
			// 
			this.ShouldResetPanCheck.AutoSize = true;
			this.tableLayoutPanel1.SetColumnSpan(this.ShouldResetPanCheck, 2);
			this.ShouldResetPanCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ShouldResetPanCheck.Location = new System.Drawing.Point(12, 148);
			this.ShouldResetPanCheck.Name = "ShouldResetPanCheck";
			this.ShouldResetPanCheck.Size = new System.Drawing.Size(342, 24);
			this.ShouldResetPanCheck.TabIndex = 4;
			this.ShouldResetPanCheck.Text = "在第一帧上重置平移/裁切";
			this.ShouldResetPanCheck.UseVisualStyleBackColor = true;
			// 
			// SkewInBox
			// 
			this.SkewInBox.DecimalPlaces = 3;
			this.SkewInBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SkewInBox.Location = new System.Drawing.Point(147, 12);
			this.SkewInBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.SkewInBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
			this.SkewInBox.Name = "SkewInBox";
			this.SkewInBox.Size = new System.Drawing.Size(207, 27);
			this.SkewInBox.TabIndex = 6;
			this.SkewInBox.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
			// 
			// Balloon
			// 
			this.Balloon.AutomaticDelay = 0;
			this.Balloon.AutoPopDelay = 60000;
			this.Balloon.InitialDelay = 0;
			this.Balloon.IsBalloon = true;
			this.Balloon.ReshowDelay = 0;
			this.Balloon.ShowAlways = true;
			this.Balloon.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.Balloon.ToolTipTitle = "填写说明";
			// 
			// CameraShakeForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(366, 267);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CameraShakeForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "镜头摇晃 - 参数";
			this.dock.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.XToYBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewOutBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewXYBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SkewInBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label SkewXYLbl;
		private System.Windows.Forms.Label SkewInLbl;
		private System.Windows.Forms.Label SkewOutLbl;
		private System.Windows.Forms.Label XToYLbl;
		private System.Windows.Forms.CheckBox ShouldResetPanCheck;
		private System.Windows.Forms.CheckBox ShouldClearFramesCheck;
		private System.Windows.Forms.NumericUpDown SkewInBox;
		private System.Windows.Forms.NumericUpDown XToYBox;
		private System.Windows.Forms.NumericUpDown SkewOutBox;
		private System.Windows.Forms.NumericUpDown SkewXYBox;
		public System.Windows.Forms.ToolTip Balloon;
	}
}