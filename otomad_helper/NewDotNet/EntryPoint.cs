//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

using NAudio.Midi;
using ScriptPortal.Vegas;

namespace VegasScript {
	public class EntryPoint {
		// 配置参数变量
		private bool VConfig { get { return configForm.VideoConfigCheck.Checked; } }
		private VideoAnimFx VConfigAnim { get { return configForm.VideoEffect; } }
		private float VConfigStartSize { get { return configForm.VideoStartSizeTrack.Value; } }
		private float VConfigEndSize { get { return configForm.VideoEndSizeTrack.Value; } }
		private int VConfigFadein { get { return configForm.VideoFadeInTrack.Value; } }
		private int VConfigFadeout { get { return configForm.VideoFadeOutTrack.Value; } }
		private bool VConfigScratch { get { return configForm.VideoScratchCheck.Checked; } }
		private bool VConfigLoop { get { return configForm.VideoLoopCheck.Checked; } }
		private bool AConfig { get { return configForm.AudioConfigCheck.Checked; } }
		private int AConfigTrack { get { return configForm.MidiChannelCombo.SelectedIndex; } }
		private int AConfigBasePitch {
			get {
				return PitchMap(
					configForm.AudioMainKeyCombo.SelectedItem.ToString() ?? "C",
					configForm.AudioMainOctaveCombo.SelectedItem.ToString() ?? "5"
				);
			}
		}
		private bool AConfigNoTune { get { return configForm.AudioTuneCheck.Checked; } }
		private double MidiConfigStartTime { get { return (double)(configForm.MidiStartSecondBox.Value * 1000); } }
		private double MidiConfigEndTime { get { return (double)(configForm.MidiEndSecondBox.Value * 1000); } }
		private double SourceConfigStartTime { get { return configForm.SourceStartTimeValue; } }
		private double SourceConfigEndTime { get { return configForm.SourceEndTimeValue; } }
		private MediaSourceFrom SourceFrom { get { return (MediaSourceFrom)configForm.ChooseSourceCombo.SelectedIndex; } }

		// 实例对象变量
		// #pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
		private ConfigForm configForm;
		private Vegas vegas;
		// #pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

		// 媒体 / MIDI 参数变量
		internal MidiFile midi = null;
		internal Media media = null;
		private double audioLength = 0;
		private double videoLength = 0;
		private string[] trackInfo = null;
		private int ticksPerQuarter = 0;
		private double msPerQuarter = 0;
		private bool IsFromSelectedMedia { get { return SourceFrom == MediaSourceFrom.SELECTED_MEDIA; } }
		private bool IsFromSelectedClip { get { return SourceFrom == MediaSourceFrom.SELECTED_CLIP; } }
		private bool IsFromBrowseFile { get { return SourceFrom == MediaSourceFrom.BROWSE_FILE; } }
		private SelectedEventSet selectedEventSet = new SelectedEventSet();

		/// <summary>
		/// 根据主音高名称转换为主音高对应的值。
		/// </summary>
		/// <param name="key">音名</param>
		/// <param name="oct">八度</param>
		/// <returns>主音高结果值</returns>
		public static int PitchMap(string key, string oct) {
			List<string> keys = new List<string>("C,C#,D,D#,E,F,F#,G,G#,A,A#,B".Split(','));
			int value = keys.IndexOf(key) + int.Parse(oct) * 12;
			// Console.WriteLine(value.ToString());
			return value;
		}

		/// <summary>
		/// 打开参数配置设置对话框。
		/// </summary>
		/// <returns>对话框最后选择的按钮是<code>“生成” (true)</code>还是<code>“取消” (false)</code>。</returns>
		public bool ShowConfigForm() {
			if (configForm == null) configForm = new ConfigForm(this);
			Application.Run(configForm);
			return configForm.AcceptConfig;
		}

