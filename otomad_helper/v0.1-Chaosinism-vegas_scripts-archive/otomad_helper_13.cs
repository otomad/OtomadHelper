
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using Sony.Vegas;
using NAudio.Midi;

public class EntryPoint {
	
	public Dictionary<string, int> pitchMap = new Dictionary<string, int>
    {
		{"C0", 0},
		{"C#0", 1},
		{"D0", 2},
		{"D#0", 3},
		{"E0", 4},
		{"F0", 5},
		{"F#0", 6},
		{"G0", 7},
		{"G#0", 8},
		{"A0", 9},
		{"A#0", 10},
		{"B0", 11},
		{"C1", 12},
		{"C#1", 13},
		{"D1", 14},
		{"D#1", 15},
		{"E1", 16},
		{"F1", 17},
		{"F#1", 18},
		{"G1", 19},
		{"G#1", 20},
		{"A1", 21},
		{"A#1", 22},
		{"B1", 23},
		{"C2", 24},
		{"C#2", 25},
		{"D2", 26},
		{"D#2", 27},
		{"E2", 28},
		{"F2", 29},
		{"F#2", 30},
		{"G2", 31},
		{"G#2", 32},
		{"A2", 33},
		{"A#2", 34},
		{"B2", 35},
		{"C3", 36},
		{"C#3", 37},
		{"D3", 38},
		{"D#3", 39},
		{"E3", 40},
		{"F3", 41},
		{"F#3", 42},
		{"G3", 43},
		{"G#3", 44},
		{"A3", 45},
		{"A#3", 46},
		{"B3", 47},
		{"C4", 48},
		{"C#4", 49},
		{"D4", 50},
		{"D#4", 51},
		{"E4", 52},
		{"F4", 53},
		{"F#4", 54},
		{"G4", 55},
		{"G#4", 56},
		{"A4", 57},
		{"A#4", 58},
		{"B4", 59},
		{"C5", 60},
		{"C#5", 61},
		{"D5", 62},
		{"D#5", 63},
		{"E5", 64},
		{"F5", 65},
		{"F#5", 66},
		{"G5", 67},
		{"G#5", 68},
		{"A5", 69},
		{"A#5", 70},
		{"B5", 71},
		{"C6", 72},
		{"C#6", 73},
		{"D6", 74},
		{"D#6", 75},
		{"E6", 76},
		{"F6", 77},
		{"F#6", 78},
		{"G6", 79},
		{"G#6", 80},
		{"A6", 81},
		{"A#6", 82},
		{"B6", 83},
		{"C7", 84},
		{"C#7", 85},
		{"D7", 86},
		{"D#7", 87},
		{"E7", 88},
		{"F7", 89},
		{"F#7", 90},
		{"G7", 91},
		{"G#7", 92},
		{"A7", 93},
		{"A#7", 94},
		{"B7", 95},
		{"C8", 96},
		{"C#8", 97},
		{"D8", 98},
		{"D#8", 99},
		{"E8", 100},
		{"F8", 101},
		{"F#8", 102},
		{"G8", 103},
		{"G#8", 104},
		{"A8", 105},
		{"A#8", 106},
		{"B8", 107},

    };

	
	String midiName;
	String clipName;
	bool vconfig=true;
	bool vconfigAutoFlip=false;
	float vconfigStartSize=100;
	float vconfigEndSize=100;
	int vconfigFadein=0;
	int vconfigFadeout=0;
	bool aconfig=true;
	int aconfigTrack=1;
	int aconfigBasePitch=60;
	bool aconfigNoTune=false;
	double configStartTime=0;
	double configEndTime=30000;

