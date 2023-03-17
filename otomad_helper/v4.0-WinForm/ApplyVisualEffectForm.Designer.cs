namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class ApplyVisualEffectForm {
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.VideoEffectsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.VideoEffectLbl = new System.Windows.Forms.Label();
			this.VideoEffectInitialValueLbl = new System.Windows.Forms.Label();
			this.VideoEffectCombo = new System.Windows.Forms.ComboBox();
			this.VideoEffectInitialValueCombo = new System.Windows.Forms.ComboBox();
			this.dock.SuspendLayout();
			this.panel1.SuspendLayout();
			this.VideoEffectsGroup.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
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
			this.dock.Location = new System.Drawing.Point(0, 122);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(519, 52);
			this.dock.TabIndex = 8;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(311, 10);
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
			this.CancelBtn.Location = new System.Drawing.Point(413, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.VideoEffectsGroup);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10);
			this.panel1.Size = new System.Drawing.Size(519, 122);
			this.panel1.TabIndex = 9;
			// 
			// VideoEffectsGroup
			// 
			this.VideoEffectsGroup.AutoSize = true;
			this.VideoEffectsGroup.Controls.Add(this.tableLayoutPanel8);
			this.VideoEffectsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.VideoEffectsGroup.Location = new System.Drawing.Point(10, 10);
			this.VideoEffectsGroup.Name = "VideoEffectsGroup";
			this.VideoEffectsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.VideoEffectsGroup.Size = new System.Drawing.Size(499, 98);
			this.VideoEffectsGroup.TabIndex = 10;
			this.VideoEffectsGroup.TabStop = false;
			this.VideoEffectsGroup.Text = "效果";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectLbl, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectInitialValueLbl, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectCombo, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectInitialValueCombo, 1, 1);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 2;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(489, 68);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// VideoEffectLbl
			// 
			this.VideoEffectLbl.AutoSize = true;
			this.VideoEffectLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEffectLbl.Location = new System.Drawing.Point(2, 0);
			this.VideoEffectLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEffectLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.VideoEffectLbl.Name = "VideoEffectLbl";
			this.VideoEffectLbl.Size = new System.Drawing.Size(69, 34);
			this.VideoEffectLbl.TabIndex = 0;
			this.VideoEffectLbl.Text = "视觉效果";
			this.VideoEffectLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEffectInitialValueLbl
			// 
			this.VideoEffectInitialValueLbl.AutoSize = true;
			this.VideoEffectInitialValueLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEffectInitialValueLbl.Location = new System.Drawing.Point(2, 34);
			this.VideoEffectInitialValueLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEffectInitialValueLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.VideoEffectInitialValueLbl.Name = "VideoEffectInitialValueLbl";
			this.VideoEffectInitialValueLbl.Size = new System.Drawing.Size(69, 34);
			this.VideoEffectInitialValueLbl.TabIndex = 1;
			this.VideoEffectInitialValueLbl.Text = "初始值";
			this.VideoEffectInitialValueLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEffectCombo
			// 
			this.VideoEffectCombo.Dock = System.Windows.Forms.DockStyle.Top;
			this.VideoEffectCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoEffectCombo.FormattingEnabled = true;
			this.VideoEffectCombo.Location = new System.Drawing.Point(75, 2);
			this.VideoEffectCombo.Margin = new System.Windows.Forms.Padding(2);
			this.VideoEffectCombo.Name = "VideoEffectCombo";
			this.VideoEffectCombo.Size = new System.Drawing.Size(412, 28);
			this.VideoEffectCombo.TabIndex = 2;
			this.VideoEffectCombo.SelectedIndexChanged += new System.EventHandler(this.VideoEffectCombo_SelectedIndexChanged);
			// 
			// VideoEffectInitialValueCombo
			// 
			this.VideoEffectInitialValueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoEffectInitialValueCombo.FormattingEnabled = true;
			this.VideoEffectInitialValueCombo.Location = new System.Drawing.Point(75, 36);
			this.VideoEffectInitialValueCombo.Margin = new System.Windows.Forms.Padding(2);
			this.VideoEffectInitialValueCombo.Name = "VideoEffectInitialValueCombo";
			this.VideoEffectInitialValueCombo.Size = new System.Drawing.Size(90, 28);
			this.VideoEffectInitialValueCombo.TabIndex = 3;
			// 
			// ApplyVisualEffectForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(519, 174);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ApplyVisualEffectForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "应用视觉效果";
			this.dock.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.VideoEffectsGroup.ResumeLayout(false);
			this.VideoEffectsGroup.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Panel panel1;
		public System.Windows.Forms.GroupBox VideoEffectsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		public System.Windows.Forms.Label VideoEffectLbl;
		public System.Windows.Forms.Label VideoEffectInitialValueLbl;
		public System.Windows.Forms.ComboBox VideoEffectCombo;
		public System.Windows.Forms.ComboBox VideoEffectInitialValueCombo;
	}
}