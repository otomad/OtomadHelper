using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScriptPortal.Vegas;

namespace VegasScript {

	public partial class ConfigForm : Form {

		public bool AcceptConfig = false;
		#if VEGAS_ENVIRONMENT
		public IniFile configIni { get { return parent.configIni; } set { parent.configIni = value; } }
		public readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		#endif

		/// <summary>
		/// ConfigForm 脚本对话框窗体的入口方法。
		/// </summary>
		/// <param name="entryPoint">调用本对象的父对象，也就是 Vegas 脚本的入口类</param>
		public ConfigForm(/*EntryPoint entryPoint*/) {
			InitializeComponent();
			MidiCustomBpmCheck.CheckedChanged += (sender, e) => { MidiCustomBpmBox.Enabled = MidiCustomBpmCheck.Checked; };
			#if VEGAS_ENVIRONMENT
			parent = entryPoint;

			#region MIDI 速度控制点击事件
			MidiProjectBpmCheck.Text = "项目速度：" + ProcessBpmDouble(parent.ProjectBpm);
			MidiCustomBpmBox.Value = (decimal)Math.Max(parent.ProjectBpm, (double)MidiCustomBpmBox.Minimum);
			#endregion

			#region 浏览并打开媒体文件
			ChooseMidiBtn.Click += (sender, e) => parent.SelectMidiFile();
			ChooseSourceBtn.Click += (sender, e) => parent.SelectVideoClip();
			parent.AudioVideoEnabledTable_Init();
			if (parent.disabledSelectIntervalPart) DisabledSelectIntervalPart();
			ChooseSourceCombo_SelectedIndexChanged(null, null);
			#endregion
			#endif

			#region 复选框设置、下拉菜单默认值
			EventHandler setCheckedEnabled = new EventHandler(SetCheckedEnabled);
			VideoConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioConfigCheck.CheckedChanged += setCheckedEnabled;
			StaffVisualizerConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioTuneMethodCombo.SelectedIndexChanged += setCheckedEnabled;
			VideoEffectCombo.SelectedIndexChanged += setCheckedEnabled;
			Tabs.SelectedIndexChanged += setCheckedEnabled;
			#if VEGAS_ENVIRONMENT
			string configIniName = "otomad_helper.ini";
			configIni = new IniFile(Path.r(vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData), configIniName).FullPath, this);
			VideoEndSizeCurveCombo.SelectedIndex = 0;
			ReadIni();
			#endif
			#endregion

			#region 程序图标
			#if VEGAS_ENVIRONMENT
			string iconName = "otomad_helper.ico";
			try {
				Icon = Icon.ExtractAssociatedIcon(Path.r(vegas.InstallationDirectory, "Script Menu", iconName).FullPath);
			} catch (Exception) { } // 如果路径不存在则不受影响
			#endif
			#endregion

			#region 裁切时间输入框的验证合法性
			//MidiStartSecondBox.ValueChanged += (sender, e) => MidiTrimTime_ValueChanged(0);
			//MidiEndSecondBox.ValueChanged += (sender, e) => MidiTrimTime_ValueChanged(1);
			//SourceStartTimeText.Leave += (sender, e) => ClipTrimTime_TextChanged(1);
			//SourceEndTimeText.Leave += (sender, e) => ClipTrimTime_TextChanged(1);
			#endregion
		}

