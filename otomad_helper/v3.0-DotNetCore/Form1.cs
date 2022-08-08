//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VegasScript {
	public partial class ConfigForm : Form {

		//private readonly EntryPoint parent;
		public bool AcceptConfig = false;

		/// <summary>
		/// ConfigForm 脚本对话框窗体的入口方法。
		/// </summary>
		/// <param name="entryPoint">调用本对象的父对象，也就是 Vegas 脚本的入口类。</param>
		public ConfigForm(/*EntryPoint entryPoint*/) {
			//parent = entryPoint;
			InitializeComponent();

			#region 复选框设置、下拉菜单默认值
			var setCheckedEnabled = new EventHandler(SetCheckedEnabled);
			VideoConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioTuneMethodCombo.SelectedIndexChanged += setCheckedEnabled;
			AudioTuneMethodCombo.SelectedIndex = 2;
			ChooseSourceCombo.SelectedIndex = 0;
			AudioMainKeyCombo.SelectedIndex = 0;
			AudioMainOctaveCombo.SelectedIndex = 5;
			VideoEffectCombo.SelectedIndex = 1;
			#endregion

			#region 程序图标
			string iconName = "otomad_helper_icon.ico";
			try {
				Icon = Icon.ExtractAssociatedIcon(@"Script Menu\" + iconName);
			} catch (Exception) { } // 如果路径不存在则不受影响
			#endregion

			#region 滑动块与数值调整框的双向绑定
			tracks.AddRange(new TrackBar[] {
				VideoStartSizeTrack,
				VideoEndSizeTrack,
				VideoFadeInTrack,
				VideoFadeOutTrack
			});
			numerics.AddRange(new NumericUpDown[] {
				VideoStartSizeBox,
				VideoEndSizeBox,
				VideoFadeInBox,
				VideoFadeOutBox
			});
			foreach (var track in tracks)
				track.Scroll += new EventHandler(Tracks_Scroll);
			foreach (var numeric in numerics)
				numeric.ValueChanged += new EventHandler(Numerics_Changed);
			#endregion

			#region 工具提示
			ToolTip balloon = new ToolTip();
			balloon.IsBalloon = true;
			balloon.SetToolTip(MidiEndSecondBox, "此处填写需要读取 MIDI 文件的时间长度。\n注意如果填写的值过小，将截去多余时间部分的音符。");
			balloon.SetToolTip(SourceStartTimeText, "此处填写媒体素材裁剪的开始时间。");
			balloon.SetToolTip(SourceEndTimeText, "注意如果此处填写的数值比入点秒数小或相等，则始终表示持续到素材时间末尾。");
			#endregion

			#region 浏览并打开媒体文件
			//ChooseMidiBtn.Click += (sender, e) => parent.SelectMidiFile();
			//ChooseSourceBtn.Click += (sender, e) => parent.SelectVideoClip();
			#endregion

			#region 裁切时间输入框的验证合法性
			MidiStartSecondBox.ValueChanged += (sender, e) => MidiTrimTime_ValueChanged(0);
			MidiEndSecondBox.ValueChanged += (sender, e) => MidiTrimTime_ValueChanged(1);
			SourceStartTimeText.Leave += (sender, e) => ClipTrimTime_TextChanged(0);
			SourceEndTimeText.Leave += (sender, e) => ClipTrimTime_TextChanged(1);
			#endregion

			MidiCustomBpmCheck.CheckedChanged += (sender, e) => { MidiCustomBpmBox.Enabled = MidiCustomBpmCheck.Checked; };
		}

		private readonly List<TrackBar> tracks = new List<TrackBar>();
		private readonly List<NumericUpDown> numerics = new List<NumericUpDown>();

		private void CancelBtn_Click(object sender, EventArgs e) {
			AcceptConfig = false;
			Close();
			// Environment.Exit(0);
			//parent.removeLastUnusedMedia();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			AcceptConfig = true;
			Close();
		}

		private void Tracks_Scroll(object sender, EventArgs e) {
			for (int i = 0; i < tracks.Count; i++)
				numerics[i].Value = tracks[i].Value;
		}

		private void Numerics_Changed(object sender, EventArgs e) {
			for (int i = 0; i < tracks.Count; i++)
				tracks[i].Value = (int)numerics[i].Value;
		}

		private void SetCheckedEnabled(object sender, EventArgs e) {
			bool isVideoConfigEnabled = VideoConfigCheck.Checked;
			VideoEffectCombo.Enabled
				= VideoStartSizeTrack.Enabled
				= VideoEndSizeTrack.Enabled
				= VideoFadeInTrack.Enabled
				= VideoFadeOutTrack.Enabled
				= VideoStartSizeBox.Enabled
				= VideoEndSizeBox.Enabled
				= VideoFadeInBox.Enabled
				= VideoFadeOutBox.Enabled
				= VideoScratchCheck.Enabled
				= VideoLoopCheck.Enabled
				= isVideoConfigEnabled;

			bool isAudioConfigEnabled = AudioConfigCheck.Checked;
			int AudioTuneMethod = AudioTuneMethodCombo.SelectedIndex;
			AudioScratchCheck.Enabled = AudioLoopCheck.Enabled = AudioTuneMethodCombo.Enabled = isAudioConfigEnabled;
			AudioMainKeyCombo.Enabled = AudioMainOctaveCombo.Enabled
				= isAudioConfigEnabled && AudioTuneMethodCombo.SelectedIndex != 0;
			if (AudioTuneMethodCombo.SelectedIndex == 3) AudioScratchCheck.Checked = AudioScratchCheck.Enabled = false;

			OkBtn.Enabled = (isVideoConfigEnabled || isAudioConfigEnabled)/* && parent.midi != null*/;
		}

		public static string aboutHelpLink = "https://www.bilibili.com/read/cv392013";
		private void UserHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			System.Diagnostics.Process.Start("explorer.exe", aboutHelpLink);
		}

		private void AboutBtn_Click(object sender, EventArgs e) {
			MessageBox.Show(
				"脚本原作者：Chaosinism\n" +
				"说明文档：" + aboutHelpLink + "\n" +
				"疑难解答：https://www.bilibili.com/read/cv495309\n" +
				"仓库地址：https://github.com/Chaosinism/vegas_script\n",
				"关于"
			);
		}

		private void MidiTrimTime_ValueChanged(int whichBox) {
			if (MidiStartSecondBox.Value > MidiEndSecondBox.Value) {
				if (whichBox == 0) MidiStartSecondBox.Value = MidiEndSecondBox.Value;
				else MidiEndSecondBox.Value = MidiStartSecondBox.Value;
			}
		}

		private void ClipTrimTime_TextChanged(int whichBox) {
			const string DEFAULT_TIME = "0:00.000";
			string startTime = SourceStartTimeText.Text.Trim(),
				endTime = SourceEndTimeText.Text.Trim();
			var dealLegal = new Func<string, string>(raw => {
				raw = new Regex(@"[:：;；]+").Replace(raw, ":");
				raw = new Regex(@"[\.。．,，、]+").Replace(raw, ".");
				raw = new Regex(@"\s").Replace(raw, "");
				raw = new Regex(@"^\.").Replace(raw, "0.");
				/*if (Regex.Matches(raw, @"[^0-9:\.]|^:|:$|:\.|\.:").Count != 0) return DEFAULT_TIME;
				raw = new Regex(@"^\.").Replace(raw, "0.");
				raw = new Regex(@"\.$").Replace(raw, ".0");
				if (Regex.Matches(raw, @"\.").Count > 1 || Regex.Matches(raw, @":").Count > 2) return DEFAULT_TIME;
				string[] rawSplit = raw.Split(':');
				for (int i = 0; i < rawSplit.Length; i++)
					if (rawSplit[i].Contains(".") && i != rawSplit.Length - 1) return DEFAULT_TIME;*/
				var matches = Regex.Matches(raw, @"(\d+:){0,2}\d+(\.\d+)?");
				if (matches.Count == 0) return DEFAULT_TIME;
				else raw = matches[0].ToString();
				return FormatClipTrimTime(raw);
			});
			startTime = dealLegal(startTime);
			endTime = dealLegal(endTime);
			if (ClipTrimTime2Ms(startTime) > ClipTrimTime2Ms(endTime)) {
				if (whichBox == 0) startTime = endTime;
				else endTime = startTime;
			}
			SourceStartTimeText.Text = startTime;
			SourceEndTimeText.Text = endTime;
		}

		public static int ClipTrimTime2Ms(string clipTrimTime) {
			int h = 0, m = 0, s = 0, ms = 0;
			string[] timeSplitDot = clipTrimTime.Split('.');
			if (timeSplitDot.Length >= 2) {
				string ms_str = timeSplitDot[1];
				while (ms_str.Length < 3) ms_str += '0';
				ms = int.Parse(ms_str);
			}
			string[] timeSplitColon = timeSplitDot[0].Split(':');
			s = int.Parse(timeSplitColon[timeSplitColon.Length - 1]); // timeSplitColon[^1]
			if (timeSplitColon.Length >= 2) m = int.Parse(timeSplitColon[timeSplitColon.Length - 2]); // timeSplitColon[^2]
			if (timeSplitColon.Length >= 3) h = int.Parse(timeSplitColon[timeSplitColon.Length - 3]); // timeSplitColon[^3]
			int value = ((h * 60 + m) * 60 + s) * 1000 + ms;
			return value;
		}

		private static string FormatClipTrimTime(string clipTrimTime) {
			int value = ClipTrimTime2Ms(clipTrimTime);
			int ms = value % 1000; value /= 1000;
			int s = value % 60; value /= 60;
			int m = value % 60; value /= 60;
			int h = value;
			return h == 0 ?
				string.Format("{0:D}:{1:D2}.{2:D3}", m, s, ms) :
				string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", h, m, s, ms);
		}

		/*public VideoAnimFx VideoEffect = VideoAnimFx.NONE;
		private void VideoEffectCombo_SelectedIndexChanged(object sender, EventArgs e) {
			VideoEffect = (VideoAnimFx)VideoEffectCombo.SelectedIndex;
		}*/

		public double SourceStartTimeValue { get { return (double)ClipTrimTime2Ms(SourceStartTimeText.Text); } }
		public double SourceEndTimeValue { get { return (double)ClipTrimTime2Ms(SourceEndTimeText.Text); } }

		private void ChooseSourceCombo_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void VideoEffectCombo_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void SelectOneEveryFewBox_ValueChanged(object sender, EventArgs e) {

		}

		private void QuickSelectIntervalBtn_Click(object sender, EventArgs e) {

		}
	}
}
