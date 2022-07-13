
namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class AutoLayoutTracksGridForm {
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
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.SquareRadio = new System.Windows.Forms.RadioButton();
			this.CustomRadio = new System.Windows.Forms.RadioButton();
			this.CustomGroup = new System.Windows.Forms.GroupBox();
			this.CustomTable = new System.Windows.Forms.TableLayoutPanel();
			this.RowCountBox = new System.Windows.Forms.NumericUpDown();
			this.RowCountLbl = new System.Windows.Forms.Label();
			this.ColumnCountLbl = new System.Windows.Forms.Label();
			this.ColumnCountBox = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.FillRadio = new System.Windows.Forms.RadioButton();
			this.AdaptRadio = new System.Windows.Forms.RadioButton();
			this.ReverseTracksCheck = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.PaddingLbl = new System.Windows.Forms.Label();
			this.PaddingBox = new System.Windows.Forms.NumericUpDown();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.CustomGroup.SuspendLayout();
			this.CustomTable.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RowCountBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ColumnCountBox)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PaddingBox)).BeginInit();
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
			this.dock.Location = new System.Drawing.Point(0, 306);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(355, 52);
			this.dock.TabIndex = 7;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(147, 10);
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
			this.CancelBtn.Location = new System.Drawing.Point(249, 10);
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
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.SquareRadio, 0, 0);
			this.table.Controls.Add(this.CustomRadio, 0, 1);
			this.table.Controls.Add(this.CustomGroup, 0, 2);
			this.table.Controls.Add(this.ReverseTracksCheck, 0, 3);
			this.table.Controls.Add(this.tableLayoutPanel4, 0, 4);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(11);
			this.table.RowCount = 5;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.table.Size = new System.Drawing.Size(355, 303);
			this.table.TabIndex = 9;
			// 
			// SquareRadio
			// 
			this.SquareRadio.AutoSize = true;
			this.SquareRadio.Checked = true;
			this.SquareRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SquareRadio.Location = new System.Drawing.Point(15, 15);
			this.SquareRadio.Margin = new System.Windows.Forms.Padding(4);
			this.SquareRadio.Name = "SquareRadio";
			this.SquareRadio.Size = new System.Drawing.Size(325, 24);
			this.SquareRadio.TabIndex = 0;
			this.SquareRadio.TabStop = true;
			this.SquareRadio.Text = "平方";
			this.SquareRadio.UseVisualStyleBackColor = true;
			this.SquareRadio.CheckedChanged += new System.EventHandler(this.CustomRadio_CheckedChanged);
			// 
			// CustomRadio
			// 
			this.CustomRadio.AutoSize = true;
			this.CustomRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CustomRadio.Location = new System.Drawing.Point(15, 47);
			this.CustomRadio.Margin = new System.Windows.Forms.Padding(4);
			this.CustomRadio.Name = "CustomRadio";
			this.CustomRadio.Size = new System.Drawing.Size(325, 24);
			this.CustomRadio.TabIndex = 1;
			this.CustomRadio.Text = "自定义";
			this.CustomRadio.UseVisualStyleBackColor = true;
			this.CustomRadio.CheckedChanged += new System.EventHandler(this.CustomRadio_CheckedChanged);
			// 
			// CustomGroup
			// 
			this.CustomGroup.AutoSize = true;
			this.CustomGroup.Controls.Add(this.CustomTable);
			this.CustomGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.CustomGroup.Location = new System.Drawing.Point(15, 79);
			this.CustomGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 8);
			this.CustomGroup.Name = "CustomGroup";
			this.CustomGroup.Padding = new System.Windows.Forms.Padding(4);
			this.CustomGroup.Size = new System.Drawing.Size(325, 138);
			this.CustomGroup.TabIndex = 2;
			this.CustomGroup.TabStop = false;
			// 
			// CustomTable
			// 
			this.CustomTable.AutoSize = true;
			this.CustomTable.ColumnCount = 2;
			this.CustomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.CustomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.CustomTable.Controls.Add(this.RowCountBox, 1, 1);
			this.CustomTable.Controls.Add(this.RowCountLbl, 0, 1);
			this.CustomTable.Controls.Add(this.ColumnCountLbl, 0, 0);
			this.CustomTable.Controls.Add(this.ColumnCountBox, 1, 0);
			this.CustomTable.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.CustomTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.CustomTable.Location = new System.Drawing.Point(4, 24);
			this.CustomTable.Margin = new System.Windows.Forms.Padding(4);
			this.CustomTable.Name = "CustomTable";
			this.CustomTable.RowCount = 3;
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.Size = new System.Drawing.Size(317, 110);
			this.CustomTable.TabIndex = 2;
			// 
			// RowCountBox
			// 
			this.RowCountBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.RowCountBox.Enabled = false;
			this.RowCountBox.Location = new System.Drawing.Point(51, 39);
			this.RowCountBox.Margin = new System.Windows.Forms.Padding(4);
			this.RowCountBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.RowCountBox.Name = "RowCountBox";
			this.RowCountBox.ReadOnly = true;
			this.RowCountBox.Size = new System.Drawing.Size(94, 27);
			this.RowCountBox.TabIndex = 3;
			this.RowCountBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// RowCountLbl
			// 
			this.RowCountLbl.AutoSize = true;
			this.RowCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RowCountLbl.Location = new System.Drawing.Point(4, 35);
			this.RowCountLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.RowCountLbl.Name = "RowCountLbl";
			this.RowCountLbl.Size = new System.Drawing.Size(39, 35);
			this.RowCountLbl.TabIndex = 1;
			this.RowCountLbl.Text = "行数";
			this.RowCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColumnCountLbl
			// 
			this.ColumnCountLbl.AutoSize = true;
			this.ColumnCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ColumnCountLbl.Location = new System.Drawing.Point(4, 0);
			this.ColumnCountLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ColumnCountLbl.Name = "ColumnCountLbl";
			this.ColumnCountLbl.Size = new System.Drawing.Size(39, 35);
			this.ColumnCountLbl.TabIndex = 0;
			this.ColumnCountLbl.Text = "列数";
			this.ColumnCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColumnCountBox
			// 
			this.ColumnCountBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.ColumnCountBox.Location = new System.Drawing.Point(51, 4);
			this.ColumnCountBox.Margin = new System.Windows.Forms.Padding(4);
			this.ColumnCountBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ColumnCountBox.Name = "ColumnCountBox";
			this.ColumnCountBox.Size = new System.Drawing.Size(94, 27);
			this.ColumnCountBox.TabIndex = 2;
			this.ColumnCountBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.ColumnCountBox.ValueChanged += new System.EventHandler(this.ColumnCountBox_ValueChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.CustomTable.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.FillRadio);
			this.flowLayoutPanel1.Controls.Add(this.AdaptRadio);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 74);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(309, 32);
			this.flowLayoutPanel1.TabIndex = 4;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// FillRadio
			// 
			this.FillRadio.AutoSize = true;
			this.FillRadio.Checked = true;
			this.FillRadio.Location = new System.Drawing.Point(4, 4);
			this.FillRadio.Margin = new System.Windows.Forms.Padding(4);
			this.FillRadio.Name = "FillRadio";
			this.FillRadio.Size = new System.Drawing.Size(60, 24);
			this.FillRadio.TabIndex = 0;
			this.FillRadio.TabStop = true;
			this.FillRadio.Text = "填充";
			this.FillRadio.UseVisualStyleBackColor = true;
			// 
			// AdaptRadio
			// 
			this.AdaptRadio.AutoSize = true;
			this.AdaptRadio.Location = new System.Drawing.Point(72, 4);
			this.AdaptRadio.Margin = new System.Windows.Forms.Padding(4);
			this.AdaptRadio.Name = "AdaptRadio";
			this.AdaptRadio.Size = new System.Drawing.Size(60, 24);
			this.AdaptRadio.TabIndex = 1;
			this.AdaptRadio.Text = "适应";
			this.AdaptRadio.UseVisualStyleBackColor = true;
			// 
			// ReverseTracksCheck
			// 
			this.ReverseTracksCheck.AutoSize = true;
			this.ReverseTracksCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReverseTracksCheck.Location = new System.Drawing.Point(15, 229);
			this.ReverseTracksCheck.Margin = new System.Windows.Forms.Padding(4);
			this.ReverseTracksCheck.Name = "ReverseTracksCheck";
			this.ReverseTracksCheck.Size = new System.Drawing.Size(325, 24);
			this.ReverseTracksCheck.TabIndex = 3;
			this.ReverseTracksCheck.Text = "降序排序";
			this.ReverseTracksCheck.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.PaddingLbl, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.PaddingBox, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(11, 257);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(333, 35);
			this.tableLayoutPanel4.TabIndex = 4;
			// 
			// PaddingLbl
			// 
			this.PaddingLbl.AutoSize = true;
			this.PaddingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PaddingLbl.Location = new System.Drawing.Point(4, 0);
			this.PaddingLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PaddingLbl.Name = "PaddingLbl";
			this.PaddingLbl.Size = new System.Drawing.Size(69, 35);
			this.PaddingLbl.TabIndex = 0;
			this.PaddingLbl.Text = "增加边距";
			this.PaddingLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PaddingBox
			// 
			this.PaddingBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.PaddingBox.Location = new System.Drawing.Point(81, 4);
			this.PaddingBox.Margin = new System.Windows.Forms.Padding(4);
			this.PaddingBox.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
			this.PaddingBox.Name = "PaddingBox";
			this.PaddingBox.Size = new System.Drawing.Size(94, 27);
			this.PaddingBox.TabIndex = 1;
			// 
			// AutoLayoutTracksGridForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(355, 358);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AutoLayoutTracksGridForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自动布局轨道 - 网格布局";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.CustomGroup.ResumeLayout(false);
			this.CustomGroup.PerformLayout();
			this.CustomTable.ResumeLayout(false);
			this.CustomTable.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RowCountBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ColumnCountBox)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PaddingBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.RadioButton SquareRadio;
		private System.Windows.Forms.RadioButton CustomRadio;
		private System.Windows.Forms.GroupBox CustomGroup;
		private System.Windows.Forms.TableLayoutPanel CustomTable;
		private System.Windows.Forms.NumericUpDown RowCountBox;
		private System.Windows.Forms.Label RowCountLbl;
		private System.Windows.Forms.Label ColumnCountLbl;
		private System.Windows.Forms.NumericUpDown ColumnCountBox;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.RadioButton FillRadio;
		private System.Windows.Forms.RadioButton AdaptRadio;
		private System.Windows.Forms.CheckBox ReverseTracksCheck;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label PaddingLbl;
		private System.Windows.Forms.NumericUpDown PaddingBox;
	}
}