		/// <summary>
		/// 打开选择 MIDI 文件对话框。
		/// </summary>
		/// <returns>是否选择了文件？而不是点击取消。</returns>
		internal bool SelectMidiFile() {
			#region 选择一个 MIDI 文件
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "MIDI 序列|*.mid|所有文件|*.*";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.FilterIndex = 1;
			if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
			string midiName = openFileDialog.FileName;
			try {
				midi = new MidiFile(midiName);
			} catch (Exception e) { ShowError(new Exceptions.NotAMidiFileException(), e); return false; }
			#endregion

			#region 生成每个 MIDI 音轨的统计信息
			var combo = configForm.MidiChannelCombo;
			combo.Items.Clear();
			trackInfo = new string[midi.Events.Tracks];
			ticksPerQuarter = midi.DeltaTicksPerQuarterNote;
			msPerQuarter = 0;
			int? selectedIndex = null; // 第一个不是零音符的音轨
			for (int i = 0; i < midi.Events.Tracks; i++) {
				List<string> info = new List<string>(new string[] { "轨道 " + i + "：", "", "音符数：", "起音 " });
				int notesCount = 0;

				foreach (MidiEvent midiEvent in midi.Events[i]) {
					if ((midiEvent is NoteEvent) && !(midiEvent is NoteOnEvent)) {
						NoteEvent noteEvent = (NoteEvent)midiEvent;
						if (notesCount++ == 0) info[3] += noteEvent.NoteName; // 起音判断
					}
					if ((midiEvent is PatchChangeEvent) && info[1].Length == 0) {
						PatchChangeEvent patchEvent = (PatchChangeEvent)midiEvent;
						for (int j = 4; j < patchEvent.ToString().Split(' ').Length; j++)
							info[1] += patchEvent.ToString().Split(' ')[j];
					}
					if ((midiEvent is TempoEvent) && msPerQuarter == 0) {
						TempoEvent tempoEvent = (TempoEvent)midiEvent;
						msPerQuarter = Convert.ToDouble(tempoEvent.MicrosecondsPerQuarterNote) / 1000; // 每四分音符多少毫秒
					}
				}

				info[2] += notesCount.ToString();
				info[0] = info[0].TrimEnd('：');
				if (notesCount == 0) info.RemoveAt(3);
				if (info[1].Length == 0) info.RemoveAt(1);
				trackInfo[i] = string.Join("；", info);
				if (notesCount != 0) selectedIndex = selectedIndex ?? i;

				#region 开始配置
				combo.Items.Add(trackInfo[i]);
				#endregion
			}
			combo.SelectedIndex = selectedIndex ?? 0;
			configForm.ChooseMidiText.Text = midiName;
			configForm.OkBtn.Enabled = true;
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
			// configForm.AudioConfigCheck.Checked = configForm.AudioConfigCheck.Enabled = media.HasAudio();
			// configForm.VideoConfigCheck.Checked = configForm.VideoConfigCheck.Enabled = media.HasVideo();
			return true;
		}
		private bool OpenMedia(string clipName) {
			removeLastUnusedMedia();
			MediaFileExist = vegas.Project.MediaPool.Contains(clipName);
			return OpenMedia(new Media(clipName));
		}
		private bool MediaFileExist = false;
		public void removeLastUnusedMedia() {
			if (this.media != null && !MediaFileExist) vegas.Project.MediaPool.Remove(this.media.KeyString);
		}

