namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class MidiChannelAdvancedForm {
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
			this.AutoLayoutTracksGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.ResetAutoLayoutTracksBtn = new System.Windows.Forms.Button();
			this.AutoLayoutTracksButtons = new System.Windows.Forms.TableLayoutPanel();
			this.GradientTracksBtn = new System.Windows.Forms.Button();
			this.AutoLayoutTracksBox3dBtn = new System.Windows.Forms.Button();
			this.AutoLayoutTracksGridBtn = new System.Windows.Forms.Button();
			this.MidiChannelAdvancedAutoLayoutTracksInfo = new System.Windows.Forms.Label();
			this.ChannelListView = new System.Windows.Forms.ListView();
			this.ChannelHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.NameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.InstrumentHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.NoteCountHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.BeginNoteHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.EditNotesBtn = new System.Windows.Forms.Button();
			this.SelectAllBtn = new System.Windows.Forms.Button();
			this.InvertSelectionButton = new System.Windows.Forms.Button();
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.AutoLayoutTracksGroup.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.AutoLayoutTracksButtons.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
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
			this.dock.Location = new System.Drawing.Point(0, 447);
			this.dock.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 8, 6, 8);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(582, 56);
			this.dock.TabIndex = 11;
			// 
			// OkBtn
			// 
			this.OkBtn.AutoSize = true;
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(382, 12);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.AutoSize = true;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(480, 12);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.AutoLayoutTracksGroup, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.ChannelListView, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(582, 447);
			this.tableLayoutPanel1.TabIndex = 13;
			// 
			// AutoLayoutTracksGroup
			// 
			this.AutoLayoutTracksGroup.AutoSize = true;
			this.AutoLayoutTracksGroup.Controls.Add(this.tableLayoutPanel2);
			this.AutoLayoutTracksGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AutoLayoutTracksGroup.Location = new System.Drawing.Point(8, 313);
			this.AutoLayoutTracksGroup.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksGroup.Name = "AutoLayoutTracksGroup";
			this.AutoLayoutTracksGroup.Padding = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksGroup.Size = new System.Drawing.Size(566, 126);
			this.AutoLayoutTracksGroup.TabIndex = 19;
			this.AutoLayoutTracksGroup.TabStop = false;
			this.AutoLayoutTracksGroup.Text = "自动布局轨道";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.ResetAutoLayoutTracksBtn, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.AutoLayoutTracksButtons, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.MidiChannelAdvancedAutoLayoutTracksInfo, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 22);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(562, 102);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// ResetAutoLayoutTracksBtn
			// 
			this.ResetAutoLayoutTracksBtn.AutoSize = true;
			this.ResetAutoLayoutTracksBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.ResetAutoLayoutTracksBtn.ForeColor = System.Drawing.Color.Red;
			this.ResetAutoLayoutTracksBtn.Location = new System.Drawing.Point(7, 63);
			this.ResetAutoLayoutTracksBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ResetAutoLayoutTracksBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.ResetAutoLayoutTracksBtn.Name = "ResetAutoLayoutTracksBtn";
			this.ResetAutoLayoutTracksBtn.Size = new System.Drawing.Size(134, 32);
			this.ResetAutoLayoutTracksBtn.TabIndex = 10;
			this.ResetAutoLayoutTracksBtn.Text = "重置";
			this.ResetAutoLayoutTracksBtn.UseVisualStyleBackColor = true;
			this.ResetAutoLayoutTracksBtn.Click += new System.EventHandler(this.ResetAutoLayoutTracksBtn_Click);
			// 
			// AutoLayoutTracksButtons
			// 
			this.AutoLayoutTracksButtons.AutoSize = true;
			this.AutoLayoutTracksButtons.ColumnCount = 4;
			this.AutoLayoutTracksButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.AutoLayoutTracksButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.AutoLayoutTracksButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.AutoLayoutTracksButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.AutoLayoutTracksButtons.Controls.Add(this.GradientTracksBtn, 0, 0);
			this.AutoLayoutTracksButtons.Controls.Add(this.AutoLayoutTracksBox3dBtn, 0, 0);
			this.AutoLayoutTracksButtons.Controls.Add(this.AutoLayoutTracksGridBtn, 0, 0);
			this.AutoLayoutTracksButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AutoLayoutTracksButtons.Location = new System.Drawing.Point(5, 25);
			this.AutoLayoutTracksButtons.Margin = new System.Windows.Forms.Padding(0);
			this.AutoLayoutTracksButtons.Name = "AutoLayoutTracksButtons";
			this.AutoLayoutTracksButtons.RowCount = 1;
			this.AutoLayoutTracksButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.AutoLayoutTracksButtons.Size = new System.Drawing.Size(552, 36);
			this.AutoLayoutTracksButtons.TabIndex = 9;
			// 
			// GradientTracksBtn
			// 
			this.GradientTracksBtn.AutoSize = true;
			this.GradientTracksBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.GradientTracksBtn.Location = new System.Drawing.Point(278, 2);
			this.GradientTracksBtn.Margin = new System.Windows.Forms.Padding(2);
			this.GradientTracksBtn.Name = "GradientTracksBtn";
			this.GradientTracksBtn.Size = new System.Drawing.Size(134, 32);
			this.GradientTracksBtn.TabIndex = 5;
			this.GradientTracksBtn.Text = "渐变轨道...";
			this.GradientTracksBtn.UseVisualStyleBackColor = true;
			this.GradientTracksBtn.Click += new System.EventHandler(this.GradientTracksBtn_Click);
			// 
			// AutoLayoutTracksBox3dBtn
			// 
			this.AutoLayoutTracksBox3dBtn.AutoSize = true;
			this.AutoLayoutTracksBox3dBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.AutoLayoutTracksBox3dBtn.Location = new System.Drawing.Point(140, 2);
			this.AutoLayoutTracksBox3dBtn.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksBox3dBtn.Name = "AutoLayoutTracksBox3dBtn";
			this.AutoLayoutTracksBox3dBtn.Size = new System.Drawing.Size(134, 32);
			this.AutoLayoutTracksBox3dBtn.TabIndex = 4;
			this.AutoLayoutTracksBox3dBtn.Text = "3D 方盒布局...";
			this.AutoLayoutTracksBox3dBtn.UseVisualStyleBackColor = true;
			this.AutoLayoutTracksBox3dBtn.Click += new System.EventHandler(this.AutoLayoutTracksBox3dBtn_Click);
			// 
			// AutoLayoutTracksGridBtn
			// 
			this.AutoLayoutTracksGridBtn.AutoSize = true;
			this.AutoLayoutTracksGridBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.AutoLayoutTracksGridBtn.Location = new System.Drawing.Point(2, 2);
			this.AutoLayoutTracksGridBtn.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksGridBtn.Name = "AutoLayoutTracksGridBtn";
			this.AutoLayoutTracksGridBtn.Size = new System.Drawing.Size(134, 32);
			this.AutoLayoutTracksGridBtn.TabIndex = 3;
			this.AutoLayoutTracksGridBtn.Text = "网格布局...";
			this.AutoLayoutTracksGridBtn.UseVisualStyleBackColor = true;
			this.AutoLayoutTracksGridBtn.Click += new System.EventHandler(this.AutoLayoutTracksGridBtn_Click);
			// 
			// MidiChannelAdvancedAutoLayoutTracksInfo
			// 
			this.MidiChannelAdvancedAutoLayoutTracksInfo.AutoSize = true;
			this.MidiChannelAdvancedAutoLayoutTracksInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelAdvancedAutoLayoutTracksInfo.Location = new System.Drawing.Point(8, 5);
			this.MidiChannelAdvancedAutoLayoutTracksInfo.Name = "MidiChannelAdvancedAutoLayoutTracksInfo";
			this.MidiChannelAdvancedAutoLayoutTracksInfo.Size = new System.Drawing.Size(546, 20);
			this.MidiChannelAdvancedAutoLayoutTracksInfo.TabIndex = 0;
			this.MidiChannelAdvancedAutoLayoutTracksInfo.Text = "仅在生成视频且不启用五线谱可视化效果时有效。";
			// 
			// ChannelListView
			// 
			this.ChannelListView.AllowColumnReorder = true;
			this.ChannelListView.CheckBoxes = true;
			this.ChannelListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChannelHeader,
            this.NameHeader,
            this.InstrumentHeader,
            this.NoteCountHeader,
            this.BeginNoteHeader});
			this.ChannelListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChannelListView.FullRowSelect = true;
			this.ChannelListView.HideSelection = false;
			this.ChannelListView.Location = new System.Drawing.Point(8, 8);
			this.ChannelListView.Margin = new System.Windows.Forms.Padding(2);
			this.ChannelListView.Name = "ChannelListView";
			this.ChannelListView.Size = new System.Drawing.Size(566, 257);
			this.ChannelListView.TabIndex = 13;
			this.ChannelListView.UseCompatibleStateImageBehavior = false;
			this.ChannelListView.View = System.Windows.Forms.View.Details;
			this.ChannelListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ChannelListView_ItemChecked);
			this.ChannelListView.SelectedIndexChanged += new System.EventHandler(this.ChannelListView_SelectedIndexChanged);
			// 
			// ChannelHeader
			// 
			this.ChannelHeader.Text = "通道";
			// 
			// NameHeader
			// 
			this.NameHeader.Text = "名称";
			this.NameHeader.Width = 200;
			// 
			// InstrumentHeader
			// 
			this.InstrumentHeader.Text = "乐器";
			this.InstrumentHeader.Width = 120;
			// 
			// NoteCountHeader
			// 
			this.NoteCountHeader.Text = "音符数";
			// 
			// BeginNoteHeader
			// 
			this.BeginNoteHeader.Text = "起音";
			this.BeginNoteHeader.Width = 120;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.EditNotesBtn);
			this.flowLayoutPanel1.Controls.Add(this.SelectAllBtn);
			this.flowLayoutPanel1.Controls.Add(this.InvertSelectionButton);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 270);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(564, 38);
			this.flowLayoutPanel1.TabIndex = 17;
			// 
			// EditNotesBtn
			// 
			this.EditNotesBtn.AutoSize = true;
			this.EditNotesBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.EditNotesBtn.Location = new System.Drawing.Point(2, 2);
			this.EditNotesBtn.Margin = new System.Windows.Forms.Padding(2);
			this.EditNotesBtn.Name = "EditNotesBtn";
			this.EditNotesBtn.Size = new System.Drawing.Size(201, 34);
			this.EditNotesBtn.TabIndex = 15;
			this.EditNotesBtn.Text = "编辑所选通道音符...";
			this.EditNotesBtn.UseVisualStyleBackColor = true;
			this.EditNotesBtn.Click += new System.EventHandler(this.EditNotesBtn_Click);
			// 
			// SelectAllBtn
			// 
			this.SelectAllBtn.AutoSize = true;
			this.SelectAllBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectAllBtn.Location = new System.Drawing.Point(208, 3);
			this.SelectAllBtn.MinimumSize = new System.Drawing.Size(94, 32);
			this.SelectAllBtn.Name = "SelectAllBtn";
			this.SelectAllBtn.Size = new System.Drawing.Size(94, 32);
			this.SelectAllBtn.TabIndex = 16;
			this.SelectAllBtn.Text = "全选";
			this.SelectAllBtn.UseVisualStyleBackColor = true;
			this.SelectAllBtn.Click += new System.EventHandler(this.SelectAllBtn_Click);
			// 
			// InvertSelectionButton
			// 
			this.InvertSelectionButton.AutoSize = true;
			this.InvertSelectionButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InvertSelectionButton.Location = new System.Drawing.Point(308, 3);
			this.InvertSelectionButton.MinimumSize = new System.Drawing.Size(94, 32);
			this.InvertSelectionButton.Name = "InvertSelectionButton";
			this.InvertSelectionButton.Size = new System.Drawing.Size(94, 32);
			this.InvertSelectionButton.TabIndex = 17;
			this.InvertSelectionButton.Text = "反选";
			this.InvertSelectionButton.UseVisualStyleBackColor = true;
			this.InvertSelectionButton.Click += new System.EventHandler(this.InvertSelectionButton_Click);
			// 
			// MidiChannelAdvancedForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(582, 503);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 550);
			this.Name = "MidiChannelAdvancedForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MIDI 通道高级属性";
			this.dock.ResumeLayout(false);
			this.dock.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.AutoLayoutTracksGroup.ResumeLayout(false);
			this.AutoLayoutTracksGroup.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.AutoLayoutTracksButtons.ResumeLayout(false);
			this.AutoLayoutTracksButtons.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.ListView ChannelListView;
		private System.Windows.Forms.ColumnHeader ChannelHeader;
		private System.Windows.Forms.ColumnHeader NameHeader;
		private System.Windows.Forms.ColumnHeader InstrumentHeader;
		private System.Windows.Forms.ColumnHeader NoteCountHeader;
		private System.Windows.Forms.ColumnHeader BeginNoteHeader;
		private System.Windows.Forms.GroupBox AutoLayoutTracksGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button ResetAutoLayoutTracksBtn;
		private System.Windows.Forms.TableLayoutPanel AutoLayoutTracksButtons;
		private System.Windows.Forms.Button GradientTracksBtn;
		private System.Windows.Forms.Button AutoLayoutTracksBox3dBtn;
		private System.Windows.Forms.Button AutoLayoutTracksGridBtn;
		private System.Windows.Forms.Label MidiChannelAdvancedAutoLayoutTracksInfo;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button EditNotesBtn;
		private System.Windows.Forms.Button SelectAllBtn;
		private System.Windows.Forms.Button InvertSelectionButton;
	}
}