/**
 * 本脚本基于原作者 Chaosinism 的开源代码二次开发，使用了 NAudio.Midi 库。
 *
 * 原作者的原参考与致谢：
 * https://github.com/evankale/VegasScripts
 * https://github.com/naudio/NAudio
 *
 * 原仓库地址：https://github.com/Chaosinism/vegas_scripts
 * 说明文档（B 站）：https://www.bilibili.com/read/cv392013
 * 说明文档（B 碗）：https://bowlroll.net/user/261124
 * 疑难解答：https://www.bilibili.com/read/cv495309
 *
 * 新版脚本由 淅琳雨 Otomad 重新编写。
 * 仓库地址：https://github.com/otomad/VegasScripts
 *
 * 修订日期：2021 年 9 月 25 日
 **/

#define VEGAS_ENVIRONMENT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using Microsoft.Win32;
using NAudio.Midi;
using ScriptPortal.Vegas;

namespace VegasScript {

	public class EntryPoint {

		// 配置参数变量
		#region 视频属性
		/* 启　　用 */ private bool VConfig { get { return configForm.VideoConfigCheck.Checked; } }
		/* 视觉效果 */ private VideoAnimFx VConfigAnim { get { return configForm.VideoEffect; } }
		/* 拉　　伸 */ private bool VConfigScratch { get { return configForm.VideoScratchCheck.Checked; } }
		/* 循　　环 */ private bool VConfigLoop { get { return configForm.VideoLoopCheck.Checked; } }
		/* 定格首帧 */ private bool VConfigFreezeFirstFrame { get { return configForm.VideoFreezeFirstFrameCheck.Checked; } }
		/* 削除空隙 */ private bool VConfigGlue { get { return configForm.VideoGlueCheck.Checked; } }
		/* 渐　　入 */ private int VConfigFadein { get { return configForm.VideoFadeInBox.Value; } }
		/* 渐　　出 */ private int VConfigFadeout { get { return configForm.VideoFadeOutBox.Value; } }
		/* 起始尺寸 */ private float VConfigStartSize { get { return configForm.VideoStartSizeBox.Value; } }
		/* 终止尺寸 */ private float VConfigEndSize { get { return configForm.VideoEndSizeBox.Value; } }
		/* 起始旋转 */ private float VConfigStartRotation { get { return (float)(configForm.VideoStartRotationBox.Value * Math.PI / 180); } }
		/* 终止旋转 */ private float VConfigEndRotation { get { return (float)(configForm.VideoEndRotationBox.Value * Math.PI / 180); } }
		/* 起始水平位移 */ private float VConfigStartHTrans { get { return configForm.VideoStartHorizontalTransBox.Value; } }
		/* 终止水平位移 */ private float VConfigEndHTrans { get { return configForm.VideoEndHorizontalTransBox.Value; } }
		/* 起始垂直位移 */ private float VConfigStartVTrans { get { return configForm.VideoStartVerticalTransBox.Value; } }
		/* 终止垂直位移 */ private float VConfigEndVTrans { get { return configForm.VideoEndVerticalTransBox.Value; } }
		/* 起始尺寸曲线 */ private VideoKeyframeType VConfigStartSizeCurve { get { return GetVideoKeyframeType(configForm.VideoStartSizeCurveCombo.SelectedIndex); } }
		/* 渐入曲线 */ private CurveType VConfigFadeinCurve { get { return GetCurveType(configForm.VideoFadeInCurveCombo.SelectedIndex); } }
		/* 渐出曲线 */ private CurveType VConfigFadeoutCurve { get { return GetCurveType(configForm.VideoFadeOutCurveCombo.SelectedIndex); } }
		/* 视觉初值 */ private int VConfigInitialValue { get { return configForm.VideoEffectInitialValueCombo.SelectedIndex; } }
		#endregion

		#region 音频属性
		/* 启　　用 */ private bool AConfig { get { return configForm.AudioConfigCheck.Checked; } }
		/* 拉　　伸 */ private bool AConfigScratch { get { return configForm.AudioScratchCheck.Checked; } }
		/* 循　　环 */ private bool AConfigLoop { get { return configForm.AudioLoopCheck.Checked; } }
		/* 标准音量 */ private bool AConfigNormalize { get { return configForm.AudioNormalizeCheck.Checked; } }
		/* 渐　　入 */ private int AConfigFadein { get { return configForm.AudioFadeInBox.Value; } }
		/* 渐　　出 */ private int AConfigFadeout { get { return configForm.AudioFadeOutBox.Value; } }
		/* 渐入曲线 */ private CurveType AConfigFadeinCurve { get { return GetCurveType(configForm.AudioFadeInCurveCombo.SelectedIndex); } }
		/* 渐出曲线 */ private CurveType AConfigFadeoutCurve { get { return GetCurveType(configForm.AudioFadeOutCurveCombo.SelectedIndex); } }
		/* 原始音高 */ private int AConfigBasePitch {
			get {
				return PitchMap(
					configForm.AudioMainKeyCombo.SelectedItem.ToString() ?? "C",
					configForm.AudioMainOctaveCombo.SelectedItem.ToString() ?? "5"
				);
			}
		}
		/* 禁　用　调　音 */ private bool AConfigNoTune { get { return configForm.AudioTuneMethodCombo.SelectedIndex == 0; } }
		/* 插　件　拉　伸 */ private bool AConfigTunePitchShiftPluginWithScratch { get { return configForm.AudioTuneMethodCombo.SelectedIndex == 2; } }
		/* 插件调音或拉伸 */ private bool AConfigTunePitchShiftPlugin { get { return configForm.AudioTuneMethodCombo.SelectedIndex == 1 || configForm.AudioTuneMethodCombo.SelectedIndex == 2; } }
		/* 加　减　拉　伸 */ private bool AConfigTunePitchWithScratch { get { return configForm.AudioTuneMethodCombo.SelectedIndex == 4; } }
		/* 加　减　调　音 */ private bool AConfigTunePlusMinusKey { get { return configForm.AudioTuneMethodCombo.SelectedIndex == 3; } }
		#endregion

		#region 迷笛属性
		/* 音　　轨 */ private int MidiConfigTrack { get { return configForm.MidiChannelCombo.SelectedIndex; } }
		/* 起始时间 */ private double MidiConfigStartTime { get { return configForm.MidiStartSecondBox.DoubleValue; } }
		/* 终止时间 */ private double MidiConfigEndTime { get { return configForm.MidiEndSecondBox.DoubleValue; } }
		/* 迷笛速度 */ private bool MidiUseMidiBpm { get { return configForm.MidiMidiBpmCheck.Checked; } }
		/* 项目速度 */ private bool MidiUseProjectBpm { get { return configForm.MidiProjectBpmCheck.Checked; } }
		/* 自拟速度 */ private bool MidiUseCustomBpm { get { return configForm.MidiCustomBpmCheck.Checked; } }
		/* 节　　拍 */ private int MidiConfigBeat { get { return configForm.MidiBeatCombo.SelectedIndex + 2; } }
		#endregion

		#region 媒体属性
		/* 起始时间 */ private double SourceConfigStartTime { get { return configForm.SourceStartTimeText.DoubleValue; } }
		/* 终止时间 */ private double SourceConfigEndTime { get { return configForm.SourceStartTimeText.DoubleValue; } }
		/* 生成位置 */ private bool GenerateAtCursor { get { return configForm.GenerateAtCursorRadio.Checked; } }
		/* 素材来源 */ private MediaSourceFrom SourceConfigFrom { get { return (MediaSourceFrom)configForm.ChooseSourceCombo.SelectedIndex; } }
		/* 上次媒体目录 */ public string lastMediaDirectory {
			get { return configIni.Read("LastMediaDirectory", "", "Source"); }
			set { configIni.Write("LastMediaDirectory", value, "Source"); }
		}
		/* 上次迷笛目录 */ public string lastMidiDirectory {
			get { return configIni.Read("LastMidiDirectory", "", "Source"); }
			set { configIni.Write("LastMidiDirectory", value, "Source"); }
		}
		#endregion
		
		#region 五线谱属性
		/* 启　　用 */ private bool SheetConfig { get { return configForm.StaffVisualizerConfigCheck.Checked; } }
		/* 生成谱面 */ private bool SheetConfigGenerateStaff { get { return configForm.StaffGenerateCheck.Checked; } }
		/* 相对位置 */ private bool SheetConfigRelative { get { return configForm.StaffRelativeValueCheck.Checked; } }
		/* 谱面宽度 */ private int SheetConfigWidth { get { return (int)configForm.StaffSurfaceWidthBox.Value; } }
		/* 谱面位置 */ private int SheetConfigPosition { get { return (int)configForm.StaffSurfacePositionBox.Value; } }
		/* 谱线间距 */ private int SheetConfigGap { get { return (int)configForm.StaffLineSpacingBox.Value; } }
		/* 谱　　号 */ private int SheetConfigCelf { get { return configForm.StaffClefCombo.SelectedIndex; } }
		/* 谱线粗细 */ private int SheetConfigThickness { get { return (int)configForm.StaffLineThicknessBox.Value; } }
		/* 谱线颜色 */ private Color SheetConfigColor { get { return configForm.StaffLineColorDialog.Color; } }
		/* 使用轨道运动方式放置音符位置 */ private bool UseLegacySheetMethod = false;
		#endregion

		// 实例对象变量
		private ConfigForm configForm;
		public Vegas vegas;
		public IniFile configIni;
		private ProgressForm progressForm;

		// 媒体 / MIDI 参数变量
		internal MIDI midi = null;
		internal Media media = null;
		private double audioLength = 0;
		private double videoLength = 0;
		private bool IsFromSelectedMedia { get { return SourceConfigFrom == MediaSourceFrom.SELECTED_MEDIA; } }
		private bool IsFromSelectedClip { get { return SourceConfigFrom == MediaSourceFrom.SELECTED_CLIP; } }
		private bool IsFromBrowseFile { get { return SourceConfigFrom == MediaSourceFrom.BROWSE_FILE; } }
		private SelectedEventSet selectedEventSet = new SelectedEventSet();
		public double ProjectBpm { get { return vegas.Project.Ruler.BeatsPerMinute; } }

		/// <summary>
		/// 根据主音高名称转换为主音高对应的值。
		/// </summary>
		/// <param name="key">音名</param>
		/// <param name="oct">八度</param>
		/// <returns>主音高结果值</returns>
		public static int PitchMap(string key, string oct) {
			List<string> keys = new List<string>("C,C#,D,D#,E,F,F#,G,G#,A,A#,B".Split(','));
			int value = keys.IndexOf(key) + int.Parse(oct) * 12;
			return value;
		}

		/// <summary>
		/// 根据音符音高获取这是五线谱上的第几根线。
		/// </summary>
		public static int[] StaffPitchMap = { 0, 0, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6 };

		/// <summary>
		/// 打开参数配置设置对话框。
		/// </summary>
		/// <returns>对话框最后选择的按钮是<c>“生成” (true)</c>还是<c>“取消” (false)</c>。</returns>
		public bool ShowConfigForm() {
			requestRestartScript = false; // 取消请求重启脚本
			statusFinish = false;
			configForm = configForm ?? new ConfigForm(this);
			progressForm = new ProgressForm();
			// try {
				configForm.ShowDialog();
			// } catch (Exception) { return false; }
			return configForm.AcceptConfig;
		}

		/// <summary>
		/// 打开选择 MIDI 文件对话框。
		/// </summary>
		/// <returns>是否选择了文件？而不是点击取消。</returns>
		internal bool SelectMidiFile() {
			#region 选择一个 MIDI 文件
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "MIDI 序列|*.mid;*.midi|所有文件|*.*";
			// openFileDialog.RestoreDirectory = true;
			openFileDialog.FilterIndex = 1;
			openFileDialog.Title = "请选择一个 MIDI 文件";
			openFileDialog.InitialDirectory = lastMidiDirectory;
			if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
			lastMidiDirectory = new Path(openFileDialog.FileName).Directory;
			return OpenMidiFile(openFileDialog.FileName, configForm);
			#endregion
		}

		internal bool OpenMidiFile(string filePath, ConfigForm configForm, bool inSilence = false) {
			if (filePath == "" || filePath == null) return false;
			MIDI _midi;
			try {
				_midi = new MIDI(filePath);
			} catch (Exception e) {
				if (!inSilence) ShowError(new Exceptions.NotAMidiFileException(), e);
				return false;
			}
			midi = _midi;

			#region 生成每个 MIDI 音轨的统计信息
			ComboBox combo = configForm.MidiChannelCombo;
			combo.Items.Clear();
			foreach (MIDI.TrackInfo info in midi.TrackInfos)
				combo.Items.Add(info.ToString());
			combo.SelectedIndex = midi.SuggestSelectedIndex ?? 0;
			configForm.ChooseMidiText.Text = filePath;
			configForm.UpdateMidiBpm(midi.Bpm);
			if (configForm.AudioConfigCheck.Checked || configForm.VideoConfigCheck.Checked) configForm.OkBtn_Enabled = true;
			#endregion
			return true;
		}

		private bool OpenMedia(Media media) {
			try {
				this.media = media;
				audioLength = videoLength = media.Length.ToMilliseconds();
			} catch (Exception e) {
				ShowError(new Exceptions.NoMediaTakeException(), e);
				this.media = null;
				return false;
			}
			return true;
		}
		private bool OpenMedia(string clipName) {
			string LastUnusedMediaBak = "";
			bool unused = !vegas.Project.MediaPool.Contains(clipName);
			if (unused) LastUnusedMediaBak = clipName;
			try {
				vegas.ImportFile(clipName, true);
			} catch (Exception e) { ShowError(new Exceptions.NoMediaTakeException(), e); return false; }
			Media media = vegas.Project.MediaPool.Find(clipName);
			RemoveLastUnusedMedia();
			if (unused) LastUnusedMedia = LastUnusedMediaBak;
			this.media = media;
			audioLength = videoLength = media.Length.ToMilliseconds();
			return true;
		}
		private string LastUnusedMedia = "";
		public void RemoveLastUnusedMedia() {
			if (LastUnusedMedia.Length != 0) {
				vegas.Project.MediaPool.Remove(LastUnusedMedia);
				LastUnusedMedia = "";
			}
		}

		/// <summary>
		/// 打开选择媒体文件对话框。
		/// </summary>
		/// <returns>是否选择了文件？而不是点击取消。</returns>
		internal bool SelectVideoClip() {
			#region 选择一个视频剪辑
			OpenFileDialog openFileDialog = new OpenFileDialog();
			// openFileDialog.InitialDirectory = @"C:\";
			openFileDialog.Filter = "支持的媒体文件|" +
				"*.veg;*.mp4;*.mpg;*.jpg;*.avi;*.aaf;*.cda;*.aa3;*.oma;*.aif;*.aiff;*.snd;*.vox;*.flac;*.gif;*.mov;*.m2t;*.ts;" +
				"*.mts;*.m2ts;*.jpe;*.jpeg;*.mkv;*.avc;*.bsf;*.264;*.mpeg;*.mmv;*.m1p;*.m1a;*.m2p;*.mp3;*.au;*.ogg;*.png;*.qt;" +
				"*.dv;*.tif;*.tiff;*.wav;*.sfa;*.dlx;*.mxf;*.pca;*.w64;*.dig;*.sd;*.tga;*.targa;*.bmp;*.dib;*.wma;*.wmv;*.asf;" +
				"*.ico;*.rle;*.psd;*.icon" +
				"|所有文件|*.*";
			// openFileDialog.RestoreDirectory = true;
			openFileDialog.FilterIndex = 1;
			openFileDialog.Title = "请选择一个视频或图片素材片段";
			openFileDialog.InitialDirectory = lastMediaDirectory;
			if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
			string clipName = openFileDialog.FileName;
			lastMediaDirectory = new Path(clipName).Directory;
			if (!OpenMedia(clipName)) {
				// audioVideoEnabledTable.FromBrowseFile.AudioEnabled = audioVideoEnabledTable.FromBrowseFile.VideoEnabled = false;
				return false;
			}
			audioVideoEnabledTable.FromBrowseFile.AudioEnabled = media.HasAudio();
			audioVideoEnabledTable.FromBrowseFile.VideoEnabled = media.HasVideo();
			configForm.ChooseSourceCombo_SelectedIndexChanged(null, null);
			ComboBox sourceCbo = configForm.ChooseSourceCombo;
			if (sourceCbo.Items.Count < 3) sourceCbo.Items.Add("");
			sourceCbo.Items[2] = clipName;
			sourceCbo.SelectedIndex = 2;
			#endregion
			return true;
		}

		/// <summary>
		/// 显示报错信息。
		/// </summary>
		public void ShowError(string str) {
			vegas.ShowError(str);
			DoingAfterShowError();
		}
		public void ShowError(Exception e) {
			vegas.ShowError(e.Message);
			DoingAfterShowError();
		}
		public void ShowError(Exception e1, Exception e2) {
			vegas.ShowError(e1.Message, e2.ToString());
			DoingAfterShowError();
		}
		public void ShowError2(string str) {
			MessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		public void ShowError2(Exception e) {
			ShowError2(e.ToString());
		}
		public void ShowError2(Exception e1, Exception e2) {
			ShowError2(e1.Message + "\n\n详细信息：\n" + e2.ToString());
		}
		private string GetExceptionInfo(Exception e) {
			return e.Message + "\n" + e.StackTrace + "\n" + e.ToString();
		}
		private void DoingAfterShowError() {
			if (configForm != null) configForm.FocusOn(null, null);
			configIni.Delete(true);
		}

		public class AudioVideoEnabledTable {
			public class AudioVideoEnabledGroup {
				public bool AudioEnabled = false;
				public bool VideoEnabled = false;
			}
			public AudioVideoEnabledGroup FromSelectedMedia = new AudioVideoEnabledGroup();
			public AudioVideoEnabledGroup FromSelectedClip = new AudioVideoEnabledGroup();
			public AudioVideoEnabledGroup FromBrowseFile = new AudioVideoEnabledGroup();
			
			public bool SelectNoMedia { get { return !FromSelectedMedia.AudioEnabled && !FromSelectedMedia.VideoEnabled; } }
			public bool SelectNoEvents { get { return !FromSelectedClip.AudioEnabled && !FromSelectedClip.VideoEnabled; } }
		}
		public MediaSourceFrom SuggestSelectedSourceFrom = MediaSourceFrom.LAST_USER_PREFERENCE;
		public AudioVideoEnabledTable audioVideoEnabledTable = new AudioVideoEnabledTable();
		public void AudioVideoEnabledTable_Init() {
			Media[] selectedMedia = vegas.Project.MediaPool.GetSelectedMedia();
			if (selectedMedia.Length != 0) {
				Media media = selectedMedia[0];
				audioVideoEnabledTable.FromSelectedMedia.AudioEnabled = media.HasAudio();
				audioVideoEnabledTable.FromSelectedMedia.VideoEnabled = media.HasVideo();
			}
			GetSelectedEventSet();
			audioVideoEnabledTable.FromSelectedClip.AudioEnabled = selectedEventSet.audioEvent != null;
			audioVideoEnabledTable.FromSelectedClip.VideoEnabled = selectedEventSet.videoEvent != null;
			int differ = ((audioVideoEnabledTable.FromSelectedClip.AudioEnabled ?1:0) + (audioVideoEnabledTable.FromSelectedClip.VideoEnabled ?1:0))
				- ((audioVideoEnabledTable.FromSelectedMedia.AudioEnabled ?1:0) + (audioVideoEnabledTable.FromSelectedMedia.VideoEnabled ?1:0));
			SuggestSelectedSourceFrom = // 下方阐述中“拥有轨道种类数目”乃拟定用词，如既拥有视频轨又拥有音轨则为 2，仅拥有视频轨或音轨之一则为 1，如果两个轨道均没有则为 0．
				differ > 0 ? MediaSourceFrom.SELECTED_CLIP : // 如果选定轨道素材中拥有轨道种类数目大于选定项目媒体素材中拥有轨道种类数目，则在素材来源设置中默认选中使用“选中的轨道素材”。
				differ < 0 ? MediaSourceFrom.SELECTED_MEDIA : // 如果选定轨道素材中拥有轨道种类数目小于选定项目媒体素材中拥有轨道种类数目，则在素材来源设置中默认选中使用“选中的媒体文件”。
				MediaSourceFrom.LAST_USER_PREFERENCE; // 如果选定轨道素材中拥有轨道种类数目等于选定项目媒体素材中拥有轨道种类数目，则由用户上一次选择的选项决定。
			if (!audioVideoEnabledTable.FromSelectedClip.AudioEnabled && !audioVideoEnabledTable.FromSelectedClip.VideoEnabled &&
				!audioVideoEnabledTable.FromSelectedMedia.AudioEnabled && !audioVideoEnabledTable.FromSelectedMedia.VideoEnabled)
				SuggestSelectedSourceFrom = MediaSourceFrom.NOTHING_SELECTED; // 如果两者都没有选择任何素材。
		}

		public static CurveType GetCurveType(int index) {
			CurveType[] curves = { CurveType.Linear, CurveType.Fast, CurveType.Slow, CurveType.Smooth, CurveType.Sharp, CurveType.None };
			return curves[index];
		}
		public static VideoKeyframeType GetVideoKeyframeType(int index) {
			VideoKeyframeType[] curves = { VideoKeyframeType.Linear, VideoKeyframeType.Fast, VideoKeyframeType.Slow, VideoKeyframeType.Smooth, VideoKeyframeType.Sharp, VideoKeyframeType.Hold };
			return curves[index];
		}

		#region 以下方法仅供测试使用
		public static object s { set { MessageBox.Show(value.ToString()); } }
		public static void test() { s = "Super Idol 的笑容都没你的甜！"; }
		#endregion

		private class SelectedEventSet {
			public AudioEvent audioEvent = null;
			public VideoEvent videoEvent = null;
			public double audioLength = 0;
			public double videoLength = 0;
		}

		public TrackEvent[] GetSelectedEvents() {
			List<TrackEvent> selectedList = new List<TrackEvent>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected)
						selectedList.Add(trackEvent);
			return selectedList.ToArray();
		}

		private void GetSelectedEventSet() {
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected) {
						if (trackEvent.IsAudio() && selectedEventSet.audioEvent == null) {
							selectedEventSet.audioEvent = (AudioEvent)trackEvent;
							selectedEventSet.audioLength = trackEvent.Length.ToMilliseconds();
						}
						if (trackEvent.IsVideo() && selectedEventSet.videoEvent == null) {
							selectedEventSet.videoEvent = (VideoEvent)trackEvent;
							selectedEventSet.videoLength = trackEvent.Length.ToMilliseconds();
						}
					}
			if (selectedEventSet.audioEvent != null && selectedEventSet.videoEvent == null && selectedEventSet.audioEvent.Group != null) {
				foreach (TrackEvent trackEvent in selectedEventSet.audioEvent.Group)
					if (trackEvent.IsVideo()) {
						selectedEventSet.videoEvent = (VideoEvent)trackEvent;
						selectedEventSet.videoLength = trackEvent.Length.ToMilliseconds();
						break;
					}
			} else if (selectedEventSet.audioEvent == null && selectedEventSet.videoEvent != null && selectedEventSet.videoEvent.Group != null) {
				foreach (TrackEvent trackEvent in selectedEventSet.videoEvent.Group) {
					if (trackEvent.IsAudio()) {
						selectedEventSet.audioEvent = (AudioEvent)trackEvent;
						selectedEventSet.audioLength = trackEvent.Length.ToMilliseconds();
						break;
					}
				}
			}
		}