		/// <summary>
		/// 打开选择媒体文件对话框。
		/// </summary>
		/// <returns>是否选择了文件？而不是点击取消。</returns>
		internal bool SelectVideoClip() {
			#region 选择一个视频剪辑
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "所有文件|*.*";
			openFileDialog.RestoreDirectory = true;
			openFileDialog.FilterIndex = 1;
			if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
			string clipName = openFileDialog.FileName;
			if (!OpenMedia(clipName)) {
				audioVideoEnabledTable.FromBrowseFile.AudioEnabled = audioVideoEnabledTable.FromBrowseFile.VideoEnabled = false;
				return false;
			}
			audioVideoEnabledTable.FromBrowseFile.AudioEnabled = media.HasAudio();
			audioVideoEnabledTable.FromBrowseFile.VideoEnabled = media.HasVideo();
			// configForm.ChooseSourceCombo.SelectedIndexChanged();
			var sourceCbo = configForm.ChooseSourceCombo;
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
		}
		public void ShowError(Exception e) {
			vegas.ShowError(e);
		}
		public void ShowError(Exception e1, Exception e2) {
			vegas.ShowError(e1.Message, e2.ToString());
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

		public class AudioVideoEnabledTable {
			public class AudioVideoEnabledGroup {
				public bool AudioEnabled = false;
				public bool VideoEnabled = false;
			}
			public AudioVideoEnabledGroup FromSelectedMedia = new AudioVideoEnabledGroup();
			public AudioVideoEnabledGroup FromSelectedClip = new AudioVideoEnabledGroup();
			public AudioVideoEnabledGroup FromBrowseFile = new AudioVideoEnabledGroup();
		}
		AudioVideoEnabledTable audioVideoEnabledTable = new AudioVideoEnabledTable();
		public void AudioVideoEnabledTable_Init() {
			var selectedMedia = vegas.Project.MediaPool.GetSelectedMedia();
			if (selectedMedia.Length != 0) {
				Media media = selectedMedia[0];
				audioVideoEnabledTable.FromSelectedMedia.AudioEnabled = media.HasAudio();
				audioVideoEnabledTable.FromSelectedMedia.VideoEnabled = media.HasVideo();
			}
			GetSelectedEventSet();
			audioVideoEnabledTable.FromSelectedClip.AudioEnabled = selectedEventSet.audioEvent != null;
			audioVideoEnabledTable.FromSelectedClip.VideoEnabled = selectedEventSet.videoEvent != null;
		}

		private class SelectedEventSet {
			public AudioEvent audioEvent = null;
			public VideoEvent videoEvent = null;
		}

		public TrackEvent[] GetSelectedEvents(Project project) {
			List<TrackEvent> selectedList = new List<TrackEvent>();
			foreach (Track track in project.Tracks)
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
							audioLength = trackEvent.Length.ToMilliseconds();
						}
						if (trackEvent.IsVideo() && selectedEventSet.videoEvent == null) {
							selectedEventSet.videoEvent = (VideoEvent)trackEvent;
							videoLength = trackEvent.Length.ToMilliseconds();
						}
					}
			if (selectedEventSet.audioEvent != null && selectedEventSet.videoEvent == null) {
				foreach (TrackEvent trackEvent in selectedEventSet.audioEvent.Group)
					if (trackEvent.IsVideo()) {
						selectedEventSet.videoEvent = (VideoEvent)trackEvent;
						videoLength = trackEvent.Length.ToMilliseconds();
						break;
					}
			} else if (selectedEventSet.audioEvent == null && selectedEventSet.videoEvent != null) {
				foreach (TrackEvent trackEvent in selectedEventSet.videoEvent.Group)
					if (trackEvent.IsAudio()) {
						selectedEventSet.audioEvent = (AudioEvent)trackEvent;
						audioLength = trackEvent.Length.ToMilliseconds();
						break;
					}
			}
		}

		private bool GetSelectedSource() {
			if (IsFromSelectedMedia) {
				var selections = vegas.Project.MediaPool.GetSelectedMedia();
				if (selections.Length == 0) {
					ShowError(new Exceptions.NoSelectedMediaException());
					media = null;
					return false;
				}
				OpenMedia(selections[0]);
			}
			if (IsFromSelectedClip) {
				GetSelectedEventSet(); // 这里记得要去掉
				if (selectedEventSet.audioEvent == null && selectedEventSet.videoEvent == null) {
					ShowError(new Exceptions.NoSelectedClipException());
					media = null;
					return false;
				}
			}
			return true;
		}

		private bool usePitchShiftPlugin = false;
		private bool usePitchWithScratch = true;

