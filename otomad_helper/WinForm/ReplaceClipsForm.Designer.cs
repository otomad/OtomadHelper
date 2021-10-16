
namespace VegasScript {
	partial class ReplaceClipsForm {
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
			this.ReplacerLbl = new System.Windows.Forms.Label();
			this.ReplaceClipsLbl = new System.Windows.Forms.Label();
			this.ReplacerCombo = new System.Windows.Forms.ComboBox();
			this.ReplacedLbl = new System.Windows.Forms.Label();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
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
			this.dock.Location = new System.Drawing.Point(0, 189);
			this.dock.Margin = new System.Windows.Forms.Padding(4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(534, 42);
			this.dock.TabIndex = 3;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(369, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "替换(&R)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(450, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 522F));
			this.table.Controls.Add(this.ReplacerLbl, 0, 1);
			this.table.Controls.Add(this.ReplaceClipsLbl, 0, 0);
			this.table.Controls.Add(this.ReplacerCombo, 0, 2);
			this.table.Controls.Add(this.ReplacedLbl, 0, 3);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(6);
			this.table.RowCount = 4;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(534, 149);
			this.table.TabIndex = 6;
			// 
			// ReplacerLbl
			// 
			this.ReplacerLbl.AutoSize = true;
			this.ReplacerLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerLbl.Location = new System.Drawing.Point(9, 78);
			this.ReplacerLbl.Name = "ReplacerLbl";
			this.ReplacerLbl.Size = new System.Drawing.Size(516, 15);
			this.ReplacerLbl.TabIndex = 4;
			this.ReplacerLbl.Text = "指定的替换项为";
			this.ReplacerLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplaceClipsLbl
			// 
			this.ReplaceClipsLbl.AutoSize = true;
			this.ReplaceClipsLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplaceClipsLbl.Location = new System.Drawing.Point(9, 12);
			this.ReplaceClipsLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.ReplaceClipsLbl.Name = "ReplaceClipsLbl";
			this.ReplaceClipsLbl.Size = new System.Drawing.Size(516, 60);
			this.ReplaceClipsLbl.TabIndex = 1;
			this.ReplaceClipsLbl.Text = "请先在轨道窗口中选中替换与被替换的素材，然后指定一个素材为替换的素材，剩余素材均为被替换素材。\r\n由于脚本功能限制，无法单独分离指定替换素材。请先将替换素材的音视" +
    "频创建分组，并确保替换素材不与其它被替换素材位于同一轨道并且尽量放置在时间靠后的位置。";
			this.ReplaceClipsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplacerCombo
			// 
			this.ReplacerCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ReplacerCombo.FormattingEnabled = true;
			this.ReplacerCombo.Location = new System.Drawing.Point(12, 99);
			this.ReplacerCombo.Margin = new System.Windows.Forms.Padding(6);
			this.ReplacerCombo.Name = "ReplacerCombo";
			this.ReplacerCombo.Size = new System.Drawing.Size(510, 23);
			this.ReplacerCombo.TabIndex = 2;
			this.ReplacerCombo.SelectedIndexChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ReplacedLbl
			// 
			this.ReplacedLbl.AutoSize = true;
			this.ReplacedLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacedLbl.Location = new System.Drawing.Point(9, 128);
			this.ReplacedLbl.Name = "ReplacedLbl";
			this.ReplacedLbl.Size = new System.Drawing.Size(516, 15);
			this.ReplacedLbl.TabIndex = 3;
			this.ReplacedLbl.Text = "则剩余 0 项轨道剪辑将被替换为选定素材。";
			this.ReplacedLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplaceClipsForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(534, 231);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ReplaceClipsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "替换轨道素材";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label ReplaceClipsLbl;
		private System.Windows.Forms.Label ReplacerLbl;
		private System.Windows.Forms.ComboBox ReplacerCombo;
		private System.Windows.Forms.Label ReplacedLbl;
	}
}