
namespace Otomad.VegasScript.OtomadHelper.V4 {
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
			this.UseTrackEventGroupCheck = new System.Windows.Forms.CheckBox();
			this.ReserveOriginalNameCheck = new System.Windows.Forms.CheckBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabs = new System.Windows.Forms.TabControl();
			this.ClassicTab = new System.Windows.Forms.TabPage();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.ReplacerCombo = new System.Windows.Forms.ListBox();
			this.ClassicReplacerLbl = new System.Windows.Forms.Label();
			this.ReplaceClipsLbl = new System.Windows.Forms.Label();
			this.ClassicReplacedLbl = new System.Windows.Forms.Label();
			this.SeparationTab = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.SeparationReplacerInfo = new System.Windows.Forms.Label();
			this.ReplacerIcon = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.SetReplacerBtn = new System.Windows.Forms.Button();
			this.BackToSelect2 = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.SeparationReplacedInfo = new System.Windows.Forms.Label();
			this.ReplacedIcon = new System.Windows.Forms.PictureBox();
			this.SeparationReplacedLbl = new System.Windows.Forms.Label();
			this.SeparationReplacerLbl = new System.Windows.Forms.Label();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.SetReplacedBtn = new System.Windows.Forms.Button();
			this.BackToSelect1 = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.ViewLbl = new System.Windows.Forms.Label();
			this.ViewSelectReplacedRadio = new System.Windows.Forms.RadioButton();
			this.ViewSelectReplacerRadio = new System.Windows.Forms.RadioButton();
			this.ViewSelectOriginalRadio = new System.Windows.Forms.RadioButton();
			this.dock.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabs.SuspendLayout();
			this.ClassicTab.SuspendLayout();
			this.table.SuspendLayout();
			this.SeparationTab.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplacerIcon)).BeginInit();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplacedIcon)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dock
			// 
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 5;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.Controls.Add(this.OkBtn, 3, 0);
			this.dock.Controls.Add(this.CancelBtn, 4, 0);
			this.dock.Controls.Add(this.UseTrackEventGroupCheck, 0, 0);
			this.dock.Controls.Add(this.ReserveOriginalNameCheck, 1, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 508);
			this.dock.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(870, 52);
			this.dock.TabIndex = 3;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(662, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "替换(&R)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(764, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "关闭(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// UseTrackEventGroupCheck
			// 
			this.UseTrackEventGroupCheck.AutoSize = true;
			this.UseTrackEventGroupCheck.Checked = true;
			this.UseTrackEventGroupCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.UseTrackEventGroupCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UseTrackEventGroupCheck.Location = new System.Drawing.Point(12, 10);
			this.UseTrackEventGroupCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.UseTrackEventGroupCheck.Name = "UseTrackEventGroupCheck";
			this.UseTrackEventGroupCheck.Size = new System.Drawing.Size(196, 32);
			this.UseTrackEventGroupCheck.TabIndex = 0;
			this.UseTrackEventGroupCheck.Text = "同时替换分组内其它剪辑";
			this.UseTrackEventGroupCheck.UseVisualStyleBackColor = true;
			// 
			// ReserveOriginalNameCheck
			// 
			this.ReserveOriginalNameCheck.AutoSize = true;
			this.ReserveOriginalNameCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReserveOriginalNameCheck.Location = new System.Drawing.Point(216, 10);
			this.ReserveOriginalNameCheck.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ReserveOriginalNameCheck.Name = "ReserveOriginalNameCheck";
			this.ReserveOriginalNameCheck.Size = new System.Drawing.Size(136, 32);
			this.ReserveOriginalNameCheck.TabIndex = 3;
			this.ReserveOriginalNameCheck.Text = "保留原剪辑名称";
			this.ReserveOriginalNameCheck.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.tabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
			this.panel1.Size = new System.Drawing.Size(870, 508);
			this.panel1.TabIndex = 4;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.ClassicTab);
			this.tabs.Controls.Add(this.SeparationTab);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(10, 10);
			this.tabs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(850, 498);
			this.tabs.TabIndex = 0;
			this.tabs.SelectedIndexChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ClassicTab
			// 
			this.ClassicTab.Controls.Add(this.table);
			this.ClassicTab.Location = new System.Drawing.Point(4, 29);
			this.ClassicTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ClassicTab.Name = "ClassicTab";
			this.ClassicTab.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ClassicTab.Size = new System.Drawing.Size(842, 465);
			this.ClassicTab.TabIndex = 0;
			this.ClassicTab.Text = "同时指定";
			this.ClassicTab.UseVisualStyleBackColor = true;
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 832F));
			this.table.Controls.Add(this.ReplacerCombo, 0, 2);
			this.table.Controls.Add(this.ClassicReplacerLbl, 0, 1);
			this.table.Controls.Add(this.ReplaceClipsLbl, 0, 0);
			this.table.Controls.Add(this.ClassicReplacedLbl, 0, 3);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(4, 4);
			this.table.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.table.RowCount = 4;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(834, 457);
			this.table.TabIndex = 7;
			// 
			// ReplacerCombo
			// 
			this.ReplacerCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerCombo.FormattingEnabled = true;
			this.ReplacerCombo.ItemHeight = 20;
			this.ReplacerCombo.Location = new System.Drawing.Point(4, 80);
			this.ReplacerCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ReplacerCombo.Name = "ReplacerCombo";
			this.ReplacerCombo.Size = new System.Drawing.Size(826, 345);
			this.ReplacerCombo.TabIndex = 3;
			this.ReplacerCombo.SelectedIndexChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ClassicReplacerLbl
			// 
			this.ClassicReplacerLbl.AutoSize = true;
			this.ClassicReplacerLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClassicReplacerLbl.Location = new System.Drawing.Point(4, 56);
			this.ClassicReplacerLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ClassicReplacerLbl.Name = "ClassicReplacerLbl";
			this.ClassicReplacerLbl.Size = new System.Drawing.Size(826, 20);
			this.ClassicReplacerLbl.TabIndex = 4;
			this.ClassicReplacerLbl.Text = "指定的替换项为";
			this.ClassicReplacerLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplaceClipsLbl
			// 
			this.ReplaceClipsLbl.AutoSize = true;
			this.ReplaceClipsLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplaceClipsLbl.Location = new System.Drawing.Point(4, 8);
			this.ReplaceClipsLbl.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
			this.ReplaceClipsLbl.Name = "ReplaceClipsLbl";
			this.ReplaceClipsLbl.Size = new System.Drawing.Size(826, 40);
			this.ReplaceClipsLbl.TabIndex = 1;
			this.ReplaceClipsLbl.Text = "请先在轨道窗口中选中替换与被替换的素材，然后指定一个素材为替换的素材，剩余素材均为被替换素材。\r\n请先将替换素材的音视频创建分组，并确保替换素材放置在时间靠后的位" +
    "置并且尽量不与其它被替换素材位于同一轨道。";
			this.ReplaceClipsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ClassicReplacedLbl
			// 
			this.ClassicReplacedLbl.AutoSize = true;
			this.ClassicReplacedLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClassicReplacedLbl.Location = new System.Drawing.Point(4, 429);
			this.ClassicReplacedLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ClassicReplacedLbl.Name = "ClassicReplacedLbl";
			this.ClassicReplacedLbl.Size = new System.Drawing.Size(826, 20);
			this.ClassicReplacedLbl.TabIndex = 3;
			this.ClassicReplacedLbl.Text = "则剩余 0 项轨道剪辑将被替换为选定素材。";
			this.ClassicReplacedLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SeparationTab
			// 
			this.SeparationTab.Controls.Add(this.tableLayoutPanel1);
			this.SeparationTab.Location = new System.Drawing.Point(4, 29);
			this.SeparationTab.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SeparationTab.Name = "SeparationTab";
			this.SeparationTab.Size = new System.Drawing.Size(842, 465);
			this.SeparationTab.TabIndex = 1;
			this.SeparationTab.Text = "分别指定";
			this.SeparationTab.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel5, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.SeparationReplacedLbl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.SeparationReplacerLbl, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4, 10, 4, 4);
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(842, 411);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.SeparationReplacerInfo, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.ReplacerIcon, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(8, 258);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(826, 92);
			this.tableLayoutPanel3.TabIndex = 10;
			// 
			// SeparationReplacerInfo
			// 
			this.SeparationReplacerInfo.AutoSize = true;
			this.SeparationReplacerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SeparationReplacerInfo.Location = new System.Drawing.Point(84, 12);
			this.SeparationReplacerInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SeparationReplacerInfo.Name = "SeparationReplacerInfo";
			this.SeparationReplacerInfo.Size = new System.Drawing.Size(726, 68);
			this.SeparationReplacerInfo.TabIndex = 5;
			this.SeparationReplacerInfo.Text = "音频：无\r\n视频：无";
			this.SeparationReplacerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplacerIcon
			// 
			this.ReplacerIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerIcon.Location = new System.Drawing.Point(16, 16);
			this.ReplacerIcon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ReplacerIcon.MinimumSize = new System.Drawing.Size(60, 60);
			this.ReplacerIcon.Name = "ReplacerIcon";
			this.ReplacerIcon.Size = new System.Drawing.Size(60, 60);
			this.ReplacerIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.ReplacerIcon.TabIndex = 4;
			this.ReplacerIcon.TabStop = false;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Controls.Add(this.SetReplacerBtn, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.BackToSelect2, 1, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(8, 358);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(826, 45);
			this.tableLayoutPanel5.TabIndex = 9;
			// 
			// SetReplacerBtn
			// 
			this.SetReplacerBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetReplacerBtn.Location = new System.Drawing.Point(4, 4);
			this.SetReplacerBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SetReplacerBtn.Name = "SetReplacerBtn";
			this.SetReplacerBtn.Size = new System.Drawing.Size(405, 37);
			this.SetReplacerBtn.TabIndex = 0;
			this.SetReplacerBtn.Text = "将选中的 0 个素材设为替换项";
			this.SetReplacerBtn.UseVisualStyleBackColor = true;
			this.SetReplacerBtn.Click += new System.EventHandler(this.SetReplacerBtn_Click);
			// 
			// BackToSelect2
			// 
			this.BackToSelect2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackToSelect2.Location = new System.Drawing.Point(417, 4);
			this.BackToSelect2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.BackToSelect2.Name = "BackToSelect2";
			this.BackToSelect2.Size = new System.Drawing.Size(405, 37);
			this.BackToSelect2.TabIndex = 1;
			this.BackToSelect2.Text = "返回 Vegas 选定素材";
			this.BackToSelect2.UseVisualStyleBackColor = true;
			this.BackToSelect2.Click += new System.EventHandler(this.BackToSelect_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.SeparationReplacedInfo, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.ReplacedIcon, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 84);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(12, 12, 12, 12);
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(826, 92);
			this.tableLayoutPanel2.TabIndex = 6;
			// 
			// SeparationReplacedInfo
			// 
			this.SeparationReplacedInfo.AutoSize = true;
			this.SeparationReplacedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SeparationReplacedInfo.Location = new System.Drawing.Point(84, 12);
			this.SeparationReplacedInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SeparationReplacedInfo.Name = "SeparationReplacedInfo";
			this.SeparationReplacedInfo.Size = new System.Drawing.Size(726, 68);
			this.SeparationReplacedInfo.TabIndex = 5;
			this.SeparationReplacedInfo.Text = "已选中 0 个轨道素材，其中 0 个音频剪辑，0 个视频剪辑。";
			this.SeparationReplacedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplacedIcon
			// 
			this.ReplacedIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacedIcon.Location = new System.Drawing.Point(16, 16);
			this.ReplacedIcon.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ReplacedIcon.MinimumSize = new System.Drawing.Size(60, 60);
			this.ReplacedIcon.Name = "ReplacedIcon";
			this.ReplacedIcon.Size = new System.Drawing.Size(60, 60);
			this.ReplacedIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.ReplacedIcon.TabIndex = 4;
			this.ReplacedIcon.TabStop = false;
			// 
			// SeparationReplacedLbl
			// 
			this.SeparationReplacedLbl.AutoSize = true;
			this.SeparationReplacedLbl.Location = new System.Drawing.Point(8, 60);
			this.SeparationReplacedLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SeparationReplacedLbl.Name = "SeparationReplacedLbl";
			this.SeparationReplacedLbl.Size = new System.Drawing.Size(69, 20);
			this.SeparationReplacedLbl.TabIndex = 0;
			this.SeparationReplacedLbl.Text = "被替换项";
			// 
			// SeparationReplacerLbl
			// 
			this.SeparationReplacerLbl.AutoSize = true;
			this.SeparationReplacerLbl.Location = new System.Drawing.Point(8, 234);
			this.SeparationReplacerLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SeparationReplacerLbl.Name = "SeparationReplacerLbl";
			this.SeparationReplacerLbl.Size = new System.Drawing.Size(54, 20);
			this.SeparationReplacerLbl.TabIndex = 1;
			this.SeparationReplacerLbl.Text = "替换项";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Controls.Add(this.SetReplacedBtn, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.BackToSelect1, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(8, 184);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(826, 46);
			this.tableLayoutPanel4.TabIndex = 8;
			// 
			// SetReplacedBtn
			// 
			this.SetReplacedBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetReplacedBtn.Location = new System.Drawing.Point(4, 4);
			this.SetReplacedBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SetReplacedBtn.Name = "SetReplacedBtn";
			this.SetReplacedBtn.Size = new System.Drawing.Size(405, 38);
			this.SetReplacedBtn.TabIndex = 0;
			this.SetReplacedBtn.Text = "将选中的 0 个素材设为被替换项";
			this.SetReplacedBtn.UseVisualStyleBackColor = true;
			this.SetReplacedBtn.Click += new System.EventHandler(this.SetReplacedBtn_Click);
			// 
			// BackToSelect1
			// 
			this.BackToSelect1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackToSelect1.Location = new System.Drawing.Point(417, 4);
			this.BackToSelect1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.BackToSelect1.Name = "BackToSelect1";
			this.BackToSelect1.Size = new System.Drawing.Size(405, 38);
			this.BackToSelect1.TabIndex = 1;
			this.BackToSelect1.Text = "返回 Vegas 选定素材";
			this.BackToSelect1.UseVisualStyleBackColor = true;
			this.BackToSelect1.Click += new System.EventHandler(this.BackToSelect_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.ViewLbl);
			this.flowLayoutPanel1.Controls.Add(this.ViewSelectReplacedRadio);
			this.flowLayoutPanel1.Controls.Add(this.ViewSelectReplacerRadio);
			this.flowLayoutPanel1.Controls.Add(this.ViewSelectOriginalRadio);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 14);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(834, 42);
			this.flowLayoutPanel1.TabIndex = 11;
			// 
			// ViewLbl
			// 
			this.ViewLbl.AutoSize = true;
			this.ViewLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewLbl.Location = new System.Drawing.Point(4, 0);
			this.ViewLbl.Margin = new System.Windows.Forms.Padding(4, 0, 11, 0);
			this.ViewLbl.Name = "ViewLbl";
			this.ViewLbl.Size = new System.Drawing.Size(39, 32);
			this.ViewLbl.TabIndex = 0;
			this.ViewLbl.Text = "查看";
			this.ViewLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ViewSelectReplacedRadio
			// 
			this.ViewSelectReplacedRadio.AutoSize = true;
			this.ViewSelectReplacedRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewSelectReplacedRadio.Location = new System.Drawing.Point(58, 4);
			this.ViewSelectReplacedRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ViewSelectReplacedRadio.Name = "ViewSelectReplacedRadio";
			this.ViewSelectReplacedRadio.Size = new System.Drawing.Size(90, 24);
			this.ViewSelectReplacedRadio.TabIndex = 1;
			this.ViewSelectReplacedRadio.Text = "被替换项";
			this.ViewSelectReplacedRadio.UseVisualStyleBackColor = true;
			this.ViewSelectReplacedRadio.CheckedChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ViewSelectReplacerRadio
			// 
			this.ViewSelectReplacerRadio.AutoSize = true;
			this.ViewSelectReplacerRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewSelectReplacerRadio.Location = new System.Drawing.Point(156, 4);
			this.ViewSelectReplacerRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ViewSelectReplacerRadio.Name = "ViewSelectReplacerRadio";
			this.ViewSelectReplacerRadio.Size = new System.Drawing.Size(75, 24);
			this.ViewSelectReplacerRadio.TabIndex = 2;
			this.ViewSelectReplacerRadio.Text = "替换项";
			this.ViewSelectReplacerRadio.UseVisualStyleBackColor = true;
			this.ViewSelectReplacerRadio.CheckedChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ViewSelectOriginalRadio
			// 
			this.ViewSelectOriginalRadio.AutoSize = true;
			this.ViewSelectOriginalRadio.Checked = true;
			this.ViewSelectOriginalRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewSelectOriginalRadio.Location = new System.Drawing.Point(239, 4);
			this.ViewSelectOriginalRadio.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ViewSelectOriginalRadio.Name = "ViewSelectOriginalRadio";
			this.ViewSelectOriginalRadio.Size = new System.Drawing.Size(75, 24);
			this.ViewSelectOriginalRadio.TabIndex = 3;
			this.ViewSelectOriginalRadio.TabStop = true;
			this.ViewSelectOriginalRadio.Text = "选中项";
			this.ViewSelectOriginalRadio.UseVisualStyleBackColor = true;
			this.ViewSelectOriginalRadio.CheckedChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ReplaceClipsForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(870, 560);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(886, 597);
			this.Name = "ReplaceClipsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "替换轨道素材";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReplaceClipsForm_FormClosing);
			this.dock.ResumeLayout(false);
			this.dock.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tabs.ResumeLayout(false);
			this.ClassicTab.ResumeLayout(false);
			this.ClassicTab.PerformLayout();
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.SeparationTab.ResumeLayout(false);
			this.SeparationTab.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplacerIcon)).EndInit();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplacedIcon)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TabControl tabs;
		private System.Windows.Forms.TabPage ClassicTab;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label ClassicReplacerLbl;
		private System.Windows.Forms.Label ReplaceClipsLbl;
		private System.Windows.Forms.Label ClassicReplacedLbl;
		private System.Windows.Forms.TabPage SeparationTab;
		private System.Windows.Forms.ListBox ReplacerCombo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label SeparationReplacedInfo;
		private System.Windows.Forms.PictureBox ReplacedIcon;
		private System.Windows.Forms.Label SeparationReplacedLbl;
		private System.Windows.Forms.Label SeparationReplacerLbl;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button SetReplacerBtn;
		private System.Windows.Forms.Button BackToSelect2;
		private System.Windows.Forms.Button SetReplacedBtn;
		private System.Windows.Forms.Button BackToSelect1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label SeparationReplacerInfo;
		private System.Windows.Forms.PictureBox ReplacerIcon;
		private System.Windows.Forms.CheckBox UseTrackEventGroupCheck;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label ViewLbl;
		private System.Windows.Forms.RadioButton ViewSelectReplacedRadio;
		private System.Windows.Forms.RadioButton ViewSelectReplacerRadio;
		private System.Windows.Forms.RadioButton ViewSelectOriginalRadio;
		private System.Windows.Forms.CheckBox ReserveOriginalNameCheck;
	}
}