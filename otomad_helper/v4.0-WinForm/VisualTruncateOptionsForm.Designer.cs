namespace Otomad.VegasScripts.OtomadHelper.V4 {
	partial class VisualTruncateOptionsForm {
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
			this.CloseBtn = new System.Windows.Forms.Button();
			this.OptionsTable = new System.Windows.Forms.TableLayoutPanel();
			this.LoopWithSplitRadio = new System.Windows.Forms.RadioButton();
			this.PingPongWithSplitRadio = new System.Windows.Forms.RadioButton();
			this.PingPongWithoutSplitRadio = new System.Windows.Forms.RadioButton();
			this.SplitThenResumeRadio = new System.Windows.Forms.RadioButton();
			this.SplitThenFreezeRadio = new System.Windows.Forms.RadioButton();
			this.TrimEndFramesRadio = new System.Windows.Forms.RadioButton();
			this.FreezeEndFramesRadio = new System.Windows.Forms.RadioButton();
			this.LengthenableRadio = new System.Windows.Forms.RadioButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.IdleEffectGroup = new System.Windows.Forms.GroupBox();
			this.IdleEffectTable = new System.Windows.Forms.TableLayoutPanel();
			this.NegativeCheck = new System.Windows.Forms.CheckBox();
			this.FadeTrack = new Otomad.VegasScripts.OtomadHelper.V4.IntegerTrackWithBox();
			this.FadeCheck = new System.Windows.Forms.CheckBox();
			this.MonochromeTrack = new Otomad.VegasScripts.OtomadHelper.V4.IntegerTrackWithBox();
			this.MonochromeCheck = new System.Windows.Forms.CheckBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.LoopRegionTable = new System.Windows.Forms.TableLayoutPanel();
			this.LoopRegionTrack = new Otomad.VegasScripts.OtomadHelper.V4.IntegerTrackWithBox();
			this.LoopRegionLbl = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.OptionsTable.SuspendLayout();
			this.panel1.SuspendLayout();
			this.IdleEffectGroup.SuspendLayout();
			this.IdleEffectTable.SuspendLayout();
			this.LoopRegionTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.CloseBtn, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 595);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 4);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(731, 80);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CloseBtn
			// 
			this.CloseBtn.AutoSize = true;
			this.CloseBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.CloseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CloseBtn.Location = new System.Drawing.Point(557, 14);
			this.CloseBtn.Margin = new System.Windows.Forms.Padding(6);
			this.CloseBtn.Name = "CloseBtn";
			this.CloseBtn.Size = new System.Drawing.Size(160, 56);
			this.CloseBtn.TabIndex = 0;
			this.CloseBtn.Text = "关闭(&C)";
			this.CloseBtn.UseVisualStyleBackColor = true;
			// 
			// OptionsTable
			// 
			this.OptionsTable.AutoSize = true;
			this.OptionsTable.ColumnCount = 2;
			this.OptionsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.OptionsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.OptionsTable.Controls.Add(this.LoopWithSplitRadio, 1, 3);
			this.OptionsTable.Controls.Add(this.PingPongWithSplitRadio, 0, 3);
			this.OptionsTable.Controls.Add(this.PingPongWithoutSplitRadio, 1, 2);
			this.OptionsTable.Controls.Add(this.SplitThenResumeRadio, 0, 2);
			this.OptionsTable.Controls.Add(this.SplitThenFreezeRadio, 1, 1);
			this.OptionsTable.Controls.Add(this.TrimEndFramesRadio, 0, 1);
			this.OptionsTable.Controls.Add(this.FreezeEndFramesRadio, 1, 0);
			this.OptionsTable.Controls.Add(this.LengthenableRadio, 0, 0);
			this.OptionsTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.OptionsTable.Location = new System.Drawing.Point(12, 12);
			this.OptionsTable.Name = "OptionsTable";
			this.OptionsTable.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			this.OptionsTable.RowCount = 4;
			this.OptionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OptionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OptionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OptionsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.OptionsTable.Size = new System.Drawing.Size(707, 204);
			this.OptionsTable.TabIndex = 2;
			// 
			// LoopWithSplitRadio
			// 
			this.LoopWithSplitRadio.AutoSize = true;
			this.LoopWithSplitRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LoopWithSplitRadio.Location = new System.Drawing.Point(356, 156);
			this.LoopWithSplitRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.LoopWithSplitRadio.Name = "LoopWithSplitRadio";
			this.LoopWithSplitRadio.Size = new System.Drawing.Size(338, 45);
			this.LoopWithSplitRadio.TabIndex = 7;
			this.LoopWithSplitRadio.Text = "循环且分割";
			this.LoopWithSplitRadio.UseVisualStyleBackColor = true;
			// 
			// PingPongWithSplitRadio
			// 
			this.PingPongWithSplitRadio.AutoSize = true;
			this.PingPongWithSplitRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PingPongWithSplitRadio.Location = new System.Drawing.Point(13, 156);
			this.PingPongWithSplitRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.PingPongWithSplitRadio.Name = "PingPongWithSplitRadio";
			this.PingPongWithSplitRadio.Size = new System.Drawing.Size(337, 45);
			this.PingPongWithSplitRadio.TabIndex = 6;
			this.PingPongWithSplitRadio.Text = "来回且分割";
			this.PingPongWithSplitRadio.UseVisualStyleBackColor = true;
			// 
			// PingPongWithoutSplitRadio
			// 
			this.PingPongWithoutSplitRadio.AutoSize = true;
			this.PingPongWithoutSplitRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PingPongWithoutSplitRadio.Location = new System.Drawing.Point(356, 105);
			this.PingPongWithoutSplitRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.PingPongWithoutSplitRadio.Name = "PingPongWithoutSplitRadio";
			this.PingPongWithoutSplitRadio.Size = new System.Drawing.Size(338, 45);
			this.PingPongWithoutSplitRadio.TabIndex = 5;
			this.PingPongWithoutSplitRadio.Text = "来回不分割";
			this.PingPongWithoutSplitRadio.UseVisualStyleBackColor = true;
			// 
			// SplitThenResumeRadio
			// 
			this.SplitThenResumeRadio.AutoSize = true;
			this.SplitThenResumeRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplitThenResumeRadio.Location = new System.Drawing.Point(13, 105);
			this.SplitThenResumeRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.SplitThenResumeRadio.Name = "SplitThenResumeRadio";
			this.SplitThenResumeRadio.Size = new System.Drawing.Size(337, 45);
			this.SplitThenResumeRadio.TabIndex = 4;
			this.SplitThenResumeRadio.Text = "分割后续播";
			this.SplitThenResumeRadio.UseVisualStyleBackColor = true;
			// 
			// SplitThenFreezeRadio
			// 
			this.SplitThenFreezeRadio.AutoSize = true;
			this.SplitThenFreezeRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SplitThenFreezeRadio.Location = new System.Drawing.Point(356, 54);
			this.SplitThenFreezeRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.SplitThenFreezeRadio.Name = "SplitThenFreezeRadio";
			this.SplitThenFreezeRadio.Size = new System.Drawing.Size(338, 45);
			this.SplitThenFreezeRadio.TabIndex = 3;
			this.SplitThenFreezeRadio.Text = "分割后冻结";
			this.SplitThenFreezeRadio.UseVisualStyleBackColor = true;
			// 
			// TrimEndFramesRadio
			// 
			this.TrimEndFramesRadio.AutoSize = true;
			this.TrimEndFramesRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TrimEndFramesRadio.Location = new System.Drawing.Point(13, 54);
			this.TrimEndFramesRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.TrimEndFramesRadio.Name = "TrimEndFramesRadio";
			this.TrimEndFramesRadio.Size = new System.Drawing.Size(337, 45);
			this.TrimEndFramesRadio.TabIndex = 2;
			this.TrimEndFramesRadio.Text = "截断尾帧";
			this.TrimEndFramesRadio.UseVisualStyleBackColor = true;
			// 
			// FreezeEndFramesRadio
			// 
			this.FreezeEndFramesRadio.AutoSize = true;
			this.FreezeEndFramesRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FreezeEndFramesRadio.Location = new System.Drawing.Point(356, 3);
			this.FreezeEndFramesRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.FreezeEndFramesRadio.Name = "FreezeEndFramesRadio";
			this.FreezeEndFramesRadio.Size = new System.Drawing.Size(338, 45);
			this.FreezeEndFramesRadio.TabIndex = 1;
			this.FreezeEndFramesRadio.Text = "冻结尾帧";
			this.FreezeEndFramesRadio.UseVisualStyleBackColor = true;
			// 
			// LengthenableRadio
			// 
			this.LengthenableRadio.AutoSize = true;
			this.LengthenableRadio.Checked = true;
			this.LengthenableRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LengthenableRadio.Location = new System.Drawing.Point(13, 3);
			this.LengthenableRadio.MinimumSize = new System.Drawing.Size(0, 45);
			this.LengthenableRadio.Name = "LengthenableRadio";
			this.LengthenableRadio.Size = new System.Drawing.Size(337, 45);
			this.LengthenableRadio.TabIndex = 0;
			this.LengthenableRadio.TabStop = true;
			this.LengthenableRadio.Text = "可延长 (关)";
			this.LengthenableRadio.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.IdleEffectGroup);
			this.panel1.Controls.Add(this.LoopRegionTable);
			this.panel1.Controls.Add(this.OptionsTable);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(12);
			this.panel1.Size = new System.Drawing.Size(731, 595);
			this.panel1.TabIndex = 3;
			// 
			// IdleEffectGroup
			// 
			this.IdleEffectGroup.AutoSize = true;
			this.IdleEffectGroup.Controls.Add(this.IdleEffectTable);
			this.IdleEffectGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.IdleEffectGroup.Location = new System.Drawing.Point(12, 322);
			this.IdleEffectGroup.Name = "IdleEffectGroup";
			this.IdleEffectGroup.Padding = new System.Windows.Forms.Padding(12, 0, 12, 12);
			this.IdleEffectGroup.Size = new System.Drawing.Size(707, 242);
			this.IdleEffectGroup.TabIndex = 3;
			this.IdleEffectGroup.TabStop = false;
			this.IdleEffectGroup.Text = "闲置效果";
			// 
			// IdleEffectTable
			// 
			this.IdleEffectTable.AutoSize = true;
			this.IdleEffectTable.ColumnCount = 2;
			this.IdleEffectTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.IdleEffectTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.IdleEffectTable.Controls.Add(this.NegativeCheck, 0, 2);
			this.IdleEffectTable.Controls.Add(this.FadeTrack, 1, 1);
			this.IdleEffectTable.Controls.Add(this.FadeCheck, 0, 1);
			this.IdleEffectTable.Controls.Add(this.MonochromeTrack, 1, 0);
			this.IdleEffectTable.Controls.Add(this.MonochromeCheck, 0, 0);
			this.IdleEffectTable.Controls.Add(this.comboBox1, 1, 2);
			this.IdleEffectTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.IdleEffectTable.Location = new System.Drawing.Point(12, 32);
			this.IdleEffectTable.Name = "IdleEffectTable";
			this.IdleEffectTable.RowCount = 3;
			this.IdleEffectTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.IdleEffectTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.IdleEffectTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.IdleEffectTable.Size = new System.Drawing.Size(683, 198);
			this.IdleEffectTable.TabIndex = 3;
			// 
			// NegativeCheck
			// 
			this.NegativeCheck.AutoSize = true;
			this.NegativeCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NegativeCheck.Location = new System.Drawing.Point(3, 134);
			this.NegativeCheck.MinimumSize = new System.Drawing.Size(0, 45);
			this.NegativeCheck.Name = "NegativeCheck";
			this.NegativeCheck.Size = new System.Drawing.Size(94, 61);
			this.NegativeCheck.TabIndex = 6;
			this.NegativeCheck.Text = "反色";
			this.NegativeCheck.UseVisualStyleBackColor = true;
			// 
			// FadeTrack
			// 
			this.FadeTrack.BackColor = System.Drawing.Color.Transparent;
			this.FadeTrack.DefaultValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.FadeTrack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FadeTrack.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FadeTrack.Location = new System.Drawing.Point(108, 73);
			this.FadeTrack.Margin = new System.Windows.Forms.Padding(8);
			this.FadeTrack.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.FadeTrack.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.FadeTrack.MinimumSize = new System.Drawing.Size(0, 50);
			this.FadeTrack.Name = "FadeTrack";
			this.FadeTrack.NumericUpDownWidth = 65;
			this.FadeTrack.Size = new System.Drawing.Size(567, 50);
			this.FadeTrack.TabIndex = 5;
			this.FadeTrack.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// FadeCheck
			// 
			this.FadeCheck.AutoSize = true;
			this.FadeCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FadeCheck.Location = new System.Drawing.Point(3, 68);
			this.FadeCheck.MinimumSize = new System.Drawing.Size(0, 45);
			this.FadeCheck.Name = "FadeCheck";
			this.FadeCheck.Size = new System.Drawing.Size(94, 60);
			this.FadeCheck.TabIndex = 4;
			this.FadeCheck.Text = "淡化";
			this.FadeCheck.UseVisualStyleBackColor = true;
			// 
			// MonochromeTrack
			// 
			this.MonochromeTrack.BackColor = System.Drawing.Color.Transparent;
			this.MonochromeTrack.DecimalPlace = 0;
			this.MonochromeTrack.DefaultValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.MonochromeTrack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MonochromeTrack.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.MonochromeTrack.Location = new System.Drawing.Point(108, 8);
			this.MonochromeTrack.Margin = new System.Windows.Forms.Padding(8);
			this.MonochromeTrack.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.MonochromeTrack.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.MonochromeTrack.MinimumSize = new System.Drawing.Size(0, 50);
			this.MonochromeTrack.Name = "MonochromeTrack";
			this.MonochromeTrack.NumericUpDownWidth = 65;
			this.MonochromeTrack.Size = new System.Drawing.Size(567, 50);
			this.MonochromeTrack.TabIndex = 3;
			this.MonochromeTrack.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			// 
			// MonochromeCheck
			// 
			this.MonochromeCheck.AutoSize = true;
			this.MonochromeCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MonochromeCheck.Location = new System.Drawing.Point(3, 3);
			this.MonochromeCheck.MinimumSize = new System.Drawing.Size(0, 45);
			this.MonochromeCheck.Name = "MonochromeCheck";
			this.MonochromeCheck.Size = new System.Drawing.Size(94, 59);
			this.MonochromeCheck.TabIndex = 0;
			this.MonochromeCheck.Text = "黑白";
			this.MonochromeCheck.UseVisualStyleBackColor = true;
			// 
			// comboBox1
			// 
			this.comboBox1.Dock = System.Windows.Forms.DockStyle.Left;
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.ItemHeight = 32;
			this.comboBox1.Items.AddRange(new object[] {
            "色相反转",
            "辉度反转",
            "颜色反转"});
			this.comboBox1.Location = new System.Drawing.Point(103, 134);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(300, 40);
			this.comboBox1.TabIndex = 7;
			// 
			// LoopRegionTable
			// 
			this.LoopRegionTable.AutoSize = true;
			this.LoopRegionTable.ColumnCount = 2;
			this.LoopRegionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.LoopRegionTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.LoopRegionTable.Controls.Add(this.LoopRegionTrack, 1, 0);
			this.LoopRegionTable.Controls.Add(this.LoopRegionLbl, 0, 0);
			this.LoopRegionTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.LoopRegionTable.Location = new System.Drawing.Point(12, 216);
			this.LoopRegionTable.Name = "LoopRegionTable";
			this.LoopRegionTable.Padding = new System.Windows.Forms.Padding(12, 20, 12, 20);
			this.LoopRegionTable.RowCount = 1;
			this.LoopRegionTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.LoopRegionTable.Size = new System.Drawing.Size(707, 106);
			this.LoopRegionTable.TabIndex = 4;
			// 
			// LoopRegionTrack
			// 
			this.LoopRegionTrack.BackColor = System.Drawing.Color.Transparent;
			this.LoopRegionTrack.DecimalPlace = 0;
			this.LoopRegionTrack.DefaultValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.LoopRegionTrack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LoopRegionTrack.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.LoopRegionTrack.Location = new System.Drawing.Point(184, 28);
			this.LoopRegionTrack.Margin = new System.Windows.Forms.Padding(8);
			this.LoopRegionTrack.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.LoopRegionTrack.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.LoopRegionTrack.MinimumSize = new System.Drawing.Size(0, 50);
			this.LoopRegionTrack.Name = "LoopRegionTrack";
			this.LoopRegionTrack.NumericUpDownWidth = 65;
			this.LoopRegionTrack.Size = new System.Drawing.Size(503, 50);
			this.LoopRegionTrack.TabIndex = 3;
			this.LoopRegionTrack.TrackRightToLeft = true;
			this.LoopRegionTrack.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// LoopRegionLbl
			// 
			this.LoopRegionLbl.AutoSize = true;
			this.LoopRegionLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LoopRegionLbl.Location = new System.Drawing.Point(15, 20);
			this.LoopRegionLbl.Name = "LoopRegionLbl";
			this.LoopRegionLbl.Size = new System.Drawing.Size(158, 66);
			this.LoopRegionLbl.TabIndex = 4;
			this.LoopRegionLbl.Text = "循环区域占比";
			this.LoopRegionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VisualTruncateOptionsForm
			// 
			this.AcceptButton = this.CloseBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CloseBtn;
			this.ClientSize = new System.Drawing.Size(731, 675);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "VisualTruncateOptionsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "截断画面选项";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.OptionsTable.ResumeLayout(false);
			this.OptionsTable.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.IdleEffectGroup.ResumeLayout(false);
			this.IdleEffectGroup.PerformLayout();
			this.IdleEffectTable.ResumeLayout(false);
			this.IdleEffectTable.PerformLayout();
			this.LoopRegionTable.ResumeLayout(false);
			this.LoopRegionTable.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button CloseBtn;
		private System.Windows.Forms.TableLayoutPanel OptionsTable;
		private System.Windows.Forms.RadioButton LengthenableRadio;
		private System.Windows.Forms.RadioButton LoopWithSplitRadio;
		private System.Windows.Forms.RadioButton PingPongWithSplitRadio;
		private System.Windows.Forms.RadioButton PingPongWithoutSplitRadio;
		private System.Windows.Forms.RadioButton SplitThenResumeRadio;
		private System.Windows.Forms.RadioButton SplitThenFreezeRadio;
		private System.Windows.Forms.RadioButton TrimEndFramesRadio;
		private System.Windows.Forms.RadioButton FreezeEndFramesRadio;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox IdleEffectGroup;
		private System.Windows.Forms.TableLayoutPanel IdleEffectTable;
		private System.Windows.Forms.CheckBox MonochromeCheck;
		private System.Windows.Forms.CheckBox NegativeCheck;
		public IntegerTrackWithBox FadeTrack;
		private System.Windows.Forms.CheckBox FadeCheck;
		public IntegerTrackWithBox MonochromeTrack;
		private System.Windows.Forms.TableLayoutPanel LoopRegionTable;
		public IntegerTrackWithBox LoopRegionTrack;
		private System.Windows.Forms.Label LoopRegionLbl;
		private System.Windows.Forms.ComboBox comboBox1;
	}
}