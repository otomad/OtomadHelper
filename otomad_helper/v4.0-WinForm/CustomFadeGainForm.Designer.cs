namespace Otomad.VegasScript.OtomadHelper.V4 {
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
			this.ToBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.FromBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.ToLbl = new System.Windows.Forms.Label();
			this.FromLbl = new System.Windows.Forms.Label();
			this.PreviewBtn = new System.Windows.Forms.Button();
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
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
			this.dock.Location = new System.Drawing.Point(0, 179);
			this.dock.Margin = new System.Windows.Forms.Padding(4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(462, 52);
			this.dock.TabIndex = 16;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(254, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(356, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
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
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(462, 179);
			this.tableLayoutPanel1.TabIndex = 17;
			// 
			// ToBox
			// 
			this.ToBox.BackColor = System.Drawing.Color.Transparent;
			this.ToBox.DefaultValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.ToBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.ToBox.Location = new System.Drawing.Point(39, 137);
			this.ToBox.Margin = new System.Windows.Forms.Padding(5);
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
			this.ToBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.ToBox.Name = "ToBox";
			this.ToBox.NumericUpDownWidth = 65;
			this.ToBox.Size = new System.Drawing.Size(412, 31);
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
            0,
            0,
            0,
            0});
			this.FromBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FromBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FromBox.Location = new System.Drawing.Point(39, 96);
			this.FromBox.Margin = new System.Windows.Forms.Padding(5);
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
			this.FromBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.FromBox.Name = "FromBox";
			this.FromBox.NumericUpDownWidth = 65;
			this.FromBox.Size = new System.Drawing.Size(412, 31);
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
			this.ToLbl.Location = new System.Drawing.Point(8, 132);
			this.ToLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ToLbl.MinimumSize = new System.Drawing.Size(0, 41);
			this.ToLbl.Name = "ToLbl";
			this.ToLbl.Size = new System.Drawing.Size(24, 41);
			this.ToLbl.TabIndex = 2;
			this.ToLbl.Text = "至";
			this.ToLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FromLbl
			// 
			this.FromLbl.AutoSize = true;
			this.FromLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FromLbl.Location = new System.Drawing.Point(8, 91);
			this.FromLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.FromLbl.MinimumSize = new System.Drawing.Size(0, 41);
			this.FromLbl.Name = "FromLbl";
			this.FromLbl.Size = new System.Drawing.Size(24, 41);
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
			this.PreviewBtn.Location = new System.Drawing.Point(20, 20);
			this.PreviewBtn.Margin = new System.Windows.Forms.Padding(14);
			this.PreviewBtn.Name = "PreviewBtn";
			this.PreviewBtn.Size = new System.Drawing.Size(422, 57);
			this.PreviewBtn.TabIndex = 5;
			this.PreviewBtn.UseVisualStyleBackColor = true;
			this.PreviewBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewBtn_Paint);
			// 
			// CustomFadeGainForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(462, 231);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CustomFadeGainForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自定渐入增益";
			this.dock.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

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
	}
}