		/// <summary>
		/// 读取配置。
		/// </summary>
		public void ReadIni() {
			#if VEGAS_ENVIRONMENT
			try {
			#region 音频配置
				configIni.StartSection("Audio");
				AudioScratchCheck.Checked = configIni.ReadBool("Scratch", false);
				AudioLoopCheck.Checked = configIni.ReadBool("Loop", false);
				AudioNormalizeCheck.Checked = configIni.ReadBool("Normalize", false);
				SetTrackBarValue(AudioFadeInTrack, AudioFadeInBox, configIni.ReadInt("FadeIn", 0), 0);
				SetTrackBarValue(AudioFadeOutTrack, AudioFadeOutBox, configIni.ReadInt("FadeOut", 0), 0);
				SetComboIndex(AudioFadeInCurveCombo, configIni.ReadInt("FadeInCurve", 1), 1);
				SetComboIndex(AudioFadeOutCurveCombo, configIni.ReadInt("FadeOutCurve", 2), 2);
				SetBasePitchCombo(configIni.Read("BasePitch", "C5"));
				SetComboIndex(AudioTuneMethodCombo, configIni.ReadInt("TuneMethod", 3), 3);
				configIni.EndSection();
			#endregion

			#region 视频配置
				configIni.StartSection("Video");
				SetComboIndex(VideoEffectCombo, configIni.ReadInt("AnimationEffect", 1), 1);
				VideoScratchCheck.Checked = configIni.ReadBool("Scratch", false);
				VideoLoopCheck.Checked = configIni.ReadBool("Loop", true);
				VideoFreezeFirstFrameCheck.Checked = configIni.ReadBool("FreezeFirstFrame", false);
				SetTrackBarValue(VideoStartSizeTrack, VideoStartSizeBox, configIni.ReadInt("StartSize", 100), 100);
				SetTrackBarValue(VideoEndSizeTrack, VideoEndSizeBox, configIni.ReadInt("EndSize", 100), 100);
				SetTrackBarValue(VideoFadeInTrack, VideoFadeInBox, configIni.ReadInt("FadeIn", 0), 0);
				SetTrackBarValue(VideoFadeOutTrack, VideoFadeOutBox, configIni.ReadInt("FadeOut", 0), 0);
				SetComboIndex(VideoStartSizeCurveCombo, configIni.ReadInt("StartSizeCurve", 1), 1);
				SetComboIndex(VideoFadeInCurveCombo, configIni.ReadInt("FadeInCurve", 3), 3);
				SetComboIndex(VideoFadeOutCurveCombo, configIni.ReadInt("FadeOutCurve", 3), 3);
				configIni.EndSection();
			#endregion

			#region 素材配置
				configIni.StartSection("Source");
				if (parent.SuggestSelectedSourceFrom == MediaSourceFrom.LAST_USER_PREFERENCE || parent.SuggestSelectedSourceFrom == MediaSourceFrom.NOTHING_SELECTED) {
					int lastMediaSourceFrom = configIni.ReadInt("LastMediaSourceFrom", 2);
					if (lastMediaSourceFrom == 0 || lastMediaSourceFrom == 1) ChooseSourceCombo.SelectedIndex = lastMediaSourceFrom;
					else if (parent.SuggestSelectedSourceFrom != MediaSourceFrom.NOTHING_SELECTED) ChooseSourceCombo.SelectedIndex = (int)MediaSourceFrom.SELECTED_CLIP;
					else ChooseSourceCombo.SelectedIndex = (int)MediaSourceFrom.SELECTED_MEDIA;
				} else ChooseSourceCombo.SelectedIndex = (int)parent.SuggestSelectedSourceFrom;
				if (parent.OpenMidiFile(configIni.Read("LastMidiFile", null), this, true))
					SetComboIndex(MidiChannelCombo, configIni.ReadInt("LastMidiChannel", -1), -1);
				else RemoveLastMidiConfig();
				configIni.EndSection();
			#endregion

			#region 五线谱配置
				configIni.StartSection("Staff");
				SetComboIndex(MidiBeatCombo, configIni.ReadInt("Beat", 4) - 2, 4 - 2);
				StaffVisualizerConfigCheck.Checked = configIni.ReadBool("Enable", false);
				StaffGenerateCheck.Checked = configIni.ReadBool("GenerateStaff", false);
				SetComboIndex(StaffClefCombo, configIni.ReadInt("Clef", 0), 0);
				SetNumericValue(StaffLineSpacingBox, configIni.ReadInt("Gap", 44), 44); // 45
				SetNumericValue(StaffSurfaceWidthBox, configIni.ReadInt("Width", 1500), 1500); // 1000
				SetNumericValue(StaffSurfacePositionBox, configIni.ReadInt("Position", -225), -225); // 0
				SetNumericValue(StaffLineThicknessBox, configIni.ReadDouble("Thickness", 0.2), 0.2);
				ReadStaffLineColor(configIni.Read("Color", "#000000"));
				configIni.EndSection();
			#endregion

			#region 间隔选择配置
				configIni.StartSection("SelectInterval");
				SetNumericValue(SelectOneEveryFewBox, configIni.ReadInt("SelectOneEveryFew", 2), 2);
				SetNumericValue(SelectWhichEachGroupBox, configIni.ReadInt("SelectWhichEachGroup", 1), 1);
				configIni.EndSection();
			#endregion

			} catch (Exception e) {
				parent.ShowError(new Exceptions.ReadConfigFailException(), e);
				configIni.Delete(true);
				AcceptConfig = false;
				Close();
			}
			#endif
		}

