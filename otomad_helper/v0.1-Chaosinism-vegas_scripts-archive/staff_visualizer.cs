
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using ScriptPortal.Vegas;
using NAudio.Midi;

public class EntryPoint {
	

    int[] pitchMap={0,0,1,1,2,3,3,4,4,5,5,6};
	
	String midiName;
	String clipName;
	
	int sheetWidth=1500;
	int sheetPosition=-225;
	int sheetGap=44;
    int sheetCelf=0;
	int sheetTempo=4;
	
	int midiTrack=15;
	
	
	partial class Form2: Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(697, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MIDI属性";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "轨道";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "拍子";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(89, 35);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(583, 26);
            this.comboBox1.TabIndex = 2;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "2/4",
            "3/4",
            "4/4"});
            this.comboBox2.Location = new System.Drawing.Point(89, 75);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(160, 26);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.Text = "4/4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox3);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.comboBox3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(22, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(697, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "视觉属性";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "谱号";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "高音",
            "低音"});
            this.comboBox3.Location = new System.Drawing.Point(187, 37);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(146, 26);
            this.comboBox3.TabIndex = 5;
            this.comboBox3.Text = "高音";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "谱面宽度（像素）";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(361, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(152, 18);
            this.label5.TabIndex = 7;
            this.label5.Text = "谱面位置（像素）";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(361, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(152, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "谱线间距（像素）";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(187, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(146, 28);
            this.textBox1.TabIndex = 9;
			this.textBox1.Text= "1000";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(519, 35);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(146, 28);
            this.textBox2.TabIndex = 10;
			this.textBox2.Text= "45";
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(519, 80);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(146, 28);
            this.textBox3.TabIndex = 11;
			this.textBox3.Text= "0";
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(584, 299);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "完成";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 343);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox comboBox2;
        public System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
		
		public String width="1000",position="0",gap="50";
		
		public Form2()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
			gap=textBox2.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
			width=textBox1.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
			position=textBox3.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
			Close();
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
        MessageBox.Show("请选择一个视频或图片素材片段。");		   
		openFileDialog.Filter="所有文件|*.*";   
		openFileDialog.RestoreDirectory=true;   
		openFileDialog.FilterIndex=1;   
		if   (openFileDialog.ShowDialog()==DialogResult.OK)   {   
			clipName=openFileDialog.FileName;   
		}else{return;}		
		Media media = new Media(clipName);
		double mediaLength=media.Length.ToMilliseconds();
		
		// start configuration
		Form2 configForm=new Form2();
		for (int i = 1; i < midi.Events.Tracks; i++){
			configForm.comboBox1.Items.Add(trackInfo[i]);
		} 
		configForm.comboBox1.SelectedIndex=0;
		Application.Run(configForm);	
		
		// apply condiguration
		for (int i = 1; i < midi.Events.Tracks; i++) if (trackInfo[i]==configForm.comboBox1.SelectedItem.ToString()){
			midiTrack=i;
		} 
		sheetWidth=int.Parse(configForm.width);
		sheetPosition=int.Parse(configForm.position);
		sheetGap=int.Parse(configForm.gap);
		if (configForm.comboBox2.Text=="2/4"){
			sheetTempo=2;
		}
		if (configForm.comboBox2.Text=="3/4"){
			sheetTempo=3;
		}
		if (configForm.comboBox3.Text=="低音"){
			sheetCelf=1;
		}
				
		// start processing MIDI
		VideoTrack[] noteTracks=new VideoTrack[100];
		int trackCount=-1;
		int trackPointer=0;
		double barStartTime=0;
		double barLength=msPerQuarter*sheetTempo;
		
		foreach (MidiEvent midiEvent in midi.Events[midiTrack]){

			if (midiEvent is NoteOnEvent){
				NoteEvent noteEvent = midiEvent as NoteEvent;
				NoteOnEvent noteOnEvent = midiEvent as NoteOnEvent;
				double startTime=midiEvent.AbsoluteTime*msPerQuarter/ticksPerQuarter;
				double duration=noteOnEvent.NoteLength*msPerQuarter/ticksPerQuarter;				
				int pitch=noteEvent.NoteNumber;
				
				
				// next page
				while (startTime>=barStartTime+barLength) {
					barStartTime=barStartTime+barLength;
					trackPointer=0;
				}
				
				// generate video events
				if (trackPointer>trackCount) {
					trackCount=trackCount+1;
					noteTracks[trackCount]=vegas.Project.AddVideoTrack();
				}
				
				VideoEvent videoEvent = noteTracks[trackPointer].AddVideoEvent(Timecode.FromMilliseconds(startTime), Timecode.FromMilliseconds(barStartTime+barLength-startTime));
				Take take = videoEvent.AddTake(media.GetVideoStreamByIndex(0));
				TrackEvent trackEvent = videoEvent as TrackEvent;
				trackEvent.Loop=true;				
				
				TrackMotionKeyframe keyFrame=noteTracks[trackPointer].TrackMotion.InsertMotionKeyframe(Timecode.FromMilliseconds(startTime));
				keyFrame.Type=VideoKeyframeType.Hold;
				keyFrame.Width=sheetGap*2*vegas.Project.Video.Width/vegas.Project.Video.Height;
				keyFrame.Height=sheetGap*2;
				keyFrame.PositionX=-sheetWidth/2+sheetWidth/barLength*(startTime-barStartTime);
				int octave=pitch/12;
				int line=pitchMap[pitch%12];
				keyFrame.PositionY=sheetPosition-sheetGap*3+(octave-5)*sheetGap*3.5+line*sheetGap*0.5+sheetCelf*12;
				
				trackPointer=trackPointer+1;

			}
		}		

		

	}
}