		private bool GetSelectedSource() {
			if (IsFromSelectedMedia) {
				Media[] selections = vegas.Project.MediaPool.GetSelectedMedia();
				if (selections.Length == 0) {
					ShowError(new Exceptions.NoSelectedMediaException());
					media = null;
					return false;
				}
				OpenMedia(selections[0]);
			}
			if (IsFromSelectedClip) {
				audioLength = selectedEventSet.audioLength;
				videoLength = selectedEventSet.videoLength;
				if (selectedEventSet.audioEvent == null && selectedEventSet.videoEvent == null) {
					ShowError(new Exceptions.NoSelectedClipException());
					media = null;
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 每几个选第几个。
		/// </summary>
		/// <param name="everyFew">每几个</param>
		/// <param name="whichOne">第几个</param>
		public void SelectInterval(int everyFew, int whichOne) {
			if (everyFew < 2 || whichOne < 0 || whichOne > everyFew) return;
			foreach (Track track in vegas.Project.Tracks) {
				for (int i = 0, j = 0; i < track.Events.Count; i++, j = i % everyFew) {
					TrackEvent trackEvent = track.Events[i];
					trackEvent.Selected = trackEvent.Selected && j == whichOne - 1;
				}
			}
		}

		/// <summary>
		/// 倒放视频事件。
		/// 注意是使用速度包络控制，而不是创建一个反转的子剪辑。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <returns>倒放后的视频事件（实际上也就是原视频事件）</returns>
		public VideoEvent ReverseVideo(VideoEvent videoEvent) {
			Timecode start = videoEvent.Start, length = videoEvent.Length;
			videoEvent.AdjustStartLength(start + length, length, true);
			videoEvent.Start = start;
			Envelope velocity = videoEvent.Envelopes.FindByType(EnvelopeType.Velocity) ?? new Envelope(EnvelopeType.Velocity);
			if (!videoEvent.Envelopes.HasEnvelope(EnvelopeType.Velocity)) videoEvent.Envelopes.Add(velocity);
			velocity.Points.Clear();
			velocity.Points[0].Y = -1;
			velocity.Points[0].Curve = CurveType.None;
			return videoEvent;
		}

		/// <summary>
		/// 定格视频事件初帧，即静止画。
		/// 如果要和倒放视频事件函数一起使用，应该放在其之后。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <returns>原视频事件</returns>
		public VideoEvent FreezeFirstFrame(VideoEvent videoEvent) {
			Envelope velocity = videoEvent.Envelopes.FindByType(EnvelopeType.Velocity) ?? new Envelope(EnvelopeType.Velocity);
			if (!videoEvent.Envelopes.HasEnvelope(EnvelopeType.Velocity)) videoEvent.Envelopes.Add(velocity);
			velocity.Points[0].Y = 0;
			velocity.Points[0].Curve = CurveType.None;
			return videoEvent;
		}

		public void AdjustDeviation(TrackEvent trackEvent, double startTime, double endTime) {
			Timecode start = trackEvent.Start, length = trackEvent.Length;
			trackEvent.AdjustStartLength(start + Timecode.FromMilliseconds(startTime), Timecode.FromMilliseconds(endTime - startTime), true);
			trackEvent.Start = start;
			trackEvent.Length = length;
		}

		/// <summary>
		/// 生成音 MAD。
		/// </summary>
		private void GenerateOtomad() {
			#region 验证数据合法
			if (!IsFromBrowseFile) { bool ok = GetSelectedSource(); if (!ok) return; }
			if (midi == null) { ShowError(new Exceptions.NoMidiException()); return; }
			if (media == null && !IsFromSelectedClip) { ShowError(new Exceptions.NoMediaException()); return; }
			if (midi.TrackInfos == null) { ShowError(new Exceptions.NoTrackInfoException()); return; }
			if (!MidiUseMidiBpm) {
				if (MidiUseCustomBpm) midi.Bpm = (double)configForm.MidiCustomBpmBox.Value;
				else midi.Bpm = ProjectBpm;
			}
			Plugin.Init(vegas);
			progressForm.Show();
			if (AConfigTunePitchShiftPlugin) if (!ExaminePitchShiftPresetExist()) return;
			progressForm.Info = "";
			#endregion

			#region 开始处理 MIDI
			string name = midi.TrackInfos[MidiConfigTrack].Name; // 所选 MIDI 轨道名称。如果没有则为空串。
			
			#region 视频操作
			const int MAX_VIDEO_TRACK_SIZE = 100;
			int tTrackCount = 0; // 总轨道计数，用于新建音频轨道。
			VideoTrack vTrack = null; // 如果不启用五线谱时用的视频轨道。
			VideoTrack[] vTracks = null; // 如果启用五线谱时用的视频轨道列表。
			bool needTwoKey = VConfigStartSize != VConfigEndSize || // 如果为起始尺寸与终止尺寸大小相等，则没有必要打两个关键帧了。
				VConfigStartRotation != VConfigEndRotation ||
				VConfigStartHTrans != VConfigEndHTrans ||
				VConfigStartVTrans != VConfigEndVTrans;
			VideoAnimationEffect anim = new VideoAnimationEffect(VConfigAnim, VConfigInitialValue);
			double lastStartTime = -1;
			if (VConfig && !SheetConfig) vegas.Project.Tracks.Add(vTrack = new VideoTrack(vegas.Project, tTrackCount++, name));
			else if (SheetConfig) vTracks = new VideoTrack[MAX_VIDEO_TRACK_SIZE];
			#endregion

			#region 音频操作
			const int MAX_AUDIO_TRACK_SIZE = 20;
			AudioTrack[] aTracks = null; // 音频轨道列表
			double[] aTrackPositions = null; // 音轨轨道长度计数列表
			int aTrackCount = 1; // 音频轨道计数，用于新建音频轨道。由于如果不生成音频也不会使用这个变量，因此初始化为 1 没有问题。
			// double? normalizeGain = null;
			if (AConfig) {
				aTracks = new AudioTrack[MAX_AUDIO_TRACK_SIZE];
				aTrackPositions = new double[MAX_AUDIO_TRACK_SIZE];
				vegas.Project.Tracks.Add(aTracks[0] = new AudioTrack(vegas.Project, tTrackCount++, name));
				aTrackPositions[0] = 0;
			}
			#endregion

			#region 如果修改了素材的入点和出点的时间
			double sourceStartTime = configForm.SourceStartTimeText.Value, sourceEndTime = configForm.SourceEndTimeText.Value;
			bool adjustTime = sourceStartTime != 0 || sourceEndTime != 0;
			if (adjustTime) {
				while (sourceEndTime <= sourceStartTime) sourceEndTime += Math.Max(audioLength, videoLength);
				audioLength = videoLength = sourceEndTime - sourceStartTime;
			}
			double generateBeginTime = !GenerateAtCursor ? 0 : vegas.Transport.CursorPosition.ToMilliseconds();
			double songLength = 0; // 指定乐曲总长
			#endregion
			
			#region 五线谱操作
			int vTrackCount = -1; // 视频轨道计数，用于新建视频轨道。仅在启用五线谱效果时使用。
			int trackPointer = 0; // 视频轨道指针
			double barStartTime = 0;
			double barLength = midi.MsPerQuarter * MidiConfigBeat;
			bool sliceComposition = MidiConfigStartTime < MidiConfigEndTime;
			int projWidth = SheetConfigRelative ? 1920 : vegas.Project.Video.Width;
			int projHeight = SheetConfigRelative ? 1080 : vegas.Project.Video.Height;
			#endregion
			
			#region 规范化音频
			AudioEvent audioEventSample = null;
			if (AConfig) {
				if (!IsFromSelectedClip) {
					audioEventSample = aTracks[0].AddAudioEvent(
						Timecode.FromMilliseconds(0),
						Timecode.FromMilliseconds(audioLength)
					);
					try {
						audioEventSample.AddTake(media.GetAudioStreamByIndex(0));
					} catch (Exception e) { ShowError(new Exceptions.NoAudioTakeException(), e); return; }
				} else {
					if (selectedEventSet.audioEvent == null) { ShowError(new Exceptions.NoAudioTakeException()); return; }
					audioEventSample = (AudioEvent)selectedEventSet.audioEvent.Copy(aTracks[0], Timecode.FromMilliseconds(0));
				}
				if (AConfigNormalize) // 将添加音频单独提取到循环之外有助于提高规范化音频的速度
					audioEventSample.Normalize = true;
			}
			#endregion

			for (int i = 0; i < midi.Events[MidiConfigTrack].Count; i++) {
				MidiEvent midiEvent = midi.Events[MidiConfigTrack][i];
				int statusProgress = (int)Math.Round(100.0 * i / midi.Events[MidiConfigTrack].Count);
				if (AConfig && AConfigTunePitchShiftPlugin) // 如果不是使用“移调”效果插件，就不要刷进度条，否则还会额外拖延时间。
					progressForm.ReportProgress(statusProgress); // 说明：只有在使用“移调”效果插件时才会生成得很慢，其它情况下都是非常快的。
				if (progressForm.RequestAbort) break;
				if (!(midiEvent is NoteOnEvent)) continue;
				NoteEvent noteEvent = (NoteEvent)midiEvent;
				NoteOnEvent noteOnEvent = (NoteOnEvent)midiEvent;
				double startTime = midiEvent.AbsoluteTime * midi.MsPerQuarter / midi.TicksPerQuarter;
				double duration = noteOnEvent.NoteLength * midi.MsPerQuarter / midi.TicksPerQuarter;
				int pitch = noteEvent.NoteNumber;
				int trackIndex = 0;

				if (startTime < MidiConfigStartTime) continue;
				if (startTime > MidiConfigEndTime && sliceComposition) break;
				songLength = startTime + duration;
				#region 下一页
				while (startTime >= barStartTime + barLength) {
					barStartTime = barStartTime + barLength;
					trackPointer = 0;
				}
				#endregion

				#region 生成音频事件
				if (AConfig) {
					while (startTime < aTrackPositions[trackIndex]) // 如果音频是多轨则放到新建的轨道，虽然有时候判断不准确，但问题不大
						if (++trackIndex == aTrackCount) {
							aTrackCount++;
							vegas.Project.Tracks.Add(aTracks[trackIndex] = new AudioTrack(vegas.Project, tTrackCount++, name));
						}
					AudioEvent audioEvent = (AudioEvent)audioEventSample.Copy(aTracks[trackIndex], Timecode.FromMilliseconds(generateBeginTime + startTime));
					audioEvent.Length = Timecode.FromMilliseconds(duration);
					aTrackPositions[trackIndex] = startTime + duration;
					try {
						audioEvent.Method = TimeStretchPitchShift.Elastique; // 这个操作没有在 Vegas 文档中写到。
						audioEvent.PitchLock = false;
					} catch (Exception e) {
						if (!AConfigNoTune && !AConfigTunePitchShiftPlugin) {
							ShowError(new Exceptions.NoTimeStretchPitchShiftException(), e);
							return;
						}
					}
					if (adjustTime) AdjustDeviation(audioEvent, sourceStartTime, sourceEndTime);
					if (AConfigScratch) audioEvent.AdjustPlaybackRate(audioLength / duration, true);
					audioEvent.Loop = AConfigLoop;

					audioEvent.FadeIn.Length = Timecode.FromMilliseconds(duration * AConfigFadein / 100);
					audioEvent.FadeOut.Length = Timecode.FromMilliseconds(duration * AConfigFadeout / 100);
					audioEvent.FadeIn.Curve = AConfigFadeinCurve;
					audioEvent.FadeOut.Curve = AConfigFadeoutCurve;

					#region 应用变调
					int pitchDelta = pitch - AConfigBasePitch;
					if (AConfigTunePitchShiftPlugin) {
						if (Plugin.pitchShift == null) { ShowError(new Exceptions.NoPluginPitchShiftException()); return; }
						int pitchDeltaTimes = pitchDelta > 0 ? 12 : -12;
						while (pitchDeltaTimes * pitchDelta > 0) { // pitchDeltaTimes > 0 ? pitchDelta > 0 : pitchDelta < 0
							Effect effect = new Effect(Plugin.pitchShift);
							audioEvent.Effects.Add(effect);
							try {
								effect.Preset = (Math.Abs(pitchDelta) <= 12 ? pitchDelta : pitchDeltaTimes).ToString()
									+ (AConfigTunePitchShiftPluginWithScratch ? "~" : "");
							} catch (Exception e) { ShowError(new Exceptions.NoPluginPresetException(), e); return; }
							pitchDelta -= pitchDeltaTimes;
						}
					} else if (AConfigTunePlusMinusKey) audioEvent.PitchSemis = (double)pitchDelta;
					else if (AConfigTunePitchWithScratch) {
						audioEvent.PitchLock = true;
						audioEvent.AdjustPlaybackRate(Math.Pow(2, (double)pitchDelta / 12.0), true);
					}
					#endregion
				}
				#endregion

				#region 生成视频事件
				if (VConfig) {
					if (!SheetConfig && lastStartTime == startTime) continue; // 避免视频重叠
					if (SheetConfig && trackPointer > vTrackCount) {
						tTrackCount++;
						vegas.Project.Tracks.Add(vTracks[++vTrackCount] = new VideoTrack(vegas.Project, 0, name));
					}
					VideoTrack _vTrack = !SheetConfig ? vTrack : vTracks[trackPointer];
					VideoEvent videoEvent = null;
					if (!IsFromSelectedClip) {
						videoEvent = _vTrack.AddVideoEvent(
							Timecode.FromMilliseconds(generateBeginTime + startTime),
							Timecode.FromMilliseconds(duration)
						);
						try {
							videoEvent.AddTake(media.GetVideoStreamByIndex(0));
						} catch (Exception) { ShowError(new Exceptions.NoVideoTakeException()); return; }
					} else {
						if (selectedEventSet.videoEvent == null) { ShowError(new Exceptions.NoVideoTakeException()); return; }
						videoEvent = (VideoEvent)selectedEventSet.videoEvent.Copy(_vTrack, Timecode.FromMilliseconds(generateBeginTime + startTime));
						videoEvent.Length = Timecode.FromMilliseconds(duration);
					}
					if (adjustTime) AdjustDeviation(videoEvent, sourceStartTime, sourceEndTime);
					if (SheetConfig) videoEvent.Length = Timecode.FromMilliseconds(barStartTime + barLength - startTime);
					if (VConfigScratch) videoEvent.AdjustPlaybackRate(videoLength / duration, true);
					if (anim.IsReverse) ReverseVideo(videoEvent); // 结论：先拉伸后反转
					if (VConfigFreezeFirstFrame) FreezeFirstFrame(videoEvent);
					videoEvent.Loop = VConfigLoop;

					videoEvent.FadeIn.Length = Timecode.FromMilliseconds(duration * VConfigFadein / 100);
					videoEvent.FadeOut.Length = Timecode.FromMilliseconds(duration * VConfigFadeout / 100);
					videoEvent.FadeIn.Curve = VConfigFadeinCurve;
					videoEvent.FadeOut.Curve = VConfigFadeoutCurve;

					VideoMotionKeyframe key0 = videoEvent.VideoMotion.Keyframes[0];
					VideoMotionKeyframe key1 = new VideoMotionKeyframe(Timecode.FromMilliseconds(duration));
					if (needTwoKey) videoEvent.VideoMotion.Keyframes.Add(key1);
					float width = key0.BottomRight.X;
					float height = key0.BottomRight.Y;
					float startRatio = VConfigStartSize / 100;
					key0.ScaleBy(new VideoMotionVertex(startRatio * anim.HorizontalFlip, startRatio * anim.VerticalFlip));
					key0.MoveBy(new VideoMotionVertex((Math.Abs(1 - startRatio) * width) / 2 * VConfigStartHTrans / 100, (Math.Abs(1 - startRatio) * height) / 2 * VConfigStartVTrans / 100));
					key0.Rotation = VConfigStartRotation + anim.RotationDeg;
					key0.Type = VConfigStartSizeCurve;
					if (needTwoKey) {
						float endRatio = VConfigEndSize / 100;
						key1.ScaleBy(new VideoMotionVertex(endRatio * anim.HorizontalFlip, endRatio * anim.VerticalFlip));
						key1.MoveBy(new VideoMotionVertex((Math.Abs(1 - endRatio) * width) / 2 * VConfigEndHTrans / 100, (Math.Abs(1 - endRatio) * height) / 2 * VConfigEndVTrans / 100));
						key1.Rotation = VConfigEndRotation + anim.RotationDeg;
					}
					// 动画效果生成
					if (anim.IsNegative) if (Plugin.invert != null) Plugin.Negative(videoEvent); else { ShowError(new Exceptions.NoPluginInvertException()); return; }
					if (anim.IsGrey) if (Plugin.blackAndWhite != null) Plugin.Grey(videoEvent); else { ShowError(new Exceptions.NoPluginNameException("黑白")); return; }
					if (anim.IsInvertLumin) if (Plugin.labAdjust != null) Plugin.InvertLumin(videoEvent); else { ShowError(new Exceptions.NoPluginNameException("LAB 调整")); return; }
					if (anim.Hue != 0) if (Plugin.hslAdjust != null) Plugin.ChangeHue(videoEvent, anim.Hue); else { ShowError(new Exceptions.NoPluginNameException("HSL 调整")); return; }
					if (anim.HorizontalMirrored != 0 || anim.VerticalMirrored != 0) if (Plugin.mirror != null) Plugin.Mirror(videoEvent, anim.HorizontalMirrored, anim.VerticalMirrored);
						else { ShowError(new Exceptions.NoPluginNameException("镜像")); return; }
					if (SheetConfig) { // 五线谱效果生成
						if (UseLegacySheetMethod) {
							TrackMotionKeyframe keyFrame = vTracks[trackPointer].TrackMotion.InsertMotionKeyframe(Timecode.FromMilliseconds(startTime));
							keyFrame.Type = VideoKeyframeType.Hold;
							keyFrame.Width = SheetConfigGap * 2 * projWidth / projHeight;
							keyFrame.Height = SheetConfigGap * 2;
							keyFrame.PositionX = -SheetConfigWidth / 2 + SheetConfigWidth / barLength * (startTime - barStartTime);
							int octave = pitch / 12;
							int line = StaffPitchMap[pitch % 12];
							keyFrame.PositionY = SheetConfigPosition - SheetConfigGap * 3 + (octave - 5) * SheetConfigGap * 3.5 + line * SheetConfigGap * 0.5 + SheetConfigCelf * 12;
						} else {
							if (Plugin.picInPic == null) { ShowError(new Exceptions.NoPluginNameException("画中画")); return; }
							Effect picInPic = new Effect(Plugin.picInPic);
							videoEvent.Effects.Add(picInPic);
							OFXDoubleParameter scale = (OFXDoubleParameter)picInPic.OFXEffect.FindParameterByName("Scale");
							scale.Value = SheetConfigGap * 2.0 / projHeight;
							OFXDoubleParameter scaleY = (OFXDoubleParameter)picInPic.OFXEffect.FindParameterByName("DistortionScaleY");
							scaleY.Value = scale.Value;
							OFXDouble2DParameter location = (OFXDouble2DParameter)picInPic.OFXEffect.FindParameterByName("Location");
							double positionX = -SheetConfigWidth / 2 + SheetConfigWidth / barLength * (startTime - barStartTime);
							int octave = pitch / 12;
							int line = StaffPitchMap[pitch % 12];
							double positionY = SheetConfigPosition - SheetConfigGap * 3 + (octave - 5) * SheetConfigGap * 3.5 + line * SheetConfigGap * 0.5 + SheetConfigCelf * 12;
							location.Value = NewDouble2D(positionX / projWidth + 0.5, positionY / projHeight + 0.5);
						}
						trackPointer++;
					}

					lastStartTime = startTime;
					anim.Next();
				}
				#endregion
			}
			#endregion
			progressForm.ReportProgress(100);
			
			#region 生成五线谱
			if (SheetConfigGenerateStaff) {
				VideoTrack sheetTrack;
				vegas.Project.Tracks.Add(sheetTrack = new VideoTrack(tTrackCount++));
				double start = generateBeginTime + MidiConfigStartTime,
					length = songLength;
				VideoEvent videoEvent = sheetTrack.AddVideoEvent(Timecode.FromMilliseconds(start), Timecode.FromMilliseconds(length));
				Media solidColor = new Media(Plugin.solidColor);
				videoEvent.AddTake(solidColor.GetVideoStreamByIndex(0));
				Color color = configForm.StaffLineColorDialog.Color;
				((OFXRGBAParameter)solidColor.Generator.OFXEffect.FindParameterByName("Color")).Value = new OFXColor(color.R / 255.0, color.G / 255.0, color.B / 255.0);
				// 此处用一段非常笨拙的方式生成五线谱。
				Effect effect1 = new Effect(Plugin.crop);
				videoEvent.Effects.Add(effect1);
				((OFXDoubleParameter)effect1.OFXEffect.FindParameterByName("YScale")).Value = SheetConfigThickness / 1000.0;
				Effect effect2 = new Effect(Plugin.mirror);
				videoEvent.Effects.Add(effect2);
				((OFXDouble2DParameter)effect2.OFXEffect.FindParameterByName("Center")).Value = NewDouble2D(0.5, 0.4);
				Effect effect3 = new Effect(Plugin.mirror);
				videoEvent.Effects.Add(effect3);
				((OFXDouble2DParameter)effect3.OFXEffect.FindParameterByName("Center")).Value = NewDouble2D(0.5, 0.3);
				Effect effect4 = new Effect(Plugin.mirror);
				videoEvent.Effects.Add(effect4);
				((OFXDoubleParameter)effect4.OFXEffect.FindParameterByName("Angle")).Value = 180;
				// 此处本来想用“画中画”效果实现拉伸的，结果里面有一个 OFXChoice 类打死也不给构造新对象。
				TrackMotionKeyframe trackKeyFrame = sheetTrack.TrackMotion.MotionKeyframes[0];
				trackKeyFrame.Type = VideoKeyframeType.Hold;
				trackKeyFrame.Height *= 1.25 / projHeight * SheetConfigGap * 4;
				trackKeyFrame.PositionY = (double)SheetConfigPosition / projHeight * vegas.Project.Video.Height;
				// 本来还想来自动生成谱号的，奈何脚本不支持蒙版锚点的编辑。
			}
			#endregion
			
			#region 消除间隙
			// ⸘视频被削除了‽
			if (VConfigGlue && !SheetConfig) {
				TrackEvents events = vTrack.Events;
				for (int i = 0; i < events.Count - 1; i++)
					events[i].End = events[i + 1].Start;
			}
			#endregion
			
			if (audioEventSample != null) audioEventSample.Track.Events.Remove(audioEventSample);
			foreach (Track[] tracks in new Track[][] { new Track[] { vTrack }, vTracks, aTracks }) {
				if (tracks == null) continue;
				foreach (Track track in tracks) {
					if (track == null) continue;
					foreach (TrackEvent trackEvent in track.Events)
						trackEvent.Selected = true;
				}
			}
		}
		
		/// <summary>
		/// 构造一个 OFXDouble2D 对象，因为 OFXDouble2D 类居然没有带参数的构造函数。
		/// </summary>
		/// <param name="x">X 坐标值</param>
		/// <param name="y">Y 坐标值</param>
		/// <returns>新的 OFXDouble2D 对象</returns>
		private OFXDouble2D NewDouble2D(double x, double y) {
			OFXDouble2D location = new OFXDouble2D();
			location.X = x;
			location.Y = y;
			return location;
		}

		private bool requestRestartScript = false;
		public int statusProgress = 0;
		public bool statusFinish = false;

		/// <summary>
		/// 检查移调插件的预设是否存在。
		/// </summary>
		/// <returns>处理是否成功。如果预设不存在并且脚本自己也无法自动生成预设，才会返回 false</returns>
		private bool ExaminePitchShiftPresetExist() {
			progressForm.Info = "正在检查移调插件的预设是否可用……";
			if (Plugin.pitchShift == null) {
				ShowError(new Exceptions.NoPluginPitchShiftException());
				return false;
			}
			var checkPresets = new Func<bool>(() => {
				EffectPresets presets = Plugin.pitchShift.Presets;
				List<string> presetNames = new List<string>();
				foreach (EffectPreset preset in presets) presetNames.Add(preset.Name);
				bool presetOk = true;
				for (int i = -12; i <= 12; i++)
					if (!presetNames.Contains(i.ToString()) || !presetNames.Contains(i.ToString() + "~")) {
						presetOk = false;
						break;
					}
				return presetOk;
			});
			if (checkPresets()) return true;
			DialogResult result = MessageBox.Show("由于您试图使用“移调”效果插件调音，但是系统发现您并没有完全配置好所需的所有音效预设。是否由脚本为您自动添加预设？\n\n" +
				"选择“是”，则由脚本尝试为您添加预设，可能会添加失败。如果失败，则请按照使用教程的说明来手动操作。\n\n" +
				"选择“否”，将会返回到参数设置界面。", "未找到所有的移调音效预设", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (result == DialogResult.No) {
				requestRestartScript = true;
				return false;
			}
			#region 开始添加预设
			var S = new Func<string, byte[], Tuple<string, byte[]>>((name, binary) => new Tuple<string, byte[]>(name, binary));
			var GetBytes = new Func<string, byte[]>(hexString => {
				hexString = hexString.Replace(" ", "");
				if (hexString.Length % 2 == 1) return null;
				if (Regex.Matches(hexString, @"[^0-9A-Fa-f]").Count != 0) return null;
				byte[] bytes = new byte[hexString.Length / 2];
				for (int i = 0; i < bytes.Length; i++)
					bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
				return bytes;
			});
			var T = new Func<string, string, Tuple<string, byte[]>>((name, binary) => S(name, GetBytes(binary)));
			RegistryKey reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\DirectShow\Presets\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}");
			Tuple<string, byte[]>[] tuples = {
				T("-12", "4400000003411bedbe93d011aebc00a0c9053912f4ffffff0000000002000000010000000800000000000000000000000000e03f000000000000000000000000f82f4441"),
				T("-11", "4400000003411bedbe93d011aebc00a0c9053912f5ffffff00000000020000000100000008000000000000006379d9928ff3e03f000000000000000000000000f82f4441"),
				T("-10", "4400000003411bedbe93d011aebc00a0c9053912f6ffffff0000000002000000010000000800000000000000c0d6c7c39af5e13f000000000000000000000000f82f4441"),
				T("-9", "4400000003411bedbe93d011aebc00a0c9053912f7ffffff000000000200000001000000080000000000000015b7310afe06e33f000000000000000000000000f82f4441"),
				T("-8", "4400000003411bedbe93d011aebc00a0c9053912f8ffffff00000000020000000100000008000000000000008b728df9a228e43f000000000000000000000000f82f4441"),
				T("-7", "4400000003411bedbe93d011aebc00a0c9053912f9ffffff00000000020000000100000008000000000000005eecf008815be53f000000000000000000000000f82f4441"),
				T("-6", "4400000003411bedbe93d011aebc00a0c9053912faffffff0000000002000000010000000800000000000000cd3b7f669ea0e63f000000000000000000000000f82f4441"),
				T("-5", "4400000003411bedbe93d011aebc00a0c9053912fbffffff0000000002000000010000000800000000000000b0cf68d710f9e73f000000000000000000000000f82f4441"),
				T("-4", "4400000003411bedbe93d011aebc00a0c9053912fcffffff00000000020000000100000008000000000000003d6e3da5fe65e93f000000000000000000000000f82f4441"),
				T("-3", "4400000003411bedbe93d011aebc00a0c9053912fdffffff0000000002000000010000000800000000000000add35a999fe8ea3f000000000000000000000000f82f4441"),
				T("-2", "4400000003411bedbe93d011aebc00a0c9053912feffffff000000000200000001000000080000000000000029c14e073e82ec3f000000000000000000000000f82f4441"),
				T("-1", "4400000003411bedbe93d011aebc00a0c9053912ffffffff0000000002000000010000000800000000000000441310e73734ee3f000000000000000000000000f82f4441"),
				T("0", "4400000003411bedbe93d011aebc00a0c9053912000000000000000002000000010000000800000000000000000000000000f03f000000000000000000000000f82f4441"),
				T("1", "4400000003411bedbe93d011aebc00a0c90539120100000000000000020000000100000008000000000000006379d9928ff3f03f000000000000000000000000f82f4441"),
				T("2", "4400000003411bedbe93d011aebc00a0c9053912020000000000000002000000010000000800000000000000c0d6c7c39af5f13f000000000000000000000000f82f4441"),
				T("3", "4400000003411bedbe93d011aebc00a0c905391203000000000000000200000001000000080000000000000015b7310afe06f33f000000000000000000000000f82f4441"),
				T("4", "4400000003411bedbe93d011aebc00a0c90539120400000000000000020000000100000008000000000000008b728df9a228f43f000000000000000000000000f82f4441"),
				T("5", "4400000003411bedbe93d011aebc00a0c90539120500000000000000020000000100000008000000000000005eecf008815bf53f000000000000000000000000f82f4441"),
				T("6", "4400000003411bedbe93d011aebc00a0c9053912060000000000000002000000010000000800000000000000cd3b7f669ea0f63f000000000000000000000000f82f4441"),
				T("7", "4400000003411bedbe93d011aebc00a0c9053912070000000000000002000000010000000800000000000000b0cf68d710f9f73f000000000000000000000000f82f4441"),
				T("8", "4400000003411bedbe93d011aebc00a0c90539120800000000000000020000000100000008000000000000003c6e3da5fe65f93f000000000000000000000000f82f4441"),
				T("9", "4400000003411bedbe93d011aebc00a0c9053912090000000000000002000000010000000800000000000000add35a999fe8fa3f000000000000000000000000f82f4441"),
				T("10", "4400000003411bedbe93d011aebc00a0c90539120a000000000000000200000001000000080000000000000029c14e073e82fc3f000000000000000000000000f82f4441"),
				T("11", "4400000003411bedbe93d011aebc00a0c90539120b0000000000000002000000010000000800000000000000431310e73734fe3f000000000000000000000000f82f4441"),
				T("12", "4400000003411bedbe93d011aebc00a0c90539120c00000000000000020000000100000008000000000000000000000000000040000000000000000000000000f82f4441"),
				T("-12~", "4400000003411bedbe93d011aebc00a0c9053912f4ffffff0000000002000000000000000800000000000000000000000000e03f000000000000000000000000f82f4441"),
				T("-11~", "4400000003411bedbe93d011aebc00a0c9053912f5ffffff00000000020000000000000008000000000000006379d9928ff3e03f000000000000000000000000f82f4441"),
				T("-10~", "4400000003411bedbe93d011aebc00a0c9053912f6ffffff0000000002000000000000000800000000000000c0d6c7c39af5e13f000000000000000000000000f82f4441"),
				T("-9~", "4400000003411bedbe93d011aebc00a0c9053912f7ffffff000000000200000000000000080000000000000015b7310afe06e33f000000000000000000000000f82f4441"),
				T("-8~", "4400000003411bedbe93d011aebc00a0c9053912f8ffffff00000000020000000000000008000000000000008b728df9a228e43f000000000000000000000000f82f4441"),
				T("-7~", "4400000003411bedbe93d011aebc00a0c9053912f9ffffff00000000020000000000000008000000000000005eecf008815be53f000000000000000000000000f82f4441"),
				T("-6~", "4400000003411bedbe93d011aebc00a0c9053912faffffff0000000002000000000000000800000000000000cd3b7f669ea0e63f000000000000000000000000f82f4441"),
				T("-5~", "4400000003411bedbe93d011aebc00a0c9053912fbffffff0000000002000000000000000800000000000000b0cf68d710f9e73f000000000000000000000000f82f4441"),
				T("-4~", "4400000003411bedbe93d011aebc00a0c9053912fcffffff00000000020000000000000008000000000000003d6e3da5fe65e93f000000000000000000000000f82f4441"),
				T("-3~", "4400000003411bedbe93d011aebc00a0c9053912fdffffff0000000002000000000000000800000000000000add35a999fe8ea3f000000000000000000000000f82f4441"),
				T("-2~", "4400000003411bedbe93d011aebc00a0c9053912feffffff000000000200000000000000080000000000000029c14e073e82ec3f000000000000000000000000f82f4441"),
				T("-1~", "4400000003411bedbe93d011aebc00a0c9053912ffffffff0000000002000000000000000800000000000000441310e73734ee3f000000000000000000000000f82f4441"),
				T("0~", "4400000003411bedbe93d011aebc00a0c9053912000000000000000002000000000000000800000000000000000000000000f03f000000000000000000000000f82f4441"),
				T("1~", "4400000003411bedbe93d011aebc00a0c90539120100000000000000020000000000000008000000000000006379d9928ff3f03f000000000000000000000000f82f4441"),
				T("2~", "4400000003411bedbe93d011aebc00a0c9053912020000000000000002000000000000000800000000000000c0d6c7c39af5f13f000000000000000000000000f82f4441"),
				T("3~", "4400000003411bedbe93d011aebc00a0c905391203000000000000000200000000000000080000000000000015b7310afe06f33f000000000000000000000000f82f4441"),
				T("4~", "4400000003411bedbe93d011aebc00a0c90539120400000000000000020000000000000008000000000000008b728df9a228f43f000000000000000000000000f82f4441"),
				T("5~", "4400000003411bedbe93d011aebc00a0c90539120500000000000000020000000000000008000000000000005eecf008815bf53f000000000000000000000000f82f4441"),
				T("6~", "4400000003411bedbe93d011aebc00a0c9053912060000000000000002000000000000000800000000000000cd3b7f669ea0f63f000000000000000000000000f82f4441"),
				T("7~", "4400000003411bedbe93d011aebc00a0c9053912070000000000000002000000000000000800000000000000b0cf68d710f9f73f000000000000000000000000f82f4441"),
				T("8~", "4400000003411bedbe93d011aebc00a0c90539120800000000000000020000000000000008000000000000003c6e3da5fe65f93f000000000000000000000000f82f4441"),
				T("9~", "4400000003411bedbe93d011aebc00a0c9053912090000000000000002000000000000000800000000000000add35a999fe8fa3f000000000000000000000000f82f4441"),
				T("10~", "4400000003411bedbe93d011aebc00a0c90539120a000000000000000200000000000000080000000000000029c14e073e82fc3f000000000000000000000000f82f4441"),
				T("11~", "4400000003411bedbe93d011aebc00a0c90539120b0000000000000002000000000000000800000000000000431310e73734fe3f000000000000000000000000f82f4441"),
				T("12~", "4400000003411bedbe93d011aebc00a0c90539120c00000000000000020000000000000008000000000000000000000000000040000000000000000000000000f82f4441"),
			};
			foreach (var item in tuples)
				reg.SetValue(item.Item1, item.Item2, RegistryValueKind.Binary);
			reg.Close();
			#endregion
			if (checkPresets()) MessageBox.Show("添加预设完成！", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else {
				MessageBox.Show("添加预设失败！", "很遗憾", MessageBoxButtons.OK, MessageBoxIcon.Error);
				ShowError(new Exceptions.NoPluginPitchShiftException());
				return false;
			}
			return true;
		}

		/// <summary>
		/// Vegas 脚本的入口方法。
		/// </summary>
		/// <param name="myVegas">Vegas 软件</param>
		public void FromVegas(Vegas myVegas) {
			vegas = myVegas;
			do {
				if (!ShowConfigForm()) return;
				GenerateOtomad();
				progressForm.Close();
			} while (requestRestartScript);
		}
	}

	/// <summary>
	/// 选中媒体的素材来源。
	/// </summary>
	public enum MediaSourceFrom {
		SELECTED_MEDIA,
		SELECTED_CLIP,
		BROWSE_FILE,
		LAST_USER_PREFERENCE,
		NOTHING_SELECTED
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
		STEP_3_CHANGE_HUE,
		STEP_4_CHANGE_HUE,
		STEP_5_CHANGE_HUE,
		STEP_6_CHANGE_HUE,
		STEP_7_CHANGE_HUE,
		STEP_8_CHANGE_HUE,
		GREY,
		PINGPONG,
		UNLIMITED // PERPETUAL_MOTION
	}

	/// <summary>
	/// 视频的动画效果类。
	/// </summary>
	public class VideoAnimationEffect {
		public static string[][] InitialValues = {
			new string[] { "正" },
			new string[] { "正", "反" },
			new string[] { "正", "倒" },
			new string[] { "0°", "-90°", "-180°", "-270°" },
			new string[] { "0°", "90°", "180°", "270°" },
			new string[] { "0°", "-90°", "-180°", "-270°" },
			new string[] { "0°", "90°", "180°", "270°" },
			new string[] { "正", "倒" },
			new string[] { "左", "右" },
			new string[] { "上", "下" },
			new string[] { "左上", "左下", "右下", "右上" },
			new string[] { "左上", "右上", "右下", "左下" },
			new string[] { "正", "负" },
			new string[] { "正", "负" },
			new string[] { "符", "对" },
			new string[] { "0°", "120°", "240°" },
			new string[] { "0°", "90°", "180°", "270°" },
			new string[] { "0°", "72°", "144°", "216°", "288°" },
			new string[] { "0°", "60°", "120°", "180°", "240°", "300°" },
			new string[] { "0°", "51.4°", "102.9°", "154.3°", "205.7°", "257.1°", "308.6°" },
			new string[] { "0°", "45°", "90°", "135°", "180°", "225°", "270°", "315°" },
			new string[] { "彩", "灰" },
			new string[] { "正", "逆" },
			new string[] { "正", "继" }
		};

		private VideoAnimFx animFx;
		private int step = 0;
		private int duration = 0;

		private bool horizontalFlip = false;
		private bool verticalFlip = false;
		private bool isNegative = false;
		private bool isReverse = false;
		private int rotationStep = 0;
		private int horizontalMirrored = 0;
		private int verticalMirrored = 0;
		private bool isInvertLumin = false;
		private bool isGrey = false;
		private bool enableHueAdjust = false;

		public int HorizontalFlip { get { return horizontalFlip ? -1 : 1; } }
		public int VerticalFlip { get { return verticalFlip ? -1 : 1; } }
		public bool IsNegative { get { return isNegative; } }
		public bool IsReverse { get { return isReverse; } }
		public double RotationDeg { get { return (double)rotationStep * Math.PI / 2; } }
		public int HorizontalMirrored { get { return horizontalMirrored; } }
		public int VerticalMirrored { get { return verticalMirrored; } }
		public bool IsInvertLumin { get { return isInvertLumin; } }
		public double Hue { get { return enableHueAdjust ? (double)step / (double)duration : 0; } }
		public bool IsGrey { get { return isGrey; } }

		public VideoAnimationEffect(VideoAnimFx fx, int initStep = 0) {
			animFx = fx;
			step = initStep;
			duration = InitialValues[(int)fx].Length;
			if (fx >= VideoAnimFx.INVERT_HUE && fx <= VideoAnimFx.STEP_8_CHANGE_HUE) enableHueAdjust = true;
			Update();
		}
		private void NextStep() {
			step = (step + 1) % duration;
		}
		public void Next() {
			NextStep();
			Update();
		}
		private void Update() {
			switch (animFx) {
				case VideoAnimFx.H_FLIP:
					horizontalFlip = step == 1;
					break;
				case VideoAnimFx.V_FLIP:
					verticalFlip = step == 1;
					break;
				case VideoAnimFx.NEGATIVE:
					isNegative = step == 1;
					break;
				case VideoAnimFx.PINGPONG:
					isReverse = step == 1;
					break;
				case VideoAnimFx.CCW_FLIP:
					horizontalFlip = step == 1 || step == 2;
					verticalFlip = step == 2 || step == 3;
					break;
				case VideoAnimFx.CW_FLIP:
					horizontalFlip = step == 2 || step == 3;
					verticalFlip = step == 1 || step == 2;
					break;
				case VideoAnimFx.CCW_ROTATE:
					rotationStep = step;
					break;
				case VideoAnimFx.CW_ROTATE:
					rotationStep = (4 - step) % 4;
					break;
				case VideoAnimFx.TURNED:
					rotationStep = step * 2;
					break;
				case VideoAnimFx.H_MIRROR:
					horizontalMirrored = step == 0 ? 1 : 2;
					break;
				case VideoAnimFx.V_MIRROR:
					verticalMirrored = step == 0 ? 1 : 2;
					break;
				case VideoAnimFx.CCW_MIRROR:
					horizontalMirrored = (step == 0 || step == 1) ? 1 : 2;
					verticalMirrored = (step == 0 || step == 3) ? 1 : 2;
					break;
				case VideoAnimFx.CW_MIRROR:
					horizontalMirrored = (step == 0 || step == 3) ? 1 : 2;
					verticalMirrored = (step == 0 || step == 1) ? 1 : 2;
					break;
				case VideoAnimFx.INVERT_LUMIN:
					isInvertLumin = step == 1;
					break;
				case VideoAnimFx.GREY:
					isGrey = step == 1;
					break;
				case VideoAnimFx.UNLIMITED:
					horizontalFlip = isReverse = step == 1;
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// 查找的插件。
	/// </summary>
	public class Plugin {
		public static PlugInNode pitchShift;
		public static PlugInNode invert;
		public static PlugInNode hslAdjust;
		public static PlugInNode labAdjust;
		public static PlugInNode blackAndWhite;
		public static PlugInNode mirror;
		public static PlugInNode picInPic;
		public static PlugInNode crop;
		public static PlugInNode solidColor;
		/// <summary>
		/// 初始化所需插件。
		/// </summary>
		/// <param name="vegas">Vegas 软件</param>
		public static void Init(Vegas vegas) {
			pitchShift = vegas.AudioFX.FindChildByName("移调")
				?? vegas.AudioFX.FindChildByName("Pitch Shift")
				?? vegas.AudioFX.FindChildByUniqueID("{ED1B4100-93BE-11D0-AEBC-00A0C9053912}");
			invert = vegas.VideoFX.FindChildByName("VEGAS 反转")
				?? vegas.VideoFX.FindChildByName("VEGAS Invert")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:invert}");
			hslAdjust = vegas.VideoFX.FindChildByName("VEGAS HSL 调整")
				?? vegas.VideoFX.FindChildByName("VEGAS HSL Adjust")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:hsladjust}");
			labAdjust = vegas.VideoFX.FindChildByName("VEGAS LAB 调整")
				?? vegas.VideoFX.FindChildByName("VEGAS LAB Adjust")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:labadjust}");
			blackAndWhite = vegas.VideoFX.FindChildByName("VEGAS 黑白")
				?? vegas.VideoFX.FindChildByName("VEGAS Black And White")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:blackandwhite}");
			mirror = vegas.VideoFX.FindChildByName("VEGAS 镜像")
				?? vegas.VideoFX.FindChildByName("VEGAS Mirror")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:mirror}");
			picInPic = vegas.VideoFX.FindChildByName("VEGAS 画中画")
				?? vegas.VideoFX.FindChildByName("VEGAS Picture In Picture")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:pictureinpicture}");
			crop = vegas.VideoFX.FindChildByName("VEGAS 修剪")
				?? vegas.VideoFX.FindChildByName("VEGAS Crop")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:crop}");
			solidColor = vegas.Generators.FindChildByName("VEGAS 纯色")
				?? vegas.VideoFX.FindChildByName("VEGAS Solid Color")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:solidcolor}");
		}
		/// <summary>
		/// 应用反转颜色效果。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		public static void Negative(VideoEvent videoEvent) {
			Effect effect = new Effect(invert);
			videoEvent.Effects.Add(effect);
		}
		/// <summary>
		/// 应用黑白效果。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		public static void Grey(VideoEvent videoEvent) {
			Effect effect = new Effect(blackAndWhite);
			videoEvent.Effects.Add(effect);
		}
		/// <summary>
		/// 应用反转亮度效果。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		public static void InvertLumin(VideoEvent videoEvent) {
			Effect effect = new Effect(labAdjust);
			videoEvent.Effects.Add(effect);
			var invertLumin = (OFXBooleanParameter)effect.OFXEffect.FindParameterByName("InvertLuminance");
			invertLumin.Value = true;
		}
		/// <summary>
		/// 应用改变色相效果。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <param name="value">色相值</param>
		public static void ChangeHue(VideoEvent videoEvent, double value) {
			Effect effect = new Effect(hslAdjust);
			videoEvent.Effects.Add(effect);
			var hue = (OFXDoubleParameter)effect.OFXEffect.FindParameterByName("AddToHue");
			hue.Value = value;
		}
		private const double MIRROR_UP = 0;
		private const double MIRROR_DOWN = 180;
		private const double MIRROR_LEFT = 90;
		private const double MIRROR_RIGHT = -90;
		/// <summary>
		/// 应用镜像效果。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <param name="horizontalDirect">水平镜像选项。0 表示不水平镜像，1 表示镜像左边，2 表示镜像右边</param>
		/// <param name="verticalDirect">垂直镜像选项。0 表示不垂直镜像，1 表示镜像上边，2 表示镜像下边</param>
		public static void Mirror(VideoEvent videoEvent, int horizontalDirect, int verticalDirect) {
			if (horizontalDirect != 0) {
				Effect effect = new Effect(mirror);
				videoEvent.Effects.Add(effect);
				var angle = (OFXDoubleParameter)effect.OFXEffect.FindParameterByName("Angle");
				angle.Value = horizontalDirect == 1 ? MIRROR_LEFT : MIRROR_RIGHT;
			}
			if (verticalDirect != 0) {
				Effect effect = new Effect(mirror);
				videoEvent.Effects.Add(effect);
				var angle = (OFXDoubleParameter)effect.OFXEffect.FindParameterByName("Angle");
				angle.Value = verticalDirect == 1 ? MIRROR_UP : MIRROR_DOWN;
			}
		}
	}

	/// <summary>
	/// 继承的 MIDI 类。
	/// </summary>
	public class MIDI : MidiFile {
		public TrackInfo[] TrackInfos = null;
		public int TicksPerQuarter = 0;
		public double MsPerQuarter = 0;
		public double Bpm {
			get { return 6e4 / MsPerQuarter; }
			set { MsPerQuarter = 6e4 / value; }
		}
		public string Path;
		public int? SuggestSelectedIndex = null; // 第一个不是零音符的音轨
		public class TrackInfo {
			public int Index;
			public string Name = "";
			public int NotesCount = 0;
			public string BeginNote = "";
			public bool HasName { get { return Name.Length != 0; } }
			public override string ToString() {
				List<string> info = new List<string>(new string[] {
					"轨道 " + Index,
					"音符数：" + NotesCount,
					"起音 " + BeginNote
				});
				if (HasName) info[0] += "：" + Name;
				if (NotesCount == 0) info.RemoveAt(2);
				return string.Join("；", info);
			}
		}
		public MIDI(string path) : base(path) {
			Path = path;
			TrackInfos = new TrackInfo[Events.Tracks];
			TicksPerQuarter = DeltaTicksPerQuarterNote;
			MsPerQuarter = 0; // 毫秒每拍
			for (int i = 0; i < Events.Tracks; i++) {
				TrackInfo info = TrackInfos[i] = new TrackInfo();
				info.Index = i;
				foreach (MidiEvent midiEvent in Events[i]) {
					if ((midiEvent is NoteEvent) && !(midiEvent is NoteOnEvent)) {
						NoteEvent noteEvent = (NoteEvent)midiEvent;
						if (info.NotesCount++ == 0) info.BeginNote = noteEvent.NoteName; // 起音判断
					}
					if ((midiEvent is PatchChangeEvent) && !info.HasName) {
						PatchChangeEvent patchEvent = (PatchChangeEvent)midiEvent;
						for (int j = 4; j < patchEvent.ToString().Split(' ').Length; j++)
							info.Name += patchEvent.ToString().Split(' ')[j]; // 音轨名称
					}
					if ((midiEvent is TempoEvent) && MsPerQuarter == 0) {
						TempoEvent tempoEvent = (TempoEvent)midiEvent;
						MsPerQuarter = Convert.ToDouble(tempoEvent.MicrosecondsPerQuarterNote) / 1000; // 每四分音符多少毫秒
						// MessageBox.Show(tempoEvent.Tempo.ToString()); // 用 Tempo 表示 BPM
					}
				}
				if (info.NotesCount != 0) SuggestSelectedIndex = SuggestSelectedIndex ?? i;
			}
		}
	}

	/// <summary>
	/// 自定义错误信息。
	/// </summary>
	internal static class Exceptions {
		public class NoMidiException : Exception {
			/// <summary>
			/// 未选择 MIDI 文件报错。
			/// </summary>
			public NoMidiException() : base("错误：未选择 MIDI 文件。\n\n" +
				"请重新打开脚本参数配置对话框，然后在“MIDI 配置”分组中点击“浏览”按钮，打开一个有效的 MIDI 文件。") { }
		}

		public class NoMediaException : Exception {
			/// <summary>
			/// 未选择媒体文件报错。
			/// </summary>
			public NoMediaException() : base("错误：未选择媒体文件。\n\n" +
				"请重新打开脚本参数配置对话框，然后在“媒体配置”分组中点击“浏览”按钮，打开一个有效的媒体文件。") { }
		}

		public class NoTrackInfoException : Exception {
			/// <summary>
			/// 无 MIDI 音轨信息报错。
			/// </summary>
			public NoTrackInfoException() : base("错误：没有 MIDI 音轨。\n\n可能的原因：\n" +
				"1. 您没有选择一个 MIDI 音轨；\n" +
				"2. 该 MIDI 文件中没有任何音轨；\n" +
				"3. 该 MIDI 文件已损坏或文件格式不受支持。") { }
		}

		public class NoPluginPitchShiftException : Exception {
			/// <summary>
			/// 无法调用移调插件报错。
			/// </summary>
			public NoPluginPitchShiftException() : base("错误：无法调用移调插件。\n\n" +
				"请按照教程文档 " + ConfigForm.aboutHelpLink + " 的指引正确操作。\n" +
				"不过，根据这个更新版本的脚本，按理应当是中英文版本均可正常运行的。\n" +
				"因此很有可能您是使用其它语言的 Vegas 造成的（逃") { }
		}

		public class NoPluginPresetException : Exception {
			/// <summary>
			/// 无法调用移调插件的预设效果报错。
			/// </summary>
			public NoPluginPresetException() : base("错误：无法调用移调插件的预设效果。\n\n" +
				"请按照教程文档 " + ConfigForm.aboutHelpLink + " 的指引正确操作。\n" +
				"确保在移调插件中手动添加了所有的 25 个预设，且命名正确。\n\n" +
				"补充说明：具体可见上述链接专栏中对于安装方法的说明。这 25 个预设是上下一个八度以内的所有变调种类，\n" +
				"缺少任何一个都有可能出错。手动添加预设的确非常麻烦，但 Vegas 无法使用脚本来指定变调的具体参数，\n" +
				"因此只好绕这个弯子。") { }
		}

		public class NoPluginInvertException : Exception {
			/// <summary>
			/// 无法调用反转插件报错。
			/// </summary>
			public NoPluginInvertException() : base("错误：无法调用反转插件。\n\n" +
				"可能您使用的是非中文版的 Vegas 或其它尚未测试版本的 Vegas。") { }
		}

		public class NoPluginNameException : Exception {
			/// <summary>
			/// 无法调用某个插件报错。
			/// </summary>
			public NoPluginNameException(string pluginName) : base("错误：无法调用" + pluginName + "插件。\n\n" +
				"可能您使用的是非中文版的 Vegas 或其它尚未测试版本的 Vegas。") { }
		}

		private const string NoTakeExceptionPS = "补充说明：若仍不能解决，说明该素材文件可能是 Vegas 不支持的格式，\n" +
			"可以手动把该文件拖入 Vegas 中看一下是否视频音频都正常。";
		public class NoAudioTakeException : Exception {
			/// <summary>
			/// 无法读取音频媒体流报错。
			/// </summary>
			public NoAudioTakeException() : base("错误：无法读取音频媒体流。\n\n" +
				"在设置界面，纯视频/图片素材不要勾选“生成音频”。\n\n" +
				NoTakeExceptionPS) { }
		}
		public class NoVideoTakeException : Exception {
			/// <summary>
			/// 无法读取视频媒体流报错。
			/// </summary>
			public NoVideoTakeException() : base("错误：无法读取视频媒体流。\n\n" +
				"在设置界面，纯音频素材不要勾选“生成视频”。\n\n" +
				NoTakeExceptionPS) { }
		}
		public class NoMediaTakeException : Exception {
			/// <summary>
			/// 无法读取媒体报错。
			/// </summary>
			public NoMediaTakeException() : base("错误：无法读取媒体。\n\n" +
				"您所选的文件格式不受 Vegas 支持，请检查该媒体文件是否损坏，或未安装对应的 Vegas 解码器。\n\n" +
				NoTakeExceptionPS) { }
		}

		public class NotAMidiFileException : Exception {
			/// <summary>
			/// 无法调用移调插件的预设效果报错。
			/// </summary>
			public NotAMidiFileException() : base("错误：无法读取 MIDI 文件。\n\n" +
				"解决方法：用宿主软件导入该 MIDI，然后重新输出一个新的 MIDI 文件。\n\n" +
				"补充说明：MIDI 文件有多种格式，脚本不保证都能够正确读取。所幸主流宿主软件在\n" +
				"默认设置下导出的 MIDI 文件一般是可以读取的。（目前测试过 FL Studio、LMMS \n" +
				"与 Music Studio for iPad。）") { }
		}

		private const string NoSelectedExceptionPS = "补充说明：如果您想手动在文件夹中选择一个媒体素材，那么请点击其右边的“浏览”按钮，\n" +
			"选择一个媒体素材。并确保左侧的下拉菜单中选中的是您所选文件所在的路径。";
		public class NoSelectedMediaException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedMediaException() : base("错误：没有在项目媒体窗口中选择任何媒体。\n\n" +
				"请在项目媒体窗口中选择一个媒体，然后重新打开参数配置窗口，并在素材设置中选择“选中的媒体文件”。\n\n" +
				NoSelectedExceptionPS) { }
		}
		public class NoSelectedClipException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedClipException() : base("错误：没有在轨道中选择任何剪辑。\n\n" +
				"请在轨道中选择一个剪辑，然后重新打开参数配置窗口，并在素材设置中选择“选中的轨道素材”。\n\n" +
				NoSelectedExceptionPS) { }
		}
		public class NoTimeStretchPitchShiftException : Exception {
			/// <summary>
			/// 音调转换方法设置为不调音的报错。
			/// </summary>
			public NoTimeStretchPitchShiftException() : base("错误：选定素材音调转换方法被设置为不调音。\n\n" +
				"很有可能您使用的是“选中的轨道素材”。出现了这个错误不怪你，要怪就怪 Vegas 这个脑残设计。\n\n" +
				"解决方法：请重新选中您的轨道素材，右键音频部分，选择底部的“属性”。将“时间拉伸/音调转换”的“方法”设定为“élastique”。\n" +
				"然后点击确定即可。\n\n" +
				"补充说明：如果某个音频事件没有进行变调操作，然后打开了它的属性，那么其属性中的“时间拉伸/音调转换”的“方法”会被\n" +
				"自动修改为“无”，点击确定就会生效。这时你会发现键盘上的 +、- 键调音操作无效了。这时必须重新打开音频事件的属性，\n" +
				"将“时间拉伸/音调转换”的“方法”设定为“élastique”，不必设置“音调更改”，点击确定即可。"/*  +
				"这里本来想用脚本自动操作的，然而在官方文档中给出了一个时间拉伸/音调转换方法的枚举，但这个枚举在任何地方都不使用，\n" +
				"因此这个中看不中用的玩意是不是作者当时喝醉了写的。" */) { }
		}
		public class ReadConfigFailException : Exception {
			/// <summary>
			/// 读取参数配置文件失败报错。
			/// </summary>
			public ReadConfigFailException() : base("错误：读取参数配置文件失败。\n\n" +
				"很遗憾您遇到了这个不可预见的错误。我们将会清除用户配置设置并恢复为默认值以便解决问题。\n" +
				"建议将这个错误告诉作者以便快速解决问题。\n" +
				"将会退出此脚本，然后劳烦阁下手动重新打开此脚本。") { }
		}
	}

	/// <summary>
	/// 路径类，用于处理路径。
	/// </summary>
	public class Path : List<string> {
		public Path(string path) : base(path.Replace("\\", "/").TrimEnd('/').Split('/')) { }
		public Path(string[] arr) : base(arr) { }
		public static Path r(params string[] arr) {
			Path path = new Path(arr[0]);
			for (int i = 1; i < arr.Length; i++)
				path += new Path(arr[i]);
			return path;
		}
		private char sep = '\\';
		private bool isWindows = true;
		public bool IsWindows {
			get { return isWindows; }
			set {
				isWindows = value;
				sep = value ? '\\' : '/';
			}
		}
		public void UpOneLevel() {
			RemoveAt(Count - 1);
		}
		public override string ToString() {
			return string.Join(sep.ToString(), this);
		}
		private string GetLastItem() {
			return this[Count - 1];
		}
		public string FullPath {
			get { return ToString(); }
		}
		public string FullFileName {
			get { return GetLastItem(); }
			set { this[Count - 1] = value; }
		}
		private static readonly Regex extReg = new Regex(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");
		public string Extension {
			get {
				MatchCollection ext = extReg.Matches(FullFileName);
				return ext.Count != 0 ? ext[0].ToString() : "";
			}
			set {
				value = value.Trim().TrimStart('.');
				if (extReg.IsMatch(FullFileName)) extReg.Replace(FullFileName, value);
				else FullFileName += '.' + value;
			}
		}
		public string FileName {
			get {
				if (Extension == "") return FullFileName;
				string fileName = extReg.Replace(FullFileName, "");
				return fileName.Substring(0, fileName.Length - 1);
			}
			set {
				FullFileName = value + '.' + Extension;
			}
		}
		public string Directory {
			get {
				if (Count == 0) return sep.ToString();
				return new Path(GetRange(0, Count - 1).ToArray()).ToString();
			}
		}
		public static bool operator ==(Path path1, Path path2) {
			// return path1.SequenceEqual(path2); // 没有 System.Linq
			if (IsNull(path1) || IsNull(path2)) return false; // 如果用 == 会递归
			if (path1.Count != path2.Count) return false;
			for (int i = 0; i < path1.Count; i++)
				if (path1[i] != path2[i])
					return false;
			return true;
		}
		public static bool operator !=(Path path1, Path path2) {
			return !(path1 == path2);
		}
		public static Path operator +(Path path1, Path path2) {
			if (path1 == null) return path2;
			path1.AddRange(path2);
			return path1;
		}
		public static Path operator +(Path path1, string path2) {
			path1.AddRange(new Path(path2));
			return path1;
		}
		public static Path operator +(string _path1, Path path2) {
			Path path1 = new Path(_path1);
			for (int i = 0; i < path1.Count; i++)
				path1.Insert(i, path1[i]);
			return path1;
		}
		public override bool Equals(Object obj) {
			if (object.ReferenceEquals(this, obj)) return true;
			if (obj == null || !(obj is Path)) return false;
			return this == (Path)obj;
		}
		public override int GetHashCode() {
			int hash = 0;
			foreach (string file in this)
				hash ^= file.GetHashCode();
			return hash;
		}
		private static bool IsNull(Path path) {
			return (object)path == null;
		}
	}

	/// <summary>
	/// 用于读取和保存 INI 配置文件。
	/// </summary>
	public class IniFile {
		// 读写 INI 文件的 API
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);
		[DllImport("kernel32")]
		private static extern int WritePrivateProfileString(string lpApplicationName, string lpKeyName, string lpString, string lpFileName);

		// 获取错误信息
		[DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
		public static extern IntPtr GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
		[DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
		public static extern bool FreeLibrary(int hModule);
		[DllImport("Kernel32.dll")]
		public extern static int FormatMessage(int flag, ref IntPtr source, int msgid, int langid, ref string buf, int size, ref IntPtr args);

		/// <summary>
		/// 获取系统错误信息描述。
		/// </summary>
		/// <param name="errCode">系统错误码</param>
		/// <returns>系统错误信息描述</returns>
		private static string GetSysErrMsg(int errCode) {
			IntPtr tempptr = IntPtr.Zero;
			string msg = null;
			FormatMessage(0x1300, ref tempptr, errCode, 0, ref msg, 255, ref tempptr);
			return msg;
		}

		private int ErrCode { get { return Marshal.GetLastWin32Error(); } }

		private readonly ConfigForm form;
		private const int MAX_VALUE_LENGTH = 1024;
		private string filePath = null;
		protected string currentSection = null;

		/// <summary>
		/// 打开或创建一个 INI 文件。
		/// </summary>
		/// <param name="filePath">INI 文件路径</param>
		public IniFile(string filePath, ConfigForm configForm) {
			form = configForm;
			this.filePath = filePath;
		}

		/// <summary>
		/// 读取 INI 文件。
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="def">没有查到的话返回的默认值</param>
		/// <param name="section">节点名</param>
		/// <returns>字符串参数值</returns>
		public string Read(string key, string def, string section = null) {
			section = section ?? currentSection;
			if (section == null || filePath == null) return def;
			StringBuilder sb = new StringBuilder(MAX_VALUE_LENGTH);
			/* if (GetPrivateProfileString(section, key, def, sb, MAX_VALUE_LENGTH, filePath) == 0) {
				int errCode = ErrCode;
				form.parent.ShowError("错误代码：" + errCode + "\n\n" + GetSysErrMsg(errCode));
				return def;
			} else return sb.ToString(); */
			GetPrivateProfileString(section, key, def, sb, MAX_VALUE_LENGTH, filePath);
			return sb.ToString();
		}

		/// <summary>
		/// 写入 INI 文件。
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="def">写入的值</param>
		/// <param name="section">节点名</param>
		/// <returns>是否成功写入</returns>
		public bool Write(string key, object value, string section = null) {
			// CheckPath(filePath);
			section = section ?? currentSection;
			if (section == null || filePath == null) return false;
			if (value is bool) value = (bool)value ? 1 : 0;
			if (value is string && (string)value == "") value = null;
			if (WritePrivateProfileString(section, key, value == null ? null : value.ToString(), filePath) == 0) {
				int errCode = ErrCode;
				form.parent.ShowError("错误代码：" + errCode + "\n\n" + GetSysErrMsg(errCode));
				return false;
			} else return true;
		}

		/// <summary>
		/// 删除节。
		/// </summary>
		/// <param name="section">节点名</param>
		/// <returns>是否成功删除</returns>
		public bool DeleteSection(string section) {
			return Write(null, null, section);
		}

		/// <summary>
		/// 删除键。
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="section">节点名</param>
		/// <returns>是否成功删除</returns>
		public bool DeleteKey(string key, string section = null) {
			return Write(key, null, section);
		}

		/// <summary>
		/// 开始节。
		/// </summary>
		/// <param name="section">节点名</param>
		public void StartSection(string section) {
			currentSection = section;
		}

		/// <summary>
		/// 结束节。
		/// </summary>
		public void EndSection() {
			currentSection = null;
		}

		public int ReadInt(string key, int def, string section = null) {
			string result = Read(key, def.ToString(), section);
			int value;
			return int.TryParse(result, out value) ? value : def;
		}
		public double ReadDouble(string key, double def, string section = null) {
			string result = Read(key, def.ToString(), section);
			double value;
			return double.TryParse(result, out value) ? value : def;
		}
		public bool ReadBool(string key, bool def, string section = null) {
			string result = Read(key, def ? "1" : "0", section).Trim().ToLower();
			return result != "0" && result != "false";
		}

		/// <summary>
		/// 删除 INI 文件。
		/// 补充：后面改成了删除文件后重新创建文件，相当于清空配置设置。
		/// </summary>
		/// <param name="ensure">确认删除，必须为 true 才能删除，否则无作用</param>
		public void Delete(bool ensure) {
			if (!ensure) return;
			File.Delete(filePath);
			// File.Create(filePath);
			// filePath = null;
		}
	}
	
	#region 其它自定义控件部分
	partial class IntegerTrackWithBox {
		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary> 
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.Track = new System.Windows.Forms.TrackBar();
			this.Numeric = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.Track)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 2;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel.Controls.Add(this.Track, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.Numeric, 1, 0);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(377, 39);
			this.tableLayoutPanel.TabIndex = 1;
			// 
			// Track
			// 
			this.Track.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.Track.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Track.Location = new System.Drawing.Point(0, 0);
			this.Track.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this.Track.Maximum = 100;
			this.Track.Name = "Track";
			this.Track.Size = new System.Drawing.Size(294, 39);
			this.Track.TabIndex = 0;
			this.Track.TickFrequency = 10;
			this.Track.Scroll += new System.EventHandler(this.Track_Scroll);
			// 
			// Numeric
			// 
			this.Numeric.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Numeric.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Numeric.Location = new System.Drawing.Point(302, 0);
			this.Numeric.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
			this.Numeric.Name = "Numeric";
			this.Numeric.Size = new System.Drawing.Size(75, 23);
			this.Numeric.TabIndex = 1;
			this.Numeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
			// 
			// IntegerTrackWithBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IntegerTrackWithBox";
			this.Size = new System.Drawing.Size(377, 39);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.Track)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Numeric)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.TrackBar Track;
		private System.Windows.Forms.NumericUpDown Numeric;
	}
	
	[ToolboxBitmap(typeof(TrackBar))]
	public partial class IntegerTrackWithBox : UserControl {
		public IntegerTrackWithBox() {
			InitializeComponent();
			Track.MouseClick += new MouseEventHandler(Track_Reset);
		}

		private void Track_Scroll(object sender, EventArgs e) {
			Numeric.Value = Track.Value;
		}

		private void Numeric_ValueChanged(object sender, EventArgs e) {
			Track.Value = (int)Numeric.Value;
		}

		[Description("指示数值控件的当前值。"), Category("Behavior"), DefaultValue(0)]
		public int Value {
			get { return Track.Value; }
			set {
				Track.Value = value;
				Numeric.Value = value;
			}
		}

		[Description("指示数值控件的最小值。"), Category("Behavior"), DefaultValue(0)]
		public int Minimum {
			get { return Track.Minimum; }
			set {
				Track.Minimum = value;
				Numeric.Minimum = value;
				UpdateTickFrequency();
			}
		}

		[Description("指示数值控件的最大值。"), Category("Behavior"), DefaultValue(100)]
		public int Maximum {
			get { return Track.Maximum; }
			set {
				Track.Maximum = value;
				Numeric.Maximum = value;
				UpdateTickFrequency();
			}
		}

		private const int tickFrequency = 10;

		private void UpdateTickFrequency() {
			Track.TickFrequency = (Track.Maximum - Track.Minimum) / tickFrequency;
		}

		[Description("指示在跟踪条上的哪些位置显示刻度。"), Category("Appearance"), DefaultValue(typeof(TickStyle), "BottomRight")]
		public TickStyle TickStyle {
			get { return Track.TickStyle; }
			set { Track.TickStyle = value; }
		}

		[Description("数值选择控件的宽度。"), Category("Layout"), DefaultValue(75)]
		public int NumericUpDownWidth {
			get { return Numeric.Width; }
			set { Numeric.Width = value; }
		}

		private int defaultValue = 0;

		[Description("指示数值控件的默认值，当用户鼠标右键跟踪条时可以重置为默认值。"), Category("Behavior"), DefaultValue(0)]
		public int DefaultValue {
			get { return defaultValue; }
			set {
				if (value < Minimum || value > Maximum)
					throw new ArgumentOutOfRangeException("Value", "输入的默认值比最小值小或比最大值大。");
				defaultValue = value;
			}
		}

		/// <summary>
		/// 右键滑动条，可以重置其值。
		/// </summary>
		private void Track_Reset(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
				Value = DefaultValue;
		}
		
		/// <summary> 
		/// 设定数值控件的当前值，并确保不受最大值或最小值限制的干扰。
		/// </summary>
		/// <param name="value">设定值</param>
		/// <param name="def">如果设定失败后的默认值，如果为 null 表示不改变。</param>
		public void SetValue(int value, int? def = null) {
			if (value < Minimum || value > Maximum) {
				if (def == null || def < Minimum || def > Maximum) return;
				Value = (int)def;
			} else Value = value;
		}
	}
	
	[ToolboxBitmap(typeof(TextBox))]
	public class TimecodeBox : TextBox {
		public TimecodeBox() : base() {
			Leave += (sender, e) => {
				base.Text = DealLegal(Text);
			};
		}

		const string DEFAULT_TIME = "0:00.000";

		[Description("与控件关联的文本。"), Category("Appearance"), DefaultValue(DEFAULT_TIME)]
		public override string Text {
			get { return base.Text; }
			set { base.Text = DealLegal(value); }
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Value {
			get { return ClipTrimTime2Ms(Text); }
			set { Text = FormatClipTrimTime(value); }
		}

		[Description("与控件关联的文本对应的毫秒双精度浮点值。"), Category("Behavior"), DefaultValue(0)]
		public double DoubleValue {
			get { return Value; }
			set { Value = (int)value; }
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Millisecond {
			get { return Value; }
			set { Value = value; }
		}

		public static string DealLegal(string raw) {
			raw = new Regex(@"[:：;；]+").Replace(raw, ":");
			raw = new Regex(@"[\.。．,，、]+").Replace(raw, ".");
			raw = new Regex(@"\s").Replace(raw, "");
			raw = new Regex(@"^\.").Replace(raw, "0.");
			MatchCollection matches = Regex.Matches(raw, @"(\d+:){0,2}\d+(\.\d{1,3})?");
			if (matches.Count == 0) return DEFAULT_TIME;
			else {
				string bestMatch = "";
				foreach (Match match in matches)
					if (match.ToString().Length > bestMatch.Length)
						bestMatch = match.ToString();
				raw = bestMatch;
			}
			return FormatClipTrimTime(raw);
		}

		/// <summary>
		/// 将输入的时间格式的字符串转换为毫秒。
		/// </summary>
		/// <param name="clipTrimTime">时间格式，如 “0:00.000”</param>
		/// <returns>毫秒值</returns>
		public static int ClipTrimTime2Ms(string clipTrimTime) {
			if (clipTrimTime == "") clipTrimTime = DEFAULT_TIME;
			int h = 0, m = 0, s = 0, ms = 0;
			string[] timeSplitDot = clipTrimTime.Split('.');
			if (timeSplitDot.Length >= 2) {
				string ms_str = timeSplitDot[1];
				while (ms_str.Length < 3) ms_str += '0';
				ms = int.Parse(ms_str);
			}
			string[] timeSplitColon = timeSplitDot[0].Split(':');
			var tryParseInt = new Func<string, int, int>((str, def) => {
				int result = def;
				bool ok = int.TryParse(str, out result);
				if (!ok) result = def;
				return result;
			});
			s = tryParseInt(timeSplitColon[timeSplitColon.Length - 1], 0); // timeSplitColon[^1]
			if (timeSplitColon.Length >= 2) m = tryParseInt(timeSplitColon[timeSplitColon.Length - 2], 0); // timeSplitColon[^2]
			if (timeSplitColon.Length >= 3) h = tryParseInt(timeSplitColon[timeSplitColon.Length - 3], 0); // timeSplitColon[^3]
			int value = ((h * 60 + m) * 60 + s) * 1000 + ms;
			return value;
		}

		/// <summary>
		/// 格式化时间值。
		/// </summary>
		/// <param name="value">毫秒值</param>
		/// <returns>时间格式，如 “0:00.000”</returns>
		public static string FormatClipTrimTime(int value) {
			int ms = value % 1000; value /= 1000;
			int s = value % 60; value /= 60;
			int m = value % 60; value /= 60;
			int h = value;
			return h == 0 ?
				string.Format("{0:D}:{1:D2}.{2:D3}", m, s, ms) :
				string.Format("{0:D}:{1:D2}:{2:D2}.{3:D3}", h, m, s, ms);
		}

		public static string FormatClipTrimTime(string clipTrimTime) {
			return FormatClipTrimTime(ClipTrimTime2Ms(clipTrimTime));
		}
	}
	
	partial class ProgressForm {
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLabel = new System.Windows.Forms.Label();
			this.PercentLabel = new System.Windows.Forms.Label();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.CancelBtn, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 141);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 42);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(386, 9);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 24);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.InfoLabel, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.PercentLabel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.ProgressBar, 0, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(3);
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(471, 141);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// InfoLabel
			// 
			this.InfoLabel.AutoSize = true;
			this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLabel.Location = new System.Drawing.Point(8, 63);
			this.InfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.InfoLabel.Size = new System.Drawing.Size(455, 33);
			this.InfoLabel.TabIndex = 0;
			this.InfoLabel.Text = "正在生成音 MAD……";
			this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PercentLabel
			// 
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PercentLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
			this.PercentLabel.Location = new System.Drawing.Point(3, 3);
			this.PercentLabel.Margin = new System.Windows.Forms.Padding(0);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(465, 60);
			this.PercentLabel.TabIndex = 1;
			this.PercentLabel.Text = "0%";
			this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.ProgressBar.Location = new System.Drawing.Point(11, 99);
			this.ProgressBar.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(449, 23);
			this.ProgressBar.Step = 1;
			this.ProgressBar.TabIndex = 2;
			this.ProgressBar.UseWaitCursor = true;
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(471, 183);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProgressForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "正在处理它";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label InfoLabel;
		private System.Windows.Forms.Label PercentLabel;
		private System.Windows.Forms.ProgressBar ProgressBar;
	}
	
	public partial class ProgressForm : Form {
		public ProgressForm() {
			InitializeComponent();
			ControlBox = false;
			Icon = ConfigForm.icon;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			RequestAbort = true;
			Close();
		}

		public bool RequestAbort = false;
		public int Minimum { get { return ProgressBar.Minimum; } }
		public int Maximum { get { return ProgressBar.Maximum; } }

		public void ReportProgress(int value) {
			if (value < Minimum) value = Minimum;
			if (value > Maximum) value = Maximum;
			ProgressBar.Value = value;
			PercentLabel.Text = value + "%";
			Application.DoEvents();
			ProgressBar.Update();
			ProgressBar.Refresh();
			PercentLabel.Update();
			PercentLabel.Refresh();
		}

		public void ReportProgress(double value) {
			ReportProgress((int)value);
		}

		public int Progress {
			get { return ProgressBar.Value; }
			set { ReportProgress(value); }
		}

		public string Info {
			get { return InfoLabel.Text; }
			set {
				InfoLabel.Text = value != "" ? value : "正在生成音 MAD……";
				InfoLabel.Update();
				InfoLabel.Refresh();
			}
		}
	}
	#endregion

	#region 设计器部分
	partial class ConfigForm {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.AboutBtn = new System.Windows.Forms.Button();
			this.UserHelpLink = new System.Windows.Forms.LinkLabel();
			this.Tabs = new System.Windows.Forms.TabControl();
			this.SourceTab = new System.Windows.Forms.TabPage();
			this.WarningInfoLabel = new System.Windows.Forms.Label();
			this.MidiConfigGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.label8 = new System.Windows.Forms.Label();
			this.MidiStartSecondBox = new VegasScript.TimecodeBox();
			this.label9 = new System.Windows.Forms.Label();
			this.MidiEndSecondBox = new VegasScript.TimecodeBox();
			this.label7 = new System.Windows.Forms.Label();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.label6 = new System.Windows.Forms.Label();
			this.MidiBeatCombo = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.ChooseMidiBtn = new System.Windows.Forms.Button();
			this.ChooseMidiText = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.MidiChannelCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
			this.MidiMidiBpmCheck = new System.Windows.Forms.RadioButton();
			this.MidiProjectBpmCheck = new System.Windows.Forms.RadioButton();
			this.MidiCustomBpmCheck = new System.Windows.Forms.RadioButton();
			this.MidiCustomBpmBox = new System.Windows.Forms.NumericUpDown();
			this.SourceConfigGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
			this.GenerateAtBeginRadio = new System.Windows.Forms.RadioButton();
			this.GenerateAtCursorRadio = new System.Windows.Forms.RadioButton();
			this.label36 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.ChooseSourceCombo = new System.Windows.Forms.ComboBox();
			this.ChooseSourceBtn = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.SourceStartTimeText = new VegasScript.TimecodeBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SourceEndTimeText = new VegasScript.TimecodeBox();
			this.AudioTab = new System.Windows.Forms.TabPage();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.AudioFadeOutCurveCombo = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.AudioFadeInBox = new VegasScript.IntegerTrackWithBox();
			this.AudioFadeOutBox = new VegasScript.IntegerTrackWithBox();
			this.AudioFadeInCurveCombo = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.AudioTuneMethodCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.AudioMainKeyCombo = new System.Windows.Forms.ComboBox();
			this.AudioMainOctaveCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.AudioConfigCheck = new System.Windows.Forms.CheckBox();
			this.AudioScratchCheck = new System.Windows.Forms.CheckBox();
			this.AudioLoopCheck = new System.Windows.Forms.CheckBox();
			this.AudioNormalizeCheck = new System.Windows.Forms.CheckBox();
			this.VideoTab = new System.Windows.Forms.TabPage();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.VideoEndVerticalTransBox = new VegasScript.IntegerTrackWithBox();
			this.VideoStartVerticalTransBox = new VegasScript.IntegerTrackWithBox();
			this.VideoEndHorizontalTransBox = new VegasScript.IntegerTrackWithBox();
			this.VideoStartHorizontalTransBox = new VegasScript.IntegerTrackWithBox();
			this.VideoEndRotationBox = new VegasScript.IntegerTrackWithBox();
			this.VideoStartRotationBox = new VegasScript.IntegerTrackWithBox();
			this.VideoEndSizeBox = new VegasScript.IntegerTrackWithBox();
			this.VideoStartSizeCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoStartSizeBox = new VegasScript.IntegerTrackWithBox();
			this.label25 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label22 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.VideoFadeOutCurveCombo = new System.Windows.Forms.ComboBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.VideoFadeInBox = new VegasScript.IntegerTrackWithBox();
			this.VideoFadeOutBox = new VegasScript.IntegerTrackWithBox();
			this.VideoFadeInCurveCombo = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.VideoEffectInitialValueCombo = new System.Windows.Forms.ComboBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.VideoEffectCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.VideoConfigCheck = new System.Windows.Forms.CheckBox();
			this.VideoScratchCheck = new System.Windows.Forms.CheckBox();
			this.VideoLoopCheck = new System.Windows.Forms.CheckBox();
			this.VideoFreezeFirstFrameCheck = new System.Windows.Forms.CheckBox();
			this.VideoGlueCheck = new System.Windows.Forms.CheckBox();
			this.SheetTab = new System.Windows.Forms.TabPage();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.StaffLineThicknessBox = new System.Windows.Forms.NumericUpDown();
			this.StaffSurfacePositionBox = new System.Windows.Forms.NumericUpDown();
			this.StaffSurfaceWidthBox = new System.Windows.Forms.NumericUpDown();
			this.label32 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.StaffClefCombo = new System.Windows.Forms.ComboBox();
			this.StaffLineSpacingBox = new System.Windows.Forms.NumericUpDown();
			this.StaffLineColorBtn = new System.Windows.Forms.Button();
			this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
			this.StaffVisualizerConfigCheck = new System.Windows.Forms.CheckBox();
			this.StaffGenerateCheck = new System.Windows.Forms.CheckBox();
			this.SheetConfigInfoLabel = new System.Windows.Forms.Label();
			this.HelperTab = new System.Windows.Forms.TabPage();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.SelectWhichEachGroupBox = new System.Windows.Forms.NumericUpDown();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.SelectOneEveryFewBox = new System.Windows.Forms.NumericUpDown();
			this.QuickSelectIntervalBtn = new System.Windows.Forms.Button();
			this.label26 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.StaffLineColorDialog = new System.Windows.Forms.ColorDialog();
			this.Balloon = new System.Windows.Forms.ToolTip(this.components);
			this.StaffRelativeValueCheck = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.Tabs.SuspendLayout();
			this.SourceTab.SuspendLayout();
			this.MidiConfigGroup.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MidiCustomBpmBox)).BeginInit();
			this.SourceConfigGroup.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.flowLayoutPanel9.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.AudioTab.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.VideoTab.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.SheetTab.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineThicknessBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfacePositionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfaceWidthBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineSpacingBox)).BeginInit();
			this.flowLayoutPanel8.SuspendLayout();
			this.HelperTab.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tableLayoutPanel11.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.OkBtn, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.CancelBtn, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.AboutBtn, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.UserHelpLink, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 580);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 2);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 42);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(338, 9);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 27);
			this.OkBtn.TabIndex = 0;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(421, 9);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 27);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// AboutBtn
			// 
			this.AboutBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AboutBtn.Location = new System.Drawing.Point(255, 9);
			this.AboutBtn.Margin = new System.Windows.Forms.Padding(4);
			this.AboutBtn.Name = "AboutBtn";
			this.AboutBtn.Size = new System.Drawing.Size(75, 27);
			this.AboutBtn.TabIndex = 2;
			this.AboutBtn.Text = "关于(&A)";
			this.AboutBtn.UseVisualStyleBackColor = true;
			this.AboutBtn.Click += new System.EventHandler(this.AboutBtn_Click);
			// 
			// UserHelpLink
			// 
			this.UserHelpLink.AutoSize = true;
			this.UserHelpLink.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UserHelpLink.Location = new System.Drawing.Point(183, 5);
			this.UserHelpLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.UserHelpLink.Name = "UserHelpLink";
			this.UserHelpLink.Size = new System.Drawing.Size(64, 35);
			this.UserHelpLink.TabIndex = 3;
			this.UserHelpLink.TabStop = true;
			this.UserHelpLink.Text = "使用说明...";
			this.UserHelpLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.UserHelpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UserHelpLink_LinkClicked);
			// 
			// Tabs
			// 
			this.Tabs.Controls.Add(this.SourceTab);
			this.Tabs.Controls.Add(this.AudioTab);
			this.Tabs.Controls.Add(this.VideoTab);
			this.Tabs.Controls.Add(this.SheetTab);
			this.Tabs.Controls.Add(this.HelperTab);
			this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tabs.Location = new System.Drawing.Point(9, 10);
			this.Tabs.Margin = new System.Windows.Forms.Padding(4);
			this.Tabs.Multiline = true;
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(500, 570);
			this.Tabs.TabIndex = 1;
			// 
			// SourceTab
			// 
			this.SourceTab.Controls.Add(this.WarningInfoLabel);
			this.SourceTab.Controls.Add(this.MidiConfigGroup);
			this.SourceTab.Controls.Add(this.SourceConfigGroup);
			this.SourceTab.Location = new System.Drawing.Point(4, 24);
			this.SourceTab.Name = "SourceTab";
			this.SourceTab.Padding = new System.Windows.Forms.Padding(5);
			this.SourceTab.Size = new System.Drawing.Size(492, 542);
			this.SourceTab.TabIndex = 0;
			this.SourceTab.Text = "媒体";
			this.SourceTab.UseVisualStyleBackColor = true;
			// 
			// WarningInfoLabel
			// 
			this.WarningInfoLabel.AutoSize = true;
			this.WarningInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.WarningInfoLabel.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
			this.WarningInfoLabel.ForeColor = System.Drawing.Color.Red;
			this.WarningInfoLabel.Location = new System.Drawing.Point(5, 417);
			this.WarningInfoLabel.Name = "WarningInfoLabel";
			this.WarningInfoLabel.Padding = new System.Windows.Forms.Padding(3);
			this.WarningInfoLabel.Size = new System.Drawing.Size(6, 26);
			this.WarningInfoLabel.TabIndex = 3;
			// 
			// MidiConfigGroup
			// 
			this.MidiConfigGroup.AutoSize = true;
			this.MidiConfigGroup.Controls.Add(this.tableLayoutPanel5);
			this.MidiConfigGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.MidiConfigGroup.Location = new System.Drawing.Point(5, 165);
			this.MidiConfigGroup.Name = "MidiConfigGroup";
			this.MidiConfigGroup.Padding = new System.Windows.Forms.Padding(5);
			this.MidiConfigGroup.Size = new System.Drawing.Size(482, 252);
			this.MidiConfigGroup.TabIndex = 2;
			this.MidiConfigGroup.TabStop = false;
			this.MidiConfigGroup.Text = "MIDI 设置";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 1;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel3, 0, 5);
			this.tableLayoutPanel5.Controls.Add(this.label7, 0, 6);
			this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel2, 0, 4);
			this.tableLayoutPanel5.Controls.Add(this.label5, 0, 2);
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.MidiChannelCombo, 0, 3);
			this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel4, 0, 7);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 8;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(472, 226);
			this.tableLayoutPanel5.TabIndex = 1;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.Controls.Add(this.label8);
			this.flowLayoutPanel3.Controls.Add(this.MidiStartSecondBox);
			this.flowLayoutPanel3.Controls.Add(this.label9);
			this.flowLayoutPanel3.Controls.Add(this.MidiEndSecondBox);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 135);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(466, 29);
			this.flowLayoutPanel3.TabIndex = 9;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label8.Location = new System.Drawing.Point(0, 0);
			this.label8.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(55, 29);
			this.label8.TabIndex = 0;
			this.label8.Text = "起始秒数";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiStartSecondBox
			// 
			this.MidiStartSecondBox.DoubleValue = 0D;
			this.MidiStartSecondBox.Location = new System.Drawing.Point(61, 3);
			this.MidiStartSecondBox.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
			this.MidiStartSecondBox.Name = "MidiStartSecondBox";
			this.MidiStartSecondBox.Size = new System.Drawing.Size(85, 23);
			this.MidiStartSecondBox.TabIndex = 3;
			this.Balloon.SetToolTip(this.MidiStartSecondBox, "用于截取 MIDI 音乐的一部分。\r\n单位：秒。");
			this.MidiStartSecondBox.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label9.Location = new System.Drawing.Point(161, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(55, 29);
			this.label9.TabIndex = 2;
			this.label9.Text = "终止秒数";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiEndSecondBox
			// 
			this.MidiEndSecondBox.DoubleValue = 0D;
			this.MidiEndSecondBox.Location = new System.Drawing.Point(222, 3);
			this.MidiEndSecondBox.Name = "MidiEndSecondBox";
			this.MidiEndSecondBox.Size = new System.Drawing.Size(85, 23);
			this.MidiEndSecondBox.TabIndex = 4;
			this.Balloon.SetToolTip(this.MidiEndSecondBox, "此处填写需要读取 MIDI 文件的时间长度。\r\n注意如果填写的值过小，将截去多余时间部分的音符。\r\n如果此处填写的值比起始秒数小或相等，则始终表示持续到整个音乐时" +
		"长末尾。\r\n单位：秒。");
			this.MidiEndSecondBox.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.Location = new System.Drawing.Point(3, 176);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(466, 15);
			this.label7.TabIndex = 7;
			this.label7.Text = "设定 BPM 速度为";
			this.label7.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.Controls.Add(this.label6);
			this.flowLayoutPanel2.Controls.Add(this.MidiBeatCombo);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 100);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(466, 29);
			this.flowLayoutPanel2.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(0, 0);
			this.label6.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 29);
			this.label6.TabIndex = 0;
			this.label6.Text = "拍子　　";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiBeatCombo
			// 
			this.MidiBeatCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MidiBeatCombo.FormattingEnabled = true;
			this.MidiBeatCombo.Items.AddRange(new object[] {
			"2∕4",
			"3∕4",
			"4∕4",
			"5∕4",
			"6∕4",
			"7∕4",
			"8∕4"});
			this.MidiBeatCombo.Location = new System.Drawing.Point(61, 3);
			this.MidiBeatCombo.Name = "MidiBeatCombo";
			this.MidiBeatCombo.Size = new System.Drawing.Size(85, 23);
			this.MidiBeatCombo.TabIndex = 1;
			this.Balloon.SetToolTip(this.MidiBeatCombo, "目前仅用于五线谱的分页功能。\r\n暂时无法通过 MIDI 文件自动推测。");
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Location = new System.Drawing.Point(3, 53);
			this.label5.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(466, 15);
			this.label5.TabIndex = 3;
			this.label5.Text = "使用 MIDI 轨道";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.ChooseMidiBtn, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.ChooseMidiText, 0, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 15);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 1;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(472, 29);
			this.tableLayoutPanel6.TabIndex = 2;
			// 
			// ChooseMidiBtn
			// 
			this.ChooseMidiBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChooseMidiBtn.Location = new System.Drawing.Point(394, 3);
			this.ChooseMidiBtn.Name = "ChooseMidiBtn";
			this.ChooseMidiBtn.Size = new System.Drawing.Size(75, 23);
			this.ChooseMidiBtn.TabIndex = 3;
			this.ChooseMidiBtn.Text = "浏览...";
			this.ChooseMidiBtn.UseVisualStyleBackColor = true;
			// 
			// ChooseMidiText
			// 
			this.ChooseMidiText.Dock = System.Windows.Forms.DockStyle.Top;
			this.ChooseMidiText.Enabled = false;
			this.ChooseMidiText.Location = new System.Drawing.Point(3, 3);
			this.ChooseMidiText.Name = "ChooseMidiText";
			this.ChooseMidiText.ReadOnly = true;
			this.ChooseMidiText.Size = new System.Drawing.Size(385, 23);
			this.ChooseMidiText.TabIndex = 2;
			this.ChooseMidiText.Text = "<未选择 MIDI 文件>";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(466, 15);
			this.label4.TabIndex = 0;
			this.label4.Text = "选择 MIDI 文件";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// MidiChannelCombo
			// 
			this.MidiChannelCombo.Dock = System.Windows.Forms.DockStyle.Top;
			this.MidiChannelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MidiChannelCombo.FormattingEnabled = true;
			this.MidiChannelCombo.Location = new System.Drawing.Point(3, 71);
			this.MidiChannelCombo.Name = "MidiChannelCombo";
			this.MidiChannelCombo.Size = new System.Drawing.Size(466, 23);
			this.MidiChannelCombo.TabIndex = 4;
			// 
			// flowLayoutPanel4
			// 
			this.flowLayoutPanel4.AutoSize = true;
			this.flowLayoutPanel4.Controls.Add(this.MidiMidiBpmCheck);
			this.flowLayoutPanel4.Controls.Add(this.MidiProjectBpmCheck);
			this.flowLayoutPanel4.Controls.Add(this.MidiCustomBpmCheck);
			this.flowLayoutPanel4.Controls.Add(this.MidiCustomBpmBox);
			this.flowLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 194);
			this.flowLayoutPanel4.Name = "flowLayoutPanel4";
			this.flowLayoutPanel4.Size = new System.Drawing.Size(466, 29);
			this.flowLayoutPanel4.TabIndex = 8;
			// 
			// MidiMidiBpmCheck
			// 
			this.MidiMidiBpmCheck.AutoSize = true;
			this.MidiMidiBpmCheck.Checked = true;
			this.MidiMidiBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiMidiBpmCheck.Enabled = false;
			this.MidiMidiBpmCheck.Location = new System.Drawing.Point(3, 3);
			this.MidiMidiBpmCheck.Name = "MidiMidiBpmCheck";
			this.MidiMidiBpmCheck.Size = new System.Drawing.Size(77, 23);
			this.MidiMidiBpmCheck.TabIndex = 0;
			this.MidiMidiBpmCheck.TabStop = true;
			this.MidiMidiBpmCheck.Text = "MIDI 速度";
			this.MidiMidiBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiProjectBpmCheck
			// 
			this.MidiProjectBpmCheck.AutoSize = true;
			this.MidiProjectBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiProjectBpmCheck.Enabled = false;
			this.MidiProjectBpmCheck.Location = new System.Drawing.Point(86, 3);
			this.MidiProjectBpmCheck.Name = "MidiProjectBpmCheck";
			this.MidiProjectBpmCheck.Size = new System.Drawing.Size(73, 23);
			this.MidiProjectBpmCheck.TabIndex = 1;
			this.MidiProjectBpmCheck.Text = "项目速度";
			this.MidiProjectBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiCustomBpmCheck
			// 
			this.MidiCustomBpmCheck.AutoSize = true;
			this.MidiCustomBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiCustomBpmCheck.Enabled = false;
			this.MidiCustomBpmCheck.Location = new System.Drawing.Point(165, 3);
			this.MidiCustomBpmCheck.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
			this.MidiCustomBpmCheck.Name = "MidiCustomBpmCheck";
			this.MidiCustomBpmCheck.Size = new System.Drawing.Size(61, 23);
			this.MidiCustomBpmCheck.TabIndex = 2;
			this.MidiCustomBpmCheck.Text = "自定义";
			this.MidiCustomBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiCustomBpmBox
			// 
			this.MidiCustomBpmBox.DecimalPlaces = 3;
			this.MidiCustomBpmBox.Enabled = false;
			this.MidiCustomBpmBox.Location = new System.Drawing.Point(226, 3);
			this.MidiCustomBpmBox.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.MidiCustomBpmBox.Maximum = new decimal(new int[] {
			1000,
			0,
			0,
			0});
			this.MidiCustomBpmBox.Minimum = new decimal(new int[] {
			30,
			0,
			0,
			0});
			this.MidiCustomBpmBox.Name = "MidiCustomBpmBox";
			this.MidiCustomBpmBox.Size = new System.Drawing.Size(88, 23);
			this.MidiCustomBpmBox.TabIndex = 3;
			this.MidiCustomBpmBox.Value = new decimal(new int[] {
			120,
			0,
			0,
			0});
			// 
			// SourceConfigGroup
			// 
			this.SourceConfigGroup.AutoSize = true;
			this.SourceConfigGroup.Controls.Add(this.tableLayoutPanel3);
			this.SourceConfigGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.SourceConfigGroup.Location = new System.Drawing.Point(5, 5);
			this.SourceConfigGroup.Name = "SourceConfigGroup";
			this.SourceConfigGroup.Padding = new System.Windows.Forms.Padding(5);
			this.SourceConfigGroup.Size = new System.Drawing.Size(482, 160);
			this.SourceConfigGroup.TabIndex = 1;
			this.SourceConfigGroup.TabStop = false;
			this.SourceConfigGroup.Text = "素材设置";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel9, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.label36, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(472, 134);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// flowLayoutPanel9
			// 
			this.flowLayoutPanel9.AutoSize = true;
			this.flowLayoutPanel9.Controls.Add(this.GenerateAtBeginRadio);
			this.flowLayoutPanel9.Controls.Add(this.GenerateAtCursorRadio);
			this.flowLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel9.Location = new System.Drawing.Point(3, 106);
			this.flowLayoutPanel9.Name = "flowLayoutPanel9";
			this.flowLayoutPanel9.Size = new System.Drawing.Size(466, 25);
			this.flowLayoutPanel9.TabIndex = 9;
			// 
			// GenerateAtBeginRadio
			// 
			this.GenerateAtBeginRadio.AutoSize = true;
			this.GenerateAtBeginRadio.Checked = true;
			this.GenerateAtBeginRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtBeginRadio.Location = new System.Drawing.Point(3, 3);
			this.GenerateAtBeginRadio.Name = "GenerateAtBeginRadio";
			this.GenerateAtBeginRadio.Size = new System.Drawing.Size(85, 19);
			this.GenerateAtBeginRadio.TabIndex = 0;
			this.GenerateAtBeginRadio.TabStop = true;
			this.GenerateAtBeginRadio.Text = "项目开始处";
			this.GenerateAtBeginRadio.UseVisualStyleBackColor = true;
			// 
			// GenerateAtCursorRadio
			// 
			this.GenerateAtCursorRadio.AutoSize = true;
			this.GenerateAtCursorRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtCursorRadio.Location = new System.Drawing.Point(94, 3);
			this.GenerateAtCursorRadio.Name = "GenerateAtCursorRadio";
			this.GenerateAtCursorRadio.Size = new System.Drawing.Size(61, 19);
			this.GenerateAtCursorRadio.TabIndex = 1;
			this.GenerateAtCursorRadio.Text = "光标处";
			this.GenerateAtCursorRadio.UseVisualStyleBackColor = true;
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label36.Location = new System.Drawing.Point(3, 88);
			this.label36.Margin = new System.Windows.Forms.Padding(3, 9, 3, 0);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(466, 15);
			this.label36.TabIndex = 8;
			this.label36.Text = "设定生成开始位置";
			this.label36.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(466, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "选择媒体素材";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.ChooseSourceCombo, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.ChooseSourceBtn, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 15);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(472, 29);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// ChooseSourceCombo
			// 
			this.ChooseSourceCombo.Dock = System.Windows.Forms.DockStyle.Top;
			this.ChooseSourceCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ChooseSourceCombo.FormattingEnabled = true;
			this.ChooseSourceCombo.Items.AddRange(new object[] {
			"选中的媒体文件",
			"选中的轨道素材"});
			this.ChooseSourceCombo.Location = new System.Drawing.Point(3, 3);
			this.ChooseSourceCombo.Name = "ChooseSourceCombo";
			this.ChooseSourceCombo.Size = new System.Drawing.Size(385, 23);
			this.ChooseSourceCombo.TabIndex = 0;
			this.ChooseSourceCombo.SelectedIndexChanged += new System.EventHandler(this.ChooseSourceCombo_SelectedIndexChanged);
			// 
			// ChooseSourceBtn
			// 
			this.ChooseSourceBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChooseSourceBtn.Location = new System.Drawing.Point(394, 3);
			this.ChooseSourceBtn.Name = "ChooseSourceBtn";
			this.ChooseSourceBtn.Size = new System.Drawing.Size(75, 23);
			this.ChooseSourceBtn.TabIndex = 1;
			this.ChooseSourceBtn.Text = "浏览...";
			this.ChooseSourceBtn.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.label2);
			this.flowLayoutPanel1.Controls.Add(this.SourceStartTimeText);
			this.flowLayoutPanel1.Controls.Add(this.label3);
			this.flowLayoutPanel1.Controls.Add(this.SourceEndTimeText);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 47);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(466, 29);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 29);
			this.label2.TabIndex = 0;
			this.label2.Text = "入点秒数";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SourceStartTimeText
			// 
			this.SourceStartTimeText.DoubleValue = 0D;
			this.SourceStartTimeText.Location = new System.Drawing.Point(61, 3);
			this.SourceStartTimeText.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
			this.SourceStartTimeText.Name = "SourceStartTimeText";
			this.SourceStartTimeText.Size = new System.Drawing.Size(85, 23);
			this.SourceStartTimeText.TabIndex = 4;
			this.Balloon.SetToolTip(this.SourceStartTimeText, "此处填写媒体素材裁剪的开始时间。\r\n单位：秒。");
			this.SourceStartTimeText.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(161, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(55, 29);
			this.label3.TabIndex = 2;
			this.label3.Text = "出点秒数";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SourceEndTimeText
			// 
			this.SourceEndTimeText.DoubleValue = 0D;
			this.SourceEndTimeText.Location = new System.Drawing.Point(222, 3);
			this.SourceEndTimeText.Name = "SourceEndTimeText";
			this.SourceEndTimeText.Size = new System.Drawing.Size(85, 23);
			this.SourceEndTimeText.TabIndex = 5;
			this.Balloon.SetToolTip(this.SourceEndTimeText, "注意如果此处填写的数值比入点秒数小或相等，则始终表示持续到素材时间末尾。\r\n单位：秒。");
			this.SourceEndTimeText.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// AudioTab
			// 
			this.AudioTab.Controls.Add(this.groupBox2);
			this.AudioTab.Controls.Add(this.groupBox1);
			this.AudioTab.Controls.Add(this.flowLayoutPanel5);
			this.AudioTab.Location = new System.Drawing.Point(4, 24);
			this.AudioTab.Name = "AudioTab";
			this.AudioTab.Padding = new System.Windows.Forms.Padding(5);
			this.AudioTab.Size = new System.Drawing.Size(492, 542);
			this.AudioTab.TabIndex = 1;
			this.AudioTab.Text = "音频";
			this.AudioTab.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.Controls.Add(this.tableLayoutPanel2);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(5, 120);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox2.Size = new System.Drawing.Size(482, 104);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "参数";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeOutCurveCombo, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.label12, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label13, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeInBox, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeOutBox, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeInCurveCombo, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(472, 78);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// AudioFadeOutCurveCombo
			// 
			this.AudioFadeOutCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeOutCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioFadeOutCurveCombo.FormattingEnabled = true;
			this.AudioFadeOutCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.AudioFadeOutCurveCombo.Location = new System.Drawing.Point(404, 42);
			this.AudioFadeOutCurveCombo.Name = "AudioFadeOutCurveCombo";
			this.AudioFadeOutCurveCombo.Size = new System.Drawing.Size(65, 23);
			this.AudioFadeOutCurveCombo.TabIndex = 5;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label12.Location = new System.Drawing.Point(3, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(55, 39);
			this.label12.TabIndex = 0;
			this.label12.Text = "渐入　　";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label13.Location = new System.Drawing.Point(3, 39);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(55, 39);
			this.label13.TabIndex = 1;
			this.label13.Text = "渐出　　";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioFadeInBox
			// 
			this.AudioFadeInBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeInBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.AudioFadeInBox.Location = new System.Drawing.Point(65, 4);
			this.AudioFadeInBox.Margin = new System.Windows.Forms.Padding(4);
			this.AudioFadeInBox.Name = "AudioFadeInBox";
			this.AudioFadeInBox.NumericUpDownWidth = 65;
			this.AudioFadeInBox.Size = new System.Drawing.Size(332, 31);
			this.AudioFadeInBox.TabIndex = 2;
			// 
			// AudioFadeOutBox
			// 
			this.AudioFadeOutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeOutBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.AudioFadeOutBox.Location = new System.Drawing.Point(65, 43);
			this.AudioFadeOutBox.Margin = new System.Windows.Forms.Padding(4);
			this.AudioFadeOutBox.Name = "AudioFadeOutBox";
			this.AudioFadeOutBox.NumericUpDownWidth = 65;
			this.AudioFadeOutBox.Size = new System.Drawing.Size(332, 31);
			this.AudioFadeOutBox.TabIndex = 4;
			this.AudioFadeOutBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// AudioFadeInCurveCombo
			// 
			this.AudioFadeInCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeInCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioFadeInCurveCombo.FormattingEnabled = true;
			this.AudioFadeInCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.AudioFadeInCurveCombo.Location = new System.Drawing.Point(404, 3);
			this.AudioFadeInCurveCombo.Name = "AudioFadeInCurveCombo";
			this.AudioFadeInCurveCombo.Size = new System.Drawing.Size(65, 23);
			this.AudioFadeInCurveCombo.TabIndex = 3;
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.tableLayoutPanel7);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(5, 36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(482, 84);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "调音";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.ColumnCount = 2;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel7.Controls.Add(this.label10, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.label11, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.AudioTuneMethodCombo, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.flowLayoutPanel6, 1, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(472, 58);
			this.tableLayoutPanel7.TabIndex = 1;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label10.Location = new System.Drawing.Point(3, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(55, 29);
			this.label10.TabIndex = 0;
			this.label10.Text = "调音方式";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label11.Location = new System.Drawing.Point(3, 29);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(55, 29);
			this.label11.TabIndex = 1;
			this.label11.Text = "原始音高";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioTuneMethodCombo
			// 
			this.AudioTuneMethodCombo.Dock = System.Windows.Forms.DockStyle.Top;
			this.AudioTuneMethodCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioTuneMethodCombo.FormattingEnabled = true;
			this.AudioTuneMethodCombo.Items.AddRange(new object[] {
			"不调音",
			"移调效果插件",
			"移调效果插件（锁定伸缩与音调）",
			"弹性音调更改",
			"弹性音调更改（锁定伸缩与音调）"});
			this.AudioTuneMethodCombo.Location = new System.Drawing.Point(64, 3);
			this.AudioTuneMethodCombo.Name = "AudioTuneMethodCombo";
			this.AudioTuneMethodCombo.Size = new System.Drawing.Size(405, 23);
			this.AudioTuneMethodCombo.TabIndex = 2;
			this.Balloon.SetToolTip(this.AudioTuneMethodCombo, "“移调效果插件”表示使用“音频 FX”中的“移调”效果插件改变音调，需要配置预设。\r\n“弹性音调更改”表示使用“Élastique”拉伸方式改变音调，也就是键盘上" +
		" +、- 键直接改变音调，\r\n有音高范围限制。\r\n“锁定伸缩与音调”表示采用重采样方式，随着速度变化而改变音高。如果使用的是“弹性音调\r\n更改（锁定伸缩与音调）" +
		"”，那么将会禁用拉伸音频功能。");
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.AudioMainKeyCombo);
			this.flowLayoutPanel6.Controls.Add(this.AudioMainOctaveCombo);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(61, 29);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(411, 29);
			this.flowLayoutPanel6.TabIndex = 3;
			// 
			// AudioMainKeyCombo
			// 
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
			this.AudioMainKeyCombo.Location = new System.Drawing.Point(3, 3);
			this.AudioMainKeyCombo.Name = "AudioMainKeyCombo";
			this.AudioMainKeyCombo.Size = new System.Drawing.Size(60, 23);
			this.AudioMainKeyCombo.TabIndex = 3;
			// 
			// AudioMainOctaveCombo
			// 
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
			this.AudioMainOctaveCombo.Location = new System.Drawing.Point(69, 3);
			this.AudioMainOctaveCombo.Name = "AudioMainOctaveCombo";
			this.AudioMainOctaveCombo.Size = new System.Drawing.Size(60, 23);
			this.AudioMainOctaveCombo.TabIndex = 4;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.Controls.Add(this.AudioConfigCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioScratchCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioLoopCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioNormalizeCheck);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(5, 5);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.flowLayoutPanel5.Size = new System.Drawing.Size(482, 31);
			this.flowLayoutPanel5.TabIndex = 0;
			// 
			// AudioConfigCheck
			// 
			this.AudioConfigCheck.AutoSize = true;
			this.AudioConfigCheck.Checked = true;
			this.AudioConfigCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AudioConfigCheck.Location = new System.Drawing.Point(3, 6);
			this.AudioConfigCheck.Name = "AudioConfigCheck";
			this.AudioConfigCheck.Size = new System.Drawing.Size(74, 19);
			this.AudioConfigCheck.TabIndex = 0;
			this.AudioConfigCheck.Text = "生成音频";
			this.AudioConfigCheck.UseVisualStyleBackColor = true;
			// 
			// AudioScratchCheck
			// 
			this.AudioScratchCheck.AutoSize = true;
			this.AudioScratchCheck.Location = new System.Drawing.Point(83, 6);
			this.AudioScratchCheck.Name = "AudioScratchCheck";
			this.AudioScratchCheck.Size = new System.Drawing.Size(74, 19);
			this.AudioScratchCheck.TabIndex = 1;
			this.AudioScratchCheck.Text = "拉伸音频";
			this.AudioScratchCheck.UseVisualStyleBackColor = true;
			// 
			// AudioLoopCheck
			// 
			this.AudioLoopCheck.AutoSize = true;
			this.AudioLoopCheck.Location = new System.Drawing.Point(163, 6);
			this.AudioLoopCheck.Name = "AudioLoopCheck";
			this.AudioLoopCheck.Size = new System.Drawing.Size(74, 19);
			this.AudioLoopCheck.TabIndex = 2;
			this.AudioLoopCheck.Text = "循环音频";
			this.AudioLoopCheck.UseVisualStyleBackColor = true;
			// 
			// AudioNormalizeCheck
			// 
			this.AudioNormalizeCheck.AutoSize = true;
			this.AudioNormalizeCheck.Location = new System.Drawing.Point(243, 6);
			this.AudioNormalizeCheck.Name = "AudioNormalizeCheck";
			this.AudioNormalizeCheck.Size = new System.Drawing.Size(86, 19);
			this.AudioNormalizeCheck.TabIndex = 3;
			this.AudioNormalizeCheck.Text = "规范化音量";
			this.AudioNormalizeCheck.UseVisualStyleBackColor = true;
			// 
			// VideoTab
			// 
			this.VideoTab.Controls.Add(this.groupBox4);
			this.VideoTab.Controls.Add(this.groupBox3);
			this.VideoTab.Controls.Add(this.flowLayoutPanel7);
			this.VideoTab.Location = new System.Drawing.Point(4, 24);
			this.VideoTab.Name = "VideoTab";
			this.VideoTab.Padding = new System.Windows.Forms.Padding(5);
			this.VideoTab.Size = new System.Drawing.Size(492, 542);
			this.VideoTab.TabIndex = 2;
			this.VideoTab.Text = "视频";
			this.VideoTab.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.AutoSize = true;
			this.groupBox4.Controls.Add(this.tableLayoutPanel9);
			this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox4.Location = new System.Drawing.Point(5, 120);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox4.Size = new System.Drawing.Size(482, 416);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "参数";
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoScroll = true;
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.ColumnCount = 3;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.VideoEndVerticalTransBox, 1, 9);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartVerticalTransBox, 1, 8);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndHorizontalTransBox, 1, 7);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHorizontalTransBox, 1, 6);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndRotationBox, 1, 5);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartRotationBox, 1, 4);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndSizeBox, 1, 3);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSizeCurveCombo, 2, 2);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSizeBox, 1, 2);
			this.tableLayoutPanel9.Controls.Add(this.label25, 0, 9);
			this.tableLayoutPanel9.Controls.Add(this.label24, 0, 6);
			this.tableLayoutPanel9.Controls.Add(this.label23, 0, 8);
			this.tableLayoutPanel9.Controls.Add(this.label22, 0, 7);
			this.tableLayoutPanel9.Controls.Add(this.label21, 0, 5);
			this.tableLayoutPanel9.Controls.Add(this.label20, 0, 4);
			this.tableLayoutPanel9.Controls.Add(this.label19, 0, 3);
			this.tableLayoutPanel9.Controls.Add(this.label18, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeOutCurveCombo, 2, 1);
			this.tableLayoutPanel9.Controls.Add(this.label16, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.label17, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeInBox, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeOutBox, 1, 1);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeInCurveCombo, 2, 0);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 10;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(472, 390);
			this.tableLayoutPanel9.TabIndex = 0;
			// 
			// VideoEndVerticalTransBox
			// 
			this.VideoEndVerticalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndVerticalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndVerticalTransBox.Location = new System.Drawing.Point(65, 355);
			this.VideoEndVerticalTransBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoEndVerticalTransBox.Minimum = -100;
			this.VideoEndVerticalTransBox.Name = "VideoEndVerticalTransBox";
			this.VideoEndVerticalTransBox.NumericUpDownWidth = 65;
			this.VideoEndVerticalTransBox.Size = new System.Drawing.Size(332, 31);
			this.VideoEndVerticalTransBox.TabIndex = 22;
			this.VideoEndVerticalTransBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoStartVerticalTransBox
			// 
			this.VideoStartVerticalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartVerticalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartVerticalTransBox.Location = new System.Drawing.Point(65, 316);
			this.VideoStartVerticalTransBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoStartVerticalTransBox.Minimum = -100;
			this.VideoStartVerticalTransBox.Name = "VideoStartVerticalTransBox";
			this.VideoStartVerticalTransBox.NumericUpDownWidth = 65;
			this.VideoStartVerticalTransBox.Size = new System.Drawing.Size(332, 31);
			this.VideoStartVerticalTransBox.TabIndex = 21;
			// 
			// VideoEndHorizontalTransBox
			// 
			this.VideoEndHorizontalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndHorizontalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndHorizontalTransBox.Location = new System.Drawing.Point(65, 277);
			this.VideoEndHorizontalTransBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoEndHorizontalTransBox.Minimum = -100;
			this.VideoEndHorizontalTransBox.Name = "VideoEndHorizontalTransBox";
			this.VideoEndHorizontalTransBox.NumericUpDownWidth = 65;
			this.VideoEndHorizontalTransBox.Size = new System.Drawing.Size(332, 31);
			this.VideoEndHorizontalTransBox.TabIndex = 20;
			this.VideoEndHorizontalTransBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoStartHorizontalTransBox
			// 
			this.VideoStartHorizontalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHorizontalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartHorizontalTransBox.Location = new System.Drawing.Point(65, 238);
			this.VideoStartHorizontalTransBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoStartHorizontalTransBox.Minimum = -100;
			this.VideoStartHorizontalTransBox.Name = "VideoStartHorizontalTransBox";
			this.VideoStartHorizontalTransBox.NumericUpDownWidth = 65;
			this.VideoStartHorizontalTransBox.Size = new System.Drawing.Size(332, 31);
			this.VideoStartHorizontalTransBox.TabIndex = 19;
			// 
			// VideoEndRotationBox
			// 
			this.VideoEndRotationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndRotationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndRotationBox.Location = new System.Drawing.Point(65, 199);
			this.VideoEndRotationBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoEndRotationBox.Maximum = 360;
			this.VideoEndRotationBox.Minimum = -360;
			this.VideoEndRotationBox.Name = "VideoEndRotationBox";
			this.VideoEndRotationBox.NumericUpDownWidth = 65;
			this.VideoEndRotationBox.Size = new System.Drawing.Size(332, 31);
			this.VideoEndRotationBox.TabIndex = 18;
			this.VideoEndRotationBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoStartRotationBox
			// 
			this.VideoStartRotationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartRotationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartRotationBox.Location = new System.Drawing.Point(65, 160);
			this.VideoStartRotationBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoStartRotationBox.Maximum = 360;
			this.VideoStartRotationBox.Minimum = -360;
			this.VideoStartRotationBox.Name = "VideoStartRotationBox";
			this.VideoStartRotationBox.NumericUpDownWidth = 65;
			this.VideoStartRotationBox.Size = new System.Drawing.Size(332, 31);
			this.VideoStartRotationBox.TabIndex = 17;
			// 
			// VideoEndSizeBox
			// 
			this.VideoEndSizeBox.DefaultValue = 100;
			this.VideoEndSizeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndSizeBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndSizeBox.Location = new System.Drawing.Point(65, 121);
			this.VideoEndSizeBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoEndSizeBox.Maximum = 200;
			this.VideoEndSizeBox.Name = "VideoEndSizeBox";
			this.VideoEndSizeBox.NumericUpDownWidth = 65;
			this.VideoEndSizeBox.Size = new System.Drawing.Size(332, 31);
			this.VideoEndSizeBox.TabIndex = 16;
			this.VideoEndSizeBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.VideoEndSizeBox.Value = 100;
			// 
			// VideoStartSizeCurveCombo
			// 
			this.VideoStartSizeCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSizeCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoStartSizeCurveCombo.FormattingEnabled = true;
			this.VideoStartSizeCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoStartSizeCurveCombo.Location = new System.Drawing.Point(404, 81);
			this.VideoStartSizeCurveCombo.Name = "VideoStartSizeCurveCombo";
			this.VideoStartSizeCurveCombo.Size = new System.Drawing.Size(65, 23);
			this.VideoStartSizeCurveCombo.TabIndex = 15;
			// 
			// VideoStartSizeBox
			// 
			this.VideoStartSizeBox.DefaultValue = 100;
			this.VideoStartSizeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSizeBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartSizeBox.Location = new System.Drawing.Point(65, 82);
			this.VideoStartSizeBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoStartSizeBox.Maximum = 200;
			this.VideoStartSizeBox.Name = "VideoStartSizeBox";
			this.VideoStartSizeBox.NumericUpDownWidth = 65;
			this.VideoStartSizeBox.Size = new System.Drawing.Size(332, 31);
			this.VideoStartSizeBox.TabIndex = 14;
			this.VideoStartSizeBox.Value = 100;
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label25.Location = new System.Drawing.Point(3, 351);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(55, 39);
			this.label25.TabIndex = 13;
			this.label25.Text = "终止直移";
			this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label24.Location = new System.Drawing.Point(3, 234);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(55, 39);
			this.label24.TabIndex = 12;
			this.label24.Text = "起始平移";
			this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label23.Location = new System.Drawing.Point(3, 312);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(55, 39);
			this.label23.TabIndex = 11;
			this.label23.Text = "起始直移";
			this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label22.Location = new System.Drawing.Point(3, 273);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(55, 39);
			this.label22.TabIndex = 10;
			this.label22.Text = "终止平移";
			this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label21.Location = new System.Drawing.Point(3, 195);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(55, 39);
			this.label21.TabIndex = 9;
			this.label21.Text = "终止旋转";
			this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label20
			// 
			this.label20.AutoSize = true;
			this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label20.Location = new System.Drawing.Point(3, 156);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(55, 39);
			this.label20.TabIndex = 8;
			this.label20.Text = "起始旋转";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label19.Location = new System.Drawing.Point(3, 117);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(55, 39);
			this.label19.TabIndex = 7;
			this.label19.Text = "终止尺寸";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label18.Location = new System.Drawing.Point(3, 78);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(55, 39);
			this.label18.TabIndex = 6;
			this.label18.Text = "起始尺寸";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoFadeOutCurveCombo
			// 
			this.VideoFadeOutCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeOutCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoFadeOutCurveCombo.FormattingEnabled = true;
			this.VideoFadeOutCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoFadeOutCurveCombo.Location = new System.Drawing.Point(404, 42);
			this.VideoFadeOutCurveCombo.Name = "VideoFadeOutCurveCombo";
			this.VideoFadeOutCurveCombo.Size = new System.Drawing.Size(65, 23);
			this.VideoFadeOutCurveCombo.TabIndex = 5;
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label16.Location = new System.Drawing.Point(3, 0);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(55, 39);
			this.label16.TabIndex = 0;
			this.label16.Text = "渐入　　";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label17.Location = new System.Drawing.Point(3, 39);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(55, 39);
			this.label17.TabIndex = 1;
			this.label17.Text = "渐出　　";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoFadeInBox
			// 
			this.VideoFadeInBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeInBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoFadeInBox.Location = new System.Drawing.Point(65, 4);
			this.VideoFadeInBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoFadeInBox.Name = "VideoFadeInBox";
			this.VideoFadeInBox.NumericUpDownWidth = 65;
			this.VideoFadeInBox.Size = new System.Drawing.Size(332, 31);
			this.VideoFadeInBox.TabIndex = 2;
			// 
			// VideoFadeOutBox
			// 
			this.VideoFadeOutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeOutBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoFadeOutBox.Location = new System.Drawing.Point(65, 43);
			this.VideoFadeOutBox.Margin = new System.Windows.Forms.Padding(4);
			this.VideoFadeOutBox.Name = "VideoFadeOutBox";
			this.VideoFadeOutBox.NumericUpDownWidth = 65;
			this.VideoFadeOutBox.Size = new System.Drawing.Size(332, 31);
			this.VideoFadeOutBox.TabIndex = 4;
			this.VideoFadeOutBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoFadeInCurveCombo
			// 
			this.VideoFadeInCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeInCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoFadeInCurveCombo.FormattingEnabled = true;
			this.VideoFadeInCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoFadeInCurveCombo.Location = new System.Drawing.Point(404, 3);
			this.VideoFadeInCurveCombo.Name = "VideoFadeInCurveCombo";
			this.VideoFadeInCurveCombo.Size = new System.Drawing.Size(65, 23);
			this.VideoFadeInCurveCombo.TabIndex = 3;
			// 
			// groupBox3
			// 
			this.groupBox3.AutoSize = true;
			this.groupBox3.Controls.Add(this.tableLayoutPanel8);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox3.Location = new System.Drawing.Point(5, 36);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox3.Size = new System.Drawing.Size(482, 84);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "效果";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectInitialValueCombo, 1, 1);
			this.tableLayoutPanel8.Controls.Add(this.label14, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.label15, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectCombo, 1, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 2;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(472, 58);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// VideoEffectInitialValueCombo
			// 
			this.VideoEffectInitialValueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoEffectInitialValueCombo.FormattingEnabled = true;
			this.VideoEffectInitialValueCombo.Location = new System.Drawing.Point(64, 32);
			this.VideoEffectInitialValueCombo.Name = "VideoEffectInitialValueCombo";
			this.VideoEffectInitialValueCombo.Size = new System.Drawing.Size(60, 23);
			this.VideoEffectInitialValueCombo.TabIndex = 3;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label14.Location = new System.Drawing.Point(3, 0);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(55, 29);
			this.label14.TabIndex = 0;
			this.label14.Text = "视觉效果";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label15.Location = new System.Drawing.Point(3, 29);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(55, 29);
			this.label15.TabIndex = 1;
			this.label15.Text = "初始值";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEffectCombo
			// 
			this.VideoEffectCombo.Dock = System.Windows.Forms.DockStyle.Top;
			this.VideoEffectCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoEffectCombo.FormattingEnabled = true;
			this.VideoEffectCombo.Items.AddRange(new object[] {
			"无效果",
			"水平翻转",
			"垂直翻转",
			"逆时针翻转",
			"顺时针翻转",
			"逆时针旋转",
			"顺时针旋转",
			"颠倒",
			"水平镜像",
			"垂直镜像",
			"逆时针镜像",
			"顺时针镜像",
			"反转颜色",
			"反转亮度",
			"反转色相",
			"逐步变色（3 步）",
			"逐步变色（4 步）",
			"逐步变色（5 步）",
			"逐步变色（6 步）",
			"逐步变色（7 步）",
			"逐步变色（8 步）",
			"黑白",
			"乒乓效应",
			"永动机"});
			this.VideoEffectCombo.Location = new System.Drawing.Point(64, 3);
			this.VideoEffectCombo.Name = "VideoEffectCombo";
			this.VideoEffectCombo.Size = new System.Drawing.Size(405, 23);
			this.VideoEffectCombo.TabIndex = 2;
			this.VideoEffectCombo.SelectedIndexChanged += new System.EventHandler(this.VideoEffectCombo_SelectedIndexChanged);
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.AutoSize = true;
			this.flowLayoutPanel7.Controls.Add(this.VideoConfigCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoScratchCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoLoopCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoFreezeFirstFrameCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoGlueCheck);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(5, 5);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.flowLayoutPanel7.Size = new System.Drawing.Size(482, 31);
			this.flowLayoutPanel7.TabIndex = 1;
			// 
			// VideoConfigCheck
			// 
			this.VideoConfigCheck.AutoSize = true;
			this.VideoConfigCheck.Checked = true;
			this.VideoConfigCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.VideoConfigCheck.Location = new System.Drawing.Point(3, 6);
			this.VideoConfigCheck.Name = "VideoConfigCheck";
			this.VideoConfigCheck.Size = new System.Drawing.Size(74, 19);
			this.VideoConfigCheck.TabIndex = 0;
			this.VideoConfigCheck.Text = "生成视频";
			this.VideoConfigCheck.UseVisualStyleBackColor = true;
			// 
			// VideoScratchCheck
			// 
			this.VideoScratchCheck.AutoSize = true;
			this.VideoScratchCheck.Location = new System.Drawing.Point(83, 6);
			this.VideoScratchCheck.Name = "VideoScratchCheck";
			this.VideoScratchCheck.Size = new System.Drawing.Size(74, 19);
			this.VideoScratchCheck.TabIndex = 1;
			this.VideoScratchCheck.Text = "拉伸视频";
			this.VideoScratchCheck.UseVisualStyleBackColor = true;
			// 
			// VideoLoopCheck
			// 
			this.VideoLoopCheck.AutoSize = true;
			this.VideoLoopCheck.Checked = true;
			this.VideoLoopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.VideoLoopCheck.Location = new System.Drawing.Point(163, 6);
			this.VideoLoopCheck.Name = "VideoLoopCheck";
			this.VideoLoopCheck.Size = new System.Drawing.Size(74, 19);
			this.VideoLoopCheck.TabIndex = 2;
			this.VideoLoopCheck.Text = "循环视频";
			this.VideoLoopCheck.UseVisualStyleBackColor = true;
			// 
			// VideoFreezeFirstFrameCheck
			// 
			this.VideoFreezeFirstFrameCheck.AutoSize = true;
			this.VideoFreezeFirstFrameCheck.Location = new System.Drawing.Point(243, 6);
			this.VideoFreezeFirstFrameCheck.Name = "VideoFreezeFirstFrameCheck";
			this.VideoFreezeFirstFrameCheck.Size = new System.Drawing.Size(74, 19);
			this.VideoFreezeFirstFrameCheck.TabIndex = 3;
			this.VideoFreezeFirstFrameCheck.Text = "定格首帧";
			this.VideoFreezeFirstFrameCheck.UseVisualStyleBackColor = true;
			// 
			// VideoGlueCheck
			// 
			this.VideoGlueCheck.AutoSize = true;
			this.VideoGlueCheck.Location = new System.Drawing.Point(323, 6);
			this.VideoGlueCheck.Name = "VideoGlueCheck";
			this.VideoGlueCheck.Size = new System.Drawing.Size(74, 19);
			this.VideoGlueCheck.TabIndex = 4;
			this.VideoGlueCheck.Text = "消除间隙";
			this.VideoGlueCheck.UseVisualStyleBackColor = true;
			// 
			// SheetTab
			// 
			this.SheetTab.Controls.Add(this.groupBox5);
			this.SheetTab.Controls.Add(this.flowLayoutPanel8);
			this.SheetTab.Controls.Add(this.SheetConfigInfoLabel);
			this.SheetTab.Location = new System.Drawing.Point(4, 24);
			this.SheetTab.Name = "SheetTab";
			this.SheetTab.Padding = new System.Windows.Forms.Padding(5);
			this.SheetTab.Size = new System.Drawing.Size(492, 542);
			this.SheetTab.TabIndex = 3;
			this.SheetTab.Text = "五线谱";
			this.SheetTab.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.AutoSize = true;
			this.groupBox5.Controls.Add(this.tableLayoutPanel10);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox5.Location = new System.Drawing.Point(5, 76);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox5.Size = new System.Drawing.Size(482, 113);
			this.groupBox5.TabIndex = 3;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "参数";
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.AutoSize = true;
			this.tableLayoutPanel10.ColumnCount = 4;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.Controls.Add(this.StaffLineThicknessBox, 1, 2);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfacePositionBox, 3, 1);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfaceWidthBox, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.label32, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this.label31, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.label30, 2, 2);
			this.tableLayoutPanel10.Controls.Add(this.label29, 2, 1);
			this.tableLayoutPanel10.Controls.Add(this.label27, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.label28, 2, 0);
			this.tableLayoutPanel10.Controls.Add(this.StaffClefCombo, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.StaffLineSpacingBox, 3, 0);
			this.tableLayoutPanel10.Controls.Add(this.StaffLineColorBtn, 3, 2);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 3;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(472, 87);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// StaffLineThicknessBox
			// 
			this.StaffLineThicknessBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineThicknessBox.Enabled = false;
			this.StaffLineThicknessBox.Location = new System.Drawing.Point(64, 61);
			this.StaffLineThicknessBox.Maximum = new decimal(new int[] {
			500,
			0,
			0,
			0});
			this.StaffLineThicknessBox.Minimum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.StaffLineThicknessBox.Name = "StaffLineThicknessBox";
			this.StaffLineThicknessBox.Size = new System.Drawing.Size(169, 23);
			this.StaffLineThicknessBox.TabIndex = 10;
			this.StaffLineThicknessBox.Value = new decimal(new int[] {
			50,
			0,
			0,
			0});
			// 
			// StaffSurfacePositionBox
			// 
			this.StaffSurfacePositionBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfacePositionBox.Enabled = false;
			this.StaffSurfacePositionBox.Location = new System.Drawing.Point(300, 32);
			this.StaffSurfacePositionBox.Maximum = new decimal(new int[] {
			65536,
			0,
			0,
			0});
			this.StaffSurfacePositionBox.Minimum = new decimal(new int[] {
			65536,
			0,
			0,
			-2147483648});
			this.StaffSurfacePositionBox.Name = "StaffSurfacePositionBox";
			this.StaffSurfacePositionBox.Size = new System.Drawing.Size(169, 23);
			this.StaffSurfacePositionBox.TabIndex = 9;
			this.Balloon.SetToolTip(this.StaffSurfacePositionBox, "五线谱中间第三根线到屏幕中心的距离，上正下负。\r\n单位：像素。");
			this.StaffSurfacePositionBox.Value = new decimal(new int[] {
			225,
			0,
			0,
			0});
			// 
			// StaffSurfaceWidthBox
			// 
			this.StaffSurfaceWidthBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfaceWidthBox.Enabled = false;
			this.StaffSurfaceWidthBox.Location = new System.Drawing.Point(64, 32);
			this.StaffSurfaceWidthBox.Maximum = new decimal(new int[] {
			65536,
			0,
			0,
			0});
			this.StaffSurfaceWidthBox.Name = "StaffSurfaceWidthBox";
			this.StaffSurfaceWidthBox.Size = new System.Drawing.Size(169, 23);
			this.StaffSurfaceWidthBox.TabIndex = 8;
			this.Balloon.SetToolTip(this.StaffSurfaceWidthBox, "将在屏幕中间所填的宽度内显示音符，用于左右留白，给左侧的谱号留间距。\r\n单位：像素。");
			this.StaffSurfaceWidthBox.Value = new decimal(new int[] {
			1500,
			0,
			0,
			0});
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label32.Location = new System.Drawing.Point(3, 29);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(55, 29);
			this.label32.TabIndex = 5;
			this.label32.Text = "谱面宽度";
			this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label31.Location = new System.Drawing.Point(3, 58);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(55, 29);
			this.label31.TabIndex = 4;
			this.label31.Text = "谱线粗细";
			this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label30.Location = new System.Drawing.Point(239, 58);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(55, 29);
			this.label30.TabIndex = 3;
			this.label30.Text = "谱线颜色";
			this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label29.Location = new System.Drawing.Point(239, 29);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(55, 29);
			this.label29.TabIndex = 2;
			this.label29.Text = "谱面位置";
			this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label27.Location = new System.Drawing.Point(3, 0);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(55, 29);
			this.label27.TabIndex = 0;
			this.label27.Text = "谱号";
			this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label28.Location = new System.Drawing.Point(239, 0);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(55, 29);
			this.label28.TabIndex = 1;
			this.label28.Text = "谱线间距";
			this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffClefCombo
			// 
			this.StaffClefCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffClefCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.StaffClefCombo.Enabled = false;
			this.StaffClefCombo.FormattingEnabled = true;
			this.StaffClefCombo.Items.AddRange(new object[] {
			"高音",
			"低音"});
			this.StaffClefCombo.Location = new System.Drawing.Point(64, 3);
			this.StaffClefCombo.Name = "StaffClefCombo";
			this.StaffClefCombo.Size = new System.Drawing.Size(169, 23);
			this.StaffClefCombo.TabIndex = 6;
			// 
			// StaffLineSpacingBox
			// 
			this.StaffLineSpacingBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineSpacingBox.Enabled = false;
			this.StaffLineSpacingBox.Location = new System.Drawing.Point(300, 3);
			this.StaffLineSpacingBox.Maximum = new decimal(new int[] {
			65536,
			0,
			0,
			0});
			this.StaffLineSpacingBox.Name = "StaffLineSpacingBox";
			this.StaffLineSpacingBox.Size = new System.Drawing.Size(169, 23);
			this.StaffLineSpacingBox.TabIndex = 7;
			this.Balloon.SetToolTip(this.StaffLineSpacingBox, "五线谱线与线之间的间距。\r\n单位：像素。");
			this.StaffLineSpacingBox.Value = new decimal(new int[] {
			44,
			0,
			0,
			0});
			// 
			// StaffLineColorBtn
			// 
			this.StaffLineColorBtn.BackColor = System.Drawing.Color.Black;
			this.StaffLineColorBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineColorBtn.Enabled = false;
			this.StaffLineColorBtn.ForeColor = System.Drawing.Color.White;
			this.StaffLineColorBtn.Location = new System.Drawing.Point(300, 61);
			this.StaffLineColorBtn.Name = "StaffLineColorBtn";
			this.StaffLineColorBtn.Size = new System.Drawing.Size(169, 23);
			this.StaffLineColorBtn.TabIndex = 11;
			this.StaffLineColorBtn.Text = "#000000";
			this.StaffLineColorBtn.UseVisualStyleBackColor = false;
			this.StaffLineColorBtn.Click += new System.EventHandler(this.StaffLineColorBtn_Click);
			// 
			// flowLayoutPanel8
			// 
			this.flowLayoutPanel8.AutoSize = true;
			this.flowLayoutPanel8.Controls.Add(this.StaffVisualizerConfigCheck);
			this.flowLayoutPanel8.Controls.Add(this.StaffGenerateCheck);
			this.flowLayoutPanel8.Controls.Add(this.StaffRelativeValueCheck);
			this.flowLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel8.Location = new System.Drawing.Point(5, 45);
			this.flowLayoutPanel8.Name = "flowLayoutPanel8";
			this.flowLayoutPanel8.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.flowLayoutPanel8.Size = new System.Drawing.Size(482, 31);
			this.flowLayoutPanel8.TabIndex = 1;
			// 
			// StaffVisualizerConfigCheck
			// 
			this.StaffVisualizerConfigCheck.AutoSize = true;
			this.StaffVisualizerConfigCheck.Location = new System.Drawing.Point(3, 6);
			this.StaffVisualizerConfigCheck.Name = "StaffVisualizerConfigCheck";
			this.StaffVisualizerConfigCheck.Size = new System.Drawing.Size(146, 19);
			this.StaffVisualizerConfigCheck.TabIndex = 4;
			this.StaffVisualizerConfigCheck.Text = "启用五线谱可视化效果";
			this.StaffVisualizerConfigCheck.UseVisualStyleBackColor = true;
			// 
			// StaffGenerateCheck
			// 
			this.StaffGenerateCheck.AutoSize = true;
			this.StaffGenerateCheck.Enabled = false;
			this.StaffGenerateCheck.Location = new System.Drawing.Point(155, 6);
			this.StaffGenerateCheck.Name = "StaffGenerateCheck";
			this.StaffGenerateCheck.Size = new System.Drawing.Size(86, 19);
			this.StaffGenerateCheck.TabIndex = 5;
			this.StaffGenerateCheck.Text = "生成五线谱";
			this.StaffGenerateCheck.UseVisualStyleBackColor = true;
			// 
			// SheetConfigInfoLabel
			// 
			this.SheetConfigInfoLabel.AutoSize = true;
			this.SheetConfigInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.SheetConfigInfoLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.SheetConfigInfoLabel.Location = new System.Drawing.Point(5, 5);
			this.SheetConfigInfoLabel.Name = "SheetConfigInfoLabel";
			this.SheetConfigInfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.SheetConfigInfoLabel.Size = new System.Drawing.Size(307, 40);
			this.SheetConfigInfoLabel.TabIndex = 0;
			this.SheetConfigInfoLabel.Text = "欲开启五线谱视觉效果，需要先开启“生成视频”选项。\r\n开启本功能会禁用视频视觉效果和视频拉伸选项。";
			// 
			// HelperTab
			// 
			this.HelperTab.Controls.Add(this.groupBox6);
			this.HelperTab.Controls.Add(this.label33);
			this.HelperTab.Location = new System.Drawing.Point(4, 24);
			this.HelperTab.Name = "HelperTab";
			this.HelperTab.Padding = new System.Windows.Forms.Padding(5);
			this.HelperTab.Size = new System.Drawing.Size(492, 542);
			this.HelperTab.TabIndex = 4;
			this.HelperTab.Text = "辅助功能";
			this.HelperTab.UseVisualStyleBackColor = true;
			// 
			// groupBox6
			// 
			this.groupBox6.AutoSize = true;
			this.groupBox6.Controls.Add(this.tableLayoutPanel11);
			this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox6.Location = new System.Drawing.Point(5, 45);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox6.Size = new System.Drawing.Size(482, 178);
			this.groupBox6.TabIndex = 2;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "间隔选择";
			// 
			// tableLayoutPanel11
			// 
			this.tableLayoutPanel11.AutoSize = true;
			this.tableLayoutPanel11.ColumnCount = 2;
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Controls.Add(this.SelectWhichEachGroupBox, 1, 2);
			this.tableLayoutPanel11.Controls.Add(this.label34, 0, 1);
			this.tableLayoutPanel11.Controls.Add(this.label35, 0, 2);
			this.tableLayoutPanel11.Controls.Add(this.SelectOneEveryFewBox, 1, 1);
			this.tableLayoutPanel11.Controls.Add(this.QuickSelectIntervalBtn, 0, 3);
			this.tableLayoutPanel11.Controls.Add(this.label26, 0, 0);
			this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel11.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			this.tableLayoutPanel11.RowCount = 4;
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel11.Size = new System.Drawing.Size(472, 152);
			this.tableLayoutPanel11.TabIndex = 0;
			// 
			// SelectWhichEachGroupBox
			// 
			this.SelectWhichEachGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectWhichEachGroupBox.Location = new System.Drawing.Point(100, 95);
			this.SelectWhichEachGroupBox.Maximum = new decimal(new int[] {
			2,
			0,
			0,
			0});
			this.SelectWhichEachGroupBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.SelectWhichEachGroupBox.Name = "SelectWhichEachGroupBox";
			this.SelectWhichEachGroupBox.Size = new System.Drawing.Size(75, 23);
			this.SelectWhichEachGroupBox.TabIndex = 3;
			this.SelectWhichEachGroupBox.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label34.Location = new System.Drawing.Point(3, 63);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(91, 29);
			this.label34.TabIndex = 0;
			this.label34.Text = "每几个选择一个";
			this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label35.Location = new System.Drawing.Point(3, 92);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(91, 29);
			this.label35.TabIndex = 1;
			this.label35.Text = "选择每组第几个";
			this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectOneEveryFewBox
			// 
			this.SelectOneEveryFewBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectOneEveryFewBox.Location = new System.Drawing.Point(100, 66);
			this.SelectOneEveryFewBox.Minimum = new decimal(new int[] {
			2,
			0,
			0,
			0});
			this.SelectOneEveryFewBox.Name = "SelectOneEveryFewBox";
			this.SelectOneEveryFewBox.Size = new System.Drawing.Size(75, 23);
			this.SelectOneEveryFewBox.TabIndex = 2;
			this.SelectOneEveryFewBox.Value = new decimal(new int[] {
			2,
			0,
			0,
			0});
			this.SelectOneEveryFewBox.ValueChanged += new System.EventHandler(this.SelectOneEveryFewBox_ValueChanged);
			// 
			// QuickSelectIntervalBtn
			// 
			this.QuickSelectIntervalBtn.AutoSize = true;
			this.tableLayoutPanel11.SetColumnSpan(this.QuickSelectIntervalBtn, 2);
			this.QuickSelectIntervalBtn.Location = new System.Drawing.Point(3, 124);
			this.QuickSelectIntervalBtn.Name = "QuickSelectIntervalBtn";
			this.QuickSelectIntervalBtn.Size = new System.Drawing.Size(90, 25);
			this.QuickSelectIntervalBtn.TabIndex = 4;
			this.QuickSelectIntervalBtn.Text = "快速间隔选择";
			this.Balloon.SetToolTip(this.QuickSelectIntervalBtn, "注意：点击之后将会关闭本对话框，\r\n部分您未保存的更改可能会丢失！");
			this.QuickSelectIntervalBtn.UseVisualStyleBackColor = true;
			this.QuickSelectIntervalBtn.Click += new System.EventHandler(this.QuickSelectIntervalBtn_Click);
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.tableLayoutPanel11.SetColumnSpan(this.label26, 2);
			this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label26.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.label26.Location = new System.Drawing.Point(3, 0);
			this.label26.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(466, 60);
			this.label26.TabIndex = 5;
			this.label26.Text = "使用说明：\r\n请先在 Vegas 轨道中选中一些素材，然后再打开本对话框，使用下面的功能。\r\n本功能旨在辅助用户每隔一个或几个选中一个素材，然后可以执行“粘贴视频" +
	"事件”等操作。";
			this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Dock = System.Windows.Forms.DockStyle.Top;
			this.label33.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.label33.Location = new System.Drawing.Point(5, 5);
			this.label33.Name = "label33";
			this.label33.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.label33.Size = new System.Drawing.Size(367, 40);
			this.label33.TabIndex = 1;
			this.label33.Text = "以下功能只是一些独立的辅助功能，与其它生成音视频的参数无关。\r\n操作之后将会关闭参数配置对话框，您可以稍后再重新打开。";
			// 
			// StaffLineColorDialog
			// 
			this.StaffLineColorDialog.AnyColor = true;
			this.StaffLineColorDialog.FullOpen = true;
			// 
			// Balloon
			// 
			this.Balloon.AutomaticDelay = 0;
			this.Balloon.AutoPopDelay = 20000;
			this.Balloon.InitialDelay = 0;
			this.Balloon.IsBalloon = true;
			this.Balloon.ReshowDelay = 0;
			this.Balloon.ShowAlways = true;
			this.Balloon.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			this.Balloon.ToolTipTitle = "填写说明";
			// 
			// StaffRelativeValueCheck
			// 
			this.StaffRelativeValueCheck.AutoSize = true;
			this.StaffRelativeValueCheck.Location = new System.Drawing.Point(247, 6);
			this.StaffRelativeValueCheck.Name = "StaffRelativeValueCheck";
			this.StaffRelativeValueCheck.Size = new System.Drawing.Size(86, 19);
			this.StaffRelativeValueCheck.TabIndex = 6;
			this.StaffRelativeValueCheck.Text = "使用相对值";
			this.Balloon.SetToolTip(this.StaffRelativeValueCheck, "勾选后，下方所填参数的像素单位将以相对于 1920 × 1080\r\n的尺寸进行定位；反之则以项目尺寸定位。\r\n");
			this.StaffRelativeValueCheck.UseVisualStyleBackColor = true;
			// 
			// ConfigForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(518, 626);
			this.Controls.Add(this.Tabs);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Location = new Point(40, 40);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(534, 665);
			this.Name = "ConfigForm";
			this.Padding = new System.Windows.Forms.Padding(9, 10, 9, 4);
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "音 MAD 助手 - 配置";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.Tabs.ResumeLayout(false);
			this.SourceTab.ResumeLayout(false);
			this.SourceTab.PerformLayout();
			this.MidiConfigGroup.ResumeLayout(false);
			this.MidiConfigGroup.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.flowLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MidiCustomBpmBox)).EndInit();
			this.SourceConfigGroup.ResumeLayout(false);
			this.SourceConfigGroup.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.flowLayoutPanel9.ResumeLayout(false);
			this.flowLayoutPanel9.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.AudioTab.ResumeLayout(false);
			this.AudioTab.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.VideoTab.ResumeLayout(false);
			this.VideoTab.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.SheetTab.ResumeLayout(false);
			this.SheetTab.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineThicknessBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfacePositionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfaceWidthBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineSpacingBox)).EndInit();
			this.flowLayoutPanel8.ResumeLayout(false);
			this.flowLayoutPanel8.PerformLayout();
			this.HelperTab.ResumeLayout(false);
			this.HelperTab.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.tableLayoutPanel11.ResumeLayout(false);
			this.tableLayoutPanel11.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.Button AboutBtn;
		public System.Windows.Forms.LinkLabel UserHelpLink;
		public System.Windows.Forms.TabControl Tabs;
		public System.Windows.Forms.TabPage SourceTab;
		public System.Windows.Forms.TabPage AudioTab;
		public System.Windows.Forms.TabPage VideoTab;
		public System.Windows.Forms.TabPage SheetTab;
		public System.Windows.Forms.TabPage HelperTab;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		public System.Windows.Forms.CheckBox AudioConfigCheck;
		public System.Windows.Forms.CheckBox AudioScratchCheck;
		public System.Windows.Forms.CheckBox AudioLoopCheck;
		public System.Windows.Forms.CheckBox AudioNormalizeCheck;
		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		public System.Windows.Forms.Label label10;
		public System.Windows.Forms.Label label11;
		public System.Windows.Forms.ComboBox AudioTuneMethodCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		public System.Windows.Forms.ComboBox AudioMainKeyCombo;
		public System.Windows.Forms.ComboBox AudioMainOctaveCombo;
		public System.Windows.Forms.GroupBox MidiConfigGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		public System.Windows.Forms.Label label7;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		public System.Windows.Forms.Label label6;
		public System.Windows.Forms.ComboBox MidiBeatCombo;
		public System.Windows.Forms.Label label5;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		public System.Windows.Forms.Button ChooseMidiBtn;
		public System.Windows.Forms.TextBox ChooseMidiText;
		public System.Windows.Forms.Label label4;
		public System.Windows.Forms.ComboBox MidiChannelCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
		public System.Windows.Forms.RadioButton MidiMidiBpmCheck;
		public System.Windows.Forms.RadioButton MidiProjectBpmCheck;
		public System.Windows.Forms.RadioButton MidiCustomBpmCheck;
		public System.Windows.Forms.NumericUpDown MidiCustomBpmBox;
		public System.Windows.Forms.GroupBox SourceConfigGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		public System.Windows.Forms.Label label1;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		public System.Windows.Forms.ComboBox ChooseSourceCombo;
		public System.Windows.Forms.Button ChooseSourceBtn;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		public System.Windows.Forms.Label label2;
		public System.Windows.Forms.Label label3;
		public System.Windows.Forms.GroupBox groupBox2;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		public System.Windows.Forms.Label label12;
		public System.Windows.Forms.Label label13;
		public IntegerTrackWithBox AudioFadeInBox;
		public IntegerTrackWithBox AudioFadeOutBox;
		public System.Windows.Forms.ComboBox AudioFadeInCurveCombo;
		public System.Windows.Forms.ComboBox AudioFadeOutCurveCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		public System.Windows.Forms.CheckBox VideoConfigCheck;
		public System.Windows.Forms.CheckBox VideoScratchCheck;
		public System.Windows.Forms.CheckBox VideoLoopCheck;
		public System.Windows.Forms.CheckBox VideoFreezeFirstFrameCheck;
		public System.Windows.Forms.GroupBox groupBox4;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		public System.Windows.Forms.ComboBox VideoFadeOutCurveCombo;
		public System.Windows.Forms.Label label16;
		public System.Windows.Forms.Label label17;
		public IntegerTrackWithBox VideoFadeInBox;
		public IntegerTrackWithBox VideoFadeOutBox;
		public System.Windows.Forms.ComboBox VideoFadeInCurveCombo;
		public System.Windows.Forms.GroupBox groupBox3;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		public System.Windows.Forms.Label label14;
		public System.Windows.Forms.Label label15;
		public System.Windows.Forms.ComboBox VideoEffectCombo;
		public System.Windows.Forms.CheckBox VideoGlueCheck;
		public System.Windows.Forms.ComboBox VideoEffectInitialValueCombo;
		public System.Windows.Forms.Label label25;
		public System.Windows.Forms.Label label24;
		public System.Windows.Forms.Label label23;
		public System.Windows.Forms.Label label22;
		public System.Windows.Forms.Label label21;
		public System.Windows.Forms.Label label20;
		public System.Windows.Forms.Label label19;
		public System.Windows.Forms.Label label18;
		public IntegerTrackWithBox VideoEndVerticalTransBox;
		public IntegerTrackWithBox VideoStartVerticalTransBox;
		public IntegerTrackWithBox VideoEndHorizontalTransBox;
		public IntegerTrackWithBox VideoStartHorizontalTransBox;
		public IntegerTrackWithBox VideoEndRotationBox;
		public IntegerTrackWithBox VideoStartRotationBox;
		public IntegerTrackWithBox VideoEndSizeBox;
		public System.Windows.Forms.ComboBox VideoStartSizeCurveCombo;
		public IntegerTrackWithBox VideoStartSizeBox;
		public System.Windows.Forms.Label SheetConfigInfoLabel;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
		public System.Windows.Forms.CheckBox StaffVisualizerConfigCheck;
		public System.Windows.Forms.CheckBox StaffGenerateCheck;
		public System.Windows.Forms.GroupBox groupBox5;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		public System.Windows.Forms.Label label32;
		public System.Windows.Forms.Label label31;
		public System.Windows.Forms.Label label30;
		public System.Windows.Forms.Label label29;
		public System.Windows.Forms.Label label27;
		public System.Windows.Forms.Label label28;
		public System.Windows.Forms.ComboBox StaffClefCombo;
		public System.Windows.Forms.NumericUpDown StaffLineSpacingBox;
		public System.Windows.Forms.NumericUpDown StaffLineThicknessBox;
		public System.Windows.Forms.NumericUpDown StaffSurfacePositionBox;
		public System.Windows.Forms.NumericUpDown StaffSurfaceWidthBox;
		public System.Windows.Forms.Button StaffLineColorBtn;
		public System.Windows.Forms.ColorDialog StaffLineColorDialog;
		public System.Windows.Forms.Label label33;
		public System.Windows.Forms.GroupBox groupBox6;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
		public System.Windows.Forms.NumericUpDown SelectWhichEachGroupBox;
		public System.Windows.Forms.Label label34;
		public System.Windows.Forms.Label label35;
		public System.Windows.Forms.NumericUpDown SelectOneEveryFewBox;
		public System.Windows.Forms.Button QuickSelectIntervalBtn;
		public System.Windows.Forms.ToolTip Balloon;
		public TimecodeBox SourceStartTimeText;
		public TimecodeBox SourceEndTimeText;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		public System.Windows.Forms.Label label8;
		public System.Windows.Forms.Label label9;
		public TimecodeBox MidiStartSecondBox;
		public TimecodeBox MidiEndSecondBox;
		public System.Windows.Forms.Label label26;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
		public System.Windows.Forms.RadioButton GenerateAtBeginRadio;
		public System.Windows.Forms.RadioButton GenerateAtCursorRadio;
		public System.Windows.Forms.Label label36;
		public System.Windows.Forms.Label WarningInfoLabel;
		public System.Windows.Forms.CheckBox StaffRelativeValueCheck;
	}
	#endregion

	public partial class ConfigForm : Form {

		public bool AcceptConfig = false;
		public static Icon icon;
		#if VEGAS_ENVIRONMENT
		public IniFile configIni { get { return parent.configIni; } set { parent.configIni = value; } }
		public readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		#endif

		/// <summary>
		/// ConfigForm 脚本对话框窗体的入口方法。
		/// </summary>
		/// <param name="entryPoint">调用本对象的父对象，也就是 Vegas 脚本的入口类</param>
		public ConfigForm(EntryPoint entryPoint) {
			InitializeComponent();
			#if VEGAS_ENVIRONMENT
			parent = entryPoint;

			#region MIDI 速度控制点击事件
			MidiCustomBpmCheck.CheckedChanged += (sender, e) => { MidiCustomBpmBox.Enabled = MidiCustomBpmCheck.Checked; };
			MidiProjectBpmCheck.Text = "项目速度：" + ProcessBpmDouble(parent.ProjectBpm);
			MidiCustomBpmBox.Value = (decimal)Math.Max(parent.ProjectBpm, (double)MidiCustomBpmBox.Minimum);
			GenerateAtBeginRadio.Text = "项目开始处：" + Timecode.FromMilliseconds(0).ToPositionString();
			GenerateAtCursorRadio.Text = "光标处：" + vegas.Transport.CursorPosition.ToPositionString();
			#endregion

			#region 浏览并打开媒体文件
			ChooseMidiBtn.Click += (sender, e) => parent.SelectMidiFile();
			ChooseSourceBtn.Click += (sender, e) => parent.SelectVideoClip();
			parent.AudioVideoEnabledTable_Init();
			if (parent.audioVideoEnabledTable.SelectNoEvents) DisabledSelectIntervalPart();
			ChooseSourceCombo_SelectedIndexChanged(null, null);
			#endregion
			#endif

			#region 复选框设置、下拉菜单默认值
			EventHandler setCheckedEnabled = new EventHandler(SetCheckedEnabled);
			VideoConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioConfigCheck.CheckedChanged += setCheckedEnabled;
			StaffVisualizerConfigCheck.CheckedChanged += setCheckedEnabled;
			StaffGenerateCheck.CheckedChanged += setCheckedEnabled;
			AudioTuneMethodCombo.SelectedIndexChanged += setCheckedEnabled;
			VideoEffectCombo.SelectedIndexChanged += setCheckedEnabled;
			Tabs.SelectedIndexChanged += setCheckedEnabled;
			#if VEGAS_ENVIRONMENT
			string configIniName = "otomad_helper.ini";
			configIni = new IniFile(Path.r(vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData), configIniName).FullPath, this);
			ReadIni();
			#endif
			#endregion

			#region 程序图标
			#if VEGAS_ENVIRONMENT
			string iconName = "otomad_helper.ico";
			try {
				Icon = Icon.ExtractAssociatedIcon(Path.r(vegas.InstallationDirectory, "Script Menu", iconName).FullPath);
				icon = Icon;
			} catch (Exception) { } // 如果路径不存在则不受影响
			#endif
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
				AudioFadeInBox.SetValue(configIni.ReadInt("FadeIn", 0), 0);
				AudioFadeOutBox.SetValue(configIni.ReadInt("FadeOut", 0), 0);
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
				VideoGlueCheck.Checked = configIni.ReadBool("Glue", false);
				VideoFadeInBox.SetValue(configIni.ReadInt("FadeIn", 0), 0);
				VideoFadeOutBox.SetValue(configIni.ReadInt("FadeOut", 0), 0);
				VideoStartSizeBox.SetValue(configIni.ReadInt("StartSize", 100), 100);
				VideoEndSizeBox.SetValue(configIni.ReadInt("EndSize", 100), 100);
				VideoStartRotationBox.SetValue(configIni.ReadInt("StartRotation", 0), 0);
				VideoEndRotationBox.SetValue(configIni.ReadInt("EndRotation", 0), 0);
				VideoStartHorizontalTransBox.SetValue(configIni.ReadInt("StartHorizontalTrans", 0), 0);
				VideoEndHorizontalTransBox.SetValue(configIni.ReadInt("EndHorizontalTrans", 0), 0);
				VideoStartVerticalTransBox.SetValue(configIni.ReadInt("StartVerticalTrans", 0), 0);
				VideoEndVerticalTransBox.SetValue(configIni.ReadInt("EndVerticalTrans", 0), 0);
				SetComboIndex(VideoStartSizeCurveCombo, configIni.ReadInt("StartSizeCurve", 1), 1);
				SetComboIndex(VideoFadeInCurveCombo, configIni.ReadInt("FadeInCurve", 3), 3);
				SetComboIndex(VideoFadeOutCurveCombo, configIni.ReadInt("FadeOutCurve", 3), 3);
				configIni.EndSection();
			#endregion

			#region 素材配置
				configIni.StartSection("Source");
				if (configIni.ReadBool("GenerateAtCursor", false)) GenerateAtCursorRadio.Checked = true;
				else GenerateAtBeginRadio.Checked = true;
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
				StaffRelativeValueCheck.Checked = configIni.ReadBool("RelativeValue", false);
				SetComboIndex(StaffClefCombo, configIni.ReadInt("Clef", 0), 0);
				SetNumericValue(StaffLineSpacingBox, configIni.ReadInt("Gap", 44), 44); // 45
				SetNumericValue(StaffSurfaceWidthBox, configIni.ReadInt("Width", 1500), 1500); // 1000
				SetNumericValue(StaffSurfacePositionBox, configIni.ReadInt("Position", 225), 225); // 0
				SetNumericValue(StaffLineThicknessBox, configIni.ReadInt("Thickness", 50), 50);
				ReadStaffLineColor(configIni.Read("Color", "#ffffff"));
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
			configIni.Write("FadeIn", AudioFadeInBox.Value);
			configIni.Write("FadeOut", AudioFadeOutBox.Value);
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
			configIni.Write("Glue", VideoGlueCheck.Checked);
			configIni.Write("FadeIn", VideoFadeInBox.Value);
			configIni.Write("FadeOut", VideoFadeOutBox.Value);
			configIni.Write("StartSize", VideoStartSizeBox.Value);
			configIni.Write("EndSize", VideoEndSizeBox.Value);
			configIni.Write("StartRotation", VideoStartRotationBox.Value);
			configIni.Write("EndRotation", VideoEndRotationBox.Value);
			configIni.Write("StartHorizontalTrans", VideoStartHorizontalTransBox.Value);
			configIni.Write("EndHorizontalTrans", VideoEndHorizontalTransBox.Value);
			configIni.Write("StartVerticalTrans", VideoStartVerticalTransBox.Value);
			configIni.Write("EndVerticalTrans", VideoEndVerticalTransBox.Value);
			configIni.Write("StartSizeCurve", VideoStartSizeCurveCombo.SelectedIndex);
			configIni.Write("FadeInCurve", VideoFadeInCurveCombo.SelectedIndex);
			configIni.Write("FadeOutCurve", VideoFadeOutCurveCombo.SelectedIndex);
			configIni.EndSection();
			#endregion

			#region 素材配置
			configIni.StartSection("Source");
			configIni.Write("GenerateAtCursor", GenerateAtCursorRadio.Checked);
			if (AcceptConfig) configIni.Write("LastMediaSourceFrom", ChooseSourceCombo.SelectedIndex);
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
			configIni.Write("RelativeValue", StaffRelativeValueCheck.Checked);
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
			// Environment.Exit(0); // 可以顺带一块ㄦ把 Vegas 干掉。
			#if VEGAS_ENVIRONMENT
			parent.RemoveLastUnusedMedia();
			#endif
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			AcceptConfig = true;
			SaveIni();
			// 特殊处理部分
			if (StaffVisualizerConfigCheck.Checked && StaffVisualizerConfigCheck.Enabled) {
				VideoEffectCombo.SelectedIndex = 0;
				VideoScratchCheck.Checked = false;
				VideoGlueCheck.Checked = false;
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
				VideoEffectCombo.Enabled = VideoEffectInitialValueCombo.Enabled =
				VideoGlueCheck.Enabled =
				VideoScratchCheck.Checked = VideoScratchCheck.Enabled = false;
			SetEnabled(SheetTab, isSheetConfigOn, new Control[] { StaffVisualizerConfigCheck, SheetConfigInfoLabel });
			if (!StaffGenerateCheck.Checked)
				StaffLineThicknessBox.Enabled = StaffLineColorBtn.Enabled = false;
		}

		/// <summary>
		/// 使能或失能某个控件及其所有子控件。
		/// </summary>
		/// <param name="container">容器</param>
		/// <param name="enabled">启用还是禁用</param>
		/// <param name="excepts">例外列表，位于列表内的控件不受影响</param>
		private void SetEnabled(Control container, bool enabled, Control[] excepts = null) {
			var contains = new Func<Control[], Control, bool>((list, item) => {
				foreach (Control control in list)
					if (control == item)
						return true;
				return false;
			});
			foreach (Control control in container.Controls) {
				if (excepts != null && contains(excepts, control)) continue;
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
				"仓库地址：https://github.com/Chaosinism/vegas_scripts\n",
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

		public void ChooseSourceCombo_SelectedIndexChanged(object sender, EventArgs e) {
			#if VEGAS_ENVIRONMENT
			int SourceConfigFrom = ChooseSourceCombo.SelectedIndex;
			var table = parent.audioVideoEnabledTable;
			var group = SourceConfigFrom == 0 ? table.FromSelectedMedia :
						SourceConfigFrom == 1 ? table.FromSelectedClip : table.FromBrowseFile;
			AudioConfigCheck.Enabled = AudioConfigCheck.Checked = group.AudioEnabled;
			VideoConfigCheck.Enabled = VideoConfigCheck.Checked = group.VideoEnabled;
			WarningInfoLabel.Text =
				SourceConfigFrom == 0 && parent.audioVideoEnabledTable.SelectNoMedia ? "警告：您没有在项目媒体窗口中选中任何有效媒体素材！" :
				SourceConfigFrom == 1 && parent.audioVideoEnabledTable.SelectNoEvents ? "警告：您没有在轨道窗口中选中任何剪辑片段！" : "";
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
			HelperTab.Parent = null; // 隐藏选项卡
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
}