		/// <summary>
		/// 保存配置。
		/// </summary>
		public void SaveIni() {
			#if VEGAS_ENVIRONMENT
			#region 音频配置
			configIni.StartSection("Audio");
			configIni.Write("Scratch", AudioScratchCheck.Checked);
			configIni.Write("Loop", AudioLoopCheck.Checked);
			configIni.Write("Normalize", AudioNormalizeCheck.Checked);
			configIni.Write("FadeIn", AudioFadeInTrack.Value);
			configIni.Write("FadeOut", AudioFadeOutTrack.Value);
			configIni.Write("FadeInCurve", AudioFadeInCurveCombo.SelectedIndex);
			configIni.Write("FadeOutCurve", AudioFadeOutCurveCombo.SelectedIndex);
			configIni.Write("BasePitch", AudioMainKeyCombo.SelectedItem.ToString() + AudioMainOctaveCombo.SelectedItem.ToString());
			configIni.Write("TuneMethod", AudioTuneMethodCombo.SelectedIndex);
			configIni.EndSection();
			#endregion

			#region 视频配置
			configIni.StartSection("Video");
			configIni.Write("AnimationEffect", VideoEffectCombo.SelectedIndex);
			configIni.Write("Scratch", VideoScratchCheck.Checked);
			configIni.Write("Loop", VideoLoopCheck.Checked);
			configIni.Write("FreezeFirstFrame", VideoFreezeFirstFrameCheck.Checked);
			configIni.Write("StartSize", VideoStartSizeTrack.Value);
			configIni.Write("EndSize", VideoEndSizeTrack.Value);
			configIni.Write("FadeIn", VideoFadeInTrack.Value);
			configIni.Write("FadeOut", VideoFadeOutTrack.Value);
			configIni.Write("StartSizeCurve", VideoStartSizeCurveCombo.SelectedIndex);
			configIni.Write("FadeInCurve", VideoFadeInCurveCombo.SelectedIndex);
			configIni.Write("FadeOutCurve", VideoFadeOutCurveCombo.SelectedIndex);
			configIni.EndSection();
			#endregion

			#region 素材配置
			configIni.StartSection("Source");
			configIni.Write("LastMediaSourceFrom", ChooseSourceCombo.SelectedIndex);
			if (ChooseMidiText.Text != "<未选择 MIDI 文件>") {
				configIni.Write("LastMidiFile", ChooseMidiText.Text);
				configIni.Write("LastMidiChannel", MidiChannelCombo.SelectedIndex);
			} else RemoveLastMidiConfig();
			configIni.EndSection();
			#endregion

			#region 五线谱配置
			configIni.StartSection("Staff");
			configIni.Write("Beat", MidiBeatCombo.SelectedIndex + 2);
			configIni.Write("Enable", StaffVisualizerConfigCheck.Checked);
			configIni.Write("GenerateStaff", StaffGenerateCheck.Checked);
			configIni.Write("Clef", StaffClefCombo.SelectedIndex);
			configIni.Write("Gap", StaffLineSpacingBox.Value);
			configIni.Write("Width", StaffSurfaceWidthBox.Value);
			configIni.Write("Position", StaffSurfacePositionBox.Value);
			configIni.Write("Thickness", StaffLineThicknessBox.Value);
			configIni.Write("Color", StaffLineColorBtn.Text);
			configIni.EndSection();
			#endregion

			#region 间隔选择配置
			configIni.StartSection("SelectInterval");
			configIni.Write("SelectOneEveryFew", SelectOneEveryFewBox.Value);
			configIni.Write("SelectWhichEachGroup", SelectWhichEachGroupBox.Value);
			configIni.EndSection();
			#endregion
			#endif
		}

		private void RemoveLastMidiConfig() {
			#if VEGAS_ENVIRONMENT
			configIni.DeleteKey("LastMidiFile");
			configIni.DeleteKey("LastMidiChannel");
			#endif
		}

