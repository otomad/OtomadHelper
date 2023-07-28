namespace Otomad.VegasScript.OtomadHelper.V4 {
	partial class ExportMidiFileForm {
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
			this.LoopRegionOnlyCheck = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.DispatchInstrumentLbl = new System.Windows.Forms.Label();
			this.InstrumentCombo = new System.Windows.Forms.ComboBox();
			this.MidiInstrumentList = new System.Windows.Forms.ListView();
			this.MidiInstrumentChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiInstrumentInstrument = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiInstrumentLbl = new System.Windows.Forms.Label();
			this.MidiChannelActions = new System.Windows.Forms.FlowLayoutPanel();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.RemoveChannelBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.InstrumentLbl = new System.Windows.Forms.Label();
			this.InstrumentTxt = new System.Windows.Forms.Label();
			this.ChannelValueLbl = new System.Windows.Forms.Label();
			this.ChannelValueCombo = new System.Windows.Forms.ComboBox();
			this.MidiTrackActions = new System.Windows.Forms.TableLayoutPanel();
			this.InsertNewTrackBtn = new System.Windows.Forms.Button();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.MoveUpBtn = new System.Windows.Forms.Button();
			this.MoveDownBtn = new System.Windows.Forms.Button();
			this.RemoveTrackBtn = new System.Windows.Forms.Button();
			this.AddNewTrackBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.MidiTrackNameLbl = new System.Windows.Forms.Label();
			this.MidiTrackNameTxt = new System.Windows.Forms.TextBox();
			this.MidiChannelList = new System.Windows.Forms.ListView();
			this.MidiChannelIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiChannelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiChannelValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiChannelNoteCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiTrackList = new System.Windows.Forms.ListView();
			this.MidiTrackIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiTrackName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiTrackContain = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiTrackNoteCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MidiChannelLbl = new System.Windows.Forms.Label();
			this.MidiTrackLbl = new System.Windows.Forms.Label();
			this.VegasTrackLbl = new System.Windows.Forms.Label();
			this.VegasTrackList = new System.Windows.Forms.ListView();
			this.VegasTrackIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VegasTrackName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VegasTrackEventCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VegasTrackMute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VegasTrackSolo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.VegasTrackActions = new System.Windows.Forms.TableLayoutPanel();
			this.AddToSameNewTrackBtn = new System.Windows.Forms.Button();
			this.AddToEachNewTrackBtn = new System.Windows.Forms.Button();
			this.AddToCurrentTrackBtn = new System.Windows.Forms.Button();
			this.PreviewTrackBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.AudioMainKeyCombo = new System.Windows.Forms.ComboBox();
			this.AudioBasePitchLbl = new System.Windows.Forms.Label();
			this.AudioMainOctaveCombo = new System.Windows.Forms.ComboBox();
			this.FilterTrackFlow = new System.Windows.Forms.FlowLayoutPanel();
			this.FilterAllRadio = new System.Windows.Forms.RadioButton();
			this.FilterVideoRadio = new System.Windows.Forms.RadioButton();
			this.FilterAudioRadio = new System.Windows.Forms.RadioButton();
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.MidiChannelActions.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.MidiTrackActions.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.VegasTrackActions.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.FilterTrackFlow.SuspendLayout();
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
			this.dock.Controls.Add(this.LoopRegionOnlyCheck, 0, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 411);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(1282, 52);
			this.dock.TabIndex = 10;
			// 
			// OkBtn
			// 
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(1074, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "导出(&E)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(1176, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			// 
			// LoopRegionOnlyCheck
			// 
			this.LoopRegionOnlyCheck.AutoSize = true;
			this.LoopRegionOnlyCheck.Dock = System.Windows.Forms.DockStyle.Left;
			this.LoopRegionOnlyCheck.Location = new System.Drawing.Point(11, 9);
			this.LoopRegionOnlyCheck.Name = "LoopRegionOnlyCheck";
			this.LoopRegionOnlyCheck.Size = new System.Drawing.Size(136, 34);
			this.LoopRegionOnlyCheck.TabIndex = 3;
			this.LoopRegionOnlyCheck.Text = "仅导出循环区域";
			this.LoopRegionOnlyCheck.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel6, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.MidiInstrumentList, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.MidiInstrumentLbl, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.MidiChannelActions, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.MidiTrackActions, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.MidiChannelList, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.MidiTrackList, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.MidiChannelLbl, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.MidiTrackLbl, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.VegasTrackLbl, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.VegasTrackList, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.VegasTrackActions, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(9);
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1282, 411);
			this.tableLayoutPanel1.TabIndex = 11;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.ColumnCount = 1;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.DispatchInstrumentLbl, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.InstrumentCombo, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(960, 287);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(310, 112);
			this.tableLayoutPanel6.TabIndex = 19;
			// 
			// DispatchInstrumentLbl
			// 
			this.DispatchInstrumentLbl.AutoSize = true;
			this.DispatchInstrumentLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DispatchInstrumentLbl.Location = new System.Drawing.Point(0, 0);
			this.DispatchInstrumentLbl.Margin = new System.Windows.Forms.Padding(0);
			this.DispatchInstrumentLbl.Name = "DispatchInstrumentLbl";
			this.DispatchInstrumentLbl.Size = new System.Drawing.Size(310, 20);
			this.DispatchInstrumentLbl.TabIndex = 8;
			this.DispatchInstrumentLbl.Text = "为通道指派乐器";
			this.DispatchInstrumentLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InstrumentCombo
			// 
			this.InstrumentCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InstrumentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.InstrumentCombo.Enabled = false;
			this.InstrumentCombo.FormattingEnabled = true;
			this.InstrumentCombo.Location = new System.Drawing.Point(3, 23);
			this.InstrumentCombo.Name = "InstrumentCombo";
			this.InstrumentCombo.Size = new System.Drawing.Size(304, 28);
			this.InstrumentCombo.TabIndex = 9;
			this.InstrumentCombo.SelectedIndexChanged += new System.EventHandler(this.InstrumentCombo_SelectedIndexChanged);
			// 
			// MidiInstrumentList
			// 
			this.MidiInstrumentList.AllowColumnReorder = true;
			this.MidiInstrumentList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MidiInstrumentChannel,
            this.MidiInstrumentInstrument});
			this.MidiInstrumentList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiInstrumentList.FullRowSelect = true;
			this.MidiInstrumentList.HideSelection = false;
			this.MidiInstrumentList.Location = new System.Drawing.Point(960, 72);
			this.MidiInstrumentList.Name = "MidiInstrumentList";
			this.MidiInstrumentList.ShowItemToolTips = true;
			this.MidiInstrumentList.Size = new System.Drawing.Size(310, 209);
			this.MidiInstrumentList.TabIndex = 18;
			this.MidiInstrumentList.UseCompatibleStateImageBehavior = false;
			this.MidiInstrumentList.View = System.Windows.Forms.View.Details;
			this.MidiInstrumentList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.List_ItemSelectionChanged);
			this.MidiInstrumentList.SelectedIndexChanged += new System.EventHandler(this.MidiInstrumentList_SelectedIndexChanged);
			// 
			// MidiInstrumentChannel
			// 
			this.MidiInstrumentChannel.Text = "通道值";
			this.MidiInstrumentChannel.Width = 70;
			// 
			// MidiInstrumentInstrument
			// 
			this.MidiInstrumentInstrument.Text = "乐器";
			this.MidiInstrumentInstrument.Width = 180;
			// 
			// MidiInstrumentLbl
			// 
			this.MidiInstrumentLbl.AutoSize = true;
			this.MidiInstrumentLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiInstrumentLbl.Location = new System.Drawing.Point(960, 49);
			this.MidiInstrumentLbl.Name = "MidiInstrumentLbl";
			this.MidiInstrumentLbl.Size = new System.Drawing.Size(310, 20);
			this.MidiInstrumentLbl.TabIndex = 17;
			this.MidiInstrumentLbl.Text = "MIDI 乐器列表";
			this.MidiInstrumentLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiChannelActions
			// 
			this.MidiChannelActions.AutoSize = true;
			this.MidiChannelActions.Controls.Add(this.flowLayoutPanel5);
			this.MidiChannelActions.Controls.Add(this.tableLayoutPanel3);
			this.MidiChannelActions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelActions.Enabled = false;
			this.MidiChannelActions.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.MidiChannelActions.Location = new System.Drawing.Point(644, 287);
			this.MidiChannelActions.Name = "MidiChannelActions";
			this.MidiChannelActions.Size = new System.Drawing.Size(310, 112);
			this.MidiChannelActions.TabIndex = 15;
			this.MidiChannelActions.WrapContents = false;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.Controls.Add(this.RemoveChannelBtn);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Size = new System.Drawing.Size(162, 38);
			this.flowLayoutPanel5.TabIndex = 0;
			// 
			// RemoveChannelBtn
			// 
			this.RemoveChannelBtn.AutoSize = true;
			this.RemoveChannelBtn.Location = new System.Drawing.Point(3, 3);
			this.RemoveChannelBtn.Name = "RemoveChannelBtn";
			this.RemoveChannelBtn.Size = new System.Drawing.Size(75, 32);
			this.RemoveChannelBtn.TabIndex = 2;
			this.RemoveChannelBtn.Text = "移除";
			this.RemoveChannelBtn.UseVisualStyleBackColor = true;
			this.RemoveChannelBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.InstrumentLbl, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.InstrumentTxt, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.ChannelValueLbl, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.ChannelValueCombo, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 41);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(156, 68);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// InstrumentLbl
			// 
			this.InstrumentLbl.AutoSize = true;
			this.InstrumentLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InstrumentLbl.Location = new System.Drawing.Point(3, 34);
			this.InstrumentLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.InstrumentLbl.Name = "InstrumentLbl";
			this.InstrumentLbl.Size = new System.Drawing.Size(54, 34);
			this.InstrumentLbl.TabIndex = 11;
			this.InstrumentLbl.Text = "乐器";
			this.InstrumentLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InstrumentTxt
			// 
			this.InstrumentTxt.AutoSize = true;
			this.InstrumentTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InstrumentTxt.Location = new System.Drawing.Point(63, 34);
			this.InstrumentTxt.MinimumSize = new System.Drawing.Size(0, 34);
			this.InstrumentTxt.Name = "InstrumentTxt";
			this.InstrumentTxt.Size = new System.Drawing.Size(90, 34);
			this.InstrumentTxt.TabIndex = 10;
			this.InstrumentTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ChannelValueLbl
			// 
			this.ChannelValueLbl.AutoSize = true;
			this.ChannelValueLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChannelValueLbl.Location = new System.Drawing.Point(3, 0);
			this.ChannelValueLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.ChannelValueLbl.Name = "ChannelValueLbl";
			this.ChannelValueLbl.Size = new System.Drawing.Size(54, 34);
			this.ChannelValueLbl.TabIndex = 8;
			this.ChannelValueLbl.Text = "通道值";
			this.ChannelValueLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ChannelValueCombo
			// 
			this.ChannelValueCombo.Dock = System.Windows.Forms.DockStyle.Left;
			this.ChannelValueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ChannelValueCombo.FormattingEnabled = true;
			this.ChannelValueCombo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
			this.ChannelValueCombo.Location = new System.Drawing.Point(63, 3);
			this.ChannelValueCombo.Name = "ChannelValueCombo";
			this.ChannelValueCombo.Size = new System.Drawing.Size(90, 28);
			this.ChannelValueCombo.TabIndex = 9;
			this.ChannelValueCombo.SelectedIndexChanged += new System.EventHandler(this.ChannelValueCombo_SelectedIndexChanged);
			// 
			// MidiTrackActions
			// 
			this.MidiTrackActions.AutoSize = true;
			this.MidiTrackActions.ColumnCount = 2;
			this.MidiTrackActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.MidiTrackActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.MidiTrackActions.Controls.Add(this.InsertNewTrackBtn, 1, 1);
			this.MidiTrackActions.Controls.Add(this.flowLayoutPanel3, 0, 0);
			this.MidiTrackActions.Controls.Add(this.AddNewTrackBtn, 0, 1);
			this.MidiTrackActions.Controls.Add(this.tableLayoutPanel2, 0, 2);
			this.MidiTrackActions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiTrackActions.Location = new System.Drawing.Point(328, 287);
			this.MidiTrackActions.Name = "MidiTrackActions";
			this.MidiTrackActions.RowCount = 4;
			this.MidiTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MidiTrackActions.Size = new System.Drawing.Size(310, 112);
			this.MidiTrackActions.TabIndex = 14;
			// 
			// InsertNewTrackBtn
			// 
			this.InsertNewTrackBtn.AutoSize = true;
			this.InsertNewTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InsertNewTrackBtn.Enabled = false;
			this.InsertNewTrackBtn.Location = new System.Drawing.Point(158, 41);
			this.InsertNewTrackBtn.Name = "InsertNewTrackBtn";
			this.InsertNewTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.InsertNewTrackBtn.TabIndex = 3;
			this.InsertNewTrackBtn.Text = "插入新的空轨道";
			this.InsertNewTrackBtn.UseVisualStyleBackColor = true;
			this.InsertNewTrackBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.MidiTrackActions.SetColumnSpan(this.flowLayoutPanel3, 2);
			this.flowLayoutPanel3.Controls.Add(this.MoveUpBtn);
			this.flowLayoutPanel3.Controls.Add(this.MoveDownBtn);
			this.flowLayoutPanel3.Controls.Add(this.RemoveTrackBtn);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel3.Enabled = false;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(310, 38);
			this.flowLayoutPanel3.TabIndex = 0;
			// 
			// MoveUpBtn
			// 
			this.MoveUpBtn.AutoSize = true;
			this.MoveUpBtn.Location = new System.Drawing.Point(3, 3);
			this.MoveUpBtn.Name = "MoveUpBtn";
			this.MoveUpBtn.Size = new System.Drawing.Size(75, 32);
			this.MoveUpBtn.TabIndex = 0;
			this.MoveUpBtn.Text = "上移";
			this.MoveUpBtn.UseVisualStyleBackColor = true;
			this.MoveUpBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// MoveDownBtn
			// 
			this.MoveDownBtn.AutoSize = true;
			this.MoveDownBtn.Location = new System.Drawing.Point(84, 3);
			this.MoveDownBtn.Name = "MoveDownBtn";
			this.MoveDownBtn.Size = new System.Drawing.Size(75, 32);
			this.MoveDownBtn.TabIndex = 1;
			this.MoveDownBtn.Text = "下移";
			this.MoveDownBtn.UseVisualStyleBackColor = true;
			this.MoveDownBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// RemoveTrackBtn
			// 
			this.RemoveTrackBtn.AutoSize = true;
			this.RemoveTrackBtn.Location = new System.Drawing.Point(165, 3);
			this.RemoveTrackBtn.Name = "RemoveTrackBtn";
			this.RemoveTrackBtn.Size = new System.Drawing.Size(75, 32);
			this.RemoveTrackBtn.TabIndex = 2;
			this.RemoveTrackBtn.Text = "移除";
			this.RemoveTrackBtn.UseVisualStyleBackColor = true;
			this.RemoveTrackBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// AddNewTrackBtn
			// 
			this.AddNewTrackBtn.AutoSize = true;
			this.AddNewTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddNewTrackBtn.Location = new System.Drawing.Point(3, 41);
			this.AddNewTrackBtn.Name = "AddNewTrackBtn";
			this.AddNewTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.AddNewTrackBtn.TabIndex = 1;
			this.AddNewTrackBtn.Text = "添加新的空轨道";
			this.AddNewTrackBtn.UseVisualStyleBackColor = true;
			this.AddNewTrackBtn.Click += new System.EventHandler(this.ActionsClick);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.MidiTrackActions.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.MidiTrackNameLbl, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.MidiTrackNameTxt, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Enabled = false;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 76);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(310, 34);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// MidiTrackNameLbl
			// 
			this.MidiTrackNameLbl.AutoSize = true;
			this.MidiTrackNameLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiTrackNameLbl.Location = new System.Drawing.Point(3, 0);
			this.MidiTrackNameLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.MidiTrackNameLbl.Name = "MidiTrackNameLbl";
			this.MidiTrackNameLbl.Size = new System.Drawing.Size(39, 34);
			this.MidiTrackNameLbl.TabIndex = 8;
			this.MidiTrackNameLbl.Text = "名称";
			this.MidiTrackNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiTrackNameTxt
			// 
			this.MidiTrackNameTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiTrackNameTxt.Location = new System.Drawing.Point(48, 3);
			this.MidiTrackNameTxt.Name = "MidiTrackNameTxt";
			this.MidiTrackNameTxt.Size = new System.Drawing.Size(259, 27);
			this.MidiTrackNameTxt.TabIndex = 9;
			this.MidiTrackNameTxt.TextChanged += new System.EventHandler(this.MidiTrackNameTxt_TextChanged);
			// 
			// MidiChannelList
			// 
			this.MidiChannelList.AllowColumnReorder = true;
			this.MidiChannelList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MidiChannelIndex,
            this.MidiChannelName,
            this.MidiChannelValue,
            this.MidiChannelNoteCount});
			this.MidiChannelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelList.Enabled = false;
			this.MidiChannelList.FullRowSelect = true;
			this.MidiChannelList.HideSelection = false;
			this.MidiChannelList.Location = new System.Drawing.Point(644, 72);
			this.MidiChannelList.Name = "MidiChannelList";
			this.MidiChannelList.ShowItemToolTips = true;
			this.MidiChannelList.Size = new System.Drawing.Size(310, 209);
			this.MidiChannelList.TabIndex = 12;
			this.MidiChannelList.UseCompatibleStateImageBehavior = false;
			this.MidiChannelList.View = System.Windows.Forms.View.Details;
			this.MidiChannelList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.List_ItemSelectionChanged);
			this.MidiChannelList.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
			// 
			// MidiChannelIndex
			// 
			this.MidiChannelIndex.Text = "#";
			this.MidiChannelIndex.Width = 30;
			// 
			// MidiChannelName
			// 
			this.MidiChannelName.Text = "原轨道名称";
			this.MidiChannelName.Width = 130;
			// 
			// MidiChannelValue
			// 
			this.MidiChannelValue.Text = "通道值";
			// 
			// MidiChannelNoteCount
			// 
			this.MidiChannelNoteCount.Text = "音符数";
			// 
			// MidiTrackList
			// 
			this.MidiTrackList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.MidiTrackIndex,
            this.MidiTrackName,
            this.MidiTrackContain,
            this.MidiTrackNoteCount});
			this.MidiTrackList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiTrackList.FullRowSelect = true;
			this.MidiTrackList.HideSelection = false;
			this.MidiTrackList.Location = new System.Drawing.Point(328, 72);
			this.MidiTrackList.Name = "MidiTrackList";
			this.MidiTrackList.ShowItemToolTips = true;
			this.MidiTrackList.Size = new System.Drawing.Size(310, 209);
			this.MidiTrackList.TabIndex = 11;
			this.MidiTrackList.UseCompatibleStateImageBehavior = false;
			this.MidiTrackList.View = System.Windows.Forms.View.Details;
			this.MidiTrackList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.List_ItemSelectionChanged);
			this.MidiTrackList.SelectedIndexChanged += new System.EventHandler(this.List_SelectedIndexChanged);
			// 
			// MidiTrackIndex
			// 
			this.MidiTrackIndex.Text = "#";
			this.MidiTrackIndex.Width = 30;
			// 
			// MidiTrackName
			// 
			this.MidiTrackName.Text = "名称";
			this.MidiTrackName.Width = 130;
			// 
			// MidiTrackContain
			// 
			this.MidiTrackContain.Text = "已包含";
			// 
			// MidiTrackNoteCount
			// 
			this.MidiTrackNoteCount.Text = "音符数";
			// 
			// MidiChannelLbl
			// 
			this.MidiChannelLbl.AutoSize = true;
			this.MidiChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelLbl.Location = new System.Drawing.Point(644, 49);
			this.MidiChannelLbl.Name = "MidiChannelLbl";
			this.MidiChannelLbl.Size = new System.Drawing.Size(310, 20);
			this.MidiChannelLbl.TabIndex = 9;
			this.MidiChannelLbl.Text = "MIDI 通道列表";
			this.MidiChannelLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiTrackLbl
			// 
			this.MidiTrackLbl.AutoSize = true;
			this.MidiTrackLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiTrackLbl.Location = new System.Drawing.Point(328, 49);
			this.MidiTrackLbl.Name = "MidiTrackLbl";
			this.MidiTrackLbl.Size = new System.Drawing.Size(310, 20);
			this.MidiTrackLbl.TabIndex = 8;
			this.MidiTrackLbl.Text = "MIDI 轨道列表";
			this.MidiTrackLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VegasTrackLbl
			// 
			this.VegasTrackLbl.AutoSize = true;
			this.VegasTrackLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VegasTrackLbl.Location = new System.Drawing.Point(12, 49);
			this.VegasTrackLbl.Name = "VegasTrackLbl";
			this.VegasTrackLbl.Size = new System.Drawing.Size(310, 20);
			this.VegasTrackLbl.TabIndex = 7;
			this.VegasTrackLbl.Text = "Vegas 轨道列表";
			this.VegasTrackLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VegasTrackList
			// 
			this.VegasTrackList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.VegasTrackIndex,
            this.VegasTrackName,
            this.VegasTrackEventCount,
            this.VegasTrackMute,
            this.VegasTrackSolo});
			this.VegasTrackList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VegasTrackList.FullRowSelect = true;
			this.VegasTrackList.HideSelection = false;
			this.VegasTrackList.Location = new System.Drawing.Point(12, 72);
			this.VegasTrackList.Name = "VegasTrackList";
			this.VegasTrackList.ShowItemToolTips = true;
			this.VegasTrackList.Size = new System.Drawing.Size(310, 209);
			this.VegasTrackList.TabIndex = 10;
			this.VegasTrackList.UseCompatibleStateImageBehavior = false;
			this.VegasTrackList.View = System.Windows.Forms.View.Details;
			this.VegasTrackList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.List_ItemSelectionChanged);
			// 
			// VegasTrackIndex
			// 
			this.VegasTrackIndex.Text = "#";
			this.VegasTrackIndex.Width = 30;
			// 
			// VegasTrackName
			// 
			this.VegasTrackName.Text = "轨道名称";
			this.VegasTrackName.Width = 130;
			// 
			// VegasTrackEventCount
			// 
			this.VegasTrackEventCount.Text = "事件数";
			// 
			// VegasTrackMute
			// 
			this.VegasTrackMute.Text = "静音";
			this.VegasTrackMute.Width = 30;
			// 
			// VegasTrackSolo
			// 
			this.VegasTrackSolo.Text = "独奏";
			this.VegasTrackSolo.Width = 30;
			// 
			// VegasTrackActions
			// 
			this.VegasTrackActions.AutoSize = true;
			this.VegasTrackActions.ColumnCount = 2;
			this.VegasTrackActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.VegasTrackActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.VegasTrackActions.Controls.Add(this.AddToSameNewTrackBtn, 1, 0);
			this.VegasTrackActions.Controls.Add(this.AddToEachNewTrackBtn, 0, 0);
			this.VegasTrackActions.Controls.Add(this.AddToCurrentTrackBtn, 0, 1);
			this.VegasTrackActions.Controls.Add(this.PreviewTrackBtn, 1, 1);
			this.VegasTrackActions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VegasTrackActions.Enabled = false;
			this.VegasTrackActions.Location = new System.Drawing.Point(12, 287);
			this.VegasTrackActions.Name = "VegasTrackActions";
			this.VegasTrackActions.RowCount = 3;
			this.VegasTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.VegasTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.VegasTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.VegasTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.VegasTrackActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.VegasTrackActions.Size = new System.Drawing.Size(310, 112);
			this.VegasTrackActions.TabIndex = 13;
			// 
			// AddToSameNewTrackBtn
			// 
			this.AddToSameNewTrackBtn.AutoSize = true;
			this.AddToSameNewTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddToSameNewTrackBtn.Location = new System.Drawing.Point(158, 3);
			this.AddToSameNewTrackBtn.Name = "AddToSameNewTrackBtn";
			this.AddToSameNewTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.AddToSameNewTrackBtn.TabIndex = 3;
			this.AddToSameNewTrackBtn.Text = "添加到同一新轨道";
			this.AddToSameNewTrackBtn.UseVisualStyleBackColor = true;
			this.AddToSameNewTrackBtn.Click += new System.EventHandler(this.AddToTrackBtn_Click);
			// 
			// AddToEachNewTrackBtn
			// 
			this.AddToEachNewTrackBtn.AutoSize = true;
			this.AddToEachNewTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddToEachNewTrackBtn.Location = new System.Drawing.Point(3, 3);
			this.AddToEachNewTrackBtn.Name = "AddToEachNewTrackBtn";
			this.AddToEachNewTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.AddToEachNewTrackBtn.TabIndex = 0;
			this.AddToEachNewTrackBtn.Text = "添加到各自新轨道";
			this.AddToEachNewTrackBtn.UseVisualStyleBackColor = true;
			this.AddToEachNewTrackBtn.Click += new System.EventHandler(this.AddToTrackBtn_Click);
			// 
			// AddToCurrentTrackBtn
			// 
			this.AddToCurrentTrackBtn.AutoSize = true;
			this.AddToCurrentTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AddToCurrentTrackBtn.Location = new System.Drawing.Point(3, 41);
			this.AddToCurrentTrackBtn.Name = "AddToCurrentTrackBtn";
			this.AddToCurrentTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.AddToCurrentTrackBtn.TabIndex = 1;
			this.AddToCurrentTrackBtn.Text = "添加到当前轨道";
			this.AddToCurrentTrackBtn.UseVisualStyleBackColor = true;
			this.AddToCurrentTrackBtn.Click += new System.EventHandler(this.AddToTrackBtn_Click);
			// 
			// PreviewTrackBtn
			// 
			this.PreviewTrackBtn.AutoSize = true;
			this.PreviewTrackBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewTrackBtn.Location = new System.Drawing.Point(158, 41);
			this.PreviewTrackBtn.Name = "PreviewTrackBtn";
			this.PreviewTrackBtn.Size = new System.Drawing.Size(149, 32);
			this.PreviewTrackBtn.TabIndex = 2;
			this.PreviewTrackBtn.Text = "预览";
			this.PreviewTrackBtn.UseVisualStyleBackColor = true;
			this.PreviewTrackBtn.Click += new System.EventHandler(this.PreviewTrackBtn_Click);
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 4);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.FilterTrackFlow, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(9, 9);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(1264, 40);
			this.tableLayoutPanel4.TabIndex = 6;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 4;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add(this.AudioMainKeyCombo, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.AudioBasePitchLbl, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.AudioMainOctaveCombo, 1, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(1054, 3);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(207, 34);
			this.tableLayoutPanel5.TabIndex = 8;
			// 
			// AudioMainKeyCombo
			// 
			this.AudioMainKeyCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioMainKeyCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioMainKeyCombo.FormattingEnabled = true;
			this.AudioMainKeyCombo.Items.AddRange(new object[] {
            "C",
            "C#",
            "D",
            "D#",
            "E",
            "F",
            "F#",
            "G",
            "G#",
            "A",
            "A#",
            "B"});
			this.AudioMainKeyCombo.Location = new System.Drawing.Point(78, 3);
			this.AudioMainKeyCombo.Name = "AudioMainKeyCombo";
			this.AudioMainKeyCombo.Size = new System.Drawing.Size(60, 28);
			this.AudioMainKeyCombo.TabIndex = 1;
			// 
			// AudioBasePitchLbl
			// 
			this.AudioBasePitchLbl.AutoSize = true;
			this.AudioBasePitchLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioBasePitchLbl.Location = new System.Drawing.Point(3, 0);
			this.AudioBasePitchLbl.MinimumSize = new System.Drawing.Size(0, 34);
			this.AudioBasePitchLbl.Name = "AudioBasePitchLbl";
			this.AudioBasePitchLbl.Size = new System.Drawing.Size(69, 34);
			this.AudioBasePitchLbl.TabIndex = 0;
			this.AudioBasePitchLbl.Text = "原始音高";
			this.AudioBasePitchLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioMainOctaveCombo
			// 
			this.AudioMainOctaveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioMainOctaveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioMainOctaveCombo.FormattingEnabled = true;
			this.AudioMainOctaveCombo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
			this.AudioMainOctaveCombo.Location = new System.Drawing.Point(144, 3);
			this.AudioMainOctaveCombo.Name = "AudioMainOctaveCombo";
			this.AudioMainOctaveCombo.Size = new System.Drawing.Size(60, 28);
			this.AudioMainOctaveCombo.TabIndex = 2;
			// 
			// FilterTrackFlow
			// 
			this.FilterTrackFlow.AutoSize = true;
			this.FilterTrackFlow.Controls.Add(this.FilterAllRadio);
			this.FilterTrackFlow.Controls.Add(this.FilterVideoRadio);
			this.FilterTrackFlow.Controls.Add(this.FilterAudioRadio);
			this.FilterTrackFlow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FilterTrackFlow.Location = new System.Drawing.Point(0, 0);
			this.FilterTrackFlow.Margin = new System.Windows.Forms.Padding(0);
			this.FilterTrackFlow.Name = "FilterTrackFlow";
			this.FilterTrackFlow.Size = new System.Drawing.Size(1051, 40);
			this.FilterTrackFlow.TabIndex = 7;
			this.FilterTrackFlow.WrapContents = false;
			// 
			// FilterAllRadio
			// 
			this.FilterAllRadio.AutoSize = true;
			this.FilterAllRadio.Location = new System.Drawing.Point(3, 3);
			this.FilterAllRadio.Name = "FilterAllRadio";
			this.FilterAllRadio.Size = new System.Drawing.Size(90, 24);
			this.FilterAllRadio.TabIndex = 0;
			this.FilterAllRadio.Text = "所有轨道";
			this.FilterAllRadio.UseVisualStyleBackColor = true;
			// 
			// FilterVideoRadio
			// 
			this.FilterVideoRadio.AutoSize = true;
			this.FilterVideoRadio.Location = new System.Drawing.Point(99, 3);
			this.FilterVideoRadio.Name = "FilterVideoRadio";
			this.FilterVideoRadio.Size = new System.Drawing.Size(90, 24);
			this.FilterVideoRadio.TabIndex = 1;
			this.FilterVideoRadio.Text = "视频轨道";
			this.FilterVideoRadio.UseVisualStyleBackColor = true;
			// 
			// FilterAudioRadio
			// 
			this.FilterAudioRadio.AutoSize = true;
			this.FilterAudioRadio.Checked = true;
			this.FilterAudioRadio.Location = new System.Drawing.Point(195, 3);
			this.FilterAudioRadio.Name = "FilterAudioRadio";
			this.FilterAudioRadio.Size = new System.Drawing.Size(90, 24);
			this.FilterAudioRadio.TabIndex = 2;
			this.FilterAudioRadio.TabStop = true;
			this.FilterAudioRadio.Text = "音频轨道";
			this.FilterAudioRadio.UseVisualStyleBackColor = true;
			// 
			// ExportMidiFileForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(1282, 463);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.dock);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExportMidiFileForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "导出 MIDI 文件";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportMidiFileForm_FormClosing);
			this.dock.ResumeLayout(false);
			this.dock.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.MidiChannelActions.ResumeLayout(false);
			this.MidiChannelActions.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.MidiTrackActions.ResumeLayout(false);
			this.MidiTrackActions.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.VegasTrackActions.ResumeLayout(false);
			this.VegasTrackActions.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.FilterTrackFlow.ResumeLayout(false);
			this.FilterTrackFlow.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label MidiChannelLbl;
		private System.Windows.Forms.Label MidiTrackLbl;
		private System.Windows.Forms.Label VegasTrackLbl;
		private System.Windows.Forms.ListView VegasTrackList;
		private System.Windows.Forms.ColumnHeader VegasTrackIndex;
		private System.Windows.Forms.ColumnHeader VegasTrackName;
		private System.Windows.Forms.ColumnHeader VegasTrackEventCount;
		private System.Windows.Forms.ListView MidiTrackList;
		private System.Windows.Forms.ColumnHeader MidiTrackIndex;
		private System.Windows.Forms.ColumnHeader MidiTrackName;
		private System.Windows.Forms.ColumnHeader MidiTrackContain;
		private System.Windows.Forms.ListView MidiChannelList;
		private System.Windows.Forms.ColumnHeader MidiChannelIndex;
		private System.Windows.Forms.ColumnHeader MidiChannelName;
		private System.Windows.Forms.ColumnHeader MidiChannelValue;
		private System.Windows.Forms.TableLayoutPanel VegasTrackActions;
		private System.Windows.Forms.Button AddToEachNewTrackBtn;
		private System.Windows.Forms.TableLayoutPanel MidiTrackActions;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.Button MoveUpBtn;
		private System.Windows.Forms.Button MoveDownBtn;
		private System.Windows.Forms.Button RemoveTrackBtn;
		private System.Windows.Forms.Button AddNewTrackBtn;
		private System.Windows.Forms.Button AddToCurrentTrackBtn;
		private System.Windows.Forms.Button PreviewTrackBtn;
		private System.Windows.Forms.FlowLayoutPanel MidiChannelActions;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		private System.Windows.Forms.Button RemoveChannelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label ChannelValueLbl;
		private System.Windows.Forms.ComboBox ChannelValueCombo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label MidiTrackNameLbl;
		private System.Windows.Forms.TextBox MidiTrackNameTxt;
		private System.Windows.Forms.ColumnHeader MidiChannelNoteCount;
		private System.Windows.Forms.ColumnHeader MidiTrackNoteCount;
		private System.Windows.Forms.ColumnHeader VegasTrackMute;
		private System.Windows.Forms.ColumnHeader VegasTrackSolo;
		private System.Windows.Forms.Button AddToSameNewTrackBtn;
		private System.Windows.Forms.Label InstrumentLbl;
		private System.Windows.Forms.Label InstrumentTxt;
		private System.Windows.Forms.Button InsertNewTrackBtn;
		private System.Windows.Forms.CheckBox LoopRegionOnlyCheck;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label AudioBasePitchLbl;
		private System.Windows.Forms.ComboBox AudioMainOctaveCombo;
		private System.Windows.Forms.FlowLayoutPanel FilterTrackFlow;
		private System.Windows.Forms.RadioButton FilterAllRadio;
		private System.Windows.Forms.RadioButton FilterVideoRadio;
		private System.Windows.Forms.RadioButton FilterAudioRadio;
		private System.Windows.Forms.ComboBox AudioMainKeyCombo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label DispatchInstrumentLbl;
		private System.Windows.Forms.ComboBox InstrumentCombo;
		private System.Windows.Forms.ListView MidiInstrumentList;
		private System.Windows.Forms.ColumnHeader MidiInstrumentChannel;
		private System.Windows.Forms.ColumnHeader MidiInstrumentInstrument;
		private System.Windows.Forms.Label MidiInstrumentLbl;
	}
}