		/// <summary>
		/// 生成音 MAD。
		/// </summary>
		private void GenerateOtomad() {
			#region 验证数据合法
			if (!IsFromBrowseFile) { bool ok = GetSelectedSource(); if (!ok) return; }
			if (midi == null) { ShowError(new Exceptions.NoMidiException()); return; }
			if (media == null && !IsFromSelectedClip) { ShowError(new Exceptions.NoMediaException()); return; }
			if (trackInfo == null) { ShowError(new Exceptions.NoTrackInfoException()); return; }
			#endregion

			#region 开始处理 MIDI
			VideoTrack vTrack = null;
			bool needTwoKey = VConfigStartSize != VConfigEndSize;
			VideoAnimationEffect anim = new VideoAnimationEffect(VConfigAnim);
			if (VConfig)
				vTrack = vegas.Project.AddVideoTrack();

			const int MAX_AUDIO_TRACK_SIZE = 20;
			AudioTrack[] aTracks = null;
			double[] aTrackPositions = null;
			int aTrackCount = 1;
			if (AConfig) {
				aTracks = new AudioTrack[MAX_AUDIO_TRACK_SIZE];
				aTrackPositions = new double[MAX_AUDIO_TRACK_SIZE];
				aTracks[0] = vegas.Project.AddAudioTrack();
				aTrackPositions[0] = 0;
			}

			PlugInNode plugIn = vegas.AudioFX.FindChildByName("移调")
				?? vegas.AudioFX.FindChildByName("Pitch Shift")
				?? vegas.AudioFX.FindChildByUniqueID("{ED1B4100-93BE-11D0-AEBC-00A0C9053912}");
			PlugInNode plugInInvert = vegas.VideoFX.FindChildByName("VEGAS 反转")
				?? vegas.VideoFX.FindChildByName("反转");

			foreach (MidiEvent midiEvent in midi.Events[AConfigTrack]) {
				if (midiEvent is NoteOnEvent) {
					NoteEvent noteEvent = (NoteEvent)midiEvent;
					NoteOnEvent noteOnEvent = (NoteOnEvent)midiEvent;
					double startTime = midiEvent.AbsoluteTime * msPerQuarter / ticksPerQuarter;
					double duration = noteOnEvent.NoteLength * msPerQuarter / ticksPerQuarter;
					int pitch = noteEvent.NoteNumber;
					int trackIndex = 0;

					if (startTime < MidiConfigStartTime) continue;
					if (startTime > MidiConfigEndTime) break;

					#region 生成音频事件
					if (AConfig) {
						if (aTracks == null || aTrackPositions == null) continue;
						while (startTime < aTrackPositions[trackIndex])
							if (++trackIndex == aTrackCount) {
								aTrackCount++;
								aTracks[trackIndex] = vegas.Project.AddAudioTrack();
							}
						AudioEvent audioEvent = null;
						if (!IsFromSelectedClip) {
							audioEvent = aTracks[trackIndex].AddAudioEvent(
								Timecode.FromMilliseconds(startTime),
								Timecode.FromMilliseconds(duration)
							);
							try {
								audioEvent.AddTake(media.GetAudioStreamByIndex(0));
							} catch (Exception e) { ShowError(new Exceptions.NoAudioTakeException(), e); return; }
						} else {
							if (selectedEventSet.audioEvent == null) { ShowError(new Exceptions.NoAudioTakeException()); return; }
							audioEvent = (AudioEvent)selectedEventSet.audioEvent.Copy(aTracks[trackIndex], Timecode.FromMilliseconds(startTime));
							audioEvent.Length = Timecode.FromMilliseconds(duration);
						}
						aTrackPositions[trackIndex] = startTime + duration;
						if (configForm.VideoScratchCheck.Checked) audioEvent.AdjustPlaybackRate(audioLength / duration, true);
						audioEvent.Loop = configForm.VideoLoopCheck.Checked;

						#region 应用变调
						if (!AConfigNoTune) {
							int pitchDelta = pitch - AConfigBasePitch;
							if (usePitchShiftPlugin) {
								if (plugIn == null) { ShowError(new Exceptions.NoPluginException()); return; }
								int pitchDeltaTimes = pitchDelta > 0 ? 12 : -12;
								while (pitchDeltaTimes * pitchDelta > 0) { // pitchDeltaTimes > 0 ? pitchDelta > 0 : pitchDelta < 0
									Effect effect = new Effect(plugIn);
									audioEvent.Effects.Add(effect);
									try {
										effect.Preset = (Math.Abs(pitchDelta) <= 12 ? pitchDelta : pitchDeltaTimes).ToString();
									} catch (Exception e) { ShowError(new Exceptions.NoPluginPresetException(), e); return; }
									pitchDelta -= pitchDeltaTimes;
								}
							} else if (!usePitchWithScratch) audioEvent.PitchSemis = (double)pitchDelta;
							else {
								audioEvent.PitchLock = true;
								audioEvent.AdjustPlaybackRate(Math.Pow(2, (double)pitchDelta / 12.0), true);
							}
						}
						#endregion
					}
					#endregion

					#region 生成视频事件
					if (VConfig) {
						if (vTrack == null) continue;
						double vTrackPosition = startTime + duration;
						VideoEvent videoEvent = null;
						if (!IsFromSelectedClip) {
							videoEvent = vTrack.AddVideoEvent(
								Timecode.FromMilliseconds(startTime),
								Timecode.FromMilliseconds(duration)
							);
							try {
								videoEvent.AddTake(media.GetVideoStreamByIndex(0));
							} catch (Exception) { ShowError(new Exceptions.NoVideoTakeException()); return; }
						} else {
							if (selectedEventSet.videoEvent == null) { ShowError(new Exceptions.NoVideoTakeException()); return; }
							videoEvent = (VideoEvent)selectedEventSet.videoEvent.Copy(vTrack, Timecode.FromMilliseconds(startTime));
							videoEvent.Length = Timecode.FromMilliseconds(duration);
						}
						if (configForm.VideoScratchCheck.Checked) videoEvent.AdjustPlaybackRate(videoLength / duration, true);
						videoEvent.Loop = configForm.VideoLoopCheck.Checked;

						videoEvent.FadeIn.Length = Timecode.FromMilliseconds(duration * VConfigFadein / 100);
						videoEvent.FadeOut.Length = Timecode.FromMilliseconds(duration * VConfigFadeout / 100);

						VideoMotionKeyframe key0 = videoEvent.VideoMotion.Keyframes[0];
						VideoMotionKeyframe key1 = new VideoMotionKeyframe(Timecode.FromMilliseconds(duration));
						if (needTwoKey) videoEvent.VideoMotion.Keyframes.Add(key1);
						key0.ScaleBy(new VideoMotionVertex((VConfigStartSize / 100) * anim.HorizontalFlip, (VConfigStartSize / 100) * anim.VerticalFlip));
						key0.Rotation = anim.RotationDeg;
						key0.Type = VideoKeyframeType.Fast;
						if (needTwoKey) {
							key1.ScaleBy(new VideoMotionVertex((VConfigEndSize / 100) * anim.HorizontalFlip, (VConfigEndSize / 100) * anim.VerticalFlip));
							key1.Rotation = anim.RotationDeg;
						}
						if (anim.IsNegative) {
							if (plugInInvert == null) { ShowError(new Exceptions.NoPluginInvertException()); return; }
							Effect effect = new Effect(plugInInvert);
							videoEvent.Effects.Add(effect);
						}

						anim.Next();
					}
					#endregion
				}
			}
			#endregion
		}