		/// <summary>
		/// 获得本窗体的句柄。
		/// </summary>
		/// <returns>本窗体的句柄</returns>
		[DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetF();

		/// <summary>
		/// 设置此窗体为活动窗体
		/// </summary>
		/// <param name="hWnd">窗体的句柄</param>
		/// <returns>操作是否成功</returns>
		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
		private static extern bool SetF(IntPtr hWnd);

		/// <summary>
		/// 请求强制聚焦本对话框。
		/// </summary>
		public void FocusOn(object sender, EventArgs e) {
			SetF(this.Handle);
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			AcceptConfig = false;
			SaveIni();
			Close();
			// Environment.Exit(0);
			#if VEGAS_ENVIRONMENT
			parent.RemoveLastUnusedMedia();
			#else
			new ProgressForm().Show();
			#endif
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			AcceptConfig = true;
			SaveIni();
			// 特殊处理部分
			if (StaffVisualizerConfigCheck.Checked && StaffVisualizerConfigCheck.Enabled) {
				VideoEffectCombo.SelectedIndex = 0;
				VideoScratchCheck.Checked = false;
			}
			if (!StaffGenerateCheck.Enabled) StaffGenerateCheck.Checked = false;
			Close();
		}

		/// <summary>
		/// 控件的使能和失能控制。
		/// </summary>
		private void SetCheckedEnabled(object sender, EventArgs e) {
			bool isVConfigOn = VideoConfigCheck.Checked;
			SetEnabled(VideoTab, isVConfigOn, new Control[] { VideoConfigCheck, VideoScratchCheck });
			StaffVisualizerConfigCheck.Enabled = isVConfigOn;
			if (!isVConfigOn) StaffVisualizerConfigCheck.Checked = false;
			if (VideoEffect == VideoAnimFx.PINGPONG || VideoEffect == VideoAnimFx.UNLIMITED) {
				VideoScratchCheck.Checked = true;
				VideoScratchCheck.Enabled = false;
			} else {
				if (!VideoScratchCheck.Enabled) VideoScratchCheck.Checked = false;
				VideoScratchCheck.Enabled = isVConfigOn;
			}

			bool isAConfigOn = AudioConfigCheck.Checked;
			SetEnabled(AudioTab, isAConfigOn, new Control[] { AudioConfigCheck });
			AudioMainKeyCombo.Enabled = AudioMainOctaveCombo.Enabled
				= isAConfigOn && AudioTuneMethodCombo.SelectedIndex != 0;
			if (AudioTuneMethodCombo.SelectedIndex == 4) AudioScratchCheck.Checked = AudioScratchCheck.Enabled = false;

			#if VEGAS_ENVIRONMENT
			OkBtn_Enabled = (isVConfigOn || isAConfigOn) && Tabs.SelectedIndex != 4 && parent.midi != null;
			#else
			OkBtn_Enabled = (isVConfigOn || isAConfigOn) && Tabs.SelectedIndex != 4;
			#endif

			bool isSheetConfigOn = StaffVisualizerConfigCheck.Checked;
			if (isSheetConfigOn)
				VideoEffectCombo.Enabled = VideoEffectInitialValueCombo.Enabled = VideoScratchCheck.Checked = VideoScratchCheck.Enabled = false;
			SetEnabled(SheetTab, isSheetConfigOn, new Control[] { StaffVisualizerConfigCheck, SheetConfigInfoLabel });
		}

		/// <summary>
		/// 使能或失能某个控件及其所有子控件。
		/// </summary>
		/// <param name="container">容器</param>
		/// <param name="enabled">启用还是禁用</param>
		/// <param name="excepts">例外列表，位于列表内的控件不受影响</param>
		private void SetEnabled(Control container, bool enabled, Control[] excepts = null) {
			foreach (Control control in container.Controls) {
				if (excepts != null && excepts.Contains(control)) continue;
				if (!(control is FlowLayoutPanel)) control.Enabled = enabled;
				if (control.Controls.Count != 0) SetEnabled(control, enabled, excepts);
			}
		}

		public bool OkBtn_Enabled {
			get { return OkBtn.Enabled; }
			set {
				OkBtn.Enabled = value;
				//balloon.SetToolTip(OkBtn, !value ? "请确保媒体、MIDI 均已正确选择，至少选择生成音频或视频任意一个，方可开始生成。" : "");
			}
		}

		/// <summary>
		/// 将 BPM 的值保留三位小数，并转换为字符串。如果此时低位小数位为 0 则省略。
		/// </summary>
		/// <param name="bpm">BPM 速度</param>
		/// <returns>处理后的 BPM 的字符串值</returns>
		private string ProcessBpmDouble(double bpm) {
			bpm = Math.Round(bpm * 1e3) / 1e3;
			return bpm.ToString();
		}

		/// <summary>
		/// 更新 MIDI 速度的值。
		/// 同时控制自定义 BPM 速度控件的使能与失能。
		/// </summary>
		/// <param name="bpm">BPM 速度</param>
		public void UpdateMidiBpm(double bpm) {
			MidiStartSecondBox.Enabled
				= MidiEndSecondBox.Enabled
				= MidiMidiBpmCheck.Enabled
				= MidiProjectBpmCheck.Enabled
				= MidiCustomBpmCheck.Enabled
				= true;
			MidiMidiBpmCheck.Text = "MIDI 速度：" + ProcessBpmDouble(bpm);
		}

		public const string aboutHelpLink = "https://www.bilibili.com/read/cv392013";
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

		private void TrimTime_ValueChanged(object sender, EventArgs e) {
			TimecodeBox start, end;
			if (sender == SourceStartTimeText || sender == SourceEndTimeText) {
				start = SourceStartTimeText;
				end = SourceEndTimeText;
			} else if (sender == MidiStartSecondBox || sender == MidiEndSecondBox) {
				start = MidiStartSecondBox;
				end = MidiEndSecondBox;
			} else return;
			if (end.Value < start.Value) end.Value = start.Value;
		}

		public VideoAnimFx VideoEffect { get { return (VideoAnimFx)VideoEffectCombo.SelectedIndex; } }
		//public double SourceStartTimeValue = 0;
		//public double SourceEndTimeValue = 0;

		public void ChooseSourceCombo_SelectedIndexChanged(object sender, EventArgs e) {
			#if VEGAS_ENVIRONMENT
			int SourceConfigFrom = ChooseSourceCombo.SelectedIndex;
			var table = parent.audioVideoEnabledTable;
			var group = SourceConfigFrom == 0 ? table.FromSelectedMedia :
						SourceConfigFrom == 1 ? table.FromSelectedClip : table.FromBrowseFile;
			AudioConfigCheck.Enabled = AudioConfigCheck.Checked = group.AudioEnabled;
			VideoConfigCheck.Enabled = VideoConfigCheck.Checked = group.VideoEnabled;
			#endif
		}

		public void VideoEffectCombo_SelectedIndexChanged(object sender, EventArgs e) {
			#if VEGAS_ENVIRONMENT
			int VideoEffectIndex = VideoEffectCombo.SelectedIndex;
			string[] VideoEffectInitialValues = VideoAnimationEffect.InitialValues[VideoEffectIndex];
			VideoEffectInitialValueCombo.Items.Clear();
			VideoEffectInitialValueCombo.Items.AddRange(VideoEffectInitialValues);
			VideoEffectInitialValueCombo.SelectedIndex = 0;
			#endif
		}

		/// <summary>
		/// 设定下拉菜单选择的编号。避免设定的编号超出下拉菜单的项目数量
		/// </summary>
		/// <param name="combo">下拉菜单</param>
		/// <param name="index">设定选项序号</param>
		/// <param name="def">设定如果设定失败的默认值，如果为 -1 表示不设定</param>
		public void SetComboIndex(ComboBox combo, int index, int def = -1) {
			int length = combo.Items.Count;
			if (length == 0) return;
			if (index >= length || index < 0) {
				if (def >= length || def < 0) return; // 你特么故意找茬是吧？
				combo.SelectedIndex = def;
			} else combo.SelectedIndex = index;
		}

		public void SetTrackBarValue(TrackBar track, NumericUpDown numeric, int value, int def = -1) {
			int min = track.Minimum, max = track.Maximum;
			if (value < min || value > max) {
				if (def < min || def > max) return;
				numeric.Value = track.Value = def;
			} else numeric.Value = track.Value = value;
		}

		public void SetNumericValue(NumericUpDown numeric, int value, int? def = null) {
			int min = (int)numeric.Minimum, max = (int)numeric.Maximum;
			if (value < min || value > max) {
				if (def < min || def > max || def == null) return;
				numeric.Value = (decimal)def;
			} else numeric.Value = value;
		}
		public void SetNumericValue(NumericUpDown numeric, double value, double? def = null) {
			double min = (double)numeric.Minimum, max = (double)numeric.Maximum;
			if (value < min || value > max) {
				if (def < min || def > max || def == null) return;
				numeric.Value = (decimal)def;
			} else numeric.Value = (decimal)value;
		}

		public void SetBasePitchCombo(string basePitch) {
			basePitch = basePitch.Trim().ToUpper();
			MatchCollection matches = Regex.Matches(basePitch, @"[A-G]#?(?=\d)");
			string key = matches.Count != 0 ? matches[0].ToString() : "C";
			bool isSet = false;
			for (int i = 0; i < AudioMainKeyCombo.Items.Count; i++)
				if (AudioMainKeyCombo.Items[i].ToString() == key) {
					AudioMainKeyCombo.SelectedIndex = i;
					isSet = true;
					break;
				}
			if (!isSet) AudioMainKeyCombo.SelectedIndex = 0;
			matches = Regex.Matches(basePitch, @"\d+$");
			int oct = matches.Count != 0 ? int.Parse(matches[0].ToString()) : 5;
			SetComboIndex(AudioMainOctaveCombo, oct, 5);
		}

		private void SelectOneEveryFewBox_ValueChanged(object sender, EventArgs e) {
			int divisor = (int)SelectOneEveryFewBox.Value, remainder = (int)SelectWhichEachGroupBox.Value;
			if (remainder > divisor) SelectWhichEachGroupBox.Value = remainder = divisor;
			SelectWhichEachGroupBox.Maximum = new decimal(new int[] { divisor, 0, 0, 0 });
		}

		private void QuickSelectIntervalBtn_Click(object sender, EventArgs e) {
			CancelBtn_Click(sender, null);
			#if VEGAS_ENVIRONMENT
			parent.SelectInterval((int)SelectOneEveryFewBox.Value, (int)SelectWhichEachGroupBox.Value);
			#endif
		}

		internal void DisabledSelectIntervalPart() {
			SelectOneEveryFewBox.Enabled = SelectWhichEachGroupBox.Enabled = QuickSelectIntervalBtn.Enabled = false;
		}

		private void StaffLineColorBtn_Click(object sender, EventArgs e) {
			DialogResult dr = StaffLineColorDialog.ShowDialog();
			if (dr == DialogResult.OK) Update_StaffLineColorBtn_Color();
		}

		public void Update_StaffLineColorBtn_Color() {
			Button btn = StaffLineColorBtn;
			Color color = btn.BackColor = StaffLineColorDialog.Color;
			double grey = color.R * 0.3 + color.G * 0.59 + color.B * 0.11;
			btn.ForeColor = grey < 128 ? Color.White : Color.Black;
			btn.Text = '#' + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}

		private void ReadStaffLineColor(string _color) {
			MatchCollection matches = Regex.Matches(_color.ToUpper(), @"#[0-9A-F]{6}");
			string color = matches.Count != 0 ? matches[0].ToString() : "#000000";
			int r = Convert.ToInt16(color.Substring(1, 2), 16),
				g = Convert.ToInt16(color.Substring(3, 2), 16),
				b = Convert.ToInt16(color.Substring(5, 2), 16);
			Color c = Color.FromArgb(r, g, b);
			StaffLineColorDialog.Color = c;
			Update_StaffLineColorBtn_Color();
		}
	}

	/// <summary>
	/// 视频的动画效果枚举。
	/// </summary>
	public enum VideoAnimFx {
		NONE,
		H_FLIP,
		V_FLIP,
		CCW_FLIP,
		CW_FLIP,
		CCW_ROTATE,
		CW_ROTATE,
		TURNED,
		H_MIRROR,
		V_MIRROR,
		CCW_MIRROR,
		CW_MIRROR,
		NEGATIVE,
		INVERT_LUMIN,
		INVERT_HUE,
		STEP_CHANGE_HUE,
		GREY,
		PINGPONG,
		UNLIMITED // PERPETUAL_MOTION
	}
}
