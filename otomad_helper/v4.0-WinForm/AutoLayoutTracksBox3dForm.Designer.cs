
namespace Otomad.VegasScripts.OtomadHelper.V4 {
	partial class AutoLayoutTracksBox3dForm {
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
			this.BottomCombo = new System.Windows.Forms.ComboBox();
			this.TopCombo = new System.Windows.Forms.ComboBox();
			this.RightCombo = new System.Windows.Forms.ComboBox();
			this.LeftCombo = new System.Windows.Forms.ComboBox();
			this.BackCombo = new System.Windows.Forms.ComboBox();
			this.BottomLbl = new System.Windows.Forms.Label();
			this.TopLbl = new System.Windows.Forms.Label();
			this.RightLbl = new System.Windows.Forms.Label();
			this.LeftLbl = new System.Windows.Forms.Label();
			this.BackLbl = new System.Windows.Forms.Label();
			this.InfoLbl = new System.Windows.Forms.Label();
			this.FrontLbl = new System.Windows.Forms.Label();
			this.FrontCombo = new System.Windows.Forms.ComboBox();
			this.UseVideoLongerSideCheck = new System.Windows.Forms.CheckBox();
			this.ReverseOrderBtn = new System.Windows.Forms.Button();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.SuspendLayout();
			//
			// OkBtn
			//
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(600, 16);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(6);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(150, 51);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			//
			// CancelBtn
			//
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(762, 16);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(6);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(150, 51);
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
			this.dock.Location = new System.Drawing.Point(0, 691);
			this.dock.Margin = new System.Windows.Forms.Padding(6);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(13, 10, 13, 10);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(931, 83);
			this.dock.TabIndex = 10;
			//
			// table
			//
			this.table.AutoSize = true;
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.BottomCombo, 1, 6);
			this.table.Controls.Add(this.TopCombo, 1, 5);
			this.table.Controls.Add(this.RightCombo, 1, 4);
			this.table.Controls.Add(this.LeftCombo, 1, 3);
			this.table.Controls.Add(this.BackCombo, 1, 2);
			this.table.Controls.Add(this.BottomLbl, 0, 6);
			this.table.Controls.Add(this.TopLbl, 0, 5);
			this.table.Controls.Add(this.RightLbl, 0, 4);
			this.table.Controls.Add(this.LeftLbl, 0, 3);
			this.table.Controls.Add(this.BackLbl, 0, 2);
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.FrontLbl, 0, 1);
			this.table.Controls.Add(this.FrontCombo, 1, 1);
			this.table.Controls.Add(this.UseVideoLongerSideCheck, 0, 7);
			this.table.Controls.Add(this.ReverseOrderBtn, 0, 8);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(8);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(19, 22, 19, 22);
			this.table.RowCount = 9;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(931, 589);
			this.table.TabIndex = 12;
			//
			// BottomCombo
			//
			this.BottomCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BottomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.BottomCombo.FormattingEnabled = true;
			this.BottomCombo.Location = new System.Drawing.Point(99, 410);
			this.BottomCombo.Margin = new System.Windows.Forms.Padding(6);
			this.BottomCombo.Name = "BottomCombo";
			this.BottomCombo.Size = new System.Drawing.Size(807, 40);
			this.BottomCombo.TabIndex = 12;
			//
			// TopCombo
			//
			this.TopCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TopCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TopCombo.FormattingEnabled = true;
			this.TopCombo.Location = new System.Drawing.Point(99, 358);
			this.TopCombo.Margin = new System.Windows.Forms.Padding(6);
			this.TopCombo.Name = "TopCombo";
			this.TopCombo.Size = new System.Drawing.Size(807, 40);
			this.TopCombo.TabIndex = 11;
			//
			// RightCombo
			//
			this.RightCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RightCombo.FormattingEnabled = true;
			this.RightCombo.Location = new System.Drawing.Point(99, 306);
			this.RightCombo.Margin = new System.Windows.Forms.Padding(6);
			this.RightCombo.Name = "RightCombo";
			this.RightCombo.Size = new System.Drawing.Size(807, 40);
			this.RightCombo.TabIndex = 10;
			//
			// LeftCombo
			//
			this.LeftCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LeftCombo.FormattingEnabled = true;
			this.LeftCombo.Location = new System.Drawing.Point(99, 254);
			this.LeftCombo.Margin = new System.Windows.Forms.Padding(6);
			this.LeftCombo.Name = "LeftCombo";
			this.LeftCombo.Size = new System.Drawing.Size(807, 40);
			this.LeftCombo.TabIndex = 9;
			//
			// BackCombo
			//
			this.BackCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.BackCombo.FormattingEnabled = true;
			this.BackCombo.Location = new System.Drawing.Point(99, 202);
			this.BackCombo.Margin = new System.Windows.Forms.Padding(6);
			this.BackCombo.Name = "BackCombo";
			this.BackCombo.Size = new System.Drawing.Size(807, 40);
			this.BackCombo.TabIndex = 8;
			//
			// BottomLbl
			//
			this.BottomLbl.AutoSize = true;
			this.BottomLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BottomLbl.Location = new System.Drawing.Point(25, 404);
			this.BottomLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.BottomLbl.Name = "BottomLbl";
			this.BottomLbl.Size = new System.Drawing.Size(62, 52);
			this.BottomLbl.TabIndex = 6;
			this.BottomLbl.Text = "底面";
			this.BottomLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// TopLbl
			//
			this.TopLbl.AutoSize = true;
			this.TopLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TopLbl.Location = new System.Drawing.Point(25, 352);
			this.TopLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.TopLbl.Name = "TopLbl";
			this.TopLbl.Size = new System.Drawing.Size(62, 52);
			this.TopLbl.TabIndex = 5;
			this.TopLbl.Text = "顶面";
			this.TopLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// RightLbl
			//
			this.RightLbl.AutoSize = true;
			this.RightLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightLbl.Location = new System.Drawing.Point(25, 300);
			this.RightLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.RightLbl.Name = "RightLbl";
			this.RightLbl.Size = new System.Drawing.Size(62, 52);
			this.RightLbl.TabIndex = 4;
			this.RightLbl.Text = "右面";
			this.RightLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// LeftLbl
			//
			this.LeftLbl.AutoSize = true;
			this.LeftLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftLbl.Location = new System.Drawing.Point(25, 248);
			this.LeftLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.LeftLbl.Name = "LeftLbl";
			this.LeftLbl.Size = new System.Drawing.Size(62, 52);
			this.LeftLbl.TabIndex = 3;
			this.LeftLbl.Text = "左面";
			this.LeftLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// BackLbl
			//
			this.BackLbl.AutoSize = true;
			this.BackLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackLbl.Location = new System.Drawing.Point(25, 196);
			this.BackLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.BackLbl.Name = "BackLbl";
			this.BackLbl.Size = new System.Drawing.Size(62, 52);
			this.BackLbl.TabIndex = 2;
			this.BackLbl.Text = "后面";
			this.BackLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// InfoLbl
			//
			this.InfoLbl.AutoSize = true;
			this.table.SetColumnSpan(this.InfoLbl, 2);
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Location = new System.Drawing.Point(25, 35);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(6, 13, 6, 13);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(881, 96);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "由于脚本功能限制，将会新建轨道并将选定轨道中的剪辑移动过去，原轨道中的轨道运动、效果等信息将会丢失。\r\n请在下方选定立方体的各个面所使用的轨道，如果为空则表示不设" +
	"定该面。";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// FrontLbl
			//
			this.FrontLbl.AutoSize = true;
			this.FrontLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FrontLbl.Location = new System.Drawing.Point(25, 144);
			this.FrontLbl.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.FrontLbl.Name = "FrontLbl";
			this.FrontLbl.Size = new System.Drawing.Size(62, 52);
			this.FrontLbl.TabIndex = 1;
			this.FrontLbl.Text = "前面";
			this.FrontLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			// FrontCombo
			//
			this.FrontCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FrontCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FrontCombo.FormattingEnabled = true;
			this.FrontCombo.Location = new System.Drawing.Point(99, 150);
			this.FrontCombo.Margin = new System.Windows.Forms.Padding(6);
			this.FrontCombo.Name = "FrontCombo";
			this.FrontCombo.Size = new System.Drawing.Size(807, 40);
			this.FrontCombo.TabIndex = 7;
			//
			// UseVideoLongerSideCheck
			//
			this.UseVideoLongerSideCheck.AutoSize = true;
			this.table.SetColumnSpan(this.UseVideoLongerSideCheck, 2);
			this.UseVideoLongerSideCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UseVideoLongerSideCheck.Location = new System.Drawing.Point(25, 468);
			this.UseVideoLongerSideCheck.Margin = new System.Windows.Forms.Padding(6, 12, 6, 6);
			this.UseVideoLongerSideCheck.Name = "UseVideoLongerSideCheck";
			this.UseVideoLongerSideCheck.Size = new System.Drawing.Size(881, 36);
			this.UseVideoLongerSideCheck.TabIndex = 14;
			this.UseVideoLongerSideCheck.Text = "使用视频的长边作为立方体的棱长";
			this.UseVideoLongerSideCheck.UseVisualStyleBackColor = true;
			//
			// ReverseOrderBtn
			//
			this.table.SetColumnSpan(this.ReverseOrderBtn, 2);
			this.ReverseOrderBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.ReverseOrderBtn.Location = new System.Drawing.Point(22, 513);
			this.ReverseOrderBtn.MaximumSize = new System.Drawing.Size(99999, 51);
			this.ReverseOrderBtn.MinimumSize = new System.Drawing.Size(150, 51);
			this.ReverseOrderBtn.Name = "ReverseOrderBtn";
			this.ReverseOrderBtn.Size = new System.Drawing.Size(175, 51);
			this.ReverseOrderBtn.TabIndex = 15;
			this.ReverseOrderBtn.Text = "反转顺序";
			this.ReverseOrderBtn.UseVisualStyleBackColor = true;
			this.ReverseOrderBtn.Click += new System.EventHandler(this.ReverseOrderBtn_Click);
			//
			// AutoLayoutTracksBox3dForm
			//
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(931, 774);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(8);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AutoLayoutTracksBox3dForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自动布局轨道 - 3D 方盒布局";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.ComboBox BottomCombo;
		private System.Windows.Forms.ComboBox TopCombo;
		private System.Windows.Forms.ComboBox RightCombo;
		private System.Windows.Forms.ComboBox LeftCombo;
		private System.Windows.Forms.ComboBox BackCombo;
		private System.Windows.Forms.Label BottomLbl;
		private System.Windows.Forms.Label TopLbl;
		private System.Windows.Forms.Label RightLbl;
		private System.Windows.Forms.Label LeftLbl;
		private System.Windows.Forms.Label BackLbl;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.Label FrontLbl;
		private System.Windows.Forms.ComboBox FrontCombo;
		private System.Windows.Forms.CheckBox UseVideoLongerSideCheck;
		private System.Windows.Forms.Button ReverseOrderBtn;
	}
}
