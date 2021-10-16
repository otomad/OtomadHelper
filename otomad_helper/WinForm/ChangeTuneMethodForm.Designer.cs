
namespace VegasScript {
	partial class ChangeTuneMethodForm {
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
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLbl = new System.Windows.Forms.Label();
			this.TimeStretchPitchShiftGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.FormantLockCheck = new System.Windows.Forms.CheckBox();
			this.StretchAttrCombo = new System.Windows.Forms.ComboBox();
			this.FormantChangeLbl = new System.Windows.Forms.Label();
			this.PitchChangeLbl = new System.Windows.Forms.Label();
			this.StretchAttrLbl = new System.Windows.Forms.Label();
			this.MethodLbl = new System.Windows.Forms.Label();
			this.MethodCombo = new System.Windows.Forms.ComboBox();
			this.PitchLockCheck = new System.Windows.Forms.CheckBox();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.TimeStretchPitchShiftGroup.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(399, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(480, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
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
			this.dock.Location = new System.Drawing.Point(0, 219);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(564, 42);
			this.dock.TabIndex = 15;
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.TimeStretchPitchShiftGroup, 0, 1);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
			this.table.RowCount = 2;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(564, 197);
			this.table.TabIndex = 17;
			// 
			// InfoLbl
			// 
			this.InfoLbl.AutoSize = true;
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.InfoLbl.Location = new System.Drawing.Point(10, 17);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this.InfoLbl.MaximumSize = new System.Drawing.Size(520, 0);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(520, 15);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "仅支持音频事件属性中的调音方法，不支持“移调”插件中的调音方法。";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TimeStretchPitchShiftGroup
			// 
			this.TimeStretchPitchShiftGroup.AutoSize = true;
			this.TimeStretchPitchShiftGroup.Controls.Add(this.tableLayoutPanel2);
			this.TimeStretchPitchShiftGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TimeStretchPitchShiftGroup.Location = new System.Drawing.Point(13, 41);
			this.TimeStretchPitchShiftGroup.Name = "TimeStretchPitchShiftGroup";
			this.TimeStretchPitchShiftGroup.Padding = new System.Windows.Forms.Padding(5);
			this.TimeStretchPitchShiftGroup.Size = new System.Drawing.Size(538, 142);
			this.TimeStretchPitchShiftGroup.TabIndex = 1;
			this.TimeStretchPitchShiftGroup.TabStop = false;
			this.TimeStretchPitchShiftGroup.Text = "时间拉伸/音调转换";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.FormantLockCheck, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.StretchAttrCombo, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.FormantChangeLbl, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.PitchChangeLbl, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.StretchAttrLbl, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.MethodLbl, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.MethodCombo, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.PitchLockCheck, 1, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(528, 116);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// FormantLockCheck
			// 
			this.FormantLockCheck.AutoSize = true;
			this.FormantLockCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FormantLockCheck.Location = new System.Drawing.Point(76, 90);
			this.FormantLockCheck.Name = "FormantLockCheck";
			this.FormantLockCheck.Size = new System.Drawing.Size(449, 23);
			this.FormantLockCheck.TabIndex = 7;
			this.FormantLockCheck.Text = "保留共振峰";
			this.FormantLockCheck.UseVisualStyleBackColor = true;
			// 
			// StretchAttrCombo
			// 
			this.StretchAttrCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StretchAttrCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.StretchAttrCombo.FormattingEnabled = true;
			this.StretchAttrCombo.Location = new System.Drawing.Point(76, 32);
			this.StretchAttrCombo.Name = "StretchAttrCombo";
			this.StretchAttrCombo.Size = new System.Drawing.Size(449, 23);
			this.StretchAttrCombo.TabIndex = 5;
			this.StretchAttrCombo.SelectedIndexChanged += new System.EventHandler(this.MethodCombo_SelectedIndexChanged);
			// 
			// FormantChangeLbl
			// 
			this.FormantChangeLbl.AutoSize = true;
			this.FormantChangeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FormantChangeLbl.Location = new System.Drawing.Point(3, 87);
			this.FormantChangeLbl.Name = "FormantChangeLbl";
			this.FormantChangeLbl.Size = new System.Drawing.Size(67, 29);
			this.FormantChangeLbl.TabIndex = 3;
			this.FormantChangeLbl.Text = "共振峰移位";
			this.FormantChangeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PitchChangeLbl
			// 
			this.PitchChangeLbl.AutoSize = true;
			this.PitchChangeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PitchChangeLbl.Location = new System.Drawing.Point(3, 58);
			this.PitchChangeLbl.Name = "PitchChangeLbl";
			this.PitchChangeLbl.Size = new System.Drawing.Size(67, 29);
			this.PitchChangeLbl.TabIndex = 2;
			this.PitchChangeLbl.Text = "音调更改";
			this.PitchChangeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StretchAttrLbl
			// 
			this.StretchAttrLbl.AutoSize = true;
			this.StretchAttrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StretchAttrLbl.Location = new System.Drawing.Point(3, 29);
			this.StretchAttrLbl.Name = "StretchAttrLbl";
			this.StretchAttrLbl.Size = new System.Drawing.Size(67, 29);
			this.StretchAttrLbl.TabIndex = 1;
			this.StretchAttrLbl.Text = "拉伸属性";
			this.StretchAttrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MethodLbl
			// 
			this.MethodLbl.AutoSize = true;
			this.MethodLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MethodLbl.Location = new System.Drawing.Point(3, 0);
			this.MethodLbl.Name = "MethodLbl";
			this.MethodLbl.Size = new System.Drawing.Size(67, 29);
			this.MethodLbl.TabIndex = 0;
			this.MethodLbl.Text = "方法";
			this.MethodLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MethodCombo
			// 
			this.MethodCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MethodCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MethodCombo.FormattingEnabled = true;
			this.MethodCombo.Items.AddRange(new object[] {
            "无",
            "élastique",
            "古典"});
			this.MethodCombo.Location = new System.Drawing.Point(76, 3);
			this.MethodCombo.Name = "MethodCombo";
			this.MethodCombo.Size = new System.Drawing.Size(449, 23);
			this.MethodCombo.TabIndex = 4;
			this.MethodCombo.SelectedIndexChanged += new System.EventHandler(this.MethodCombo_SelectedIndexChanged);
			// 
			// PitchLockCheck
			// 
			this.PitchLockCheck.AutoSize = true;
			this.PitchLockCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PitchLockCheck.Location = new System.Drawing.Point(76, 61);
			this.PitchLockCheck.Name = "PitchLockCheck";
			this.PitchLockCheck.Size = new System.Drawing.Size(449, 23);
			this.PitchLockCheck.TabIndex = 6;
			this.PitchLockCheck.Text = "锁定以拉伸";
			this.PitchLockCheck.UseVisualStyleBackColor = true;
			this.PitchLockCheck.CheckedChanged += new System.EventHandler(this.MethodCombo_SelectedIndexChanged);
			// 
			// ChangeTuneMethodForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(564, 261);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangeTuneMethodForm";
			this.ShowInTaskbar = false;
			this.Text = "更改调音算法";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.TimeStretchPitchShiftGroup.ResumeLayout(false);
			this.TimeStretchPitchShiftGroup.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.GroupBox TimeStretchPitchShiftGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox FormantLockCheck;
		private System.Windows.Forms.ComboBox StretchAttrCombo;
		private System.Windows.Forms.Label FormantChangeLbl;
		private System.Windows.Forms.Label PitchChangeLbl;
		private System.Windows.Forms.Label StretchAttrLbl;
		private System.Windows.Forms.Label MethodLbl;
		private System.Windows.Forms.ComboBox MethodCombo;
		private System.Windows.Forms.CheckBox PitchLockCheck;
	}
}