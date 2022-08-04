namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class FindClipsForm {
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
			this.MatchSourceAndOffsetRadio = new System.Windows.Forms.RadioButton();
			this.MatchSourceRadio = new System.Windows.Forms.RadioButton();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.MatchNameRadio = new System.Windows.Forms.RadioButton();
			this.SelectInfo = new System.Windows.Forms.Label();
			this.ClipNameTxt = new System.Windows.Forms.TextBox();
			this.ClipNameList = new System.Windows.Forms.ListView();
			this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.numHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.FindClipsInfo = new System.Windows.Forms.Label();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table.SuspendLayout();
			this.dock.SuspendLayout();
			this.SuspendLayout();
			// 
			// MatchSourceAndOffsetRadio
			// 
			this.MatchSourceAndOffsetRadio.AutoSize = true;
			this.MatchSourceAndOffsetRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MatchSourceAndOffsetRadio.Location = new System.Drawing.Point(15, 83);
			this.MatchSourceAndOffsetRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MatchSourceAndOffsetRadio.Name = "MatchSourceAndOffsetRadio";
			this.MatchSourceAndOffsetRadio.Size = new System.Drawing.Size(559, 24);
			this.MatchSourceAndOffsetRadio.TabIndex = 1;
			this.MatchSourceAndOffsetRadio.Text = "与选中轨道剪辑相同且开始偏移量相等的所有剪辑";
			this.MatchSourceAndOffsetRadio.UseVisualStyleBackColor = true;
			this.MatchSourceAndOffsetRadio.CheckedChanged += new System.EventHandler(this.SetEnabled);
			// 
			// MatchSourceRadio
			// 
			this.MatchSourceRadio.AutoSize = true;
			this.MatchSourceRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MatchSourceRadio.Location = new System.Drawing.Point(15, 51);
			this.MatchSourceRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MatchSourceRadio.Name = "MatchSourceRadio";
			this.MatchSourceRadio.Size = new System.Drawing.Size(559, 24);
			this.MatchSourceRadio.TabIndex = 0;
			this.MatchSourceRadio.Text = "与选中轨道剪辑相同的所有剪辑";
			this.MatchSourceRadio.UseVisualStyleBackColor = true;
			this.MatchSourceRadio.CheckedChanged += new System.EventHandler(this.SetEnabled);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.MatchSourceRadio, 0, 1);
			this.table.Controls.Add(this.MatchSourceAndOffsetRadio, 0, 2);
			this.table.Controls.Add(this.MatchNameRadio, 0, 3);
			this.table.Controls.Add(this.SelectInfo, 0, 0);
			this.table.Controls.Add(this.ClipNameTxt, 0, 4);
			this.table.Controls.Add(this.ClipNameList, 0, 5);
			this.table.Controls.Add(this.FindClipsInfo, 0, 6);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(11, 11, 11, 11);
			this.table.RowCount = 7;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(589, 622);
			this.table.TabIndex = 11;
			// 
			// MatchNameRadio
			// 
			this.MatchNameRadio.AutoSize = true;
			this.MatchNameRadio.Checked = true;
			this.MatchNameRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MatchNameRadio.Location = new System.Drawing.Point(15, 115);
			this.MatchNameRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MatchNameRadio.Name = "MatchNameRadio";
			this.MatchNameRadio.Size = new System.Drawing.Size(559, 24);
			this.MatchNameRadio.TabIndex = 2;
			this.MatchNameRadio.TabStop = true;
			this.MatchNameRadio.Text = "与指定名称相匹配的剪辑";
			this.MatchNameRadio.UseVisualStyleBackColor = true;
			this.MatchNameRadio.CheckedChanged += new System.EventHandler(this.SetEnabled);
			// 
			// SelectInfo
			// 
			this.SelectInfo.AutoSize = true;
			this.SelectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectInfo.Location = new System.Drawing.Point(11, 19);
			this.SelectInfo.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
			this.SelectInfo.Name = "SelectInfo";
			this.SelectInfo.Size = new System.Drawing.Size(567, 20);
			this.SelectInfo.TabIndex = 3;
			this.SelectInfo.Text = "选中的第一个轨道剪辑：无";
			this.SelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ClipNameTxt
			// 
			this.ClipNameTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClipNameTxt.Location = new System.Drawing.Point(15, 151);
			this.ClipNameTxt.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
			this.ClipNameTxt.Name = "ClipNameTxt";
			this.ClipNameTxt.Size = new System.Drawing.Size(559, 27);
			this.ClipNameTxt.TabIndex = 4;
			this.ClipNameTxt.Click += new System.EventHandler(this.AutoSelectMatchName);
			this.ClipNameTxt.TextChanged += new System.EventHandler(this.ClipNameTxt_TextChanged);
			// 
			// ClipNameList
			// 
			this.ClipNameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.numHeader});
			this.ClipNameList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClipNameList.FullRowSelect = true;
			this.ClipNameList.HideSelection = false;
			this.ClipNameList.Location = new System.Drawing.Point(15, 186);
			this.ClipNameList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ClipNameList.Name = "ClipNameList";
			this.ClipNameList.Size = new System.Drawing.Size(559, 393);
			this.ClipNameList.TabIndex = 5;
			this.ClipNameList.UseCompatibleStateImageBehavior = false;
			this.ClipNameList.View = System.Windows.Forms.View.Details;
			this.ClipNameList.SelectedIndexChanged += new System.EventHandler(this.AutoSelectMatchName);
			// 
			// nameHeader
			// 
			this.nameHeader.Text = "名称";
			this.nameHeader.Width = 300;
			// 
			// numHeader
			// 
			this.numHeader.Text = "数目";
			this.numHeader.Width = 75;
			// 
			// FindClipsInfo
			// 
			this.FindClipsInfo.AutoSize = true;
			this.FindClipsInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FindClipsInfo.Location = new System.Drawing.Point(11, 587);
			this.FindClipsInfo.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.FindClipsInfo.Name = "FindClipsInfo";
			this.FindClipsInfo.Size = new System.Drawing.Size(567, 20);
			this.FindClipsInfo.TabIndex = 6;
			this.FindClipsInfo.Text = "在上方选中相匹配的剪辑，确定之后将会选中这些剪辑。";
			this.FindClipsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(381, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
			this.CancelBtn.Location = new System.Drawing.Point(483, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
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
			this.dock.Location = new System.Drawing.Point(0, 622);
			this.dock.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(589, 52);
			this.dock.TabIndex = 10;
			// 
			// FindClipsForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(589, 674);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(604, 711);
			this.Name = "FindClipsForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "查找轨道素材";
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.dock.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RadioButton MatchSourceAndOffsetRadio;
		private System.Windows.Forms.RadioButton MatchSourceRadio;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.RadioButton MatchNameRadio;
		private System.Windows.Forms.Label SelectInfo;
		private System.Windows.Forms.TextBox ClipNameTxt;
		private System.Windows.Forms.ListView ClipNameList;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.ColumnHeader nameHeader;
		private System.Windows.Forms.ColumnHeader numHeader;
		private System.Windows.Forms.Label FindClipsInfo;
	}
}