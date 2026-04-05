namespace Otomad.VegasScripts.OtomadHelper.V4 {
	partial class CustomFadeGainForm {
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
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ToBox = new Otomad.VegasScripts.OtomadHelper.V4.IntegerTrackWithBox();
			this.FromBox = new Otomad.VegasScripts.OtomadHelper.V4.IntegerTrackWithBox();
			this.ToLbl = new System.Windows.Forms.Label();
			this.FromLbl = new System.Windows.Forms.Label();
			this.PreviewBtn = new System.Windows.Forms.Button();
			this.MultiplyGainCheck = new System.Windows.Forms.CheckBox();
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dock
			// 
			this.dock.AutoSize = true;
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 3;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.Controls.Add(this.OkBtn, 1, 0);
			this.dock.Controls.Add(this.CancelBtn, 2, 0);
			this.dock.Controls.Add(this.MultiplyGainCheck, 0, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 361);
			this.dock.Margin = new System.Windows.Forms.Padding(6);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(13, 10, 13, 10);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(800, 87);
			this.dock.TabIndex = 16;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(463, 18);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(150, 51);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(629, 18);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(150, 51);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.ToBox, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.FromBox, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.ToLbl, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.FromLbl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.PreviewBtn, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(12);
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 361);
			this.tableLayoutPanel1.TabIndex = 17;
			// 
			// ToBox
			// 
			this.ToBox.BackColor = System.Drawing.Color.Transparent;
			this.ToBox.DefaultValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.ToBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.ToBox.Location = new System.Drawing.Point(68, 277);
			this.ToBox.Margin = new System.Windows.Forms.Padding(10);
			this.ToBox.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.ToBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ToBox.MinimumSize = new System.Drawing.Size(0, 62);
			this.ToBox.Name = "ToBox";
			this.ToBox.NumericUpDownWidth = 65;
			this.ToBox.Size = new System.Drawing.Size(710, 62);
			this.ToBox.TabIndex = 4;
			this.ToBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.ToBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.ToBox.ValueChanged += new System.EventHandler(this.FadeBox_ValueChanged);
			// 
			// FromBox
			// 
			this.FromBox.BackColor = System.Drawing.Color.Transparent;
			this.FromBox.DefaultValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.FromBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FromBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FromBox.Location = new System.Drawing.Point(68, 195);
			this.FromBox.Margin = new System.Windows.Forms.Padding(10);
			this.FromBox.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.FromBox.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.FromBox.MinimumSize = new System.Drawing.Size(0, 62);
			this.FromBox.Name = "FromBox";
			this.FromBox.NumericUpDownWidth = 65;
			this.FromBox.Size = new System.Drawing.Size(710, 62);
			this.FromBox.TabIndex = 3;
			this.FromBox.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.FromBox.ValueChanged += new System.EventHandler(this.FadeBox_ValueChanged);
			// 
			// ToLbl
			// 
			this.ToLbl.AutoSize = true;
			this.ToLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToLbl.Location = new System.Drawing.Point(16, 267);
			this.ToLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ToLbl.MinimumSize = new System.Drawing.Size(0, 82);
			this.ToLbl.Name = "ToLbl";
			this.ToLbl.Size = new System.Drawing.Size(38, 82);
			this.ToLbl.TabIndex = 2;
			this.ToLbl.Text = "至";
			this.ToLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FromLbl
			// 
			this.FromLbl.AutoSize = true;
			this.FromLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FromLbl.Location = new System.Drawing.Point(16, 185);
			this.FromLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.FromLbl.MinimumSize = new System.Drawing.Size(0, 82);
			this.FromLbl.Name = "FromLbl";
			this.FromLbl.Size = new System.Drawing.Size(38, 82);
			this.FromLbl.TabIndex = 1;
			this.FromLbl.Text = "从";
			this.FromLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PreviewBtn
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.PreviewBtn, 2);
			this.PreviewBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewBtn.Enabled = false;
			this.PreviewBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
			this.PreviewBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
			this.PreviewBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(253)))), ((int)(((byte)(253)))));
			this.PreviewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.PreviewBtn.Location = new System.Drawing.Point(40, 40);
			this.PreviewBtn.Margin = new System.Windows.Forms.Padding(28);
			this.PreviewBtn.Name = "PreviewBtn";
			this.PreviewBtn.Size = new System.Drawing.Size(720, 117);
			this.PreviewBtn.TabIndex = 5;
			this.PreviewBtn.UseVisualStyleBackColor = true;
			this.PreviewBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewBtn_Paint);
			// 
			// MultiplyGainCheck
			// 
			this.MultiplyGainCheck.AutoSize = true;
			this.MultiplyGainCheck.Dock = System.Windows.Forms.DockStyle.Left;
			this.MultiplyGainCheck.Location = new System.Drawing.Point(16, 13);
			this.MultiplyGainCheck.Name = "MultiplyGainCheck";
			this.MultiplyGainCheck.Size = new System.Drawing.Size(190, 61);
			this.MultiplyGainCheck.TabIndex = 3;
			this.MultiplyGainCheck.Text = "乘以当前增益";
			this.MultiplyGainCheck.UseVisualStyleBackColor = true;
			// 
			// CustomFadeGainForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(800, 448);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomFadeGainForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自定渐入增益";
			this.dock.ResumeLayout(false);
			this.dock.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.Label ToLbl;
		public System.Windows.Forms.Label FromLbl;
		public IntegerTrackWithBox ToBox;
		public IntegerTrackWithBox FromBox;
		private System.Windows.Forms.Button PreviewBtn;
		private System.Windows.Forms.CheckBox MultiplyGainCheck;
	}
}