		/// <summary>
		/// Vegas 脚本的入口方法。
		/// </summary>
		/// <param name="myVegas">你的 Vegas 软件。</param>
		public void FromVegas(Vegas myVegas) {
			vegas = myVegas;
			if (!ShowConfigForm()) return;
			//ReadConfig();
			GenerateOtomad();
		}
	}

	/// <summary>
	/// 选中媒体的素材来源。
	/// </summary>
	public enum MediaSourceFrom {
		SELECTED_MEDIA,
		SELECTED_CLIP,
		BROWSE_FILE
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
		NEGATIVE,
		PINGPONG
	}

	/// <summary>
	/// 视频的动画效果类。
	/// </summary>
	public class VideoAnimationEffect {
		private VideoAnimFx animFx;
		private int step = 0;
		private int duration = 0;

		private bool horizontalFlip = false;
		private bool verticalFlip = false;
		private bool isNegative = false;
		private bool isReverse = false;
		private int rotationStep = 0;

		public int HorizontalFlip { get { return horizontalFlip ? -1 : 1; } }
		public int VerticalFlip { get { return verticalFlip ? -1 : 1; } }
		public bool IsNegative { get { return isNegative; } }
		public bool IsReverse { get { return isReverse; } }
		public double RotationDeg { get { return (double)rotationStep * Math.PI / 2; } }

		public VideoAnimationEffect(VideoAnimFx fx) {
			animFx = fx;
			step = 0;
			switch (fx) {
				case VideoAnimFx.H_FLIP:
				case VideoAnimFx.V_FLIP:
				case VideoAnimFx.NEGATIVE:
				case VideoAnimFx.PINGPONG:
					duration = 2; break;
				case VideoAnimFx.CCW_FLIP:
				case VideoAnimFx.CW_FLIP:
				case VideoAnimFx.CCW_ROTATE:
				case VideoAnimFx.CW_ROTATE:
					duration = 4; break;
				case VideoAnimFx.NONE:
					duration = 1; break;
				default:
					duration = 0; break;
			}
		}
		private void NextStep() {
			step = (step + 1) % duration;
		}
		public void Next() {
			NextStep();
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
				default:
					break;
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

		public class NoPluginException : Exception {
			/// <summary>
			/// 无法调用移调插件报错。
			/// </summary>
			public NoPluginException() : base("错误：无法调用移调插件。\n\n" +
				"请按照教程文档 " + ConfigForm.aboutHelpLink + " 的指引正确操作。\n" +
				"不过，根据这个更新版本的脚本，按理应当是中英文版本均可正常运行的。" +
				"因此很有可能您是使用其它语言的 Vegas 造成的（逃") { }
		}

		public class NoPluginPresetException : Exception {
			/// <summary>
			/// 无法调用移调插件的预设效果报错。
			/// </summary>
			public NoPluginPresetException() : base("错误：无法调用移调插件的预设效果。\n\n" +
				"请按照教程文档 " + ConfigForm.aboutHelpLink + " 的指引正确操作。\n" +
				"确保在移调插件中手动添加了所有的 25 个预设，且命名正确。\n\n" +
				"补充说明：具体可见上述链接专栏中对于安装方法的说明。这 25 个预设是上下一个八度以内的所有变调种类，" +
				"缺少任何一个都有可能出错。手动添加预设的确非常麻烦，但 Vegas 无法使用脚本来指定变调的具体参数，" +
				"因此只好绕这个弯子。") { }
		}

		public class NoPluginInvertException : Exception {
			/// <summary>
			/// 无法调用反转插件报错。
			/// </summary>
			public NoPluginInvertException() : base("错误：无法调用反转插件。") { }
		}

		private static readonly string NoTakeExceptionPS = "补充说明：若仍不能解决，说明该素材文件可能是 Vegas 不支持的格式，" +
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
				"补充说明：MIDI 文件有多种格式，脚本不保证都能够正确读取。所幸主流宿主软件在" +
				"默认设置下导出的 MIDI 文件一般是可以读取的。（目前测试过 FL Studio、LMMS " +
				"与 Music Studio for iPad。）") { }
		}

		private static readonly string NoSelectedExceptionPS = "如果您想手动在文件夹中选择一个媒体素材，那么请点击其右边的“浏览”按钮，选择一个媒体素材。" +
			"并确保左侧的下拉菜单中选中的是您所选文件所在的路径。";
		public class NoSelectedMediaException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedMediaException() : base("错误：没有在项目媒体窗口中选择任何媒体。\n\n" +
				"请在项目媒体窗口中选择一个媒体，然后重新打开参数配置窗口，并在素材设置中选择“选中的媒体文件”。\n" +
				NoSelectedExceptionPS) { }
		}
		public class NoSelectedClipException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedClipException() : base("错误：没有在轨道中选择任何剪辑。\n\n" +
				"请在轨道中选择一个剪辑，然后重新打开参数配置窗口，并在素材设置中选择“选中的轨道素材”。\n" +
				NoSelectedExceptionPS) { }
		}
	}
}