	public partial class ConfigForm : Form
    {
		       private System.ComponentModel.IContainer components = null;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        private void InitializeComponent()
        {
            this.checkBoxA = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxNoTune = new System.Windows.Forms.CheckBox();
            this.comboBoxTrack = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxA2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxA1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.hScrollBar3 = new System.Windows.Forms.HScrollBar();
            this.hScrollBar4 = new System.Windows.Forms.HScrollBar();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.checkBoxFlip = new System.Windows.Forms.CheckBox();
            this.checkBoxV = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxA
            // 
            this.checkBoxA.AutoSize = true;
            this.checkBoxA.Checked = true;
            this.checkBoxA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxA.Location = new System.Drawing.Point(25, 27);
            this.checkBoxA.Name = "checkBoxA";
            this.checkBoxA.Size = new System.Drawing.Size(106, 22);
            this.checkBoxA.TabIndex = 0;
            this.checkBoxA.Text = "生成音频";
            this.checkBoxA.UseVisualStyleBackColor = true;
            this.checkBoxA.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxNoTune);
            this.groupBox1.Controls.Add(this.comboBoxTrack);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBoxA2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxA1);
            this.groupBox1.Controls.Add(this.checkBoxA);
            this.groupBox1.Location = new System.Drawing.Point(21, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 223);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "音频设置";
            // 
            // checkBoxNoTune
            // 
            this.checkBoxNoTune.AutoSize = true;
            this.checkBoxNoTune.Location = new System.Drawing.Point(25, 187);
            this.checkBoxNoTune.Name = "checkBoxNoTune";
            this.checkBoxNoTune.Size = new System.Drawing.Size(88, 22);
            this.checkBoxNoTune.TabIndex = 6;
            this.checkBoxNoTune.Text = "不调音";
            this.checkBoxNoTune.UseVisualStyleBackColor = true;
            this.checkBoxNoTune.CheckedChanged += new System.EventHandler(this.checkBoxNoTune_CheckedChanged);
            // 
            // comboBoxTrack
            // 
            this.comboBoxTrack.FormattingEnabled = true;
            this.comboBoxTrack.Location = new System.Drawing.Point(25, 144);
            this.comboBoxTrack.Name = "comboBoxTrack";
            this.comboBoxTrack.Size = new System.Drawing.Size(501, 26);
            this.comboBoxTrack.TabIndex = 5;
            this.comboBoxTrack.SelectedIndexChanged += new System.EventHandler(this.comboBoxTrack_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "使用MIDI轨道";
            // 
            // comboBoxA2
            // 
            this.comboBoxA2.FormattingEnabled = true;
            this.comboBoxA2.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.comboBoxA2.Location = new System.Drawing.Point(108, 83);
            this.comboBoxA2.Name = "comboBoxA2";
            this.comboBoxA2.Size = new System.Drawing.Size(77, 26);
            this.comboBoxA2.TabIndex = 3;
			this.comboBoxA2.SelectedIndex=4;
            this.comboBoxA2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "素材原始音高";

            // 
            // comboBoxA1
            // 
            this.comboBoxA1.FormattingEnabled = true;
            this.comboBoxA1.Items.AddRange(new object[] {
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
            this.comboBoxA1.Location = new System.Drawing.Point(25, 83);
            this.comboBoxA1.Name = "comboBoxA1";
            this.comboBoxA1.Size = new System.Drawing.Size(77, 26);
            this.comboBoxA1.TabIndex = 1;
			this.comboBoxA1.SelectedIndex=0;
            this.comboBoxA1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.hScrollBar3);
            this.groupBox2.Controls.Add(this.hScrollBar4);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.hScrollBar2);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.hScrollBar1);
            this.groupBox2.Controls.Add(this.checkBoxFlip);
            this.groupBox2.Controls.Add(this.checkBoxV);
            this.groupBox2.Location = new System.Drawing.Point(21, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 223);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "视频设置";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(491, 194);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 18);
            this.label9.TabIndex = 17;
            this.label9.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(491, 160);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 18);
            this.label10.TabIndex = 16;
            this.label10.Text = "0";
            // 
            // hScrollBar3
            // 
            this.hScrollBar3.LargeChange = 1;
            this.hScrollBar3.Location = new System.Drawing.Point(108, 188);
            this.hScrollBar3.Maximum = 50;
            this.hScrollBar3.Name = "hScrollBar3";
            this.hScrollBar3.Size = new System.Drawing.Size(380, 24);
            this.hScrollBar3.TabIndex = 15;
            this.hScrollBar3.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar3_Scroll);
            // 
            // hScrollBar4
            // 
            this.hScrollBar4.LargeChange = 1;
            this.hScrollBar4.Location = new System.Drawing.Point(108, 154);
            this.hScrollBar4.Maximum = 50;
            this.hScrollBar4.Name = "hScrollBar4";
            this.hScrollBar4.Size = new System.Drawing.Size(380, 24);
            this.hScrollBar4.TabIndex = 14;
            this.hScrollBar4.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar4_Scroll);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 18);
            this.label8.TabIndex = 13;
            this.label8.Text = "渐出";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 160);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 18);
            this.label7.TabIndex = 12;
            this.label7.Text = "渐入";

            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(491, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 18);
            this.label5.TabIndex = 11;
            this.label5.Text = "100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "终止尺寸";

            // 
            // hScrollBar2
            // 
            this.hScrollBar2.LargeChange = 1;
            this.hScrollBar2.Location = new System.Drawing.Point(108, 118);
            this.hScrollBar2.Maximum = 200;
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(380, 24);
            this.hScrollBar2.TabIndex = 9;
            this.hScrollBar2.Value = 100;
            this.hScrollBar2.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar2_Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(491, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "100";

            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "起始尺寸";

            // 
            // hScrollBar1
            // 
            this.hScrollBar1.LargeChange = 1;
            this.hScrollBar1.Location = new System.Drawing.Point(108, 84);
            this.hScrollBar1.Maximum = 200;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(380, 24);
            this.hScrollBar1.TabIndex = 2;
            this.hScrollBar1.Value = 100;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // checkBoxFlip
            // 
            this.checkBoxFlip.AutoSize = true;
            this.checkBoxFlip.Location = new System.Drawing.Point(25, 55);
            this.checkBoxFlip.Name = "checkBoxFlip";
            this.checkBoxFlip.Size = new System.Drawing.Size(142, 22);
            this.checkBoxFlip.TabIndex = 1;
            this.checkBoxFlip.Text = "自动左右翻转";
            this.checkBoxFlip.UseVisualStyleBackColor = true;
            this.checkBoxFlip.CheckedChanged += new System.EventHandler(this.checkBoxFlip_CheckedChanged);
            // 
            // checkBoxV
            // 
            this.checkBoxV.AutoSize = true;
            this.checkBoxV.Checked = true;
            this.checkBoxV.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxV.Location = new System.Drawing.Point(25, 27);
            this.checkBoxV.Name = "checkBoxV";
            this.checkBoxV.Size = new System.Drawing.Size(106, 22);
            this.checkBoxV.TabIndex = 0;
            this.checkBoxV.Text = "生成视频";
            this.checkBoxV.UseVisualStyleBackColor = true;
            this.checkBoxV.CheckedChanged += new System.EventHandler(this.checkBoxV_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(463, 510);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 38);
            this.button1.TabIndex = 8;
            this.button1.Text = "完成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 520);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 18);
            this.label11.TabIndex = 9;
            this.label11.Text = "起始秒数";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(210, 520);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 18);
            this.label12.TabIndex = 10;
            this.label12.Text = "终止秒数";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(98, 517);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(61, 28);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "0";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(296, 517);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(61, 28);
            this.textBox2.TabIndex = 12;
            this.textBox2.Text = "30";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(588, 560);
			this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ConfigForm";
            this.Text = "脚本设置";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.CheckBox checkBoxA;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBoxA1;
        public System.Windows.Forms.ComboBox comboBoxA2;
        public System.Windows.Forms.ComboBox comboBoxTrack;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.CheckBox checkBoxNoTune;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.CheckBox checkBoxV;
        public System.Windows.Forms.CheckBox checkBoxFlip;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.HScrollBar hScrollBar2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.HScrollBar hScrollBar3;
        public System.Windows.Forms.HScrollBar hScrollBar4;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.TextBox textBox1;
        public System.Windows.Forms.TextBox textBox2;
		public String startT="0";
		public String endT="30";
		
		
        public ConfigForm()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label4.Text = hScrollBar1.Value.ToString();

        }


        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            label5.Text = hScrollBar2.Value.ToString();

        }


        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            label10.Text = hScrollBar4.Value.ToString();

        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            label9.Text = hScrollBar3.Value.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
			Close();
        }

        private void checkBoxV_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxTrack_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxNoTune_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxFlip_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			startT=textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
			endT=textBox2.Text;
        }
    }
	
    public void FromVegas(Vegas vegas) {

		
		// select a midi file
        MessageBox.Show("请选择一个MIDI文件。");
		OpenFileDialog openFileDialog=new OpenFileDialog();     
		openFileDialog.Filter="*.mid|*.mid|所有文件|*.*";   
		openFileDialog.RestoreDirectory=true;   
		openFileDialog.FilterIndex=1;   
		if   (openFileDialog.ShowDialog()==DialogResult.OK)   {   
			midiName=openFileDialog.FileName;   
		}else{return;}

		MidiFile midi = new MidiFile(midiName);
		
		// generate statistics of each midi track
		String[] trackInfo=new String[midi.Events.Tracks];
		int ticksPerQuarter=midi.DeltaTicksPerQuarterNote;
		double msPerQuarter=0;
		for (int i = 0; i < midi.Events.Tracks; i++){
			String info1="轨道 "+i.ToString()+": ";
			String info2="";
			int notesCount=0;
			String info3="起音 ";

			foreach (MidiEvent midiEvent in midi.Events[i]){

				if ((midiEvent is NoteEvent) && !(midiEvent is NoteOnEvent)){
					NoteEvent noteEvent = midiEvent as NoteEvent;
					if (notesCount==0){
						info3=info3+noteEvent.NoteName;
					}
					notesCount++;
				}
				if ((midiEvent is PatchChangeEvent) && info2.Length==0){
					PatchChangeEvent patchEvent = midiEvent as PatchChangeEvent;
					for (int j=4;j<patchEvent.ToString().Split(' ').Length;j++) info2+=patchEvent.ToString().Split(' ')[j];
				}
				if ((midiEvent is TempoEvent) && msPerQuarter==0){
					TempoEvent tempoEvent = midiEvent as TempoEvent;
					msPerQuarter=Convert.ToDouble(tempoEvent.MicrosecondsPerQuarterNote)/1000;
				}
			}

		trackInfo[i]=info1+info2+"; 音符数: "+notesCount.ToString()+"; "+info3;
		}
	
		// select a video clip
        MessageBox.Show("请选择一个视频或音频素材片段。");		   
		openFileDialog.Filter="所有文件|*.*";   
		openFileDialog.RestoreDirectory=true;   
		openFileDialog.FilterIndex=1;   
		if   (openFileDialog.ShowDialog()==DialogResult.OK)   {   
			clipName=openFileDialog.FileName;   
		}else{return;}		
		Media media = new Media(clipName);
		double mediaLength=media.Length.ToMilliseconds();
	
		// start configuration
		ConfigForm configForm=new ConfigForm();
		for (int i = 0; i < midi.Events.Tracks; i++){
			configForm.comboBoxTrack.Items.Add(trackInfo[i]);
		} 
		configForm.comboBoxTrack.SelectedIndex=0;
		Application.Run(configForm);	
		
		// apply configuration
		aconfig=configForm.checkBoxA.Checked;
		aconfigNoTune=configForm.checkBoxNoTune.Checked;
		vconfig=configForm.checkBoxV.Checked;
		vconfigAutoFlip=configForm.checkBoxFlip.Checked;
		vconfigStartSize=configForm.hScrollBar1.Value;
		vconfigEndSize=configForm.hScrollBar2.Value;
		vconfigFadein=configForm.hScrollBar4.Value;
		vconfigFadeout=configForm.hScrollBar3.Value;
		aconfigBasePitch=pitchMap[configForm.comboBoxA1.SelectedItem.ToString()+configForm.comboBoxA2.SelectedItem.ToString()];
		for (int i = 0; i < midi.Events.Tracks; i++) if (trackInfo[i]==configForm.comboBoxTrack.SelectedItem.ToString()){
			aconfigTrack=i;
		} 
		configStartTime=Convert.ToDouble(configForm.startT)*1000;
		configEndTime=Convert.ToDouble(configForm.endT)*1000;
		
		// start processing MIDI
		VideoTrack vTrack = vegas.Project.AddVideoTrack();
		AudioTrack[] aTracks=new AudioTrack[20];
		double vTrackPosition=0;
		int vTrackDirection=1;
		double[] aTrackPositions=new double[20];
		aTracks[0]=vegas.Project.AddAudioTrack();
		aTrackPositions[0]=0;
		int aTrackCount=1;
		
		foreach (MidiEvent midiEvent in midi.Events[aconfigTrack]){

			if (midiEvent is NoteOnEvent){
				NoteEvent noteEvent = midiEvent as NoteEvent;
				NoteOnEvent noteOnEvent = midiEvent as NoteOnEvent;
				double startTime=midiEvent.AbsoluteTime*msPerQuarter/ticksPerQuarter;
				double duration=noteOnEvent.NoteLength*msPerQuarter/ticksPerQuarter;
				int pitch=noteEvent.NoteNumber;
				int trackIndex=0;
				
				if (startTime<configStartTime) continue;
				if (startTime>configEndTime) break;
				
				// generate audio events
				if (aconfig==true){
					while (startTime<aTrackPositions[trackIndex]){
						trackIndex++;
						if (trackIndex==aTrackCount){
							aTrackCount++;
							aTracks[trackIndex]=vegas.Project.AddAudioTrack();
						}
					}
					AudioEvent audioEvent = aTracks[trackIndex].AddAudioEvent(Timecode.FromMilliseconds(startTime), Timecode.FromMilliseconds(duration));
					Take take = audioEvent.AddTake(media.GetAudioStreamByIndex(0));
					aTrackPositions[trackIndex]=startTime+duration;
					TrackEvent trackEvent = audioEvent as TrackEvent;
					trackEvent.PlaybackRate = mediaLength/duration;
					trackEvent.Loop=false;
					
					// apply pitch shifting

					if (aconfigNoTune==false){
						int pitchDelta=pitch-aconfigBasePitch;
						if (pitchDelta>0){
							while(pitchDelta>12){
								PlugInNode plugIn0 = vegas.AudioFX.FindChildByName("Pitch Shift");
								Effect effect0 = new Effect(plugIn0);
								audioEvent.Effects.Add(effect0);
								effect0.Preset="12";
								pitchDelta-=12;
							}
							PlugInNode plugIn = vegas.AudioFX.FindChildByName("Pitch Shift");
							Effect effect = new Effect(plugIn);
							audioEvent.Effects.Add(effect);
							effect.Preset=pitchDelta.ToString();
						}else{
							while(pitchDelta<-12){
								PlugInNode plugIn0 = vegas.AudioFX.FindChildByName("Pitch Shift");
								Effect effect0 = new Effect(plugIn0);
								audioEvent.Effects.Add(effect0);
								effect0.Preset="-12";
								pitchDelta+=12;
							}
							PlugInNode plugIn = vegas.AudioFX.FindChildByName("Pitch Shift");
							Effect effect = new Effect(plugIn);
							audioEvent.Effects.Add(effect);
							effect.Preset=pitchDelta.ToString();							
						}
					}
					
				}
				
				// generate video events
				if (vconfig==true) {
					vTrackPosition=startTime+duration;
					VideoEvent videoEvent = vTrack.AddVideoEvent(Timecode.FromMilliseconds(startTime), Timecode.FromMilliseconds(duration));
					Take take = videoEvent.AddTake(media.GetVideoStreamByIndex(0));
					TrackEvent trackEvent = videoEvent as TrackEvent;
					trackEvent.PlaybackRate = mediaLength/duration;
					trackEvent.Loop=false;				
					
					videoEvent.FadeIn.Length = Timecode.FromMilliseconds(duration*vconfigFadein/100);
					videoEvent.FadeOut.Length = Timecode.FromMilliseconds(duration*vconfigFadeout/100);
					
					VideoMotionKeyframe key0 = videoEvent.VideoMotion.Keyframes[0];
					VideoMotionKeyframe key1 = new VideoMotionKeyframe(Timecode.FromMilliseconds(duration));
					videoEvent.VideoMotion.Keyframes.Add(key1);
					key0.ScaleBy(new VideoMotionVertex((vconfigStartSize/100)*vTrackDirection,(vconfigStartSize/100)));
					key1.ScaleBy(new VideoMotionVertex((vconfigEndSize/100)*vTrackDirection,(vconfigEndSize/100)));
					
					if (vconfigAutoFlip==true) vTrackDirection*=-1;
				}	
			}
		}		

		

	}
}