
namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class SelectIntervalForm {
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
			this.ApplyBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.SubmitSelectBtn = new System.Windows.Forms.Button();
			this.SelectHowManyEachTimesBox = new System.Windows.Forms.NumericUpDown();
			this.SelectHowManyEachTimesLbl = new System.Windows.Forms.Label();
			this.SelectWhichEachGroupBox = new System.Windows.Forms.NumericUpDown();
			this.SelectOneEveryFewLbl = new System.Windows.Forms.Label();
			this.SelectWhichEachGroupLbl = new System.Windows.Forms.Label();
			this.SelectOneEveryFewBox = new System.Windows.Forms.NumericUpDown();
			this.ResetBtn = new System.Windows.Forms.Button();
			this.SelectIntervalLbl = new System.Windows.Forms.Label();
			this.SelectInfo = new System.Windows.Forms.Label();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectHowManyEachTimesBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).BeginInit();
			this.SuspendLayout();
			// 
			// dock
			// 
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 3;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.Controls.Add(this.ApplyBtn, 1, 0);
			this.dock.Controls.Add(this.CancelBtn, 2, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 229);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(544, 42);
			this.dock.TabIndex = 7;
			// 
			// ApplyBtn
			// 
			this.ApplyBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ApplyBtn.Location = new System.Drawing.Point(379, 8);
			this.ApplyBtn.Name = "ApplyBtn";
			this.ApplyBtn.Size = new System.Drawing.Size(75, 26);
			this.ApplyBtn.TabIndex = 1;
			this.ApplyBtn.Text = "应用(&A)";
			this.ApplyBtn.UseVisualStyleBackColor = true;
			this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(460, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "关闭(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.SubmitSelectBtn, 1, 5);
			this.table.Controls.Add(this.SelectHowManyEachTimesBox, 1, 4);
			this.table.Controls.Add(this.SelectHowManyEachTimesLbl, 0, 4);
			this.table.Controls.Add(this.SelectWhichEachGroupBox, 1, 3);
			this.table.Controls.Add(this.SelectOneEveryFewLbl, 0, 2);
			this.table.Controls.Add(this.SelectWhichEachGroupLbl, 0, 3);
			this.table.Controls.Add(this.SelectOneEveryFewBox, 1, 2);
			this.table.Controls.Add(this.ResetBtn, 0, 5);
			this.table.Controls.Add(this.SelectIntervalLbl, 0, 0);
			this.table.Controls.Add(this.SelectInfo, 0, 1);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(6);
			this.table.RowCount = 6;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(544, 190);
			this.table.TabIndex = 8;
			// 
			// SubmitSelectBtn
			// 
			this.SubmitSelectBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.SubmitSelectBtn.Location = new System.Drawing.Point(106, 156);
			this.SubmitSelectBtn.Name = "SubmitSelectBtn";
			this.SubmitSelectBtn.Size = new System.Drawing.Size(75, 25);
			this.SubmitSelectBtn.TabIndex = 10;
			this.SubmitSelectBtn.Text = "设定选中(&S)";
			this.SubmitSelectBtn.UseVisualStyleBackColor = true;
			this.SubmitSelectBtn.Visible = false;
			this.SubmitSelectBtn.Click += new System.EventHandler(this.SubmitSelectBtn_Click);
			// 
			// SelectHowManyEachTimesBox
			// 
			this.SelectHowManyEachTimesBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectHowManyEachTimesBox.Location = new System.Drawing.Point(106, 127);
			this.SelectHowManyEachTimesBox.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.SelectHowManyEachTimesBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.SelectHowManyEachTimesBox.Name = "SelectHowManyEachTimesBox";
			this.SelectHowManyEachTimesBox.Size = new System.Drawing.Size(75, 23);
			this.SelectHowManyEachTimesBox.TabIndex = 4;
			this.SelectHowManyEachTimesBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// SelectHowManyEachTimesLbl
			// 
			this.SelectHowManyEachTimesLbl.AutoSize = true;
			this.SelectHowManyEachTimesLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectHowManyEachTimesLbl.Location = new System.Drawing.Point(9, 124);
			this.SelectHowManyEachTimesLbl.Name = "SelectHowManyEachTimesLbl";
			this.SelectHowManyEachTimesLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectHowManyEachTimesLbl.TabIndex = 8;
			this.SelectHowManyEachTimesLbl.Text = "每次要选取几个";
			this.SelectHowManyEachTimesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectWhichEachGroupBox
			// 
			this.SelectWhichEachGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectWhichEachGroupBox.Location = new System.Drawing.Point(106, 98);
			this.SelectWhichEachGroupBox.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.SelectWhichEachGroupBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.SelectWhichEachGroupBox.Name = "SelectWhichEachGroupBox";
			this.SelectWhichEachGroupBox.Size = new System.Drawing.Size(75, 23);
			this.SelectWhichEachGroupBox.TabIndex = 3;
			this.SelectWhichEachGroupBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// SelectOneEveryFewLbl
			// 
			this.SelectOneEveryFewLbl.AutoSize = true;
			this.SelectOneEveryFewLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectOneEveryFewLbl.Location = new System.Drawing.Point(9, 66);
			this.SelectOneEveryFewLbl.Name = "SelectOneEveryFewLbl";
			this.SelectOneEveryFewLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectOneEveryFewLbl.TabIndex = 0;
			this.SelectOneEveryFewLbl.Text = "每几个选择一个";
			this.SelectOneEveryFewLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectWhichEachGroupLbl
			// 
			this.SelectWhichEachGroupLbl.AutoSize = true;
			this.SelectWhichEachGroupLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectWhichEachGroupLbl.Location = new System.Drawing.Point(9, 95);
			this.SelectWhichEachGroupLbl.Name = "SelectWhichEachGroupLbl";
			this.SelectWhichEachGroupLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectWhichEachGroupLbl.TabIndex = 1;
			this.SelectWhichEachGroupLbl.Text = "选择每组第几个";
			this.SelectWhichEachGroupLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectOneEveryFewBox
			// 
			this.SelectOneEveryFewBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectOneEveryFewBox.Location = new System.Drawing.Point(106, 69);
			this.SelectOneEveryFewBox.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.SelectOneEveryFewBox.Name = "SelectOneEveryFewBox";
			this.SelectOneEveryFewBox.Size = new System.Drawing.Size(75, 23);
			this.SelectOneEveryFewBox.TabIndex = 2;
			this.SelectOneEveryFewBox.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.SelectOneEveryFewBox.ValueChanged += new System.EventHandler(this.SelectOneEveryFewBox_ValueChanged);
			// 
			// ResetBtn
			// 
			this.ResetBtn.AutoSize = true;
			this.ResetBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResetBtn.Location = new System.Drawing.Point(9, 156);
			this.ResetBtn.MaximumSize = new System.Drawing.Size(0, 25);
			this.ResetBtn.Name = "ResetBtn";
			this.ResetBtn.Size = new System.Drawing.Size(91, 25);
			this.ResetBtn.TabIndex = 5;
			this.ResetBtn.Text = "重置选择(&R)";
			this.ResetBtn.UseVisualStyleBackColor = true;
			this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
			// 
			// SelectIntervalLbl
			// 
			this.SelectIntervalLbl.AutoSize = true;
			this.table.SetColumnSpan(this.SelectIntervalLbl, 2);
			this.SelectIntervalLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectIntervalLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.SelectIntervalLbl.Location = new System.Drawing.Point(9, 12);
			this.SelectIntervalLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.SelectIntervalLbl.Name = "SelectIntervalLbl";
			this.SelectIntervalLbl.Size = new System.Drawing.Size(526, 30);
			this.SelectIntervalLbl.TabIndex = 5;
			this.SelectIntervalLbl.Text = "请先在 Vegas 轨道中选中一些素材，然后再打开本对话框，使用下面的功能。\r\n本功能旨在辅助用户每隔一个或几个选中一个素材，然后可以执行“粘贴视频事件”等操作。" +
    "";
			this.SelectIntervalLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectInfo
			// 
			this.SelectInfo.AutoSize = true;
			this.table.SetColumnSpan(this.SelectInfo, 2);
			this.SelectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectInfo.Location = new System.Drawing.Point(9, 48);
			this.SelectInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.SelectInfo.Name = "SelectInfo";
			this.SelectInfo.Size = new System.Drawing.Size(526, 15);
			this.SelectInfo.TabIndex = 6;
			this.SelectInfo.Text = "已选中 0 个轨道剪辑。";
			this.SelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectIntervalForm
			// 
			this.AcceptButton = this.ApplyBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(544, 271);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectIntervalForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "快速间隔选择";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectHowManyEachTimesBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button ApplyBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel table;
		public System.Windows.Forms.NumericUpDown SelectWhichEachGroupBox;
		public System.Windows.Forms.Label SelectOneEveryFewLbl;
		public System.Windows.Forms.Label SelectWhichEachGroupLbl;
		public System.Windows.Forms.NumericUpDown SelectOneEveryFewBox;
		public System.Windows.Forms.Button ResetBtn;
		public System.Windows.Forms.Label SelectIntervalLbl;
		public System.Windows.Forms.Label SelectInfo;
		public System.Windows.Forms.NumericUpDown SelectHowManyEachTimesBox;
		public System.Windows.Forms.Label SelectHowManyEachTimesLbl;
		public System.Windows.Forms.Button SubmitSelectBtn;
	}
}