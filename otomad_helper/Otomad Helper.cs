/**
 * 音 MAD 助手 Vegas 版，旨在使 Vegas 接受 MIDI 序列文件作为输入，自动生成音 MAD / YTPMV 的轨道。
 * Vegas 16 及以上的版本支持所有功能，Vegas 13 及以上的版本可以兼容运行。
 * 将本脚本及其它所有附属文件放置在您 Vegas 安装目录下的 Script Menu 文件夹中。
 * 具体说明请参见下方的说明文档链接。
 * 本脚本基于原作者 Chaosinism 的开源代码二次开发，此外使用了 NAudio 库。
 *
 * 新版脚本由 淅琳雨 Otomad 重新编写。
 * 在此处获取最新版：https://github.com/otomad/VegasScripts/releases/latest
 * 仓库地址：https://github.com/otomad/VegasScripts
 *
 * Evauation 的说明文档（英语）：https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit
 * Cassidy 的教程视频（英语）：https://www.youtube.com/watch?v=8vSpzgL_86A
 *
 * 旧版说明文档：
 * 更新日志 (v4.9.25.0)（中文）：https://www.bilibili.com/read/cv13335178
 * 更新日志 (v4.10.17.0)（中文）：https://www.bilibili.com/read/cv13614419
 * 说明文档（B 站）(v0.1)（中文）：https://www.bilibili.com/read/cv392013
 * 说明文档（B 碗）(v0.1)（日语）：https://bowlroll.net/user/261124
 * 五线谱可视化说明文档 (v0.1)（中文）：https://www.bilibili.com/read/cv1027442
 * 疑难解答（中文）：https://www.bilibili.com/read/cv495309
 * 教程视频 (v0.1)（中文）：https://www.bilibili.com/video/av22226321
 *
 * 参考与致谢：
 * 原仓库地址：https://github.com/Chaosinism/vegas_scripts
 * https://github.com/evankale/VegasScripts
 * https://github.com/naudio/NAudio
 * https://www.jetdv.com/
 * https://www.vegascreativesoftware.info/us/vegas-pro-forum/scripting/
 *
 * 开工时间：公元 2021 年 9 月 5 日 星期日，上午 4:14:26
 * Copyright (c) 2021，淅琳雨
 *
 * 本程序是一个自由的软件，你可以重新分发它，可以魔改它，但要遵守 GPL 3.0 版本或者后续其它版本。
 * 我们希望本程序是有用的，但是我们不保证它能用，不保证它好用，我们不提供任何保证。
 * 更多请见 GPL 全文，如果理解不了，找人话版看看：https://zhuanlan.zhihu.com/p/185628074
 * 按道理你在得到本软件时，应该已经得到了一份 GPL，如果你没找到，写信给自由软件基金会 (FSF)：
 * 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 * 要是嫌运费或漫游费太贵就点击这里吧：https://www.gnu.org/licenses/
 *
 ***********************************************************************************************
 *
 * Otomad Helper for Vegas, which is designed to allow Vegas to accept MIDI files as input,
 * and automatically generate Otomad/YTPMV tracks.
 * Vegas 16 and above support all features, and Vegas 13 and above are compatible to run.
 * Place this script and all other attached files in the Script Menu folder in your Vegas installation directory.
 * See the documentation link for instructions.
 * The script is based on the original author Chaosinism's open source code redevelopment and uses the NAudio library.
 * Get the latest version here: https://github.com/otomad/VegasScripts/releases/latest
 * Project start date: Sun, Sep 5th 2021 A.D., 4:14:26 a.m.
 *
 * This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public
 * License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any
 * later version.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with this program; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 **/

#define VEGAS_ENVIRONMENT // Vegas 运行环境。取消定义后避免在 Visual Studio 中调试时因缺少环境而导致报错。
#define INTERNATIONALIZED // 启用国际化翻译操作。取消定义后可暂时禁用翻译操作并锁定为简体中文，如不需要翻译时也许可以加快脚本启动速度，但其实也没快多少。
#define PRODUCTION // 用于生产环境。定义后可以吞掉一些无关紧要的错误。取消定义后可以展现一些可能会产生隐患的错误。
// 以下宏定义为版本号标记。如您的软件本体版本号低于这些标记，应注释掉它们。注意若启用高版本号的标记，比它更低的版本号标记必须同时启用。
#define VER_GEQ_16 // Vegas 版本号大于或等于 16。定义后可正常使用调音算法属性等功能。
#define VER_GEQ_14 // Vegas 版本号大于或等于 14。定义后将依赖库切换到 Magix 版本。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using NAudio.Midi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
#if VER_GEQ_14
	using ScriptPortal.Vegas;
#else
	using Sony.Vegas;
#endif

namespace Otomad.VegasScript.OtomadHelper.V4 {

	public class EntryPoint {
		/// <summary>版本号</summary>
		public static readonly Version VERSION = new Version(4, 18, 4, 0);
		/// <summary>修订日期</summary>
		public static readonly DateTime REVISION_DATE = new DateTime(2022, 6, 4);

		// 配置参数变量
		#region 视频属性
		/* 启　　用 */ private bool VConfig { get { return configForm.VideoConfigCheck.Checked; } }
		/* 视觉效果 */ private PvVisualEffectType VConfigEffect { get { return configForm.VideoEffect; } }
		/* 拉　　伸 */ private bool VConfigScratch { get { return configForm.VideoScratchCheck.Checked; } }
		/* 循　　环 */ private bool VConfigLoop { get { return configForm.VideoLoopCheck.Checked; } }
		/* 定格首帧 */ private bool VConfigFreezeFirstFrame { get { return configForm.VideoFreezeFirstFrameCheck.Checked; } }
		/* 定格尾帧 */ private bool VConfigFreezeLastFrame { get { return configForm.VideoFreezeLastFrameCheck.Checked; } }
		/* 削除空隙 */ private bool VConfigLegato { get { return configForm.VideoLegatoCheck.Checked; } }
		/* 渐　　入 */ private int VConfigFadein { get { return configForm.VideoFadeInBox.Value; } }
		/* 渐　　出 */ private int VConfigFadeout { get { return configForm.VideoFadeOutBox.Value; } }
		/* 发　　光 */ private int VConfigGlow { get { return configForm.VideoGlowBox.Value; } }
		/* 发光亮度 */ private int VConfigGlowBright { get { return configForm.VideoGlowBrightBox.Value; } }
		/* 起始尺寸 */ private float VConfigStartSize { get { return configForm.VideoStartSizeBox.Value; } }
		/* 终止尺寸 */ private float VConfigEndSize { get { return configForm.VideoEndSizeBox.Value; } }
		/* 起始旋转 */ private float VConfigStartRotation { get { return (float)(configForm.VideoStartRotationBox.Value * Math.PI / 180); } }
		/* 终止旋转 */ private float VConfigEndRotation { get { return (float)(configForm.VideoEndRotationBox.Value * Math.PI / 180); } }
		/* 起始水平位移 */ private float VConfigStartHTrans { get { return configForm.VideoStartHorizontalTransBox.Value; } }
		/* 终止水平位移 */ private float VConfigEndHTrans { get { return configForm.VideoEndHorizontalTransBox.Value; } }
		/* 起始垂直位移 */ private float VConfigStartVTrans { get { return configForm.VideoStartVerticalTransBox.Value; } }
		/* 终止垂直位移 */ private float VConfigEndVTrans { get { return configForm.VideoEndVerticalTransBox.Value; } }
		/* 起始色相 */ private int VConfigStartHue { get { return configForm.VideoStartHueBox.Value; } }
		/* 终止色相 */ private int VConfigEndHue { get { return configForm.VideoEndHueBox.Value; } }
		/* 起始饱和 */ private int VConfigStartSaturation { get { return configForm.VideoStartSaturationBox.Value; } }
		/* 终止饱和 */ private int VConfigEndSaturation { get { return configForm.VideoEndSaturationBox.Value; } }
		/* 起始对比 */ private int VConfigStartContrast { get { return configForm.VideoStartContrastBox.Value; } }
		/* 终止对比 */ private int VConfigEndContrast { get { return configForm.VideoEndContrastBox.Value; } }
		/* 起始阈值 */ private int VConfigStartThreshold { get { return configForm.VideoStartThresholdBox.Value; } }
		/* 终止阈值 */ private int VConfigEndThreshold { get { return configForm.VideoEndThresholdBox.Value; } }
		/* 起始尺寸曲线 */ private VideoKeyframeType VConfigStartSizeCurve { get { return GetVideoKeyframeType(configForm.VideoStartSizeCurveCombo.SelectedIndex); } }
		/* 渐入曲线 */ private CurveType VConfigFadeinCurve { get { return GetCurveType(configForm.VideoFadeInCurveCombo.SelectedIndex); } }
		/* 渐出曲线 */ private CurveType VConfigFadeoutCurve { get { return GetCurveType(configForm.VideoFadeOutCurveCombo.SelectedIndex); } }
		/* 发光曲线 */ private OFXInterpolationType VConfigGlowCurve { get { return GetOFXInterpolationType(configForm.VideoGlowCurveCombo.SelectedIndex); } }
		/* 起始色相曲线 */ private OFXInterpolationType VConfigStartHueCurve { get { return GetOFXInterpolationType(configForm.VideoStartHueCurveCombo.SelectedIndex); } }
		/* 起始色相曲线 */ private OFXInterpolationType VConfigStartSaturationCurve { get { return GetOFXInterpolationType(configForm.VideoStartSaturationCurveCombo.SelectedIndex); } }
		/* 起始对比曲线 */ private OFXInterpolationType VConfigStartContrastCurve { get { return GetOFXInterpolationType(configForm.VideoStartContrastCurveCombo.SelectedIndex); } }
		/* 起始阈值曲线 */ private OFXInterpolationType VConfigStartThresholdCurve { get { return GetOFXInterpolationType(configForm.VideoStartThresholdCurveCombo.SelectedIndex); } }
		/* 视觉初值 */ private int VConfigInitialValue { get { return configForm.VideoEffectInitialValueCombo.SelectedIndex; } }
		/* 复音多轨 */ private bool VConfigMultitrackForChords { get { return configForm.VideoMultitrackForChordsCheck.Checked; } }
		#endregion

		#region 音频属性
		/* 启　　用 */ private bool AConfig { get { return configForm.AudioConfigCheck.Checked; } }
		/* 拉　　伸 */ private bool AConfigScratch { get { return configForm.AudioScratchCheck.Checked; } }
		/* 循　　环 */ private bool AConfigLoop { get { return configForm.AudioLoopCheck.Checked; } }
		/* 标准音量 */ private bool AConfigNormalize { get { return configForm.AudioNormalizeCheck.Checked; } }
		/* 定格尾帧 */ private bool AConfigFreezeLastFrame { get { return configForm.AudioFreezeLastFrameCheck.Checked; } }
		/* 连　　奏 */ private bool AConfigLegato { get { return configForm.AudioLegatoCheck.Checked; } }
		/* 渐　　入 */ private int AConfigFadein { get { return configForm.AudioFadeInBox.Value; } }
		/* 渐　　出 */ private int AConfigFadeout { get { return configForm.AudioFadeOutBox.Value; } }
		/* 渐入曲线 */ private CurveType AConfigFadeinCurve { get { return GetCurveType(configForm.AudioFadeInCurveCombo.SelectedIndex); } }
		/* 渐出曲线 */ private CurveType AConfigFadeoutCurve { get { return GetCurveType(configForm.AudioFadeOutCurveCombo.SelectedIndex); } }
		/* 原始音高 */ private int AConfigBasePitch { get { return configForm.BasePitch; } }
		/* 调音方法 */ private AudioTuneMethod AConfigMethod { get { return (AudioTuneMethod)configForm.AudioTuneMethodCombo.SelectedIndex; } }
		#if VER_GEQ_16
		/* 弹性属性 */ private ElastiqueStretchAttributes? AConfigElastiqueAttr { get { return AConfigMethod == AudioTuneMethod.ELASTIQUE ?
			(ElastiqueStretchAttributes?)configForm.AudioStretchAttrCombo.SelectedIndex : null; } }
		/* 古典属性 */ private ClassicStretchAttributes? AConfigClassicAttr { get { return AConfigMethod == AudioTuneMethod.CLASSIC ?
			(ClassicStretchAttributes?)configForm.AudioStretchAttrCombo.SelectedIndex : null; } }
		#endif
		/* 伸缩变调 */ private bool AConfigLockStretchPitch { get { return configForm.AudioLockStretchPitchCheck.Checked; } }
		/* 保留共振 */ private bool AConfigReserveFormant { get { return configForm.AudioReserveFormantCheck.Checked; } }
		/* 创建分组 */ private bool ConfigCreateEventGroup { get { return configForm.CreateEventGroupCheck.Checked; } }
		#endregion

		#region 迷笛属性
		/* 音　　轨 */ public MidiChannels MidiConfigTrack;
		/* 起始时间 */ private double MidiConfigStartTime { get { return configForm.MidiStartSecondBox.DoubleValue; } }
		/* 终止时间 */ private double MidiConfigEndTime { get { return configForm.MidiEndSecondBox.DoubleValue; } }
		/* 迷笛变速 */ private bool MidiUseDynamicMidiBpm { get { return configForm.MidiDynamicMidiBpmCheck.Checked; } }
		/* 迷笛速度 */ private bool MidiUseMidiBpm { get { return configForm.MidiMidiBpmCheck.Checked; } }
		/* 项目速度 */ private bool MidiUseProjectBpm { get { return configForm.MidiProjectBpmCheck.Checked; } }
		/* 自拟速度 */ private bool MidiUseCustomBpm { get { return configForm.MidiCustomBpmCheck.Checked; } }
		/* 迷笛变速形式 */ private int MidiUseDynamicMidiBpmForm { get { return configForm.MidiDynamicMidiBpmFormCombo.SelectedIndex; } }
		#endregion

		#region 媒体属性
		/* 起始时间 */
		private double SourceConfigStartTime { get { return configForm.SourceStartTimeText.DoubleValue; } }
		/* 终止时间 */ private double SourceConfigEndTime { get { return configForm.SourceStartTimeText.DoubleValue; } }
		/* 素材来源 */ private MediaSourceFrom SourceConfigFrom { get { return (MediaSourceFrom)configForm.ChooseSourceCombo.SelectedIndex; } }
		/* 生成位置 */ private GenerateAt GenerateAt { get { return configForm.GenerateAt; } }
		/* 自定生成位置 */ private Timecode GenerateAtCustomTimecode { get { return configForm.GenerateAtCustomTimecode; } }
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
		/* 音符偏移 */ private int SheetConfigShift { get { return (int)configForm.StaffNotesShiftBox.Value; } }
		/* 使用轨道运动方式放置音符位置 */ private bool UseLegacySheetMethod { get { return configForm.StaffLegacyEngineCheck.Checked; } }
		#endregion

		#region YTP 属性
		/* 启　　用 */ private bool YtpConfig { get { return configForm.IsGenerateYtp; } }
		/* 最小长度 */ private int YtpConfigMinLen { get { return (int)configForm.YtpMinLenBox.Value; } }
		/* 最大长度 */ private int YtpConfigMaxLen { get { return (int)configForm.YtpMaxLenBox.Value; } }
		/* 剪辑数目 */ private int YtpConfigClipsCount { get { return (int)configForm.YtpClipsCountBox.Value; } }
		/* 启用效果 */ private YtpEffectType[] YtpConfigEffects { get { return configForm.selectedYtpEffects; } }
		#endregion

		// 实例对象变量
		public ConfigForm configForm;
		public Vegas vegas;
		public ConfigIni configIni;
		private ProgressForm progressForm;
		internal static EntryPoint instance;
		private readonly List<VideoTrack> generatedVideoTracks = new List<VideoTrack>();

		// 媒体 / MIDI 参数变量
		internal MIDI midi = null;
		internal Media media = null;
		private double audioLength = 0;
		private double videoLength = 0;
		private bool IsFromSelectedMedia { get { return SourceConfigFrom == MediaSourceFrom.SELECTED_MEDIA; } }
		private bool IsFromSelectedClip { get { return SourceConfigFrom == MediaSourceFrom.SELECTED_CLIP; } }
		private bool IsFromBrowseFile { get { return SourceConfigFrom == MediaSourceFrom.BROWSE_FILE; } }
		private EventSet selectedEventSet = new EventSet();
		public double ProjectBpm { get { return vegas.Project.Ruler.BeatsPerMinute; } }
		private AutoLayoutTracksInfos LayoutInfos { get { return configForm.IsMultiMidiChannel ? configForm.layoutInfos : null; } }
		private bool IsMultiMidiChannel { get { return MidiConfigTrack.IsMultiMidiChannel; } }

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
		public static readonly int[] StaffPitchMap = { 0, 0, 1, 1, 2, 3, 3, 4, 4, 5, 5, 6 };

		/// <summary>
		/// 打开参数配置设置对话框。
		/// </summary>
		/// <returns>对话框最后选择的按钮是 <c>“完成” (true)</c> 还是 <c>“取消” (false)</c>。</returns>
		public bool ShowConfigForm() {
			requestRestartScript = false; // 取消请求重启脚本
			configForm = configForm ?? new ConfigForm(this);
			progressForm = new ProgressForm();
			#if SWALLOW_DIALOG_ERROR
			try {
				configForm.ShowDialog();
			} catch (Exception) { return false; }
			#else
			configForm.ShowDialog();
			#endif
			Type helper = configForm.RequestToShowHelperDialog;
			if (helper != null) {
				(Activator.CreateInstance(helper, new object[] { this }) as Form).ShowDialog();
				if (!configForm.CloseAfterOpenHelperCheck.Checked) requestRestartScript = true;
			}
			vegas.Transport.CursorPosition = configForm.originalCursorPosition;
			return configForm.AcceptConfig;
		}

		/// <summary>
		/// 打开选择 MIDI 文件对话框。
		/// </summary>
		/// <returns>打开的 MIDI 文件路径。如果点击取消则返回空字符串。</returns>
		internal string SelectMidiFile() {
			#region 选择一个 MIDI 文件
			OpenFileDialog openFileDialog = new OpenFileDialog {
				Filter = GetOpenFileDialogFilter(Lang.str.midi_file_name, "*.mid;*.midi", Lang.str.all_files, "*.*"),
				// RestoreDirectory = true,
				FilterIndex = 1,
				Title = Lang.str.choose_a_midi_file,
				InitialDirectory = lastMidiDirectory,
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK) return "";
			lastMidiDirectory = new Path(openFileDialog.FileName).Directory;
			return openFileDialog.FileName;
			#endregion
		}

		internal bool OpenMidiFile(string filePath, out MIDI outMidi, bool inSilence = false) {
			outMidi = null;
			if (string.IsNullOrWhiteSpace(filePath)) return false;
			MIDI _midi;
			try {
				_midi = new MIDI(filePath);
			} catch (Exception e) {
				if (!inSilence) ShowError(new Exceptions.NotAMidiFileException(), e);
				return false;
			}
			if (_midi.TrackInfos.Length == 0) {
				ShowError(new Exceptions.NoTrackInfoException());
				return false;
			}
			outMidi = midi = _midi;
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
		internal bool OpenMedia(string clipName) {
			Exception e;
			if (!TestMediaValid(clipName, out e, ref audioVideoEnabledTable.FromBrowseFile.AudioEnabled, ref audioVideoEnabledTable.FromBrowseFile.VideoEnabled)) {
				ShowError(new Exceptions.NoMediaTakeException(), e);
				return false;
			}
			try {
				vegas.ImportFile(clipName, true);
			} catch (Exception ee) { ShowError(new Exceptions.NoMediaTakeException(), ee); return false; }
			Media media = vegas.Project.MediaPool.Find(clipName);
			this.media = media;
			audioLength = videoLength = media.Length.ToMilliseconds();
			return true;
		}

		/// <summary>
		/// 测试媒体文件是否合法。
		/// </summary>
		/// <param name="clipName">媒体文件路径。</param>
		/// <param name="exception">如果失败的报错信息。</param>
		/// <param name="hasAudio">是否包含音频部分。</param>
		/// <param name="hasVideo">是否包含视频部分。</param>
		/// <returns>媒体是否合法的布尔值。</returns>
		public bool TestMediaValid(string clipName, out Exception exception, ref bool hasAudio, ref bool hasVideo) {
			int originalMediaCount = vegas.Project.MediaPool.Count;
			bool valid = true;
			exception = null;
			try {
				vegas.ImportFile(clipName, true);
			} catch (Exception e) {
				valid = false;
				exception = e;
			}
			if (valid) {
				Media media = vegas.Project.MediaPool.Find(clipName);
				try {
					double length = media.Length.ToMilliseconds();
					hasAudio = media.HasAudio();
					hasVideo = media.HasVideo();
				} catch (Exception e) {
					valid = false;
					exception = e;
				}
			}
			bool isNewFile = originalMediaCount != vegas.Project.MediaPool.Count;
			if (isNewFile) vegas.Project.MediaPool.Remove(clipName);
			return valid;
		}
		/// <summary>
		/// 测试媒体文件是否合法。
		/// </summary>
		/// <param name="clipName">媒体文件路径。</param>
		/// <param name="hasAudio">是否包含音频部分。</param>
		/// <param name="hasVideo">是否包含视频部分。</param>
		/// <returns>媒体是否合法的布尔值。</returns>
		public bool TestMediaValid(string clipName, ref bool hasAudio, ref bool hasVideo) {
			Exception _;
			return TestMediaValid(clipName, out _, ref hasAudio, ref hasVideo);
		}

		/// <summary>
		/// 打开选择媒体文件对话框。
		/// </summary>
		/// <returns>选定文件的路径。如果点击取消则返回空字符串。</returns>
		internal string SelectVideoClip() {
			#region 选择一个视频剪辑
			OpenFileDialog openFileDialog = new OpenFileDialog {
				// InitialDirectory = @"C:\",
				Filter = GetOpenFileDialogFilter(Lang.str.media_file_name, MEDIA_EXTENSION, Lang.str.all_files, "*.*"),
				// openFileDialog.RestoreDirectory = true,
				FilterIndex = 1,
				Title = Lang.str.choose_a_source_file,
				InitialDirectory = lastMediaDirectory
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK) return "";
			string clipName = openFileDialog.FileName;
			lastMediaDirectory = new Path(clipName).Directory;
			#endregion
			return SelectVideoClip(clipName);
		}

		internal string SelectVideoClip(string clipName) {
			Exception e;
			if (!TestMediaValid(clipName, out e, ref audioVideoEnabledTable.FromBrowseFile.AudioEnabled, ref audioVideoEnabledTable.FromBrowseFile.VideoEnabled)) {
				ShowError(new Exceptions.NoMediaTakeException(), e);
				return "";
			}
			return clipName;
		}

		/// <summary>
		/// 获取文件打开对话框的文件格式筛选值。
		/// </summary>
		/// <param name="exts">类型名称与类型扩展名交替填写，参数数目必须为 2 的倍数。如果扩展名有多个，以分号隔开。</param>
		/// <returns>文件格式筛选值</returns>
		private static string GetOpenFileDialogFilter(params string[] exts) {
			return string.Join("|", exts);
		}

		internal const string MEDIA_EXTENSION =
			"*.veg;*.mp4;*.mpg;*.jpg;*.avi;*.aaf;*.cda;*.aa3;*.oma;*.aif;*.aiff;*.snd;*.vox;*.flac;*.gif;*.mov;*.m2t;*.ts;" +
			"*.mts;*.m2ts;*.jpe;*.jpeg;*.mkv;*.avc;*.bsf;*.264;*.mpeg;*.mmv;*.m1p;*.m1a;*.m2p;*.mp3;*.au;*.ogg;*.png;*.qt;" +
			"*.dv;*.tif;*.tiff;*.wav;*.sfa;*.dlx;*.mxf;*.pca;*.w64;*.dig;*.sd;*.tga;*.targa;*.bmp;*.dib;*.wma;*.wmv;*.asf;" +
			"*.ico;*.rle;*.psd;*.icon";

		/// <summary>
		/// 显示报错信息。
		/// </summary>
		public static void ShowError(string str, ShowErrorState state = ShowErrorState.NORMAL) {
			instance.vegas.ShowError(str);
			DoingAfterShowError(state);
		}
		public static void ShowError(Exception e, ShowErrorState state = ShowErrorState.NORMAL) {
			instance.vegas.ShowError(e.Message);
			DoingAfterShowError(state);
		}
		public static void ShowError(Exception e1, Exception e2, ShowErrorState state = ShowErrorState.NORMAL) {
			instance.vegas.ShowError(e1.Message, e2.ToString());
			DoingAfterShowError(state);
		}
		public static void ShowError2(string str) {
			MessageBox.Show(str, Lang.str.error, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		public static void ShowError2(Exception e) {
			ShowError2(e.ToString());
		}
		public static void ShowError2(Exception e1, Exception e2) {
			ShowError2(e1.Message + "\n\n" + Lang.str.details + "\n" + e2.ToString());
		}
		private string GetExceptionInfo(Exception e) {
			return e.Message + "\n" + e.StackTrace + "\n" + e.ToString();
		}
		private static void DoingAfterShowError(ShowErrorState state) {
			if (state == ShowErrorState.SILENCE) return;
			if (instance.progressForm != null) instance.progressForm.Close();
			if (state == ShowErrorState.NORMAL) {
				if (instance.configForm != null) instance.configForm.FocusOn(null, null);
				instance.configIni.Delete(true);
			}
		}

		public class AudioVideoEnabledTable {
			public struct AudioVideoEnabledGroup {
				public bool AudioEnabled;
				public bool VideoEnabled;
			}
			public AudioVideoEnabledGroup FromSelectedMedia;
			public AudioVideoEnabledGroup FromSelectedClip;
			public AudioVideoEnabledGroup FromBrowseFile;
			public bool SelectNoMedia { get { return !FromSelectedMedia.AudioEnabled && !FromSelectedMedia.VideoEnabled; } }
			public bool SelectNoEvents { get { return !FromSelectedClip.AudioEnabled && !FromSelectedClip.VideoEnabled; } }
			public string SelectMediaName = "";
			public string SelectClipName = "";
			public AudioVideoEnabledTable(EntryPoint p) {
				Media[] selectedMedia = p.vegas.Project.MediaPool.GetSelectedMedia();
				if (selectedMedia.Length != 0) {
					Media media = selectedMedia[0];
					FromSelectedMedia.AudioEnabled = media.HasAudio();
					FromSelectedMedia.VideoEnabled = media.HasVideo();
					SelectMediaName = new Path(media.FilePath).FullFileName;
				}
				p.GetSelectedEventSet();
				FromSelectedClip.AudioEnabled = p.selectedEventSet.audioEvent != null;
				FromSelectedClip.VideoEnabled = p.selectedEventSet.videoEvent != null;
				int differ = ((FromSelectedClip.AudioEnabled ? 1 : 0) + (FromSelectedClip.VideoEnabled ? 1 : 0))
					- ((FromSelectedMedia.AudioEnabled ? 1 : 0) + (FromSelectedMedia.VideoEnabled ? 1 : 0));
				p.SuggestSelectedSourceFrom = // 下方阐述中“拥有轨道种类数目”乃拟定用词，如既拥有视频轨又拥有音轨则为 2，仅拥有视频轨或音轨之一则为 1，如果两个轨道均没有则为 0．
					differ > 0 ? MediaSourceFrom.SELECTED_CLIP : // 如果选定轨道素材中拥有轨道种类数目大于选定项目媒体素材中拥有轨道种类数目，则在素材来源设置中默认选中使用“选中的轨道素材”。
					differ < 0 ? MediaSourceFrom.SELECTED_MEDIA : // 如果选定轨道素材中拥有轨道种类数目小于选定项目媒体素材中拥有轨道种类数目，则在素材来源设置中默认选中使用“选中的媒体文件”。
					MediaSourceFrom.LAST_USER_PREFERENCE; // 如果选定轨道素材中拥有轨道种类数目等于选定项目媒体素材中拥有轨道种类数目，则由用户上一次选择的选项决定。
				if (!FromSelectedClip.AudioEnabled && !FromSelectedClip.VideoEnabled &&
					!FromSelectedMedia.AudioEnabled && !FromSelectedMedia.VideoEnabled)
					p.SuggestSelectedSourceFrom = MediaSourceFrom.NOTHING_SELECTED; // 如果两者都没有选择任何素材。
				List<string> ClipNames = new List<string>();
				if (p.selectedEventSet.videoEvent != null && p.selectedEventSet.videoEvent.ActiveTake != null) ClipNames.Add(p.selectedEventSet.videoEvent.ActiveTake.Name);
				if (p.selectedEventSet.audioEvent != null && p.selectedEventSet.audioEvent.ActiveTake != null) ClipNames.Add(p.selectedEventSet.audioEvent.ActiveTake.Name);
				if (ClipNames.Count >= 2 && ClipNames[0] == ClipNames[1]) ClipNames.RemoveAt(1);
				SelectClipName = string.Join(" + ", ClipNames);
			}
		}
		public MediaSourceFrom SuggestSelectedSourceFrom = MediaSourceFrom.LAST_USER_PREFERENCE;
		public AudioVideoEnabledTable audioVideoEnabledTable;

		public static CurveType GetCurveType(int index) {
			return new CurveType[] { CurveType.Linear, CurveType.Fast, CurveType.Slow, CurveType.Smooth, CurveType.Sharp, CurveType.None }[index];
		}
		public static VideoKeyframeType GetVideoKeyframeType(int index) {
			return new VideoKeyframeType[] { VideoKeyframeType.Linear, VideoKeyframeType.Fast, VideoKeyframeType.Slow, VideoKeyframeType.Smooth, VideoKeyframeType.Sharp, VideoKeyframeType.Hold }[index];
		}
		public static OFXInterpolationType GetOFXInterpolationType(int index) {
			return new OFXInterpolationType[] { OFXInterpolationType.Linear, OFXInterpolationType.Fast, OFXInterpolationType.Slow, OFXInterpolationType.Smooth, OFXInterpolationType.Sharp, OFXInterpolationType.Hold }[index];
		}

		/// <summary>
		/// 分别包含音视频事件的集合。
		/// </summary>
		public class EventSet {
			public AudioEvent audioEvent = null;
			public VideoEvent videoEvent = null;
			public double audioLength { get { return audioEvent == null ? 0 : audioEvent.Length.ToMilliseconds(); } }
			public double videoLength { get { return videoEvent == null ? 0 : videoEvent.Length.ToMilliseconds(); } }
			public Subclip audioReverse = null;
			public Subclip videoReverse = null;
			/// <summary>
			/// 初始化一个空的集合。
			/// </summary>
			public EventSet() { }
			/// <summary>
			/// 初始化一个仅包含音频事件与视频事件的集合/
			/// </summary>
			/// <param name="audioEvent">音频事件</param>
			/// <param name="videoEvent">视频事件</param>
			public EventSet(AudioEvent audioEvent, VideoEvent videoEvent) {
				this.audioEvent = audioEvent;
				this.videoEvent = videoEvent;
			}
			/// <summary>
			/// 初始化一个包含音频事件、视频事件、音频倒放子剪辑和视频倒放子剪辑的集合。
			/// </summary>
			/// <param name="audioEvent">音频事件</param>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="audioReverse">音频倒放子剪辑</param>
			/// <param name="videoReverse">视频倒放子剪辑</param>
			public EventSet(AudioEvent audioEvent, VideoEvent videoEvent, Subclip audioReverse, Subclip videoReverse) : this(audioEvent, videoEvent) {
				this.audioReverse = audioReverse;
				this.videoReverse = videoReverse;
			}
			public override bool Equals(object obj) {
				if (object.ReferenceEquals(this, obj)) return true;
				if (obj == null || !(obj is EventSet)) return false;
				EventSet other = obj as EventSet;
				return other.audioEvent == audioEvent && other.videoEvent == videoEvent;
			}
			public override int GetHashCode() {
				return audioEvent.GetHashCode() ^ videoEvent.GetHashCode();
			}
			/// <summary>
			/// 在轨道事件集合数组中消除重复的轨道事件集合对象。
			/// </summary>
			/// <param name="eventSets">轨道事件集合对象数组。</param>
			/// <returns>原轨道事件集合对象数组，只是处理过了。</returns>
			public static EventSet[] EliminateDuplicates(ref EventSet[] eventSets) {
				List<EventSet> sets = new List<EventSet>();
				for (int i = 0; i < eventSets.Length; i++) {
					EventSet current = eventSets[i];
					bool same = false;
					for (int j = 0; j < i; j++) {
						EventSet contrast = eventSets[j];
						if (current.Equals(contrast)) {
							same = true;
							break;
						}
					}
					if (!same) sets.Add(current);
				}
				if (sets.Count == 0) ShowError(new Exceptions.YtpEliminateDuplicatesFinallyNullException(), ShowErrorState.RESUME_NEXT);
				return eventSets = sets.ToArray();
			}
			/// <summary>
			/// 根据给定的一个轨道事件数组，获取第一个满足条件的音频事件与视频事件。<br />
			/// 可以单独指定是否包含其中轨道事件分组中的相关对立事件。
			/// </summary>
			/// <param name="events">轨道事件数组。</param>
			/// <param name="useEventGroup">如果为 true，则若在给定的轨道事件数组中不能同时找到音频事件和视频事件，
			/// 将会尝试在已找到的类型的轨道事件的分组中寻找缺失的轨道事件。</param>
			public EventSet(TrackEvent[] events, bool useEventGroup = true) {
				foreach (TrackEvent trackEvent in events) {
					if (trackEvent != null && trackEvent.IsAudio() && audioEvent == null)
						audioEvent = trackEvent as AudioEvent;
					if (trackEvent != null && trackEvent.IsVideo() && videoEvent == null)
						videoEvent = trackEvent as VideoEvent;
				}
				if (useEventGroup) {
					if (audioEvent != null && videoEvent == null && audioEvent.Group != null) {
						foreach (TrackEvent trackEvent in audioEvent.Group)
							if (trackEvent != null && trackEvent.IsVideo()) {
								videoEvent = trackEvent as VideoEvent;
								break;
							}
					} else if (audioEvent == null && videoEvent != null && videoEvent.Group != null) {
						foreach (TrackEvent trackEvent in videoEvent.Group) {
							if (trackEvent != null && trackEvent.IsAudio()) {
								audioEvent = trackEvent as AudioEvent;
								break;
							}
						}
					}
				}
			}
			/// <summary>
			/// 是否包含倒放子剪辑。
			/// </summary>
			public bool IncludeReverseSubClip {
				get {
					return audioReverse != null || videoReverse != null;
				}
			}
		}

		/// <summary>
		/// 获取所有选中的轨道剪辑。
		/// </summary>
		/// <returns>选中的轨道剪辑数组。</returns>
		public T[] GetSelectedEvents<T>() where T : TrackEvent {
			List<T> selectedList = new List<T>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected && trackEvent is T)
						selectedList.Add(trackEvent as T);
			return selectedList.ToArray();
		}
		public VideoEvent[] GetSelectedVideoEvents() {
			return GetSelectedEvents<VideoEvent>();
		}
		public AudioEvent[] GetSelectedAudioEvents() {
			return GetSelectedEvents<AudioEvent>();
		}
		public TrackEvent[] GetSelectedEvents() {
			return GetSelectedEvents<TrackEvent>();
		}

		/// <summary>
		/// 获取选中的第一个轨道剪辑。
		/// </summary>
		/// <returns>选中的第一个轨道剪辑。</returns>
		public TrackEvent GetSelectedFirstEvents() {
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected && trackEvent.ActiveTake != null && trackEvent.ActiveTake.Media != null)
						return trackEvent;
			return null;
		}

		/// <summary>
		/// 获取所有选中的轨道。
		/// </summary>
		/// <typeparam name="T">轨道种类</typeparam>
		/// <returns>选中的轨道</returns>
		public T[] GetSelectedTracks<T>() where T : Track {
			List<T> selectedList = new List<T>();
			foreach (Track track in vegas.Project.Tracks)
				if (track.Selected && track is T)
					selectedList.Add(track as T);
			return selectedList.ToArray();
		}

		/// <summary>
		/// 获取所有选中的视频轨道。
		/// </summary>
		/// <returns>选中的视频轨道</returns>
		public VideoTrack[] GetSelectedVideoTracks() {
			return GetSelectedTracks<VideoTrack>();
		}
		public AudioTrack[] GetSelectedAudioTracks() {
			return GetSelectedTracks<AudioTrack>();
		}
		public Track[] GetSelectedTracks() {
			return GetSelectedTracks<Track>();
		}

		public Media[] GetSelectedMedia() {
			return vegas.Project.MediaPool.GetSelectedMedia();
		}

		/// <summary>
		/// 获取选中的轨道事件所满足条件的第一组轨道事件集合。
		/// </summary>
		/// <returns>选中的轨道事件所满足条件的第一组轨道事件集合。</returns>
		private EventSet GetSelectedEventSet() {
			return selectedEventSet = new EventSet(GetSelectedEvents(), true);
		}

		public struct CountEventsType {
			public int audioCount;
			public int videoCount;
			public int count;
			public CountEventsType(TrackEvent[] events) {
				audioCount = 0;
				videoCount = 0;
				count = events.Length;
				foreach (TrackEvent trackEvent in events) {
					if (trackEvent.IsAudio()) audioCount++;
					if (trackEvent.IsVideo()) videoCount++;
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
		/// 获取所有轨道剪辑。
		/// </summary>
		/// <typeparam name="T">轨道剪辑类型。</typeparam>
		/// <param name="excludeEmptyTake">不包含空片段的轨道事件。<br />
		/// 如果为 <c>true</c>，则若一个轨道事件不包含任何片段和媒体，则不会将其添加到结果中。</param>
		/// <returns>所有轨道剪辑。</returns>
		public T[] GetAllEvents<T>(bool excludeEmptyTake = true) where T : TrackEvent {
			List<T> events = new List<T>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent is T)
						events.Add(trackEvent as T);
			return events.ToArray();
		}

		/// <summary>
		/// 获取所有轨道剪辑。
		/// </summary>
		/// <returns>所有轨道剪辑。</returns>
		public TrackEvent[] GetAllEvents() {
			return GetAllEvents<TrackEvent>();
		}

		/// <summary>
		/// 查找与指定活动片段名称相匹配的所有轨道剪辑。
		/// </summary>
		/// <typeparam name="T">轨道剪辑类型。</typeparam>
		/// <param name="name">指定的活动片段名称。</param>
		/// <returns>与指定名称相匹配的所有轨道剪辑。</returns>
		public T[] GetEventsByTakeName<T>(string name) where T : TrackEvent {
			List<T> events = new List<T>();
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent is T && trackEvent.ActiveTake != null && trackEvent.ActiveTake.Name == name)
						events.Add(trackEvent as T);
			return events.ToArray();
		}

		/// <summary>
		/// 查找与指定活动片段名称相匹配的所有轨道剪辑。
		/// </summary>
		/// <param name="name">指定的活动片段名称。</param>
		/// <returns>与指定名称相匹配的所有轨道剪辑。</returns>
		public TrackEvent[] GetEventsByTakeName(string name) {
			return GetEventsByTakeName<TrackEvent>(name);
		}

		/// <summary>
		/// 取消选中轨道素材。
		/// </summary>
		public void SelectNoneEvents() {
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					trackEvent.Selected = false;
		}

		/// <summary>
		/// 给定一组轨道剪辑数组，将其中每一个轨道剪辑的分组（如果有）中的其它轨道剪辑也加在数组内。<br />
		/// 数组内不会包含重复的数据。<br />
		/// 不会更改原数组，将会返回一个新的数组。
		/// </summary>
		/// <param name="originalEvents">原始轨道剪辑数组。</param>
		/// <returns>追加分组内其它轨道剪辑的数组。</returns>
		public TrackEvent[] AppendGroupedEvents(TrackEvent[] originalEvents) {
			List<TrackEvent> events = new List<TrackEvent>();
			foreach (TrackEvent trackEvent in originalEvents)
				if (!events.Contains(trackEvent)) {
					events.Add(trackEvent);
					if (trackEvent.IsGrouped)
						foreach (TrackEvent trackEventInGroup in trackEvent.Group)
							if (!events.Contains(trackEventInGroup))
								events.Add(trackEventInGroup);
				}
			return events.ToArray();
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

		/// <summary>
		/// 定格视频事件末帧，防止素材播放到超过其规定长度范围的部分。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <param name="duration">规定的长度</param>
		/// <returns>原视频事件</returns>
		public VideoEvent FreezeLastFrame(VideoEvent videoEvent, double duration) {
			Timecode length = Timecode.FromMilliseconds(duration);
			if (!VConfigLegato) if (length > videoEvent.Length) return videoEvent; // 该功能会和“填补间隙”功能冲突
			Envelope velocity = videoEvent.Envelopes.FindByType(EnvelopeType.Velocity) ?? new Envelope(EnvelopeType.Velocity);
			if (!videoEvent.Envelopes.HasEnvelope(EnvelopeType.Velocity)) videoEvent.Envelopes.Add(velocity);
			velocity.Points[0].Curve = CurveType.None;
			EnvelopePoint point = velocity.Points.GetPointAtX(length);
			if (point == null) velocity.Points.Add(new EnvelopePoint(length, 0, CurveType.None));
			else {
				point.Y = 0;
				point.Curve = CurveType.None;
			}
			return videoEvent;
		}

		public void AdjustDeviation(TrackEvent trackEvent, double startTime, double endTime) {
			Timecode start = trackEvent.Start, length = trackEvent.Length;
			trackEvent.AdjustStartLength(start + Timecode.FromMilliseconds(startTime), Timecode.FromMilliseconds(endTime - startTime), true);
			trackEvent.Start = start;
			trackEvent.Length = length;
		}

		public bool PutPreviewAudioEvent(AudioTrack track, Timecode position) {
			AudioEvent _;
			return PutPreviewAudioEvent(track, position, out _);
		}
		public bool PutPreviewAudioEvent(AudioTrack track, Timecode position, out AudioEvent audioEventSample) {
			audioEventSample = null;
			if (!IsFromBrowseFile) { bool ok = GetSelectedSource(); if (!ok) return false; }
			if (!IsFromSelectedClip) {
				audioEventSample = track.AddAudioEvent(
					position,
					Timecode.FromMilliseconds(audioLength)
				);
				try {
					audioEventSample.AddTake(media.GetAudioStreamByIndex(0));
				} catch (Exception) { /*ShowError(new Exceptions.NoAudioTakeException(), e);*/ return false; }
			} else {
				if (selectedEventSet.audioEvent == null) { /*ShowError(new Exceptions.NoAudioTakeException());*/ return false; }
				audioEventSample = selectedEventSet.audioEvent.Copy(track, position) as AudioEvent;
			}
			if (AConfigNormalize) audioEventSample.Normalize = true;
			return true;
		}

		/// <summary>
		/// 生成音声 Music Anime Dōga / YouTube Poop Music Video。
		/// </summary>
		private bool GenerateOtomad() {
			#region 验证数据合法
			if (!AConfig && !VConfig) return false;
			if (!IsFromBrowseFile) { bool ok = GetSelectedSource(); if (!ok) return false; }
			if (media == null && !IsFromSelectedClip) { ShowError(new Exceptions.NoMediaException()); return false; }
			if (!YtpConfig) {
				if (MidiConfigTrack == null || MidiConfigTrack.Count == 0) { ShowError(new Exceptions.NoTrackInfoException()); return false; }
				if (midi == null) { ShowError(new Exceptions.NoMidiException()); return false; }
				if (midi.TrackInfos == null) { ShowError(new Exceptions.NoTrackInfoException()); return false; }
				if (!MidiUseMidiBpm && !MidiUseDynamicMidiBpm)
					midi.Bpm = MidiUseCustomBpm ? (double)configForm.MidiCustomBpmBox.Value : ProjectBpm;
			}
			Plugin.Init(vegas);
			bool requestShowProgress = false;
			long startMakingTime = DateTime.Now.Ticks; // 单位：100 纳秒
			const long MUST_SHOW_PROGRESS_WAITING_TIME = 10000000L; // 1 秒
			if (progressForm.IsDisposed) // 这里有一个特别蛇皮的 bug，就是如果手动打开一个不受 Vegas 支持的媒体文件，触发打不开媒体文件的报错。然后再打开一个 Vegas 支持的媒体文件，最后点击生成。这竟然会导致进度条对话框被销毁且不为 null 的奇葩问题，而且因果毫无任何关系。反正这样可以解决问题就行了。
				progressForm = new ProgressForm();
			progressForm.Show();
			if (YtpConfig) { GenerateYtp(); return true; }
			if (AConfig && AConfigMethod == AudioTuneMethod.PITCH_SHIFT) if (!ExaminePitchShiftPresetsExist()) return false;
			progressForm.Info = "";
			#endregion
			#region 开始处理 MIDI
			string name = MidiConfigTrack[0].Name; // 所选 MIDI 轨道名称。如果没有则为空串。

			#region 视频操作
			const int MAX_VIDEO_TRACK_SIZE = 100;
			int tTrackCount = 0; // 总轨道计数，用于新建音频轨道。
			VideoTrack vTrack = null; // 如果不启用五线谱时用的视频轨道。
			VideoTrack[] vTracks = null; // 如果启用五线谱时用的视频轨道列表。
			bool needTwoKey = VConfigStartSize != VConfigEndSize || // 如果为起始尺寸与终止尺寸大小相等，则没有必要打两个关键帧了。
				VConfigStartRotation != VConfigEndRotation ||
				VConfigStartHTrans != VConfigEndHTrans ||
				VConfigStartVTrans != VConfigEndVTrans;
			PvVisualEffect anime = new PvVisualEffect(VConfigEffect, VConfigInitialValue);
			PvVisualEffect[] anims = null;
			#if !VER_GEQ_16
			if (anime.fx >= PvVisualEffectType.VERTICAL_EXPANSION && anime.fx <= PvVisualEffectType.PUYO_PUYO) {
				ShowError(new Exceptions.UsePicInPicOnUnsupportedVegasException(), ShowErrorState.RESUME_NEXT);
				return;
			}
			#endif
			double lastStartTime = -1;
			bool multitrack = SheetConfig || VConfigMultitrackForChords;
			if (VConfig && !multitrack) vegas.Project.Tracks.Add(vTrack = new VideoTrack(vegas.Project, tTrackCount++, name));
			else if (multitrack) {
				vTracks = new VideoTrack[MAX_VIDEO_TRACK_SIZE];
				anims = new PvVisualEffect[MAX_VIDEO_TRACK_SIZE];
			}
			#endregion

			#region 音频操作
			const int MAX_AUDIO_TRACK_SIZE = 100;
			AudioTrack[] aTracks = null; // 音频轨道列表
			double[] aTrackPositions = null; // 音轨轨道长度计数列表
			int aTrackCount = 1; // 音频轨道计数，用于新建音频轨道。由于如果不生成音频也不会使用这个变量，因此初始化为 1 没有问题。
			if (AConfig) {
				aTracks = new AudioTrack[MAX_AUDIO_TRACK_SIZE];
				aTrackPositions = new double[MAX_AUDIO_TRACK_SIZE];
				vegas.Project.Tracks.Add(aTracks[0] = new AudioTrack(vegas.Project, tTrackCount++, name));
				aTrackPositions[0] = 0;
			}
			TempEventGroup tempEventGroup = new TempEventGroup(this); // 事件分组临时列表。
			#endregion

			#region 如果修改了素材的入点和出点的时间
			double sourceStartTime = configForm.SourceStartTimeText.Value, sourceEndTime = configForm.SourceEndTimeText.Value;
			bool adjustTime = sourceStartTime != 0 || sourceEndTime != 0;
			if (adjustTime) {
				while (sourceEndTime <= sourceStartTime) sourceEndTime += Math.Max(audioLength, videoLength);
				audioLength = videoLength = sourceEndTime - sourceStartTime;
			}
			double generateBeginTime = GenerateAt == GenerateAt.CUSTOM ? GenerateAtCustomTimecode.ToMilliseconds() :
				GenerateAt == GenerateAt.CURSOR ? vegas.Transport.CursorPosition.ToMilliseconds() : 0;
			double songLength = 0; // 指定乐曲总长
			#endregion

			#region 五线谱操作
			const int DEFAULT_MIDI_CONFIG_BEAT = 4;
			int vTrackCount = -1; // 视频轨道计数，用于新建视频轨道。仅在启用五线谱效果时使用。
			int trackPointer = 0; // 视频轨道指针
			double barStartTime = 0;
			double barLength = midi.MsPerQuarter * DEFAULT_MIDI_CONFIG_BEAT;
			bool sliceComposition = MidiConfigStartTime < MidiConfigEndTime;
			int projWidth = SheetConfigRelative ? 1920 : vegas.Project.Video.Width;
			int projHeight = SheetConfigRelative ? 1080 : vegas.Project.Video.Height;

			Func<VideoTrack> NewVideoTrack = new Func<VideoTrack>(() => {
				tTrackCount++;
				vegas.Project.Tracks.Add(vTracks[++vTrackCount] = new VideoTrack(vegas.Project, 0, name));
				anims[vTrackCount] = new PvVisualEffect(VConfigEffect, VConfigInitialValue);
				return vTracks[vTrackCount];
			});
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
					} catch (Exception e) { ShowError(new Exceptions.NoAudioTakeException(), e); return false; }
				} else {
					if (selectedEventSet.audioEvent == null) { ShowError(new Exceptions.NoAudioTakeException()); return false; }
					audioEventSample = selectedEventSet.audioEvent.Copy(aTracks[0], Timecode.FromMilliseconds(0)) as AudioEvent;
				}
				if (AConfigNormalize) { // 将添加音频单独提取到循环之外有助于提高规范化音频的速度
					audioEventSample.RecalculateNorm();
					audioEventSample.Normalize = true;
				}
			}
			#endregion

			#region 动态 BPM 处理
			DynamicBpmIntegrator integrator = null;
			if (MidiUseDynamicMidiBpm) integrator = new DynamicBpmIntegrator(midi, MidiUseDynamicMidiBpmForm == 1);
			#endregion
			for (int i = 0; i < MidiConfigTrack[0].Events.Count; i++) {
				MidiEvent midiEvent = MidiConfigTrack[0].Events[i];
				if (!(midiEvent is NoteOnEvent)) continue;
				double statusProgress = Math.Round(100.0 * i / MidiConfigTrack[0].Events.Count);
				if (IsMultiMidiChannel) statusProgress = MidiConfigTrack.GetPercent(statusProgress);
				long curTime = DateTime.Now.Ticks;
				if (!requestShowProgress && curTime - startMakingTime > MUST_SHOW_PROGRESS_WAITING_TIME)
					requestShowProgress = true; // 超过规定等待的时间，则还是会显示进度条
				if (AConfig && AConfigMethod == AudioTuneMethod.PITCH_SHIFT || requestShowProgress) { // 如果不是使用“移调”效果插件，就不要刷进度条，否则还会额外拖延时间。
					progressForm.ReportProgress(statusProgress); // 说明：只有在使用“移调”效果插件时才会生成得很慢，其它情况下都是非常快的。
					if (progressForm.RealTimeUpdateCheck.Checked)
						vegas.UpdateUI(); // 可以让 Vegas 实时更新 UI，但是会更慢。
				}
				if (progressForm.RequestAbort) break;
				if (SheetConfig)
					foreach (MidiEvent _midiEvent in midi.TimeSignatureTrack)
						if (_midiEvent is TimeSignatureEvent) {
							TimeSignatureEvent timeSignatureEvent = _midiEvent as TimeSignatureEvent;
							if (midiEvent.AbsoluteTime >= timeSignatureEvent.AbsoluteTime)
								barLength = !MidiUseDynamicMidiBpm
									? midi.MsPerQuarter * timeSignatureEvent.Numerator
									: integrator.GetActualTime(timeSignatureEvent.Numerator);
						}
				NoteEvent noteEvent = midiEvent as NoteEvent;
				NoteOnEvent noteOnEvent = midiEvent as NoteOnEvent;
				double startTime, duration;
				if (!MidiUseDynamicMidiBpm) {
					startTime = midiEvent.AbsoluteTime * midi.MsPerQuarter / midi.TicksPerQuarter;
					duration = noteOnEvent.NoteLength * midi.MsPerQuarter / midi.TicksPerQuarter;
				} else {
					Tuple<double, double> _ = integrator.GetActualTime(midiEvent.AbsoluteTime, noteOnEvent.NoteLength);
					startTime = _.Item1;
					duration = _.Item2;
				}
				int pitch = noteEvent.NoteNumber;
				int _pitch = pitch + SheetConfigShift; // 五线谱音符偏移量。
				int trackIndex = 0;

				if (startTime < MidiConfigStartTime) continue;
				if (startTime > MidiConfigEndTime && sliceComposition) break;
				songLength = startTime + duration;
				#region 下一页
				while (Math.Ceiling(startTime) >= barStartTime + barLength) {
					barStartTime += barLength;
					trackPointer = 0;
				}
				#endregion

				#region 生成音频事件
				if (AConfig) {
					while (Math.Ceiling(startTime) < aTrackPositions[trackIndex]) // 如果音频是多轨则放到新建的轨道，虽然有时候判断不准确，但问题不大。 // 后来改用整数加以限制，效果就好很多了。
						if (++trackIndex == aTrackCount) {
							aTrackCount++;
							vegas.Project.Tracks.Add(aTracks[trackIndex] = new AudioTrack(vegas.Project, tTrackCount++, name));
						}
					AudioEvent audioEvent = audioEventSample.Copy(aTracks[trackIndex], Timecode.FromMilliseconds(generateBeginTime + startTime)) as AudioEvent;
					audioEvent.Length = Timecode.FromMilliseconds(duration);
					if (AConfigFreezeLastFrame && !AConfigScratch && duration > audioLength) audioEvent.Length = Timecode.FromMilliseconds(audioLength);
					aTrackPositions[trackIndex] = startTime + duration;
					try {
						#if VER_GEQ_16
							audioEvent.Method = AConfigMethod == AudioTuneMethod.CLASSIC ? TimeStretchPitchShift.Classic : TimeStretchPitchShift.Elastique; // 这个操作没有在 Vegas 文档中写到。
							audioEvent.PitchLock = false;
						#endif
					} catch (Exception e) {
						if (AConfigMethod == AudioTuneMethod.ELASTIQUE || AConfigMethod == AudioTuneMethod.CLASSIC) {
							ShowError(new Exceptions.NoTimeStretchPitchShiftException(), e); return false;
						}
					}
					if (adjustTime) AdjustDeviation(audioEvent, sourceStartTime, sourceEndTime);
					if (AConfigScratch) audioEvent.AdjustPlaybackRate(audioLength / duration, true);
					audioEvent.Loop = AConfigLoop;

					audioEvent.FadeIn.Length = Timecode.FromMilliseconds(duration * AConfigFadein / 100);
					audioEvent.FadeOut.Length = Timecode.FromMilliseconds(duration * AConfigFadeout / 100);
					audioEvent.FadeIn.Curve = AConfigFadeinCurve;
					audioEvent.FadeOut.Curve = AConfigFadeoutCurve;
					tempEventGroup.Add(audioEvent);

					#region 应用变调
					int pitchDelta = pitch - AConfigBasePitch;
					if (AConfigMethod == AudioTuneMethod.PITCH_SHIFT) { // 注意：Vegas 版本号 15 及以下仅能使用移调插件调音。
						if (Plugin.pitchShift == null) { ShowError(new Exceptions.NoPluginPitchShiftException()); return false; }
						double _stretchRate = Pitch2Stretch(pitchDelta);
						if (AConfigLockStretchPitch && AConfigFreezeLastFrame && !AConfigScratch && duration > audioLength / _stretchRate)
							audioEvent.Length = Timecode.FromMilliseconds(audioLength / _stretchRate);
						int pitchDeltaTimes = pitchDelta > 0 ? 12 : -12;
						while (pitchDeltaTimes * pitchDelta > 0) { // pitchDeltaTimes > 0 ? pitchDelta > 0 : pitchDelta < 0
							Effect effect = audioEvent.Effects.AddEffect(Plugin.pitchShift);
							try {
								effect.Preset = (Math.Abs(pitchDelta) <= 12 ? pitchDelta : pitchDeltaTimes).ToString()
									+ (AConfigLockStretchPitch ? "~" : "");
							} catch (Exception e) { ShowError(new Exceptions.NoPluginPresetsException(), e); return false; }
							pitchDelta -= pitchDeltaTimes;
						}
					} else if (AConfigMethod == AudioTuneMethod.ELASTIQUE || AConfigMethod == AudioTuneMethod.CLASSIC) {
						#if VER_GEQ_16
							if (AConfigMethod == AudioTuneMethod.ELASTIQUE) {
								audioEvent.ElastiqueAttribute = (ElastiqueStretchAttributes)AConfigElastiqueAttr;
								if (AConfigElastiqueAttr == ElastiqueStretchAttributes.Pro) audioEvent.FormantLock = AConfigReserveFormant;
							} else if (AConfigMethod == AudioTuneMethod.CLASSIC)
								audioEvent.ClassicAttribute = (ClassicStretchAttributes)AConfigClassicAttr;
							if (!AConfigLockStretchPitch) audioEvent.PitchSemis += pitchDelta;
							else {
								double origPitch = audioEvent.PitchSemis;
								audioEvent.PitchLock = true;
								audioEvent.AdjustPlaybackRate(Pitch2Stretch(origPitch + pitchDelta), true);
								if (AConfigFreezeLastFrame && !AConfigScratch && duration > audioLength / audioEvent.PlaybackRate)
									audioEvent.Length = Timecode.FromMilliseconds(audioLength / audioEvent.PlaybackRate);
							}
						#endif
					} else if (AConfigMethod == AudioTuneMethod.FOOL_TUNING) {
						#if VER_GEQ_16
							audioEvent.ElastiqueAttribute = ElastiqueStretchAttributes.Efficient;
							audioEvent.PitchLock = true;
						#endif
					}
					#endregion
				}
				#endregion

				#region 生成视频事件
				if (VConfig) {
					if (!multitrack && lastStartTime == startTime) continue; // 避免视频重叠
					if (SheetConfig && trackPointer > vTrackCount) NewVideoTrack();
					else if (VConfigMultitrackForChords)
						for (trackPointer = 0; trackPointer < vTracks.Length; trackPointer++) {
							VideoTrack videoTrack = vTracks[trackPointer];
							if (videoTrack == null) videoTrack = NewVideoTrack();
							if ((int)Math.Ceiling(generateBeginTime + startTime) >= (int)videoTrack.Length.ToMilliseconds()) break;
						}
					VideoTrack _vTrack = !multitrack ? vTrack : vTracks[trackPointer];
					PvVisualEffect anim = !multitrack ? anime : anims[trackPointer];
					VideoEvent videoEvent;
					if (!IsFromSelectedClip) {
						videoEvent = _vTrack.AddVideoEvent(
							Timecode.FromMilliseconds(generateBeginTime + startTime),
							Timecode.FromMilliseconds(duration)
						);
						try {
							videoEvent.AddTake(media.GetVideoStreamByIndex(0));
						} catch (Exception) { ShowError(new Exceptions.NoVideoTakeException()); return false; }
					} else {
						if (selectedEventSet.videoEvent == null) { ShowError(new Exceptions.NoVideoTakeException()); return false; }
						videoEvent = selectedEventSet.videoEvent.Copy(_vTrack, Timecode.FromMilliseconds(generateBeginTime + startTime)) as VideoEvent;
						videoEvent.Length = Timecode.FromMilliseconds(duration);
					}
					if (adjustTime) AdjustDeviation(videoEvent, sourceStartTime, sourceEndTime);
					if (SheetConfig) videoEvent.Length = Timecode.FromMilliseconds(barStartTime + barLength - startTime);
					if (VConfigScratch) videoEvent.AdjustPlaybackRate(videoLength / duration, true);
					if (anim.IsReverse) ReverseVideo(videoEvent); // 结论：先拉伸后反转
					if (VConfigFreezeFirstFrame) FreezeFirstFrame(videoEvent);
					if (VConfigFreezeLastFrame && !VConfigScratch) FreezeLastFrame(videoEvent, videoLength);
					videoEvent.Loop = VConfigLoop;
					tempEventGroup.Add(videoEvent);
					// 淡入淡出
					videoEvent.FadeIn.Length = Timecode.FromMilliseconds(duration * VConfigFadein / 100);
					videoEvent.FadeOut.Length = Timecode.FromMilliseconds(duration * VConfigFadeout / 100);
					videoEvent.FadeIn.Curve = VConfigFadeinCurve;
					videoEvent.FadeOut.Curve = VConfigFadeoutCurve;
					// 视频平移/裁切调整
					VideoMotionKeyframe key0 = videoEvent.VideoMotion.Keyframes[0];
					VideoMotionKeyframe key1 = new VideoMotionKeyframe(Timecode.FromMilliseconds(duration));
					if (needTwoKey) videoEvent.VideoMotion.Keyframes.Add(key1);
					float width = key0.BottomRight.X;
					float height = key0.BottomRight.Y;
					float startRatio = VConfigStartSize / 100;
					key0.ScaleBy(new VideoMotionVertex(startRatio, startRatio));
					key0.MoveBy(new VideoMotionVertex(Math.Abs(1 - startRatio) * width / 2 * VConfigStartHTrans / 100, Math.Abs(1 - startRatio) * height / 2 * VConfigStartVTrans / 100));
					key0.Type = VConfigStartSizeCurve;
					if (needTwoKey) {
						float endRatio = VConfigEndSize / 100;
						key1.ScaleBy(new VideoMotionVertex(endRatio, endRatio));
						key1.MoveBy(new VideoMotionVertex(Math.Abs(1 - endRatio) * width / 2 * VConfigEndHTrans / 100, Math.Abs(1 - endRatio) * height / 2 * VConfigEndVTrans / 100));
					}
					// 色相、饱和度、对比度
					if (Plugin.hslAdjust != null) Plugin.ForVideoEvents.HueAndSaturationParam(videoEvent, VConfigStartHue, VConfigEndHue, VConfigStartSaturation, VConfigEndSaturation, VConfigStartHueCurve, VConfigStartSaturationCurve); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.hsl_adjust)); return false; }
					if (Plugin.contrast != null) Plugin.ForVideoEvents.ContrastAndThresholdParam(videoEvent, VConfigStartContrast, VConfigEndContrast, VConfigStartThreshold, VConfigEndThreshold, VConfigStartContrastCurve, VConfigStartThresholdCurve); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.brightness_and_contrast)); return false; }
					// 单独对所有关键帧处理翻转
					videoEvent.FlipAllKeyframe(anim.HorizontalFlip, anim.VerticalFlip);
					// 旋转属性单独调整，因为和上面“翻转所有关键帧”功能冲突了
					key0.RotateBy(VConfigStartRotation + anim.RotationDeg);
					if (needTwoKey) key1.RotateBy(VConfigEndRotation + anim.RotationDeg);
					// 动画效果生成
					if (VConfigGlow != 0) if (Plugin.contrast != null) Plugin.ForVideoEvents.Glow(videoEvent, VConfigGlow, VConfigGlowCurve, VConfigGlowBright);
						else { ShowError(new Exceptions.NoPluginNameException(Lang.str.brightness_and_contrast)); return false; }
					if (anim.IsNegative) if (Plugin.invert != null) Plugin.ForVideoEvents.Negative(videoEvent); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.invert)); return false; }
					if (anim.IsGrey) if (Plugin.blackAndWhite != null) Plugin.ForVideoEvents.Grey(videoEvent); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.black_and_white)); return false; }
					if (anim.IsInvertLumin) if (Plugin.labAdjust != null) Plugin.ForVideoEvents.InvertLumin(videoEvent); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.lab_adjust)); return false; }
					if (anim.Hue != 0) if (Plugin.hslAdjust != null) Plugin.ForVideoEvents.ChangeHue(videoEvent, anim.Hue); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.hsl_adjust)); return false; }
					if (anim.HorizontalMirrored != 0 || anim.VerticalMirrored != 0) if (Plugin.mirror != null) Plugin.ForVideoEvents.Mirror(videoEvent, anim.HorizontalMirrored, anim.VerticalMirrored);
						else { ShowError(new Exceptions.NoPluginNameException(Lang.str.mirror)); return false; }
					if (anim.ObliqueDirect != 0) if (Plugin.picInPic != null) Plugin.ForVideoEvents.ObliqueExpansion(videoEvent, anim.ObliqueDirect); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.pic_in_pic)); return false; }
					if (anim.PuyoShape != 0) if (Plugin.picInPic != null) Plugin.ForVideoEvents.PuyoPuyo(videoEvent, anim.PuyoShape); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.pic_in_pic)); return false; }
					if (anim.SwingDirect != 0) if (Plugin.picInPic != null) Plugin.ForVideoEvents.Pendulum(videoEvent, anim.SwingDirect); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.pic_in_pic)); return false; }
					if (anim.IsSingleDuration) {
						if (anim.fx == PvVisualEffectType.GAUSSIAN_BLUR) if (Plugin.gaussianBlur != null) Plugin.ForVideoEvents.GaussianBlur(videoEvent); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.gaussian_blur)); return false; }
						if (anim.fx == PvVisualEffectType.RADIAL_BLUR) if (Plugin.radialBlur != null) Plugin.ForVideoEvents.RadialBlur(videoEvent); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.radial_blur)); return false; }
						if (anim.fx >= PvVisualEffectType.VERTICAL_EXPANSION && anim.fx <= PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND) if (Plugin.picInPic != null) Plugin.ForVideoEvents.Expansion(videoEvent, anim.fx); else { ShowError(new Exceptions.NoPluginNameException(Lang.str.pic_in_pic)); return false; }
					}
					// 五线谱效果生成
					if (SheetConfig) {
						if (UseLegacySheetMethod) {
							double sheetConfigGap = SheetConfigGap,
								sheetConfigWidth = SheetConfigWidth,
								sheetConfigPosition = SheetConfigPosition;
							if (SheetConfigRelative) {
								sheetConfigGap *= vegas.Project.Video.Height / 1080.0;
								sheetConfigWidth *= vegas.Project.Video.Width / 1920.0;
								sheetConfigPosition *= vegas.Project.Video.Height / 1080.0;
							}
							TrackMotionKeyframe keyFrame = vTracks[trackPointer].TrackMotion.InsertMotionKeyframe(Timecode.FromMilliseconds(startTime));
							keyFrame.Type = VideoKeyframeType.Hold;
							keyFrame.Width = sheetConfigGap * 2 * projWidth / projHeight;
							keyFrame.Height = sheetConfigGap * 2;
							keyFrame.PositionX = -sheetConfigWidth / 2 + sheetConfigWidth / barLength * (startTime - barStartTime);
							int octave = _pitch / 12;
							int line = StaffPitchMap[_pitch % 12];
							keyFrame.PositionY = sheetConfigPosition - sheetConfigGap * 3 + (octave - 5) * sheetConfigGap * 3.5 + line * sheetConfigGap * 0.5 + SheetConfigCelf * 12;
						} else {
							if (Plugin.picInPic == null) { ShowError(new Exceptions.NoPluginNameException(Lang.str.pic_in_pic)); return false; }
							Effect picInPic = videoEvent.Effects.AddEffect(Plugin.picInPic);
							OFXDoubleParameter scale = picInPic.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter;
							scale.Value = SheetConfigGap * 2.0 / projHeight;
							OFXDoubleParameter scaleY = picInPic.OFXEffect.FindParameterByName("DistortionScaleY") as OFXDoubleParameter; // Vegas 15 及以下版本不支持。
							if (scaleY != null) scaleY.Value = scale.Value;
							OFXDouble2DParameter location = picInPic.OFXEffect.FindParameterByName("Location") as OFXDouble2DParameter;
							double positionX = -SheetConfigWidth / 2 + SheetConfigWidth / barLength * (startTime - barStartTime);
							int octave = _pitch / 12;
							int line = StaffPitchMap[_pitch % 12];
							double positionY = SheetConfigPosition - SheetConfigGap * 3 + (octave - 5) * SheetConfigGap * 3.5 + line * SheetConfigGap * 0.5 + SheetConfigCelf * 12;
							location.Value = new OFXDouble2D { X = positionX / projWidth + 0.5, Y = positionY / projHeight + 0.5 };
						}
						trackPointer++;
					}

					lastStartTime = startTime;
					anim.Next();
				}
				#endregion
			}
			#endregion
			tempEventGroup.Bundle();

			#region 生成五线谱
			if (SheetConfigGenerateStaff) {
				if (Plugin.crop == null) { ShowError(new Exceptions.NoPluginNameException(Lang.str.crop)); return false; }
				if (Plugin.mirror == null) { ShowError(new Exceptions.NoPluginNameException(Lang.str.mirror)); return false; }
				VideoTrack sheetTrack;
				vegas.Project.Tracks.Add(sheetTrack = new VideoTrack(tTrackCount++));
				double start = generateBeginTime + MidiConfigStartTime,
					length = songLength;
				VideoEvent videoEvent = sheetTrack.AddVideoEvent(Timecode.FromMilliseconds(start), Timecode.FromMilliseconds(length));
				Media solidColor = new Media(Plugin.solidColor);
				videoEvent.AddTake(solidColor.GetVideoStreamByIndex(0));
				Color color = SheetConfigColor;
				(solidColor.Generator.OFXEffect.FindParameterByName("Color") as OFXRGBAParameter).Value = new OFXColor(color.R / 255.0, color.G / 255.0, color.B / 255.0);
				// 此处用一段非常笨拙的方式生成五线谱。
				Effect effect1 = videoEvent.Effects.AddEffect(Plugin.crop);
				(effect1.OFXEffect.FindParameterByName("YScale") as OFXDoubleParameter).Value = SheetConfigThickness / 500.0;
				Effect effect2 = videoEvent.Effects.AddEffect(Plugin.mirror);
				(effect2.OFXEffect.FindParameterByName("Center") as OFXDouble2DParameter).Value = new OFXDouble2D { X = 0.5, Y = 0.4 };
				Effect effect3 = videoEvent.Effects.AddEffect(Plugin.mirror);
				(effect3.OFXEffect.FindParameterByName("Center") as OFXDouble2DParameter).Value = new OFXDouble2D { X = 0.5, Y = 0.3 };
				Effect effect4 = videoEvent.Effects.AddEffect(Plugin.mirror);
				(effect4.OFXEffect.FindParameterByName("Angle") as OFXDoubleParameter).Value = 180;
				Effect effect5 = videoEvent.Effects.AddEffect(Plugin.picInPic);
				double transformScaleY = 1.25 / projHeight * SheetConfigGap * 4;
				double transformPositionY = (double)SheetConfigPosition / projHeight + 0.5;
				OFXDoubleParameter scaleY = effect5.OFXEffect.FindParameterByName("DistortionScaleY") as OFXDoubleParameter;
				if (scaleY != null) {
					scaleY.Value = transformScaleY;
					(effect5.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter).Value = 1.0;
					(effect5.OFXEffect.FindParameterByName("Location") as OFXDouble2DParameter).Value = new OFXDouble2D { X = 0.5, Y = transformPositionY };
					OFXChoiceParameter proportion = effect5.OFXEffect.FindParameterByName("KeepProportions") as OFXChoiceParameter;
					proportion.Value = proportion.Choices[1]; // 固定平行四边形
				} else { // 如果是旧版本 Vegas 不支持“缩放高度”参数。
					videoEvent.Effects.Remove(effect5);
					TrackMotionKeyframe keyFrame = sheetTrack.TrackMotion.MotionKeyframes[0];
					keyFrame.Type = VideoKeyframeType.Hold;
					keyFrame.Height = transformScaleY * vegas.Project.Video.Height;
					keyFrame.PositionY = (transformPositionY - 0.5) * vegas.Project.Video.Height;
				}
				// 本来还想来自动生成谱号的，奈何脚本不支持蒙版锚点的编辑。
			}
			#endregion

			#region 消除间隙
			// ⸘视频被削除了‽
			if (VConfigLegato && !SheetConfig && VConfig) {
				List<VideoTrack> videoTracks = new List<VideoTrack> { vTrack };
				if (vTracks != null) videoTracks.AddRange(vTracks);
				foreach (VideoTrack track in videoTracks)
					if (track != null) {
						TrackEvents events = track.Events;
						for (int i = 0; i < events.Count - 1; i++)
							events[i].End = events[i + 1].Start;
					}
			}
			if (AConfigLegato && AConfig && aTracks != null)
				foreach (AudioTrack aTrack in aTracks)
					if (aTrack != null) {
						TrackEvents events = aTrack.Events;
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

			if (vTrack != null) generatedVideoTracks.Add(vTrack);
			else foreach (VideoTrack videoTrack in vTracks)
				if (videoTrack != null) generatedVideoTracks.Add(videoTrack);
			return !progressForm.RequestAbort;
		}

		/// <summary>
		/// 相对音高到拉伸值的转换。
		/// </summary>
		/// <param name="pitch">相对音高</param>
		/// <returns>拉伸值</returns>
		public static double Pitch2Stretch(double pitch) {
			return Math.Pow(2, pitch / 12.0);
		}

		/// <summary>
		/// 拉伸值到相对音高的转换。
		/// </summary>
		/// <param name="stretch">拉伸值</param>
		/// <returns>相对音高</returns>
		public static double Stretch2Pitch(double stretch) {
			return 12.0 * Math.Log(stretch, 2.0);
		}

		private bool requestRestartScript = false;

		/// <summary>
		/// 检查移调插件的预设是否存在。
		/// </summary>
		/// <returns>处理是否成功。如果预设不存在并且脚本自己也无法自动生成预设，才会返回 false。</returns>
		private bool ExaminePitchShiftPresetsExist() {
			progressForm.Info = Lang.str.check_pitch_shift_presets;
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
			DialogResult result = MessageBoxEx.Show(Lang.str.no_pitch_shift_presets, Lang.str.no_pitch_shift_presets_title, MessageBoxButtons.YesNo, new string[] { Lang.str.yes_to_add_pitch_shift_presets, Lang.str.no_to_add_pitch_shift_presets } , MessageBoxIcon.Exclamation);
			if (result == DialogResult.No) {
				requestRestartScript = true;
				return false;
			}
			LoadPitchShiftPresets();
			if (checkPresets()) MessageBox.Show(Lang.str.add_pitch_shift_presets_successful, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
			else {
				MessageBox.Show(Lang.str.add_pitch_shift_presets_fail, Lang.str.add_pitch_shift_presets_fail_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				ShowError(new Exceptions.NoPluginPitchShiftException());
				return false;
			}
			return true;
		}

		private const string PITCH_SHIFT_PRESETS_PATH = @"SOFTWARE\DirectShow\Presets\{ED1B4100-93BE-11D0-AEBC-00A0C9053912}";

		internal void LoadPitchShiftPresets() {
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
			RegistryKey reg = Registry.CurrentUser.CreateSubKey(PITCH_SHIFT_PRESETS_PATH);
			Tuple<string, byte[]>[] tuples = {
				#region 二进制数据
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
				#endregion
			};
			foreach (var item in tuples)
				reg.SetValue(item.Item1, item.Item2, RegistryValueKind.Binary);
			reg.Close();
			#endregion
		}

		internal void UnloadPitchShiftPresets() {
			RegistryKey reg = Registry.CurrentUser.CreateSubKey(PITCH_SHIFT_PRESETS_PATH);
			for (int i = -12; i <= 12; i++) {
				reg.DeleteValue(i + "", false);
				reg.DeleteValue(i + "~", false);
			}
			reg.Close();
		}

		/// <summary>
		/// 获取反转子剪辑。
		/// </summary>
		/// <param name="media">现有媒体</param>
		/// <returns>反转子剪辑</returns>
		public Subclip GetReversedSubclip(Media media, Timecode start = null, Timecode length = null) {
			bool isReversed = true;
			start = start ?? (media is Subclip ? media.TimecodeIn + (media as Subclip).Start : media.TimecodeIn);
			length = length ?? media.Length;
			if (media is Subclip && (media as Subclip).IsReversed) isReversed = false;
			return new Subclip(
				vegas.Project,
				media.FilePath,
				start,
				length,
				isReversed,
				new Path(media.FilePath).FullFileName + ' ' + Lang.str.reverse_suffix_tag
			);
		}

		/// <summary>
		/// 获取反转子剪辑。
		/// </summary>
		/// <param name="trackEvent">现有轨道剪辑</param>
		/// <returns>反转子剪辑</returns>
		public Subclip GetReversedSubclip(TrackEvent trackEvent) {
			Take take = trackEvent.ActiveTake;
			return GetReversedSubclip(take.Media, take.Offset, take.AvailableLength);
		}

		private bool Track_AppendMedia(Track track, Media media, out TrackEvent trackEvent) {
			/* typeof(T) == typeof(AudioEvent) */
			if (track is AudioTrack) {
				trackEvent = (track as AudioTrack).AddAudioEvent(track.Length, media.Length);
				try {
					trackEvent.AddTake(media.GetAudioStreamByIndex(0));
				} catch (Exception e) { ShowError(new Exceptions.NoAudioTakeException(), e); return false; }
			} else if (track is VideoTrack) {
				trackEvent = (track as VideoTrack).AddVideoEvent(track.Length, media.Length);
				try {
					trackEvent.AddTake(media.GetVideoStreamByIndex(0));
				} catch (Exception e) { ShowError(new Exceptions.NoVideoTakeException(), e); return false; }
			} else { trackEvent = null; return false; }
			return true;
		}
		/// <summary>
		/// 在轨道追加媒体文件的音频。
		/// </summary>
		/// <param name="track">轨道</param>
		/// <param name="media">媒体</param>
		/// <param name="trackEvent">输出的轨道事件</param>
		/// <returns>是否成功添加媒体</returns>
		public bool Track_AppendMedia(AudioTrack track, Media media, out AudioEvent trackEvent) {
			TrackEvent _trackEvent;
			bool ok = Track_AppendMedia(track, media, out _trackEvent);
			trackEvent = _trackEvent as AudioEvent;
			return ok;
		}
		/// <summary>
		/// 在轨道追加媒体文件的视频。
		/// </summary>
		/// <param name="track">轨道</param>
		/// <param name="media">媒体</param>
		/// <param name="trackEvent">输出的轨道事件</param>
		/// <returns>是否成功添加媒体</returns>
		public bool Track_AppendMedia(VideoTrack track, Media media, out VideoEvent trackEvent) {
			TrackEvent _trackEvent;
			bool ok = Track_AppendMedia(track, media, out _trackEvent);
			trackEvent = _trackEvent as VideoEvent;
			return ok;
		}

		private TrackEvent Track_Append(Track track, TrackEvent source, bool isCopy = false) {
			Timecode start = track.Length;
			if (!isCopy) {
				source.Track = track;
				source.Start = start;
				return source;
			} else return source.Copy(track, start);
		}
		public AudioEvent Track_Append(Track track, AudioEvent source, bool isCopy = false) {
			return Track_Append(track, source as TrackEvent, isCopy) as AudioEvent;
		}
		public VideoEvent Track_Append(Track track, VideoEvent source, bool isCopy = false) {
			return Track_Append(track, source as TrackEvent, isCopy) as VideoEvent;
		}

		private TrackEvent GetAssociatedEvent(TrackEvent trackEvent) {
			if (!trackEvent.IsGrouped) return null;
			foreach (TrackEvent eventInGroup in trackEvent.Group)
				if (trackEvent is AudioEvent && eventInGroup is TrackEvent)
					return eventInGroup;
			return null;
		}
		public VideoEvent GetAssociatedEvent(AudioEvent trackEvent) {
			return GetAssociatedEvent(trackEvent as TrackEvent) as VideoEvent;
		}
		public AudioEvent GetAssociatedEvent(VideoEvent trackEvent) {
			return GetAssociatedEvent(trackEvent as TrackEvent) as AudioEvent;
		}

		private bool GetSelectedSources(AudioTrack aSmpTrack, VideoTrack vSmpTrack, out EventSet[] eventSetArr) {
			eventSetArr = null;
			List<EventSet> eventSets = new List<EventSet>();
			if (!IsFromSelectedClip) {
				List<Media> selections = new List<Media>();
				#region 验证合法性
				bool ytpOverLength = false;
				bool ytpInMediaGenerator = false;
				bool noMediaTake = false;
				Exception noMediaTakeException = null;
				if (IsFromSelectedMedia)
					foreach (Media media in vegas.Project.MediaPool.GetSelectedMedia()) {
						try {
							if (!media.IsValid())
								noMediaTake = true;
							else if (YtpConfigMinLen > media.Length.ToMilliseconds())
								ytpOverLength = true;
							else if (media.Generator != null)
								ytpInMediaGenerator = true;
							else selections.Add(media);
						} catch (Exception e) {
							noMediaTake = true;
							noMediaTakeException = noMediaTakeException ?? e;
							continue;
						}
					}
				if (selections.Count == 0) {
					if (YtpConfigMinLen > media.Length.ToMilliseconds())
						ytpOverLength = true;
					else if (media.Generator != null)
						ytpInMediaGenerator = true;
					else selections.Add(media);
				}
				if (selections.Count /* still */ == 0) {
					if (ytpOverLength) {
						ShowError(new Exceptions.YtpOverLengthException(), ShowErrorState.RESUME_NEXT);
						return false;
					}
					if (ytpInMediaGenerator) {
						ShowError(new Exceptions.YtpInMediaGeneratorException(), ShowErrorState.RESUME_NEXT);
						return false;
					}
					if (noMediaTake) {
						if (noMediaTakeException != null)
							ShowError(new Exceptions.NoMediaTakeException(), noMediaTakeException, ShowErrorState.RESUME_NEXT);
						else ShowError(new Exceptions.NoMediaTakeException(), ShowErrorState.RESUME_NEXT);
						return false;
					}
				}
				#endregion
				#region 放置示例轨道剪辑
				foreach (Media media in selections) {
					AudioEvent aSmp = null; VideoEvent vSmp = null;
					Subclip aReverse = null, vReverse = null;
					if (AConfig) if (!Track_AppendMedia(aSmpTrack, media, out aSmp)) return false;
					if (VConfig) if (!Track_AppendMedia(vSmpTrack, media, out vSmp)) return false;
					aReverse = vReverse = GetReversedSubclip(media);
					eventSets.Add(new EventSet(aSmp, vSmp, aReverse, vReverse));
				}
				#endregion
			} else {
				#region 放置示例轨道剪辑
				var appendOne = new Func<AudioEvent, VideoEvent, bool, bool>((aEvent, vEvent, notIgnore) => {
					var SE = new Action<Exception>(e => { // 色氵炎々（误
						if (notIgnore) ShowError(e, ShowErrorState.RESUME_NEXT);
					});
					AudioEvent aSmp = null; VideoEvent vSmp = null;
					Subclip aReverse = null, vReverse = null;
					if (AConfig) {
						if (aEvent == null) { SE(new Exceptions.NoAudioTakeException()); return false; }
						if (YtpConfigMinLen > aEvent.Length.ToMilliseconds()) goto YtpOverLength;
						aReverse = vReverse = GetReversedSubclip(aEvent);
					}
					if (VConfig) {
						if (vEvent == null) { SE(new Exceptions.NoVideoTakeException()); return false; }
						if (YtpConfigMinLen > vEvent.Length.ToMilliseconds()) goto YtpOverLength;
						if (vEvent.ActiveTake.Media.Generator != null) goto YtpInMediaGenerator;
						if (!AConfig || vEvent.ActiveTake.Media != aEvent.ActiveTake.Media)
							vReverse = GetReversedSubclip(vEvent);
					}
					// 在后面单独添加，避免之后报错又消不掉。
					if (AConfig) aSmp = Track_Append(aSmpTrack, aEvent, true);
					if (VConfig) vSmp = Track_Append(vSmpTrack, vEvent, true);
					eventSets.Add(new EventSet(aSmp, vSmp, aReverse, vReverse));
					return true;
				YtpOverLength:
					SE(new Exceptions.YtpOverLengthException());
					return false;
				YtpInMediaGenerator:
					SE(new Exceptions.YtpInMediaGeneratorException());
					return false;
				});
				TrackEvent[] _selected = GetSelectedEvents();
				AudioEvent[] _selectedAudio = GetSelectedAudioEvents();
				VideoEvent[] _selectedVideo = GetSelectedVideoEvents();
				#endregion
				#region 验证合法性
				if (_selectedAudio.Length == 1 && _selectedVideo.Length == 1)
					appendOne(_selectedAudio[0], _selectedVideo[0], true);
				else if (_selected.Length == 1)
					appendOne(selectedEventSet.audioEvent, selectedEventSet.videoEvent, true);
				else {
					foreach (TrackEvent trackEvent in _selected) {
						if (!trackEvent.IsGrouped) continue;
						AudioEvent aEvent = null;
						VideoEvent vEvent = null;
						if (trackEvent is AudioEvent) {
							aEvent = trackEvent as AudioEvent;
							vEvent = GetAssociatedEvent(aEvent);
							if (vEvent == null) continue;
						} else if (trackEvent is VideoEvent) {
							vEvent = trackEvent as VideoEvent;
							aEvent = GetAssociatedEvent(vEvent);
							if (aEvent == null) continue;
						} else continue;
						appendOne(aEvent, vEvent, false);
					}
					if (eventSets.Count == 0) appendOne(selectedEventSet.audioEvent, selectedEventSet.videoEvent, true);
				}
				#endregion
			}
			eventSetArr = eventSets.ToArray();
			return true;
		}

		/// <summary>
		/// 为指定的轨道剪辑创建一个新的分组。
		/// </summary>
		/// <param name="trackEvents">多个轨道剪辑</param>
		/// <returns>轨道剪辑组</returns>
		public TrackEventGroup GroupTrackEvents(params TrackEvent[] trackEvents) {
			TrackEventGroup group = new TrackEventGroup(vegas.Project);
			vegas.Project.TrackEventGroups.Add(group);
			foreach (TrackEvent trackEvent in trackEvents)
				group.Add(trackEvent);
			return group;
		}

		public class TempEventGroup {
			private Timecode startTime;
			private List<TrackEvent> events = new List<TrackEvent>();
			private readonly EntryPoint parent;
			private bool Enabled { get { return parent.ConfigCreateEventGroup && parent.AConfig; } }
			public TempEventGroup(EntryPoint parent) { this.parent = parent; }
			public void Add(TrackEvent trackEvent) {
				if (!Enabled) return;
				if (trackEvent == null) { Bundle(); return; }
				if (startTime == null) startTime = trackEvent.Start;
				if (startTime != trackEvent.Start) Bundle();
				events.Add(trackEvent);
			}
			public TrackEventGroup Bundle() {
				if (!Enabled) return null;
				TrackEventGroup group = null;
				if (events.Count > 1) group = parent.GroupTrackEvents(events.ToArray());
				startTime = null;
				events = new List<TrackEvent>();
				return group;
			}
		}

		public static readonly Timecode ZERO = Timecode.FromMilliseconds(0);

		/// <summary>
		/// 将指定的一些轨道的剪辑堆积起来，消除中间的空隙。<br />
		/// 注意：第一个轨道剪辑之前的空隙不会消除。
		/// </summary>
		/// <param name="tracks">指定的多个轨道。如果为空，则为选中的轨道。</param>
		public void StackingLegatoTracks(Track[] tracks = null) {
			tracks = tracks ?? GetSelectedTracks();
			foreach (Track track in tracks) {
				for (int i = 1; i < track.Events.Count; i++) {
					TrackEvent currentEvent = track.Events[i];
					TrackEvent previousEvent = track.Events[i - 1];
					currentEvent.Start = previousEvent.End;
				}
			}
		}

		/// <summary>
		/// 为指定的一些轨道以拉伸的方式填补中间的空隙。<br />
		/// 注意：第一个轨道剪辑之前的空隙不会消除。
		/// </summary>
		/// <param name="forceStretch">
		/// 是否强制拉伸？Vegas 限制拉伸素材的速率在 0.25 ~ 2 之间或更小（取决于变调操作）。<br />
		/// 如果为否，则当素材拉伸到极限之后将不再继续拉伸；<br />
		/// 如果为是，则当素材拉伸到极限之后改变素材的长度，播放原素材裁剪范围之外的内容（拉长）或裁去多余长度的内容（缩短）。
		/// </param>
		/// <param name="tracks">指定的多个轨道。如果为空，则为选中的轨道。</param>
		public void StretchLegatoTracks(bool forceStretch = false, Track[] tracks = null) {
			tracks = tracks ?? GetSelectedTracks();
			foreach (Track track in tracks) {
				for (int i = 0; i < track.Events.Count - 1; i++) {
					TrackEvent currentEvent = track.Events[i];
					TrackEvent nextEvent = track.Events[i + 1];
					double rate = currentEvent.Length.ToMilliseconds() / (nextEvent.Start - currentEvent.Start).ToMilliseconds();
					currentEvent.RelativeAdjustPlaybackRate(rate, forceStretch);
				}
			}
		}

		/// <summary>
		/// 为指定的一些轨道以改变持续时间的方式填补中间的空隙。<br />
		/// 注意：第一个轨道剪辑之前的空隙不会消除。
		/// </summary>
		/// <param name="tracks">指定的多个轨道。如果为空，则为选中的轨道。</param>
		public void LengthenLegatoTracks(Track[] tracks = null) {
			tracks = tracks ?? GetSelectedTracks();
			foreach (Track track in tracks) {
				for (int i = 0; i < track.Events.Count - 1; i++) {
					TrackEvent currentEvent = track.Events[i];
					TrackEvent nextEvent = track.Events[i + 1];
					currentEvent.Length = nextEvent.Start - currentEvent.Start;
				}
			}
		}

		/// <summary>
		/// 生成 YouTube Poop。
		/// </summary>
		private bool GenerateYtp() {
			progressForm.Info = Lang.str.processing_ytp;
			AudioTrack aSmpTrack = null, aTrack = null;
			VideoTrack vSmpTrack = null, vTrack = null;
			if (AConfig) vegas.Project.Tracks.Add(aSmpTrack = new AudioTrack(vegas.Project, 0, "YTP Audio Sample Track"));
			if (VConfig) vegas.Project.Tracks.Add(vSmpTrack = new VideoTrack(vegas.Project, 0, "YTP Video Sample Track"));
			if (AConfig) vegas.Project.Tracks.Add(aTrack = new AudioTrack(vegas.Project, 0, ""));
			if (VConfig) vegas.Project.Tracks.Add(vTrack = new VideoTrack(vegas.Project, 0, ""));
			var DeleteYtpSampleTracks = new Action<bool>(reserveYtpTracks => {
				if (aSmpTrack != null) vegas.Project.Tracks.Remove(aSmpTrack);
				if (vSmpTrack != null) vegas.Project.Tracks.Remove(vSmpTrack);
				if (!reserveYtpTracks) {
					if (aTrack != null) vegas.Project.Tracks.Remove(aTrack);
					if (vTrack != null) vegas.Project.Tracks.Remove(vTrack);
				}
			});
			EventSet[] eventSets;
			#region 多素材支持
			if (!GetSelectedSources(aSmpTrack, vSmpTrack, out eventSets) || eventSets.Length == 0) goto CleanUpRuins;
			EventSet.EliminateDuplicates(ref eventSets);
			if (eventSets.Length == 0) goto CleanUpRuins;
			#endregion
			#region 获取选中需要使用的效果
			Random rand = new Random();
			for (int i = 0; i < YtpConfigClipsCount; i++) {
				// int statusProgress = (int)Math.Round(100.0 * i / (YtpConfigClipsCount - 1.0));
				// progressForm.ReportProgress(statusProgress);
				progressForm.ReportProgress(i + 1, YtpConfigClipsCount);
				if (progressForm.RealTimeUpdateCheck.Checked)
					vegas.UpdateUI();
				if (progressForm.RequestAbort) break;
				EventSet randClip = eventSets[rand.Next(eventSets.Length)];
				double randLen = rand.Next(YtpConfigMinLen, YtpConfigMaxLen + 1);
				int sourceAvailableLength = (int)((AConfig && VConfig ? Math.Min(randClip.audioLength, randClip.videoLength) :
					AConfig ? randClip.audioLength : randClip.videoLength) - randLen);
				double randStart = sourceAvailableLength <= 0 ? 0 : rand.Next(sourceAvailableLength);
				AudioEvent aEvent = null;
				VideoEvent vEvent = null;
				#region 布置素材
				if (AConfig) {
					aEvent = randClip.audioEvent.Copy(aSmpTrack, ZERO) as AudioEvent;
					aEvent.AdjustStartLength(Timecode.FromMilliseconds(randStart), Timecode.FromMilliseconds(randLen), true);
					Track_Append(aTrack, aEvent, false);
				}
				if (VConfig) {
					vEvent = randClip.videoEvent.Copy(vSmpTrack, ZERO) as VideoEvent;
					vEvent.AdjustStartLength(Timecode.FromMilliseconds(randStart), Timecode.FromMilliseconds(randLen), true);
					Track_Append(vTrack, vEvent, false);
				}
				if (AConfig && VConfig) // 为音频与视频事件创建一个新的分组
					GroupTrackEvents(vEvent, aEvent);
				#endregion
				#region 应用随机效果
				Plugin.ForYtps.GetRandomEffect(aEvent, vEvent, AConfig, VConfig, randClip.audioReverse, randClip.videoReverse, this, YtpConfigEffects);
				#endregion
			}
			#endregion
			progressForm.ReportProgress(YtpConfigClipsCount, YtpConfigClipsCount);
			DeleteYtpSampleTracks(true);
			return true;
		CleanUpRuins:
			DeleteYtpSampleTracks(false);
			return false;
		}

		public void NormalizeSelectedAudioEvents() {
			AudioEvent[] events = GetSelectedAudioEvents();
			foreach (AudioEvent audioEvent in events) {
				audioEvent.RecalculateNorm();
				audioEvent.Normalize = true;
			}
		}

		private void Generate() {
			MidiConfigTrack.CompleteConfig();
			if (YtpConfig || !IsMultiMidiChannel) {
				GenerateOtomad();
				return;
			}
			while (MidiConfigTrack.IsNonEmpty) {
				if (!GenerateOtomad()) return;
				MidiConfigTrack.Next();
			}
			if (!SheetConfig) {
				VideoTrack[] videoTracks = generatedVideoTracks.ToArray();
				AutoLayoutTracksGridForm.Arrange(videoTracks, LayoutInfos.Grid, this);
				GradientTracksForm.Arrange(videoTracks, LayoutInfos.GradientTracks, this);
			}
		}

		/// <summary>最低支持 Vegas 版本号。</summary>
		public static readonly Version MIN_SUPPORTED_VERSION =
			#if VER_GEQ_16
				new Version(16, 0);
			#elif VER_GEQ_14
				new Version(14, 0);
			#else
				new Version(13, 0);
			#endif
		/// <summary>最高支持 Vegas 版本号。</summary>
		public static readonly Version MAX_SUPPORTED_VERSION =
			#if VER_GEQ_16
				null;
			#elif VER_GEQ_14
				new Version(15, 0);
			#else
				new Version(13, 0);
			#endif
		public static Version CurrentVegasVersion;

		/// <summary>
		/// 检查 Vegas 版本是否支持。
		/// </summary>
		/// <returns>版本支持情况状态值。</returns>
		public SupportVegasVersionState CheckVersionSupport() {
			Version curVer = CurrentVegasVersion = GetVegasVersion();
			if (curVer == null) return SupportVegasVersionState.UNDEFINED;
			if (curVer >= MIN_SUPPORTED_VERSION &&
				(MAX_SUPPORTED_VERSION == null || curVer <= MAX_SUPPORTED_VERSION)
			) return SupportVegasVersionState.SUPPORTED;
			return SupportVegasVersionState.UNSUPPORTED;
		}

		/// <summary>
		/// 获取当前 Vegas 版本号对象。
		/// </summary>
		/// <returns>Vegas 版本号对象。</returns>
		public Version GetVegasVersion() {
			try {
				MatchCollection matches = Regex.Matches(vegas.Version, @"(\d+\.)+\d+");
				if (matches.Count <= 0) return null;
				string version = matches[0].Value;
				List<object> versionItems = new List<object>(); // 必须用 object 不能用 int。
				const int VERSION_MAX_LENGTH = 4;
				int versionNum;
				foreach (string item in version.Split('.'))
					if (versionItems.Count < VERSION_MAX_LENGTH)
						if (int.TryParse(item, out versionNum))
							versionItems.Add(versionNum);
				if (versionItems.Count == 0) return null;
				return Activator.CreateInstance(typeof(Version), versionItems.ToArray()) as Version;
			} catch (Exception) { return null; }
		}
		
		public static string GetScriptSupportedVersionRange() {
			string supportedVersion = "None";
			if (MAX_SUPPORTED_VERSION != null && MIN_SUPPORTED_VERSION != null)
				supportedVersion = MIN_SUPPORTED_VERSION.ToString() + " ~ " + MAX_SUPPORTED_VERSION.ToString();
			else if (MIN_SUPPORTED_VERSION != null)
				supportedVersion = MIN_SUPPORTED_VERSION.ToString() + " +";
			else if (MAX_SUPPORTED_VERSION != null)
				supportedVersion = "0 ~ " + MAX_SUPPORTED_VERSION.ToString();
			return supportedVersion;
		}

		public static void AlertUnsupportVersion() {
			MessageBox.Show(string.Format(Lang.str.unsupported_vegas_version, GetScriptSupportedVersionRange(), CurrentVegasVersion.ToString()), Lang.str.unsupported_vegas_version_title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
		}

		private SupportVegasVersionState _supported = SupportVegasVersionState.UNDEFINED;
		public SupportVegasVersionState Supported { get { return _supported; } }

		/// <summary>
		/// Vegas 脚本的入口方法。
		/// </summary>
		/// <param name="myVegas">Vegas 软件</param>
		public void FromVegas(Vegas myVegas) {
			vegas = myVegas;
			if (instance != null) return;
			instance = this;
			_supported = CheckVersionSupport();
			#if PRODUCTION
			try {
			#endif
				do {
					if (!ShowConfigForm()) continue;
					Generate();
					progressForm.ReportProgress(100);
					progressForm.Close();
				} while (requestRestartScript);
			#if PRODUCTION
			} catch (Exception e) {
				ShowError(new Exceptions.UnknownException(), e);
			}
			#endif
		}
	}

	#region 其它东西
	/// <summary>
	/// 该类中的方法仅供测试使用。<br />
	/// 切勿在生产版本中使用该类中的任何方法，即便只是为了显示一个消息框。<br />
	/// <small>名称 S 源于 <c>MessageBox.Show</c> 中 Show 的第一个字母。</small>
	/// </summary>
	[Obsolete]
	public static class S {
		#region 以下方法仅供测试使用
		public static object s { set { MessageBox.Show(value == null ? "null" : value.ToString()); } }
		public static void test() { s = "Super Idol 的笑容都没你的甜！"; }
		public static void update() { EntryPoint.instance.vegas.UpdateUI(); test(); }
		#endregion
	}

	/// <summary>
	/// 扩展类方法<br/>
	/// C# 的 this 参数，就是 JavaScript 的 prototype。
	/// </summary>
	public static class Extensions {
		/// <summary>
		/// 给老娘展示内容！
		/// </summary>
		/// <param name="value">内容</param>
		public static string s(this object value) { string str = value == null ? "null" : value.ToString(); MessageBox.Show(str); return str; }

		/// <summary>
		/// 在视频事件平移/裁切中将其所有关键帧进行翻转操作。
		/// 注意将会覆盖原有翻转设置，而不是相对于原有值进行修改。
		/// </summary>
		/// <param name="videoEvent">视频事件</param>
		/// <param name="hFlip">水平翻转？</param>
		/// <param name="vFlip">垂直翻转？</param>
		public static void FlipAllKeyframe(this VideoEvent videoEvent, bool hFlip, bool vFlip) {
			foreach (VideoMotionKeyframe key in videoEvent.VideoMotion.Keyframes) {
				bool isXFlip = key.TopLeft.X > key.TopRight.X, isYFlip = key.TopRight.Y > key.BottomRight.Y;
				key.ScaleBy(new VideoMotionVertex(isXFlip == hFlip ? 1 : -1, isYFlip == vFlip ? 1 : -1));
			}
		}

		/// <summary>
		/// 若给定的时间码没有量化为帧，则将进位到下一帧所对应的时间码，而不是默认的舍去回上一帧的时间码。
		/// </summary>
		/// <param name="timecode">给定的时间码</param>
		public static Timecode FixToFrame(this Timecode timecode) {
			long frame = timecode.FrameCount;
			if (Timecode.FromFrames(frame) < timecode) timecode = Timecode.FromFrames(frame + 1);
			return timecode;
		}

		/// <summary>
		/// 设定下拉菜单选择的编号，避免设定的编号超出下拉菜单的项目数量。
		/// </summary>
		/// <param name="combo">下拉菜单</param>
		/// <param name="index">设定选项序号</param>
		/// <param name="def">设定如果设定失败的默认值，如果为 -1 或留空表示不设定</param>
		public static void SetIndex(this ComboBox combo, int index, int def = -1) {
			int length = combo.Items.Count;
			if (length == 0) return;
			if (index >= length || index < 0) {
				if (def >= length || def < 0) return; // 你特么故意找茬是吧？
				combo.SelectedIndex = def;
			} else combo.SelectedIndex = index;
		}

		/// <summary>
		/// 设定数值上下调节框的值，避免指定的数值超出控件的最小值或最大值。
		/// </summary>
		/// <typeparam name="T">接收数字的类型</typeparam>
		/// <param name="numeric">数值上下调节框</param>
		/// <param name="value">设定值</param>
		/// <param name="def">设定如果设定失败的默认值，如果为 null 或留空表示不设定</param>
		public static void SetValue(this NumericUpDown numeric, decimal value, decimal? def = null) {
			decimal min = numeric.Minimum, max = numeric.Maximum;
			if (value < min || value > max) {
				if (def == null || def < min || def > max) return;
				numeric.Value = (decimal)def;
			} else numeric.Value = value;
		}

		/// <summary>
		/// 批量设置复选框列表的值。
		/// </summary>
		/// <param name="checks">复选框列表</param>
		/// <param name="value">一个字符序列依次表示各个复选框的值，用 1 表示 true，0 表示 false。</param>
		public static void BatchSet(this CheckedListBox checks, string value) {
			value = new Regex(@"\s").Replace(value.Trim(), "");
			value = new Regex(@"[^0]").Replace(value, "1");
			for (int i = 0; i < Math.Min(checks.Items.Count, value.Length); i++)
				checks.SetItemChecked(i, value.Substring(i, 1) != "0");
		}
		/// <summary>
		/// 批量获取复选框列表的值。
		/// </summary>
		/// <param name="checks">复选框列表</param>
		/// <returns>一个字符序列依次表示各个复选框的值，用 1 表示 true，0 表示 false。</returns>
		public static string BatchGet(this CheckedListBox checks) {
			StringBuilder value = new StringBuilder();
			for (int i = 0; i < checks.Items.Count; i++)
				value.Append(checks.GetItemChecked(i) ? 1 : 0);
			return value.ToString();
		}

		/// <summary>
		/// 数组截取。
		/// 实际上使用时不必给定泛型 T。
		/// </summary>
		/// <typeparam name="T">数组成员类型</typeparam>
		/// <param name="array">数组</param>
		/// <param name="from">开始截取下标</param>
		/// <param name="length">截取长度。若为 -1 或留空表示持续到结尾。</param>
		/// <returns>截取后的数组</returns>
		public static T[] Slice<T>(this T[] array, int from, int length = -1) {
			List<T> list = new List<T>();
			if (length < 0 || from + length > array.Length) length = array.Length - from;
			for (int i = from; i < from + length; i++)
				list.Add(array[i]);
			return list.ToArray();
		}

		/// <summary>
		/// 对时间码对象扩展乘法运算。
		/// 但是为什么不允许使用 <c>operator *</c> 呢？
		/// </summary>
		/// <param name="self">原始时间码对象</param>
		/// <param name="ratio">乘以的倍数</param>
		/// <returns>求得的新时间码</returns>
		public static Timecode Multiply(this Timecode self, double ratio) {
			return Timecode.FromMilliseconds(self.ToMilliseconds() * ratio);
		}

		/// <summary>
		/// 指定一个相对速率来拉伸素材。
		/// 比如如果原素材速率为 0.5，此时你指定的拉伸速率为 2，那么最终素材速率将会为 1 而不是 2。
		/// </summary>
		/// <param name="trackEvent">轨道剪辑</param>
		/// <param name="rate">相对速率</param>
		/// <param name="forceStretch">
		/// 是否强制拉伸？Vegas 限制拉伸素材的速率在 0.25 ~ 2 之间或更小（取决于变调操作）。
		/// 如果为否，则当素材拉伸到极限之后将不再继续拉伸；
		/// 如果为是，则当素材拉伸到极限之后改变素材的长度，播放原素材裁剪范围之外的内容（拉长）或裁去多余长度的内容（缩短）。
		/// </param>
		/// <returns></returns>
		public static TrackEvent RelativeAdjustPlaybackRate(this TrackEvent trackEvent, double rate, bool forceStretch = false) {
			double originalRate = trackEvent.PlaybackRate;
			double expectRate = rate * originalRate;
			trackEvent.AdjustPlaybackRate(expectRate, true);
			double actualRate = trackEvent.PlaybackRate;
			trackEvent.Length = Timecode.FromMilliseconds(trackEvent.Length.ToMilliseconds() / (forceStretch ? expectRate : actualRate) * originalRate);
			return trackEvent;
		}

		/// <summary>
		/// 将拖放文件事件参数转换为拖放的文件名称数组。如果拖放的不是文件，返回 null。
		/// </summary>
		/// <param name="e">拖放事件参数</param>
		/// <returns>拖放的文件名称数组</returns>
		public static string[] GetFileNames(this DragEventArgs e) {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return null;
			return e.Data.GetData(DataFormats.FileDrop) as string[];
		}
	}

	/// <summary>
	/// 显示错误警告之后的处理操作状态。
	/// </summary>
	public enum ShowErrorState {
		/// <summary>警告之后自动删除用户配置文件，并且关闭进度条对话框。</summary>
		NORMAL,
		/// <summary>不执行任何操作。</summary>
		SILENCE,
		/// <summary>仅关闭进度条对话框。</summary>
		RESUME_NEXT,
	}

	/// <summary>
	/// 支持 Vegas 版本号情况状态。
	/// </summary>
	public enum SupportVegasVersionState {
		/// <summary>未知。</summary>
		UNDEFINED = -1,
		/// <summary>不支持。</summary>
		UNSUPPORTED,
		/// <summary>支持。</summary>
		SUPPORTED,
	}

	/// <summary>
	/// 选中媒体的素材来源。
	/// </summary>
	public enum MediaSourceFrom {
		/// <summary>在项目媒体窗口中选中的媒体文件。</summary>
		SELECTED_MEDIA,
		/// <summary>在时间线窗口选中的轨道素材剪辑事件。</summary>
		SELECTED_CLIP,
		/// <summary>从脚本对话框中浏览的媒体文件。</summary>
		BROWSE_FILE,
		/// <summary>上一次用户的选择。</summary>
		LAST_USER_PREFERENCE,
		/// <summary>没有选中任何素材。</summary>
		NOTHING_SELECTED,
	}

	/// <summary>
	/// 设定生成开始位置枚举。
	/// </summary>
	public enum GenerateAt {
		/// <summary>项目开始处。</summary>
		BEGIN,
		/// <summary>光标处。</summary>
		CURSOR,
		/// <summary>自定义。</summary>
		CUSTOM,
	}
	
	/// <summary>
	/// 设定为 MIDI 使用 BPM 配置的枚举。
	/// </summary>
	public enum MidiUseBpm {
		/// <summary>动态 MIDI 速度。</summary>
		DYNAMIC_MIDI,
		/// <summary>MIDI 速度。</summary>
		MIDI,
		/// <summary>项目速度。</summary>
		PROJECT,
		/// <summary>自定义。</summary>
		CUSTOM,
	}

	/// <summary>
	/// 调音方式算法枚举。
	/// </summary>
	public enum AudioTuneMethod {
		/// <summary>不调音。</summary>
		NO_TUNE,
		/// <summary>移调效果插件。</summary>
		PITCH_SHIFT,
		/// <summary>弹性音调更改。</summary>
		ELASTIQUE,
		/// <summary>古典音调更改。</summary>
		CLASSIC,
		/// <summary>瞎调音。</summary>
		FOOL_TUNING,
		/// <summary>酸性风格调音。（包含在 API 中但任何地方均不引用，使用后会报错。）</summary>
		ACID_STYLE = 20,
	}

	/// <summary>
	/// 自动布局轨道的布局方式。
	/// </summary>
	public enum AutoLayoutTracksType {
		/// <summary>网格布局。</summary>
		GRID,
		/// <summary>3D 方盒布局。</summary>
		BOX_3D,
	}

	/// <summary>
	/// 视频轨道的渐变效果枚举。
	/// </summary>
	public enum VideoTrackGradientEffectType {
		/// <summary>彩虹色。</summary>
		RAINBOW,
		/// <summary>逐渐饱和。</summary>
		GRADUALLY_SATURATED,
		/// <summary>逐渐对比。</summary>
		GRADUALLY_CONTRASTED,
		/// <summary>阈值。</summary>
		THRESHOLD,
		/// <summary>彩灰交替。</summary>
		ALTERNATELY_CHROMATIC,
		/// <summary>正负交替。</summary>
		ALTERNATELY_NEGATIVE,
	}

	/// <summary>
	/// YTP 效果枚举。
	/// 目前将视频和音频效果都杂糅在一起了，如果需要的话，未来可能会分离两部分内容。
	/// </summary>
	public enum YtpEffectType {
		CHORUS,
		PITCH_CHANGE,
		VIBRATO, // Probably attach video Wavy
		REVERSE,
		ROBOT_DELAY,
		SPEED_CHANGE,
		HUE_CHANGE,
		HUE_ROTATE,
		MONOCHROME,
		NEGATIVE, // Probably attach audio Low Pitch
		HIGH_FREQ_REPEAT,
		RANDOM_TONE, // attach video Horizontal Flip
		ENLARGE, // attach audio Loud
		SPHERIZE,
		MIRROR,
		HIGH_CONTRAST, // attach audio Loud
		OVERSATURATION, // Probably attach audio High Pitch
		EMPHASIZE_THRICE, // attach video Enlarge Focus, Probably attach video Monochrome if Speed Down
	}

	/// <summary>
	/// Promotion Video 的节奏视觉效果枚举。
	/// </summary>
	public enum PvVisualEffectType {
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
		LUMIN_INVERT,
		HUE_INVERT,
		STEP_3_CHANGE_HUE,
		STEP_4_CHANGE_HUE,
		STEP_5_CHANGE_HUE,
		STEP_6_CHANGE_HUE,
		STEP_7_CHANGE_HUE,
		STEP_8_CHANGE_HUE,
		GREY,
		PINGPONG,
		WHIRL, // PERPETUAL_MOTION / UNLIMITED
		VERTICAL_EXPANSION,
		VERTICAL_EXPANSION_WITH_REBOUND,
		VERTICAL_COMPRESSION,
		VERTICAL_COMPRESSION_WITH_REBOUND,
		OBLIQUE_EXPANSION_AND_COMPRESSION,
		PUYO_PUYO, // PUYO_POP
		PENDULUM, // SWING
		GAUSSIAN_BLUR,
		RADIAL_BLUR,
	}

	/// <summary>
	/// Promotion Video 的节奏视觉效果类。
	/// </summary>
	public class PvVisualEffect {
		public static string[][] InitialValues {
			get {
				Lang str = Lang.str;
				return new string[][] {
					new string[] { str.effect_init_forward },
					new string[] { str.effect_init_forward, str.effect_init_reversed },
					new string[] { str.effect_init_forward, str.effect_init_turned },
					new string[] { "0°", "-90°", "-180°", "-270°" },
					new string[] { "0°", "90°", "180°", "270°" },
					new string[] { "0°", "-90°", "-180°", "-270°" },
					new string[] { "0°", "90°", "180°", "270°" },
					new string[] { str.effect_init_forward, str.effect_init_turned },
					new string[] { str.effect_init_left, str.effect_init_right },
					new string[] { str.effect_init_up, str.effect_init_down },
					new string[] { str.effect_init_left_up, str.effect_init_left_down, str.effect_init_right_down, str.effect_init_right_up },
					new string[] { str.effect_init_left_up, str.effect_init_right_up, str.effect_init_right_down, str.effect_init_left_down },
					new string[] { str.effect_init_forward, str.effect_init_invert },
					new string[] { str.effect_init_forward, str.effect_init_invert },
					new string[] { str.effect_init_conform, str.effect_init_opposite },
					new string[] { "0°", "120°", "240°" },
					new string[] { "0°", "90°", "180°", "270°" },
					new string[] { "0°", "72°", "144°", "216°", "288°" },
					new string[] { "0°", "60°", "120°", "180°", "240°", "300°" },
					new string[] { "0°", "51.4°", "102.9°", "154.3°", "205.7°", "257.1°", "308.6°" },
					new string[] { "0°", "45°", "90°", "135°", "180°", "225°", "270°", "315°" },
					new string[] { str.effect_init_chromatic, str.effect_init_monochrome },
					new string[] { str.effect_init_forward, str.effect_init_counter },
					new string[] { str.effect_init_forward, str.effect_init_stepon },
					new string[] { str.effect_init_up },
					new string[] { str.effect_init_up },
					new string[] { str.effect_init_down },
					new string[] { str.effect_init_down },
					new string[] { str.effect_init_right, str.effect_init_left },
					new string[] { str.effect_init_flat, str.effect_init_thin },
					new string[] { str.effect_init_right, str.effect_init_left },
					new string[] { str.effect_init_blur },
					new string[] { str.effect_init_blur },
				};
			}
		}

		public readonly PvVisualEffectType fx;
		private int step = 0;
		private readonly int duration = 0;

		private bool horizontalFlip = false;
		private bool verticalFlip = false;
		private bool isNegative = false;
		private bool isReverse = false;
		private int rotationStep = 0;
		private int horizontalMirrored = 0;
		private int verticalMirrored = 0;
		private bool isInvertLumin = false;
		private bool isGrey = false;
		private readonly bool enableHueAdjust = false;
		private int obliqueDirect = 0;
		private int swingDirect = 0;
		private int puyoShape = 0;

		public bool HorizontalFlip { get { return horizontalFlip; } }
		public bool VerticalFlip { get { return verticalFlip; } }
		public bool IsNegative { get { return isNegative; } }
		public bool IsReverse { get { return isReverse; } }
		public double RotationDeg { get { return (double)rotationStep * Math.PI / 2; } }
		public int HorizontalMirrored { get { return horizontalMirrored; } }
		public int VerticalMirrored { get { return verticalMirrored; } }
		public bool IsInvertLumin { get { return isInvertLumin; } }
		public double Hue { get { return enableHueAdjust ? (double)step / (double)duration : 0; } }
		public bool IsGrey { get { return isGrey; } }
		public bool IsSingleDuration { get { return duration == 1; } }
		public int ObliqueDirect { get { return obliqueDirect; } }
		public int SwingDirect { get { return swingDirect; } }
		public int PuyoShape { get { return puyoShape; } }

		public PvVisualEffect(PvVisualEffectType fx, int initStep = 0) {
			this.fx = fx;
			step = initStep;
			duration = InitialValues[(int)fx].Length;
			if (fx >= PvVisualEffectType.HUE_INVERT && fx <= PvVisualEffectType.STEP_8_CHANGE_HUE) enableHueAdjust = true;
			Update();
		}
		private void NextStep() {
			step = (step + 1) % duration;
		}
		/// <summary>
		/// 下一步节奏视觉效果。
		/// </summary>
		public void Next() {
			NextStep();
			Update();
		}
		private void Update() {
			switch (fx) {
				case PvVisualEffectType.H_FLIP:
					horizontalFlip = step == 1;
					break;
				case PvVisualEffectType.V_FLIP:
					verticalFlip = step == 1;
					break;
				case PvVisualEffectType.NEGATIVE:
					isNegative = step == 1;
					break;
				case PvVisualEffectType.PINGPONG:
					isReverse = step == 1;
					break;
				case PvVisualEffectType.CCW_FLIP:
					horizontalFlip = step == 1 || step == 2;
					verticalFlip = step == 2 || step == 3;
					break;
				case PvVisualEffectType.CW_FLIP:
					horizontalFlip = step == 2 || step == 3;
					verticalFlip = step == 1 || step == 2;
					break;
				case PvVisualEffectType.CCW_ROTATE:
					rotationStep = step;
					break;
				case PvVisualEffectType.CW_ROTATE:
					rotationStep = (4 - step) % 4;
					break;
				case PvVisualEffectType.TURNED:
					rotationStep = step * 2;
					break;
				case PvVisualEffectType.H_MIRROR:
					horizontalMirrored = step == 0 ? 1 : 2;
					break;
				case PvVisualEffectType.V_MIRROR:
					verticalMirrored = step == 0 ? 1 : 2;
					break;
				case PvVisualEffectType.CCW_MIRROR:
					horizontalMirrored = (step == 0 || step == 1) ? 1 : 2;
					verticalMirrored = (step == 0 || step == 3) ? 1 : 2;
					break;
				case PvVisualEffectType.CW_MIRROR:
					horizontalMirrored = (step == 0 || step == 3) ? 1 : 2;
					verticalMirrored = (step == 0 || step == 1) ? 1 : 2;
					break;
				case PvVisualEffectType.LUMIN_INVERT:
					isInvertLumin = step == 1;
					break;
				case PvVisualEffectType.GREY:
					isGrey = step == 1;
					break;
				case PvVisualEffectType.WHIRL:
					horizontalFlip = isReverse = step == 1;
					break;
				case PvVisualEffectType.OBLIQUE_EXPANSION_AND_COMPRESSION:
					obliqueDirect = step == 0 ? -1 : step == 1 ? 1 : 0;
					break;
				case PvVisualEffectType.PUYO_PUYO:
					puyoShape = step == 0 ? -1 : step == 1 ? 1 : 0;
					break;
				case PvVisualEffectType.PENDULUM:
					swingDirect = step == 0 ? 1 : step == 1 ? -1 : 0;
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
		private static Vegas vegas;
		/// <summary> 移调 </summary>
		public static PlugInNode pitchShift;
		/// <summary> 反转 </summary>
		public static PlugInNode invert;
		/// <summary> HSL 调整 </summary>
		public static PlugInNode hslAdjust;
		/// <summary> LAB 调整 </summary>
		public static PlugInNode labAdjust;
		/// <summary> 黑白 </summary>
		public static PlugInNode blackAndWhite;
		/// <summary> 镜像 </summary>
		public static PlugInNode mirror;
		/// <summary> 画中画 </summary>
		public static PlugInNode picInPic;
		/// <summary> 修剪 </summary>
		public static PlugInNode crop;
		/// <summary> 纯色 </summary>
		public static PlugInNode solidColor;
		/// <summary> 亮度和对比度 </summary>
		public static PlugInNode contrast;
		/// <summary> 合唱 </summary>
		public static PlugInNode chorus;
		/// <summary> 颤音 1 </summary>
		public static PlugInNode vibrato1;
		/// <summary> 颤音 2 </summary>
		public static PlugInNode vibrato2;
		/// <summary> 波浪 </summary>
		public static PlugInNode wave;
		/// <summary> 延迟 </summary>
		public static PlugInNode delay;
		/// <summary> 球面化 </summary>
		public static PlugInNode spherize;
		/// <summary> 高斯模糊 </summary>
		public static PlugInNode gaussianBlur;
		/// <summary> 径向模糊 </summary>
		public static PlugInNode radialBlur;
		/// <summary> 字幕和文字 </summary>
		public static PlugInNode titlesAndText;

		/// <summary>
		/// 初始化所需插件。
		/// </summary>
		/// <param name="vegas">Vegas 软件</param>
		public static void Init(Vegas vegas) {
			Plugin.vegas = vegas;
			if (isInit) return;

			#region 音频 FX
			pitchShift = vegas.AudioFX.FindChildByName("移调")
				?? vegas.AudioFX.FindChildByName("Pitch Shift")
				?? vegas.AudioFX.FindChildByUniqueID("{ED1B4100-93BE-11D0-AEBC-00A0C9053912}");
			chorus = vegas.AudioFX.FindChildByName("合唱")
				?? vegas.AudioFX.FindChildByName("Chorus")
				?? vegas.AudioFX.FindChildByUniqueID("{28D9F1E0-6ECC-11D0-AEBC-00A0C9053912}");
			delay = vegas.AudioFX.FindChildByName("延迟")
				?? vegas.AudioFX.FindChildByUniqueID("{7298A3E0-78EE-11D0-AEBC-00A0C9053912}");
			vibrato1 = vegas.AudioFX.FindChildByUniqueID("{3F901A20-79BE-11D0-AEBC-00A0C9053912}");
			vibrato2 = vegas.AudioFX.FindChildByUniqueID("{D6802BA0-A056-11D0-AEBC-00A0C9053912}");
			#endregion

			#region 视频 FX
			invert = vegas.VideoFX.FindChildByName("反转")
				?? vegas.VideoFX.FindChildByName("Invert")
				?? vegas.VideoFX.FindChildByName("VEGAS 反转")
				?? vegas.VideoFX.FindChildByName("VEGAS Invert")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:invert}");
			hslAdjust = vegas.VideoFX.FindChildByName("HSL 调整")
				?? vegas.VideoFX.FindChildByName("HSL Adjust")
				?? vegas.VideoFX.FindChildByName("VEGAS HSL 调整")
				?? vegas.VideoFX.FindChildByName("VEGAS HSL Adjust")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:hsladjust}");
			labAdjust = vegas.VideoFX.FindChildByName("LAB 调整")
				?? vegas.VideoFX.FindChildByName("LAB Adjust")
				?? vegas.VideoFX.FindChildByName("VEGAS LAB 调整")
				?? vegas.VideoFX.FindChildByName("VEGAS LAB Adjust")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:labadjust}");
			blackAndWhite = vegas.VideoFX.FindChildByName("黑白")
				?? vegas.VideoFX.FindChildByName("Black and White")
				?? vegas.VideoFX.FindChildByName("VEGAS 黑白")
				?? vegas.VideoFX.FindChildByName("VEGAS Black and White")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:blackandwhite}");
			mirror = vegas.VideoFX.FindChildByName("镜像")
				?? vegas.VideoFX.FindChildByName("Mirror")
				?? vegas.VideoFX.FindChildByName("VEGAS 镜像")
				?? vegas.VideoFX.FindChildByName("VEGAS Mirror")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:mirror}");
			picInPic = vegas.VideoFX.FindChildByName("画中画")
				?? vegas.VideoFX.FindChildByName("Picture In Picture")
				?? vegas.VideoFX.FindChildByName("VEGAS 画中画")
				?? vegas.VideoFX.FindChildByName("VEGAS Picture In Picture")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:pictureinpicture}");
			crop = vegas.VideoFX.FindChildByName("修剪")
				?? vegas.VideoFX.FindChildByName("Crop")
				?? vegas.VideoFX.FindChildByName("VEGAS 修剪")
				?? vegas.VideoFX.FindChildByName("VEGAS Crop")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:crop}");
			contrast = vegas.VideoFX.FindChildByName("亮度和对比度")
				?? vegas.VideoFX.FindChildByName("Brightness and Contrast")
				?? vegas.VideoFX.FindChildByName("VEGAS 亮度和对比度")
				?? vegas.VideoFX.FindChildByName("VEGAS Brightness and Contrast")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:brightnessandcontrast}");
			wave = vegas.VideoFX.FindChildByName("波浪")
				?? vegas.VideoFX.FindChildByName("Wave")
				?? vegas.VideoFX.FindChildByName("VEGAS 波浪")
				?? vegas.VideoFX.FindChildByName("VEGAS Wave")
				?? vegas.VideoFX.FindChildByUniqueID("{EC1A2314-0C38-11D2-9AAC-00A0C99B12C5}");
			spherize = vegas.VideoFX.FindChildByName("球面化")
				?? vegas.VideoFX.FindChildByName("Spherize")
				?? vegas.VideoFX.FindChildByName("VEGAS 球面化")
				?? vegas.VideoFX.FindChildByName("VEGAS Spherize")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:spherize}");
			gaussianBlur = vegas.VideoFX.FindChildByName("高斯模糊")
				?? vegas.VideoFX.FindChildByName("Gaussian Blur")
				?? vegas.VideoFX.FindChildByName("VEGAS 高斯模糊")
				?? vegas.VideoFX.FindChildByName("VEGAS Gaussian Blur")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:gaussianblur}");
			radialBlur = vegas.VideoFX.FindChildByName("径向模糊")
				?? vegas.VideoFX.FindChildByName("Radial Blur")
				?? vegas.VideoFX.FindChildByName("VEGAS 径向模糊")
				?? vegas.VideoFX.FindChildByName("VEGAS Radial Blur")
				?? vegas.VideoFX.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:radialblur}");
			#endregion

			#region 媒体发生器
			solidColor = vegas.Generators.FindChildByName("纯色")
				?? vegas.Generators.FindChildByName("Solid Color")
				?? vegas.Generators.FindChildByName("VEGAS 纯色")
				?? vegas.Generators.FindChildByName("VEGAS Solid Color")
				?? vegas.Generators.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:solidcolor}");
			titlesAndText = vegas.Generators.FindChildByName("字幕和文字")
				?? vegas.Generators.FindChildByName("Titles & Text")
				?? vegas.Generators.FindChildByName("VEGAS 字幕和文字")
				?? vegas.Generators.FindChildByName("VEGAS Titles & Text")
				?? vegas.Generators.FindChildByUniqueID("{Svfx:com.vegascreativesoftware:titlesandtext}");
			#endregion

			isInit = true;
		}
		private static bool isInit = false;
		/// <summary>
		/// 针对视频事件的效果。
		/// </summary>
		public static class ForVideoEvents {
			/// <summary>
			/// 应用反转颜色效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void Negative(VideoEvent videoEvent) {
				videoEvent.Effects.AddEffect(invert);
			}
			/// <summary>
			/// 应用黑白效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void Grey(VideoEvent videoEvent) {
				videoEvent.Effects.AddEffect(blackAndWhite);
			}
			/// <summary>
			/// 应用反转亮度效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void InvertLumin(VideoEvent videoEvent) {
				Effect effect = videoEvent.Effects.AddEffect(labAdjust);
				(effect.OFXEffect.FindParameterByName("InvertLuminance") as OFXBooleanParameter).Value = true;
			}
			/// <summary>
			/// 应用改变色相效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="value">色相值</param>
			public static void ChangeHue(VideoEvent videoEvent, double value) {
				Effect effect = videoEvent.Effects.AddEffect(hslAdjust);
				(effect.OFXEffect.FindParameterByName("AddToHue") as OFXDoubleParameter).Value = value;
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
					Effect effect = videoEvent.Effects.AddEffect(mirror);
					(effect.OFXEffect.FindParameterByName("Angle") as OFXDoubleParameter).Value =
						horizontalDirect == 1 ? MIRROR_LEFT : MIRROR_RIGHT;
				}
				if (verticalDirect != 0) {
					Effect effect = videoEvent.Effects.AddEffect(mirror);
					(effect.OFXEffect.FindParameterByName("Angle") as OFXDoubleParameter).Value =
						verticalDirect == 1 ? MIRROR_UP : MIRROR_DOWN;
				}
			}
			/// <summary>
			/// 应用发光效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="duration">发光持续长度。如果为正表示发光淡入，为负表示发光淡出。</param>
			/// <param name="curve">缓动插值类型</param>
			/// <param name="brightness">发光亮度</param>
			public static void Glow(VideoEvent videoEvent, int duration, OFXInterpolationType curve, int brightness) {
				if (duration == 0) return;
				double length = videoEvent.Length.ToMilliseconds(), dur, bright = brightness / 100.0;
				Effect effect = videoEvent.Effects.AddEffect(contrast);
				OFXDoubleParameter
					brightnessParam = effect.OFXEffect.FindParameterByName("Brightness") as OFXDoubleParameter,
					contrastParam = effect.OFXEffect.FindParameterByName("Contrast") as OFXDoubleParameter;
				List<OFXDoubleParameter> @params = new List<OFXDoubleParameter> { brightnessParam };
				if (brightness > 0) @params.Add(contrastParam);
				foreach (OFXDoubleParameter param in @params) {
					param.IsAnimated = true;
					if (duration > 0) {
						dur = duration / 100.0 * length;
						param.SetValueAtTime(Timecode.FromMilliseconds(0), bright);
						param.SetValueAtTime(Timecode.FromMilliseconds(dur), 0);
						param.Keyframes[0].Interpolation = curve;
					} else {
						dur = (1.0 + duration / 100.0) * length;
						param.SetValueAtTime(Timecode.FromMilliseconds(0), 0);
						param.SetValueAtTime(Timecode.FromMilliseconds(dur), 0);
						param.SetValueAtTime(Timecode.FromMilliseconds(length), bright);
						param.Keyframes[0 != dur ? 1 : 0].Interpolation = curve;
					}
				}
			}
			/// <summary>
			/// 应用波浪效果。
			/// 由于该效果插件不是 OFX 插件，只能使用自带预设。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void Wave(VideoEvent videoEvent) {
				Effect effect = videoEvent.Effects.AddEffect(wave);
				ForYtps.GetRandomPreset(effect);
			}
			/// <summary>
			/// 应用旋转色相效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void RotateHue(VideoEvent videoEvent) {
				Effect effect = videoEvent.Effects.AddEffect(hslAdjust);
				OFXDoubleParameter param = effect.OFXEffect.FindParameterByName("AddToHue") as OFXDoubleParameter;
				param.IsAnimated = true;
				param.SetValueAtTime(Timecode.FromMilliseconds(0), 0);
				param.SetValueAtTime(videoEvent.Length, 1);
			}
			/// <summary>
			/// 应用放大动画效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			public static void Enlarge(VideoEvent videoEvent) {
				const int FINAL_SIZE = 100;
				videoEvent.VideoMotion.Keyframes[0].Type = VideoKeyframeType.Slow;
				videoEvent.VideoMotion.Keyframes[0].Smoothness = 0;
				VideoMotionKeyframe final = new VideoMotionKeyframe(videoEvent.Length);
				videoEvent.VideoMotion.Keyframes.Add(final);
				float width = final.BottomRight.X - final.BottomLeft.X;
				float height = final.BottomRight.Y - final.TopRight.Y;
				float newWidth = FINAL_SIZE, newHeight = FINAL_SIZE;
				if (width <= height) newWidth = newHeight / height * width;
				else newHeight = newWidth / width * height;
				float randX = RandomFloatBetween(0, width - newWidth);
				float randY = RandomFloatBetween(0, height - newHeight);
				final.Bounds = GetARectangleBounds(randX, randY, newWidth, newHeight);
			}
			/// <summary>
			/// 应用球面化效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="amount">数量</param>
			public static void Spherize(VideoEvent videoEvent, double amount = 1) {
				Effect effect = videoEvent.Effects.AddEffect(spherize);
				(effect.OFXEffect.FindParameterByName("Amount") as OFXDoubleParameter).Value = amount;
			}
			/// <summary>
			/// 应用高对比效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="center">对比度中心</param>
			public static void Contrast(VideoEvent videoEvent, double center = 0.5) {
				Effect effect = videoEvent.Effects.AddEffect(contrast);
				(effect.OFXEffect.FindParameterByName("Contrast") as OFXDoubleParameter).Value = 1;
				(effect.OFXEffect.FindParameterByName("ContrastCenter") as OFXDoubleParameter).Value = center;
			}
			/// <summary>
			/// 应用饱和度效果。
			/// </summary>
			/// <param name="videoEvent">视频事件</param>
			/// <param name="saturation">饱和度</param>
			public static void Saturate(VideoEvent videoEvent, double saturation = 2) {
				Effect effect = videoEvent.Effects.AddEffect(hslAdjust);
				(effect.OFXEffect.FindParameterByName("Saturation") as OFXDoubleParameter).Value = saturation;
			}
			/// <summary>
			/// 应用放大并突出重点效果。
			/// </summary>
			/// <param name="videoEvents">视频事件列表</param>
			public static void Focus(List<VideoEvent> videoEvents) {
				int length;
				if (videoEvents == null || (length = videoEvents.Count) == 0) return;
				VideoMotionKeyframe firstFrame = videoEvents[0].VideoMotion.Keyframes[0];
				float width = firstFrame.BottomRight.X - firstFrame.BottomLeft.X,
					height = firstFrame.BottomRight.Y - firstFrame.TopRight.Y,
					topLeftX = firstFrame.TopLeft.X, topLeftY = firstFrame.TopLeft.Y;
				float finalRatio = RandomFloatBetween(0.5f, 0.75f);
				float finalWidth = width * finalRatio, finalHeight = height * finalRatio,
					finalTopLeftX = RandomFloatBetween(0, width - finalWidth) + topLeftX,
					finalTopLeftY = RandomFloatBetween(0, height - finalHeight) + topLeftY;
				for (int i = 1; i < length; i++) {
					VideoMotionKeyframe frame = videoEvents[i].VideoMotion.Keyframes[0];
					double progress = (double)i / (length - 1);
					frame.Bounds = GetARectangleBounds(
						GetPercentageInRange(topLeftX, finalTopLeftX, progress),
						GetPercentageInRange(topLeftY, finalTopLeftY, progress),
						GetPercentageInRange(width, finalWidth, progress),
						GetPercentageInRange(height, finalHeight, progress)
					);
				}
			}
			/// <summary>
			/// 垂直扩缩效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="type">所选的 PV 节奏视觉效果。必须是垂直扩缩类中的效果才可使用。</param>
			/// <param name="scale">缩小比例。<br /><strong>V4 (WinForm) 版本暂不支持修改。</strong></param>
			public static void Expansion(VideoEvent videoEvent, PvVisualEffectType type, double scale = 0.8) {
				if (type < PvVisualEffectType.VERTICAL_EXPANSION || type > PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND) return;
				Timecode startTime = Timecode.FromMilliseconds(0), endTime = videoEvent.Length, centerTime = Timecode.FromMilliseconds(videoEvent.Length.ToMilliseconds() / 2);
				Effect effect = videoEvent.Effects.AddEffect(picInPic);
				OFXChoiceParameter keepProp = effect.OFXEffect.FindParameterByName("KeepProportions") as OFXChoiceParameter;
				keepProp.Value = keepProp.Choices[1];
				(effect.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter).Value = 1;
				OFXDoubleParameter scaleYParam = effect.OFXEffect.FindParameterByName("DistortionScaleY") as OFXDoubleParameter;
				bool isCompression = type == PvVisualEffectType.VERTICAL_COMPRESSION || type == PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND;
				scaleYParam.Value = isCompression ? 1 : scale; // 如果不设置该项，会出现闪屏错误。
				OFXDouble2DParameter locationParam = effect.OFXEffect.FindParameterByName("Location") as OFXDouble2DParameter;
				Func<bool, OFXDouble2D> setLocationIsMax = max => max ? new OFXDouble2D { X = 0.5, Y = 0.5 } : new OFXDouble2D { X = 0.5, Y = scale / 2 };
				locationParam.Value = setLocationIsMax(isCompression);
				scaleYParam.IsAnimated = true;
				locationParam.IsAnimated = true;
				Action<Timecode, bool> setY = (timecode, max) => {
					scaleYParam.SetValueAtTime(timecode, max ? 1 : scale);
					locationParam.SetValueAtTime(timecode, setLocationIsMax(max));
				};
				if (type == PvVisualEffectType.VERTICAL_EXPANSION || type == PvVisualEffectType.VERTICAL_COMPRESSION) {
					setY(startTime, type == PvVisualEffectType.VERTICAL_COMPRESSION);
					setY(endTime, type == PvVisualEffectType.VERTICAL_EXPANSION);
				} else {
					setY(startTime, type == PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND);
					setY(centerTime, type == PvVisualEffectType.VERTICAL_EXPANSION_WITH_REBOUND);
					setY(endTime, type == PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND);
				}
				scaleYParam.Keyframes[0].Interpolation = locationParam.Keyframes[0].Interpolation = OFXInterpolationType.Fast;
				scaleYParam.Keyframes[1].Interpolation = locationParam.Keyframes[1].Interpolation = OFXInterpolationType.Slow;
			}
			/// <summary>
			/// 斜向扩缩效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="direct">变形方向。<br />正数表示向右变形，负数表示向左变形，0 表示不变形。</param>
			/// <param name="scale">缩小比例。<br /><strong>V4 (WinForm) 版本暂不支持修改。</strong></param>
			public static void ObliqueExpansion(VideoEvent videoEvent, int direct, double scale = 0.7) {
				if ((direct = Math.Sign(direct)) == 0) return;
				VideoStream stream = videoEvent.ActiveTake.MediaStream as VideoStream;
				int width = stream.Width, height = stream.Height;
				//int width = 16, height = 9;
				double radAngle = Math.Atan2((width - width * scale) / 2, height);
				double angle = radAngle / Math.PI * 180;
				double shear = Math.Tan(radAngle);
				Timecode startTime = Timecode.FromMilliseconds(0), endTime = videoEvent.Length, centerTime = Timecode.FromMilliseconds(videoEvent.Length.ToMilliseconds() / 2);
				Effect effect = videoEvent.Effects.AddEffect(picInPic);
				{
					OFXChoiceParameter keepProp = effect.OFXEffect.FindParameterByName("KeepProportions") as OFXChoiceParameter;
					keepProp.Value = keepProp.Choices[1];
					OFXDoubleParameter angleParam = effect.OFXEffect.FindParameterByName("Angle") as OFXDoubleParameter,
						scaleXParam = effect.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter,
						scaleYParam = effect.OFXEffect.FindParameterByName("DistortionScaleY") as OFXDoubleParameter,
						shearParam = effect.OFXEffect.FindParameterByName("DistortionShear") as OFXDoubleParameter;
					OFXDouble2DParameter locationParam = effect.OFXEffect.FindParameterByName("Location") as OFXDouble2DParameter;
					OFXParameter[] @params = { angleParam, scaleXParam, scaleYParam, shearParam, locationParam };
					foreach (OFXParameter param in @params)
						param.IsAnimated = true;
					angleParam.SetValueAtTime(startTime, direct * angle);
					scaleXParam.SetValueAtTime(startTime, scale * Math.Cos(radAngle));
					scaleYParam.SetValueAtTime(startTime, 1 / Math.Cos(radAngle));
					shearParam.SetValueAtTime(startTime, -direct * shear);
					locationParam.SetValueAtTime(direct < 0 ? startTime : endTime, new OFXDouble2D { X = (1 + scale) / 4, Y = 0.5 });
					angleParam.SetValueAtTime(centerTime, 0);
					scaleXParam.SetValueAtTime(centerTime, scale);
					scaleYParam.SetValueAtTime(centerTime, 1);
					shearParam.SetValueAtTime(centerTime, 0);
					angleParam.SetValueAtTime(endTime, -direct * angle);
					scaleXParam.SetValueAtTime(endTime, scaleXParam.GetValueAtTime(startTime));
					scaleYParam.SetValueAtTime(endTime, scaleYParam.GetValueAtTime(startTime));
					shearParam.SetValueAtTime(endTime, direct * shear);
					locationParam.SetValueAtTime(direct > 0 ? startTime : endTime, new OFXDouble2D { X = 1 - (1 + scale) / 4, Y = 0.5 });
					foreach (OFXParameter _param in @params) {
						if (_param is OFXDoubleParameter) {
							OFXDoubleParameter param = _param as OFXDoubleParameter;
							foreach (OFXDoubleKeyframe keyframe in param.Keyframes)
								keyframe.Interpolation = OFXInterpolationType.Linear;
						}
						if (_param is OFXDouble2DParameter) {
							OFXDouble2DParameter param = _param as OFXDouble2DParameter;
							foreach (OFXDouble2DKeyframe keyframe in param.Keyframes)
								keyframe.Interpolation = OFXInterpolationType.Linear;
						}
					}
				}
				Expansion(videoEvent, PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND, scale);
			}
			/// <summary>
			/// 噗哟噗哟 / 魔法气泡效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="shape">变形形状。<br />正数表示变细，负数表示变扁，0 表示不变。</param>
			/// <param name="amount">变形的比率。<br /><strong>V4 (WinForm) 版本暂不支持修改。</strong></param>
			public static void PuyoPuyo(VideoEvent videoEvent, int shape, double amount = 0.625) {
				if ((shape = Math.Sign(shape)) == 0) return;
				Effect effect = videoEvent.Effects.AddEffect(picInPic);
				OFXChoiceParameter keepProp = effect.OFXEffect.FindParameterByName("KeepProportions") as OFXChoiceParameter;
				keepProp.Value = keepProp.Choices[1];
				OFXDoubleParameter scaleX = effect.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter;
				OFXDoubleParameter scaleY = effect.OFXEffect.FindParameterByName("DistortionScaleY") as OFXDoubleParameter;
				foreach (OFXDoubleParameter scale in new OFXDoubleParameter[] { scaleX, scaleY }) {
					scale.IsAnimated = true;
					scale.SetValueAtTime(Timecode.FromMilliseconds(0),
						scale == scaleX ? shape < 0 ? amount : 1 :
						shape > 0 ? amount : 1
					);
					scale.SetValueAtTime(videoEvent.Length,
						scale == scaleX ? shape > 0 ? amount : 1 :
						shape < 0 ? amount : 1
					);
					scale.Keyframes[0].Interpolation = OFXInterpolationType.Fast;
				}
			}
			/// <summary>
			/// 钟摆 / 左右摇摆效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="direct">摇摆方向。<br />正数表示向右摆动，负数表示向左摆动，0 表示不摆动。</param>
			/// <param name="angle">摆动的角度。<br /><strong>V4 (WinForm) 版本暂不支持修改。</strong></param>
			public static void Pendulum(VideoEvent videoEvent, int direct, double angle = 15) {
				if ((direct = Math.Sign(direct)) == 0) return;
				double radAngle = angle * Math.PI / 180;
				Effect effect = videoEvent.Effects.AddEffect(picInPic);
				VideoStream stream = videoEvent.ActiveTake.MediaStream as VideoStream;
				int width = stream.Width, height = stream.Height;
				double maxRadAngle = Math.Atan2(width, height);
				if (Math.Abs(radAngle) > maxRadAngle) radAngle = Math.Sign(radAngle) * maxRadAngle;
				(effect.OFXEffect.FindParameterByName("Scale") as OFXDoubleParameter).Value = height / (width * Math.Sin(radAngle) + height * Math.Cos(radAngle));
				OFXDoubleParameter angleParam = effect.OFXEffect.FindParameterByName("Angle") as OFXDoubleParameter;
				angleParam.IsAnimated = true;
				angleParam.SetValueAtTime(Timecode.FromMilliseconds(0), -direct * angle);
				angleParam.SetValueAtTime(videoEvent.Length, direct * angle);
				angleParam.Keyframes[0].Interpolation = OFXInterpolationType.Fast;
			}
			/// <summary>
			/// 高斯模糊效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="range">模糊级别范围。</param>
			public static void GaussianBlur(VideoEvent videoEvent, double range = 0.1) {
				Effect effect = videoEvent.Effects.AddEffect(gaussianBlur);
				foreach (OFXDoubleParameter param in new OFXDoubleParameter[] { effect.OFXEffect.FindParameterByName("HorizontalRange") as OFXDoubleParameter, effect.OFXEffect.FindParameterByName("VerticalRange") as OFXDoubleParameter }) {
					param.IsAnimated = true;
					param.SetValueAtTime(Timecode.FromMilliseconds(0), range);
					param.SetValueAtTime(videoEvent.Length, 0);
					param.Keyframes[0].Interpolation = OFXInterpolationType.Fast;
				}
			}
			/// <summary>
			/// 径向模糊效果。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="range">模糊级别范围。</param>
			public static void RadialBlur(VideoEvent videoEvent, double range = 0.8) {
				Effect effect = videoEvent.Effects.AddEffect(radialBlur);
				OFXChoiceParameter type = effect.OFXEffect.FindParameterByName("Type") as OFXChoiceParameter;
				type.Value = type.Choices[1];
				OFXDoubleParameter strength = effect.OFXEffect.FindParameterByName("Strength") as OFXDoubleParameter;
				strength.IsAnimated = true;
				strength.SetValueAtTime(Timecode.FromMilliseconds(0), range);
				strength.SetValueAtTime(videoEvent.Length, 0);
				strength.Keyframes[0].Interpolation = OFXInterpolationType.Linear;
			}
			/// <summary>
			/// 应用色相和饱和度的参数。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="startHue">起始色相。输入范围：0~360；输出范围：0~1。</param>
			/// <param name="endHue">终止色相。输入范围：0~360；输出范围：0~1。</param>
			/// <param name="startSaturation">起始饱和度。输入范围：0~200；输出范围：0~2。</param>
			/// <param name="endSaturation">终止饱和度。输入范围：0~200；输出范围：0~2。</param>
			/// <param name="startHueCurve">起始色相曲线。</param>
			/// <param name="startSaturationCurve">起始饱和度曲线。</param>
			public static void HueAndSaturationParam(VideoEvent videoEvent, int startHue, int endHue, int startSaturation, int endSaturation, OFXInterpolationType startHueCurve, OFXInterpolationType startSaturationCurve) {
				Effect effect = null;
				double _startHue = startHue / 360.0, _endHue = endHue / 360.0,
					_startSaturation = startSaturation / 100.0, _endSaturation = endSaturation / 100.0;
				if (!(startHue == 0 && endHue == 0 || startHue == 360 && endHue == 360)) {
					effect = videoEvent.Effects.AddEffect(hslAdjust);
					OFXDoubleParameter hue = effect.OFXEffect.FindParameterByName("AddToHue") as OFXDoubleParameter;
					if (startHue == endHue) hue.Value = _startHue;
					else {
						hue.IsAnimated = true;
						hue.SetValueAtTime(Timecode.FromMilliseconds(0), _startHue);
						hue.SetValueAtTime(videoEvent.Length, _endHue);
						hue.Keyframes[0].Interpolation = startHueCurve;
					}
				}
				if (startSaturation != 100 || endSaturation != 100) {
					if (effect == null) effect = videoEvent.Effects.AddEffect(hslAdjust);
					OFXDoubleParameter saturation = effect.OFXEffect.FindParameterByName("Saturation") as OFXDoubleParameter;
					if (startSaturation == endSaturation) saturation.Value = _startSaturation;
					else {
						saturation.IsAnimated = true;
						saturation.SetValueAtTime(Timecode.FromMilliseconds(0), _startSaturation);
						saturation.SetValueAtTime(videoEvent.Length, _endSaturation);
						saturation.Keyframes[0].Interpolation = startSaturationCurve;
					}
				}
			}
			/// <summary>
			/// 应用对比度和对比度中心（阈值）的参数。
			/// </summary>
			/// <param name="videoEvent">视频事件。</param>
			/// <param name="startContrast">起始对比度。输入范围：-100~100；输出范围：-1~1。</param>
			/// <param name="endContrast">终止对比度。输入范围：-100~100；输出范围：-1~1。</param>
			/// <param name="startThreshold">起始对比度中心。输入范围：0~100；输出范围：0~1。</param>
			/// <param name="endThreshold">终止对比度中心。输入范围：0~100；输出范围：0~1。</param>
			/// <param name="startContrastCurve">起始对比度曲线。</param>
			/// <param name="startThresholdCurve">起始对比度中心曲线。</param>
			public static void ContrastAndThresholdParam(VideoEvent videoEvent, int startContrast, int endContrast, int startThreshold, int endThreshold, OFXInterpolationType startContrastCurve, OFXInterpolationType startThresholdCurve) {
				double _startContrast = startContrast / 100.0, _endContrast = endContrast / 100.0,
					_startThreshold = startThreshold / 100.0, _endThreshold = endThreshold / 100.0;
				if (startContrast != 0 || endContrast != 0) {
					Effect effect = videoEvent.Effects.AddEffect(contrast);
					OFXDoubleParameter contrastParam = effect.OFXEffect.FindParameterByName("Contrast") as OFXDoubleParameter;
					if (startContrast == endContrast) contrastParam.Value = _startContrast;
					else {
						contrastParam.IsAnimated = true;
						contrastParam.SetValueAtTime(Timecode.FromMilliseconds(0), _startContrast);
						contrastParam.SetValueAtTime(videoEvent.Length, _endContrast);
						contrastParam.Keyframes[0].Interpolation = startContrastCurve;
					}
					OFXDoubleParameter contrastCenter = effect.OFXEffect.FindParameterByName("ContrastCenter") as OFXDoubleParameter;
					if (startThreshold == endThreshold) contrastCenter.Value = _startThreshold;
					else {
						contrastCenter.IsAnimated = true;
						contrastCenter.SetValueAtTime(Timecode.FromMilliseconds(0), _startThreshold);
						contrastCenter.SetValueAtTime(videoEvent.Length, _endThreshold);
						contrastCenter.Keyframes[0].Interpolation = startThresholdCurve;
					}
				}
			}
		}

		/// <summary>
		/// 针对轨道的效果。
		/// </summary>
		public static class ForVideoTracks {
			private static void GraduallyBase(Track[] tracks, bool isReversed, Action<Track, double> action) {
				for (int i = 0, j = tracks.Length - 1; i < tracks.Length; i++, j--) {
					Track track = tracks[!isReversed ? i : j];
					// double value = (double)(!isReversed ? i : tracks.Length - i) / tracks.Length;
					double value = (double)i / tracks.Length;
					action(track, value);
				}
			}
			private static void GraduallyBaseMap(Track[] tracks, bool isReversed, double min, double max, Action<Track, double> action) {
				for (int i = 0; i < tracks.Length; i++) {
					Track track = tracks[i];
					double value = (double)(!isReversed ? i : tracks.Length - 1 - i) / (tracks.Length - 1);
					value = value * (max - min) + min;
					action(track, value);
				}
			}
			private static void AlternatelyBase(Track[] tracks, bool isAlternative, Action<Track> action) {
				for (int i = 0; i < tracks.Length; i++) {
					Track track = tracks[i];
					if (i % 2 == (isAlternative ? 1 : 0)) action(track);
				}
			}
			/// <summary>
			/// 彩虹渐变
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isReversed">是否倒序</param>
			public static void Rainbow(Track[] tracks, bool isReversed = false) {
				GraduallyBase(tracks, isReversed, (track, value) => {
					Effect effect = track.Effects.AddEffect(hslAdjust);
					(effect.OFXEffect.FindParameterByName("AddToHue") as OFXDoubleParameter).Value = value;
				});
			}
			/// <summary>
			/// 逐渐饱和
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isReversed">是否倒序</param>
			public static void Saturated(Track[] tracks, bool isReversed = false) {
				const double MIN = 0, MAX = 2;
				GraduallyBaseMap(tracks, isReversed, MIN, MAX, (track, value) => {
					Effect effect = track.Effects.AddEffect(hslAdjust);
					(effect.OFXEffect.FindParameterByName("Saturation") as OFXDoubleParameter).Value = value;
				});
			}
			/// <summary>
			/// 逐渐对比
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isReversed">是否倒序</param>
			public static void Contrasted(Track[] tracks, bool isReversed = false) {
				const double MIN = 0, MAX = 1;
				GraduallyBaseMap(tracks, isReversed, MIN, MAX, (track, value) => {
					Effect effect = track.Effects.AddEffect(contrast);
					(effect.OFXEffect.FindParameterByName("Contrast") as OFXDoubleParameter).Value = value;
				});
			}
			/// <summary>
			/// 阈值
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isReversed">是否倒序</param>
			public static void Threshold(Track[] tracks, bool isReversed = false) {
				const double MIN = 0.1, MAX = 0.9;
				GraduallyBaseMap(tracks, isReversed, MIN, MAX, (track, value) => {
					Effect effect = track.Effects.AddEffect(contrast); // 然而“阈值”效果不是 OFX 滤镜
					(effect.OFXEffect.FindParameterByName("Contrast") as OFXDoubleParameter).Value = 1;
					(effect.OFXEffect.FindParameterByName("ContrastCenter") as OFXDoubleParameter).Value = value;
				});
			}
			/// <summary>
			/// 交替彩灰
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isAlternative">替代顺序</param>
			public static void Chromatic(Track[] tracks, bool isAlternative = false) {
				AlternatelyBase(tracks, !isAlternative, track => {
					Effect effect = track.Effects.AddEffect(blackAndWhite);
				});
			}
			/// <summary>
			/// 交替正负
			/// </summary>
			/// <param name="tracks">轨道数组</param>
			/// <param name="isAlternative">替代顺序</param>
			public static void Negative(Track[] tracks, bool isAlternative = false) {
				AlternatelyBase(tracks, isAlternative, track => {
					Effect effect = track.Effects.AddEffect(invert);
				});
			}
		}
		/// <summary>
		/// 针对音频事件的效果。
		/// </summary>
		public static class ForAudioEvents {
			/// <summary>
			/// 应用合唱效果。
			/// </summary>
			/// <param name="audioEvent">音频事件</param>
			public static void Chorus(AudioEvent audioEvent) {
				audioEvent.Effects.AddEffect(chorus);
			}
			/// <summary>
			/// 应用颤音效果。
			/// </summary>
			/// <param name="audioEvent">音频事件/param>
			/// <param name="plugin">选择哪一种颤音效果插件</param>
			public static void Vibrato(AudioEvent audioEvent, PlugInNode plugin) {
				Effect effect = audioEvent.Effects.AddEffect(plugin);
				ForYtps.GetRandomPreset(effect);
			}
			/// <summary>
			/// 应用多拍延迟效果。
			/// </summary>
			/// <param name="audioEvent">音频事件</param>
			public static void Delay(AudioEvent audioEvent) {
				audioEvent.Effects.AddEffect(delay);
			}
		}
		/// <summary>
		/// 针对 YouTube Poop 的效果。
		/// </summary>
		public static class ForYtps {
			/// <summary>
			/// 获取随机效果。
			/// </summary>
			/// <param name="aEvent">音频事件</param>
			/// <param name="vEvent">视频事件</param>
			/// <param name="aConfig">包含音频</param>
			/// <param name="vConfig">包含视频</param>
			/// <param name="aReverse">倒放音频子剪辑</param>
			/// <param name="vReverse">倒放视频子剪辑</param>
			/// <param name="p">Vegas 入口类</param>
			/// <param name="effects">指定希望使用到的效果的数组</param>
			public static void GetRandomEffect(AudioEvent aEvent, VideoEvent vEvent, bool aConfig, bool vConfig, Subclip aReverse, Subclip vReverse, EntryPoint p, YtpEffectType[] effects = null) {
				YtpEffectType? effect;
				if (effects == null) effect = GetRandomYtpEffectType();
				else effect = GetRandomYtpEffectType(effects);
				if (effect == null) return;
				bool probably = RandomBool(); // 如果有可能的话。
				Action RestoreAudioEventMethod = new Action(() => {
					#if VER_GEQ_16
						aEvent.Method = TimeStretchPitchShift.Elastique;
						try {
							aEvent.PitchLock = aEvent.PitchLock;
						} catch (Exception e) {
							EntryPoint.ShowError(new Exceptions.NoTimeStretchPitchShiftException(), e);
						}
					#endif
				});
				switch (effect) {
					// 为什么 case 里面也打花括号？因为下面会出现一个在不同 case 里面定义相同标识符变量，结果报了重名错误的奇葩问题。
					case YtpEffectType.CHORUS: {
						if (aConfig) {
							if (chorus == null) WarningMissingPlugin(Lang.str.chorus);
							else ForAudioEvents.Chorus(aEvent);
						}
					} break;
					case YtpEffectType.PITCH_CHANGE: {
						if (aConfig) {
							int randomPitch = random.Next(-12, 12 + 1);
							randomPitch = (int)ExtremeFunction(randomPitch);
							RestoreAudioEventMethod();
							#if VER_GEQ_16
								aEvent.PitchSemis = randomPitch;
							#else
								SetPitchSemis_ForVersionsBelow16(aEvent, randomPitch);
							#endif
						}
					} break;
					case YtpEffectType.VIBRATO: {
						if (aConfig) {
							PlugInNode vibrato = random.Next(3) == 2 ? vibrato1 : vibrato2; // 有俩颤音插件，随便选一个。
							if (vibrato == null) WarningMissingPlugin(Lang.str.vibrato);
							else ForAudioEvents.Vibrato(aEvent, vibrato);
						}
						if (vConfig /* && probably */) {
							if (wave == null) WarningMissingPlugin(Lang.str.wave);
							else ForVideoEvents.Wave(vEvent);
						}
					} break;
					case YtpEffectType.REVERSE: {
						try {
							if (aConfig) {
								Timecode offset = aEvent.ActiveTake.Offset;
								aEvent.AddTake(aReverse.GetAudioStreamByIndex(0), true);
								aEvent.ActiveTake.Offset = offset;
							}
						} catch (Exception) { }
						try {
							if (vConfig) {
								Timecode offset = vEvent.ActiveTake.Offset;
								vEvent.AddTake(vReverse.GetVideoStreamByIndex(0), true);
								vEvent.ActiveTake.Offset = offset;
							}
						} catch (Exception) { }
					} break;
					case YtpEffectType.ROBOT_DELAY: {
						if (aConfig) {
							if (delay == null) WarningMissingPlugin(Lang.str.multi_beat_delay);
							else ForAudioEvents.Delay(aEvent);
						}
					} break;
					case YtpEffectType.SPEED_CHANGE: {
						double randomRate = RandomDoubleBetween(0.25, 4);
						if (aConfig)
							aEvent.AdjustPlaybackRate(randomRate, true);
						if (vConfig)
							vEvent.AdjustPlaybackRate(randomRate, true);
					} break;
					case YtpEffectType.HUE_CHANGE: {
						if (vConfig) {
							if (hslAdjust == null) WarningMissingPlugin(Lang.str.hsl_adjust);
							else ForVideoEvents.ChangeHue(vEvent, random.NextDouble());
						}
					} break;
					case YtpEffectType.HUE_ROTATE: {
						if (vConfig) {
							if (hslAdjust == null) WarningMissingPlugin(Lang.str.hsl_adjust);
							else ForVideoEvents.RotateHue(vEvent);
						}
					} break;
					case YtpEffectType.MONOCHROME: {
						if (vConfig) {
							if (blackAndWhite == null) WarningMissingPlugin(Lang.str.black_and_white);
							else ForVideoEvents.Grey(vEvent);
						}
					} break;
					case YtpEffectType.NEGATIVE: {
						if (vConfig) {
							if (invert == null) WarningMissingPlugin(Lang.str.invert);
							else ForVideoEvents.Negative(vEvent);
							if (probably) vEvent.AdjustPlaybackRate(0.5, true);
						}
						if (aConfig && probably) {
							RestoreAudioEventMethod();
							#if VER_GEQ_16
								aEvent.PitchLock = true;
							#endif
							aEvent.AdjustPlaybackRate(0.5, true);
						}
					} break;
					case YtpEffectType.HIGH_FREQ_REPEAT: {
						int repeatTimes = random.Next(MIN_HIGH_FREQ_REPEAT, MAX_HIGH_FREQ_REPEAT + 1);
						TrackEvent[] repeatClips = null;
						if (aConfig) {
							repeatClips = new AudioEvent[repeatTimes];
							aEvent.Length = Timecode.FromMilliseconds(HIGH_FREQ_REPEAT_FREQ);
							for (int i = 1; i < repeatTimes; i++)
								repeatClips[i] = aEvent.Copy(aEvent.Track, aEvent.Track.Length);
						}
						if (vConfig) {
							vEvent.Length = Timecode.FromMilliseconds(HIGH_FREQ_REPEAT_FREQ);
							for (int i = 1; i < repeatTimes; i++) {
								TrackEvent clip = vEvent.Copy(vEvent.Track, vEvent.Track.Length);
								if (repeatClips != null) p.GroupTrackEvents(clip, repeatClips[i]);
							}
						}
					} break;
					case YtpEffectType.RANDOM_TONE: {
						int count = random.Next(5, 10 + 1);
						Timecode shorterLength = Timecode.FromMilliseconds(random.Next(HIGH_FREQ_REPEAT_FREQ, MAX_PITCH_CHANGE_LENGTH));
						Timecode MAX_LENGTH = Timecode.FromMilliseconds(MAX_PITCH_CHANGE_LENGTH);
						AudioEvent[] repeatClips = null;
						if (aConfig) {
							repeatClips = new AudioEvent[count];
							if (aEvent.Length > MAX_LENGTH) aEvent.Length = shorterLength;
							RestoreAudioEventMethod();
							for (int i = 0; i < count; i++) {
								AudioEvent clip = aEvent;
								if (i != 0) {
									clip = aEvent.Copy(aEvent.Track, aEvent.Track.Length) as AudioEvent;
									repeatClips[i] = clip;
								}
								int randomPitch = random.Next(MIN_TONE_PITCH_CHANGE, MAX_TONE_PITCH_CHANGE + 1);
								#if VER_GEQ_16
									clip.PitchSemis = randomPitch;
								#else
									SetPitchSemis_ForVersionsBelow16(clip, randomPitch);
								#endif
							}
						}
						if (vConfig) {
							if (vEvent.Length > MAX_LENGTH) vEvent.Length = shorterLength;
							for (int i = 0; i < count; i++) {
								VideoEvent clip = vEvent;
								if (i != 0) {
									clip = vEvent.Copy(vEvent.Track, vEvent.Track.Length) as VideoEvent;
									if (repeatClips != null) p.GroupTrackEvents(clip, repeatClips[i]);
								}
								bool hFlip = i % 2 == 0;
								clip.FlipAllKeyframe(hFlip, false);
							}
						}
					} break;
					case YtpEffectType.ENLARGE: {
						if (aConfig) {
							aEvent.RecalculateNorm();
							aEvent.Normalize = true;
						}
						if (vConfig) ForVideoEvents.Enlarge(vEvent);
					} break;
					case YtpEffectType.SPHERIZE: {
						if (vConfig) {
							double amount = ExtremeFunction(RandomDoubleBetween(-1, 1));
							if (spherize == null) WarningMissingPlugin(Lang.str.spherize);
							else ForVideoEvents.Spherize(vEvent, amount);
						}
					} break;
					case YtpEffectType.MIRROR: {
						if (vConfig) {
							int i = random.Next(1, 3 * 3);
							if (mirror == null) WarningMissingPlugin(Lang.str.mirror);
							else ForVideoEvents.Mirror(vEvent, i % 3, i / 3);
						}
					} break;
					case YtpEffectType.HIGH_CONTRAST: {
						if (aConfig) {
							aEvent.RecalculateNorm();
							aEvent.Normalize = true;
						}
						if (vConfig) {
							double center = RandomDoubleBetween(0.4, 0.6);
							if (contrast == null) WarningMissingPlugin(Lang.str.brightness_and_contrast);
							else ForVideoEvents.Contrast(vEvent, center);
						}
					} break;
					case YtpEffectType.OVERSATURATION: {
						if (vConfig) {
							if (hslAdjust == null) WarningMissingPlugin(Lang.str.hsl_adjust);
							else ForVideoEvents.Saturate(vEvent, 2);
							if (probably) vEvent.AdjustPlaybackRate(2, true);
						}
						if (aConfig && probably) {
							RestoreAudioEventMethod();
							#if VER_GEQ_16
								aEvent.PitchLock = true;
							#endif
							aEvent.AdjustPlaybackRate(2, true);
						}
					} break;
					case YtpEffectType.EMPHASIZE_THRICE: {
						Repeat3State state = GetRandomRepeat3State(); // 去掉了 1.5 倍减速这种情况。
						bool isSpeedUp = state != Repeat3State.SPEED_DOWN_MULTIPLY, isMultiply = state != Repeat3State.SPEED_UP_1_5_TIMES;
						int count = isMultiply ? 3 : 6;
						var getRate = new Func<int, double>(index => {
							if (!(index >= 0 && (isMultiply && index < 3 || !isMultiply && index < 6))) throw new ArgumentOutOfRangeException();
							double[] rates2 = { 1, 2, 4 }, rates1_5 = { 1, 1.25, 1.5, 2, 3, 4 };
							return Math.Pow((isMultiply ? rates2 : rates1_5)[index], isSpeedUp ? 1 : -1);
						});
						AudioEvent[] repeatClips = null;
						if (aConfig) {
							repeatClips = new AudioEvent[count];
							RestoreAudioEventMethod();
							#if VER_GEQ_16
								aEvent.PitchLock = true;
							#endif
							for (int i = 1; i < count; i++) {
								AudioEvent clip = aEvent.Copy(aEvent.Track, aEvent.Track.Length) as AudioEvent;
								repeatClips[i] = clip;
								double rate = getRate(i);
								clip.AdjustPlaybackRate(rate, true);
								clip.Length = clip.Length.Multiply(1 / rate);
							}
						}
						if (vConfig) {
							if (blackAndWhite == null) WarningMissingPlugin(Lang.str.black_and_white);
							List<VideoEvent> videoEvents = new List<VideoEvent>();
							videoEvents.Add(vEvent);
							for (int i = 1; i < count; i++) {
								VideoEvent clip = vEvent.Copy(vEvent.Track, vEvent.Track.Length) as VideoEvent;
								if (repeatClips != null) p.GroupTrackEvents(clip, repeatClips[i]);
								if (probably && !isSpeedUp) ForVideoEvents.Grey(clip);
								double rate = getRate(i);
								clip.AdjustPlaybackRate(rate, true);
								clip.Length = clip.Length.Multiply(1 / rate);
								videoEvents.Add(clip);
							}
							ForVideoEvents.Focus(videoEvents);
						}
					} break;
					default:
						break;
				}
			}
			/// <summary>
			/// 在所有 YTP 效果的范围内随机选取一个效果。
			/// </summary>
			/// <returns>随机选择的效果</returns>
			/// <exception cref="null">如果 YTP 效果枚举中没有任何效果，将会返回 null。</exception>
			private static YtpEffectType? GetRandomYtpEffectType() {
				int count = Enum.GetNames(new YtpEffectType().GetType()).Length;
				if (count == 0) return null;
				int rand = random.Next(count);
				return (YtpEffectType)rand;
			}
			/// <summary>
			/// 在指定 YTP 效果数组的范围内随机选取一个效果。
			/// </summary>
			/// <param name="effects">指定的 YTP 效果数组</param>
			/// <returns>随机选择的效果</returns>
			/// <exception cref="null">如果指定的 YTP 效果数组中没有任何效果，将会返回 null。</exception>
			private static YtpEffectType? GetRandomYtpEffectType(YtpEffectType[] effects) {
				int count = effects.Length;
				if (count == 0) return null;
				int rand = random.Next(count);
				return effects[rand];
			}
			private const int HIGH_FREQ_REPEAT_FREQ = 50;
			private const int MAX_PITCH_CHANGE_LENGTH = 500;
			private const int MIN_HIGH_FREQ_REPEAT = 5;
			private const int MAX_HIGH_FREQ_REPEAT = 20;
			private const int MIN_TONE_PITCH_CHANGE = -9;
			private const int MAX_TONE_PITCH_CHANGE = 9;
			private static void WarningMissingPlugin(string pluginName) { // 遇到缺少插件时不报错，而是仅仅弹出一个警告，点击确定后可以跳过。
				MessageBox.Show(string.Format(Lang.str.warning_missing_plugin, pluginName), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			/// <summary>
			/// 重要的事情说三遍效果的状态枚举。
			/// </summary>
			public enum Repeat3State {
				SPEED_UP_MULTIPLY,
				SPEED_UP_1_5_TIMES,
				SPEED_DOWN_MULTIPLY,
			}
			/// <summary>
			/// 获取一个随机的重要的事情说三遍效果状态。
			/// </summary>
			/// <returns>随机选取的重说三状态</returns>
			private static Repeat3State GetRandomRepeat3State() {
				int count = Enum.GetNames(new Repeat3State().GetType()).Length;
				int rand = random.Next(count);
				return (Repeat3State)rand;
			}
			/// <summary>
			/// 指定插件效果应用随机预设。
			/// </summary>
			/// <param name="effect">插件效果</param>
			public static void GetRandomPreset(Effect effect) {
				EffectPresets presets = effect.Presets;
				int count = presets.Count;
				string randomPreset;
				if (count == 0) return;
				else if (count == 1) randomPreset = presets[0].Name;
				else {
					do {
						randomPreset = presets[random.Next(presets.Count)].Name;
					} while (randomPreset == DEFAULT_PARAMETER || randomPreset.StartsWith("("));
				}
				effect.Preset = randomPreset;
			}
			private const string DEFAULT_PARAMETER = "[Sys] Default all parameters";
			/// <summary>
			/// 针对 Vegas 15 及以下版本兼容而使用的传统移调插件调音方式。
			/// </summary>
			/// <param name="audioEvent">音频轨道事件。</param>
			/// <param name="pitchDelta">移调值。</param>
			public static void SetPitchSemis_ForVersionsBelow16(AudioEvent audioEvent, int pitchDelta) {
				if (pitchShift == null) { WarningMissingPlugin(Lang.str.no_plugin_pitch_shift_exception); return; }
				int pitchDeltaTimes = pitchDelta > 0 ? 12 : -12;
				const bool lockStretchPitch = false;
				while (pitchDeltaTimes * pitchDelta > 0) { // pitchDeltaTimes > 0 ? pitchDelta > 0 : pitchDelta < 0
					Effect effect = audioEvent.Effects.AddEffect(pitchShift);
					try {
						effect.Preset = (Math.Abs(pitchDelta) <= 12 ? pitchDelta : pitchDeltaTimes).ToString()
							+ (lockStretchPitch ? "~" : "");
					} catch (Exception) { WarningMissingPlugin(Lang.str.no_plugin_presets_exception); return; }
					pitchDelta -= pitchDeltaTimes;
				}
			}
		}
		private static readonly Random random = new Random();
		/// <summary>
		/// 在指定的最小值与最大值之间获取一个随机的双精度浮点值。
		/// </summary>
		/// <param name="min">最小值</param>
		/// <param name="max">最大值</param>
		/// <returns>随机的双精度浮点值。且一定不会恰好等于最大值。</returns>
		public static double RandomDoubleBetween(double min, double max) {
			return random.NextDouble() * (max - min) + min;
		}
		public static float RandomFloatBetween(float min, float max) {
			return (float)RandomDoubleBetween(min, max);
		}
		/// <summary>
		/// 一个数学函数。
		/// 旨在让（某个线性的随机）值更大概率接近 -1 和 1，而更小概率接近 0。给定的参数最好在 -1 ≤ x ≤ 1 的范围之内。
		/// 函数图象是一个正半轴为 y = √x 的奇函数。
		/// </summary>
		/// <param name="x">给定一个 -1 ≤ x ≤ 1 的值</param>
		/// <returns>函数所得值</returns>
		private static double ExtremeFunction(double x) {
			if (x >= 0) return Math.Sqrt(x);
			else return -Math.Sqrt(-x);
		}
		/// <summary>
		/// 获取一个随机的布尔值（真假的概率相等）。
		/// </summary>
		/// <returns>随机的布尔值</returns>
		public static bool RandomBool() {
			return random.Next(2) == 1;
		}
		/// <summary>
		/// 在指定初始值和最终值的范围内获得一个指定完成进度的值。
		/// 指定的进度的取值范围必须在 0 ~ 1 内。
		/// </summary>
		/// <param name="begin">初始值</param>
		/// <param name="final">最终值</param>
		/// <param name="progress">进度 ∈ [0 ~ 1]</param>
		/// <returns>指定范围内完成进度的值</returns>
		public static double GetPercentageInRange(double begin, double final, double progress) {
			if (progress < 0) progress = 0;
			if (progress > 1) progress = 1;
			return (final - begin) * progress + begin;
		}
		public static float GetPercentageInRange(float begin, float final, double progress) {
			return (float)GetPercentageInRange((double)begin, (double)final, progress);
		}
		/// <summary>
		/// 获得一个表示视频运动关键帧中使用的矩形，而不是不规则四边形。<br/>
		/// “bounds” 用复数形式表示“边界”含义，然后又用 “get a” 表示获得一个。真搞不懂英文怎么命名的。
		/// </summary>
		/// <param name="x">矩形左上角的点的横坐标</param>
		/// <param name="y">矩形左上角的点的纵坐标</param>
		/// <param name="width">矩形宽度</param>
		/// <param name="height">矩形高度</param>
		/// <returns>矩形</returns>
		public static VideoMotionBounds GetARectangleBounds(float x, float y, float width, float height) {
			return new VideoMotionBounds(
				x, y, x + width, y,
				x + width, y + height, x, y + height
			);
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
		public IList<MidiEvent> MsPerQuarterTrack;
		public string TimeSignature = "";
		public IList<MidiEvent> TimeSignatureTrack;
		public string Path;
		public class TrackInfo {
			public int Index;
			public string Name = "";
			public string Instrument = "";
			public int NotesCount = 0;
			public string BeginNote = "";
			public bool HasName { get { return !string.IsNullOrWhiteSpace(Name); } }
			public bool HasInstrument { get { return !string.IsNullOrWhiteSpace(Instrument); } }
			public override string ToString() {
				List<string> info = new List<string>();
				info.Add(Lang.str.midi_channel + ' ' + Index + (HasName ? Lang.str.colon + Name : ""));
				if (HasInstrument) info.Add(Lang.str.instrument + Lang.str.colon + Instrument);
				info.Add(Lang.str.midi_notes_count + Lang.str.colon + NotesCount);
				if (NotesCount != 0) info.Add(Lang.str.midi_begin_note + Lang.str.colon + BeginNote);
				return string.Join(Lang.str.semicolon, info);
			}
			public IList<MidiEvent> Events;
		}
		public MIDI(string path) : base(path) {
			Path = path;
			List<TrackInfo> trackInfos = new List<TrackInfo>();
			TicksPerQuarter = DeltaTicksPerQuarterNote;
			MsPerQuarter = 0; // 毫秒每拍
			for (int i = 0; i < Events.Tracks; i++) {
				TrackInfo info = new TrackInfo { Index = i, Events = Events[i] };
				foreach (MidiEvent midiEvent in info.Events) {
					if (midiEvent is NoteEvent && !(midiEvent is NoteOnEvent)) {
						NoteEvent noteEvent = midiEvent as NoteEvent;
						if (info.NotesCount++ == 0) info.BeginNote = noteEvent.NoteName; // 起音判断
					}
					if (midiEvent is PatchChangeEvent && !info.HasInstrument) {
						PatchChangeEvent patchEvent = midiEvent as PatchChangeEvent;
						info.Instrument = string.Join(" ", patchEvent.ToString().Split(' ').Slice(4)); // 乐器名称
						info.Index = patchEvent.Channel;
					}
					if (midiEvent is TempoEvent && MsPerQuarter == 0) {
						TempoEvent tempoEvent = midiEvent as TempoEvent;
						MsPerQuarter = Convert.ToDouble(tempoEvent.MicrosecondsPerQuarterNote) / 1000; // 每四分音符多少毫秒
						// MessageBox.Show(tempoEvent.Tempo.ToString()); // 用 Tempo 表示 BPM
						MsPerQuarterTrack = info.Events;
					}
					if (midiEvent is TextEvent && !info.HasName) {
						TextEvent textEvent = midiEvent as TextEvent;
						info.Name = textEvent.Text; // 乐轨名称
					}
					if (midiEvent is TimeSignatureEvent && TimeSignature.Length == 0) {
						TimeSignatureEvent timeSignatureEvent = midiEvent as TimeSignatureEvent;
						TimeSignature = timeSignatureEvent.TimeSignature; // 初始节拍
						TimeSignatureTrack = info.Events;
					}
				}
				if (info.NotesCount != 0) trackInfos.Add(info);
			}
			TrackInfos = trackInfos.ToArray();
		}
		public static int CountIf<MidiEventType>(IList<MidiEvent> track) where MidiEventType : MidiEvent {
			int num = 0;
			if (track != null)
				foreach (MidiEvent midiEvent in track)
					if (midiEvent is MidiEventType)
						num++;
			return num;
		}
		public bool IsDynamicBeat { get { return CountIf<TimeSignatureEvent>(TimeSignatureTrack) > 1; } }
		public bool IsDynamicBpm {
			get {
				double tempTempo = -1;
				if (MsPerQuarterTrack != null)
					foreach (MidiEvent midiEvent in MsPerQuarterTrack)
						if (midiEvent is TempoEvent) {
							TempoEvent tempo = midiEvent as TempoEvent;
							if (tempTempo < 0) tempTempo = tempo.Tempo;
							else if (tempTempo != tempo.Tempo) return true;
						}
				return false;
			}
		}
	}

	/// <summary>
	/// 动态 BPM 积分器。
	/// </summary>
	public class DynamicBpmIntegrator {
		private readonly MIDI midi;
		private readonly TempoEvent[] msPerQuarterTrack;
		private readonly BpmKeysData[] bpmKeysDatas;
		private readonly bool useLinearKeyframes = false;

		/// <summary>
		/// 动态 BPM 积分器。
		/// </summary>
		/// <param name="midi">MIDI 对象。</param>
		/// <param name="useLinearKeyframes">如果为 false，则关键帧间插值使用定格（矩形）；如果为 true，则关键帧间插值使用线性（梯形）。<br />
		/// 一般来说 MIDI 的动态 BPM 属性的关键帧插值都是定格的。</param>
		public DynamicBpmIntegrator(MIDI midi, bool useLinearKeyframes = false) {
			this.midi = midi;
			this.useLinearKeyframes = useLinearKeyframes;
			double totalMs = 0;
			BpmKeysData previousData = null;
			IList<MidiEvent> _msPerQuarterTrack_IList = midi.MsPerQuarterTrack;
			List<TempoEvent> _msPerQuarterTrack_List = new List<TempoEvent>();
			List<BpmKeysData> _bpmKeysDatas_List = new List<BpmKeysData>();
			foreach (MidiEvent midiEvent in _msPerQuarterTrack_IList)
				if (midiEvent is TempoEvent)
					_msPerQuarterTrack_List.Add(midiEvent as TempoEvent);
			msPerQuarterTrack = _msPerQuarterTrack_List.ToArray();
			for (int i = 0; i < msPerQuarterTrack.Length; i++) {
				TempoEvent tempoEvent = msPerQuarterTrack[i];
				double msPerQuarter = (double)tempoEvent.MicrosecondsPerQuarterNote / 1000;
				double startTicks = tempoEvent.AbsoluteTime;
				if (i + 1 < msPerQuarterTrack.Length && msPerQuarterTrack[i + 1].AbsoluteTime == startTicks) continue;
				// 经改正，MIDI 的动态 BPM 应该呈矩形而不是梯形。
				double previousStartTicks = previousData == null ? 0 : previousData.startTicks;
				double previousMsPerQuarter = previousData == null ? 0 : previousData.msPerQuarter;
				if (!useLinearKeyframes)
					totalMs += (startTicks - previousStartTicks) / midi.TicksPerQuarter * previousMsPerQuarter;
				else
					totalMs += (startTicks - previousStartTicks) / midi.TicksPerQuarter * (previousMsPerQuarter + msPerQuarter) / 2.0;
				_bpmKeysDatas_List.Add(previousData = new BpmKeysData(msPerQuarter, startTicks, totalMs));
			}
			bpmKeysDatas = _bpmKeysDatas_List.ToArray();
		}
		/// <summary>
		/// 存储 BPM 关键帧数据的类。<br />
		/// <small>原计划采用结构体而不是类来存储，但是那样使用起来不是很方便。</small>
		/// </summary>
		private class BpmKeysData {
			public readonly double msPerQuarter;
			public readonly double startTicks;
			public readonly double previousMs;
			/// <summary>
			/// 存储 BPM 关键帧数据的类。
			/// </summary>
			/// <param name="msPerQuarter">此刻的毫秒每四分音符的值（即当前速度）。</param>
			/// <param name="startTicks">相对开始位置。</param>
			/// <param name="previousMs">之前所有数据实际毫秒值的总和。</param>
			public BpmKeysData(double msPerQuarter, double startTicks, double previousMs) {
				this.msPerQuarter = msPerQuarter;
				this.startTicks = startTicks;
				this.previousMs = previousMs;
			}
		}
		/// <summary>
		/// 根据 MIDI 音符的相对时刻获取音频播放的实际时刻（毫秒）。
		/// </summary>
		/// <param name="absoluteTime">音符相对时刻。</param>
		/// <returns>音频播放的实际时刻。</returns>
		public double GetActualTime(double absoluteTime) {
			for (int i = 0; i < bpmKeysDatas.Length; i++) {
				BpmKeysData curData = bpmKeysDatas[i], nextData = null;
				if (i + 1 < bpmKeysDatas.Length) {
					nextData = bpmKeysDatas[i + 1];
					if (absoluteTime > nextData.startTicks) continue;
				}
				double curMs;
				if (nextData == null || !useLinearKeyframes)
					curMs = (absoluteTime - curData.startTicks) * curData.msPerQuarter / midi.TicksPerQuarter;
				else {
					double curPosition = absoluteTime - curData.startTicks;
					double curProportion = curPosition / (nextData.startTicks - curData.startTicks);
					double curMsPerQuarter = curData.msPerQuarter * (1 - curProportion) + nextData.msPerQuarter * curProportion;
					curMs = (absoluteTime - curData.startTicks) * (curData.msPerQuarter + curMsPerQuarter) / 2.0 / midi.TicksPerQuarter;
				}
				return curMs + curData.previousMs;
			}
			// 针对没有任何 BPM 关键帧却误打误撞进入这个函数环节的。
			MessageBox.Show("No Bpm Keys!");
			return absoluteTime;
		}
		/// <summary>
		/// 根据 MIDI 音符的相对时刻与时长获取音频播放的实际时刻与时长（毫秒）。
		/// </summary>
		/// <param name="absoluteStart">音符相对时刻。</param>
		/// <param name="absoluteDuration">音符相对时长。</param>
		/// <returns>一个元组，其<c>Item1</c>和<c>Item2</c>值分别为音频播放的实际时刻与时长。</returns>
		public Tuple<double, double> GetActualTime(double absoluteStart, double absoluteDuration) {
			double start = GetActualTime(absoluteStart);
			double duration = GetActualTime(absoluteStart + absoluteDuration) - start;
			return new Tuple<double, double>(start, duration);
		}
	}

	/// <summary>
	/// 自定义错误信息。
	/// </summary>
	namespace Exceptions {
		public class NoMidiException : Exception {
			/// <summary>
			/// 未选择 MIDI 文件报错。
			/// </summary>
			public NoMidiException() : base(Lang.str.no_midi_exception) { }
		}

		public class NoMediaException : Exception {
			/// <summary>
			/// 未选择媒体文件报错。
			/// </summary>
			public NoMediaException() : base(Lang.str.no_media_exception) { }
		}

		public class NoTrackInfoException : Exception {
			/// <summary>
			/// 无 MIDI 音轨信息报错。
			/// </summary>
			public NoTrackInfoException() : base(Lang.str.no_track_info_exception) { }
		}

		public class NoPluginPitchShiftException : Exception {
			/// <summary>
			/// 无法调用移调插件报错。
			/// </summary>
			public NoPluginPitchShiftException() : base(string.Format(Lang.str.no_plugin_pitch_shift_exception, ConfigForm.Links.DOCUMENTATION_V0_1)) { }
		}

		public class NoPluginPresetsException : Exception {
			/// <summary>
			/// 无法调用移调插件的预设效果报错。
			/// </summary>
			public NoPluginPresetsException() : base(string.Format(Lang.str.no_plugin_presets_exception, ConfigForm.Links.DOCUMENTATION_V0_1)) { }
		}

		public class NoPluginNameException : Exception {
			/// <summary>
			/// 无法调用某个插件报错。
			/// </summary>
			public NoPluginNameException(string pluginName) : base(string.Format(Lang.str.no_plugin_name_exception, pluginName)) { }
		}

		public class NoAudioTakeException : Exception {
			/// <summary>
			/// 无法读取音频媒体流报错。
			/// </summary>
			public NoAudioTakeException() : base(Lang.str.no_audio_take_exception +
				Lang.str.no_take_exception_ps) { }
		}
		public class NoVideoTakeException : Exception {
			/// <summary>
			/// 无法读取视频媒体流报错。
			/// </summary>
			public NoVideoTakeException() : base(Lang.str.no_video_take_exception +
				Lang.str.no_take_exception_ps) { }
		}
		public class NoMediaTakeException : Exception {
			/// <summary>
			/// 无法读取媒体报错。
			/// </summary>
			public NoMediaTakeException() : base(Lang.str.no_media_take_exception +
				Lang.str.no_take_exception_ps) { }
		}

		public class NotAMidiFileException : Exception {
			/// <summary>
			/// 无法读取 MIDI 文件报错。
			/// </summary>
			public NotAMidiFileException() : base(Lang.str.not_a_midi_file_exception) { }
		}

		public class NoSelectedMediaException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedMediaException() : base(Lang.str.no_selected_media_exception + Lang.str.no_selected_exception_ps) { }
		}
		public class NoSelectedClipException : Exception {
			/// <summary>
			/// 没有在项目媒体中选择任何媒体报错。
			/// </summary>
			public NoSelectedClipException() : base(Lang.str.no_selected_clip_exception + Lang.str.no_selected_exception_ps) { }
		}
		public class NoTimeStretchPitchShiftException : Exception {
			/// <summary>
			/// 音调转换方法设置为不调音的报错。
			/// </summary>
			public NoTimeStretchPitchShiftException() : base(Lang.str.no_time_stretch_pitch_shift_exception) { }
		}
		public class ReadConfigFailException : Exception {
			/// <summary>
			/// 读取参数配置文件失败报错。
			/// </summary>
			public ReadConfigFailException() : base(Lang.str.read_config_fail_exception) { }
		}

		public class FailToSelectClipsException : Exception {
			/// <summary>
			/// 选取轨道剪辑出错。
			/// </summary>
			public FailToSelectClipsException() : base(Lang.str.fail_to_select_clips_exception) { }
		}

		public class FailToSelectTracksException : Exception {
			/// <summary>
			/// 选取轨道出错。
			/// </summary>
			public FailToSelectTracksException() : base(Lang.str.fail_to_select_tracks_exception) { }
		}

		public class YtpOverLengthException : Exception {
			/// <summary>
			/// 指定的 YTP 最小长度超过了媒体长度报错。
			/// </summary>
			public YtpOverLengthException() : base(Lang.str.ytp_over_length_exception) { }
		}

		public class YtpInMediaGeneratorException : Exception {
			/// <summary>
			/// 对媒体生成器产生的媒体应用 YTP 报错。
			/// </summary>
			public YtpInMediaGeneratorException() : base(Lang.str.ytp_in_media_generator_exception) { }
		}

		public class YtpEliminateDuplicatesFinallyNullException : NullReferenceException {
			/// <summary>
			/// 对 YTP 素材列表去重最后变为空的技术异常。
			/// </summary>
			public YtpEliminateDuplicatesFinallyNullException() : base(Lang.str.ytp_eliminate_duplicates_finally_null_exception) { }
		}

		public class UnknownException : Exception {
			/// <summary>
			/// 未知异常。
			/// </summary>
			public UnknownException() : base(Lang.str.unknown_exception) { }
		}

		public class UsePicInPicOnUnsupportedVegasException : Exception {
			/// <summary>
			/// 在低版本 Vegas 的画中画插件中使用不支持的操作报错。
			/// </summary>
			public UsePicInPicOnUnsupportedVegasException() : base(Lang.str.use_pic_in_pic_on_unsupported_vegas_exception) { }
		}
	}

	/// <summary>
	/// 路径类，用于处理路径。<br />
	/// 虽然系统自带有 <c>System.IO.Path</c> 类，但那是一个静态类，不怎么面向对象。
	/// </summary>
	public class Path : List<string> {
		/// <summary>
		/// 通过一个字符串构造一个路径类。
		/// </summary>
		/// <param name="path">路径字符串</param>
		public Path(string path) : base(path.Replace("\\", "/").TrimEnd('/').Split('/')) { }
		/// <summary>
		/// 通过一段目录数组构造一个路径类。
		/// </summary>
		/// <param name="arr">目录数组</param>
		public Path(string[] arr) : base(arr) { }
		/// <summary>
		/// 通过输入的内容逐个拼接成一个新的路径类。
		/// </summary>
		/// <param name="arr">内容</param>
		/// <returns>新的路径类</returns>
		public static Path r(params string[] arr) {
			Path path = new Path(arr[0]);
			for (int i = 1; i < arr.Length; i++)
				path += new Path(arr[i]);
			return path;
		}
		private char sep = System.IO.Path.DirectorySeparatorChar;
		private bool isWindows = true;
		/// <summary>
		/// 是否输入为 Windows 格式路径？
		/// 这将决定最终生成的路径的分隔符形式。
		/// 默认为 <c>true</c>。
		/// </summary>
		public bool IsWindows {
			get { return isWindows; }
			set {
				isWindows = value;
				sep = value ? System.IO.Path.DirectorySeparatorChar : System.IO.Path.AltDirectorySeparatorChar;
			}
		}
		/// <summary>
		/// 向上一级。
		/// </summary>
		public void UpOneLevel() {
			RemoveAt(Count - 1);
		}
		public override string ToString() {
			return string.Join(sep.ToString(), this);
		}
		/// <summary>
		/// 拷贝一份当前实例的副本。
		/// </summary>
		/// <returns>副本</returns>
		public Path Copy() {
			return new Path(ToString()) {
				IsWindows = this.IsWindows
			};
		}
		private string GetLastItem() {
			return this[Count - 1];
		}
		/// <summary>
		/// 获取完整路径文本。
		/// </summary>
		public string FullPath {
			get { return ToString(); }
		}
		/// <summary>
		/// 读取或更改路径最终指向文件的文件名 + 扩展名。
		/// </summary>
		public string FullFileName {
			get { return GetLastItem(); }
			set { this[Count - 1] = value; }
		}
		private static readonly Regex extReg = new Regex(@"(?<=\.)[^\.\\/:\*\?""<>\|]*$");
		/// <summary>
		/// 读取或更改路径最终指向文件的扩展名。
		/// </summary>
		public string Extension {
			get {
				//MatchCollection ext = extReg.Matches(FullFileName);
				//return ext.Count != 0 ? ext[0].ToString() : "";
				return System.IO.Path.GetExtension(FullFileName);
			}
			set {
				value = value.Trim().TrimStart('.');
				//if (extReg.IsMatch(FullFileName)) {extReg.Replace(FullFileName, value); S.s = FullFileName; }
				//else FullFileName += '.' + value;
				FullFileName = System.IO.Path.ChangeExtension(FullFileName, value);
			}
		}
		/// <summary>
		/// 读取或更改路径最终指向文件的文件名。
		/// </summary>
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
		/// <summary>
		/// 获取路径最终指向文件所在的目录。
		/// </summary>
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
	public class ConfigIni {
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
		[DllImport("kernel32.dll")]
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
		public ConfigIni(string filePath, ConfigForm configForm) {
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
				form.parent.ShowError(Lang.str.error_code + errCode + "\n\n" + GetSysErrMsg(errCode));
				return def;
			} else return sb.ToString(); */
			GetPrivateProfileString(section, key, def, sb, MAX_VALUE_LENGTH, filePath);
			return sb.ToString();
		}

		/// <summary>
		/// 写入 INI 文件。
		/// </summary>
		/// <param name="key">键名</param>
		/// <param name="value">写入的值</param>
		/// <param name="section">节点名</param>
		/// <returns>是否成功写入</returns>
		public bool Write(string key, object value, string section = null) {
			section = section ?? currentSection;
			if (section == null || filePath == null) return false;
			if (value is bool) value = (bool)value ? 1 : 0;
			if (value is string && (string)value == "") value = null;
			if (WritePrivateProfileString(section, key, value == null ? null : value.ToString(), filePath) == 0) {
				int errCode = ErrCode;
				EntryPoint.ShowError(Lang.str.error_code + errCode + "\n\n" + GetSysErrMsg(errCode));
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

		/// <summary>
		/// 读取 INI 文件。
		/// </summary>
		/// <typeparam name="T">读取数据的类型</typeparam>
		/// <param name="key">键名</param>
		/// <param name="def">没有查到的话返回的默认值</param>
		/// <param name="section">节点名</param>
		/// <returns>指定类型的参数值</returns>
		public T Read<T>(string key, T def, string section = null) where T : IConvertible {
			string def_str = def.ToString();
			if ((def is bool || def is bool?) && def != null) def_str = (bool)(def as bool?) ? "1" : "0";
			string result = Read(key, def_str, section);
			if (typeof(T) == typeof(string)) return (T)(object)result;
			else if (typeof(T) == typeof(bool)) { string r = result.Trim().ToLower(); return (T)(object)(r != "0" && r != "false"); }
			else if (typeof(T) == typeof(int)) { int value; return int.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(double)) { double value; return double.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(byte)) { byte value; return byte.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(char)) { char value; return char.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(decimal)) { decimal value; return decimal.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(float)) { float value; return float.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(long)) { long value; return long.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(sbyte)) { sbyte value; return sbyte.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(short)) { short value; return short.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(uint)) { uint value; return uint.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(ulong)) { ulong value; return ulong.TryParse(result, out value) ? (T)(object)value : def; }
			else if (typeof(T) == typeof(ushort)) { ushort value; return ushort.TryParse(result, out value) ? (T)(object)value : def; }
			else throw new TypeLoadException("Unsupported Type!");
		}

		/// <summary>
		/// 读取 INI 文件。
		/// </summary>
		/// <typeparam name="T">读取数据的类型</typeparam>
		/// <param name="key">键名</param>
		/// <param name="result">返回并输出的指定类型的参数值</param>
		/// <param name="def">没有查到的话返回的默认值</param>
		/// <param name="section">节点名</param>
		/// <returns>指定类型的参数值</returns>
		public T Read<T>(string key, out T result, T def, string section = null) where T : IConvertible {
			return result = Read(key, def, section);
		}

		/// <summary>
		/// 检查是否有给定的键名。<br />
		/// <strong>注意：即便返回值为空、空串或空白字符组成也算不包含该键。</strong>
		/// </summary>
		/// <param name="key">键名。</param>
		/// <param name="section">节点名。</param>
		/// <returns>是否有给定的键名。</returns>
		public bool HasKey(string key, string section = null) {
			return !string.IsNullOrWhiteSpace(Read(key, "", section));
		}
	}

	/// <summary>
	/// 需要使用（包括读取和修改）INI 配置设置文件的接口。
	/// </summary>
	public interface IConfigIniUser {
		/// <summary>
		/// 保存用户配置设置文件。
		/// </summary>
		void SaveIni();
		/// <summary>
		/// 读取用户配置设置文件。
		/// </summary>
		void ReadIni();
	}
	#endregion

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
			this.tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
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
			this.Track.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
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
			this.Numeric.Size = new System.Drawing.Size(75, 27);
			this.Numeric.TabIndex = 1;
			this.Numeric.ValueChanged += new System.EventHandler(this.Numeric_ValueChanged);
			// 
			// IntegerTrackWithBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.Transparent;
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
			Track.MouseClick += new MouseEventHandler(Track_MouseClick);
			Track.MouseDown += new MouseEventHandler(Track_MouseDown);
			Numeric.MouseWheel += AutoLayoutTracksGridForm.NumericUpDown_MouseWheel;
			doubleClickTimer.Tick += new EventHandler((sender, e) => {
				isWaitingDoubleClick = false;
				doubleClickTimer.Stop();
			});
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
		private void Track_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Right && e.Clicks == 1)
				ResetDefaultValue();
		}

		/// <summary>
		/// 双击滑动条，也可以重置其值。
		/// </summary>
		private void Track_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && e.Clicks == 1) {
				if (!isWaitingDoubleClick) {
					isWaitingDoubleClick = true;
					doubleClickTimer.Start();
				} else {
					doubleClickTimer.Stop();
					isWaitingDoubleClick = false;
					ResetDefaultValue();
				}
			}
		}

		private const int DOUBLE_CLICK_DURATION = 500;
		private readonly System.Windows.Forms.Timer doubleClickTimer = new System.Windows.Forms.Timer { Interval = DOUBLE_CLICK_DURATION };
		private bool isWaitingDoubleClick = false;

		private void ResetDefaultValue() {
			Value = DefaultValue;
		}

		/// <summary>
		/// 设定数值控件的当前值，并确保不受最大值或最小值限制的干扰。
		/// </summary>
		/// <param name="value">设定值</param>
		/// <param name="def">如果设定失败后的默认值，如果为 null 表示不改变。</param>
		public void SetValue(int value, int? def = null) {
			if (value < Minimum || value > Maximum) {
				if (def == null || def < Minimum || def > Maximum)
					return;
				Value = (int)def;
			} else
				Value = value;
		}
	}

	[ToolboxBitmap(typeof(TextBox))]
	public class TimecodeBox : TextBox {
		public TimecodeBox() : base() {
			Leave += (sender, e) => {
				Text = base.Text;
			};
		}

		private bool useTimecodeDeal = false;
		private const string DEFAULT_TIME = "0:00.000";
		private const RulerFormat format = RulerFormat.Time;
		//private Timecode timecode = Timecode.FromMilliseconds(0);
		//private double value = 0;

		[Description("与控件关联的文本。"), Category("Appearance"), DefaultValue(DEFAULT_TIME)]
		public override string Text {
			get { return base.Text; }
			set {
				if (!useTimecodeDeal) base.Text = DealLegal(value);
				else Timecode = Timecode.FromPositionString(value, format);
			}
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Value {
			get { return ClipTrimTime2Ms(Text); }
			set {
				if (!useTimecodeDeal) base.Text = FormatClipTrimTime(value);
				else DoubleValue = value;
			}
		}

		[Description("与控件关联的文本对应的毫秒双精度浮点值。"), Category("Behavior"), DefaultValue(0)]
		public double DoubleValue {
			get { return Value; }
			set {
				if (!useTimecodeDeal) Value = (int)value;
				else Timecode = Timecode.FromMilliseconds(value);
			}
		}

		[Description("与控件关联的文本对应的毫秒整型值。"), Category("Behavior"), DefaultValue(0)]
		public int Millisecond {
			get { return Value; }
			set { Value = value; }
		}

		[Description("与控件关联的文本对应的时间码值。"), Category("Behavior")]
		public Timecode Timecode {
			get { return Timecode.FromMilliseconds(DoubleValue); }
			set {
				base.Text = value.ToPositionString();
			}
		}

		[Description("是否使用 Vegas 时间码处理。"), Category("Behavior"), DefaultValue(false)]
		public bool UseTimecodeDeal {
			get { return useTimecodeDeal; }
			set { useTimecodeDeal = value; }
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
			this.RealTimeUpdateCheck = new System.Windows.Forms.CheckBox();
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 174);
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
			this.tableLayoutPanel2.Controls.Add(this.RealTimeUpdateCheck, 0, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(6);
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(471, 174);
			this.tableLayoutPanel2.TabIndex = 2;
			// 
			// InfoLabel
			// 
			this.InfoLabel.AutoSize = true;
			this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLabel.Location = new System.Drawing.Point(11, 67);
			this.InfoLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.InfoLabel.Name = "InfoLabel";
			this.InfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.InfoLabel.Size = new System.Drawing.Size(449, 34);
			this.InfoLabel.TabIndex = 0;
			this.InfoLabel.Text = "正在生成音 MAD / YTPMV⋯⋯";
			this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PercentLabel
			// 
			this.PercentLabel.AutoSize = true;
			this.PercentLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PercentLabel.Font = new System.Drawing.Font("Segoe UI", 24F);
			this.PercentLabel.Location = new System.Drawing.Point(6, 6);
			this.PercentLabel.Margin = new System.Windows.Forms.Padding(0);
			this.PercentLabel.Name = "PercentLabel";
			this.PercentLabel.Size = new System.Drawing.Size(459, 61);
			this.PercentLabel.TabIndex = 1;
			this.PercentLabel.Text = "0%";
			this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.ProgressBar.Location = new System.Drawing.Point(14, 104);
			this.ProgressBar.Margin = new System.Windows.Forms.Padding(8, 3, 8, 3);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(443, 23);
			this.ProgressBar.Step = 1;
			this.ProgressBar.TabIndex = 2;
			this.ProgressBar.UseWaitCursor = true;
			// 
			// RealTimeUpdateCheck
			// 
			this.RealTimeUpdateCheck.AutoSize = true;
			this.RealTimeUpdateCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RealTimeUpdateCheck.Location = new System.Drawing.Point(9, 145);
			this.RealTimeUpdateCheck.Name = "RealTimeUpdateCheck";
			this.RealTimeUpdateCheck.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
			this.RealTimeUpdateCheck.Size = new System.Drawing.Size(453, 20);
			this.RealTimeUpdateCheck.TabIndex = 3;
			this.RealTimeUpdateCheck.Text = "实时更新轨道 UI（会减慢生成速度）";
			this.RealTimeUpdateCheck.UseVisualStyleBackColor = true;
			// 
			// ProgressForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(471, 216);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
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
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		public System.Windows.Forms.Label InfoLabel;
		public System.Windows.Forms.Label PercentLabel;
		public System.Windows.Forms.ProgressBar ProgressBar;
		public System.Windows.Forms.CheckBox RealTimeUpdateCheck;
	}

	public partial class ProgressForm : Form, IInterpret {
		public ProgressForm() {
			InitializeComponent();
			Icon = ConfigForm.icon;
			ProgressBar.Style = ProgressBarStyle.Marquee;
			Translate();
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
			if (value == 0) ProgressBar.Style = ProgressBarStyle.Marquee;
			else ProgressBar.Style = ProgressBarStyle.Blocks;
			ProgressBar.Value = value;
			PercentLabel.Text = value + "%";
			_Update();
		}

		public void ReportProgress(double value) {
			ReportProgress((int)value);
		}

		public void ReportProgress(int current, int length) {
			int value = (int)Math.Round(100.0 * current / length);
			if (value < Minimum) value = Minimum;
			if (value > Maximum) value = Maximum;
			if (value == 0) ProgressBar.Style = ProgressBarStyle.Marquee;
			else ProgressBar.Style = ProgressBarStyle.Blocks;
			ProgressBar.Value = value;
			PercentLabel.Text = current + " / " + length;
			_Update();
		}

		private void _Update() {
			Application.DoEvents();
			ProgressBar.Update();
			ProgressBar.Refresh();
			PercentLabel.Update();
			PercentLabel.Refresh();
		}

		public int Progress {
			get { return ProgressBar.Value; }
			set { ReportProgress(value); }
		}

		public string Info {
			get { return InfoLabel.Text; }
			set {
				InfoLabel.Text = value != "" ? value : Lang.str.processing_otomad;
				InfoLabel.Update();
				InfoLabel.Refresh();
			}
		}

		public void Translate() {
			Lang str = Lang.str;
			CancelBtn.Text = str.cancel;
			Text = str.processing_it;
			InfoLabel.Text = Lang.str.processing_otomad;
			RealTimeUpdateCheck.Text = Lang.str.real_time_update;
		}
	}

	public class ToolStripRadioButtonMenuItem : ToolStripMenuItem {
		public ToolStripRadioButtonMenuItem()
			: base() {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text)
			: base(text, null, (EventHandler)null) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(Image image)
			: base(null, image, (EventHandler)null) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text, Image image)
			: base(text, image, (EventHandler)null) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text, Image image,
			EventHandler onClick)
			: base(text, image, onClick) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text, Image image,
			EventHandler onClick, string name)
			: base(text, image, onClick, name) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text, Image image,
			params ToolStripItem[] dropDownItems)
			: base(text, image, dropDownItems) {
			Initialize();
		}

		public ToolStripRadioButtonMenuItem(string text, Image image,
			EventHandler onClick, Keys shortcutKeys)
			: base(text, image, onClick) {
			Initialize();
			this.ShortcutKeys = shortcutKeys;
		}

		// 由所有构造函数调用以初始化“选中时单击”。
		private void Initialize() {
			CheckOnClick = true;
		}

		protected override void OnCheckedChanged(EventArgs e) {
			base.OnCheckedChanged(e);

			// 如果此项不再处于选中状态或其父项尚未初始化，则不执行任何操作。
			if (!Checked || Parent == null) return;

			// 清除所有同级的选中状态。
			foreach (ToolStripItem item in Parent.Items) {
				ToolStripRadioButtonMenuItem radioItem =
					item as ToolStripRadioButtonMenuItem;
				if (radioItem != null && radioItem != this && radioItem.Checked) {
					radioItem.Checked = false;

					// 一次只能选择一个项目，因此无需继续。
					return;
				}
			}
		}

		protected override void OnClick(EventArgs e) {
			// 如果该项已处于选中状态，则不要调用基方法，这样会切换该值。
			if (Checked) return;

			base.OnClick(e);
		}

		// 让项目自行绘制，然后绘制通常显示为复选标记的单选按钮。
		protected override void OnPaint(PaintEventArgs e) {
			if (Image != null) {
				// 如果客户端设置“图像”属性，则选择行为保持不变，但不显示单选按钮，并且选择仅由选择矩形指示。
				base.OnPaint(e);
				return;
			} else {
				// 如果未设置“图像”属性，请在暂时清除“选中状态”属性的情况下调用基类 OnPaint
				// 方法，以防止绘制复选标记。
				CheckState currentState = CheckState;
				CheckState = CheckState.Unchecked;
				base.OnPaint(e);
				CheckState = currentState;
			}

			// 确定单选按钮的正确状态。
			RadioButtonState buttonState = RadioButtonState.UncheckedNormal;
			if (Enabled) {
				if (mouseDownState)
					buttonState = Checked ? RadioButtonState.CheckedPressed : RadioButtonState.UncheckedPressed;
				else if (mouseHoverState)
					buttonState = Checked ? RadioButtonState.CheckedHot : RadioButtonState.UncheckedHot;
				else if (Checked) buttonState = RadioButtonState.CheckedNormal;
			} else buttonState = Checked ? RadioButtonState.CheckedDisabled : RadioButtonState.UncheckedDisabled;

			// 计算显示单选按钮的位置。
			int offset = (ContentRectangle.Height -
				RadioButtonRenderer.GetGlyphSize(
				e.Graphics, buttonState).Height) / 2;
			Point imageLocation = new Point(
				ContentRectangle.Location.X + 4,
				ContentRectangle.Location.Y + offset);

			// 绘制单选按钮。
			//RadioButtonRenderer.DrawRadioButton(e.Graphics, imageLocation, buttonState);

			// 重新画一下。
			const int CHECK_DOT_DIAMETER = 5;
			int checkAreaLength = ContentRectangle.Height;
			int checkTopLeft = (checkAreaLength - CHECK_DOT_DIAMETER) / 2;
			Rectangle checkRect = new Rectangle(checkTopLeft + 4, checkTopLeft, CHECK_DOT_DIAMETER, CHECK_DOT_DIAMETER);
			SolidBrush brush = new SolidBrush(Enabled ? Color.Black : Color.DarkGray);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			if (Checked) e.Graphics.FillEllipse(brush, checkRect);
		}

		private bool mouseHoverState = false;

		protected override void OnMouseEnter(EventArgs e) {
			mouseHoverState = true;

			// 强制项目使用新的单选按钮状态重新绘制。
			Invalidate();

			base.OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e) {
			mouseHoverState = false;
			base.OnMouseLeave(e);
		}

		private bool mouseDownState = false;

		protected override void OnMouseDown(MouseEventArgs e) {
			mouseDownState = true;

			// 强制项目使用新的单选按钮状态重新绘制。
			Invalidate();

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			mouseDownState = false;
			base.OnMouseUp(e);
		}

		// 仅当其父项处于选中状态且其“启用”属性未显式设置为 false 时，才启用该项。
		public override bool Enabled {
			get {
				ToolStripMenuItem ownerMenuItem =
					OwnerItem as ToolStripMenuItem;

				// 在设计模式中使用基准值可防止设计者将基准值设置为计算值。
				if (!DesignMode &&
					ownerMenuItem != null && ownerMenuItem.CheckOnClick) {
					return base.Enabled && ownerMenuItem.Checked;
				} else {
					return base.Enabled;
				}
			}
			set {
				base.Enabled = value;
			}
		}

		// 当“父项”可用时，如果它是一个工具条菜单项，且“选中时单击”属性值为 true，
		// 则订阅其“选中状态更改”事件。
		protected override void OnOwnerChanged(EventArgs e) {
			ToolStripMenuItem ownerMenuItem =
				OwnerItem as ToolStripMenuItem;
			if (ownerMenuItem != null && ownerMenuItem.CheckOnClick) {
				ownerMenuItem.CheckedChanged +=
					new EventHandler(OwnerMenuItem_CheckedChanged);
			}
			base.OnOwnerChanged(e);
		}

		// 当父项的选中状态更改时，请重新绘制该项，以便显示新的启用状态。
		private void OwnerMenuItem_CheckedChanged(
			object sender, EventArgs e) {
			Invalidate();
		}
	}

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
			this.dock.Location = new System.Drawing.Point(0, 406);
			this.dock.Margin = new System.Windows.Forms.Padding(4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(696, 42);
			this.dock.TabIndex = 3;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(531, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "替换(&R)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(612, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
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
			this.UseTrackEventGroupCheck.Location = new System.Drawing.Point(9, 8);
			this.UseTrackEventGroupCheck.Name = "UseTrackEventGroupCheck";
			this.UseTrackEventGroupCheck.Size = new System.Drawing.Size(158, 26);
			this.UseTrackEventGroupCheck.TabIndex = 0;
			this.UseTrackEventGroupCheck.Text = "同时替换分组内其它剪辑";
			this.UseTrackEventGroupCheck.UseVisualStyleBackColor = true;
			// 
			// ReserveOriginalNameCheck
			// 
			this.ReserveOriginalNameCheck.AutoSize = true;
			this.ReserveOriginalNameCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReserveOriginalNameCheck.Location = new System.Drawing.Point(138, 6);
			this.ReserveOriginalNameCheck.Name = "ReserveOriginalNameCheck";
			this.ReserveOriginalNameCheck.Size = new System.Drawing.Size(88, 21);
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
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(8, 8, 8, 0);
			this.panel1.Size = new System.Drawing.Size(696, 406);
			this.panel1.TabIndex = 4;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.ClassicTab);
			this.tabs.Controls.Add(this.SeparationTab);
			this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabs.Location = new System.Drawing.Point(8, 8);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(680, 398);
			this.tabs.TabIndex = 0;
			this.tabs.SelectedIndexChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ClassicTab
			// 
			this.ClassicTab.Controls.Add(this.table);
			this.ClassicTab.Location = new System.Drawing.Point(4, 24);
			this.ClassicTab.Name = "ClassicTab";
			this.ClassicTab.Padding = new System.Windows.Forms.Padding(3);
			this.ClassicTab.Size = new System.Drawing.Size(672, 370);
			this.ClassicTab.TabIndex = 0;
			this.ClassicTab.Text = "同时指定";
			this.ClassicTab.UseVisualStyleBackColor = true;
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 664F));
			this.table.Controls.Add(this.ReplacerCombo, 0, 2);
			this.table.Controls.Add(this.ClassicReplacerLbl, 0, 1);
			this.table.Controls.Add(this.ReplaceClipsLbl, 0, 0);
			this.table.Controls.Add(this.ClassicReplacedLbl, 0, 3);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(3, 3);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
			this.table.RowCount = 4;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(666, 364);
			this.table.TabIndex = 7;
			// 
			// ReplacerCombo
			// 
			this.ReplacerCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerCombo.FormattingEnabled = true;
			this.ReplacerCombo.ItemHeight = 15;
			this.ReplacerCombo.Location = new System.Drawing.Point(3, 60);
			this.ReplacerCombo.Name = "ReplacerCombo";
			this.ReplacerCombo.Size = new System.Drawing.Size(660, 280);
			this.ReplacerCombo.TabIndex = 3;
			this.ReplacerCombo.SelectedIndexChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ClassicReplacerLbl
			// 
			this.ClassicReplacerLbl.AutoSize = true;
			this.ClassicReplacerLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClassicReplacerLbl.Location = new System.Drawing.Point(3, 42);
			this.ClassicReplacerLbl.Name = "ClassicReplacerLbl";
			this.ClassicReplacerLbl.Size = new System.Drawing.Size(660, 15);
			this.ClassicReplacerLbl.TabIndex = 4;
			this.ClassicReplacerLbl.Text = "指定的替换项为";
			this.ClassicReplacerLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplaceClipsLbl
			// 
			this.ReplaceClipsLbl.AutoSize = true;
			this.ReplaceClipsLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplaceClipsLbl.Location = new System.Drawing.Point(3, 6);
			this.ReplaceClipsLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.ReplaceClipsLbl.Name = "ReplaceClipsLbl";
			this.ReplaceClipsLbl.Size = new System.Drawing.Size(660, 30);
			this.ReplaceClipsLbl.TabIndex = 1;
			this.ReplaceClipsLbl.Text = "请先在轨道窗口中选中替换与被替换的素材，然后指定一个素材为替换的素材，剩余素材均为被替换素材。\r\n请先将替换素材的音视频创建分组，并确保替换素材放置在时间靠后的位" +
	"置并且尽量不与其它被替换素材位于同一轨道。";
			this.ReplaceClipsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ClassicReplacedLbl
			// 
			this.ClassicReplacedLbl.AutoSize = true;
			this.ClassicReplacedLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClassicReplacedLbl.Location = new System.Drawing.Point(3, 343);
			this.ClassicReplacedLbl.Name = "ClassicReplacedLbl";
			this.ClassicReplacedLbl.Size = new System.Drawing.Size(660, 15);
			this.ClassicReplacedLbl.TabIndex = 3;
			this.ClassicReplacedLbl.Text = "则剩余 0 项轨道剪辑将被替换为选定素材。";
			this.ClassicReplacedLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SeparationTab
			// 
			this.SeparationTab.Controls.Add(this.tableLayoutPanel1);
			this.SeparationTab.Location = new System.Drawing.Point(4, 24);
			this.SeparationTab.Name = "SeparationTab";
			this.SeparationTab.Size = new System.Drawing.Size(670, 362);
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
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3, 8, 3, 3);
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 325);
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
			this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 203);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(10);
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(658, 74);
			this.tableLayoutPanel3.TabIndex = 10;
			// 
			// SeparationReplacerInfo
			// 
			this.SeparationReplacerInfo.AutoSize = true;
			this.SeparationReplacerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SeparationReplacerInfo.Location = new System.Drawing.Point(67, 10);
			this.SeparationReplacerInfo.Name = "SeparationReplacerInfo";
			this.SeparationReplacerInfo.Size = new System.Drawing.Size(578, 54);
			this.SeparationReplacerInfo.TabIndex = 5;
			this.SeparationReplacerInfo.Text = "音频：无\r\n视频：无";
			this.SeparationReplacerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplacerIcon
			// 
			this.ReplacerIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacerIcon.Location = new System.Drawing.Point(13, 13);
			this.ReplacerIcon.MinimumSize = new System.Drawing.Size(48, 48);
			this.ReplacerIcon.Name = "ReplacerIcon";
			this.ReplacerIcon.Size = new System.Drawing.Size(48, 48);
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
			this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 283);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(658, 36);
			this.tableLayoutPanel5.TabIndex = 9;
			// 
			// SetReplacerBtn
			// 
			this.SetReplacerBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetReplacerBtn.Location = new System.Drawing.Point(3, 3);
			this.SetReplacerBtn.Name = "SetReplacerBtn";
			this.SetReplacerBtn.Size = new System.Drawing.Size(323, 30);
			this.SetReplacerBtn.TabIndex = 0;
			this.SetReplacerBtn.Text = "将选中的 0 个素材设为替换项";
			this.SetReplacerBtn.UseVisualStyleBackColor = true;
			this.SetReplacerBtn.Click += new System.EventHandler(this.SetReplacerBtn_Click);
			// 
			// BackToSelect2
			// 
			this.BackToSelect2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackToSelect2.Location = new System.Drawing.Point(332, 3);
			this.BackToSelect2.Name = "BackToSelect2";
			this.BackToSelect2.Size = new System.Drawing.Size(323, 30);
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
			this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 66);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(10);
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(658, 74);
			this.tableLayoutPanel2.TabIndex = 6;
			// 
			// SeparationReplacedInfo
			// 
			this.SeparationReplacedInfo.AutoSize = true;
			this.SeparationReplacedInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SeparationReplacedInfo.Location = new System.Drawing.Point(67, 10);
			this.SeparationReplacedInfo.Name = "SeparationReplacedInfo";
			this.SeparationReplacedInfo.Size = new System.Drawing.Size(578, 54);
			this.SeparationReplacedInfo.TabIndex = 5;
			this.SeparationReplacedInfo.Text = "已选中 0 个轨道素材，其中 0 个音频剪辑，0 个视频剪辑。";
			this.SeparationReplacedInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ReplacedIcon
			// 
			this.ReplacedIcon.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplacedIcon.Location = new System.Drawing.Point(13, 13);
			this.ReplacedIcon.MinimumSize = new System.Drawing.Size(48, 48);
			this.ReplacedIcon.Name = "ReplacedIcon";
			this.ReplacedIcon.Size = new System.Drawing.Size(48, 48);
			this.ReplacedIcon.TabIndex = 4;
			this.ReplacedIcon.TabStop = false;
			// 
			// SeparationReplacedLbl
			// 
			this.SeparationReplacedLbl.AutoSize = true;
			this.SeparationReplacedLbl.Location = new System.Drawing.Point(6, 48);
			this.SeparationReplacedLbl.Name = "SeparationReplacedLbl";
			this.SeparationReplacedLbl.Size = new System.Drawing.Size(55, 15);
			this.SeparationReplacedLbl.TabIndex = 0;
			this.SeparationReplacedLbl.Text = "被替换项";
			// 
			// SeparationReplacerLbl
			// 
			this.SeparationReplacerLbl.AutoSize = true;
			this.SeparationReplacerLbl.Location = new System.Drawing.Point(6, 185);
			this.SeparationReplacerLbl.Name = "SeparationReplacerLbl";
			this.SeparationReplacerLbl.Size = new System.Drawing.Size(43, 15);
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
			this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 146);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(658, 36);
			this.tableLayoutPanel4.TabIndex = 8;
			// 
			// SetReplacedBtn
			// 
			this.SetReplacedBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetReplacedBtn.Location = new System.Drawing.Point(3, 3);
			this.SetReplacedBtn.Name = "SetReplacedBtn";
			this.SetReplacedBtn.Size = new System.Drawing.Size(323, 30);
			this.SetReplacedBtn.TabIndex = 0;
			this.SetReplacedBtn.Text = "将选中的 0 个素材设为被替换项";
			this.SetReplacedBtn.UseVisualStyleBackColor = true;
			this.SetReplacedBtn.Click += new System.EventHandler(this.SetReplacedBtn_Click);
			// 
			// BackToSelect1
			// 
			this.BackToSelect1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackToSelect1.Location = new System.Drawing.Point(332, 3);
			this.BackToSelect1.Name = "BackToSelect1";
			this.BackToSelect1.Size = new System.Drawing.Size(323, 30);
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
			this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 11);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(664, 34);
			this.flowLayoutPanel1.TabIndex = 11;
			// 
			// ViewLbl
			// 
			this.ViewLbl.AutoSize = true;
			this.ViewLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewLbl.Location = new System.Drawing.Point(3, 0);
			this.ViewLbl.Margin = new System.Windows.Forms.Padding(3, 0, 9, 0);
			this.ViewLbl.Name = "ViewLbl";
			this.ViewLbl.Size = new System.Drawing.Size(31, 25);
			this.ViewLbl.TabIndex = 0;
			this.ViewLbl.Text = "查看";
			this.ViewLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ViewSelectReplacedRadio
			// 
			this.ViewSelectReplacedRadio.AutoSize = true;
			this.ViewSelectReplacedRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewSelectReplacedRadio.Location = new System.Drawing.Point(46, 3);
			this.ViewSelectReplacedRadio.Name = "ViewSelectReplacedRadio";
			this.ViewSelectReplacedRadio.Size = new System.Drawing.Size(73, 19);
			this.ViewSelectReplacedRadio.TabIndex = 1;
			this.ViewSelectReplacedRadio.Text = "被替换项";
			this.ViewSelectReplacedRadio.UseVisualStyleBackColor = true;
			this.ViewSelectReplacedRadio.CheckedChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ViewSelectReplacerRadio
			// 
			this.ViewSelectReplacerRadio.AutoSize = true;
			this.ViewSelectReplacerRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ViewSelectReplacerRadio.Location = new System.Drawing.Point(125, 3);
			this.ViewSelectReplacerRadio.Name = "ViewSelectReplacerRadio";
			this.ViewSelectReplacerRadio.Size = new System.Drawing.Size(61, 19);
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
			this.ViewSelectOriginalRadio.Location = new System.Drawing.Point(192, 3);
			this.ViewSelectOriginalRadio.Name = "ViewSelectOriginalRadio";
			this.ViewSelectOriginalRadio.Size = new System.Drawing.Size(61, 19);
			this.ViewSelectOriginalRadio.TabIndex = 3;
			this.ViewSelectOriginalRadio.TabStop = true;
			this.ViewSelectOriginalRadio.Text = "选中项";
			this.ViewSelectOriginalRadio.UseVisualStyleBackColor = true;
			this.ViewSelectOriginalRadio.CheckedChanged += new System.EventHandler(this.ReplacerCombo_SelectedIndexChanged);
			// 
			// ReplaceClipsForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(696, 448);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(712, 487);
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

	public partial class ReplaceClipsForm : Form, IInterpret, IConfigIniUser {
		private readonly EntryPoint parent;
		private readonly TrackEvent[] events;
		private Vegas vegas { get { return parent.vegas; } }
		private ConfigIni configIni { get { return parent.configIni; } }
		private List<ValidTrack> validTracks;
		private int suggestTrack = 0;
		private SeparationSpecifier separation;
		private bool requestClearAllReplacementTag = true;
		public ReplaceClipsForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			ReadIni();
			Translate();
			//Height = table.Height + dock.Height + SelectIntervalForm.MARGIN;
			events = parent.GetSelectedEvents();
			separation = new SeparationSpecifier(this);
			if (!FindReplacer() || validTracks.Count == 0) {
				EntryPoint.ShowError(new Exceptions.FailToSelectClipsException());
				Close();
			}
			if (requestClearAllReplacementTag) separation.ClearAllReplacementTag();
			UpdateSeparationInfo();
			UseTrackEventGroupCheck.CheckedChanged += (sender, e) => UpdateSeparationInfo();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			bool reserve = ReserveOriginalNameCheck.Checked;
			if (IsClassicTab) { // 同时指定
				ValidTrack replacerEvent = validTracks[ReplacerCombo.SelectedIndex];
				DoReplace(events, replacerEvent.LastSelectedAudioEvent, replacerEvent.LastSelectedVideoEvent, reserve);
			} else if (IsSeparationTab) { // 分别指定
				EntryPoint.EventSet replacer = separation.Replacer;
				DoReplace(separation.Replaced, replacer.audioEvent, replacer.videoEvent, reserve);
			}
			ResetSelect();
			Close();
		}

		private bool IsClassicTab { get { return tabs.SelectedTab == ClassicTab; } }
		private bool IsSeparationTab { get { return tabs.SelectedTab == SeparationTab; } }

		/// <summary>
		/// 执行替换轨道素材的主函数。
		/// </summary>
		/// <param name="replaced">被替换项的数组。</param>
		/// <param name="replacerAudio">替换项的音频部分。</param>
		/// <param name="replacerVideo">替换项的视频部分。</param>
		/// <param name="reserveClipName">保留原剪辑名称。</param>
		public static void DoReplace(TrackEvent[] replaced, AudioEvent replacerAudio, VideoEvent replacerVideo, bool reserveClipName = true) {
			foreach (TrackEvent trackEvent in replaced) {
				if (trackEvent == replacerAudio || trackEvent == replacerVideo) continue;
				Take expectTake;
				if (trackEvent.MediaType == MediaType.Video && replacerVideo != null)
					expectTake = replacerVideo.ActiveTake;
				else if (trackEvent.MediaType == MediaType.Audio && replacerAudio != null)
					expectTake = replacerAudio.ActiveTake;
				else continue;
				string name = trackEvent.ActiveTake.Name;
				trackEvent.Takes.Remove(trackEvent.ActiveTake);
				trackEvent.AddTake(expectTake.MediaStream, true);
				trackEvent.ActiveTake.Offset = expectTake.Offset;
				if (reserveClipName) trackEvent.ActiveTake.Name = name;
			}
		}

		#region 同时指定部分
		private class ValidTrack {
			private ReplaceClipsForm form;
			public Track Track;
			public List<TrackEvent> SelectedEvents = new List<TrackEvent>();
			public int TrackIndex { get { return Track.Index; } }
			public int SelectedEventsCount { get { return SelectedEvents.Count; } }
			public TrackEvent LastSelectedEvent = null;
			public VideoEvent LastSelectedVideoEvent = null;
			public AudioEvent LastSelectedAudioEvent = null;
			public ValidTrack(Track track, ReplaceClipsForm form) {
				Track = track;
				this.form = form;
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected)
						SelectedEvents.Add(trackEvent);
				if (SelectedEventsCount != 0) {
					LastSelectedEvent = SelectedEvents[SelectedEventsCount - 1];
					if (LastSelectedEvent.MediaType == MediaType.Video) LastSelectedVideoEvent = LastSelectedEvent as VideoEvent;
					if (LastSelectedEvent.MediaType == MediaType.Audio) LastSelectedAudioEvent = LastSelectedEvent as AudioEvent;
					if (LastSelectedEvent.IsGrouped && form.UseTrackEventGroupCheck.Checked) {
						foreach (TrackEvent trackEvent in LastSelectedEvent.Group) {
							if (trackEvent == LastSelectedEvent) continue;
							if (LastSelectedEvent.MediaType == MediaType.Video && trackEvent.MediaType == MediaType.Audio) {
								LastSelectedAudioEvent = trackEvent as AudioEvent;
								break;
							}
							if (LastSelectedEvent.MediaType == MediaType.Audio && trackEvent.MediaType == MediaType.Video) {
								LastSelectedVideoEvent = trackEvent as VideoEvent;
								break;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 查找用于替换的素材。
		/// </summary>
		/// <returns>返回是否成功找到。理论上不应该返回 false。</returns>
		private bool FindReplacer() {
			validTracks = new List<ValidTrack>();
			foreach (Track track in vegas.Project.Tracks) {
				ValidTrack validTrack = new ValidTrack(track, this);
				if (validTrack.SelectedEventsCount != 0) validTracks.Add(validTrack);
			}
			if (validTracks.Count == 0) return false;
			#region 去重
			/* var isEqualToTake = new Func<TrackEvent, TrackEvent, bool>((event1, event2) => {
				Take take1 = event1.ActiveTake, take2 = event2.ActiveTake;
				if (take1 == null || take2 == null) return true;
				return take1.Media == take2.Media && take1.Offset == take2.Offset;
			});
			for (int i = 1; i < validTracks.Count; i++)
				for (int j = 0; j < i; j++)
					if (isEqualToTake(validTracks[j].LastSelectedEvent, validTracks[i].LastSelectedEvent))
						validTracks.RemoveAt(i--); */
			var isEqualToTake2 = new Func<AudioEvent, VideoEvent, AudioEvent, VideoEvent, bool>((ae1, ve1, ae2, ve2) => {
				if (ae1 == null || ae2 == null) {
					if (ve1 == null || ve2 == null || ve1.ActiveTake == null || ve2.ActiveTake == null) return false;
					return ve1.ActiveTake.Media == ve2.ActiveTake.Media && ve1.ActiveTake.Offset == ve2.ActiveTake.Offset;
				} else if (ve1 == null || ve2 == null) {
					if (ae1 == null || ae2 == null || ae1.ActiveTake == null || ae2.ActiveTake == null) return false;
					return ae1.ActiveTake.Media == ae2.ActiveTake.Media && ae1.ActiveTake.Offset == ae2.ActiveTake.Offset;
				} else {
					if (ae1 == null || ae2 == null || ve1 == null || ve2 == null ||
						ae1.ActiveTake == null || ae2.ActiveTake == null || ve1.ActiveTake == null || ve2.ActiveTake == null) return false;
					return ae1.ActiveTake.Media == ae2.ActiveTake.Media && ae1.ActiveTake.Offset == ae2.ActiveTake.Offset &&
						ve1.ActiveTake.Media == ve2.ActiveTake.Media && ve1.ActiveTake.Offset == ve2.ActiveTake.Offset;
				}
			});
			for (int i = 1; i < validTracks.Count; i++)
				for (int j = 0; j < i; j++)
					if (isEqualToTake2(validTracks[j].LastSelectedAudioEvent, validTracks[j].LastSelectedVideoEvent,
						validTracks[i].LastSelectedAudioEvent, validTracks[i].LastSelectedVideoEvent))
						validTracks.RemoveAt(i--);
			if (validTracks.Count == 0) return false;
			#endregion
			int minCount = int.MaxValue;
			foreach (ValidTrack track in validTracks)
				if (track.SelectedEventsCount < minCount) minCount = track.SelectedEventsCount;
			if (minCount == int.MaxValue) return false;
			List<int> minCountTracks = new List<int>();
			for (int i = 0; i < validTracks.Count; i++)
				if (validTracks[i].SelectedEventsCount == minCount) minCountTracks.Add(i);
			if (minCountTracks.Count == 0) return false;
			else if (minCountTracks.Count == 1) suggestTrack = minCountTracks[0];
			else {
				double lastEventStart = -1;
				int lastEventIndex = -1;
				foreach (int i in minCountTracks) {
					TrackEvent lastEvent = validTracks[i].LastSelectedEvent;
					if (lastEvent == null) return false;
					double start = lastEvent.Start.ToMilliseconds();
					if (start > lastEventStart) {
						lastEventStart = start;
						lastEventIndex = i;
					}
				}
				if (lastEventStart == -1 || lastEventIndex == -1) return false;
				suggestTrack = lastEventIndex;
			}
			foreach (ValidTrack track in validTracks) {
				TrackEvent lastEvent = track.LastSelectedEvent;
				ReplacerCombo.Items.Add(string.Format("{5} {0:D}{4}{1}{3}{2}", track.TrackIndex, lastEvent.Start.ToString(), lastEvent.ActiveTake.Name, Lang.str.semicolon, Lang.str.colon, Lang.str.track));
			}
			ReplacerCombo.SelectedIndex = suggestTrack;
			return true;
		}

		private void SelectNone() {
			parent.SelectNoneEvents();
		}

		private void ResetSelect() {
			SelectNone();
			foreach (TrackEvent trackEvent in events)
				trackEvent.Selected = true;
		}

		private void SelectSpecifiedEvents(ValidTrack track) {
			SelectNone();
			if (track.LastSelectedVideoEvent != null) track.LastSelectedVideoEvent.Selected = true;
			if (track.LastSelectedAudioEvent != null) track.LastSelectedAudioEvent.Selected = true;
		}

		private void SelectSpecifiedEvents(TrackEvent[] events) {
			SelectNone();
			foreach (TrackEvent trackEvent in events)
				if (trackEvent != null)
					trackEvent.Selected = true;
		}

		private void SelectSpecifiedEvents(EntryPoint.EventSet eventSet) {
			TrackEvent[] events = { eventSet.audioEvent, eventSet.videoEvent };
			SelectSpecifiedEvents(events);
		}

		private void ReplacerCombo_SelectedIndexChanged(object sender, EventArgs e) {
			if (IsClassicTab) {
				int index = ReplacerCombo.SelectedIndex;
				ValidTrack track = validTracks[index];
				ClassicReplacedLbl.Text = string.Format(Lang.str.replaced_info, CountRemainEvents(track));
				SelectSpecifiedEvents(track);
				vegas.Transport.CursorPosition = track.LastSelectedEvent.Start.FixToFrame();
				vegas.Transport.ViewCursor(true);
				OkBtn.Enabled = ReplacerCombo.Items.Count > 0;
			} else if (IsSeparationTab) {
				SetReplacedBtn.Text = string.Format(Lang.str.separation_set_replaced, events.Length);
				SetReplacerBtn.Text = string.Format(Lang.str.separation_set_replacer, events.Length);
				if (ViewSelectOriginalRadio.Checked) SelectSpecifiedEvents(events);
				else if (ViewSelectReplacedRadio.Checked) SelectSpecifiedEvents(separation.Replaced);
				else if (ViewSelectReplacerRadio.Checked) SelectSpecifiedEvents(separation.Replacer);
				OkBtn.Enabled = IsSeparationReplacedOk && IsSeparationReplacerOk;
			}
			vegas.UpdateUI();
		}

		private int CountRemainEvents(ValidTrack replacer) {
			List<TrackEvent> remainEvents = new List<TrackEvent>();
			foreach (TrackEvent trackEvent in events) {
				if (trackEvent == replacer.LastSelectedAudioEvent || trackEvent == replacer.LastSelectedVideoEvent) continue;
				remainEvents.Add(trackEvent);
			}
			return remainEvents.Count;
		}
		#endregion

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			OkBtn.Text = str.replace;
			CancelBtn.Text = str.close;
			ClassicReplacerLbl.Text = str.replacer_is;
			ReplaceClipsLbl.Text = str.replacer_info;
			ClassicReplacedLbl.Text = str.replaced_info;
			Text = str.replace_clips;
			ClassicTab.Text = str.replacements_classic_specify;
			SeparationTab.Text = str.replacements_separation_specify;
			SeparationReplacedLbl.Text = str.replaced_label;
			SeparationReplacerLbl.Text = str.replacer_label;
			BackToSelect1.Text = BackToSelect2.Text = str.separation_back_to_select;
			UseTrackEventGroupCheck.Text = str.separation_use_clip_group;
			ReserveOriginalNameCheck.Text = str.separation_reserve_original_name;
			SeparationReplacedInfo.Text = str.separation_replaced_info;
			SeparationReplacerInfo.Text = str.audio + str.colon + str.none + '\n' + str.video + str.colon + str.none;
			SetReplacedBtn.Text = str.separation_set_replaced;
			SetReplacerBtn.Text = str.separation_set_replacer;
			ViewLbl.Text = str.view;
			ViewSelectReplacedRadio.Text = str.replaced_label;
			ViewSelectReplacerRadio.Text = str.replacer_label;
			ViewSelectOriginalRadio.Text = str.selected_items;
		}

		#region 分别指定部分
		private void BackToSelect_Click(object sender, EventArgs e) {
			parent.configForm.CloseAfterOpenHelperCheck.Checked = true;
			OpenToolDirectlyNext = true;
			Close();
		}

		private bool OpenToolDirectlyNext = false;

		private void SetReplacedBtn_Click(object sender, EventArgs e) {
			separation.Replaced = events;
			UpdateSeparationInfo();
			ReplacerCombo_SelectedIndexChanged(null, null);
			(sender as Button).Enabled = false;
		}

		private void SetReplacerBtn_Click(object sender, EventArgs e) {
			separation.SetReplacer(events);
			UpdateSeparationInfo();
			ReplacerCombo_SelectedIndexChanged(null, null);
			(sender as Button).Enabled = false;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool MessageBeep(uint type);

		[DllImport("Shell32.dll")]
		public static extern int ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, int nIcons);

		public static Icon LoadIconFromLib(int index, string libName = "imageres.dll") {
			IntPtr[] largeIcon = new IntPtr[1], smallIcon = new IntPtr[1];
			ExtractIconEx(libName, index, largeIcon, smallIcon, 1);
			Icon ic = Icon.FromHandle(largeIcon[0]);
			return ic;
		}

		private enum IconType {
			CROSS, CHECK
		}

		private Image GetIcon(IconType iconType) {
			Image icon = null;
			try {
				icon = iconType == IconType.CROSS ? LoadIconFromLib(100).ToBitmap() :
					iconType == IconType.CHECK ? LoadIconFromLib(101).ToBitmap() :
					null;
			} catch (Exception) { }
			return icon;
		}

		private class SeparationSpecifier {
			private readonly ReplaceClipsForm form;
			private EntryPoint parent { get { return form.parent; } }
			private Vegas vegas { get { return form.vegas; } }
			private static readonly Guid _GUID = Guid.Parse("2671A8C8-FDB1-4653-8C79-5B91A0CCFD1E");
			public static Guid REPLACEMENT_TAG_GUID { get { return _GUID; } }
			public SeparationSpecifier(ReplaceClipsForm form) { this.form = form; }
			private bool UseTrackEventGroup { get { return form.UseTrackEventGroupCheck.Checked; } }
			// 此处不可以使用枚举类型来替代。
			private const int INVALID_TAG = 0;
			private const int REPLACED_TAG = 1;
			private const int REPLACER_TAG = 2;
			public TrackEvent[] Replaced {
				get {
					return GetSpecifiedReplacement(REPLACED_TAG);
				}
				set {
					DeleteSpecifiedReplacement(REPLACED_TAG);
					Action<TrackEvent> SetTag = @event => @event.CustomData.SetObject(REPLACEMENT_TAG_GUID, REPLACED_TAG);
					foreach (TrackEvent trackEvent in value) {
						SetTag(trackEvent);
						if (trackEvent.IsGrouped && UseTrackEventGroup)
							foreach (TrackEvent trackEventInGroup in trackEvent.Group)
								SetTag(trackEventInGroup);
					}
				}
			}
			public EntryPoint.EventSet Replacer {
				get {
					return new EntryPoint.EventSet(GetSpecifiedReplacement(REPLACER_TAG), false); // 尤其注意这里是 false 而不是 UseTrackEventGroup
				}
				set {
					DeleteSpecifiedReplacement(REPLACER_TAG);
					foreach (TrackEvent trackEvent in new TrackEvent[] { value.audioEvent, value.videoEvent })
						if (trackEvent != null)
							trackEvent.CustomData.SetObject(REPLACEMENT_TAG_GUID, REPLACER_TAG);
				}
			}
			public void SetReplacer(EntryPoint.EventSet value) {
				Replacer = value;
			}
			public void SetReplacer(TrackEvent[] events) {
				Replacer = new EntryPoint.EventSet(events, UseTrackEventGroup);
			}
			private TrackEvent[] GetSpecifiedReplacement(int replacementTag) {
				List<TrackEvent> replacements = new List<TrackEvent>();
				foreach (TrackEvent trackEvent in parent.GetAllEvents()) {
					object replacement = trackEvent.CustomData.GetObject(REPLACEMENT_TAG_GUID);
					if (replacement is int && (int)replacement == replacementTag)
						replacements.Add(trackEvent);
				}
				return replacements.ToArray();
			}
			private void DeleteSpecifiedReplacement(int replacementTag) {
				foreach (TrackEvent trackEvent in parent.GetAllEvents()) {
					object replacement = trackEvent.CustomData.GetObject(REPLACEMENT_TAG_GUID);
					if (replacement != null && replacement is int && (int)replacement == replacementTag)
						trackEvent.CustomData.DeleteData(REPLACEMENT_TAG_GUID);
				}
			}
			public void ClearAllReplacementTag() {
				ClearAllReplacementTag(parent);
			}

			// 在未实例化对象外使用。
			public static void ClearAllReplacementTag(EntryPoint ep) {
				foreach (TrackEvent trackEvent in ep.GetAllEvents())
					if (trackEvent.CustomData.GetObject(REPLACEMENT_TAG_GUID) != null)
						trackEvent.CustomData.DeleteData(REPLACEMENT_TAG_GUID);
			}
		}

		private void UpdateSeparationReplacedInfo() {
			TrackEvent[] replaceds = separation.Replaced;
			EntryPoint.CountEventsType counts = new EntryPoint.CountEventsType(replaceds);
			SeparationReplacedInfo.Text = string.Format(Lang.str.separation_replaced_info, counts.count, counts.audioCount, counts.videoCount);
			IsSeparationReplacedOk = counts.count > 0;
		}

		private void UpdateSeparationReplacerInfo() {
			EntryPoint.EventSet replacers = separation.Replacer;
			string audioName = Lang.str.none, videoName = Lang.str.none;
			if (replacers.audioEvent != null) audioName = replacers.audioEvent.ActiveTake.Name;
			if (replacers.videoEvent != null) videoName = replacers.videoEvent.ActiveTake.Name;
			SeparationReplacerInfo.Text = Lang.str.audio + Lang.str.colon + audioName + '\n' + Lang.str.video + Lang.str.colon + videoName;
			IsSeparationReplacerOk = replacers.audioEvent != null && replacers.videoEvent != null;
		}

		private bool _isSeparationReplacedOk = false;
		private bool IsSeparationReplacedOk {
			get { return _isSeparationReplacedOk; }
			set {
				_isSeparationReplacedOk = value;
				ReplacedIcon.Image = GetIcon(value ? IconType.CHECK : IconType.CROSS);
			}
		}
		private bool _isSeparationReplacerOk = false;
		private bool IsSeparationReplacerOk {
			get { return _isSeparationReplacerOk; }
			set {
				_isSeparationReplacerOk = value;
				ReplacerIcon.Image = GetIcon(value ? IconType.CHECK : IconType.CROSS);
			}
		}

		private void ExamineSetSelectEnable() {
			// HashSet 不知道为啥反正就是用不了。
			TrackEvent[] selected = UseTrackEventGroupCheck.Checked ? parent.AppendGroupedEvents(events) : events;
			SetReplacedBtn.Enabled = !CompareList(new List<TrackEvent>(selected), new List<TrackEvent>(separation.Replaced));
			EntryPoint.EventSet settedSet = separation.Replacer; // set 的过去式理应不变形但是那样无法体现功能。
			EntryPoint.EventSet settingSet = new EntryPoint.EventSet(events, UseTrackEventGroupCheck.Checked);
			SetReplacerBtn.Enabled = settedSet.audioEvent != settingSet.audioEvent || settedSet.videoEvent != settingSet.videoEvent;
		}

		/// <summary>
		/// 列表去重。
		/// </summary>
		/// <typeparam name="T">列表类型。</typeparam>
		/// <param name="list">列表。</param>
		/// <param name="modify">是否修改原列表。</param>
		/// <returns>去重后的列表。</returns>
		public static List<T> DistinctList<T>(List<T> list, bool modify = true) where T : class {
			List<T> dealt = new List<T>();
			for (int i = 0; i < list.Count; i++) {
				bool isSame = false;
				for (int j = i + 1; j < list.Count; j++)
					if (list[j] == list[i]) {
						isSame = true;
						break;
					}
				if (!isSame) dealt.Add(list[i]);
			}
			if (modify) {
				list.Clear();
				list.AddRange(dealt);
			}
			return dealt;
		}

		/// <summary>
		/// 判断两个列表内元素是否相等。不计顺序及重复。
		/// </summary>
		/// <typeparam name="T">列表类型。</typeparam>
		/// <param name="list1">列表 1。</param>
		/// <param name="list2">列表 2。</param>
		/// <returns>是否相等。</returns>
		public static bool CompareList<T>(List<T> list1, List<T> list2) where T : class {
			list1 = DistinctList(list1, false);
			list2 = DistinctList(list2, false);
			if (list1.Count != list2.Count) return false;
			foreach (T item in list1)
				if (!list2.Contains(item))
					return false;
			return true;
		}

		private void UpdateSeparationInfo() {
			UpdateSeparationReplacedInfo();
			UpdateSeparationReplacerInfo();
			ExamineSetSelectEnable();
		}
		#endregion

		public void SaveIni() {
			configIni.StartSection("ReplaceClips");
			configIni.Write("Mode", tabs.SelectedIndex);
			configIni.Write("UseTrackEventGroup", UseTrackEventGroupCheck.Checked);
			configIni.Write("OpenToolDirectlyNext", OpenToolDirectlyNext);
			configIni.Write("ReserveOriginalName", ReserveOriginalNameCheck.Checked);
			configIni.EndSection();
		}

		public void ReadIni() {
			configIni.StartSection("ReplaceClips");
			int startMode = configIni.Read("Mode", DEFAULT_START_MODE);
			if (startMode != 0 && startMode != 1) startMode = DEFAULT_START_MODE;
			tabs.SelectedIndex = startMode;
			UseTrackEventGroupCheck.Checked = configIni.Read("UseTrackEventGroup", true);
			if (configIni.Read("OpenToolDirectlyNext", false))
				requestClearAllReplacementTag = false;
			configIni.Write("OpenToolDirectlyNext", false);
			ReserveOriginalNameCheck.Checked = configIni.Read("ReserveOriginalName", false);
			configIni.EndSection();
		}

		private const int DEFAULT_START_MODE = 1;

		private void ReplaceClipsForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (!OpenToolDirectlyNext) {
				bool cancel = false;
				if (IsSeparationReplacedOk || IsSeparationReplacerOk)
					e.Cancel = cancel = MessageBoxEx.Show(Lang.str.replace_clips_form_ensure_to_close, "", MessageBoxButtons.OKCancel, new string[] { Lang.str.exit, Lang.str.cancel }, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK;
				if (!cancel) separation.ClearAllReplacementTag();
			}
			ResetSelect();
			SaveIni();
		}
	}

	partial class SelectIntervalForm {
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
			this.ApplyBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.SubmitSelectBtn = new System.Windows.Forms.Button();
			this.SelectHowManyEachTimesBox = new System.Windows.Forms.NumericUpDown();
			this.SelectHowManyEachTimesLbl = new System.Windows.Forms.Label();
			this.SelectWhichEachGroupBox = new System.Windows.Forms.NumericUpDown();
			this.SelectOneEveryFewLbl = new System.Windows.Forms.Label();
			this.SelectWhichEachGroupLbl = new System.Windows.Forms.Label();
			this.SelectOneEveryFewBox = new System.Windows.Forms.NumericUpDown();
			this.ResetBtn = new System.Windows.Forms.Button();
			this.SelectIntervalLbl = new System.Windows.Forms.Label();
			this.SelectInfo = new System.Windows.Forms.Label();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectHowManyEachTimesBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).BeginInit();
			this.SuspendLayout();
			// 
			// dock
			// 
			this.dock.BackColor = System.Drawing.SystemColors.Control;
			this.dock.ColumnCount = 3;
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.dock.Controls.Add(this.ApplyBtn, 1, 0);
			this.dock.Controls.Add(this.CancelBtn, 2, 0);
			this.dock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dock.Location = new System.Drawing.Point(0, 216);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(544, 42);
			this.dock.TabIndex = 7;
			// 
			// ApplyBtn
			// 
			this.ApplyBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ApplyBtn.Location = new System.Drawing.Point(379, 8);
			this.ApplyBtn.Name = "ApplyBtn";
			this.ApplyBtn.Size = new System.Drawing.Size(75, 26);
			this.ApplyBtn.TabIndex = 1;
			this.ApplyBtn.Text = "应用(&A)";
			this.ApplyBtn.UseVisualStyleBackColor = true;
			this.ApplyBtn.Click += new System.EventHandler(this.ApplyBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(460, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "关闭(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.SubmitSelectBtn, 1, 5);
			this.table.Controls.Add(this.SelectHowManyEachTimesBox, 1, 4);
			this.table.Controls.Add(this.SelectHowManyEachTimesLbl, 0, 4);
			this.table.Controls.Add(this.SelectWhichEachGroupBox, 1, 3);
			this.table.Controls.Add(this.SelectOneEveryFewLbl, 0, 2);
			this.table.Controls.Add(this.SelectWhichEachGroupLbl, 0, 3);
			this.table.Controls.Add(this.SelectOneEveryFewBox, 1, 2);
			this.table.Controls.Add(this.ResetBtn, 0, 5);
			this.table.Controls.Add(this.SelectIntervalLbl, 0, 0);
			this.table.Controls.Add(this.SelectInfo, 0, 1);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(6);
			this.table.RowCount = 6;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(544, 190);
			this.table.TabIndex = 8;
			// 
			// SubmitSelectBtn
			// 
			this.SubmitSelectBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.SubmitSelectBtn.Location = new System.Drawing.Point(106, 156);
			this.SubmitSelectBtn.Name = "SubmitSelectBtn";
			this.SubmitSelectBtn.Size = new System.Drawing.Size(75, 25);
			this.SubmitSelectBtn.TabIndex = 10;
			this.SubmitSelectBtn.Text = "设定选中(&S)";
			this.SubmitSelectBtn.UseVisualStyleBackColor = true;
			this.SubmitSelectBtn.Visible = false;
			this.SubmitSelectBtn.Click += new System.EventHandler(this.SubmitSelectBtn_Click);
			// 
			// SelectHowManyEachTimesBox
			// 
			this.SelectHowManyEachTimesBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectHowManyEachTimesBox.Location = new System.Drawing.Point(106, 127);
			this.SelectHowManyEachTimesBox.Maximum = new decimal(new int[] {
			2,
			0,
			0,
			0});
			this.SelectHowManyEachTimesBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.SelectHowManyEachTimesBox.Name = "SelectHowManyEachTimesBox";
			this.SelectHowManyEachTimesBox.Size = new System.Drawing.Size(75, 23);
			this.SelectHowManyEachTimesBox.TabIndex = 4;
			this.SelectHowManyEachTimesBox.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// SelectHowManyEachTimesLbl
			// 
			this.SelectHowManyEachTimesLbl.AutoSize = true;
			this.SelectHowManyEachTimesLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectHowManyEachTimesLbl.Location = new System.Drawing.Point(9, 124);
			this.SelectHowManyEachTimesLbl.Name = "SelectHowManyEachTimesLbl";
			this.SelectHowManyEachTimesLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectHowManyEachTimesLbl.TabIndex = 8;
			this.SelectHowManyEachTimesLbl.Text = "每次要选取几个";
			this.SelectHowManyEachTimesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectWhichEachGroupBox
			// 
			this.SelectWhichEachGroupBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectWhichEachGroupBox.Location = new System.Drawing.Point(106, 98);
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
			// SelectOneEveryFewLbl
			// 
			this.SelectOneEveryFewLbl.AutoSize = true;
			this.SelectOneEveryFewLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectOneEveryFewLbl.Location = new System.Drawing.Point(9, 66);
			this.SelectOneEveryFewLbl.Name = "SelectOneEveryFewLbl";
			this.SelectOneEveryFewLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectOneEveryFewLbl.TabIndex = 0;
			this.SelectOneEveryFewLbl.Text = "每几个选择一个";
			this.SelectOneEveryFewLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectWhichEachGroupLbl
			// 
			this.SelectWhichEachGroupLbl.AutoSize = true;
			this.SelectWhichEachGroupLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectWhichEachGroupLbl.Location = new System.Drawing.Point(9, 95);
			this.SelectWhichEachGroupLbl.Name = "SelectWhichEachGroupLbl";
			this.SelectWhichEachGroupLbl.Size = new System.Drawing.Size(91, 29);
			this.SelectWhichEachGroupLbl.TabIndex = 1;
			this.SelectWhichEachGroupLbl.Text = "选择每组第几个";
			this.SelectWhichEachGroupLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectOneEveryFewBox
			// 
			this.SelectOneEveryFewBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SelectOneEveryFewBox.Location = new System.Drawing.Point(106, 69);
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
			// ResetBtn
			// 
			this.ResetBtn.AutoSize = true;
			this.ResetBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ResetBtn.Location = new System.Drawing.Point(9, 156);
			this.ResetBtn.MaximumSize = new System.Drawing.Size(0, 25);
			this.ResetBtn.Name = "ResetBtn";
			this.ResetBtn.Size = new System.Drawing.Size(91, 25);
			this.ResetBtn.TabIndex = 5;
			this.ResetBtn.Text = "重置选择(&R)";
			this.ResetBtn.UseVisualStyleBackColor = true;
			this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
			// 
			// SelectIntervalLbl
			// 
			this.SelectIntervalLbl.AutoSize = true;
			this.table.SetColumnSpan(this.SelectIntervalLbl, 2);
			this.SelectIntervalLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectIntervalLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.SelectIntervalLbl.Location = new System.Drawing.Point(9, 12);
			this.SelectIntervalLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.SelectIntervalLbl.Name = "SelectIntervalLbl";
			this.SelectIntervalLbl.Size = new System.Drawing.Size(526, 30);
			this.SelectIntervalLbl.TabIndex = 5;
			this.SelectIntervalLbl.Text = "请先在 Vegas 轨道中选中一些素材，然后再打开本对话框，使用下面的功能。\r\n本功能旨在辅助用户每隔一个或几个选中一个素材，然后可以执行“粘贴视频事件”等操作。" +
	"";
			this.SelectIntervalLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectInfo
			// 
			this.SelectInfo.AutoSize = true;
			this.table.SetColumnSpan(this.SelectInfo, 2);
			this.SelectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectInfo.Location = new System.Drawing.Point(9, 48);
			this.SelectInfo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.SelectInfo.Name = "SelectInfo";
			this.SelectInfo.Size = new System.Drawing.Size(526, 15);
			this.SelectInfo.TabIndex = 6;
			this.SelectInfo.Text = "已选中 0 个轨道剪辑。";
			this.SelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectIntervalForm
			// 
			this.AcceptButton = this.ApplyBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(544, 258);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectIntervalForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "快速间隔选择";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.SelectHowManyEachTimesBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectWhichEachGroupBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SelectOneEveryFewBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button ApplyBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel table;
		public System.Windows.Forms.NumericUpDown SelectWhichEachGroupBox;
		public System.Windows.Forms.Label SelectOneEveryFewLbl;
		public System.Windows.Forms.Label SelectWhichEachGroupLbl;
		public System.Windows.Forms.NumericUpDown SelectOneEveryFewBox;
		public System.Windows.Forms.Button ResetBtn;
		public System.Windows.Forms.Label SelectIntervalLbl;
		public System.Windows.Forms.Label SelectInfo;
		public System.Windows.Forms.NumericUpDown SelectHowManyEachTimesBox;
		public System.Windows.Forms.Label SelectHowManyEachTimesLbl;
		public System.Windows.Forms.Button SubmitSelectBtn;
	}

	public partial class SelectIntervalForm : Form, IInterpret {
		private readonly EntryPoint parent;
		private TrackEvent[] events;
		private Vegas vegas { get { return parent.vegas; } }
		internal const int MARGIN = 40;
		public SelectIntervalForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			Translate();
			Height = table.Height + dock.Height + MARGIN;
			SubmitSelectBtn_Click(null, null);
			foreach (Control control in table.Controls)
				if (control is NumericUpDown)
					control.MouseWheel += AutoLayoutTracksGridForm.NumericUpDown_MouseWheel;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ApplyBtn_Click(object sender, EventArgs e) {
			SelectInterval((int)SelectOneEveryFewBox.Value, (int)SelectWhichEachGroupBox.Value, (int)SelectHowManyEachTimesBox.Value);
			vegas.UpdateUI();
		}

		private void SelectOneEveryFewBox_ValueChanged(object sender, EventArgs e) {
			int divisor = (int)SelectOneEveryFewBox.Value, remainder = (int)SelectWhichEachGroupBox.Value, remainder2 = (int)SelectHowManyEachTimesBox.Value;
			if (remainder > divisor) SelectWhichEachGroupBox.Value = remainder = divisor;
			if (remainder2 > divisor) SelectHowManyEachTimesBox.Value = remainder2 = divisor;
			SelectWhichEachGroupBox.Maximum = divisor;
			SelectHowManyEachTimesBox.Maximum = divisor;
		}

		private void SubmitSelectBtn_Click(object sender, EventArgs e) {
			events = parent.GetSelectedEvents();
			SelectInfo.Text = string.Format(Lang.str.select_events_count_info, events.Length);
		}

		private void ResetBtn_Click(object sender, EventArgs e) {
			ResetSelect();
			vegas.UpdateUI();
		}

		/// <summary>
		/// 每几个选第几个。
		/// </summary>
		/// <param name="everyFew">每几个</param>
		/// <param name="whichOne">第几个</param>
		/// <param name="howMany">选几个</param>
		public void SelectInterval(int everyFew, int whichOne, int howMany) {
			ResetSelect();
			whichOne--; howMany--;
			if (everyFew < 2 || whichOne < 0 || howMany < 0 || whichOne > everyFew) return;
			foreach (Track track in vegas.Project.Tracks) {
				int j = -1;
				foreach (TrackEvent trackEvent in track.Events)
					if (trackEvent.Selected) {
						j = (j + 1) % everyFew;
						bool selected = j >= whichOne && j <= whichOne + howMany || j + everyFew >= whichOne && j + everyFew <= whichOne + howMany;
						trackEvent.Selected = selected;
					}
			}
		}

		public void ResetSelect() {
			var contains = new Func<TrackEvent[], TrackEvent, bool>((list, item) => {
				foreach (TrackEvent control in list)
					if (control == item)
						return true;
				return false;
			});
			foreach (Track track in vegas.Project.Tracks)
				foreach (TrackEvent trackEvent in track.Events)
					trackEvent.Selected = contains(events, trackEvent);
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			SelectIntervalLbl.Font = new Font(str.info_label_font, 9F);
			ApplyBtn.Text = str.apply;
			CancelBtn.Text = str.close;
			SubmitSelectBtn.Text = str.submit_select;
			SelectHowManyEachTimesLbl.Text = str.select_how_many;
			SelectOneEveryFewLbl.Text = str.every_few;
			SelectWhichEachGroupLbl.Text = str.which_one;
			ResetBtn.Text = str.reset_select;
			SelectIntervalLbl.Text = str.select_interval_info + "\r\n" + str.select_interval_configform_info;
			SelectInfo.Text = str.select_events_count_info;
			Text = str.quick_select_interval;
		}
	}

	/// <summary>
	/// 需要解释翻译的接口。
	/// </summary>
	public interface IInterpret {
		/// <summary>
		/// 本地化字符串。
		/// </summary>
		void Translate();
	}

	/// <summary>
	/// 自动布局轨道类的接口。
	/// </summary>
	public interface IAutoLayoutTracks {
		/// <summary>
		/// 自动布局轨道的方法。
		/// </summary>
		void Arrange();
		/// <summary>
		/// 从自动布局轨道属性对象中读取配置。
		/// </summary>
		//void ReadFromInfo(AutoLayoutTracksInfos.BaseAutoLayoutTracksInfo info = null);
	}

	partial class AutoLayoutTracksGridForm {
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
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.SquareRadio = new System.Windows.Forms.RadioButton();
			this.CustomRadio = new System.Windows.Forms.RadioButton();
			this.CustomGroup = new System.Windows.Forms.GroupBox();
			this.CustomTable = new System.Windows.Forms.TableLayoutPanel();
			this.RowCountBox = new System.Windows.Forms.NumericUpDown();
			this.RowCountLbl = new System.Windows.Forms.Label();
			this.ColumnCountLbl = new System.Windows.Forms.Label();
			this.ColumnCountBox = new System.Windows.Forms.NumericUpDown();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.FillRadio = new System.Windows.Forms.RadioButton();
			this.AdaptRadio = new System.Windows.Forms.RadioButton();
			this.ReverseTracksCheck = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.PaddingLbl = new System.Windows.Forms.Label();
			this.PaddingBox = new System.Windows.Forms.NumericUpDown();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.CustomGroup.SuspendLayout();
			this.CustomTable.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RowCountBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ColumnCountBox)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PaddingBox)).BeginInit();
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
			this.dock.Location = new System.Drawing.Point(0, 306);
			this.dock.Margin = new System.Windows.Forms.Padding(5);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(355, 52);
			this.dock.TabIndex = 7;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(147, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(249, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.SquareRadio, 0, 0);
			this.table.Controls.Add(this.CustomRadio, 0, 1);
			this.table.Controls.Add(this.CustomGroup, 0, 2);
			this.table.Controls.Add(this.ReverseTracksCheck, 0, 3);
			this.table.Controls.Add(this.tableLayoutPanel4, 0, 4);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(11);
			this.table.RowCount = 5;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.table.Size = new System.Drawing.Size(355, 303);
			this.table.TabIndex = 9;
			// 
			// SquareRadio
			// 
			this.SquareRadio.AutoSize = true;
			this.SquareRadio.Checked = true;
			this.SquareRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SquareRadio.Location = new System.Drawing.Point(15, 15);
			this.SquareRadio.Margin = new System.Windows.Forms.Padding(4);
			this.SquareRadio.Name = "SquareRadio";
			this.SquareRadio.Size = new System.Drawing.Size(325, 24);
			this.SquareRadio.TabIndex = 0;
			this.SquareRadio.TabStop = true;
			this.SquareRadio.Text = "平方";
			this.SquareRadio.UseVisualStyleBackColor = true;
			this.SquareRadio.CheckedChanged += new System.EventHandler(this.CustomRadio_CheckedChanged);
			// 
			// CustomRadio
			// 
			this.CustomRadio.AutoSize = true;
			this.CustomRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CustomRadio.Location = new System.Drawing.Point(15, 47);
			this.CustomRadio.Margin = new System.Windows.Forms.Padding(4);
			this.CustomRadio.Name = "CustomRadio";
			this.CustomRadio.Size = new System.Drawing.Size(325, 24);
			this.CustomRadio.TabIndex = 1;
			this.CustomRadio.Text = "自定义";
			this.CustomRadio.UseVisualStyleBackColor = true;
			this.CustomRadio.CheckedChanged += new System.EventHandler(this.CustomRadio_CheckedChanged);
			// 
			// CustomGroup
			// 
			this.CustomGroup.AutoSize = true;
			this.CustomGroup.Controls.Add(this.CustomTable);
			this.CustomGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.CustomGroup.Location = new System.Drawing.Point(15, 79);
			this.CustomGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 8);
			this.CustomGroup.Name = "CustomGroup";
			this.CustomGroup.Padding = new System.Windows.Forms.Padding(4);
			this.CustomGroup.Size = new System.Drawing.Size(325, 138);
			this.CustomGroup.TabIndex = 2;
			this.CustomGroup.TabStop = false;
			// 
			// CustomTable
			// 
			this.CustomTable.AutoSize = true;
			this.CustomTable.ColumnCount = 2;
			this.CustomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.CustomTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.CustomTable.Controls.Add(this.RowCountBox, 1, 1);
			this.CustomTable.Controls.Add(this.RowCountLbl, 0, 1);
			this.CustomTable.Controls.Add(this.ColumnCountLbl, 0, 0);
			this.CustomTable.Controls.Add(this.ColumnCountBox, 1, 0);
			this.CustomTable.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.CustomTable.Dock = System.Windows.Forms.DockStyle.Top;
			this.CustomTable.Location = new System.Drawing.Point(4, 24);
			this.CustomTable.Margin = new System.Windows.Forms.Padding(4);
			this.CustomTable.Name = "CustomTable";
			this.CustomTable.RowCount = 3;
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.CustomTable.Size = new System.Drawing.Size(317, 110);
			this.CustomTable.TabIndex = 2;
			// 
			// RowCountBox
			// 
			this.RowCountBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.RowCountBox.Enabled = false;
			this.RowCountBox.Location = new System.Drawing.Point(51, 39);
			this.RowCountBox.Margin = new System.Windows.Forms.Padding(4);
			this.RowCountBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.RowCountBox.Name = "RowCountBox";
			this.RowCountBox.ReadOnly = true;
			this.RowCountBox.Size = new System.Drawing.Size(94, 27);
			this.RowCountBox.TabIndex = 3;
			this.RowCountBox.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			// 
			// RowCountLbl
			// 
			this.RowCountLbl.AutoSize = true;
			this.RowCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RowCountLbl.Location = new System.Drawing.Point(4, 35);
			this.RowCountLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.RowCountLbl.Name = "RowCountLbl";
			this.RowCountLbl.Size = new System.Drawing.Size(39, 35);
			this.RowCountLbl.TabIndex = 1;
			this.RowCountLbl.Text = "行数";
			this.RowCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColumnCountLbl
			// 
			this.ColumnCountLbl.AutoSize = true;
			this.ColumnCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ColumnCountLbl.Location = new System.Drawing.Point(4, 0);
			this.ColumnCountLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ColumnCountLbl.Name = "ColumnCountLbl";
			this.ColumnCountLbl.Size = new System.Drawing.Size(39, 35);
			this.ColumnCountLbl.TabIndex = 0;
			this.ColumnCountLbl.Text = "列数";
			this.ColumnCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ColumnCountBox
			// 
			this.ColumnCountBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.ColumnCountBox.Location = new System.Drawing.Point(51, 4);
			this.ColumnCountBox.Margin = new System.Windows.Forms.Padding(4);
			this.ColumnCountBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.ColumnCountBox.Name = "ColumnCountBox";
			this.ColumnCountBox.Size = new System.Drawing.Size(94, 27);
			this.ColumnCountBox.TabIndex = 2;
			this.ColumnCountBox.Value = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.ColumnCountBox.ValueChanged += new System.EventHandler(this.ColumnCountBox_ValueChanged);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.CustomTable.SetColumnSpan(this.flowLayoutPanel1, 2);
			this.flowLayoutPanel1.Controls.Add(this.FillRadio);
			this.flowLayoutPanel1.Controls.Add(this.AdaptRadio);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(4, 74);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(309, 32);
			this.flowLayoutPanel1.TabIndex = 4;
			this.flowLayoutPanel1.WrapContents = false;
			// 
			// FillRadio
			// 
			this.FillRadio.AutoSize = true;
			this.FillRadio.Checked = true;
			this.FillRadio.Location = new System.Drawing.Point(4, 4);
			this.FillRadio.Margin = new System.Windows.Forms.Padding(4);
			this.FillRadio.Name = "FillRadio";
			this.FillRadio.Size = new System.Drawing.Size(60, 24);
			this.FillRadio.TabIndex = 0;
			this.FillRadio.TabStop = true;
			this.FillRadio.Text = "填充";
			this.FillRadio.UseVisualStyleBackColor = true;
			// 
			// AdaptRadio
			// 
			this.AdaptRadio.AutoSize = true;
			this.AdaptRadio.Location = new System.Drawing.Point(72, 4);
			this.AdaptRadio.Margin = new System.Windows.Forms.Padding(4);
			this.AdaptRadio.Name = "AdaptRadio";
			this.AdaptRadio.Size = new System.Drawing.Size(60, 24);
			this.AdaptRadio.TabIndex = 1;
			this.AdaptRadio.Text = "适应";
			this.AdaptRadio.UseVisualStyleBackColor = true;
			// 
			// ReverseTracksCheck
			// 
			this.ReverseTracksCheck.AutoSize = true;
			this.ReverseTracksCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReverseTracksCheck.Location = new System.Drawing.Point(15, 229);
			this.ReverseTracksCheck.Margin = new System.Windows.Forms.Padding(4);
			this.ReverseTracksCheck.Name = "ReverseTracksCheck";
			this.ReverseTracksCheck.Size = new System.Drawing.Size(325, 24);
			this.ReverseTracksCheck.TabIndex = 3;
			this.ReverseTracksCheck.Text = "降序排序";
			this.ReverseTracksCheck.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Controls.Add(this.PaddingLbl, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.PaddingBox, 1, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(11, 257);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(333, 35);
			this.tableLayoutPanel4.TabIndex = 4;
			// 
			// PaddingLbl
			// 
			this.PaddingLbl.AutoSize = true;
			this.PaddingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PaddingLbl.Location = new System.Drawing.Point(4, 0);
			this.PaddingLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PaddingLbl.Name = "PaddingLbl";
			this.PaddingLbl.Size = new System.Drawing.Size(69, 35);
			this.PaddingLbl.TabIndex = 0;
			this.PaddingLbl.Text = "增加边距";
			this.PaddingLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PaddingBox
			// 
			this.PaddingBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.PaddingBox.Location = new System.Drawing.Point(81, 4);
			this.PaddingBox.Margin = new System.Windows.Forms.Padding(4);
			this.PaddingBox.Maximum = new decimal(new int[] {
			50,
			0,
			0,
			0});
			this.PaddingBox.Name = "PaddingBox";
			this.PaddingBox.Size = new System.Drawing.Size(94, 27);
			this.PaddingBox.TabIndex = 1;
			// 
			// AutoLayoutTracksGridForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSize = true;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(355, 358);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AutoLayoutTracksGridForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自动布局轨道 - 网格布局";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.CustomGroup.ResumeLayout(false);
			this.CustomGroup.PerformLayout();
			this.CustomTable.ResumeLayout(false);
			this.CustomTable.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RowCountBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ColumnCountBox)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PaddingBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.RadioButton SquareRadio;
		private System.Windows.Forms.RadioButton CustomRadio;
		private System.Windows.Forms.GroupBox CustomGroup;
		private System.Windows.Forms.TableLayoutPanel CustomTable;
		private System.Windows.Forms.NumericUpDown RowCountBox;
		private System.Windows.Forms.Label RowCountLbl;
		private System.Windows.Forms.Label ColumnCountLbl;
		private System.Windows.Forms.NumericUpDown ColumnCountBox;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.RadioButton FillRadio;
		private System.Windows.Forms.RadioButton AdaptRadio;
		private System.Windows.Forms.CheckBox ReverseTracksCheck;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label PaddingLbl;
		private System.Windows.Forms.NumericUpDown PaddingBox;
	}

	public partial class AutoLayoutTracksGridForm : Form, IAutoLayoutTracks, IConfigIniUser, IInterpret {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		private ConfigIni configIni { get { return parent.configIni; } }
		private AutoLayoutTracksInfos.GridInfo info;
		private readonly VideoTrack[] tracks;
		private int Count {
			get {
				return info != null ? info.Channels.Count : tracks != null ? tracks.Length : 0;
			}
		}
		public bool IsToolMode { get { return info == null; } }
		public AutoLayoutTracksGridForm(EntryPoint e) : this(e, null, null) { }
		public AutoLayoutTracksGridForm(EntryPoint entryPoint, AutoLayoutTracksInfos.GridInfo info = null, VideoTrack[] tracks = null) {
			InitializeComponent();
			parent = entryPoint;
			this.info = info;
			Icon = ConfigForm.icon;
			Translate();
			Height = table.Height + dock.Height + SelectIntervalForm.MARGIN;
			CustomGroup.Enabled = false;
			this.tracks = tracks ?? parent.GetSelectedVideoTracks();
			if (Count == 0) {
				EntryPoint.ShowError(new Exceptions.FailToSelectTracksException());
				Close();
			} else if (Count > 100) {
				MessageBox.Show(string.Format(Lang.str.selected_tracks_too_much, Count),
					Lang.str.selected_tracks_too_much_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Close();
			}
			if (IsToolMode) ColumnCountBox.Maximum = Math.Min(Count, 100);
			ReadIni();
			ReadFromInfo();
			CustomRadio_CheckedChanged(null, null);
			ColumnCountBox.MouseWheel += NumericUpDown_MouseWheel;
			if (tracks == null) FormClosing += (sender, e) => SaveIni();
		}

		public static void NumericUpDown_MouseWheel(object sender, MouseEventArgs e) {
			NumericUpDown numeric = sender as NumericUpDown;
			HandledMouseEventArgs hme = e as HandledMouseEventArgs;
			if (hme != null) hme.Handled = true;
			// if (e is HandledMouseEventArgs hme) hme.Handled = true; // 不支持
			decimal increment = numeric.Increment;
			if (e.Delta > 0) {
				if (numeric.Value + increment > numeric.Maximum) numeric.Value = numeric.Maximum;
				else numeric.Value += increment;
			} else if (e.Delta < 0) {
				if (numeric.Value - increment < numeric.Minimum) numeric.Value = numeric.Minimum;
				else numeric.Value -= increment;
			}
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			if (IsToolMode) Arrange();
			else {
				info.enabled = true;
				info.isSquare = SquareRadio.Checked;
				info.columns = (int)ColumnCountBox.Value;
				info.isFill = FillRadio.Checked;
				info.padding = (int)PaddingBox.Value;
				info.descending = ReverseTracksCheck.Checked;
			}
			Close();
		}

		private void ReadFromInfo(AutoLayoutTracksInfos.GridInfo _info = null) {
			if (_info != null) info = _info;
			if (IsToolMode || !info.enabled) return;

			if (info.isSquare) SquareRadio.Checked = true; else CustomRadio.Checked = true;
			ColumnCountBox.SetValue(info.columns);
			if (info.columns > ColumnCountBox.Maximum) ColumnCountBox.Value = ColumnCountBox.Maximum;
			if (info.isFill) FillRadio.Checked = true; else AdaptRadio.Checked = true;
			PaddingBox.SetValue(info.padding);
			ReverseTracksCheck.Checked = info.descending;
			if (_info != null) ReverseTracksCheck.Checked = !ReverseTracksCheck.Checked;

			if (_info != null) info = null;
		}

		private bool IsCustom { get { return CustomRadio.Checked; } }
		private bool IsFill { get { return FillRadio.Checked; } }
		private int Column { get { return (int)ColumnCountBox.Value; } }
		private int Row {
			get {
				return !IsCustom ? Column : (int)Math.Ceiling(Count / (double)Column);
			}
		}
		private bool IsReverse { get { return ReverseTracksCheck.Checked; } }
		private int TracksPadding { get { return (int)PaddingBox.Value; } }

		private void ColumnCountBox_ValueChanged(object sender, EventArgs e) {
			if (!IsCustom) ColumnCountBox.SetValue((int)Math.Ceiling(Math.Sqrt(Count)));
			RowCountBox.SetValue(Row);
		}

		private void CustomRadio_CheckedChanged(object sender, EventArgs e) {
			CustomGroup.Enabled = IsCustom;
			ColumnCountBox_ValueChanged(null, null);
		}

		public void Arrange() {
			ColumnCountBox_ValueChanged(null, null);
			int HEIGHT = vegas.Project.Video.Height, WIDTH = vegas.Project.Video.Width;
			float padding = (100 - TracksPadding) / 100.0f;
			for (int index = 0; index < Count; index++) {
				VideoTrack track = tracks[index];
				int n = !IsReverse ? index : Count - 1 - index; // 序数
				int i = n % Column; // 列
				int j = n / Column; // 行
				TrackMotionKeyframe frame = track.TrackMotion.MotionKeyframes[0];
				double x = WIDTH / 2.0 * ((2.0 * i + 1.0) / Column - 1.0);
				double y = HEIGHT / 2.0 * (1.0 - (2.0 * j + 1.0) / Row);
				frame.PositionX = x;
				frame.PositionY = y;
				double width = (double)WIDTH / Column, height = (double)HEIGHT / Row;
				double newWidth = width, newHeight = width / WIDTH * HEIGHT;
				if (IsCustom && (newHeight < height && IsFill || newHeight > height && !IsFill)) {
					newHeight = height;
					newWidth = height / HEIGHT * WIDTH;
				}
				frame.Width = newWidth * padding;
				frame.Height = newHeight * padding;
				if (IsCustom && IsFill) {
					Plugin.Init(vegas);
					if (Plugin.crop == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.crop)); return; }
					Effect effect = track.Effects.AddEffect(Plugin.crop);
					(effect.OFXEffect.FindParameterByName("XScale") as OFXDoubleParameter).Value = width / newWidth;
					(effect.OFXEffect.FindParameterByName("YScale") as OFXDoubleParameter).Value = height / newHeight;
				}
			}
		}

		public static void Arrange(VideoTrack[] tracks, AutoLayoutTracksInfos.GridInfo info, EntryPoint entryPoint) {
			if (!info.enabled) return;
			AutoLayoutTracksGridForm me = new AutoLayoutTracksGridForm(entryPoint, null, tracks);
			me.ReadFromInfo(info);
			me.Arrange();
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			OkBtn.Text = str.complete;
			CancelBtn.Text = str.cancel;
			SquareRadio.Text = str.square;
			CustomRadio.Text = str.custom;
			RowCountLbl.Text = str.row_count;
			ColumnCountLbl.Text = str.column_count;
			FillRadio.Text = str.fill;
			AdaptRadio.Text = str.adapt;
			ReverseTracksCheck.Text = IsToolMode ? str.descending : str.ascending;
			PaddingLbl.Text = str.increase_padding;
			Text = str.auto_layout_tracks + " - " + str.grid_layout;
		}

		public void ReadIni() {
			configIni.StartSection("AutoLayoutTracksGrid");
			ReverseTracksCheck.Checked = configIni.Read("ReverseTracks", true);
			PaddingBox.SetValue(configIni.Read("Padding", 0), 0);
			bool isSquare = configIni.Read("IsSquare", true);
			if (isSquare) SquareRadio.Checked = true;
			else {
				CustomRadio.Checked = true;
				if (configIni.Read("IsFill", true)) FillRadio.Checked = true;
				else AdaptRadio.Checked = true;
				ColumnCountBox.SetValue(configIni.Read("ColumnCount", -1), null);
			}
			configIni.EndSection();
		}

		public void SaveIni() {
			configIni.StartSection("AutoLayoutTracksGrid");
			configIni.Write("IsSquare", SquareRadio.Checked);
			configIni.Write("IsFill", FillRadio.Checked);
			configIni.Write("ReverseTracks", ReverseTracksCheck.Checked);
			configIni.Write("ColumnCount", ColumnCountBox.Value);
			configIni.Write("Padding", PaddingBox.Value);
			configIni.EndSection();
		}
	}

	partial class AutoLayoutTracksBox3dForm {
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
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.BottomCombo = new System.Windows.Forms.ComboBox();
			this.TopCombo = new System.Windows.Forms.ComboBox();
			this.RightCombo = new System.Windows.Forms.ComboBox();
			this.LeftCombo = new System.Windows.Forms.ComboBox();
			this.BackCombo = new System.Windows.Forms.ComboBox();
			this.BottomLbl = new System.Windows.Forms.Label();
			this.TopLbl = new System.Windows.Forms.Label();
			this.RightLbl = new System.Windows.Forms.Label();
			this.LeftLbl = new System.Windows.Forms.Label();
			this.BackLbl = new System.Windows.Forms.Label();
			this.InfoLbl = new System.Windows.Forms.Label();
			this.FrontLbl = new System.Windows.Forms.Label();
			this.FrontCombo = new System.Windows.Forms.ComboBox();
			this.DelOrigTrackCheck = new System.Windows.Forms.CheckBox();
			this.UseVideoLongerSideCheck = new System.Windows.Forms.CheckBox();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(299, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(380, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
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
			this.dock.Location = new System.Drawing.Point(0, 327);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(464, 42);
			this.dock.TabIndex = 10;
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 2;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.Controls.Add(this.BottomCombo, 1, 6);
			this.table.Controls.Add(this.TopCombo, 1, 5);
			this.table.Controls.Add(this.RightCombo, 1, 4);
			this.table.Controls.Add(this.LeftCombo, 1, 3);
			this.table.Controls.Add(this.BackCombo, 1, 2);
			this.table.Controls.Add(this.BottomLbl, 0, 6);
			this.table.Controls.Add(this.TopLbl, 0, 5);
			this.table.Controls.Add(this.RightLbl, 0, 4);
			this.table.Controls.Add(this.LeftLbl, 0, 3);
			this.table.Controls.Add(this.BackLbl, 0, 2);
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.FrontLbl, 0, 1);
			this.table.Controls.Add(this.FrontCombo, 1, 1);
			this.table.Controls.Add(this.DelOrigTrackCheck, 0, 7);
			this.table.Controls.Add(this.UseVideoLongerSideCheck, 0, 8);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
			this.table.RowCount = 9;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(464, 306);
			this.table.TabIndex = 12;
			// 
			// BottomCombo
			// 
			this.BottomCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BottomCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.BottomCombo.FormattingEnabled = true;
			this.BottomCombo.Location = new System.Drawing.Point(50, 216);
			this.BottomCombo.Name = "BottomCombo";
			this.BottomCombo.Size = new System.Drawing.Size(401, 23);
			this.BottomCombo.TabIndex = 12;
			this.BottomCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// TopCombo
			// 
			this.TopCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TopCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TopCombo.FormattingEnabled = true;
			this.TopCombo.Location = new System.Drawing.Point(50, 187);
			this.TopCombo.Name = "TopCombo";
			this.TopCombo.Size = new System.Drawing.Size(401, 23);
			this.TopCombo.TabIndex = 11;
			this.TopCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// RightCombo
			// 
			this.RightCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.RightCombo.FormattingEnabled = true;
			this.RightCombo.Location = new System.Drawing.Point(50, 158);
			this.RightCombo.Name = "RightCombo";
			this.RightCombo.Size = new System.Drawing.Size(401, 23);
			this.RightCombo.TabIndex = 10;
			this.RightCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// LeftCombo
			// 
			this.LeftCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.LeftCombo.FormattingEnabled = true;
			this.LeftCombo.Location = new System.Drawing.Point(50, 129);
			this.LeftCombo.Name = "LeftCombo";
			this.LeftCombo.Size = new System.Drawing.Size(401, 23);
			this.LeftCombo.TabIndex = 9;
			this.LeftCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// BackCombo
			// 
			this.BackCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.BackCombo.FormattingEnabled = true;
			this.BackCombo.Location = new System.Drawing.Point(50, 100);
			this.BackCombo.Name = "BackCombo";
			this.BackCombo.Size = new System.Drawing.Size(401, 23);
			this.BackCombo.TabIndex = 8;
			this.BackCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// BottomLbl
			// 
			this.BottomLbl.AutoSize = true;
			this.BottomLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BottomLbl.Location = new System.Drawing.Point(13, 213);
			this.BottomLbl.Name = "BottomLbl";
			this.BottomLbl.Size = new System.Drawing.Size(31, 29);
			this.BottomLbl.TabIndex = 6;
			this.BottomLbl.Text = "底面";
			this.BottomLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TopLbl
			// 
			this.TopLbl.AutoSize = true;
			this.TopLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TopLbl.Location = new System.Drawing.Point(13, 184);
			this.TopLbl.Name = "TopLbl";
			this.TopLbl.Size = new System.Drawing.Size(31, 29);
			this.TopLbl.TabIndex = 5;
			this.TopLbl.Text = "顶面";
			this.TopLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RightLbl
			// 
			this.RightLbl.AutoSize = true;
			this.RightLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RightLbl.Location = new System.Drawing.Point(13, 155);
			this.RightLbl.Name = "RightLbl";
			this.RightLbl.Size = new System.Drawing.Size(31, 29);
			this.RightLbl.TabIndex = 4;
			this.RightLbl.Text = "右面";
			this.RightLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LeftLbl
			// 
			this.LeftLbl.AutoSize = true;
			this.LeftLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftLbl.Location = new System.Drawing.Point(13, 126);
			this.LeftLbl.Name = "LeftLbl";
			this.LeftLbl.Size = new System.Drawing.Size(31, 29);
			this.LeftLbl.TabIndex = 3;
			this.LeftLbl.Text = "左面";
			this.LeftLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BackLbl
			// 
			this.BackLbl.AutoSize = true;
			this.BackLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BackLbl.Location = new System.Drawing.Point(13, 97);
			this.BackLbl.Name = "BackLbl";
			this.BackLbl.Size = new System.Drawing.Size(31, 29);
			this.BackLbl.TabIndex = 2;
			this.BackLbl.Text = "后面";
			this.BackLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// InfoLbl
			// 
			this.InfoLbl.AutoSize = true;
			this.table.SetColumnSpan(this.InfoLbl, 2);
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Location = new System.Drawing.Point(13, 17);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(438, 45);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "由于脚本功能限制，将会新建轨道并将选定轨道中的剪辑移动过去，原轨道中的轨道运动、效果等信息将会丢失。\r\n请在下方选定立方体的各个面所使用的轨道，如果为空则表示不设" +
	"定该面。";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FrontLbl
			// 
			this.FrontLbl.AutoSize = true;
			this.FrontLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FrontLbl.Location = new System.Drawing.Point(13, 68);
			this.FrontLbl.Name = "FrontLbl";
			this.FrontLbl.Size = new System.Drawing.Size(31, 29);
			this.FrontLbl.TabIndex = 1;
			this.FrontLbl.Text = "前面";
			this.FrontLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FrontCombo
			// 
			this.FrontCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FrontCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.FrontCombo.FormattingEnabled = true;
			this.FrontCombo.Location = new System.Drawing.Point(50, 71);
			this.FrontCombo.Name = "FrontCombo";
			this.FrontCombo.Size = new System.Drawing.Size(401, 23);
			this.FrontCombo.TabIndex = 7;
			this.FrontCombo.SelectedIndexChanged += new System.EventHandler(this.Combo_SelectedIndexChanged);
			// 
			// DelOrigTrackCheck
			// 
			this.DelOrigTrackCheck.AutoSize = true;
			this.DelOrigTrackCheck.Checked = true;
			this.DelOrigTrackCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.table.SetColumnSpan(this.DelOrigTrackCheck, 2);
			this.DelOrigTrackCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DelOrigTrackCheck.Location = new System.Drawing.Point(13, 248);
			this.DelOrigTrackCheck.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.DelOrigTrackCheck.Name = "DelOrigTrackCheck";
			this.DelOrigTrackCheck.Size = new System.Drawing.Size(438, 19);
			this.DelOrigTrackCheck.TabIndex = 13;
			this.DelOrigTrackCheck.Text = "删除原轨道";
			this.DelOrigTrackCheck.UseVisualStyleBackColor = true;
			// 
			// UseVideoLongerSideCheck
			// 
			this.UseVideoLongerSideCheck.AutoSize = true;
			this.table.SetColumnSpan(this.UseVideoLongerSideCheck, 2);
			this.UseVideoLongerSideCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UseVideoLongerSideCheck.Location = new System.Drawing.Point(13, 273);
			this.UseVideoLongerSideCheck.Name = "UseVideoLongerSideCheck";
			this.UseVideoLongerSideCheck.Size = new System.Drawing.Size(438, 19);
			this.UseVideoLongerSideCheck.TabIndex = 14;
			this.UseVideoLongerSideCheck.Text = "使用视频的长边作为立方体的棱长";
			this.UseVideoLongerSideCheck.UseVisualStyleBackColor = true;
			// 
			// AutoLayoutTracksBox3dForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(464, 369);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AutoLayoutTracksBox3dForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "自动布局轨道 - 3D 方盒布局";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.ComboBox BottomCombo;
		private System.Windows.Forms.ComboBox TopCombo;
		private System.Windows.Forms.ComboBox RightCombo;
		private System.Windows.Forms.ComboBox LeftCombo;
		private System.Windows.Forms.ComboBox BackCombo;
		private System.Windows.Forms.Label BottomLbl;
		private System.Windows.Forms.Label TopLbl;
		private System.Windows.Forms.Label RightLbl;
		private System.Windows.Forms.Label LeftLbl;
		private System.Windows.Forms.Label BackLbl;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.Label FrontLbl;
		private System.Windows.Forms.ComboBox FrontCombo;
		private System.Windows.Forms.CheckBox DelOrigTrackCheck;
		private System.Windows.Forms.CheckBox UseVideoLongerSideCheck;
	}

	public partial class AutoLayoutTracksBox3dForm : Form, IAutoLayoutTracks, IConfigIniUser, IInterpret {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		private ConfigIni configIni { get { return parent.configIni; } }
		private readonly List<VideoTrack> tracks;
		private readonly List<string> trackNames;
		private readonly ComboBox[] combos;
		public AutoLayoutTracksBox3dForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			Translate();
			Height = table.Height + dock.Height + SelectIntervalForm.MARGIN;
			Lang str = Lang.str;
			ToolTip toolTip = new ToolTip();
			toolTip.SetToolTip(UseVideoLongerSideCheck, str.use_video_longer_side_tooltip);
			SideName = new string[] { str.front_surface, str.back_surface, str.left_surface, str.right_surface, str.top_surface, str.bottom_surface };
			tracks = new List<VideoTrack> { null };
			VideoTrack[] _ = parent.GetSelectedVideoTracks();
			if (_.Length == 0) {
				EntryPoint.ShowError(new Exceptions.FailToSelectTracksException());
				Close();
			}
			tracks.AddRange(_);
			trackNames = new List<string>();
			foreach (VideoTrack track in tracks) {
				string name = "";
				if (track != null) {
					name += str.track + " " + (track.Index + 1);
					if (!string.IsNullOrWhiteSpace(track.Name)) name += str.colon + track.Name;
				}
				trackNames.Add(name);
			}
			combos = new ComboBox[] { FrontCombo, BackCombo, LeftCombo, RightCombo, TopCombo, BottomCombo };
			int _count = trackNames.Count;
			for (int i = 0; i < combos.Length; i++) {
				int j = i + 1;
				ComboBox combo = combos[i];
				combo.Items.AddRange(trackNames.ToArray());
				if (j < _count) combo.SelectedIndex = j;
				else combo.SelectedIndex = 0;
			}
			ReadIni();
			FormClosing += (sender, e) => SaveIni();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Arrange();
			Close();
		}

		public void ReadIni() {
			configIni.StartSection("AutoLayoutTracksBox3d");
			DelOrigTrackCheck.Checked = configIni.Read("DeleteOriginalTracks", true);
			UseVideoLongerSideCheck.Checked = configIni.Read("UseVideoLongerSide", false);
			configIni.EndSection();
		}

		public void SaveIni() {
			configIni.StartSection("AutoLayoutTracksBox3d");
			configIni.Write("DeleteOriginalTracks", DelOrigTrackCheck.Checked);
			configIni.Write("UseVideoLongerSide", UseVideoLongerSideCheck.Checked);
			configIni.EndSection();
		}

		private void Combo_SelectedIndexChanged(object sender, EventArgs e) {
			if (!(sender is ComboBox)) return;
			ComboBox combo = sender as ComboBox;
			int selected = combo.SelectedIndex;
			if (selected == 0 || selected == -1) return;
			foreach (Control control in table.Controls) {
				if (control is ComboBox) {
					ComboBox comboBox = control as ComboBox;
					if (comboBox == combo) continue;
					if (comboBox.SelectedIndex == selected) comboBox.SelectedIndex = 0;
				}
			}
		}

		private enum Side {
			FRONT, BACK, LEFT, RIGHT, TOP, BOTTOM
		}
		private static string[] SideName;
		private readonly VideoTrack[] origTracks = new VideoTrack[6];
		private readonly VideoTrack[] newTracks = new VideoTrack[6];
		private VideoTrack parentTrack;
		private int parentNestLevel = NONE_TRACKS_USED;
		private int childNestLevel { get { return parentNestLevel + 1; } }
		private int firstTrackIndex = NONE_TRACKS_USED;
		private const int NONE_TRACKS_USED = -1;

		/// <summary>
		/// 移动轨道。
		/// 由于功能限制，只能移动轨道剪辑，不能移动轨道运动和轨道效果等信息。
		/// </summary>
		/// <param name="orig">原轨道</param>
		/// <param name="side">是立方体的哪一边</param>
		/// <param name="index">移动到哪个序号</param>
		/// <returns></returns>
		private VideoTrack MoveTrack(VideoTrack orig, Side side, int index = 1) {
			if (orig == null) return null;
			string name = orig.Name;
			string extendedName = SideName[(int)side];
			if (!string.IsNullOrWhiteSpace(name)) extendedName += "：" + name;
			VideoTrack track = new VideoTrack(vegas.Project, index, extendedName);
			vegas.Project.Tracks.Add(track);
			while (orig.Events.Count != 0) orig.Events[0].Track = track;
			track.CompositeNestingLevel = childNestLevel;
			track.CompositeMode = CompositeMode.SrcAlpha3D;
			TrackMotionKeyframe frame = track.TrackMotion.MotionKeyframes[0];
			double trans = (
				!UseVideoLongerSideCheck.Checked ?
				Math.Min(vegas.Project.Video.Height, vegas.Project.Video.Width) :
				Math.Max(vegas.Project.Video.Height, vegas.Project.Video.Width)
			) / 2;
			switch (side) {
				case Side.FRONT:
					frame.PositionZ = -trans;
					break;
				case Side.BACK:
					frame.PositionZ = trans;
					frame.OrientationY = 180;
					break;
				case Side.LEFT:
					frame.PositionX = -trans;
					frame.OrientationY = 90;
					break;
				case Side.RIGHT:
					frame.PositionX = trans;
					frame.OrientationY = -90;
					break;
				case Side.TOP:
					frame.PositionY = trans;
					frame.OrientationX = -90;
					break;
				case Side.BOTTOM:
					frame.PositionY = trans;
					frame.OrientationX = -90;
					break;
				default:
					break;
			}

			if (DelOrigTrackCheck.Checked) vegas.Project.Tracks.Remove(orig);
			return track;
		}

		public void Arrange() {
			for (int i = 0; i < origTracks.Length; i++) {
				VideoTrack track = origTracks[i] = tracks[combos[i].SelectedIndex];
				if (track != null && parentNestLevel == NONE_TRACKS_USED) {
					parentNestLevel = track.CompositeNestingLevel;
					firstTrackIndex = track.Index;
				}
			}
			if (parentNestLevel == NONE_TRACKS_USED) return;
			vegas.Project.Tracks.Add(parentTrack = new VideoTrack(vegas.Project, firstTrackIndex++, ""));
			parentTrack.CompositeNestingLevel = parentNestLevel;
			for (int i = newTracks.Length - 1; i >= 0; i--)
				newTracks[i] = MoveTrack(origTracks[i], (Side)i, firstTrackIndex);
			parentTrack.CompositeMode = CompositeMode.SrcAlpha3D;
			parentTrack.ParentCompositeMode = CompositeMode.SrcAlpha3D;
			TrackMotionKeyframe example = parentTrack.ParentTrackMotion.MotionKeyframes[0];
			example.Width = example.Height = example.Depth = vegas.Project.Video.Height / 2.0;
			example.OrientationX = 35;
			example.OrientationY = 30;
			example.OrientationZ = -20;
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			OkBtn.Text = str.complete;
			CancelBtn.Text = str.cancel;
			BottomLbl.Text = str.bottom_surface;
			TopLbl.Text = str.top_surface;
			RightLbl.Text = str.right_surface;
			LeftLbl.Text = str.left_surface;
			BackLbl.Text = str.back_surface;
			FrontLbl.Text = str.front_surface;
			InfoLbl.Text = str.box_3d_layout_info;
			DelOrigTrackCheck.Text =str.delete_original_tracks;
			UseVideoLongerSideCheck.Text = str.use_video_longer_side;
			Text = str.auto_layout_tracks + " - " + str.box_3d_layout;
		}

		private void ReadFromInfo() {
			throw new NotImplementedException();
		}
	}

	partial class GradientTracksForm {
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
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLbl = new System.Windows.Forms.Label();
			this.EffectsCombo = new System.Windows.Forms.ComboBox();
			this.ReverseCheck = new System.Windows.Forms.CheckBox();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table.SuspendLayout();
			this.dock.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(397, 10);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(499, 10);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.EffectsCombo, 0, 1);
			this.table.Controls.Add(this.ReverseCheck, 0, 2);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(12, 14, 12, 14);
			this.table.RowCount = 3;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.table.Size = new System.Drawing.Size(605, 132);
			this.table.TabIndex = 14;
			// 
			// InfoLbl
			// 
			this.InfoLbl.AutoSize = true;
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Location = new System.Drawing.Point(12, 22);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(0, 8, 0, 4);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(581, 20);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "选择一种渐变效果应用到所选的视频轨道：";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// EffectsCombo
			// 
			this.EffectsCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EffectsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EffectsCombo.FormattingEnabled = true;
			this.EffectsCombo.Items.AddRange(new object[] {
			"彩虹色",
			"逐渐饱和",
			"逐渐对比",
			"阈值",
			"彩灰交替",
			"正负交替"});
			this.EffectsCombo.Location = new System.Drawing.Point(16, 50);
			this.EffectsCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.EffectsCombo.Name = "EffectsCombo";
			this.EffectsCombo.Size = new System.Drawing.Size(573, 28);
			this.EffectsCombo.TabIndex = 1;
			// 
			// ReverseCheck
			// 
			this.ReverseCheck.AutoSize = true;
			this.ReverseCheck.Location = new System.Drawing.Point(16, 90);
			this.ReverseCheck.Margin = new System.Windows.Forms.Padding(4, 8, 4, 4);
			this.ReverseCheck.Name = "ReverseCheck";
			this.ReverseCheck.Size = new System.Drawing.Size(91, 24);
			this.ReverseCheck.TabIndex = 2;
			this.ReverseCheck.Text = "降序排序";
			this.ReverseCheck.UseVisualStyleBackColor = true;
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
			this.dock.Location = new System.Drawing.Point(0, 137);
			this.dock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(605, 52);
			this.dock.TabIndex = 13;
			// 
			// GradientTracksForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(605, 189);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GradientTracksForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "渐变轨道";
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.dock.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel table;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.ComboBox EffectsCombo;
		private System.Windows.Forms.CheckBox ReverseCheck;
	}

	public partial class GradientTracksForm : Form, IAutoLayoutTracks, IInterpret {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		private AutoLayoutTracksInfos.GradientTracksInfo info;
		private readonly VideoTrack[] tracks;
		public bool IsToolMode { get { return info == null; } }
		public GradientTracksForm(EntryPoint e) : this(e, null, null) { }
		public GradientTracksForm(EntryPoint entryPoint, AutoLayoutTracksInfos.GradientTracksInfo info = null, VideoTrack[] tracks = null) {
			InitializeComponent();
			parent = entryPoint;
			this.info = info;
			Icon = ConfigForm.icon;
			Translate();
			EffectsCombo.SelectedIndex = 0;
			ReverseCheck.Checked = true;
			ReadFromInfo();
			this.tracks = tracks ?? parent.GetSelectedVideoTracks();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			if (IsToolMode) Arrange();
			else {
				info.enabled = true;
				info.descending = ReverseCheck.Checked;
				info.effect = (VideoTrackGradientEffectType)EffectsCombo.SelectedIndex;
			}
			Close();
		}

		private void ReadFromInfo(AutoLayoutTracksInfos.GradientTracksInfo _info = null) {
			if (_info != null) info = _info;
			if (IsToolMode || !info.enabled) return;

			EffectsCombo.SetIndex((int)info.effect, 0);
			ReverseCheck.Checked = info.descending;
			if (_info != null) ReverseCheck.Checked = !ReverseCheck.Checked;

			if (_info != null) info = null;
		}

		public void Arrange() {
			Plugin.Init(vegas);
			VideoTrackGradientEffectType effect = (VideoTrackGradientEffectType)EffectsCombo.SelectedIndex;
			bool isReversed = ReverseCheck.Checked;
			switch (effect) {
				case VideoTrackGradientEffectType.RAINBOW:
					if (Plugin.hslAdjust == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.hsl_adjust)); return; }
					Plugin.ForVideoTracks.Rainbow(tracks, isReversed);
					break;
				case VideoTrackGradientEffectType.GRADUALLY_SATURATED:
					if (Plugin.hslAdjust == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.hsl_adjust)); return; }
					Plugin.ForVideoTracks.Saturated(tracks, isReversed);
					break;
				case VideoTrackGradientEffectType.GRADUALLY_CONTRASTED:
					if (Plugin.contrast == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.brightness_and_contrast)); return; }
					Plugin.ForVideoTracks.Contrasted(tracks, isReversed);
					break;
				case VideoTrackGradientEffectType.THRESHOLD:
					if (Plugin.contrast == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.brightness_and_contrast)); return; }
					Plugin.ForVideoTracks.Threshold(tracks, isReversed);
					break;
				case VideoTrackGradientEffectType.ALTERNATELY_CHROMATIC:
					if (Plugin.blackAndWhite == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.black_and_white)); return; }
					Plugin.ForVideoTracks.Chromatic(tracks, isReversed);
					break;
				case VideoTrackGradientEffectType.ALTERNATELY_NEGATIVE:
					if (Plugin.invert == null) { EntryPoint.ShowError(new Exceptions.NoPluginNameException(Lang.str.invert)); return; }
					Plugin.ForVideoTracks.Negative(tracks, isReversed);
					break;
				default:
					break;
			}
		}

		public static void Arrange(VideoTrack[] tracks, AutoLayoutTracksInfos.GradientTracksInfo info, EntryPoint entryPoint) {
			if (!info.enabled) return;
			GradientTracksForm me = new GradientTracksForm(entryPoint, null, tracks);
			me.ReadFromInfo(info);
			me.Arrange();
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			OkBtn.Text = str.complete;
			CancelBtn.Text = str.cancel;
			InfoLbl.Text = str.gradient_tracks_info;
			EffectsCombo.Items.Clear();
			EffectsCombo.Items.AddRange(new string[] {
				str.rainbow_color, str.gradually_saturated, str.gradually_contrasted,
				str.threshold, str.alternately_chromatic, str.alternately_negative
			});
			ReverseCheck.Text = IsToolMode ? str.descending : str.ascending;
			Text = str.gradient_tracks;
		}
	}

	partial class ChangeTuneMethodForm {
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
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.InfoLbl = new System.Windows.Forms.Label();
			this.TimeStretchPitchShiftGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.FormantLockCheck = new System.Windows.Forms.CheckBox();
			this.StretchAttrCombo = new System.Windows.Forms.ComboBox();
			this.FormantChangeLbl = new System.Windows.Forms.Label();
			this.PitchChangeLbl = new System.Windows.Forms.Label();
			this.StretchAttrLbl = new System.Windows.Forms.Label();
			this.MethodLbl = new System.Windows.Forms.Label();
			this.MethodCombo = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LockPitchInsteadOfRateCheck = new System.Windows.Forms.CheckBox();
			this.PitchLockCheck = new System.Windows.Forms.CheckBox();
			this.dock.SuspendLayout();
			this.table.SuspendLayout();
			this.TimeStretchPitchShiftGroup.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(418, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(499, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
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
			this.dock.Location = new System.Drawing.Point(0, 206);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(583, 42);
			this.dock.TabIndex = 15;
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.InfoLbl, 0, 0);
			this.table.Controls.Add(this.TimeStretchPitchShiftGroup, 0, 1);
			this.table.Dock = System.Windows.Forms.DockStyle.Top;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
			this.table.RowCount = 2;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table.Size = new System.Drawing.Size(583, 189);
			this.table.TabIndex = 17;
			// 
			// InfoLbl
			// 
			this.InfoLbl.AutoSize = true;
			this.InfoLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.InfoLbl.Location = new System.Drawing.Point(10, 17);
			this.InfoLbl.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this.InfoLbl.MaximumSize = new System.Drawing.Size(520, 0);
			this.InfoLbl.Name = "InfoLbl";
			this.InfoLbl.Size = new System.Drawing.Size(520, 15);
			this.InfoLbl.TabIndex = 0;
			this.InfoLbl.Text = "仅支持音频事件属性中的调音方法，不支持“移调”插件中的调音方法。";
			this.InfoLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// TimeStretchPitchShiftGroup
			// 
			this.TimeStretchPitchShiftGroup.AutoSize = true;
			this.TimeStretchPitchShiftGroup.Controls.Add(this.tableLayoutPanel2);
			this.TimeStretchPitchShiftGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TimeStretchPitchShiftGroup.Location = new System.Drawing.Point(13, 41);
			this.TimeStretchPitchShiftGroup.Name = "TimeStretchPitchShiftGroup";
			this.TimeStretchPitchShiftGroup.Padding = new System.Windows.Forms.Padding(5);
			this.TimeStretchPitchShiftGroup.Size = new System.Drawing.Size(557, 134);
			this.TimeStretchPitchShiftGroup.TabIndex = 1;
			this.TimeStretchPitchShiftGroup.TabStop = false;
			this.TimeStretchPitchShiftGroup.Text = "时间拉伸/音调转换";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.FormantLockCheck, 1, 3);
			this.tableLayoutPanel2.Controls.Add(this.StretchAttrCombo, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.FormantChangeLbl, 0, 3);
			this.tableLayoutPanel2.Controls.Add(this.PitchChangeLbl, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.StretchAttrLbl, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.MethodLbl, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.MethodCombo, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 1, 2);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 21);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(547, 108);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// FormantLockCheck
			// 
			this.FormantLockCheck.AutoSize = true;
			this.FormantLockCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FormantLockCheck.Location = new System.Drawing.Point(76, 86);
			this.FormantLockCheck.Name = "FormantLockCheck";
			this.FormantLockCheck.Size = new System.Drawing.Size(468, 19);
			this.FormantLockCheck.TabIndex = 7;
			this.FormantLockCheck.Text = "保持共振峰";
			this.FormantLockCheck.UseVisualStyleBackColor = true;
			// 
			// StretchAttrCombo
			// 
			this.StretchAttrCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StretchAttrCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.StretchAttrCombo.FormattingEnabled = true;
			this.StretchAttrCombo.Location = new System.Drawing.Point(76, 32);
			this.StretchAttrCombo.Name = "StretchAttrCombo";
			this.StretchAttrCombo.Size = new System.Drawing.Size(468, 23);
			this.StretchAttrCombo.TabIndex = 5;
			this.StretchAttrCombo.SelectedIndexChanged += new System.EventHandler(this.MethodCombo_SelectedIndexChanged);
			// 
			// FormantChangeLbl
			// 
			this.FormantChangeLbl.AutoSize = true;
			this.FormantChangeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FormantChangeLbl.Location = new System.Drawing.Point(3, 83);
			this.FormantChangeLbl.Name = "FormantChangeLbl";
			this.FormantChangeLbl.Size = new System.Drawing.Size(67, 25);
			this.FormantChangeLbl.TabIndex = 3;
			this.FormantChangeLbl.Text = "共振峰移位";
			this.FormantChangeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PitchChangeLbl
			// 
			this.PitchChangeLbl.AutoSize = true;
			this.PitchChangeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PitchChangeLbl.Location = new System.Drawing.Point(3, 58);
			this.PitchChangeLbl.Name = "PitchChangeLbl";
			this.PitchChangeLbl.Size = new System.Drawing.Size(67, 25);
			this.PitchChangeLbl.TabIndex = 2;
			this.PitchChangeLbl.Text = "音调更改";
			this.PitchChangeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StretchAttrLbl
			// 
			this.StretchAttrLbl.AutoSize = true;
			this.StretchAttrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StretchAttrLbl.Location = new System.Drawing.Point(3, 29);
			this.StretchAttrLbl.Name = "StretchAttrLbl";
			this.StretchAttrLbl.Size = new System.Drawing.Size(67, 29);
			this.StretchAttrLbl.TabIndex = 1;
			this.StretchAttrLbl.Text = "拉伸属性";
			this.StretchAttrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MethodLbl
			// 
			this.MethodLbl.AutoSize = true;
			this.MethodLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MethodLbl.Location = new System.Drawing.Point(3, 0);
			this.MethodLbl.Name = "MethodLbl";
			this.MethodLbl.Size = new System.Drawing.Size(67, 29);
			this.MethodLbl.TabIndex = 0;
			this.MethodLbl.Text = "方法";
			this.MethodLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MethodCombo
			// 
			this.MethodCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MethodCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MethodCombo.FormattingEnabled = true;
			this.MethodCombo.Items.AddRange(new object[] {
			"无",
			"élastique",
			"古典"});
			this.MethodCombo.Location = new System.Drawing.Point(76, 3);
			this.MethodCombo.Name = "MethodCombo";
			this.MethodCombo.Size = new System.Drawing.Size(468, 23);
			this.MethodCombo.TabIndex = 4;
			this.MethodCombo.SelectedIndexChanged += new System.EventHandler(this.MethodCombo_SelectedIndexChanged);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.LockPitchInsteadOfRateCheck, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.PitchLockCheck, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(73, 58);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(474, 25);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// LockPitchInsteadOfRateCheck
			// 
			this.LockPitchInsteadOfRateCheck.AutoSize = true;
			this.LockPitchInsteadOfRateCheck.Checked = true;
			this.LockPitchInsteadOfRateCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.LockPitchInsteadOfRateCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LockPitchInsteadOfRateCheck.Location = new System.Drawing.Point(95, 3);
			this.LockPitchInsteadOfRateCheck.Name = "LockPitchInsteadOfRateCheck";
			this.LockPitchInsteadOfRateCheck.Size = new System.Drawing.Size(134, 19);
			this.LockPitchInsteadOfRateCheck.TabIndex = 8;
			this.LockPitchInsteadOfRateCheck.Text = "锁定音高而不是速度";
			this.LockPitchInsteadOfRateCheck.UseVisualStyleBackColor = true;
			// 
			// PitchLockCheck
			// 
			this.PitchLockCheck.AutoSize = true;
			this.PitchLockCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PitchLockCheck.Location = new System.Drawing.Point(3, 3);
			this.PitchLockCheck.Name = "PitchLockCheck";
			this.PitchLockCheck.Size = new System.Drawing.Size(86, 19);
			this.PitchLockCheck.TabIndex = 7;
			this.PitchLockCheck.Text = "锁定以拉伸";
			this.PitchLockCheck.UseVisualStyleBackColor = true;
			// 
			// ChangeTuneMethodForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(583, 248);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangeTuneMethodForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "更改调音算法";
			this.dock.ResumeLayout(false);
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.TimeStretchPitchShiftGroup.ResumeLayout(false);
			this.TimeStretchPitchShiftGroup.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.TableLayoutPanel dock;
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label InfoLbl;
		private System.Windows.Forms.GroupBox TimeStretchPitchShiftGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox FormantLockCheck;
		private System.Windows.Forms.ComboBox StretchAttrCombo;
		private System.Windows.Forms.Label FormantChangeLbl;
		private System.Windows.Forms.Label PitchChangeLbl;
		private System.Windows.Forms.Label StretchAttrLbl;
		private System.Windows.Forms.Label MethodLbl;
		private System.Windows.Forms.ComboBox MethodCombo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox LockPitchInsteadOfRateCheck;
		private System.Windows.Forms.CheckBox PitchLockCheck;
	}

	public partial class ChangeTuneMethodForm : Form, IInterpret {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		public ChangeTuneMethodForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			Translate();
			Height = table.Height + dock.Height + SelectIntervalForm.MARGIN;
			MethodCombo.SelectedIndex = 1;
			MethodCombo_SelectedIndexChanged(null, null);
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
			#if VER_GEQ_16 // 不重要了，低于 16 的版本整个功能都不能用了，加上仅保证不会报错。
			TimeStretchPitchShift method = GetMethod(MethodCombo.SelectedIndex);
			bool isLockPitch = PitchLockCheck.Checked;
			bool isLockFormant = FormantLockCheck.Checked;
			ElastiqueStretchAttributes elastique = new ElastiqueStretchAttributes();
			ClassicStretchAttributes classic = new ClassicStretchAttributes();
			if (method == TimeStretchPitchShift.Elastique) elastique = (ElastiqueStretchAttributes)StretchAttrCombo.SelectedIndex;
			if (method == TimeStretchPitchShift.Classic) classic = (ClassicStretchAttributes)StretchAttrCombo.SelectedIndex;
			AudioEvent[] audioEvents = parent.GetSelectedAudioEvents();
			foreach (AudioEvent audioEvent in audioEvents) {
				audioEvent.Method = method;
				if (method == TimeStretchPitchShift.None) continue;
					if (method == TimeStretchPitchShift.Elastique) audioEvent.ElastiqueAttribute = elastique;
					if (method == TimeStretchPitchShift.Classic) audioEvent.ClassicAttribute = classic;
					if (method == TimeStretchPitchShift.Elastique && elastique == ElastiqueStretchAttributes.Pro) audioEvent.FormantLock = isLockFormant;
				if (audioEvent.PitchLock != isLockPitch) {
					if (LockPitchInsteadOfRateCheck.Checked) {
						if (isLockPitch) {
							double originalPitch = audioEvent.PitchSemis;
							audioEvent.PitchLock = isLockPitch;
							audioEvent.AdjustPlaybackRate(EntryPoint.Pitch2Stretch(originalPitch), true);
						} else {
							double originalRate = audioEvent.PlaybackRate;
							audioEvent.PitchLock = isLockPitch;
							audioEvent.PitchSemis = EntryPoint.Stretch2Pitch(originalRate);
						}
					} else audioEvent.PitchLock = isLockPitch;
				}
			}
			#endif
		}

		private TimeStretchPitchShift GetMethod(int selectedIndex) {
			return selectedIndex == 1 ? TimeStretchPitchShift.Elastique :
				selectedIndex == 2 ? TimeStretchPitchShift.Classic :
				TimeStretchPitchShift.None;
		}

		private void MethodCombo_SelectedIndexChanged(object sender, EventArgs e) {
			#if VER_GEQ_16
			TimeStretchPitchShift method = GetMethod(MethodCombo.SelectedIndex);
			LockPitchInsteadOfRateCheck.Enabled = StretchAttrCombo.Enabled = PitchLockCheck.Enabled = method != TimeStretchPitchShift.None;
			FormantLockCheck.Enabled = method == TimeStretchPitchShift.Elastique && StretchAttrCombo.SelectedIndex == 0;
			if (method != lastMethod) {
				lastMethod = method;
				StretchAttrCombo.Items.Clear();
				if (method == TimeStretchPitchShift.Elastique) {
					StretchAttrCombo.Items.AddRange(ElastiqueAttrArray);
					StretchAttrCombo.SelectedIndex = 1;
				} else if (method == TimeStretchPitchShift.Classic) {
					StretchAttrCombo.Items.AddRange(ClassicAttrArray);
					StretchAttrCombo.SelectedIndex = 0;
				}
			}
				if (method == TimeStretchPitchShift.Elastique) {
					ElastiqueStretchAttributes attr = (ElastiqueStretchAttributes)StretchAttrCombo.SelectedIndex;
					if (attr == ElastiqueStretchAttributes.Efficient || attr == ElastiqueStretchAttributes.Soloist_Speech) FormantLockCheck.Checked = false;
					else if (attr == ElastiqueStretchAttributes.Soloist_Monophonic) FormantLockCheck.Checked = true;
				}
			if (PitchLockCheck.Checked) StretchAttrCombo.Enabled = FormantLockCheck.Enabled = false;
			#endif
		}

		public static string[] ElastiqueAttrArray {
			get {
				Lang str = Lang.str;
				return new string[] {
					str.elastique_pro, str.elastique_efficient, str.elastique_soloist_monophonic, str.elastique_soloist_speech
				};
			}
		}
		public static string[] ClassicAttrArray {
			get {
				Lang str = Lang.str;
				return new string[] {
					str.classic_a01, str.classic_a02, str.classic_a03, str.classic_a04, str.classic_a05, str.classic_a06,
					str.classic_a07, str.classic_a08, str.classic_a09, str.classic_a10, str.classic_a11, str.classic_a12,
					str.classic_a13, str.classic_a14, str.classic_a15, str.classic_a16, str.classic_a17, str.classic_a18,
					str.classic_a19,
				};
			}
		}
		private TimeStretchPitchShift? lastMethod = null;

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			InfoLbl.Font = new Font(str.info_label_font, 9F);
			OkBtn.Text = str.complete;
			CancelBtn.Text = str.cancel;
			InfoLbl.Text = str.change_tune_method_info;
			TimeStretchPitchShiftGroup.Text = str.time_stretch_pitch_shift;
			FormantLockCheck.Text = str.reserve_formant;
			FormantChangeLbl.Text = str.formant_change;
			PitchChangeLbl.Text = str.pitch_change;
			StretchAttrLbl.Text = str.stretch_attr;
			MethodLbl.Text = str.method;
			MethodCombo.Items.Clear();
			MethodCombo.Items.AddRange(new string[] {
				str.none, str.elastique, str.classic
			});
			PitchLockCheck.Text = str.pitch_lock;
			LockPitchInsteadOfRateCheck.Text = str.lock_pitch_instead_of_rate;
			Text = str.change_tune_method;
		}
		
		#if !VER_GEQ_16
			public enum TimeStretchPitchShift { // 兼容使用，不起任何效果。
				Classic,
				AcidStyle,
				Elastique,
				None,
			}
		#endif
	}

	partial class BatchSubtitleGenerationForm {
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
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.table = new System.Windows.Forms.TableLayoutPanel();
			this.PresetsLbl = new System.Windows.Forms.Label();
			this.PresetsCombo = new System.Windows.Forms.ComboBox();
			this.SubtitlesLbl = new System.Windows.Forms.Label();
			this.SubtitlesTxt = new System.Windows.Forms.TextBox();
			this.SuggestionInfo = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.SingleDurationLbl = new System.Windows.Forms.Label();
			this.SingleDurationTxt = new System.Windows.Forms.TextBox();
			this.dock = new System.Windows.Forms.TableLayoutPanel();
			this.table.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.dock.SuspendLayout();
			this.SuspendLayout();
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(479, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(560, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
			this.CancelBtn.TabIndex = 2;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// table
			// 
			this.table.AutoSize = true;
			this.table.BackColor = System.Drawing.SystemColors.Window;
			this.table.ColumnCount = 1;
			this.table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table.Controls.Add(this.PresetsLbl, 0, 0);
			this.table.Controls.Add(this.PresetsCombo, 0, 1);
			this.table.Controls.Add(this.SubtitlesLbl, 0, 2);
			this.table.Controls.Add(this.SubtitlesTxt, 0, 3);
			this.table.Controls.Add(this.SuggestionInfo, 0, 5);
			this.table.Controls.Add(this.tableLayoutPanel1, 0, 4);
			this.table.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table.Location = new System.Drawing.Point(0, 0);
			this.table.Margin = new System.Windows.Forms.Padding(4);
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(10, 11, 10, 11);
			this.table.RowCount = 6;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(644, 459);
			this.table.TabIndex = 19;
			// 
			// PresetsLbl
			// 
			this.PresetsLbl.AutoSize = true;
			this.PresetsLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PresetsLbl.Location = new System.Drawing.Point(13, 11);
			this.PresetsLbl.Name = "PresetsLbl";
			this.PresetsLbl.Size = new System.Drawing.Size(618, 15);
			this.PresetsLbl.TabIndex = 0;
			this.PresetsLbl.Text = "选择一个预先设定好的“字幕和文字”媒体发生器的预设：";
			this.PresetsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// PresetsCombo
			// 
			this.PresetsCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PresetsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PresetsCombo.FormattingEnabled = true;
			this.PresetsCombo.Location = new System.Drawing.Point(13, 29);
			this.PresetsCombo.Name = "PresetsCombo";
			this.PresetsCombo.Size = new System.Drawing.Size(618, 23);
			this.PresetsCombo.TabIndex = 1;
			// 
			// SubtitlesLbl
			// 
			this.SubtitlesLbl.AutoSize = true;
			this.SubtitlesLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SubtitlesLbl.Location = new System.Drawing.Point(13, 61);
			this.SubtitlesLbl.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
			this.SubtitlesLbl.Name = "SubtitlesLbl";
			this.SubtitlesLbl.Size = new System.Drawing.Size(618, 15);
			this.SubtitlesLbl.TabIndex = 2;
			this.SubtitlesLbl.Text = "输入要插入的字幕文本（一行一个，忽略空行）：";
			this.SubtitlesLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SubtitlesTxt
			// 
			this.SubtitlesTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SubtitlesTxt.Location = new System.Drawing.Point(13, 79);
			this.SubtitlesTxt.MaxLength = 65535;
			this.SubtitlesTxt.Multiline = true;
			this.SubtitlesTxt.Name = "SubtitlesTxt";
			this.SubtitlesTxt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.SubtitlesTxt.Size = new System.Drawing.Size(618, 316);
			this.SubtitlesTxt.TabIndex = 3;
			this.SubtitlesTxt.WordWrap = false;
			// 
			// SuggestionInfo
			// 
			this.SuggestionInfo.AutoSize = true;
			this.SuggestionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SuggestionInfo.Location = new System.Drawing.Point(13, 433);
			this.SuggestionInfo.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
			this.SuggestionInfo.Name = "SuggestionInfo";
			this.SuggestionInfo.Size = new System.Drawing.Size(618, 15);
			this.SuggestionInfo.TabIndex = 5;
			this.SuggestionInfo.Text = "稍后可开启“自动跟进”功能以便后续调整时间。";
			this.SuggestionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.SingleDurationLbl, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.SingleDurationTxt, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 398);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(624, 29);
			this.tableLayoutPanel1.TabIndex = 6;
			// 
			// SingleDurationLbl
			// 
			this.SingleDurationLbl.AutoSize = true;
			this.SingleDurationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SingleDurationLbl.Location = new System.Drawing.Point(3, 0);
			this.SingleDurationLbl.Name = "SingleDurationLbl";
			this.SingleDurationLbl.Size = new System.Drawing.Size(103, 29);
			this.SingleDurationLbl.TabIndex = 0;
			this.SingleDurationLbl.Text = "每个字幕持续时间";
			this.SingleDurationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SingleDurationTxt
			// 
			this.SingleDurationTxt.Dock = System.Windows.Forms.DockStyle.Left;
			this.SingleDurationTxt.Location = new System.Drawing.Point(112, 3);
			this.SingleDurationTxt.Name = "SingleDurationTxt";
			this.SingleDurationTxt.Size = new System.Drawing.Size(150, 23);
			this.SingleDurationTxt.TabIndex = 1;
			this.SingleDurationTxt.Leave += new System.EventHandler(this.SingleDurationTxt_Leave);
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
			this.dock.Location = new System.Drawing.Point(0, 459);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(644, 42);
			this.dock.TabIndex = 18;
			// 
			// BatchSubtitleGenerationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(644, 501);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(660, 540);
			this.Name = "BatchSubtitleGenerationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "批量生成字幕";
			this.table.ResumeLayout(false);
			this.table.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.dock.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TableLayoutPanel table;
		private System.Windows.Forms.Label PresetsLbl;
		private System.Windows.Forms.ComboBox PresetsCombo;
		private System.Windows.Forms.Label SubtitlesLbl;
		private System.Windows.Forms.TextBox SubtitlesTxt;
		private System.Windows.Forms.Label SuggestionInfo;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label SingleDurationLbl;
		private System.Windows.Forms.TextBox SingleDurationTxt;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel dock;
	}

	public partial class BatchSubtitleGenerationForm : Form, IInterpret, IConfigIniUser {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		private ConfigIni configIni { get { return parent.configIni; } }
		public BatchSubtitleGenerationForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			Translate();
			FormClosing += (sender, e) => SaveIni();
			Plugin.Init(vegas);
			PresetsCombo.Items.Clear();
			PresetsCombo.Items.AddRange(new List<EffectPreset>(Plugin.titlesAndText.Presets).ToArray());
			PresetsCombo.SelectedIndex = 0;
			ReadIni();
		}

		public void ReadIni() {
			configIni.StartSection("BatchSubtitleGeneration");
			SingleDurationTxt.Text = Timecode.FromMilliseconds(configIni.Read("SingleDuration", DEFAULT_SINGLE_DURATION_MS)).ToString();
			SingleDurationTxt_Leave(null, null);
			string preset = configIni.Read("Preset", "");
			foreach (EffectPreset curPreset in PresetsCombo.Items)
				if (curPreset.ToString() == preset)
					PresetsCombo.SelectedItem = curPreset;
			configIni.EndSection();
		}

		public void SaveIni() {
			configIni.StartSection("BatchSubtitleGeneration");
			configIni.Write("SingleDuration", SingleDuration.ToMilliseconds());
			configIni.Write("Preset", PresetsCombo.SelectedItem);
			configIni.EndSection();
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			PresetsLbl.Text = str.batch_subtitle_generation_presets;
			SubtitlesLbl.Text = str.batch_subtitle_generation_subtitles;
			SingleDurationLbl.Text = str.batch_subtitle_generation_single_duration;
			SuggestionInfo.Text = str.batch_subtitle_generation_suggestion_info;
			Text = str.batch_subtitle_generation;
			OkBtn.Text = str.ok;
			CancelBtn.Text = str.cancel;
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
			Generate();
		}

		private const double MIN_SINGLE_DURATION_MS = 10;
		private const double DEFAULT_SINGLE_DURATION_MS = 1000;
		private void SingleDurationTxt_Leave(object sender, EventArgs e) {
			SingleDurationTxt.Text = SingleDuration.ToString();
		}

		private static Timecode GetSingleDuration(string text) {
			Timecode timecode = Timecode.FromString(text);
			if (timecode.ToMilliseconds() <= 0)
				timecode = Timecode.FromMilliseconds(DEFAULT_SINGLE_DURATION_MS);
			else if (timecode.ToMilliseconds() < MIN_SINGLE_DURATION_MS)
				timecode = Timecode.FromMilliseconds(MIN_SINGLE_DURATION_MS);
			return timecode;
		}

		private Timecode SingleDuration { get { return GetSingleDuration(SingleDurationTxt.Text); } }

		public bool PresetsContains(string preset) {
			List<string> presetNames = new List<string>();
			foreach (EffectPreset _preset in Plugin.titlesAndText.Presets) presetNames.Add(_preset.ToString());
			return presetNames.Contains(preset);
		}

		private void Generate() {
			List<string> lines = new List<string>();
			foreach (string line in SubtitlesTxt.Lines) {
				if (string.IsNullOrWhiteSpace(line)) continue;
				lines.Add(line.Trim());
			}
			if (lines.Count == 0) return;
			VideoTrack track = vegas.Project.AddVideoTrack();
			Timecode duration = SingleDuration;
			foreach (string line in lines) {
				VideoEvent subtitle = track.AddVideoEvent(track.Length, duration);
				Media media = new Media(Plugin.titlesAndText) { Length = duration };
				subtitle.AddTake(media.GetVideoStreamByIndex(0));
				Effect gen = subtitle.ActiveTake.Media.Generator;
				gen.Preset = PresetsCombo.SelectedItem.ToString();
				OFXStringParameter textParam = gen.OFXEffect.FindParameterByName("Text") as OFXStringParameter;
				string templateRtf = textParam.Value;
				RichTextBox rtfBox = new RichTextBox { Rtf = templateRtf, Text = line };
				textParam.Value = rtfBox.Rtf;
			}
		}
	}

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
			this.MatchSourceAndOffsetRadio.Location = new System.Drawing.Point(12, 64);
			this.MatchSourceAndOffsetRadio.Name = "MatchSourceAndOffsetRadio";
			this.MatchSourceAndOffsetRadio.Size = new System.Drawing.Size(447, 19);
			this.MatchSourceAndOffsetRadio.TabIndex = 1;
			this.MatchSourceAndOffsetRadio.Text = "与选中轨道剪辑相同且开始偏移量相等的所有剪辑";
			this.MatchSourceAndOffsetRadio.UseVisualStyleBackColor = true;
			this.MatchSourceAndOffsetRadio.CheckedChanged += new System.EventHandler(this.SetEnabled);
			// 
			// MatchSourceRadio
			// 
			this.MatchSourceRadio.AutoSize = true;
			this.MatchSourceRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MatchSourceRadio.Location = new System.Drawing.Point(12, 39);
			this.MatchSourceRadio.Name = "MatchSourceRadio";
			this.MatchSourceRadio.Size = new System.Drawing.Size(447, 19);
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
			this.table.Name = "table";
			this.table.Padding = new System.Windows.Forms.Padding(9);
			this.table.RowCount = 7;
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table.Size = new System.Drawing.Size(471, 497);
			this.table.TabIndex = 11;
			// 
			// MatchNameRadio
			// 
			this.MatchNameRadio.AutoSize = true;
			this.MatchNameRadio.Checked = true;
			this.MatchNameRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MatchNameRadio.Location = new System.Drawing.Point(12, 89);
			this.MatchNameRadio.Name = "MatchNameRadio";
			this.MatchNameRadio.Size = new System.Drawing.Size(447, 19);
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
			this.SelectInfo.Location = new System.Drawing.Point(9, 15);
			this.SelectInfo.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
			this.SelectInfo.Name = "SelectInfo";
			this.SelectInfo.Size = new System.Drawing.Size(453, 15);
			this.SelectInfo.TabIndex = 3;
			this.SelectInfo.Text = "选中的第一个轨道剪辑：无";
			this.SelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// ClipNameTxt
			// 
			this.ClipNameTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClipNameTxt.Location = new System.Drawing.Point(12, 117);
			this.ClipNameTxt.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
			this.ClipNameTxt.Name = "ClipNameTxt";
			this.ClipNameTxt.Size = new System.Drawing.Size(447, 23);
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
			this.ClipNameList.Location = new System.Drawing.Point(12, 146);
			this.ClipNameList.Name = "ClipNameList";
			this.ClipNameList.Size = new System.Drawing.Size(447, 318);
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
			this.FindClipsInfo.Location = new System.Drawing.Point(9, 470);
			this.FindClipsInfo.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.FindClipsInfo.Name = "FindClipsInfo";
			this.FindClipsInfo.Size = new System.Drawing.Size(453, 15);
			this.FindClipsInfo.TabIndex = 6;
			this.FindClipsInfo.Text = "在上方选中相匹配的剪辑，确定之后将会选中这些剪辑。";
			this.FindClipsInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// OkBtn
			// 
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(306, 8);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(75, 26);
			this.OkBtn.TabIndex = 1;
			this.OkBtn.Text = "确定(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(387, 8);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(75, 26);
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
			this.dock.Location = new System.Drawing.Point(0, 497);
			this.dock.Margin = new System.Windows.Forms.Padding(4);
			this.dock.Name = "dock";
			this.dock.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
			this.dock.RowCount = 1;
			this.dock.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.dock.Size = new System.Drawing.Size(471, 42);
			this.dock.TabIndex = 10;
			// 
			// FindClipsForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(471, 539);
			this.Controls.Add(this.table);
			this.Controls.Add(this.dock);
			this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.Location = new System.Drawing.Point(60, 60);
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(487, 578);
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

	public partial class FindClipsForm : Form, IConfigIniUser, IInterpret {
		private readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		private ConfigIni configIni { get { return parent.configIni; } }
		private readonly TrackEvent selectedFirstEvent = null;
		public FindClipsForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			Icon = ConfigForm.icon;
			Translate();
			ReadIni();
			selectedFirstEvent = parent.GetSelectedFirstEvents();
			if (selectedFirstEvent == null) {
				MatchSourceRadio.Enabled = MatchSourceAndOffsetRadio.Enabled = false;
				MatchNameRadio.Checked = true;
			} else SelectInfo.Text = Lang.str.find_clips_select_info + Lang.str.colon + selectedFirstEvent.ActiveTake.Name;
			SetEnabled(null, null);
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			SaveIni();
			ApplySelection();
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void ApplySelection() {
			parent.SelectNoneEvents();
			if (MatchMethod == MatchMethodType.MATCH_NAME)
				foreach (ListViewItem item in ClipNameList.SelectedItems)
					foreach (TrackEvent trackEvent in parent.GetEventsByTakeName(item.Text))
						trackEvent.Selected = true;
			else foreach (TrackEvent trackEvent in parent.GetAllEvents()) {
				if (trackEvent.ActiveTake.Media != selectedFirstEvent.ActiveTake.Media) continue;
				if (MatchMethod == MatchMethodType.MATCH_SOURCE_AND_OFFSET &&
					trackEvent.ActiveTake.Offset != selectedFirstEvent.ActiveTake.Offset) continue;
				trackEvent.Selected = true;
			}
		}

		private void SetEnabled(object sender, EventArgs e) {
			bool isMatchName = MatchNameRadio.Checked;
			ClipNameTxt.Enabled = ClipNameList.Enabled = isMatchName;
			ClipNameList.HideSelection = !ClipNameList.Enabled;
			OkBtn.Enabled = MatchMethod != MatchMethodType.NONE && (
				MatchMethod != MatchMethodType.MATCH_NAME && selectedFirstEvent != null ||
				MatchMethod == MatchMethodType.MATCH_NAME && ClipNameList.SelectedItems.Count > 0
			);
		}

		private void AutoSelectMatchName(object sender, EventArgs e) {
			MatchNameRadio.Checked = true;
			SetEnabled(null, null);
		}

		private void ClipNameTxt_TextChanged(object sender, EventArgs e) {
			ClipNameList.Items.Clear();
			string query = ClipNameTxt.Text.Trim().ToLower();
			if (string.IsNullOrWhiteSpace(query)) goto check;
			List<string> eventNames = new List<string>();
			foreach (TrackEvent trackEvent in parent.GetAllEvents()) {
				if (trackEvent.ActiveTake == null) continue;
				string eventName = trackEvent.ActiveTake.Name;
				if (!eventName.ToLower().Contains(query) || eventNames.Contains(eventName)) continue;
				eventNames.Add(eventName);
				ListViewItem item = new ListViewItem(new string[] { eventName, parent.GetEventsByTakeName(eventName).Length.ToString() });
				if (eventName == query) item.Selected = true;
				ClipNameList.Items.Add(item);
			}
		check:
			AutoSelectMatchName(null, null);
		}

		public void Translate() {
			Lang str = Lang.str;
			Font = new Font(str.ui_font, 9F);
			SelectInfo.Text = str.find_clips_select_info + str.colon + str.none;
			FindClipsInfo.Text = str.find_clips_info;
			nameHeader.Text = str.find_clips_header_name;
			numHeader.Text = str.find_clips_header_num;
			MatchSourceRadio.Text = str.find_clips_match_source;
			MatchSourceAndOffsetRadio.Text = str.find_clips_match_source_and_offset;
			MatchNameRadio.Text = str.find_clips_match_name;
			Text = str.find_clips;
			OkBtn.Text = str.ok;
			CancelBtn.Text = str.cancel;
		}

		private RadioButton[] MatchMethodRadios {
			get {
				return new RadioButton[] {
					MatchSourceRadio,
					MatchSourceAndOffsetRadio,
					MatchNameRadio,
				};
			}
		}

		private int MatchMethod_int {
			get {
				for (int i = 0; i < MatchMethodRadios.Length; i++) {
					RadioButton radio = MatchMethodRadios[i];
					if (radio.Checked) return i;
				}
				return -1;
			}
			set {
				if (value >= 0 && value < MatchMethodRadios.Length)
					MatchMethodRadios[value].Checked = true;
			}
		}

		private MatchMethodType MatchMethod {
			get { return (MatchMethodType)MatchMethod_int; }
			set { MatchMethod_int = (int)value; }
		}

		private enum MatchMethodType {
			NONE = -1,
			MATCH_SOURCE,
			MATCH_SOURCE_AND_OFFSET,
			MATCH_NAME,
		}

		public void SaveIni() {
			configIni.StartSection("FindClips");
			configIni.Write("MatchMethod", MatchMethod_int);
			configIni.EndSection();
		}

		public void ReadIni() {
			configIni.StartSection("FindClips");
			MatchMethod_int = configIni.Read("MatchMethod", 2);
			configIni.EndSection();
		}
	}

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
			this.EditNotesBtn = new System.Windows.Forms.Button();
			this.dock.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.AutoLayoutTracksGroup.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.AutoLayoutTracksButtons.SuspendLayout();
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
			this.tableLayoutPanel1.Controls.Add(this.EditNotesBtn, 0, 1);
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
			this.AutoLayoutTracksGroup.TabIndex = 16;
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
			this.ChannelListView.Size = new System.Drawing.Size(566, 263);
			this.ChannelListView.TabIndex = 13;
			this.ChannelListView.UseCompatibleStateImageBehavior = false;
			this.ChannelListView.View = System.Windows.Forms.View.Details;
			this.ChannelListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ChannelListView_ItemChecked);
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
			// EditNotesBtn
			// 
			this.EditNotesBtn.AutoSize = true;
			this.EditNotesBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.EditNotesBtn.Location = new System.Drawing.Point(8, 275);
			this.EditNotesBtn.Margin = new System.Windows.Forms.Padding(2);
			this.EditNotesBtn.Name = "EditNotesBtn";
			this.EditNotesBtn.Size = new System.Drawing.Size(201, 34);
			this.EditNotesBtn.TabIndex = 14;
			this.EditNotesBtn.Text = "编辑所选通道音符...";
			this.EditNotesBtn.UseVisualStyleBackColor = true;
			this.EditNotesBtn.Click += new System.EventHandler(this.EditNotesBtn_Click);
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
			this.ResumeLayout(false);

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel dock;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.ListView ChannelListView;
		private System.Windows.Forms.ColumnHeader ChannelHeader;
		private System.Windows.Forms.ColumnHeader NameHeader;
		private System.Windows.Forms.ColumnHeader InstrumentHeader;
		private System.Windows.Forms.ColumnHeader NoteCountHeader;
		private System.Windows.Forms.ColumnHeader BeginNoteHeader;
		private System.Windows.Forms.Button EditNotesBtn;
		private System.Windows.Forms.GroupBox AutoLayoutTracksGroup;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		public System.Windows.Forms.Button ResetAutoLayoutTracksBtn;
		public System.Windows.Forms.TableLayoutPanel AutoLayoutTracksButtons;
		public System.Windows.Forms.Button GradientTracksBtn;
		public System.Windows.Forms.Button AutoLayoutTracksBox3dBtn;
		public System.Windows.Forms.Button AutoLayoutTracksGridBtn;
		private System.Windows.Forms.Label MidiChannelAdvancedAutoLayoutTracksInfo;
	}

	public partial class MidiChannelAdvancedForm : Form, IInterpret {
		private readonly ConfigForm configForm;
		private EntryPoint entryPoint { get { return configForm.parent; } }
		private readonly MidiChannels channels;
		private readonly AutoLayoutTracksInfos layoutInfos;
		public MidiChannels Channels { get { return channels; } }
		public AutoLayoutTracksInfos LayoutInfos { get { return layoutInfos; } }
		public MidiChannelAdvancedForm(ConfigForm configForm, MidiChannels existChannel = null, AutoLayoutTracksInfos existLayoutInfos = null) {
			InitializeComponent();
			this.configForm = configForm;
			Icon = configForm.Icon;
			CheckNonEmpty(true);
			channels = existChannel == null ? new MidiChannels() : new MidiChannels(existChannel);
			layoutInfos = existLayoutInfos == null ? new AutoLayoutTracksInfos(channels) : AutoLayoutTracksInfos.CopyFrom(existLayoutInfos, channels);
			CheckAutoLayoutTracksButtonActived();
			foreach (object _midiChannel in configForm.MidiChannelCombo.Items)
				if (_midiChannel is MIDI.TrackInfo) {
					MIDI.TrackInfo midiChannel = _midiChannel as MIDI.TrackInfo;
					ListViewItem item = ChannelListView.Items.Add(new ListViewItem(new string[] {
						midiChannel.Index.ToString(), midiChannel.Name, midiChannel.Instrument,
						midiChannel.NotesCount.ToString(), midiChannel.BeginNote
					}));
					item.Tag = midiChannel;
					if (existChannel == null) {
						if (configForm.MidiChannelCombo.SelectedItem == _midiChannel) item.Checked = true;
					} else if (existChannel.Contains(midiChannel)) item.Checked = true;
				}
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CancelBtn_Click(object sender, EventArgs e) {
			Close();
		}

		private void CommingSoon() {
			MessageBox.Show("Comming soon in v5.x!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private bool CheckNonEmpty(bool isInitial = false) {
			bool result = EditNotesBtn.Enabled = OkBtn.Enabled =
				configForm.MidiChannelCombo.Items.Count != 0 && (isInitial || ChannelListView.CheckedItems.Count != 0);
			AutoLayoutTracksGroup.Enabled = result && ChannelListView.CheckedItems.Count >= 2;
			return result;
		}

		private void EditNotesBtn_Click(object sender, EventArgs e) {
			CommingSoon();
		}

		private void AutoLayoutTracksBox3dBtn_Click(object sender, EventArgs e) {
			CommingSoon();
		}

		private void AutoLayoutTracksGridBtn_Click(object sender, EventArgs e) {
			new AutoLayoutTracksGridForm(entryPoint, layoutInfos.Grid).ShowDialog();
			CheckAutoLayoutTracksButtonActived();
		}

		private void GradientTracksBtn_Click(object sender, EventArgs e) {
			new GradientTracksForm(entryPoint, layoutInfos.GradientTracks).ShowDialog();
			CheckAutoLayoutTracksButtonActived();
		}

		private void ResetAutoLayoutTracksBtn_Click(object sender, EventArgs e) {
			layoutInfos.Grid.enabled = false;
			layoutInfos.GradientTracks.enabled = false;
			CheckAutoLayoutTracksButtonActived();
		}

		private void CheckAutoLayoutTracksButtonActived() {
			SetButtonActive(AutoLayoutTracksGridBtn, layoutInfos.Grid.enabled);
			SetButtonActive(GradientTracksBtn, layoutInfos.GradientTracks.enabled);
		}

		private void ChannelListView_ItemChecked(object sender, ItemCheckedEventArgs e) {
			channels.Clear();
			channels.AddRange(ChannelListView.CheckedItems);
			CheckNonEmpty();
		}

		private void SetButtonActive(Button button, bool isActive) {
			Lang str = Lang.str;
			Dictionary<Button, string> dict = new Dictionary<Button, string> {
				{ AutoLayoutTracksGridBtn, str.grid_layout },
				{ AutoLayoutTracksBox3dBtn, str.box_3d_layout },
				{ GradientTracksBtn, str.gradient_tracks }
			};
			if (!dict.ContainsKey(button)) return;
			button.Text = (isActive ? "* " : "") + dict[button];
			button.ForeColor = isActive ? Color.Green : SystemColors.ControlText;
		}

		public void Translate() {

		}
	}
	public class MidiChannels : List<MIDI.TrackInfo> {
		public void Next() {
			if (Count != 0) {
				RemoveAt(0);
				currentChannel++;
			}
		}
		private int selectedChannelCount = 0;
		public int SelectedChannelCount { get { return selectedChannelCount; } }
		private int currentChannel = 0;
		public int CurrentChannel { get { return currentChannel; } }
		public void CompleteConfig() {
			selectedChannelCount = Count;
		}
		public double GetPercent(double currentTrackPercent) {
			double aChannelPercent = 100.0 / selectedChannelCount;
			double completedChannelPercent = currentChannel * aChannelPercent;
			double currentChannelPercent = currentTrackPercent / 100.0 * aChannelPercent;
			return completedChannelPercent + currentChannelPercent;
		}
		public MidiChannels() : base() { }
		public MidiChannels(IEnumerable<MIDI.TrackInfo> tracks) : base(tracks) { }
		public MidiChannels(ListView.CheckedListViewItemCollection items) : base() {
			AddRange(items);
		}
		public void AddRange(ListView.CheckedListViewItemCollection items) {
			foreach (ListViewItem item in items)
				if (item.Tag is MIDI.TrackInfo)
					Add(item.Tag as MIDI.TrackInfo);
		}

		public override string ToString() {
			List<string> list = new List<string>();
			foreach (MIDI.TrackInfo track in this) {
				StringBuilder text = new StringBuilder(track.Index.ToString());
				if (track.HasName) text.Append(" ").Append(track.Name);
				list.Add(text.ToString());
			}
			return string.Join(Lang.str.semicolon, list);
		}

		public bool IsMultiMidiChannel { get { return Count > 1; } }
		public bool IsNonEmpty { get { return Count > 0; } }

		// 用于解决新问题：直接将本对象添加进入下拉菜单会显示为“(集合)”而不是重写的 toString 方法，需使用下面这个属性解决。
		public string DisplayValue { get { return ToString(); } }
	}

	public class AutoLayoutTracksInfos {
		public readonly MidiChannels channels;
		public AutoLayoutTracksInfos(MidiChannels channels) {
			this.channels = channels;
			gradientTracks = new GradientTracksInfo(channels);
			grid = new GridInfo(channels);
		}
		public abstract class BaseAutoLayoutTracksInfo {
			public bool enabled = false;
			private readonly MidiChannels channels;
			public MidiChannels Channels { get { return channels; } }
			public BaseAutoLayoutTracksInfo(MidiChannels channels) {
				this.channels = channels;
			}
			public BaseAutoLayoutTracksInfo Clone() {
				return MemberwiseClone() as BaseAutoLayoutTracksInfo;
			}
		}
		public class GradientTracksInfo : BaseAutoLayoutTracksInfo {
			public GradientTracksInfo(MidiChannels channels) : base(channels) { }
			public VideoTrackGradientEffectType effect;
			public bool descending;
		}
		public class GridInfo : BaseAutoLayoutTracksInfo {
			public GridInfo(MidiChannels channels) : base(channels) { }
			public bool isSquare;
			public int columns;
			public bool isFill;
			public bool descending;
			public int padding;
		}
		private GradientTracksInfo gradientTracks;
		private GridInfo grid;
		public GradientTracksInfo GradientTracks { get { return gradientTracks; } }
		public GridInfo Grid { get { return grid; } }

		public static AutoLayoutTracksInfos CopyFrom(AutoLayoutTracksInfos existing, MidiChannels channels) {
			AutoLayoutTracksInfos infos = new AutoLayoutTracksInfos(channels) {
				gradientTracks = existing.gradientTracks.Clone() as GradientTracksInfo,
				grid = existing.grid.Clone() as GridInfo,
			};
			return infos;
		}
	}

	public class Windows10StyledContextMenuStripRenderer : ToolStripProfessionalRenderer {
		public Windows10StyledContextMenuStripRenderer() : base(new Windows10StyledContextMenuStripColorTable()) { }
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e) {
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle r = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
			r.Inflate(-2, -6);
			e.Graphics.DrawLines(Pens.Black, new Point[] {
				new Point(r.Left + r.Width / 8, r.Top),
				new Point(r.Right, r.Top + r.Height / 2),
				new Point(r.Left + r.Width / 8, r.Top + r.Height)
			});
		}

		protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e) {
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle r = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
			r.Inflate(-4, -6);
			e.Graphics.DrawLines(Pens.Black, new Point[] {
				new Point(r.Left, r.Bottom - r.Height / 2),
				new Point(r.Left + r.Width / 3, r.Bottom),
				new Point(r.Right, r.Top)
			});
		}
	}

	public class Windows10StyledContextMenuStripColorTable : ProfessionalColorTable {
		private static readonly Color MenuBarSelectedColor = Color.FromArgb(224, 224, 224);
		public override Color MenuItemBorder { get { return Color.WhiteSmoke; } }
		public override Color MenuItemSelected { get { return Color.WhiteSmoke; } }
		public override Color ToolStripDropDownBackground { get { return Color.White; } }
		public override Color ImageMarginGradientBegin { get { return Color.White; } }
		public override Color ImageMarginGradientMiddle { get { return Color.White; } }
		public override Color ImageMarginGradientEnd { get { return Color.White; } }
		public override Color MenuItemSelectedGradientBegin { get { return MenuBarSelectedColor; } }
		public override Color MenuItemSelectedGradientEnd { get { return MenuBarSelectedColor; } }
		public override Color MenuItemPressedGradientBegin { get { return Color.White; } }
		public override Color MenuItemPressedGradientEnd { get { return Color.White; } }
		public override Color MenuItemPressedGradientMiddle { get { return Color.White; } }
	}

	public class CommandLinkButton : Button {
		private bool _commandLink = false;
		private string _commandLinkNote = "";

		public CommandLinkButton() : base() {
			// 在基类上设置默认属性值以避免过时的警告
			base.FlatStyle = FlatStyle.System;
		}

		[Category("Appearance"), DefaultValue(false), Description("指定此按钮应使用命令链接样式。（仅适用于 Windows Vista 及更高版本。）")]
		public bool CommandLink {
			get {
				return _commandLink;
			}
			set {
				if (_commandLink != value) {
					_commandLink = value;
					UpdateCommandLink();
				}
			}
		}

		[Category("Appearance"), DefaultValue(""), Description("设置命令链接按钮的说明文字。（仅适用于 Windows Vista 及更高版本。）"),
			Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
		public string CommandLinkNote {
			get {
				return _commandLinkNote;
			}
			set {
				if (_commandLinkNote != value) {
					_commandLinkNote = value;
					UpdateCommandLink();
				}
			}
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), Obsolete("CommandLinkButton 控件不支持此属性。"), DefaultValue(typeof(FlatStyle), "System")]
		public new FlatStyle FlatStyle {
			// 将默认展开样式设置为“系统”，并隐藏此属性，
			// 因为如果不将其设置为“系统”，任何自定义属性都无法工作
			get {
				return base.FlatStyle;
			}
			set {
				base.FlatStyle = value;
			}
		}

		#region P/Invoke Stuff
		private const int BS_COMMANDLINK = 0xE;
		private const int BCM_SETNOTE = 0x1609;

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = false)]
		private extern static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

		internal void UpdateCommandLink() {
			RecreateHandle();
			SendMessage(Handle, BCM_SETNOTE, IntPtr.Zero, _commandLinkNote);
		}

		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				if (CommandLink) cp.Style |= BS_COMMANDLINK;
				return cp;
			}
		}
		#endregion
	}

	public class NumericUpDownWithUnit : NumericUpDown {

		#region| Fields |
		private string suffix = "";
		private string prefix = "";
		private bool enableDecimalPlaces = false;
		#endregion

		#region| Properties |
		[Description("后缀单位。"), Category("Appearance"), DefaultValue("")]
		public string Suffix {
			get {
				return suffix;
			}
			set {
				suffix = value;
				UpdateEditText();
			}
		}

		[Description("前缀单位。"), Category("Appearance"), DefaultValue("")]
		public string Prefix {
			get {
				return prefix;
			}
			set {
				prefix = value;
				UpdateEditText();
			}
		}

		/// </summary>
		/// 返回限制在最小值和最大值内的提供值。这与基类中的值完全相同（基类是私有的，因此我们不能直接使用它）。
		/// </summary>
		[Description("返回限制在最小值和最大值内的提供值。这与基类中的值完全相同（基类是私有的，因此我们不能直接使用它）。"), Category("Appearance")]
		public decimal Constrain {
			get {
				return Value;
			}
			set {
				if (value < Minimum) Value = Minimum;
				else if (value > Maximum) Value = Maximum;
				else Value = value;
			}
		}

		[Description("是否启用数字显示框中要显示的十进制位数。"), Category("Data"), DefaultValue(false)]
		public bool EnableDecimalPlaces {
			get {
				return enableDecimalPlaces;
			}
			set {
				enableDecimalPlaces = value;
				decimal _value = Value;
				Value = Minimum;
				Value = _value;
			}
		}
		#endregion

		#region| Methods |

		/// <summary>
		/// 更新 NumericUpDown 文本时调用的方法。
		/// </summary>
		protected override void UpdateEditText() {
			List<string> list = new List<string> { enableDecimalPlaces ? Value.ToString("F" + DecimalPlaces) : Regex.Replace(Value.ToString(), @"(?<=\..*)0+$", "") };
			if (!string.IsNullOrWhiteSpace(suffix)) list.Add(suffix);
			if (!string.IsNullOrWhiteSpace(prefix)) list.Insert(0, prefix);
			Text = string.Join(" ", list);
		}

		/// <summary>
		/// 在实际更新文本之前验证调用的方法。这与基类完全相同，但它将使用该类中的新 ParseEditText。
		/// </summary>
		protected override void ValidateEditText() {
			// 考虑到标签单位，查看编辑文本是否解析为有效的小数
			ParseEditText();
			ToFixed();
			UpdateEditText();
		}

		/// <summary>
		/// 将 NumericUpDown 控件中显示的文本转换为数值并对其求值。
		/// </summary>
		protected new void ParseEditText() {
			try {
				// 此方法与基本方法的唯一区别在于，文本直接替换为属性文本，而不是使用正则表达式。
				// 现在，我们知道文本框上可能只有我们提供的单位中的字符。因为 NumericUpDown 为我们处理来自用户的无效输入。
				// 这就是魔法发生的地方。此正则表达式将匹配单位中的所有字符（因此单位不能有数字）。
				// 您可以更改此正则表达式以满足您的需要。
				Regex regex = new Regex(string.Format(@"[^(?!{0}{1} )]+", suffix, prefix));
				Match match = regex.Match(Text);
				if (match.Success) {
					string text = match.Value;
					// VSWhidbey 173332: 在尝试设置Value属性之前，请验证用户没有以“-”开头字符串，
					// 因为“-”是用于表示负数的字符串的有效字符。
					if (!string.IsNullOrEmpty(text) && !(text.Length == 1 && text == "-")) {
						if (Hexadecimal) Constrain = Convert.ToDecimal(Convert.ToInt32(Text, 16));
						else Constrain = decimal.Parse(text, CultureInfo.CurrentCulture);
					}
				}
			} catch {
				// 保持原样
			} finally {
				UserEdit = false;
				ToFixed();
			}
		}

		private void ToFixed() {
			if (enableDecimalPlaces)
				Constrain = decimal.Round(Constrain, DecimalPlaces);
		}
		#endregion
	}

	public class GroupedRadioButton : RadioButton {
		public GroupedRadioButton() : base() {
			CheckedChanged += OnCheckedChanged;
		}

		private string _group = "";
		/// <summary>
		/// 指定单选按钮的分组名称。
		/// </summary>
		[Description("指定单选按钮的分组名称。"), Category("Behavior"), DefaultValue("")]
		public string Group {
			get { return _group; }
			set {
				_group = value;
				if (string.IsNullOrWhiteSpace(value)) _group = "";
			}
		}

		private Form Form { get { return FindForm(); } }

		private void OnCheckedChanged(object sender, EventArgs e) {
			if (isOnCheckedChanged) return;
			isOnCheckedChanged = true;
			if (string.IsNullOrWhiteSpace(Group)) goto End;
			if (Form == null) goto End;
			ForEachInGroup(Form, radio => {
				if (radio.Group == Group) radio.Checked = false;
				if (radio == this) Checked = true;
			});
		End:
			isOnCheckedChanged = false;
		}

		private static void ForEachInGroup(Control container, Action<GroupedRadioButton> action) {
			foreach (Control control in container.Controls) {
				if (control is GroupedRadioButton) {
					GroupedRadioButton radio = control as GroupedRadioButton;
					action(radio);
				}
				if (control.Controls.Count != 0) ForEachInGroup(control, action);
			}
		}

		private static bool isOnCheckedChanged = false;

		/// <summary>
		/// 单选按钮组。
		/// </summary>
		public class RadioButtonGroup {
			internal RadioButtonGroup(GroupedRadioButton currentRadio) {
				if (string.IsNullOrWhiteSpace(currentRadio.Group)) {
					_array = new GroupedRadioButton[0];
					return;
				}
				List<GroupedRadioButton> group = new List<GroupedRadioButton>();
				ForEachInGroup(currentRadio.Form, radio => {
					if (!string.IsNullOrWhiteSpace(radio.Group) && radio.Group == currentRadio.Group)
						group.Add(radio);
				});
				_array = group.ToArray();
			}
			/// <summary>
			/// 设定或读取该单选按钮组中所选中的单选按钮。如果没有勾选任何单选按钮，返回 null。
			/// </summary>
			public GroupedRadioButton Selected {
				get {
					foreach (GroupedRadioButton radio in this)
						if (radio.Checked)
							return radio;
					return null;
				}
				set {
					foreach (GroupedRadioButton radio in this)
						if (radio == value)
							radio.Checked = true;
				}
			}

			private readonly GroupedRadioButton[] _array;

			public int Count { get { return ((ICollection<GroupedRadioButton>)_array).Count; } }
			public bool IsReadOnly { get { return ((ICollection<GroupedRadioButton>)_array).IsReadOnly; } }
			public GroupedRadioButton this[int index] { get { return ((IList<GroupedRadioButton>)_array)[index]; } set { ((IList<GroupedRadioButton>)_array)[index] = value; } }
			public int IndexOf(GroupedRadioButton item) { return ((IList<GroupedRadioButton>)_array).IndexOf(item); }
			protected void Insert(int index, GroupedRadioButton item) { ((IList<GroupedRadioButton>)_array).Insert(index, item); }
			protected void RemoveAt(int index) { ((IList<GroupedRadioButton>)_array).RemoveAt(index); }
			protected void Add(GroupedRadioButton item) { ((ICollection<GroupedRadioButton>)_array).Add(item); }
			protected void Clear() { ((ICollection<GroupedRadioButton>)_array).Clear(); }
			public bool Contains(GroupedRadioButton item) { return ((ICollection<GroupedRadioButton>)_array).Contains(item); }
			protected void CopyTo(GroupedRadioButton[] array, int arrayIndex) { ((ICollection<GroupedRadioButton>)_array).CopyTo(array, arrayIndex); }
			protected bool Remove(GroupedRadioButton item) { return ((ICollection<GroupedRadioButton>)_array).Remove(item); }
			public IEnumerator<GroupedRadioButton> GetEnumerator() { return ((IEnumerable<GroupedRadioButton>)_array).GetEnumerator(); }
		}

		/// <summary>
		/// 获取单选按钮所在的单选按钮组。
		/// </summary>
		public RadioButtonGroup GetGroup() {
			return new RadioButtonGroup(this);
		}

		/// <summary>
		/// 获取单选按钮所在的单选按钮组。
		/// </summary>
		public RadioButtonGroup Related { get { return GetGroup(); } }
	}

	public class MessageBoxEx {
		//测试样例
		protected static void Test() {
			Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[] { "按钮一(&O)", "按钮二(&T)", "按钮三(&H)" });
		}
		/// <summary>
		/// 测试样例<br />
		/// <code>Show("提示消息", "提示标题", MessageBoxButtons.YesNoCancel, new string[] { "按钮一(＆O)", "按钮二(＆T)", "按钮三(＆H)" });</code>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="caption"></param>
		/// <param name="buttons"></param>
		/// <param name="buttonTitles"></param>
		/// <returns></returns>
		public static DialogResult Show(string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, string[] buttonTitles = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
			if (buttonTitles == null) return MessageBox.Show(text, caption, buttons, icon, defaultButton);
			DummyForm frm = new DummyForm(buttons, buttonTitles);
			frm.Show();
			frm.WatchForActivate = true;
			DialogResult result = MessageBox.Show(frm, text, caption, buttons, icon, defaultButton);
			frm.Close();
			return result;
		}

		class DummyForm : Form {
			IntPtr _handle;
			MessageBoxButtons _buttons;
			string[] _buttonTitles = null;

			bool _watchForActivate = false;

			public bool WatchForActivate {
				get { return _watchForActivate; }
				set { _watchForActivate = value; }
			}

			public DummyForm(MessageBoxButtons buttons, string[] buttonTitles) {
				_buttons = buttons;
				_buttonTitles = buttonTitles;

				//让自己在界面上看不到
				this.Text = "";
				this.StartPosition = FormStartPosition.Manual;
				this.Location = new Point(-32000, -32000);
				this.ShowInTaskbar = false;
			}

			protected override void OnShown(EventArgs e) {
				base.OnShown(e);
				//把自己藏起来，在任务列表里也看不到
				NativeWin32API.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, 659);
			}

			protected override void WndProc(ref System.Windows.Forms.Message m) {
				if (_watchForActivate && m.Msg == 0x0006) {
					_watchForActivate = false;
					_handle = m.LParam;
					CheckMsgbox();
				}
				base.WndProc(ref m);
			}

			private void CheckMsgbox() {
				if (_buttonTitles == null || _buttonTitles.Length == 0)
					return;

				//按钮标题的索引
				int buttonTitleIndex = 0;
				//获取子控件的句柄
				IntPtr h = NativeWin32API.GetWindow(_handle, OCCommon.Message.GW_CHILD);
				while (h != IntPtr.Zero) {
					//按顺序把按钮标题赋上
					if (NativeWin32API.GetWindowClassName(h).Equals("Button")) {
						if (_buttonTitles.Length > buttonTitleIndex) {
							NativeWin32API.SetWindowText(h, _buttonTitles[buttonTitleIndex]);
							buttonTitleIndex++;
						}
					}
					h = NativeWin32API.GetWindow(h, OCCommon.Message.GW_HWNDNEXT);
				}
			}


			private static class OCCommon {
				/// <summary>
				/// Messages
				/// </summary>
				public static class Message {
					public const int GW_CHILD = 5;
					public const int GW_HWNDNEXT = 2;
				}
			}

			/// <summary>
			/// Win32 API
			/// </summary>
			private static class NativeWin32API {
				[DllImport("user32.dll", CharSet = CharSet.Auto)]
				public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);
				[DllImport("user32.dll")]
				public static extern IntPtr GetWindow(IntPtr hWnd, long wCmd);
				[DllImport("user32.dll")]
				public static extern bool SetWindowText(IntPtr hWnd, string lpString);
				[DllImport("user32.dll")]
				public static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

				public static string GetWindowClassName(IntPtr handle) {
					StringBuilder sb = new StringBuilder(256);

					GetClassNameW(handle, sb, sb.Capacity); //得到窗口类名并保存在strClass中
					return sb.ToString();
				}
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
			this.UserHelpLink = new System.Windows.Forms.LinkLabel();
			this.AboutBtn = new System.Windows.Forms.Button();
			this.OkBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.StaffLineColorDialog = new System.Windows.Forms.ColorDialog();
			this.Balloon = new System.Windows.Forms.ToolTip(this.components);
			this.AudioTuneMethodCombo = new System.Windows.Forms.ComboBox();
			this.AudioLockStretchPitchCheck = new System.Windows.Forms.CheckBox();
			this.PreviewTuneAudioCheck = new System.Windows.Forms.CheckBox();
			this.StaffRelativeValueCheck = new System.Windows.Forms.CheckBox();
			this.StaffLegacyEngineCheck = new System.Windows.Forms.CheckBox();
			this.MidiStartSecondBox = new Otomad.VegasScript.OtomadHelper.V4.TimecodeBox();
			this.MidiEndSecondBox = new Otomad.VegasScript.OtomadHelper.V4.TimecodeBox();
			this.SourceStartTimeText = new Otomad.VegasScript.OtomadHelper.V4.TimecodeBox();
			this.SourceEndTimeText = new Otomad.VegasScript.OtomadHelper.V4.TimecodeBox();
			this.PreviewBeepDurationBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.StaffLineThicknessBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.StaffLineSpacingBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.StaffSurfacePositionBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.StaffSurfaceWidthBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.YtpMinLenBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.YtpMaxLenBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.PreviewBasePitchBtn = new System.Windows.Forms.Button();
			this.AudioStretchAttrCombo = new System.Windows.Forms.ComboBox();
			this.menu = new System.Windows.Forms.MenuStrip();
			this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.formSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rememberFormSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.rememberOnceFormSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreDefaultFormSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.pitchShiftPresetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadPresetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unloadPresetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitDiscardingChangesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.versionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.whyOkBtnIsDisabledToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.checkUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.githubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.roadmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.updateLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.englishDocumentationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.documentationEnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tutorialVideoEnglishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chineseDocumentationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.troubleShootingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.releaseNotesV4_9_25_0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.releaseNotesV4_10_17_0ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.documentationV0_1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.staffVisualizerV0_1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tutorialVideoV0_1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.chineseToolStripMenuItem = new Otomad.VegasScript.OtomadHelper.V4.ToolStripRadioButtonMenuItem();
			this.tchineseToolStripMenuItem = new Otomad.VegasScript.OtomadHelper.V4.ToolStripRadioButtonMenuItem();
			this.englishToolStripMenuItem = new Otomad.VegasScript.OtomadHelper.V4.ToolStripRadioButtonMenuItem();
			this.japaneseToolStripMenuItem = new Otomad.VegasScript.OtomadHelper.V4.ToolStripRadioButtonMenuItem();
			this.russianToolStripMenuItem = new Otomad.VegasScript.OtomadHelper.V4.ToolStripRadioButtonMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.Tabs = new System.Windows.Forms.TabControl();
			this.SourceTab = new System.Windows.Forms.TabPage();
			this.WarningInfoLabel = new System.Windows.Forms.Label();
			this.MidiConfigGroup = new System.Windows.Forms.GroupBox();
			this.MidiConfigTablePanel = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.MidiChannelCombo = new System.Windows.Forms.ComboBox();
			this.MidiChannelAdvancedBtn = new System.Windows.Forms.Button();
			this.ChooseMidiLbl = new System.Windows.Forms.Label();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.ChooseMidiText = new System.Windows.Forms.TextBox();
			this.ChooseMidiBtn = new System.Windows.Forms.Button();
			this.MidiChannelLbl = new System.Windows.Forms.Label();
			this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
			this.MidiStartSecondLbl = new System.Windows.Forms.Label();
			this.MidiEndSecondLbl = new System.Windows.Forms.Label();
			this.MidiBpmLbl = new System.Windows.Forms.Label();
			this.MidiBpmFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.MidiDynamicTempoFlow = new System.Windows.Forms.FlowLayoutPanel();
			this.MidiDynamicMidiBpmCheck = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.MidiDynamicMidiBpmFormCombo = new System.Windows.Forms.ComboBox();
			this.MidiMidiBpmCheck = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.MidiProjectBpmCheck = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
			this.MidiCustomBpmCheck = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.MidiCustomBpmBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
			this.MidiBeatLbl = new System.Windows.Forms.Label();
			this.MidiBeatTxt = new System.Windows.Forms.Label();
			this.SourceConfigGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.ChooseSourceLbl = new System.Windows.Forms.Label();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.ChooseSourceCombo = new System.Windows.Forms.ComboBox();
			this.ChooseSourceBtn = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SourceStartTimeLbl = new System.Windows.Forms.Label();
			this.SourceEndTimeLbl = new System.Windows.Forms.Label();
			this.GenerateAtLbl = new System.Windows.Forms.Label();
			this.flowLayoutPanel9 = new System.Windows.Forms.FlowLayoutPanel();
			this.GenerateAtBeginRadio = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.GenerateAtCursorRadio = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.flowLayoutPanel11 = new System.Windows.Forms.FlowLayoutPanel();
			this.GenerateAtCustomRadio = new Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton();
			this.GenerateAtCustomText = new System.Windows.Forms.TextBox();
			this.AudioTab = new System.Windows.Forms.TabPage();
			this.AudioParamsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.AudioFadeInLbl = new System.Windows.Forms.Label();
			this.AudioFadeInBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.AudioFadeInCurveCombo = new System.Windows.Forms.ComboBox();
			this.AudioFadeOutLbl = new System.Windows.Forms.Label();
			this.AudioFadeOutBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.AudioFadeOutCurveCombo = new System.Windows.Forms.ComboBox();
			this.AudioTuneGroup = new System.Windows.Forms.GroupBox();
			this.AudioTuneTablePanel = new System.Windows.Forms.TableLayoutPanel();
			this.AudioTuneMethodLbl = new System.Windows.Forms.Label();
			this.AudioStretchAttrLbl = new System.Windows.Forms.Label();
			this.AudioLockAttrLbl = new System.Windows.Forms.Label();
			this.flowLayoutPanel10 = new System.Windows.Forms.FlowLayoutPanel();
			this.AudioReserveFormantCheck = new System.Windows.Forms.CheckBox();
			this.AudioBasePitchLbl = new System.Windows.Forms.Label();
			this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
			this.AudioMainKeyCombo = new System.Windows.Forms.ComboBox();
			this.AudioMainOctaveCombo = new System.Windows.Forms.ComboBox();
			this.AudioPreviewLbl = new System.Windows.Forms.Label();
			this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
			this.PreviewAudioBtn = new System.Windows.Forms.Button();
			this.AudioPreviewAttrLbl = new System.Windows.Forms.Label();
			this.AudioPreviewAttrLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.PreviewBeepEngineCombo = new System.Windows.Forms.ComboBox();
			this.PreviewBeepWaveFormCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
			this.AudioConfigCheck = new System.Windows.Forms.CheckBox();
			this.AudioScratchCheck = new System.Windows.Forms.CheckBox();
			this.AudioLoopCheck = new System.Windows.Forms.CheckBox();
			this.AudioNormalizeCheck = new System.Windows.Forms.CheckBox();
			this.AudioFreezeLastFrameCheck = new System.Windows.Forms.CheckBox();
			this.AudioLegatoCheck = new System.Windows.Forms.CheckBox();
			this.CreateEventGroupCheck = new System.Windows.Forms.CheckBox();
			this.VideoTab = new System.Windows.Forms.TabPage();
			this.VideoParamsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.VideoStartThresholdCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoStartSaturationCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoStartContrastCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoStartHueCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoEndContrastBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndSaturationBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndHueBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndThresholdBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartContrastBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartSaturationBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartThresholdBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartHueBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartThresholdLbl = new System.Windows.Forms.Label();
			this.VideoEndThresholdLbl = new System.Windows.Forms.Label();
			this.VideoEndSaturationLbl = new System.Windows.Forms.Label();
			this.VideoEndContrastLbl = new System.Windows.Forms.Label();
			this.VideoStartContrastLbl = new System.Windows.Forms.Label();
			this.VideoStartHueLbl = new System.Windows.Forms.Label();
			this.VideoStartSaturationLbl = new System.Windows.Forms.Label();
			this.VideoEndHueLbl = new System.Windows.Forms.Label();
			this.VideoFadeInLbl = new System.Windows.Forms.Label();
			this.VideoFadeInBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoFadeInCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoFadeOutLbl = new System.Windows.Forms.Label();
			this.VideoFadeOutBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoFadeOutCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoGlowLbl = new System.Windows.Forms.Label();
			this.VideoGlowBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoGlowCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoGlowBrightLbl = new System.Windows.Forms.Label();
			this.VideoGlowBrightBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartSizeLbl = new System.Windows.Forms.Label();
			this.VideoStartSizeBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartSizeCurveCombo = new System.Windows.Forms.ComboBox();
			this.VideoEndSizeLbl = new System.Windows.Forms.Label();
			this.VideoEndSizeBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartRotationLbl = new System.Windows.Forms.Label();
			this.VideoStartRotationBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndRotationLbl = new System.Windows.Forms.Label();
			this.VideoEndRotationBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartHorizontalTransLbl = new System.Windows.Forms.Label();
			this.VideoStartHorizontalTransBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndHorizontalTransLbl = new System.Windows.Forms.Label();
			this.VideoEndHorizontalTransBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoStartVerticalTransLbl = new System.Windows.Forms.Label();
			this.VideoStartVerticalTransBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEndVerticalTransLbl = new System.Windows.Forms.Label();
			this.VideoEndVerticalTransBox = new Otomad.VegasScript.OtomadHelper.V4.IntegerTrackWithBox();
			this.VideoEffectsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.VideoEffectLbl = new System.Windows.Forms.Label();
			this.VideoEffectInitialValueLbl = new System.Windows.Forms.Label();
			this.VideoEffectCombo = new System.Windows.Forms.ComboBox();
			this.VideoEffectInitialValueCombo = new System.Windows.Forms.ComboBox();
			this.flowLayoutPanel7 = new System.Windows.Forms.FlowLayoutPanel();
			this.VideoConfigCheck = new System.Windows.Forms.CheckBox();
			this.VideoScratchCheck = new System.Windows.Forms.CheckBox();
			this.VideoLoopCheck = new System.Windows.Forms.CheckBox();
			this.VideoFreezeFirstFrameCheck = new System.Windows.Forms.CheckBox();
			this.VideoFreezeLastFrameCheck = new System.Windows.Forms.CheckBox();
			this.VideoLegatoCheck = new System.Windows.Forms.CheckBox();
			this.VideoMultitrackForChordsCheck = new System.Windows.Forms.CheckBox();
			this.SheetTab = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.StaffGenerateCheck = new System.Windows.Forms.CheckBox();
			this.StaffLineColorBtn = new System.Windows.Forms.Button();
			this.StaffLineColorLbl = new System.Windows.Forms.Label();
			this.StaffLineThicknessLbl = new System.Windows.Forms.Label();
			this.StaffParamsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.StaffClefLbl = new System.Windows.Forms.Label();
			this.StaffClefCombo = new System.Windows.Forms.ComboBox();
			this.StaffLineSpacingLbl = new System.Windows.Forms.Label();
			this.StaffSurfaceWidthLbl = new System.Windows.Forms.Label();
			this.StaffSurfacePositionLbl = new System.Windows.Forms.Label();
			this.StaffNotesShiftLbl = new System.Windows.Forms.Label();
			this.StaffNotesShiftBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.flowLayoutPanel8 = new System.Windows.Forms.FlowLayoutPanel();
			this.StaffVisualizerConfigCheck = new System.Windows.Forms.CheckBox();
			this.SheetConfigInfoLabel = new System.Windows.Forms.Label();
			this.YtpTab = new System.Windows.Forms.TabPage();
			this.YtpParamsGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
			this.YtpMinLenLbl = new System.Windows.Forms.Label();
			this.YtpClipsCountLbl = new System.Windows.Forms.Label();
			this.YtpClipsCountBox = new Otomad.VegasScript.OtomadHelper.V4.NumericUpDownWithUnit();
			this.YtpMaxLenLbl = new System.Windows.Forms.Label();
			this.YtpEffectsGroup = new System.Windows.Forms.GroupBox();
			this.YtpEnableAllEffectsCheck = new System.Windows.Forms.CheckBox();
			this.YtpEffectsCheckList = new System.Windows.Forms.CheckedListBox();
			this.YtpSelectInfo = new System.Windows.Forms.Label();
			this.YtpLbl = new System.Windows.Forms.Label();
			this.HelperTab = new System.Windows.Forms.TabPage();
			this.toolsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.CloseAfterOpenHelperCheck = new System.Windows.Forms.CheckBox();
			this.AutoLayoutTracksGroup = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
			this.AutoLayoutTracksLbl = new System.Windows.Forms.Label();
			this.AutoLayoutTracksSelectInfo = new System.Windows.Forms.Label();
			this.AutoLayoutTracksButtons = new System.Windows.Forms.TableLayoutPanel();
			this.GradientTracksBtn = new System.Windows.Forms.Button();
			this.AutoLayoutTracksBox3dBtn = new System.Windows.Forms.Button();
			this.AutoLayoutTracksGridBtn = new System.Windows.Forms.Button();
			this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
			this.TrackLegatoBtn = new System.Windows.Forms.Button();
			this.ClearTrackMotionBtn = new System.Windows.Forms.Button();
			this.ClearTrackEffectBtn = new System.Windows.Forms.Button();
			this.QuickSelectIntervalBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.QuickNormalizeBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.ReplaceClipsBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.ChangeTuneMethodBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.BatchSubtitleGenerationBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.FindClipsBtn = new Otomad.VegasScript.OtomadHelper.V4.CommandLinkButton();
			this.tableLayoutPanel19 = new System.Windows.Forms.TableLayoutPanel();
			this.HelperLbl = new System.Windows.Forms.Label();
			this.TrackLegatoMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.stackingTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.limitStretchLegatoTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.stretchLegatoTracksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lengthenLegatoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewBeepDurationBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineThicknessBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineSpacingBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfacePositionBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfaceWidthBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.YtpMinLenBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.YtpMaxLenBox)).BeginInit();
			this.menu.SuspendLayout();
			this.panel1.SuspendLayout();
			this.Tabs.SuspendLayout();
			this.SourceTab.SuspendLayout();
			this.MidiConfigGroup.SuspendLayout();
			this.MidiConfigTablePanel.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.flowLayoutPanel3.SuspendLayout();
			this.MidiBpmFlowPanel.SuspendLayout();
			this.MidiDynamicTempoFlow.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MidiCustomBpmBox)).BeginInit();
			this.tableLayoutPanel12.SuspendLayout();
			this.SourceConfigGroup.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel9.SuspendLayout();
			this.flowLayoutPanel11.SuspendLayout();
			this.AudioTab.SuspendLayout();
			this.AudioParamsGroup.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.AudioTuneGroup.SuspendLayout();
			this.AudioTuneTablePanel.SuspendLayout();
			this.flowLayoutPanel10.SuspendLayout();
			this.flowLayoutPanel6.SuspendLayout();
			this.tableLayoutPanel17.SuspendLayout();
			this.AudioPreviewAttrLayoutPanel.SuspendLayout();
			this.flowLayoutPanel5.SuspendLayout();
			this.VideoTab.SuspendLayout();
			this.VideoParamsGroup.SuspendLayout();
			this.tableLayoutPanel9.SuspendLayout();
			this.VideoEffectsGroup.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.flowLayoutPanel7.SuspendLayout();
			this.SheetTab.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.StaffParamsGroup.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.StaffNotesShiftBox)).BeginInit();
			this.flowLayoutPanel8.SuspendLayout();
			this.YtpTab.SuspendLayout();
			this.YtpParamsGroup.SuspendLayout();
			this.tableLayoutPanel16.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.YtpClipsCountBox)).BeginInit();
			this.YtpEffectsGroup.SuspendLayout();
			this.HelperTab.SuspendLayout();
			this.toolsTableLayoutPanel.SuspendLayout();
			this.AutoLayoutTracksGroup.SuspendLayout();
			this.tableLayoutPanel14.SuspendLayout();
			this.AutoLayoutTracksButtons.SuspendLayout();
			this.tableLayoutPanel15.SuspendLayout();
			this.tableLayoutPanel19.SuspendLayout();
			this.TrackLegatoMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.UserHelpLink, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.AboutBtn, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.OkBtn, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.CancelBtn, 4, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 670);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 2);
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(682, 47);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// UserHelpLink
			// 
			this.UserHelpLink.AutoSize = true;
			this.UserHelpLink.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UserHelpLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(95)))), ((int)(((byte)(184)))));
			this.UserHelpLink.Location = new System.Drawing.Point(289, 5);
			this.UserHelpLink.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.UserHelpLink.Name = "UserHelpLink";
			this.UserHelpLink.Size = new System.Drawing.Size(78, 40);
			this.UserHelpLink.TabIndex = 3;
			this.UserHelpLink.TabStop = true;
			this.UserHelpLink.Text = "使用说明...";
			this.UserHelpLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.UserHelpLink.Visible = false;
			this.UserHelpLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.UserHelpLink_LinkClicked);
			// 
			// AboutBtn
			// 
			this.AboutBtn.AutoSize = true;
			this.AboutBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AboutBtn.Location = new System.Drawing.Point(375, 9);
			this.AboutBtn.Margin = new System.Windows.Forms.Padding(4);
			this.AboutBtn.Name = "AboutBtn";
			this.AboutBtn.Size = new System.Drawing.Size(94, 32);
			this.AboutBtn.TabIndex = 2;
			this.AboutBtn.Text = "关于(&A)";
			this.AboutBtn.UseVisualStyleBackColor = true;
			this.AboutBtn.Click += new System.EventHandler(this.AboutBtn_Click);
			// 
			// OkBtn
			// 
			this.OkBtn.AutoSize = true;
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OkBtn.Location = new System.Drawing.Point(477, 9);
			this.OkBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(94, 32);
			this.OkBtn.TabIndex = 0;
			this.OkBtn.Text = "完成(&O)";
			this.OkBtn.UseVisualStyleBackColor = true;
			this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.AutoSize = true;
			this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelBtn.Location = new System.Drawing.Point(579, 9);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(94, 32);
			this.CancelBtn.TabIndex = 1;
			this.CancelBtn.Text = "取消(&C)";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// StaffLineColorDialog
			// 
			this.StaffLineColorDialog.AnyColor = true;
			this.StaffLineColorDialog.Color = System.Drawing.Color.White;
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
			// AudioTuneMethodCombo
			// 
			this.AudioTuneMethodCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioTuneMethodCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioTuneMethodCombo.FormattingEnabled = true;
			this.AudioTuneMethodCombo.Items.AddRange(new object[] {
			"不调音",
			"移调效果插件",
			"弹性音调更改",
			"古典音调更改",
			"瞎调音"});
			this.AudioTuneMethodCombo.Location = new System.Drawing.Point(75, 2);
			this.AudioTuneMethodCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 5);
			this.AudioTuneMethodCombo.Name = "AudioTuneMethodCombo";
			this.AudioTuneMethodCombo.Size = new System.Drawing.Size(561, 28);
			this.AudioTuneMethodCombo.TabIndex = 2;
			this.Balloon.SetToolTip(this.AudioTuneMethodCombo, "“移调效果插件”表示使用“音频 FX”中的“移调”效果插件改变音调，需要配置预设。\r\n“弹性音调更改”表示使用“Élastique”拉伸方式改变音调，也就是键盘上" +
		" +、- 键直接改变音调，\r\n有音高范围限制。");
			this.AudioTuneMethodCombo.SelectedIndexChanged += new System.EventHandler(this.AudioTuneMethodCombo_SelectedIndexChanged);
			// 
			// AudioLockStretchPitchCheck
			// 
			this.AudioLockStretchPitchCheck.AutoSize = true;
			this.AudioLockStretchPitchCheck.Location = new System.Drawing.Point(2, 2);
			this.AudioLockStretchPitchCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioLockStretchPitchCheck.Name = "AudioLockStretchPitchCheck";
			this.AudioLockStretchPitchCheck.Size = new System.Drawing.Size(136, 24);
			this.AudioLockStretchPitchCheck.TabIndex = 0;
			this.AudioLockStretchPitchCheck.Text = "锁定伸缩与音调";
			this.Balloon.SetToolTip(this.AudioLockStretchPitchCheck, "采用重采样方式，随着速度变化而改变音高。如果使用的是“弹性音调\r\n更改”方法，那么将会禁用拉伸音频功能。");
			this.AudioLockStretchPitchCheck.UseVisualStyleBackColor = true;
			// 
			// PreviewTuneAudioCheck
			// 
			this.PreviewTuneAudioCheck.AutoSize = true;
			this.PreviewTuneAudioCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewTuneAudioCheck.Location = new System.Drawing.Point(328, 2);
			this.PreviewTuneAudioCheck.Margin = new System.Windows.Forms.Padding(6, 2, 2, 2);
			this.PreviewTuneAudioCheck.Name = "PreviewTuneAudioCheck";
			this.PreviewTuneAudioCheck.Size = new System.Drawing.Size(166, 28);
			this.PreviewTuneAudioCheck.TabIndex = 7;
			this.PreviewTuneAudioCheck.Text = "使音频调整到主音高";
			this.Balloon.SetToolTip(this.PreviewTuneAudioCheck, "勾选后，预听音频时会将音频素材调整到主音高中央 C。\r\n否则，预听标准音高将会播放原始音高所设定的音高。");
			this.PreviewTuneAudioCheck.UseVisualStyleBackColor = true;
			// 
			// StaffRelativeValueCheck
			// 
			this.StaffRelativeValueCheck.AutoSize = true;
			this.StaffRelativeValueCheck.Checked = true;
			this.StaffRelativeValueCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel10.SetColumnSpan(this.StaffRelativeValueCheck, 4);
			this.StaffRelativeValueCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffRelativeValueCheck.Location = new System.Drawing.Point(2, 2);
			this.StaffRelativeValueCheck.Margin = new System.Windows.Forms.Padding(2);
			this.StaffRelativeValueCheck.Name = "StaffRelativeValueCheck";
			this.StaffRelativeValueCheck.Size = new System.Drawing.Size(634, 27);
			this.StaffRelativeValueCheck.TabIndex = 0;
			this.StaffRelativeValueCheck.Text = "使用相对值";
			this.Balloon.SetToolTip(this.StaffRelativeValueCheck, "勾选后，下方所填参数的像素单位将以相对于 1920 × 1080\r\n的尺寸进行定位；反之则以项目尺寸定位。\r\n");
			this.StaffRelativeValueCheck.UseVisualStyleBackColor = true;
			// 
			// StaffLegacyEngineCheck
			// 
			this.StaffLegacyEngineCheck.AutoSize = true;
			this.StaffLegacyEngineCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLegacyEngineCheck.Location = new System.Drawing.Point(192, 4);
			this.StaffLegacyEngineCheck.Margin = new System.Windows.Forms.Padding(2);
			this.StaffLegacyEngineCheck.Name = "StaffLegacyEngineCheck";
			this.StaffLegacyEngineCheck.Size = new System.Drawing.Size(91, 24);
			this.StaffLegacyEngineCheck.TabIndex = 5;
			this.StaffLegacyEngineCheck.Text = "旧版引擎";
			this.Balloon.SetToolTip(this.StaffLegacyEngineCheck, "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\r\n即音符和谱线的位置和形状由轨道运动定位。");
			this.StaffLegacyEngineCheck.UseVisualStyleBackColor = true;
			// 
			// MidiStartSecondBox
			// 
			this.MidiStartSecondBox.DoubleValue = 0D;
			this.MidiStartSecondBox.Enabled = false;
			this.MidiStartSecondBox.Location = new System.Drawing.Point(73, 2);
			this.MidiStartSecondBox.Margin = new System.Windows.Forms.Padding(2, 2, 12, 2);
			this.MidiStartSecondBox.Name = "MidiStartSecondBox";
			this.MidiStartSecondBox.Size = new System.Drawing.Size(156, 27);
			this.MidiStartSecondBox.TabIndex = 3;
			this.Balloon.SetToolTip(this.MidiStartSecondBox, "用于截取 MIDI 音乐的一部分。\r\n单位：秒。");
			this.MidiStartSecondBox.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// MidiEndSecondBox
			// 
			this.MidiEndSecondBox.DoubleValue = 0D;
			this.MidiEndSecondBox.Enabled = false;
			this.MidiEndSecondBox.Location = new System.Drawing.Point(316, 2);
			this.MidiEndSecondBox.Margin = new System.Windows.Forms.Padding(2);
			this.MidiEndSecondBox.Name = "MidiEndSecondBox";
			this.MidiEndSecondBox.Size = new System.Drawing.Size(156, 27);
			this.MidiEndSecondBox.TabIndex = 4;
			this.Balloon.SetToolTip(this.MidiEndSecondBox, "此处填写需要读取 MIDI 文件的时间长度。\r\n注意如果填写的值过小，将截去多余时间部分的音符。\r\n如果此处填写的值比起始秒数小或相等，则始终表示持续到整个音乐时" +
		"长末尾。\r\n单位：秒。");
			this.MidiEndSecondBox.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// SourceStartTimeText
			// 
			this.SourceStartTimeText.DoubleValue = 0D;
			this.SourceStartTimeText.Location = new System.Drawing.Point(73, 2);
			this.SourceStartTimeText.Margin = new System.Windows.Forms.Padding(2, 2, 12, 2);
			this.SourceStartTimeText.Name = "SourceStartTimeText";
			this.SourceStartTimeText.Size = new System.Drawing.Size(156, 27);
			this.SourceStartTimeText.TabIndex = 4;
			this.Balloon.SetToolTip(this.SourceStartTimeText, "此处填写媒体素材裁剪的开始时间。\r\n单位：秒。");
			this.SourceStartTimeText.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// SourceEndTimeText
			// 
			this.SourceEndTimeText.DoubleValue = 0D;
			this.SourceEndTimeText.Location = new System.Drawing.Point(316, 2);
			this.SourceEndTimeText.Margin = new System.Windows.Forms.Padding(2);
			this.SourceEndTimeText.Name = "SourceEndTimeText";
			this.SourceEndTimeText.Size = new System.Drawing.Size(156, 27);
			this.SourceEndTimeText.TabIndex = 5;
			this.Balloon.SetToolTip(this.SourceEndTimeText, "注意如果此处填写的数值比入点秒数小或相等，则始终表示持续到素材时间末尾。\r\n单位：秒。");
			this.SourceEndTimeText.Leave += new System.EventHandler(this.TrimTime_ValueChanged);
			// 
			// PreviewBeepDurationBox
			// 
			this.PreviewBeepDurationBox.Constrain = new decimal(new int[] {
			500,
			0,
			0,
			0});
			this.PreviewBeepDurationBox.EnableDecimalPlaces = true;
			this.PreviewBeepDurationBox.Location = new System.Drawing.Point(220, 2);
			this.PreviewBeepDurationBox.Margin = new System.Windows.Forms.Padding(2);
			this.PreviewBeepDurationBox.Maximum = new decimal(new int[] {
			2000,
			0,
			0,
			0});
			this.PreviewBeepDurationBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.PreviewBeepDurationBox.Name = "PreviewBeepDurationBox";
			this.PreviewBeepDurationBox.Size = new System.Drawing.Size(100, 27);
			this.PreviewBeepDurationBox.Suffix = "ms";
			this.PreviewBeepDurationBox.TabIndex = 5;
			this.Balloon.SetToolTip(this.PreviewBeepDurationBox, "预听标准音高所持续的时间。\r\n单位：毫秒。");
			this.PreviewBeepDurationBox.Value = new decimal(new int[] {
			500,
			0,
			0,
			0});
			// 
			// StaffLineThicknessBox
			// 
			this.StaffLineThicknessBox.Constrain = new decimal(new int[] {
			25,
			0,
			0,
			0});
			this.StaffLineThicknessBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineThicknessBox.Enabled = false;
			this.StaffLineThicknessBox.EnableDecimalPlaces = true;
			this.StaffLineThicknessBox.Location = new System.Drawing.Point(75, 30);
			this.StaffLineThicknessBox.Margin = new System.Windows.Forms.Padding(2);
			this.StaffLineThicknessBox.Minimum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.StaffLineThicknessBox.Name = "StaffLineThicknessBox";
			this.StaffLineThicknessBox.Size = new System.Drawing.Size(242, 27);
			this.StaffLineThicknessBox.Suffix = "%";
			this.StaffLineThicknessBox.TabIndex = 11;
			this.Balloon.SetToolTip(this.StaffLineThicknessBox, "生成五线谱的谱线时，五线谱谱线的粗细。当粗细值达到 100 % 后，谱线将会占满整个谱线间距。\r\n单位：百分比。");
			this.StaffLineThicknessBox.Value = new decimal(new int[] {
			25,
			0,
			0,
			0});
			// 
			// StaffLineSpacingBox
			// 
			this.StaffLineSpacingBox.Constrain = new decimal(new int[] {
			44,
			0,
			0,
			0});
			this.StaffLineSpacingBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineSpacingBox.Enabled = false;
			this.StaffLineSpacingBox.EnableDecimalPlaces = true;
			this.StaffLineSpacingBox.Location = new System.Drawing.Point(394, 33);
			this.StaffLineSpacingBox.Margin = new System.Windows.Forms.Padding(2);
			this.StaffLineSpacingBox.Maximum = new decimal(new int[] {
			65536,
			0,
			0,
			0});
			this.StaffLineSpacingBox.Name = "StaffLineSpacingBox";
			this.StaffLineSpacingBox.Size = new System.Drawing.Size(242, 27);
			this.StaffLineSpacingBox.Suffix = "px";
			this.StaffLineSpacingBox.TabIndex = 7;
			this.Balloon.SetToolTip(this.StaffLineSpacingBox, "五线谱线与线之间的间距。\r\n单位：像素。");
			this.StaffLineSpacingBox.Value = new decimal(new int[] {
			44,
			0,
			0,
			0});
			// 
			// StaffSurfacePositionBox
			// 
			this.StaffSurfacePositionBox.Constrain = new decimal(new int[] {
			225,
			0,
			0,
			0});
			this.StaffSurfacePositionBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfacePositionBox.Enabled = false;
			this.StaffSurfacePositionBox.EnableDecimalPlaces = true;
			this.StaffSurfacePositionBox.Location = new System.Drawing.Point(394, 64);
			this.StaffSurfacePositionBox.Margin = new System.Windows.Forms.Padding(2);
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
			this.StaffSurfacePositionBox.Size = new System.Drawing.Size(242, 27);
			this.StaffSurfacePositionBox.Suffix = "px";
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
			this.StaffSurfaceWidthBox.Constrain = new decimal(new int[] {
			1500,
			0,
			0,
			0});
			this.StaffSurfaceWidthBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfaceWidthBox.Enabled = false;
			this.StaffSurfaceWidthBox.EnableDecimalPlaces = true;
			this.StaffSurfaceWidthBox.Location = new System.Drawing.Point(75, 64);
			this.StaffSurfaceWidthBox.Margin = new System.Windows.Forms.Padding(2);
			this.StaffSurfaceWidthBox.Maximum = new decimal(new int[] {
			65536,
			0,
			0,
			0});
			this.StaffSurfaceWidthBox.Name = "StaffSurfaceWidthBox";
			this.StaffSurfaceWidthBox.Size = new System.Drawing.Size(242, 27);
			this.StaffSurfaceWidthBox.Suffix = "px";
			this.StaffSurfaceWidthBox.TabIndex = 8;
			this.Balloon.SetToolTip(this.StaffSurfaceWidthBox, "将在屏幕中间所填的宽度内显示音符，用于左右留白，给左侧的谱号留间距。\r\n单位：像素。");
			this.StaffSurfaceWidthBox.Value = new decimal(new int[] {
			1500,
			0,
			0,
			0});
			// 
			// YtpMinLenBox
			// 
			this.YtpMinLenBox.Constrain = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.YtpMinLenBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpMinLenBox.EnableDecimalPlaces = true;
			this.YtpMinLenBox.Location = new System.Drawing.Point(75, 2);
			this.YtpMinLenBox.Margin = new System.Windows.Forms.Padding(2);
			this.YtpMinLenBox.Maximum = new decimal(new int[] {
			30000,
			0,
			0,
			0});
			this.YtpMinLenBox.Minimum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.YtpMinLenBox.Name = "YtpMinLenBox";
			this.YtpMinLenBox.Size = new System.Drawing.Size(242, 27);
			this.YtpMinLenBox.Suffix = "ms";
			this.YtpMinLenBox.TabIndex = 8;
			this.Balloon.SetToolTip(this.YtpMinLenBox, "指定单个轨道剪辑的最小长度。\r\n单位：毫秒。");
			this.YtpMinLenBox.Value = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.YtpMinLenBox.ValueChanged += new System.EventHandler(this.YtpLenBox_ValueChanged);
			// 
			// YtpMaxLenBox
			// 
			this.YtpMaxLenBox.Constrain = new decimal(new int[] {
			5000,
			0,
			0,
			0});
			this.YtpMaxLenBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpMaxLenBox.EnableDecimalPlaces = true;
			this.YtpMaxLenBox.Location = new System.Drawing.Point(394, 2);
			this.YtpMaxLenBox.Margin = new System.Windows.Forms.Padding(2);
			this.YtpMaxLenBox.Maximum = new decimal(new int[] {
			30000,
			0,
			0,
			0});
			this.YtpMaxLenBox.Minimum = new decimal(new int[] {
			10,
			0,
			0,
			0});
			this.YtpMaxLenBox.Name = "YtpMaxLenBox";
			this.YtpMaxLenBox.Size = new System.Drawing.Size(242, 27);
			this.YtpMaxLenBox.Suffix = "ms";
			this.YtpMaxLenBox.TabIndex = 9;
			this.Balloon.SetToolTip(this.YtpMaxLenBox, "指定单个轨道剪辑的最大长度。\r\n单位：毫秒。");
			this.YtpMaxLenBox.Value = new decimal(new int[] {
			5000,
			0,
			0,
			0});
			this.YtpMaxLenBox.ValueChanged += new System.EventHandler(this.YtpLenBox_ValueChanged);
			// 
			// PreviewBasePitchBtn
			// 
			this.PreviewBasePitchBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewBasePitchBtn.Location = new System.Drawing.Point(2, 2);
			this.PreviewBasePitchBtn.Margin = new System.Windows.Forms.Padding(2);
			this.PreviewBasePitchBtn.Name = "PreviewBasePitchBtn";
			this.PreviewBasePitchBtn.Size = new System.Drawing.Size(279, 31);
			this.PreviewBasePitchBtn.TabIndex = 1;
			this.PreviewBasePitchBtn.Text = "预听标准音高(&B)";
			this.PreviewBasePitchBtn.UseVisualStyleBackColor = true;
			this.PreviewBasePitchBtn.Click += new System.EventHandler(this.PreviewBasePitchBtn_MouseDown);
			// 
			// AudioStretchAttrCombo
			// 
			this.AudioStretchAttrCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioStretchAttrCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.AudioStretchAttrCombo.FormattingEnabled = true;
			this.AudioStretchAttrCombo.Location = new System.Drawing.Point(75, 37);
			this.AudioStretchAttrCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 5);
			this.AudioStretchAttrCombo.Name = "AudioStretchAttrCombo";
			this.AudioStretchAttrCombo.Size = new System.Drawing.Size(561, 28);
			this.AudioStretchAttrCombo.TabIndex = 3;
			// 
			// menu
			// 
			this.menu.BackColor = System.Drawing.Color.Transparent;
			this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.fileMenuItem,
			this.helpToolStripMenuItem,
			this.languageToolStripMenuItem});
			this.menu.Location = new System.Drawing.Point(0, 0);
			this.menu.Name = "menu";
			this.menu.Padding = new System.Windows.Forms.Padding(6, 2, 0, 4);
			this.menu.Size = new System.Drawing.Size(682, 30);
			this.menu.TabIndex = 2;
			this.menu.Text = "menuStrip1";
			// 
			// fileMenuItem
			// 
			this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.saveConfigToolStripMenuItem,
			this.resetConfigToolStripMenuItem,
			this.formSizeToolStripMenuItem,
			this.toolStripSeparator6,
			this.pitchShiftPresetMenuItem,
			this.toolStripSeparator1,
			this.exitDiscardingChangesToolStripMenuItem,
			this.exitToolStripMenuItem});
			this.fileMenuItem.Name = "fileMenuItem";
			this.fileMenuItem.Size = new System.Drawing.Size(70, 24);
			this.fileMenuItem.Text = "文件(&F)";
			// 
			// saveConfigToolStripMenuItem
			// 
			this.saveConfigToolStripMenuItem.Name = "saveConfigToolStripMenuItem";
			this.saveConfigToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveConfigToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
			this.saveConfigToolStripMenuItem.Text = "保存用户配置(&S)";
			// 
			// resetConfigToolStripMenuItem
			// 
			this.resetConfigToolStripMenuItem.Name = "resetConfigToolStripMenuItem";
			this.resetConfigToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
			this.resetConfigToolStripMenuItem.Text = "重置用户配置(&R)";
			this.resetConfigToolStripMenuItem.Click += new System.EventHandler(this.resetConfigToolStripMenuItem_Click);
			// 
			// formSizeToolStripMenuItem
			// 
			this.formSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.rememberFormSizeToolStripMenuItem,
			this.toolStripSeparator2,
			this.rememberOnceFormSizeToolStripMenuItem,
			this.restoreDefaultFormSizeToolStripMenuItem});
			this.formSizeToolStripMenuItem.Name = "formSizeToolStripMenuItem";
			this.formSizeToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
			this.formSizeToolStripMenuItem.Text = "窗体大小";
			// 
			// rememberFormSizeToolStripMenuItem
			// 
			this.rememberFormSizeToolStripMenuItem.CheckOnClick = true;
			this.rememberFormSizeToolStripMenuItem.Name = "rememberFormSizeToolStripMenuItem";
			this.rememberFormSizeToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
			this.rememberFormSizeToolStripMenuItem.Text = "记住窗体大小";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(239, 6);
			// 
			// rememberOnceFormSizeToolStripMenuItem
			// 
			this.rememberOnceFormSizeToolStripMenuItem.Name = "rememberOnceFormSizeToolStripMenuItem";
			this.rememberOnceFormSizeToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
			this.rememberOnceFormSizeToolStripMenuItem.Text = "记住一次窗体大小";
			this.rememberOnceFormSizeToolStripMenuItem.Click += new System.EventHandler(this.RememberOnceFormSizeToolStripMenuItem_Click);
			// 
			// restoreDefaultFormSizeToolStripMenuItem
			// 
			this.restoreDefaultFormSizeToolStripMenuItem.Name = "restoreDefaultFormSizeToolStripMenuItem";
			this.restoreDefaultFormSizeToolStripMenuItem.Size = new System.Drawing.Size(242, 26);
			this.restoreDefaultFormSizeToolStripMenuItem.Text = "下次恢复默认窗体大小";
			this.restoreDefaultFormSizeToolStripMenuItem.Click += new System.EventHandler(this.RestoreDefaultFormSizeToolStripMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(268, 6);
			// 
			// pitchShiftPresetMenuItem
			// 
			this.pitchShiftPresetMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.loadPresetsToolStripMenuItem,
			this.unloadPresetsToolStripMenuItem});
			this.pitchShiftPresetMenuItem.Name = "pitchShiftPresetMenuItem";
			this.pitchShiftPresetMenuItem.Size = new System.Drawing.Size(271, 26);
			this.pitchShiftPresetMenuItem.Text = "移调插件预设(&P)";
			// 
			// loadPresetsToolStripMenuItem
			// 
			this.loadPresetsToolStripMenuItem.Name = "loadPresetsToolStripMenuItem";
			this.loadPresetsToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
			this.loadPresetsToolStripMenuItem.Text = "加载预设...";
			this.loadPresetsToolStripMenuItem.Click += new System.EventHandler(this.LoadPresetsToolStripMenuItem_Click);
			// 
			// unloadPresetsToolStripMenuItem
			// 
			this.unloadPresetsToolStripMenuItem.Name = "unloadPresetsToolStripMenuItem";
			this.unloadPresetsToolStripMenuItem.Size = new System.Drawing.Size(161, 26);
			this.unloadPresetsToolStripMenuItem.Text = "卸载预设...";
			this.unloadPresetsToolStripMenuItem.Click += new System.EventHandler(this.LoadPresetsToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(268, 6);
			// 
			// exitDiscardingChangesToolStripMenuItem
			// 
			this.exitDiscardingChangesToolStripMenuItem.Name = "exitDiscardingChangesToolStripMenuItem";
			this.exitDiscardingChangesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.exitDiscardingChangesToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
			this.exitDiscardingChangesToolStripMenuItem.Text = "放弃更改并退出(&D)";
			this.exitDiscardingChangesToolStripMenuItem.Click += new System.EventHandler(this.exitDiscardingChangesToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Esc";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(271, 26);
			this.exitToolStripMenuItem.Text = "退出(&X)";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.versionToolStripMenuItem,
			this.aboutToolStripMenuItem,
			this.whyOkBtnIsDisabledToolStripMenuItem,
			this.toolStripSeparator7,
			this.checkUpdateToolStripMenuItem,
			this.githubToolStripMenuItem,
			this.roadmapToolStripMenuItem,
			this.updateLogsToolStripMenuItem,
			this.toolStripSeparator3,
			this.englishDocumentationsToolStripMenuItem,
			this.chineseDocumentationsToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
			this.helpToolStripMenuItem.Text = "帮助(&H)";
			// 
			// versionToolStripMenuItem
			// 
			this.versionToolStripMenuItem.Enabled = false;
			this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
			this.versionToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.versionToolStripMenuItem.Text = "版本号";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.ShortcutKeyDisplayString = "Alt+A";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.aboutToolStripMenuItem.Text = "关于(&A)";
			// 
			// whyOkBtnIsDisabledToolStripMenuItem
			// 
			this.whyOkBtnIsDisabledToolStripMenuItem.Name = "whyOkBtnIsDisabledToolStripMenuItem";
			this.whyOkBtnIsDisabledToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.whyOkBtnIsDisabledToolStripMenuItem.Text = "为什么无法点击完成按钮？";
			this.whyOkBtnIsDisabledToolStripMenuItem.Visible = false;
			this.whyOkBtnIsDisabledToolStripMenuItem.Click += new System.EventHandler(this.WhyOkBtnIsDisabledToolStripMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(269, 6);
			// 
			// checkUpdateToolStripMenuItem
			// 
			this.checkUpdateToolStripMenuItem.Name = "checkUpdateToolStripMenuItem";
			this.checkUpdateToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.checkUpdateToolStripMenuItem.Text = "检查更新(&U)";
			// 
			// githubToolStripMenuItem
			// 
			this.githubToolStripMenuItem.Name = "githubToolStripMenuItem";
			this.githubToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.githubToolStripMenuItem.Text = "仓库地址";
			// 
			// roadmapToolStripMenuItem
			// 
			this.roadmapToolStripMenuItem.Name = "roadmapToolStripMenuItem";
			this.roadmapToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.roadmapToolStripMenuItem.Text = "路线图";
			// 
			// updateLogsToolStripMenuItem
			// 
			this.updateLogsToolStripMenuItem.Name = "updateLogsToolStripMenuItem";
			this.updateLogsToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.updateLogsToolStripMenuItem.Text = "更新日志";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(269, 6);
			// 
			// englishDocumentationsToolStripMenuItem
			// 
			this.englishDocumentationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.documentationEnglishToolStripMenuItem,
			this.tutorialVideoEnglishToolStripMenuItem});
			this.englishDocumentationsToolStripMenuItem.Name = "englishDocumentationsToolStripMenuItem";
			this.englishDocumentationsToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.englishDocumentationsToolStripMenuItem.Text = "英语说明文档";
			// 
			// documentationEnglishToolStripMenuItem
			// 
			this.documentationEnglishToolStripMenuItem.Name = "documentationEnglishToolStripMenuItem";
			this.documentationEnglishToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.documentationEnglishToolStripMenuItem.Text = "说明文档";
			// 
			// tutorialVideoEnglishToolStripMenuItem
			// 
			this.tutorialVideoEnglishToolStripMenuItem.Name = "tutorialVideoEnglishToolStripMenuItem";
			this.tutorialVideoEnglishToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.tutorialVideoEnglishToolStripMenuItem.Text = "教程视频";
			// 
			// chineseDocumentationsToolStripMenuItem
			// 
			this.chineseDocumentationsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.troubleShootingToolStripMenuItem,
			this.toolStripSeparator4,
			this.releaseNotesV4_9_25_0ToolStripMenuItem,
			this.releaseNotesV4_10_17_0ToolStripMenuItem,
			this.toolStripSeparator5,
			this.documentationV0_1ToolStripMenuItem,
			this.staffVisualizerV0_1ToolStripMenuItem,
			this.tutorialVideoV0_1ToolStripMenuItem});
			this.chineseDocumentationsToolStripMenuItem.Name = "chineseDocumentationsToolStripMenuItem";
			this.chineseDocumentationsToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
			this.chineseDocumentationsToolStripMenuItem.Text = "中文说明文档";
			// 
			// troubleShootingToolStripMenuItem
			// 
			this.troubleShootingToolStripMenuItem.Name = "troubleShootingToolStripMenuItem";
			this.troubleShootingToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.troubleShootingToolStripMenuItem.Text = "疑难解答";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(249, 6);
			// 
			// releaseNotesV4_9_25_0ToolStripMenuItem
			// 
			this.releaseNotesV4_9_25_0ToolStripMenuItem.Name = "releaseNotesV4_9_25_0ToolStripMenuItem";
			this.releaseNotesV4_9_25_0ToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.releaseNotesV4_9_25_0ToolStripMenuItem.Text = "发行说明 (v4.9.25.0)";
			// 
			// releaseNotesV4_10_17_0ToolStripMenuItem
			// 
			this.releaseNotesV4_10_17_0ToolStripMenuItem.Name = "releaseNotesV4_10_17_0ToolStripMenuItem";
			this.releaseNotesV4_10_17_0ToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.releaseNotesV4_10_17_0ToolStripMenuItem.Text = "发行说明 (v4.10.17.0)";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(249, 6);
			// 
			// documentationV0_1ToolStripMenuItem
			// 
			this.documentationV0_1ToolStripMenuItem.Name = "documentationV0_1ToolStripMenuItem";
			this.documentationV0_1ToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.documentationV0_1ToolStripMenuItem.Text = "说明文档 (v0.1)";
			// 
			// staffVisualizerV0_1ToolStripMenuItem
			// 
			this.staffVisualizerV0_1ToolStripMenuItem.Name = "staffVisualizerV0_1ToolStripMenuItem";
			this.staffVisualizerV0_1ToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.staffVisualizerV0_1ToolStripMenuItem.Text = "五线谱可视化文档 (v0.1)";
			// 
			// tutorialVideoV0_1ToolStripMenuItem
			// 
			this.tutorialVideoV0_1ToolStripMenuItem.Name = "tutorialVideoV0_1ToolStripMenuItem";
			this.tutorialVideoV0_1ToolStripMenuItem.Size = new System.Drawing.Size(252, 26);
			this.tutorialVideoV0_1ToolStripMenuItem.Text = "教程视频 (v0.1)";
			// 
			// languageToolStripMenuItem
			// 
			this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.chineseToolStripMenuItem,
			this.tchineseToolStripMenuItem,
			this.englishToolStripMenuItem,
			this.japaneseToolStripMenuItem,
			this.russianToolStripMenuItem});
			this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
			this.languageToolStripMenuItem.Size = new System.Drawing.Size(88, 24);
			this.languageToolStripMenuItem.Text = "&Language";
			// 
			// chineseToolStripMenuItem
			// 
			this.chineseToolStripMenuItem.Checked = true;
			this.chineseToolStripMenuItem.CheckOnClick = true;
			this.chineseToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chineseToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.chineseToolStripMenuItem.Name = "chineseToolStripMenuItem";
			this.chineseToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.chineseToolStripMenuItem.Text = "简体中文";
			// 
			// tchineseToolStripMenuItem
			// 
			this.tchineseToolStripMenuItem.CheckOnClick = true;
			this.tchineseToolStripMenuItem.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tchineseToolStripMenuItem.Name = "tchineseToolStripMenuItem";
			this.tchineseToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.tchineseToolStripMenuItem.Text = "繁體中文";
			// 
			// englishToolStripMenuItem
			// 
			this.englishToolStripMenuItem.CheckOnClick = true;
			this.englishToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
			this.englishToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.englishToolStripMenuItem.Text = "English";
			// 
			// japaneseToolStripMenuItem
			// 
			this.japaneseToolStripMenuItem.CheckOnClick = true;
			this.japaneseToolStripMenuItem.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.japaneseToolStripMenuItem.Name = "japaneseToolStripMenuItem";
			this.japaneseToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.japaneseToolStripMenuItem.Text = "日本語";
			// 
			// russianToolStripMenuItem
			// 
			this.russianToolStripMenuItem.CheckOnClick = true;
			this.russianToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.russianToolStripMenuItem.Name = "russianToolStripMenuItem";
			this.russianToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
			this.russianToolStripMenuItem.Text = "Русский";
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.Tabs);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 30);
			this.panel1.Margin = new System.Windows.Forms.Padding(2);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.panel1.Size = new System.Drawing.Size(682, 640);
			this.panel1.TabIndex = 3;
			// 
			// Tabs
			// 
			this.Tabs.Controls.Add(this.SourceTab);
			this.Tabs.Controls.Add(this.AudioTab);
			this.Tabs.Controls.Add(this.VideoTab);
			this.Tabs.Controls.Add(this.SheetTab);
			this.Tabs.Controls.Add(this.YtpTab);
			this.Tabs.Controls.Add(this.HelperTab);
			this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tabs.Location = new System.Drawing.Point(8, 0);
			this.Tabs.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.Tabs.Multiline = true;
			this.Tabs.Name = "Tabs";
			this.Tabs.SelectedIndex = 0;
			this.Tabs.Size = new System.Drawing.Size(666, 640);
			this.Tabs.TabIndex = 2;
			// 
			// SourceTab
			// 
			this.SourceTab.AutoScroll = true;
			this.SourceTab.BackColor = System.Drawing.Color.Transparent;
			this.SourceTab.Controls.Add(this.WarningInfoLabel);
			this.SourceTab.Controls.Add(this.MidiConfigGroup);
			this.SourceTab.Controls.Add(this.SourceConfigGroup);
			this.SourceTab.Location = new System.Drawing.Point(4, 29);
			this.SourceTab.Margin = new System.Windows.Forms.Padding(2);
			this.SourceTab.Name = "SourceTab";
			this.SourceTab.Padding = new System.Windows.Forms.Padding(5);
			this.SourceTab.Size = new System.Drawing.Size(658, 607);
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
			this.WarningInfoLabel.Location = new System.Drawing.Point(5, 470);
			this.WarningInfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.WarningInfoLabel.MaximumSize = new System.Drawing.Size(540, 0);
			this.WarningInfoLabel.Name = "WarningInfoLabel";
			this.WarningInfoLabel.Padding = new System.Windows.Forms.Padding(2);
			this.WarningInfoLabel.Size = new System.Drawing.Size(4, 30);
			this.WarningInfoLabel.TabIndex = 3;
			// 
			// MidiConfigGroup
			// 
			this.MidiConfigGroup.AutoSize = true;
			this.MidiConfigGroup.Controls.Add(this.MidiConfigTablePanel);
			this.MidiConfigGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.MidiConfigGroup.Location = new System.Drawing.Point(5, 190);
			this.MidiConfigGroup.Margin = new System.Windows.Forms.Padding(2);
			this.MidiConfigGroup.Name = "MidiConfigGroup";
			this.MidiConfigGroup.Padding = new System.Windows.Forms.Padding(5);
			this.MidiConfigGroup.Size = new System.Drawing.Size(648, 280);
			this.MidiConfigGroup.TabIndex = 2;
			this.MidiConfigGroup.TabStop = false;
			this.MidiConfigGroup.Text = "MIDI 属性";
			this.MidiConfigGroup.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.MidiConfigGroup.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.MidiConfigGroup.DragLeave += new System.EventHandler(this.OnDragLeave);
			// 
			// MidiConfigTablePanel
			// 
			this.MidiConfigTablePanel.AutoSize = true;
			this.MidiConfigTablePanel.ColumnCount = 1;
			this.MidiConfigTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MidiConfigTablePanel.Controls.Add(this.tableLayoutPanel5, 0, 3);
			this.MidiConfigTablePanel.Controls.Add(this.ChooseMidiLbl, 0, 0);
			this.MidiConfigTablePanel.Controls.Add(this.tableLayoutPanel6, 0, 1);
			this.MidiConfigTablePanel.Controls.Add(this.MidiChannelLbl, 0, 2);
			this.MidiConfigTablePanel.Controls.Add(this.flowLayoutPanel3, 0, 5);
			this.MidiConfigTablePanel.Controls.Add(this.MidiBpmLbl, 0, 6);
			this.MidiConfigTablePanel.Controls.Add(this.MidiBpmFlowPanel, 0, 7);
			this.MidiConfigTablePanel.Controls.Add(this.tableLayoutPanel12, 0, 4);
			this.MidiConfigTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiConfigTablePanel.Location = new System.Drawing.Point(5, 25);
			this.MidiConfigTablePanel.Margin = new System.Windows.Forms.Padding(2);
			this.MidiConfigTablePanel.Name = "MidiConfigTablePanel";
			this.MidiConfigTablePanel.RowCount = 8;
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MidiConfigTablePanel.Size = new System.Drawing.Size(638, 250);
			this.MidiConfigTablePanel.TabIndex = 1;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.MidiChannelCombo, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.MidiChannelAdvancedBtn, 1, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 78);
			this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(638, 33);
			this.tableLayoutPanel5.TabIndex = 15;
			// 
			// MidiChannelCombo
			// 
			this.MidiChannelCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MidiChannelCombo.Enabled = false;
			this.MidiChannelCombo.FormattingEnabled = true;
			this.MidiChannelCombo.Location = new System.Drawing.Point(2, 2);
			this.MidiChannelCombo.Margin = new System.Windows.Forms.Padding(2);
			this.MidiChannelCombo.Name = "MidiChannelCombo";
			this.MidiChannelCombo.Size = new System.Drawing.Size(555, 28);
			this.MidiChannelCombo.TabIndex = 7;
			this.MidiChannelCombo.SelectedIndexChanged += new System.EventHandler(this.MidiChannelCombo_SelectedIndexChanged);
			// 
			// MidiChannelAdvancedBtn
			// 
			this.MidiChannelAdvancedBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelAdvancedBtn.Enabled = false;
			this.MidiChannelAdvancedBtn.Location = new System.Drawing.Point(561, 2);
			this.MidiChannelAdvancedBtn.Margin = new System.Windows.Forms.Padding(2);
			this.MidiChannelAdvancedBtn.Name = "MidiChannelAdvancedBtn";
			this.MidiChannelAdvancedBtn.Size = new System.Drawing.Size(75, 29);
			this.MidiChannelAdvancedBtn.TabIndex = 1;
			this.MidiChannelAdvancedBtn.Text = "高级...";
			this.MidiChannelAdvancedBtn.UseVisualStyleBackColor = true;
			this.MidiChannelAdvancedBtn.Click += new System.EventHandler(this.MidiChannelAdvancedBtn_Click);
			// 
			// ChooseMidiLbl
			// 
			this.ChooseMidiLbl.AutoSize = true;
			this.ChooseMidiLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChooseMidiLbl.Location = new System.Drawing.Point(2, 0);
			this.ChooseMidiLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ChooseMidiLbl.Name = "ChooseMidiLbl";
			this.ChooseMidiLbl.Size = new System.Drawing.Size(634, 20);
			this.ChooseMidiLbl.TabIndex = 0;
			this.ChooseMidiLbl.Text = "选择 MIDI 文件";
			this.ChooseMidiLbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.ChooseMidiText, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.ChooseMidiBtn, 1, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 20);
			this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 1;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(638, 32);
			this.tableLayoutPanel6.TabIndex = 2;
			// 
			// ChooseMidiText
			// 
			this.ChooseMidiText.Dock = System.Windows.Forms.DockStyle.Top;
			this.ChooseMidiText.Location = new System.Drawing.Point(2, 2);
			this.ChooseMidiText.Margin = new System.Windows.Forms.Padding(2);
			this.ChooseMidiText.Name = "ChooseMidiText";
			this.ChooseMidiText.ReadOnly = true;
			this.ChooseMidiText.Size = new System.Drawing.Size(555, 27);
			this.ChooseMidiText.TabIndex = 2;
			this.ChooseMidiText.Text = "<未选择 MIDI 文件>";
			// 
			// ChooseMidiBtn
			// 
			this.ChooseMidiBtn.Dock = System.Windows.Forms.DockStyle.Top;
			this.ChooseMidiBtn.Location = new System.Drawing.Point(561, 2);
			this.ChooseMidiBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ChooseMidiBtn.Name = "ChooseMidiBtn";
			this.ChooseMidiBtn.Size = new System.Drawing.Size(75, 28);
			this.ChooseMidiBtn.TabIndex = 3;
			this.ChooseMidiBtn.Text = "浏览...";
			this.ChooseMidiBtn.UseVisualStyleBackColor = true;
			this.ChooseMidiBtn.Click += new System.EventHandler(this.ChooseMidiBtn_Click);
			// 
			// MidiChannelLbl
			// 
			this.MidiChannelLbl.AutoSize = true;
			this.MidiChannelLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiChannelLbl.Location = new System.Drawing.Point(2, 58);
			this.MidiChannelLbl.Margin = new System.Windows.Forms.Padding(2, 6, 2, 0);
			this.MidiChannelLbl.Name = "MidiChannelLbl";
			this.MidiChannelLbl.Size = new System.Drawing.Size(634, 20);
			this.MidiChannelLbl.TabIndex = 3;
			this.MidiChannelLbl.Text = "使用 MIDI 通道";
			this.MidiChannelLbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// flowLayoutPanel3
			// 
			this.flowLayoutPanel3.AutoSize = true;
			this.flowLayoutPanel3.Controls.Add(this.MidiStartSecondLbl);
			this.flowLayoutPanel3.Controls.Add(this.MidiStartSecondBox);
			this.flowLayoutPanel3.Controls.Add(this.MidiEndSecondLbl);
			this.flowLayoutPanel3.Controls.Add(this.MidiEndSecondBox);
			this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel3.Location = new System.Drawing.Point(2, 149);
			this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel3.Name = "flowLayoutPanel3";
			this.flowLayoutPanel3.Size = new System.Drawing.Size(634, 31);
			this.flowLayoutPanel3.TabIndex = 9;
			// 
			// MidiStartSecondLbl
			// 
			this.MidiStartSecondLbl.AutoSize = true;
			this.MidiStartSecondLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiStartSecondLbl.Location = new System.Drawing.Point(0, 0);
			this.MidiStartSecondLbl.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.MidiStartSecondLbl.Name = "MidiStartSecondLbl";
			this.MidiStartSecondLbl.Size = new System.Drawing.Size(69, 31);
			this.MidiStartSecondLbl.TabIndex = 0;
			this.MidiStartSecondLbl.Text = "起始秒数";
			this.MidiStartSecondLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiEndSecondLbl
			// 
			this.MidiEndSecondLbl.AutoSize = true;
			this.MidiEndSecondLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiEndSecondLbl.Location = new System.Drawing.Point(243, 0);
			this.MidiEndSecondLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.MidiEndSecondLbl.Name = "MidiEndSecondLbl";
			this.MidiEndSecondLbl.Size = new System.Drawing.Size(69, 31);
			this.MidiEndSecondLbl.TabIndex = 2;
			this.MidiEndSecondLbl.Text = "终止秒数";
			this.MidiEndSecondLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiBpmLbl
			// 
			this.MidiBpmLbl.AutoSize = true;
			this.MidiBpmLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiBpmLbl.Location = new System.Drawing.Point(2, 191);
			this.MidiBpmLbl.Margin = new System.Windows.Forms.Padding(2, 9, 2, 0);
			this.MidiBpmLbl.Name = "MidiBpmLbl";
			this.MidiBpmLbl.Size = new System.Drawing.Size(634, 20);
			this.MidiBpmLbl.TabIndex = 10;
			this.MidiBpmLbl.Text = "设定 BPM 速度为";
			this.MidiBpmLbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// MidiBpmFlowPanel
			// 
			this.MidiBpmFlowPanel.AutoSize = true;
			this.MidiBpmFlowPanel.Controls.Add(this.MidiDynamicTempoFlow);
			this.MidiBpmFlowPanel.Controls.Add(this.MidiMidiBpmCheck);
			this.MidiBpmFlowPanel.Controls.Add(this.MidiProjectBpmCheck);
			this.MidiBpmFlowPanel.Controls.Add(this.flowLayoutPanel2);
			this.MidiBpmFlowPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.MidiBpmFlowPanel.Location = new System.Drawing.Point(2, 213);
			this.MidiBpmFlowPanel.Margin = new System.Windows.Forms.Padding(2);
			this.MidiBpmFlowPanel.Name = "MidiBpmFlowPanel";
			this.MidiBpmFlowPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.MidiBpmFlowPanel.Size = new System.Drawing.Size(634, 35);
			this.MidiBpmFlowPanel.TabIndex = 12;
			// 
			// MidiDynamicTempoFlow
			// 
			this.MidiDynamicTempoFlow.AutoSize = true;
			this.MidiDynamicTempoFlow.Controls.Add(this.MidiDynamicMidiBpmCheck);
			this.MidiDynamicTempoFlow.Controls.Add(this.MidiDynamicMidiBpmFormCombo);
			this.MidiDynamicTempoFlow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiDynamicTempoFlow.Location = new System.Drawing.Point(0, 2);
			this.MidiDynamicTempoFlow.Margin = new System.Windows.Forms.Padding(0);
			this.MidiDynamicTempoFlow.Name = "MidiDynamicTempoFlow";
			this.MidiDynamicTempoFlow.Size = new System.Drawing.Size(203, 31);
			this.MidiDynamicTempoFlow.TabIndex = 10;
			this.MidiDynamicTempoFlow.Visible = false;
			this.MidiDynamicTempoFlow.WrapContents = false;
			// 
			// MidiDynamicMidiBpmCheck
			// 
			this.MidiDynamicMidiBpmCheck.AutoSize = true;
			this.MidiDynamicMidiBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiDynamicMidiBpmCheck.Group = "BpmTempo";
			this.MidiDynamicMidiBpmCheck.Location = new System.Drawing.Point(2, 2);
			this.MidiDynamicMidiBpmCheck.Margin = new System.Windows.Forms.Padding(2);
			this.MidiDynamicMidiBpmCheck.Name = "MidiDynamicMidiBpmCheck";
			this.MidiDynamicMidiBpmCheck.Size = new System.Drawing.Size(130, 24);
			this.MidiDynamicMidiBpmCheck.TabIndex = 1;
			this.MidiDynamicMidiBpmCheck.TabStop = true;
			this.MidiDynamicMidiBpmCheck.Text = "动态 MIDI 速度";
			this.MidiDynamicMidiBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiDynamicMidiBpmFormCombo
			// 
			this.MidiDynamicMidiBpmFormCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiDynamicMidiBpmFormCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.MidiDynamicMidiBpmFormCombo.FormattingEnabled = true;
			this.MidiDynamicMidiBpmFormCombo.Items.AddRange(new object[] {
			"保留",
			"线性"});
			this.MidiDynamicMidiBpmFormCombo.Location = new System.Drawing.Point(134, 0);
			this.MidiDynamicMidiBpmFormCombo.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
			this.MidiDynamicMidiBpmFormCombo.Name = "MidiDynamicMidiBpmFormCombo";
			this.MidiDynamicMidiBpmFormCombo.Size = new System.Drawing.Size(65, 28);
			this.MidiDynamicMidiBpmFormCombo.TabIndex = 4;
			// 
			// MidiMidiBpmCheck
			// 
			this.MidiMidiBpmCheck.AutoSize = true;
			this.MidiMidiBpmCheck.Checked = true;
			this.MidiMidiBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiMidiBpmCheck.Enabled = false;
			this.MidiMidiBpmCheck.Group = "BpmTempo";
			this.MidiMidiBpmCheck.Location = new System.Drawing.Point(205, 4);
			this.MidiMidiBpmCheck.Margin = new System.Windows.Forms.Padding(2);
			this.MidiMidiBpmCheck.Name = "MidiMidiBpmCheck";
			this.MidiMidiBpmCheck.Size = new System.Drawing.Size(96, 27);
			this.MidiMidiBpmCheck.TabIndex = 1;
			this.MidiMidiBpmCheck.TabStop = true;
			this.MidiMidiBpmCheck.Text = "MIDI 速度";
			this.MidiMidiBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiProjectBpmCheck
			// 
			this.MidiProjectBpmCheck.AutoSize = true;
			this.MidiProjectBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiProjectBpmCheck.Enabled = false;
			this.MidiProjectBpmCheck.Group = "BpmTempo";
			this.MidiProjectBpmCheck.Location = new System.Drawing.Point(305, 4);
			this.MidiProjectBpmCheck.Margin = new System.Windows.Forms.Padding(2);
			this.MidiProjectBpmCheck.Name = "MidiProjectBpmCheck";
			this.MidiProjectBpmCheck.Size = new System.Drawing.Size(90, 27);
			this.MidiProjectBpmCheck.TabIndex = 2;
			this.MidiProjectBpmCheck.Text = "项目速度";
			this.MidiProjectBpmCheck.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel2
			// 
			this.flowLayoutPanel2.AutoSize = true;
			this.flowLayoutPanel2.Controls.Add(this.MidiCustomBpmCheck);
			this.flowLayoutPanel2.Controls.Add(this.MidiCustomBpmBox);
			this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel2.Location = new System.Drawing.Point(397, 2);
			this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel2.Name = "flowLayoutPanel2";
			this.flowLayoutPanel2.Size = new System.Drawing.Size(204, 31);
			this.flowLayoutPanel2.TabIndex = 9;
			this.flowLayoutPanel2.WrapContents = false;
			// 
			// MidiCustomBpmCheck
			// 
			this.MidiCustomBpmCheck.AutoSize = true;
			this.MidiCustomBpmCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiCustomBpmCheck.Enabled = false;
			this.MidiCustomBpmCheck.Group = "BpmTempo";
			this.MidiCustomBpmCheck.Location = new System.Drawing.Point(2, 2);
			this.MidiCustomBpmCheck.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.MidiCustomBpmCheck.Name = "MidiCustomBpmCheck";
			this.MidiCustomBpmCheck.Size = new System.Drawing.Size(75, 27);
			this.MidiCustomBpmCheck.TabIndex = 9;
			this.MidiCustomBpmCheck.Text = "自定义";
			this.MidiCustomBpmCheck.UseVisualStyleBackColor = true;
			// 
			// MidiCustomBpmBox
			// 
			this.MidiCustomBpmBox.Constrain = new decimal(new int[] {
			120,
			0,
			0,
			0});
			this.MidiCustomBpmBox.DecimalPlaces = 3;
			this.MidiCustomBpmBox.Enabled = false;
			this.MidiCustomBpmBox.Location = new System.Drawing.Point(77, 2);
			this.MidiCustomBpmBox.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
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
			this.MidiCustomBpmBox.Size = new System.Drawing.Size(125, 27);
			this.MidiCustomBpmBox.Suffix = "BPM";
			this.MidiCustomBpmBox.TabIndex = 10;
			this.MidiCustomBpmBox.Value = new decimal(new int[] {
			120,
			0,
			0,
			0});
			// 
			// tableLayoutPanel12
			// 
			this.tableLayoutPanel12.AutoSize = true;
			this.tableLayoutPanel12.ColumnCount = 2;
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel12.Controls.Add(this.MidiBeatLbl, 0, 0);
			this.tableLayoutPanel12.Controls.Add(this.MidiBeatTxt, 1, 0);
			this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel12.Location = new System.Drawing.Point(2, 119);
			this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(2, 8, 2, 8);
			this.tableLayoutPanel12.Name = "tableLayoutPanel12";
			this.tableLayoutPanel12.RowCount = 1;
			this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel12.Size = new System.Drawing.Size(634, 20);
			this.tableLayoutPanel12.TabIndex = 13;
			// 
			// MidiBeatLbl
			// 
			this.MidiBeatLbl.AutoSize = true;
			this.MidiBeatLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiBeatLbl.Location = new System.Drawing.Point(0, 0);
			this.MidiBeatLbl.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.MidiBeatLbl.Name = "MidiBeatLbl";
			this.MidiBeatLbl.Size = new System.Drawing.Size(69, 20);
			this.MidiBeatLbl.TabIndex = 3;
			this.MidiBeatLbl.Text = "节拍　　";
			this.MidiBeatLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// MidiBeatTxt
			// 
			this.MidiBeatTxt.AutoSize = true;
			this.MidiBeatTxt.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MidiBeatTxt.Location = new System.Drawing.Point(73, 0);
			this.MidiBeatTxt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.MidiBeatTxt.Name = "MidiBeatTxt";
			this.MidiBeatTxt.Size = new System.Drawing.Size(559, 20);
			this.MidiBeatTxt.TabIndex = 5;
			this.MidiBeatTxt.Text = "无";
			this.MidiBeatTxt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SourceConfigGroup
			// 
			this.SourceConfigGroup.AutoSize = true;
			this.SourceConfigGroup.Controls.Add(this.tableLayoutPanel3);
			this.SourceConfigGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.SourceConfigGroup.Location = new System.Drawing.Point(5, 5);
			this.SourceConfigGroup.Margin = new System.Windows.Forms.Padding(2);
			this.SourceConfigGroup.Name = "SourceConfigGroup";
			this.SourceConfigGroup.Padding = new System.Windows.Forms.Padding(5);
			this.SourceConfigGroup.Size = new System.Drawing.Size(648, 185);
			this.SourceConfigGroup.TabIndex = 1;
			this.SourceConfigGroup.TabStop = false;
			this.SourceConfigGroup.Text = "素材属性";
			this.SourceConfigGroup.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
			this.SourceConfigGroup.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
			this.SourceConfigGroup.DragLeave += new System.EventHandler(this.OnDragLeave);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.ChooseSourceLbl, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.GenerateAtLbl, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel9, 0, 4);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 5;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(638, 155);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// ChooseSourceLbl
			// 
			this.ChooseSourceLbl.AutoSize = true;
			this.ChooseSourceLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChooseSourceLbl.Location = new System.Drawing.Point(2, 0);
			this.ChooseSourceLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.ChooseSourceLbl.Name = "ChooseSourceLbl";
			this.ChooseSourceLbl.Size = new System.Drawing.Size(634, 20);
			this.ChooseSourceLbl.TabIndex = 0;
			this.ChooseSourceLbl.Text = "选择媒体素材";
			this.ChooseSourceLbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 20);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(638, 32);
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
			this.ChooseSourceCombo.Location = new System.Drawing.Point(2, 2);
			this.ChooseSourceCombo.Margin = new System.Windows.Forms.Padding(2);
			this.ChooseSourceCombo.Name = "ChooseSourceCombo";
			this.ChooseSourceCombo.Size = new System.Drawing.Size(555, 28);
			this.ChooseSourceCombo.TabIndex = 0;
			this.ChooseSourceCombo.SelectedIndexChanged += new System.EventHandler(this.ChooseSourceCombo_SelectedIndexChanged);
			// 
			// ChooseSourceBtn
			// 
			this.ChooseSourceBtn.Dock = System.Windows.Forms.DockStyle.Top;
			this.ChooseSourceBtn.Location = new System.Drawing.Point(561, 2);
			this.ChooseSourceBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ChooseSourceBtn.Name = "ChooseSourceBtn";
			this.ChooseSourceBtn.Size = new System.Drawing.Size(75, 28);
			this.ChooseSourceBtn.TabIndex = 1;
			this.ChooseSourceBtn.Text = "浏览...";
			this.ChooseSourceBtn.UseVisualStyleBackColor = true;
			this.ChooseSourceBtn.Click += new System.EventHandler(this.ChooseSourceBtn_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.Controls.Add(this.SourceStartTimeLbl);
			this.flowLayoutPanel1.Controls.Add(this.SourceStartTimeText);
			this.flowLayoutPanel1.Controls.Add(this.SourceEndTimeLbl);
			this.flowLayoutPanel1.Controls.Add(this.SourceEndTimeText);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 54);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(634, 31);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// SourceStartTimeLbl
			// 
			this.SourceStartTimeLbl.AutoSize = true;
			this.SourceStartTimeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SourceStartTimeLbl.Location = new System.Drawing.Point(0, 0);
			this.SourceStartTimeLbl.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.SourceStartTimeLbl.Name = "SourceStartTimeLbl";
			this.SourceStartTimeLbl.Size = new System.Drawing.Size(69, 31);
			this.SourceStartTimeLbl.TabIndex = 0;
			this.SourceStartTimeLbl.Text = "入点秒数";
			this.SourceStartTimeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SourceEndTimeLbl
			// 
			this.SourceEndTimeLbl.AutoSize = true;
			this.SourceEndTimeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SourceEndTimeLbl.Location = new System.Drawing.Point(243, 0);
			this.SourceEndTimeLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.SourceEndTimeLbl.Name = "SourceEndTimeLbl";
			this.SourceEndTimeLbl.Size = new System.Drawing.Size(69, 31);
			this.SourceEndTimeLbl.TabIndex = 2;
			this.SourceEndTimeLbl.Text = "出点秒数";
			this.SourceEndTimeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// GenerateAtLbl
			// 
			this.GenerateAtLbl.AutoSize = true;
			this.GenerateAtLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtLbl.Location = new System.Drawing.Point(2, 96);
			this.GenerateAtLbl.Margin = new System.Windows.Forms.Padding(2, 9, 2, 0);
			this.GenerateAtLbl.Name = "GenerateAtLbl";
			this.GenerateAtLbl.Size = new System.Drawing.Size(634, 20);
			this.GenerateAtLbl.TabIndex = 8;
			this.GenerateAtLbl.Text = "设定生成开始位置";
			this.GenerateAtLbl.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// flowLayoutPanel9
			// 
			this.flowLayoutPanel9.AutoSize = true;
			this.flowLayoutPanel9.Controls.Add(this.GenerateAtBeginRadio);
			this.flowLayoutPanel9.Controls.Add(this.GenerateAtCursorRadio);
			this.flowLayoutPanel9.Controls.Add(this.flowLayoutPanel11);
			this.flowLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel9.Location = new System.Drawing.Point(2, 118);
			this.flowLayoutPanel9.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel9.Name = "flowLayoutPanel9";
			this.flowLayoutPanel9.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.flowLayoutPanel9.Size = new System.Drawing.Size(634, 35);
			this.flowLayoutPanel9.TabIndex = 9;
			// 
			// GenerateAtBeginRadio
			// 
			this.GenerateAtBeginRadio.AutoSize = true;
			this.GenerateAtBeginRadio.Checked = true;
			this.GenerateAtBeginRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtBeginRadio.Group = "GenerateAt";
			this.GenerateAtBeginRadio.Location = new System.Drawing.Point(2, 4);
			this.GenerateAtBeginRadio.Margin = new System.Windows.Forms.Padding(2);
			this.GenerateAtBeginRadio.Name = "GenerateAtBeginRadio";
			this.GenerateAtBeginRadio.Size = new System.Drawing.Size(105, 27);
			this.GenerateAtBeginRadio.TabIndex = 0;
			this.GenerateAtBeginRadio.TabStop = true;
			this.GenerateAtBeginRadio.Text = "项目开始处";
			this.GenerateAtBeginRadio.UseVisualStyleBackColor = true;
			// 
			// GenerateAtCursorRadio
			// 
			this.GenerateAtCursorRadio.AutoSize = true;
			this.GenerateAtCursorRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtCursorRadio.Group = "GenerateAt";
			this.GenerateAtCursorRadio.Location = new System.Drawing.Point(111, 4);
			this.GenerateAtCursorRadio.Margin = new System.Windows.Forms.Padding(2);
			this.GenerateAtCursorRadio.Name = "GenerateAtCursorRadio";
			this.GenerateAtCursorRadio.Size = new System.Drawing.Size(75, 27);
			this.GenerateAtCursorRadio.TabIndex = 1;
			this.GenerateAtCursorRadio.Text = "光标处";
			this.GenerateAtCursorRadio.UseVisualStyleBackColor = true;
			// 
			// flowLayoutPanel11
			// 
			this.flowLayoutPanel11.AutoSize = true;
			this.flowLayoutPanel11.Controls.Add(this.GenerateAtCustomRadio);
			this.flowLayoutPanel11.Controls.Add(this.GenerateAtCustomText);
			this.flowLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel11.Location = new System.Drawing.Point(188, 2);
			this.flowLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel11.Name = "flowLayoutPanel11";
			this.flowLayoutPanel11.Size = new System.Drawing.Size(204, 31);
			this.flowLayoutPanel11.TabIndex = 5;
			this.flowLayoutPanel11.WrapContents = false;
			// 
			// GenerateAtCustomRadio
			// 
			this.GenerateAtCustomRadio.AutoSize = true;
			this.GenerateAtCustomRadio.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GenerateAtCustomRadio.Group = "GenerateAt";
			this.GenerateAtCustomRadio.Location = new System.Drawing.Point(2, 2);
			this.GenerateAtCustomRadio.Margin = new System.Windows.Forms.Padding(2, 2, 0, 2);
			this.GenerateAtCustomRadio.Name = "GenerateAtCustomRadio";
			this.GenerateAtCustomRadio.Size = new System.Drawing.Size(75, 27);
			this.GenerateAtCustomRadio.TabIndex = 5;
			this.GenerateAtCustomRadio.Text = "自定义";
			this.GenerateAtCustomRadio.UseVisualStyleBackColor = true;
			// 
			// GenerateAtCustomText
			// 
			this.GenerateAtCustomText.Enabled = false;
			this.GenerateAtCustomText.Location = new System.Drawing.Point(77, 2);
			this.GenerateAtCustomText.Margin = new System.Windows.Forms.Padding(0, 2, 2, 2);
			this.GenerateAtCustomText.Name = "GenerateAtCustomText";
			this.GenerateAtCustomText.Size = new System.Drawing.Size(125, 27);
			this.GenerateAtCustomText.TabIndex = 6;
			// 
			// AudioTab
			// 
			this.AudioTab.AutoScroll = true;
			this.AudioTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			this.AudioTab.Controls.Add(this.AudioParamsGroup);
			this.AudioTab.Controls.Add(this.AudioTuneGroup);
			this.AudioTab.Controls.Add(this.flowLayoutPanel5);
			this.AudioTab.Location = new System.Drawing.Point(4, 29);
			this.AudioTab.Margin = new System.Windows.Forms.Padding(2);
			this.AudioTab.Name = "AudioTab";
			this.AudioTab.Padding = new System.Windows.Forms.Padding(5);
			this.AudioTab.Size = new System.Drawing.Size(658, 607);
			this.AudioTab.TabIndex = 1;
			this.AudioTab.Text = "音频";
			this.AudioTab.UseVisualStyleBackColor = true;
			// 
			// AudioParamsGroup
			// 
			this.AudioParamsGroup.AutoSize = true;
			this.AudioParamsGroup.Controls.Add(this.tableLayoutPanel2);
			this.AudioParamsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.AudioParamsGroup.Location = new System.Drawing.Point(5, 292);
			this.AudioParamsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.AudioParamsGroup.Name = "AudioParamsGroup";
			this.AudioParamsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.AudioParamsGroup.Size = new System.Drawing.Size(648, 112);
			this.AudioParamsGroup.TabIndex = 2;
			this.AudioParamsGroup.TabStop = false;
			this.AudioParamsGroup.Text = "参数";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeInLbl, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeInBox, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeInCurveCombo, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeOutLbl, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeOutBox, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.AudioFadeOutCurveCombo, 2, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(638, 82);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// AudioFadeInLbl
			// 
			this.AudioFadeInLbl.AutoSize = true;
			this.AudioFadeInLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeInLbl.Location = new System.Drawing.Point(2, 0);
			this.AudioFadeInLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioFadeInLbl.Name = "AudioFadeInLbl";
			this.AudioFadeInLbl.Size = new System.Drawing.Size(69, 41);
			this.AudioFadeInLbl.TabIndex = 0;
			this.AudioFadeInLbl.Text = "渐入　　";
			this.AudioFadeInLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioFadeInBox
			// 
			this.AudioFadeInBox.BackColor = System.Drawing.Color.Transparent;
			this.AudioFadeInBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeInBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.AudioFadeInBox.Location = new System.Drawing.Point(78, 5);
			this.AudioFadeInBox.Margin = new System.Windows.Forms.Padding(5);
			this.AudioFadeInBox.Name = "AudioFadeInBox";
			this.AudioFadeInBox.NumericUpDownWidth = 65;
			this.AudioFadeInBox.Size = new System.Drawing.Size(486, 31);
			this.AudioFadeInBox.TabIndex = 2;
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
			this.AudioFadeInCurveCombo.Location = new System.Drawing.Point(571, 4);
			this.AudioFadeInCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.AudioFadeInCurveCombo.Name = "AudioFadeInCurveCombo";
			this.AudioFadeInCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.AudioFadeInCurveCombo.TabIndex = 3;
			// 
			// AudioFadeOutLbl
			// 
			this.AudioFadeOutLbl.AutoSize = true;
			this.AudioFadeOutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeOutLbl.Location = new System.Drawing.Point(2, 41);
			this.AudioFadeOutLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioFadeOutLbl.Name = "AudioFadeOutLbl";
			this.AudioFadeOutLbl.Size = new System.Drawing.Size(69, 41);
			this.AudioFadeOutLbl.TabIndex = 1;
			this.AudioFadeOutLbl.Text = "渐出　　";
			this.AudioFadeOutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioFadeOutBox
			// 
			this.AudioFadeOutBox.BackColor = System.Drawing.Color.Transparent;
			this.AudioFadeOutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioFadeOutBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.AudioFadeOutBox.Location = new System.Drawing.Point(78, 46);
			this.AudioFadeOutBox.Margin = new System.Windows.Forms.Padding(5);
			this.AudioFadeOutBox.Name = "AudioFadeOutBox";
			this.AudioFadeOutBox.NumericUpDownWidth = 65;
			this.AudioFadeOutBox.Size = new System.Drawing.Size(486, 31);
			this.AudioFadeOutBox.TabIndex = 4;
			this.AudioFadeOutBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
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
			this.AudioFadeOutCurveCombo.Location = new System.Drawing.Point(571, 45);
			this.AudioFadeOutCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.AudioFadeOutCurveCombo.Name = "AudioFadeOutCurveCombo";
			this.AudioFadeOutCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.AudioFadeOutCurveCombo.TabIndex = 5;
			// 
			// AudioTuneGroup
			// 
			this.AudioTuneGroup.AutoSize = true;
			this.AudioTuneGroup.Controls.Add(this.AudioTuneTablePanel);
			this.AudioTuneGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.AudioTuneGroup.Location = new System.Drawing.Point(5, 65);
			this.AudioTuneGroup.Margin = new System.Windows.Forms.Padding(2);
			this.AudioTuneGroup.Name = "AudioTuneGroup";
			this.AudioTuneGroup.Padding = new System.Windows.Forms.Padding(5);
			this.AudioTuneGroup.Size = new System.Drawing.Size(648, 227);
			this.AudioTuneGroup.TabIndex = 1;
			this.AudioTuneGroup.TabStop = false;
			this.AudioTuneGroup.Text = "调音";
			// 
			// AudioTuneTablePanel
			// 
			this.AudioTuneTablePanel.AutoSize = true;
			this.AudioTuneTablePanel.ColumnCount = 2;
			this.AudioTuneTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.AudioTuneTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.AudioTuneTablePanel.Controls.Add(this.AudioTuneMethodLbl, 0, 0);
			this.AudioTuneTablePanel.Controls.Add(this.AudioTuneMethodCombo, 1, 0);
			this.AudioTuneTablePanel.Controls.Add(this.AudioStretchAttrLbl, 0, 1);
			this.AudioTuneTablePanel.Controls.Add(this.AudioStretchAttrCombo, 1, 1);
			this.AudioTuneTablePanel.Controls.Add(this.AudioLockAttrLbl, 0, 2);
			this.AudioTuneTablePanel.Controls.Add(this.flowLayoutPanel10, 1, 2);
			this.AudioTuneTablePanel.Controls.Add(this.AudioBasePitchLbl, 0, 3);
			this.AudioTuneTablePanel.Controls.Add(this.flowLayoutPanel6, 1, 3);
			this.AudioTuneTablePanel.Controls.Add(this.AudioPreviewLbl, 0, 4);
			this.AudioTuneTablePanel.Controls.Add(this.tableLayoutPanel17, 1, 4);
			this.AudioTuneTablePanel.Controls.Add(this.AudioPreviewAttrLbl, 0, 5);
			this.AudioTuneTablePanel.Controls.Add(this.AudioPreviewAttrLayoutPanel, 1, 5);
			this.AudioTuneTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioTuneTablePanel.Location = new System.Drawing.Point(5, 25);
			this.AudioTuneTablePanel.Margin = new System.Windows.Forms.Padding(4);
			this.AudioTuneTablePanel.Name = "AudioTuneTablePanel";
			this.AudioTuneTablePanel.RowCount = 6;
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.AudioTuneTablePanel.Size = new System.Drawing.Size(638, 197);
			this.AudioTuneTablePanel.TabIndex = 2;
			// 
			// AudioTuneMethodLbl
			// 
			this.AudioTuneMethodLbl.AutoSize = true;
			this.AudioTuneMethodLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioTuneMethodLbl.Location = new System.Drawing.Point(2, 0);
			this.AudioTuneMethodLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioTuneMethodLbl.Name = "AudioTuneMethodLbl";
			this.AudioTuneMethodLbl.Size = new System.Drawing.Size(69, 35);
			this.AudioTuneMethodLbl.TabIndex = 0;
			this.AudioTuneMethodLbl.Text = "调音方法";
			this.AudioTuneMethodLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioStretchAttrLbl
			// 
			this.AudioStretchAttrLbl.AutoSize = true;
			this.AudioStretchAttrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioStretchAttrLbl.Location = new System.Drawing.Point(2, 35);
			this.AudioStretchAttrLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioStretchAttrLbl.Name = "AudioStretchAttrLbl";
			this.AudioStretchAttrLbl.Size = new System.Drawing.Size(69, 35);
			this.AudioStretchAttrLbl.TabIndex = 6;
			this.AudioStretchAttrLbl.Text = "拉伸属性";
			this.AudioStretchAttrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioLockAttrLbl
			// 
			this.AudioLockAttrLbl.AutoSize = true;
			this.AudioLockAttrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioLockAttrLbl.Location = new System.Drawing.Point(2, 70);
			this.AudioLockAttrLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioLockAttrLbl.Name = "AudioLockAttrLbl";
			this.AudioLockAttrLbl.Size = new System.Drawing.Size(69, 28);
			this.AudioLockAttrLbl.TabIndex = 8;
			this.AudioLockAttrLbl.Text = "锁定属性";
			this.AudioLockAttrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutPanel10
			// 
			this.flowLayoutPanel10.AutoSize = true;
			this.flowLayoutPanel10.Controls.Add(this.AudioLockStretchPitchCheck);
			this.flowLayoutPanel10.Controls.Add(this.AudioReserveFormantCheck);
			this.flowLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel10.Location = new System.Drawing.Point(73, 70);
			this.flowLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel10.Name = "flowLayoutPanel10";
			this.flowLayoutPanel10.Size = new System.Drawing.Size(565, 28);
			this.flowLayoutPanel10.TabIndex = 4;
			// 
			// AudioReserveFormantCheck
			// 
			this.AudioReserveFormantCheck.AutoSize = true;
			this.AudioReserveFormantCheck.Location = new System.Drawing.Point(142, 2);
			this.AudioReserveFormantCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioReserveFormantCheck.Name = "AudioReserveFormantCheck";
			this.AudioReserveFormantCheck.Size = new System.Drawing.Size(106, 24);
			this.AudioReserveFormantCheck.TabIndex = 1;
			this.AudioReserveFormantCheck.Text = "保持共振峰";
			this.AudioReserveFormantCheck.UseVisualStyleBackColor = true;
			// 
			// AudioBasePitchLbl
			// 
			this.AudioBasePitchLbl.AutoSize = true;
			this.AudioBasePitchLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioBasePitchLbl.Location = new System.Drawing.Point(2, 98);
			this.AudioBasePitchLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioBasePitchLbl.Name = "AudioBasePitchLbl";
			this.AudioBasePitchLbl.Size = new System.Drawing.Size(69, 32);
			this.AudioBasePitchLbl.TabIndex = 1;
			this.AudioBasePitchLbl.Text = "原始音高";
			this.AudioBasePitchLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// flowLayoutPanel6
			// 
			this.flowLayoutPanel6.AutoSize = true;
			this.flowLayoutPanel6.Controls.Add(this.AudioMainKeyCombo);
			this.flowLayoutPanel6.Controls.Add(this.AudioMainOctaveCombo);
			this.flowLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel6.Location = new System.Drawing.Point(73, 98);
			this.flowLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel6.Name = "flowLayoutPanel6";
			this.flowLayoutPanel6.Size = new System.Drawing.Size(565, 32);
			this.flowLayoutPanel6.TabIndex = 5;
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
			this.AudioMainKeyCombo.Location = new System.Drawing.Point(2, 2);
			this.AudioMainKeyCombo.Margin = new System.Windows.Forms.Padding(2);
			this.AudioMainKeyCombo.Name = "AudioMainKeyCombo";
			this.AudioMainKeyCombo.Size = new System.Drawing.Size(60, 28);
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
			this.AudioMainOctaveCombo.Location = new System.Drawing.Point(66, 2);
			this.AudioMainOctaveCombo.Margin = new System.Windows.Forms.Padding(2);
			this.AudioMainOctaveCombo.Name = "AudioMainOctaveCombo";
			this.AudioMainOctaveCombo.Size = new System.Drawing.Size(60, 28);
			this.AudioMainOctaveCombo.TabIndex = 4;
			// 
			// AudioPreviewLbl
			// 
			this.AudioPreviewLbl.AutoSize = true;
			this.AudioPreviewLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioPreviewLbl.Location = new System.Drawing.Point(2, 130);
			this.AudioPreviewLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioPreviewLbl.Name = "AudioPreviewLbl";
			this.AudioPreviewLbl.Size = new System.Drawing.Size(69, 35);
			this.AudioPreviewLbl.TabIndex = 9;
			this.AudioPreviewLbl.Text = "预听";
			this.AudioPreviewLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tableLayoutPanel17
			// 
			this.tableLayoutPanel17.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel17.ColumnCount = 2;
			this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.19084F));
			this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.80916F));
			this.tableLayoutPanel17.Controls.Add(this.PreviewBasePitchBtn, 0, 0);
			this.tableLayoutPanel17.Controls.Add(this.PreviewAudioBtn, 1, 0);
			this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel17.Location = new System.Drawing.Point(73, 130);
			this.tableLayoutPanel17.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel17.Name = "tableLayoutPanel17";
			this.tableLayoutPanel17.RowCount = 1;
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel17.Size = new System.Drawing.Size(565, 35);
			this.tableLayoutPanel17.TabIndex = 6;
			// 
			// PreviewAudioBtn
			// 
			this.PreviewAudioBtn.AutoSize = true;
			this.PreviewAudioBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewAudioBtn.Location = new System.Drawing.Point(285, 2);
			this.PreviewAudioBtn.Margin = new System.Windows.Forms.Padding(2);
			this.PreviewAudioBtn.Name = "PreviewAudioBtn";
			this.PreviewAudioBtn.Size = new System.Drawing.Size(278, 31);
			this.PreviewAudioBtn.TabIndex = 2;
			this.PreviewAudioBtn.Text = "预听音频(&P)";
			this.PreviewAudioBtn.UseVisualStyleBackColor = true;
			this.PreviewAudioBtn.Click += new System.EventHandler(this.PreviewAudioBtn_Click);
			// 
			// AudioPreviewAttrLbl
			// 
			this.AudioPreviewAttrLbl.AutoSize = true;
			this.AudioPreviewAttrLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioPreviewAttrLbl.Location = new System.Drawing.Point(2, 165);
			this.AudioPreviewAttrLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.AudioPreviewAttrLbl.Name = "AudioPreviewAttrLbl";
			this.AudioPreviewAttrLbl.Size = new System.Drawing.Size(69, 32);
			this.AudioPreviewAttrLbl.TabIndex = 10;
			this.AudioPreviewAttrLbl.Text = "预听属性";
			this.AudioPreviewAttrLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AudioPreviewAttrLayoutPanel
			// 
			this.AudioPreviewAttrLayoutPanel.AutoSize = true;
			this.AudioPreviewAttrLayoutPanel.Controls.Add(this.PreviewBeepEngineCombo);
			this.AudioPreviewAttrLayoutPanel.Controls.Add(this.PreviewBeepWaveFormCombo);
			this.AudioPreviewAttrLayoutPanel.Controls.Add(this.PreviewBeepDurationBox);
			this.AudioPreviewAttrLayoutPanel.Controls.Add(this.PreviewTuneAudioCheck);
			this.AudioPreviewAttrLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AudioPreviewAttrLayoutPanel.Location = new System.Drawing.Point(73, 165);
			this.AudioPreviewAttrLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.AudioPreviewAttrLayoutPanel.Name = "AudioPreviewAttrLayoutPanel";
			this.AudioPreviewAttrLayoutPanel.Size = new System.Drawing.Size(565, 32);
			this.AudioPreviewAttrLayoutPanel.TabIndex = 7;
			// 
			// PreviewBeepEngineCombo
			// 
			this.PreviewBeepEngineCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PreviewBeepEngineCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PreviewBeepEngineCombo.FormattingEnabled = true;
			this.PreviewBeepEngineCombo.Items.AddRange(new object[] {
			"ConsoleBeep",
			"PowerShell",
			"NAudio"});
			this.PreviewBeepEngineCombo.Location = new System.Drawing.Point(2, 2);
			this.PreviewBeepEngineCombo.Margin = new System.Windows.Forms.Padding(2);
			this.PreviewBeepEngineCombo.Name = "PreviewBeepEngineCombo";
			this.PreviewBeepEngineCombo.Size = new System.Drawing.Size(110, 28);
			this.PreviewBeepEngineCombo.TabIndex = 0;
			// 
			// PreviewBeepWaveFormCombo
			// 
			this.PreviewBeepWaveFormCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.PreviewBeepWaveFormCombo.FormattingEnabled = true;
			this.PreviewBeepWaveFormCombo.Items.AddRange(new object[] {
			"正弦波",
			"三角波",
			"方波",
			"锯齿波"});
			this.PreviewBeepWaveFormCombo.Location = new System.Drawing.Point(116, 2);
			this.PreviewBeepWaveFormCombo.Margin = new System.Windows.Forms.Padding(2);
			this.PreviewBeepWaveFormCombo.Name = "PreviewBeepWaveFormCombo";
			this.PreviewBeepWaveFormCombo.Size = new System.Drawing.Size(100, 28);
			this.PreviewBeepWaveFormCombo.TabIndex = 3;
			// 
			// flowLayoutPanel5
			// 
			this.flowLayoutPanel5.AutoSize = true;
			this.flowLayoutPanel5.Controls.Add(this.AudioConfigCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioScratchCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioLoopCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioNormalizeCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioFreezeLastFrameCheck);
			this.flowLayoutPanel5.Controls.Add(this.AudioLegatoCheck);
			this.flowLayoutPanel5.Controls.Add(this.CreateEventGroupCheck);
			this.flowLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel5.Location = new System.Drawing.Point(5, 5);
			this.flowLayoutPanel5.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel5.Name = "flowLayoutPanel5";
			this.flowLayoutPanel5.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.flowLayoutPanel5.Size = new System.Drawing.Size(648, 60);
			this.flowLayoutPanel5.TabIndex = 0;
			// 
			// AudioConfigCheck
			// 
			this.AudioConfigCheck.AutoSize = true;
			this.AudioConfigCheck.Checked = true;
			this.AudioConfigCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AudioConfigCheck.Location = new System.Drawing.Point(2, 4);
			this.AudioConfigCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioConfigCheck.Name = "AudioConfigCheck";
			this.AudioConfigCheck.Size = new System.Drawing.Size(91, 24);
			this.AudioConfigCheck.TabIndex = 0;
			this.AudioConfigCheck.Text = "生成音频";
			this.AudioConfigCheck.UseVisualStyleBackColor = true;
			// 
			// AudioScratchCheck
			// 
			this.AudioScratchCheck.AutoSize = true;
			this.AudioScratchCheck.Location = new System.Drawing.Point(97, 4);
			this.AudioScratchCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioScratchCheck.Name = "AudioScratchCheck";
			this.AudioScratchCheck.Size = new System.Drawing.Size(91, 24);
			this.AudioScratchCheck.TabIndex = 1;
			this.AudioScratchCheck.Text = "拉伸音频";
			this.AudioScratchCheck.UseVisualStyleBackColor = true;
			// 
			// AudioLoopCheck
			// 
			this.AudioLoopCheck.AutoSize = true;
			this.AudioLoopCheck.Location = new System.Drawing.Point(192, 4);
			this.AudioLoopCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioLoopCheck.Name = "AudioLoopCheck";
			this.AudioLoopCheck.Size = new System.Drawing.Size(91, 24);
			this.AudioLoopCheck.TabIndex = 2;
			this.AudioLoopCheck.Text = "循环音频";
			this.AudioLoopCheck.UseVisualStyleBackColor = true;
			// 
			// AudioNormalizeCheck
			// 
			this.AudioNormalizeCheck.AutoSize = true;
			this.AudioNormalizeCheck.Checked = true;
			this.AudioNormalizeCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AudioNormalizeCheck.Location = new System.Drawing.Point(287, 4);
			this.AudioNormalizeCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioNormalizeCheck.Name = "AudioNormalizeCheck";
			this.AudioNormalizeCheck.Size = new System.Drawing.Size(106, 24);
			this.AudioNormalizeCheck.TabIndex = 3;
			this.AudioNormalizeCheck.Text = "规范化音量";
			this.AudioNormalizeCheck.UseVisualStyleBackColor = true;
			// 
			// AudioFreezeLastFrameCheck
			// 
			this.AudioFreezeLastFrameCheck.AutoSize = true;
			this.AudioFreezeLastFrameCheck.Location = new System.Drawing.Point(397, 4);
			this.AudioFreezeLastFrameCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioFreezeLastFrameCheck.Name = "AudioFreezeLastFrameCheck";
			this.AudioFreezeLastFrameCheck.Size = new System.Drawing.Size(91, 24);
			this.AudioFreezeLastFrameCheck.TabIndex = 6;
			this.AudioFreezeLastFrameCheck.Text = "禁止延长";
			this.AudioFreezeLastFrameCheck.UseVisualStyleBackColor = true;
			this.AudioFreezeLastFrameCheck.CheckedChanged += new System.EventHandler(this.AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged);
			// 
			// AudioLegatoCheck
			// 
			this.AudioLegatoCheck.AutoSize = true;
			this.AudioLegatoCheck.Location = new System.Drawing.Point(492, 4);
			this.AudioLegatoCheck.Margin = new System.Windows.Forms.Padding(2);
			this.AudioLegatoCheck.Name = "AudioLegatoCheck";
			this.AudioLegatoCheck.Size = new System.Drawing.Size(91, 24);
			this.AudioLegatoCheck.TabIndex = 7;
			this.AudioLegatoCheck.Text = "填补间隙";
			this.AudioLegatoCheck.UseVisualStyleBackColor = true;
			this.AudioLegatoCheck.CheckedChanged += new System.EventHandler(this.AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged);
			// 
			// CreateEventGroupCheck
			// 
			this.CreateEventGroupCheck.AutoSize = true;
			this.CreateEventGroupCheck.Location = new System.Drawing.Point(2, 32);
			this.CreateEventGroupCheck.Margin = new System.Windows.Forms.Padding(2);
			this.CreateEventGroupCheck.Name = "CreateEventGroupCheck";
			this.CreateEventGroupCheck.Size = new System.Drawing.Size(91, 24);
			this.CreateEventGroupCheck.TabIndex = 8;
			this.CreateEventGroupCheck.Text = "创建分组";
			this.CreateEventGroupCheck.UseVisualStyleBackColor = true;
			// 
			// VideoTab
			// 
			this.VideoTab.AutoScroll = true;
			this.VideoTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			this.VideoTab.Controls.Add(this.VideoParamsGroup);
			this.VideoTab.Controls.Add(this.VideoEffectsGroup);
			this.VideoTab.Controls.Add(this.flowLayoutPanel7);
			this.VideoTab.Location = new System.Drawing.Point(4, 29);
			this.VideoTab.Margin = new System.Windows.Forms.Padding(2);
			this.VideoTab.Name = "VideoTab";
			this.VideoTab.Padding = new System.Windows.Forms.Padding(5);
			this.VideoTab.Size = new System.Drawing.Size(658, 607);
			this.VideoTab.TabIndex = 2;
			this.VideoTab.Text = "视频";
			this.VideoTab.UseVisualStyleBackColor = true;
			// 
			// VideoParamsGroup
			// 
			this.VideoParamsGroup.AutoSize = true;
			this.VideoParamsGroup.Controls.Add(this.tableLayoutPanel9);
			this.VideoParamsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.VideoParamsGroup.Location = new System.Drawing.Point(5, 166);
			this.VideoParamsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.VideoParamsGroup.Name = "VideoParamsGroup";
			this.VideoParamsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.VideoParamsGroup.Size = new System.Drawing.Size(627, 850);
			this.VideoParamsGroup.TabIndex = 3;
			this.VideoParamsGroup.TabStop = false;
			this.VideoParamsGroup.Text = "参数";
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.ColumnCount = 3;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.VideoStartThresholdCurveCombo, 2, 18);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSaturationCurveCombo, 2, 14);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartContrastCurveCombo, 2, 16);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHueCurveCombo, 2, 12);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndContrastBox, 1, 17);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndSaturationBox, 1, 15);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndHueBox, 1, 13);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndThresholdBox, 1, 19);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartContrastBox, 1, 16);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSaturationBox, 1, 14);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartThresholdBox, 1, 18);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHueBox, 1, 12);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartThresholdLbl, 0, 18);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndThresholdLbl, 0, 19);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndSaturationLbl, 0, 15);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndContrastLbl, 0, 17);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartContrastLbl, 0, 16);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHueLbl, 0, 12);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSaturationLbl, 0, 14);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndHueLbl, 0, 13);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeInLbl, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeInBox, 1, 0);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeInCurveCombo, 2, 0);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeOutLbl, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeOutBox, 1, 1);
			this.tableLayoutPanel9.Controls.Add(this.VideoFadeOutCurveCombo, 2, 1);
			this.tableLayoutPanel9.Controls.Add(this.VideoGlowLbl, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.VideoGlowBox, 1, 2);
			this.tableLayoutPanel9.Controls.Add(this.VideoGlowCurveCombo, 2, 2);
			this.tableLayoutPanel9.Controls.Add(this.VideoGlowBrightLbl, 0, 3);
			this.tableLayoutPanel9.Controls.Add(this.VideoGlowBrightBox, 1, 3);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSizeLbl, 0, 4);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSizeBox, 1, 4);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartSizeCurveCombo, 2, 4);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndSizeLbl, 0, 5);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndSizeBox, 1, 5);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartRotationLbl, 0, 6);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartRotationBox, 1, 6);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndRotationLbl, 0, 7);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndRotationBox, 1, 7);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHorizontalTransLbl, 0, 8);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartHorizontalTransBox, 1, 8);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndHorizontalTransLbl, 0, 9);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndHorizontalTransBox, 1, 9);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartVerticalTransLbl, 0, 10);
			this.tableLayoutPanel9.Controls.Add(this.VideoStartVerticalTransBox, 1, 10);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndVerticalTransLbl, 0, 11);
			this.tableLayoutPanel9.Controls.Add(this.VideoEndVerticalTransBox, 1, 11);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel9.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 20;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.Size = new System.Drawing.Size(617, 820);
			this.tableLayoutPanel9.TabIndex = 0;
			// 
			// VideoStartThresholdCurveCombo
			// 
			this.VideoStartThresholdCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartThresholdCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoStartThresholdCurveCombo.FormattingEnabled = true;
			this.VideoStartThresholdCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoStartThresholdCurveCombo.Location = new System.Drawing.Point(550, 742);
			this.VideoStartThresholdCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoStartThresholdCurveCombo.Name = "VideoStartThresholdCurveCombo";
			this.VideoStartThresholdCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoStartThresholdCurveCombo.TabIndex = 43;
			// 
			// VideoStartSaturationCurveCombo
			// 
			this.VideoStartSaturationCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSaturationCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoStartSaturationCurveCombo.FormattingEnabled = true;
			this.VideoStartSaturationCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoStartSaturationCurveCombo.Location = new System.Drawing.Point(550, 578);
			this.VideoStartSaturationCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoStartSaturationCurveCombo.Name = "VideoStartSaturationCurveCombo";
			this.VideoStartSaturationCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoStartSaturationCurveCombo.TabIndex = 37;
			// 
			// VideoStartContrastCurveCombo
			// 
			this.VideoStartContrastCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartContrastCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoStartContrastCurveCombo.FormattingEnabled = true;
			this.VideoStartContrastCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoStartContrastCurveCombo.Location = new System.Drawing.Point(550, 660);
			this.VideoStartContrastCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoStartContrastCurveCombo.Name = "VideoStartContrastCurveCombo";
			this.VideoStartContrastCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoStartContrastCurveCombo.TabIndex = 40;
			// 
			// VideoStartHueCurveCombo
			// 
			this.VideoStartHueCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHueCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoStartHueCurveCombo.FormattingEnabled = true;
			this.VideoStartHueCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoStartHueCurveCombo.Location = new System.Drawing.Point(550, 496);
			this.VideoStartHueCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoStartHueCurveCombo.Name = "VideoStartHueCurveCombo";
			this.VideoStartHueCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoStartHueCurveCombo.TabIndex = 34;
			// 
			// VideoEndContrastBox
			// 
			this.VideoEndContrastBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndContrastBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndContrastBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndContrastBox.Location = new System.Drawing.Point(78, 702);
			this.VideoEndContrastBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndContrastBox.Minimum = -100;
			this.VideoEndContrastBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndContrastBox.Name = "VideoEndContrastBox";
			this.VideoEndContrastBox.NumericUpDownWidth = 65;
			this.VideoEndContrastBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndContrastBox.TabIndex = 41;
			this.VideoEndContrastBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoEndSaturationBox
			// 
			this.VideoEndSaturationBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndSaturationBox.DefaultValue = 100;
			this.VideoEndSaturationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndSaturationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndSaturationBox.Location = new System.Drawing.Point(78, 620);
			this.VideoEndSaturationBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndSaturationBox.Maximum = 200;
			this.VideoEndSaturationBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndSaturationBox.Name = "VideoEndSaturationBox";
			this.VideoEndSaturationBox.NumericUpDownWidth = 65;
			this.VideoEndSaturationBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndSaturationBox.TabIndex = 38;
			this.VideoEndSaturationBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.VideoEndSaturationBox.Value = 100;
			// 
			// VideoEndHueBox
			// 
			this.VideoEndHueBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndHueBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndHueBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndHueBox.Location = new System.Drawing.Point(78, 538);
			this.VideoEndHueBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndHueBox.Maximum = 360;
			this.VideoEndHueBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndHueBox.Name = "VideoEndHueBox";
			this.VideoEndHueBox.NumericUpDownWidth = 65;
			this.VideoEndHueBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndHueBox.TabIndex = 35;
			this.VideoEndHueBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoEndThresholdBox
			// 
			this.VideoEndThresholdBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndThresholdBox.DefaultValue = 50;
			this.VideoEndThresholdBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndThresholdBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndThresholdBox.Location = new System.Drawing.Point(78, 784);
			this.VideoEndThresholdBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndThresholdBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndThresholdBox.Name = "VideoEndThresholdBox";
			this.VideoEndThresholdBox.NumericUpDownWidth = 65;
			this.VideoEndThresholdBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndThresholdBox.TabIndex = 44;
			this.VideoEndThresholdBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.VideoEndThresholdBox.Value = 50;
			// 
			// VideoStartContrastBox
			// 
			this.VideoStartContrastBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartContrastBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartContrastBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartContrastBox.Location = new System.Drawing.Point(78, 661);
			this.VideoStartContrastBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartContrastBox.Minimum = -100;
			this.VideoStartContrastBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartContrastBox.Name = "VideoStartContrastBox";
			this.VideoStartContrastBox.NumericUpDownWidth = 65;
			this.VideoStartContrastBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartContrastBox.TabIndex = 39;
			// 
			// VideoStartSaturationBox
			// 
			this.VideoStartSaturationBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartSaturationBox.DefaultValue = 100;
			this.VideoStartSaturationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSaturationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartSaturationBox.Location = new System.Drawing.Point(78, 579);
			this.VideoStartSaturationBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartSaturationBox.Maximum = 200;
			this.VideoStartSaturationBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartSaturationBox.Name = "VideoStartSaturationBox";
			this.VideoStartSaturationBox.NumericUpDownWidth = 65;
			this.VideoStartSaturationBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartSaturationBox.TabIndex = 36;
			this.VideoStartSaturationBox.Value = 100;
			// 
			// VideoStartThresholdBox
			// 
			this.VideoStartThresholdBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartThresholdBox.DefaultValue = 50;
			this.VideoStartThresholdBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartThresholdBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartThresholdBox.Location = new System.Drawing.Point(78, 743);
			this.VideoStartThresholdBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartThresholdBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartThresholdBox.Name = "VideoStartThresholdBox";
			this.VideoStartThresholdBox.NumericUpDownWidth = 65;
			this.VideoStartThresholdBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartThresholdBox.TabIndex = 42;
			this.VideoStartThresholdBox.Value = 50;
			// 
			// VideoStartHueBox
			// 
			this.VideoStartHueBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartHueBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHueBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartHueBox.Location = new System.Drawing.Point(78, 497);
			this.VideoStartHueBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartHueBox.Maximum = 360;
			this.VideoStartHueBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartHueBox.Name = "VideoStartHueBox";
			this.VideoStartHueBox.NumericUpDownWidth = 65;
			this.VideoStartHueBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartHueBox.TabIndex = 33;
			// 
			// VideoStartThresholdLbl
			// 
			this.VideoStartThresholdLbl.AutoSize = true;
			this.VideoStartThresholdLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartThresholdLbl.Location = new System.Drawing.Point(2, 738);
			this.VideoStartThresholdLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartThresholdLbl.Name = "VideoStartThresholdLbl";
			this.VideoStartThresholdLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartThresholdLbl.TabIndex = 32;
			this.VideoStartThresholdLbl.Text = "起始阈值";
			this.VideoStartThresholdLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndThresholdLbl
			// 
			this.VideoEndThresholdLbl.AutoSize = true;
			this.VideoEndThresholdLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndThresholdLbl.Location = new System.Drawing.Point(2, 779);
			this.VideoEndThresholdLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndThresholdLbl.Name = "VideoEndThresholdLbl";
			this.VideoEndThresholdLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndThresholdLbl.TabIndex = 31;
			this.VideoEndThresholdLbl.Text = "终止阈值";
			this.VideoEndThresholdLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndSaturationLbl
			// 
			this.VideoEndSaturationLbl.AutoSize = true;
			this.VideoEndSaturationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndSaturationLbl.Location = new System.Drawing.Point(2, 615);
			this.VideoEndSaturationLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndSaturationLbl.Name = "VideoEndSaturationLbl";
			this.VideoEndSaturationLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndSaturationLbl.TabIndex = 30;
			this.VideoEndSaturationLbl.Text = "终止饱和";
			this.VideoEndSaturationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndContrastLbl
			// 
			this.VideoEndContrastLbl.AutoSize = true;
			this.VideoEndContrastLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndContrastLbl.Location = new System.Drawing.Point(2, 697);
			this.VideoEndContrastLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndContrastLbl.Name = "VideoEndContrastLbl";
			this.VideoEndContrastLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndContrastLbl.TabIndex = 29;
			this.VideoEndContrastLbl.Text = "终止对比";
			this.VideoEndContrastLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartContrastLbl
			// 
			this.VideoStartContrastLbl.AutoSize = true;
			this.VideoStartContrastLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartContrastLbl.Location = new System.Drawing.Point(2, 656);
			this.VideoStartContrastLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartContrastLbl.Name = "VideoStartContrastLbl";
			this.VideoStartContrastLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartContrastLbl.TabIndex = 28;
			this.VideoStartContrastLbl.Text = "起始对比";
			this.VideoStartContrastLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartHueLbl
			// 
			this.VideoStartHueLbl.AutoSize = true;
			this.VideoStartHueLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHueLbl.Location = new System.Drawing.Point(2, 492);
			this.VideoStartHueLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartHueLbl.Name = "VideoStartHueLbl";
			this.VideoStartHueLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartHueLbl.TabIndex = 27;
			this.VideoStartHueLbl.Text = "起始色相";
			this.VideoStartHueLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartSaturationLbl
			// 
			this.VideoStartSaturationLbl.AutoSize = true;
			this.VideoStartSaturationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSaturationLbl.Location = new System.Drawing.Point(2, 574);
			this.VideoStartSaturationLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartSaturationLbl.Name = "VideoStartSaturationLbl";
			this.VideoStartSaturationLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartSaturationLbl.TabIndex = 26;
			this.VideoStartSaturationLbl.Text = "起始饱和";
			this.VideoStartSaturationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndHueLbl
			// 
			this.VideoEndHueLbl.AutoSize = true;
			this.VideoEndHueLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndHueLbl.Location = new System.Drawing.Point(2, 533);
			this.VideoEndHueLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndHueLbl.Name = "VideoEndHueLbl";
			this.VideoEndHueLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndHueLbl.TabIndex = 25;
			this.VideoEndHueLbl.Text = "终止色相";
			this.VideoEndHueLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoFadeInLbl
			// 
			this.VideoFadeInLbl.AutoSize = true;
			this.VideoFadeInLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeInLbl.Location = new System.Drawing.Point(2, 0);
			this.VideoFadeInLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoFadeInLbl.Name = "VideoFadeInLbl";
			this.VideoFadeInLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoFadeInLbl.TabIndex = 0;
			this.VideoFadeInLbl.Text = "渐入　　";
			this.VideoFadeInLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoFadeInBox
			// 
			this.VideoFadeInBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoFadeInBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeInBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoFadeInBox.Location = new System.Drawing.Point(78, 5);
			this.VideoFadeInBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoFadeInBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoFadeInBox.Name = "VideoFadeInBox";
			this.VideoFadeInBox.NumericUpDownWidth = 65;
			this.VideoFadeInBox.Size = new System.Drawing.Size(465, 31);
			this.VideoFadeInBox.TabIndex = 2;
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
			this.VideoFadeInCurveCombo.Location = new System.Drawing.Point(550, 4);
			this.VideoFadeInCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoFadeInCurveCombo.Name = "VideoFadeInCurveCombo";
			this.VideoFadeInCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoFadeInCurveCombo.TabIndex = 3;
			// 
			// VideoFadeOutLbl
			// 
			this.VideoFadeOutLbl.AutoSize = true;
			this.VideoFadeOutLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeOutLbl.Location = new System.Drawing.Point(2, 41);
			this.VideoFadeOutLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoFadeOutLbl.Name = "VideoFadeOutLbl";
			this.VideoFadeOutLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoFadeOutLbl.TabIndex = 1;
			this.VideoFadeOutLbl.Text = "渐出　　";
			this.VideoFadeOutLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoFadeOutBox
			// 
			this.VideoFadeOutBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoFadeOutBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoFadeOutBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoFadeOutBox.Location = new System.Drawing.Point(78, 46);
			this.VideoFadeOutBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoFadeOutBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoFadeOutBox.Name = "VideoFadeOutBox";
			this.VideoFadeOutBox.NumericUpDownWidth = 65;
			this.VideoFadeOutBox.Size = new System.Drawing.Size(465, 31);
			this.VideoFadeOutBox.TabIndex = 4;
			this.VideoFadeOutBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
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
			this.VideoFadeOutCurveCombo.Location = new System.Drawing.Point(550, 45);
			this.VideoFadeOutCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoFadeOutCurveCombo.Name = "VideoFadeOutCurveCombo";
			this.VideoFadeOutCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoFadeOutCurveCombo.TabIndex = 5;
			// 
			// VideoGlowLbl
			// 
			this.VideoGlowLbl.AutoSize = true;
			this.VideoGlowLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoGlowLbl.Location = new System.Drawing.Point(2, 82);
			this.VideoGlowLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoGlowLbl.Name = "VideoGlowLbl";
			this.VideoGlowLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoGlowLbl.TabIndex = 23;
			this.VideoGlowLbl.Text = "发光";
			this.VideoGlowLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoGlowBox
			// 
			this.VideoGlowBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoGlowBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoGlowBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoGlowBox.Location = new System.Drawing.Point(78, 87);
			this.VideoGlowBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoGlowBox.Minimum = -100;
			this.VideoGlowBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoGlowBox.Name = "VideoGlowBox";
			this.VideoGlowBox.NumericUpDownWidth = 65;
			this.VideoGlowBox.Size = new System.Drawing.Size(465, 31);
			this.VideoGlowBox.TabIndex = 10;
			// 
			// VideoGlowCurveCombo
			// 
			this.VideoGlowCurveCombo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoGlowCurveCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoGlowCurveCombo.FormattingEnabled = true;
			this.VideoGlowCurveCombo.Items.AddRange(new object[] {
			"线性",
			"快速",
			"慢速",
			"平滑",
			"急剧"});
			this.VideoGlowCurveCombo.Location = new System.Drawing.Point(550, 86);
			this.VideoGlowCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoGlowCurveCombo.Name = "VideoGlowCurveCombo";
			this.VideoGlowCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoGlowCurveCombo.TabIndex = 11;
			// 
			// VideoGlowBrightLbl
			// 
			this.VideoGlowBrightLbl.AutoSize = true;
			this.VideoGlowBrightLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoGlowBrightLbl.Location = new System.Drawing.Point(2, 123);
			this.VideoGlowBrightLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoGlowBrightLbl.Name = "VideoGlowBrightLbl";
			this.VideoGlowBrightLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoGlowBrightLbl.TabIndex = 24;
			this.VideoGlowBrightLbl.Text = "发光亮度";
			this.VideoGlowBrightLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoGlowBrightBox
			// 
			this.VideoGlowBrightBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoGlowBrightBox.DefaultValue = 100;
			this.VideoGlowBrightBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoGlowBrightBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoGlowBrightBox.Location = new System.Drawing.Point(78, 128);
			this.VideoGlowBrightBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoGlowBrightBox.Minimum = -100;
			this.VideoGlowBrightBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoGlowBrightBox.Name = "VideoGlowBrightBox";
			this.VideoGlowBrightBox.NumericUpDownWidth = 65;
			this.VideoGlowBrightBox.Size = new System.Drawing.Size(465, 31);
			this.VideoGlowBrightBox.TabIndex = 12;
			this.VideoGlowBrightBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.VideoGlowBrightBox.Value = 100;
			// 
			// VideoStartSizeLbl
			// 
			this.VideoStartSizeLbl.AutoSize = true;
			this.VideoStartSizeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSizeLbl.Location = new System.Drawing.Point(2, 164);
			this.VideoStartSizeLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartSizeLbl.Name = "VideoStartSizeLbl";
			this.VideoStartSizeLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartSizeLbl.TabIndex = 6;
			this.VideoStartSizeLbl.Text = "起始尺寸";
			this.VideoStartSizeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartSizeBox
			// 
			this.VideoStartSizeBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartSizeBox.DefaultValue = 90;
			this.VideoStartSizeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartSizeBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartSizeBox.Location = new System.Drawing.Point(78, 169);
			this.VideoStartSizeBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartSizeBox.Maximum = 200;
			this.VideoStartSizeBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartSizeBox.Name = "VideoStartSizeBox";
			this.VideoStartSizeBox.NumericUpDownWidth = 65;
			this.VideoStartSizeBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartSizeBox.TabIndex = 14;
			this.VideoStartSizeBox.Value = 90;
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
			this.VideoStartSizeCurveCombo.Location = new System.Drawing.Point(550, 168);
			this.VideoStartSizeCurveCombo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.VideoStartSizeCurveCombo.Name = "VideoStartSizeCurveCombo";
			this.VideoStartSizeCurveCombo.Size = new System.Drawing.Size(65, 28);
			this.VideoStartSizeCurveCombo.TabIndex = 15;
			// 
			// VideoEndSizeLbl
			// 
			this.VideoEndSizeLbl.AutoSize = true;
			this.VideoEndSizeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndSizeLbl.Location = new System.Drawing.Point(2, 205);
			this.VideoEndSizeLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndSizeLbl.Name = "VideoEndSizeLbl";
			this.VideoEndSizeLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndSizeLbl.TabIndex = 7;
			this.VideoEndSizeLbl.Text = "终止尺寸";
			this.VideoEndSizeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndSizeBox
			// 
			this.VideoEndSizeBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndSizeBox.DefaultValue = 100;
			this.VideoEndSizeBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndSizeBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndSizeBox.Location = new System.Drawing.Point(78, 210);
			this.VideoEndSizeBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndSizeBox.Maximum = 200;
			this.VideoEndSizeBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndSizeBox.Name = "VideoEndSizeBox";
			this.VideoEndSizeBox.NumericUpDownWidth = 65;
			this.VideoEndSizeBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndSizeBox.TabIndex = 16;
			this.VideoEndSizeBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			this.VideoEndSizeBox.Value = 100;
			// 
			// VideoStartRotationLbl
			// 
			this.VideoStartRotationLbl.AutoSize = true;
			this.VideoStartRotationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartRotationLbl.Location = new System.Drawing.Point(2, 246);
			this.VideoStartRotationLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartRotationLbl.Name = "VideoStartRotationLbl";
			this.VideoStartRotationLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartRotationLbl.TabIndex = 8;
			this.VideoStartRotationLbl.Text = "起始旋转";
			this.VideoStartRotationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartRotationBox
			// 
			this.VideoStartRotationBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartRotationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartRotationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartRotationBox.Location = new System.Drawing.Point(78, 251);
			this.VideoStartRotationBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartRotationBox.Maximum = 360;
			this.VideoStartRotationBox.Minimum = -360;
			this.VideoStartRotationBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartRotationBox.Name = "VideoStartRotationBox";
			this.VideoStartRotationBox.NumericUpDownWidth = 65;
			this.VideoStartRotationBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartRotationBox.TabIndex = 17;
			// 
			// VideoEndRotationLbl
			// 
			this.VideoEndRotationLbl.AutoSize = true;
			this.VideoEndRotationLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndRotationLbl.Location = new System.Drawing.Point(2, 287);
			this.VideoEndRotationLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndRotationLbl.Name = "VideoEndRotationLbl";
			this.VideoEndRotationLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndRotationLbl.TabIndex = 9;
			this.VideoEndRotationLbl.Text = "终止旋转";
			this.VideoEndRotationLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndRotationBox
			// 
			this.VideoEndRotationBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndRotationBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndRotationBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndRotationBox.Location = new System.Drawing.Point(78, 292);
			this.VideoEndRotationBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndRotationBox.Maximum = 360;
			this.VideoEndRotationBox.Minimum = -360;
			this.VideoEndRotationBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndRotationBox.Name = "VideoEndRotationBox";
			this.VideoEndRotationBox.NumericUpDownWidth = 65;
			this.VideoEndRotationBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndRotationBox.TabIndex = 18;
			this.VideoEndRotationBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoStartHorizontalTransLbl
			// 
			this.VideoStartHorizontalTransLbl.AutoSize = true;
			this.VideoStartHorizontalTransLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHorizontalTransLbl.Location = new System.Drawing.Point(2, 328);
			this.VideoStartHorizontalTransLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartHorizontalTransLbl.Name = "VideoStartHorizontalTransLbl";
			this.VideoStartHorizontalTransLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartHorizontalTransLbl.TabIndex = 12;
			this.VideoStartHorizontalTransLbl.Text = "起始平移";
			this.VideoStartHorizontalTransLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartHorizontalTransBox
			// 
			this.VideoStartHorizontalTransBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartHorizontalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartHorizontalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartHorizontalTransBox.Location = new System.Drawing.Point(78, 333);
			this.VideoStartHorizontalTransBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartHorizontalTransBox.Minimum = -100;
			this.VideoStartHorizontalTransBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartHorizontalTransBox.Name = "VideoStartHorizontalTransBox";
			this.VideoStartHorizontalTransBox.NumericUpDownWidth = 65;
			this.VideoStartHorizontalTransBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartHorizontalTransBox.TabIndex = 19;
			// 
			// VideoEndHorizontalTransLbl
			// 
			this.VideoEndHorizontalTransLbl.AutoSize = true;
			this.VideoEndHorizontalTransLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndHorizontalTransLbl.Location = new System.Drawing.Point(2, 369);
			this.VideoEndHorizontalTransLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndHorizontalTransLbl.Name = "VideoEndHorizontalTransLbl";
			this.VideoEndHorizontalTransLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndHorizontalTransLbl.TabIndex = 10;
			this.VideoEndHorizontalTransLbl.Text = "终止平移";
			this.VideoEndHorizontalTransLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndHorizontalTransBox
			// 
			this.VideoEndHorizontalTransBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndHorizontalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndHorizontalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndHorizontalTransBox.Location = new System.Drawing.Point(78, 374);
			this.VideoEndHorizontalTransBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndHorizontalTransBox.Minimum = -100;
			this.VideoEndHorizontalTransBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndHorizontalTransBox.Name = "VideoEndHorizontalTransBox";
			this.VideoEndHorizontalTransBox.NumericUpDownWidth = 65;
			this.VideoEndHorizontalTransBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndHorizontalTransBox.TabIndex = 20;
			this.VideoEndHorizontalTransBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoStartVerticalTransLbl
			// 
			this.VideoStartVerticalTransLbl.AutoSize = true;
			this.VideoStartVerticalTransLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartVerticalTransLbl.Location = new System.Drawing.Point(2, 410);
			this.VideoStartVerticalTransLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoStartVerticalTransLbl.Name = "VideoStartVerticalTransLbl";
			this.VideoStartVerticalTransLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoStartVerticalTransLbl.TabIndex = 11;
			this.VideoStartVerticalTransLbl.Text = "起始直移";
			this.VideoStartVerticalTransLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoStartVerticalTransBox
			// 
			this.VideoStartVerticalTransBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoStartVerticalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoStartVerticalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoStartVerticalTransBox.Location = new System.Drawing.Point(78, 415);
			this.VideoStartVerticalTransBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoStartVerticalTransBox.Minimum = -100;
			this.VideoStartVerticalTransBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoStartVerticalTransBox.Name = "VideoStartVerticalTransBox";
			this.VideoStartVerticalTransBox.NumericUpDownWidth = 65;
			this.VideoStartVerticalTransBox.Size = new System.Drawing.Size(465, 31);
			this.VideoStartVerticalTransBox.TabIndex = 21;
			// 
			// VideoEndVerticalTransLbl
			// 
			this.VideoEndVerticalTransLbl.AutoSize = true;
			this.VideoEndVerticalTransLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndVerticalTransLbl.Location = new System.Drawing.Point(2, 451);
			this.VideoEndVerticalTransLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEndVerticalTransLbl.Name = "VideoEndVerticalTransLbl";
			this.VideoEndVerticalTransLbl.Size = new System.Drawing.Size(69, 41);
			this.VideoEndVerticalTransLbl.TabIndex = 13;
			this.VideoEndVerticalTransLbl.Text = "终止直移";
			this.VideoEndVerticalTransLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEndVerticalTransBox
			// 
			this.VideoEndVerticalTransBox.BackColor = System.Drawing.Color.Transparent;
			this.VideoEndVerticalTransBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEndVerticalTransBox.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.VideoEndVerticalTransBox.Location = new System.Drawing.Point(78, 456);
			this.VideoEndVerticalTransBox.Margin = new System.Windows.Forms.Padding(5);
			this.VideoEndVerticalTransBox.Minimum = -100;
			this.VideoEndVerticalTransBox.MinimumSize = new System.Drawing.Size(0, 31);
			this.VideoEndVerticalTransBox.Name = "VideoEndVerticalTransBox";
			this.VideoEndVerticalTransBox.NumericUpDownWidth = 65;
			this.VideoEndVerticalTransBox.Size = new System.Drawing.Size(465, 31);
			this.VideoEndVerticalTransBox.TabIndex = 22;
			this.VideoEndVerticalTransBox.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
			// 
			// VideoEffectsGroup
			// 
			this.VideoEffectsGroup.AutoSize = true;
			this.VideoEffectsGroup.Controls.Add(this.tableLayoutPanel8);
			this.VideoEffectsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.VideoEffectsGroup.Location = new System.Drawing.Point(5, 65);
			this.VideoEffectsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.VideoEffectsGroup.Name = "VideoEffectsGroup";
			this.VideoEffectsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.VideoEffectsGroup.Size = new System.Drawing.Size(627, 101);
			this.VideoEffectsGroup.TabIndex = 2;
			this.VideoEffectsGroup.TabStop = false;
			this.VideoEffectsGroup.Text = "效果";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectLbl, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectInitialValueLbl, 0, 1);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectCombo, 1, 0);
			this.tableLayoutPanel8.Controls.Add(this.VideoEffectInitialValueCombo, 1, 1);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(4);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 2;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel8.Size = new System.Drawing.Size(617, 71);
			this.tableLayoutPanel8.TabIndex = 1;
			// 
			// VideoEffectLbl
			// 
			this.VideoEffectLbl.AutoSize = true;
			this.VideoEffectLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEffectLbl.Location = new System.Drawing.Point(2, 0);
			this.VideoEffectLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEffectLbl.Name = "VideoEffectLbl";
			this.VideoEffectLbl.Size = new System.Drawing.Size(69, 35);
			this.VideoEffectLbl.TabIndex = 0;
			this.VideoEffectLbl.Text = "视觉效果";
			this.VideoEffectLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// VideoEffectInitialValueLbl
			// 
			this.VideoEffectInitialValueLbl.AutoSize = true;
			this.VideoEffectInitialValueLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VideoEffectInitialValueLbl.Location = new System.Drawing.Point(2, 35);
			this.VideoEffectInitialValueLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.VideoEffectInitialValueLbl.Name = "VideoEffectInitialValueLbl";
			this.VideoEffectInitialValueLbl.Size = new System.Drawing.Size(69, 36);
			this.VideoEffectInitialValueLbl.TabIndex = 1;
			this.VideoEffectInitialValueLbl.Text = "初始值";
			this.VideoEffectInitialValueLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			"颜色反转",
			"亮度反转",
			"色相反转",
			"3 步色差",
			"4 步色差",
			"5 步色差",
			"6 步色差",
			"7 步色差",
			"8 步色差",
			"彩灰",
			"乒乓效应",
			"爱的魔力转圈圈",
			"垂直扩张",
			"垂直扩张并回弹",
			"垂直压缩",
			"垂直压缩并回弹",
			"斜向扩缩",
			"魔法气泡",
			"左右摇摆",
			"高斯模糊",
			"径向模糊"});
			this.VideoEffectCombo.Location = new System.Drawing.Point(75, 2);
			this.VideoEffectCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 5);
			this.VideoEffectCombo.Name = "VideoEffectCombo";
			this.VideoEffectCombo.Size = new System.Drawing.Size(540, 28);
			this.VideoEffectCombo.TabIndex = 2;
			this.VideoEffectCombo.SelectedIndexChanged += new System.EventHandler(this.VideoEffectCombo_SelectedIndexChanged);
			// 
			// VideoEffectInitialValueCombo
			// 
			this.VideoEffectInitialValueCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.VideoEffectInitialValueCombo.FormattingEnabled = true;
			this.VideoEffectInitialValueCombo.Location = new System.Drawing.Point(75, 37);
			this.VideoEffectInitialValueCombo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 6);
			this.VideoEffectInitialValueCombo.Name = "VideoEffectInitialValueCombo";
			this.VideoEffectInitialValueCombo.Size = new System.Drawing.Size(90, 28);
			this.VideoEffectInitialValueCombo.TabIndex = 3;
			// 
			// flowLayoutPanel7
			// 
			this.flowLayoutPanel7.AutoSize = true;
			this.flowLayoutPanel7.Controls.Add(this.VideoConfigCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoScratchCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoLoopCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoFreezeFirstFrameCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoFreezeLastFrameCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoLegatoCheck);
			this.flowLayoutPanel7.Controls.Add(this.VideoMultitrackForChordsCheck);
			this.flowLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel7.Location = new System.Drawing.Point(5, 5);
			this.flowLayoutPanel7.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel7.Name = "flowLayoutPanel7";
			this.flowLayoutPanel7.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.flowLayoutPanel7.Size = new System.Drawing.Size(627, 60);
			this.flowLayoutPanel7.TabIndex = 1;
			// 
			// VideoConfigCheck
			// 
			this.VideoConfigCheck.AutoSize = true;
			this.VideoConfigCheck.Checked = true;
			this.VideoConfigCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.VideoConfigCheck.Location = new System.Drawing.Point(2, 4);
			this.VideoConfigCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoConfigCheck.Name = "VideoConfigCheck";
			this.VideoConfigCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoConfigCheck.TabIndex = 0;
			this.VideoConfigCheck.Text = "生成视频";
			this.VideoConfigCheck.UseVisualStyleBackColor = true;
			// 
			// VideoScratchCheck
			// 
			this.VideoScratchCheck.AutoSize = true;
			this.VideoScratchCheck.Location = new System.Drawing.Point(97, 4);
			this.VideoScratchCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoScratchCheck.Name = "VideoScratchCheck";
			this.VideoScratchCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoScratchCheck.TabIndex = 1;
			this.VideoScratchCheck.Text = "拉伸视频";
			this.VideoScratchCheck.UseVisualStyleBackColor = true;
			// 
			// VideoLoopCheck
			// 
			this.VideoLoopCheck.AutoSize = true;
			this.VideoLoopCheck.Checked = true;
			this.VideoLoopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.VideoLoopCheck.Location = new System.Drawing.Point(192, 4);
			this.VideoLoopCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoLoopCheck.Name = "VideoLoopCheck";
			this.VideoLoopCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoLoopCheck.TabIndex = 2;
			this.VideoLoopCheck.Text = "循环视频";
			this.VideoLoopCheck.UseVisualStyleBackColor = true;
			// 
			// VideoFreezeFirstFrameCheck
			// 
			this.VideoFreezeFirstFrameCheck.AutoSize = true;
			this.VideoFreezeFirstFrameCheck.Location = new System.Drawing.Point(287, 4);
			this.VideoFreezeFirstFrameCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoFreezeFirstFrameCheck.Name = "VideoFreezeFirstFrameCheck";
			this.VideoFreezeFirstFrameCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoFreezeFirstFrameCheck.TabIndex = 3;
			this.VideoFreezeFirstFrameCheck.Text = "静态画面";
			this.VideoFreezeFirstFrameCheck.UseVisualStyleBackColor = true;
			// 
			// VideoFreezeLastFrameCheck
			// 
			this.VideoFreezeLastFrameCheck.AutoSize = true;
			this.VideoFreezeLastFrameCheck.Location = new System.Drawing.Point(382, 4);
			this.VideoFreezeLastFrameCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoFreezeLastFrameCheck.Name = "VideoFreezeLastFrameCheck";
			this.VideoFreezeLastFrameCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoFreezeLastFrameCheck.TabIndex = 4;
			this.VideoFreezeLastFrameCheck.Text = "禁止延长";
			this.VideoFreezeLastFrameCheck.UseVisualStyleBackColor = true;
			// 
			// VideoLegatoCheck
			// 
			this.VideoLegatoCheck.AutoSize = true;
			this.VideoLegatoCheck.Checked = true;
			this.VideoLegatoCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.VideoLegatoCheck.Location = new System.Drawing.Point(477, 4);
			this.VideoLegatoCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoLegatoCheck.Name = "VideoLegatoCheck";
			this.VideoLegatoCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoLegatoCheck.TabIndex = 5;
			this.VideoLegatoCheck.Text = "填补间隙";
			this.VideoLegatoCheck.UseVisualStyleBackColor = true;
			// 
			// VideoMultitrackForChordsCheck
			// 
			this.VideoMultitrackForChordsCheck.AutoSize = true;
			this.VideoMultitrackForChordsCheck.Location = new System.Drawing.Point(2, 32);
			this.VideoMultitrackForChordsCheck.Margin = new System.Windows.Forms.Padding(2);
			this.VideoMultitrackForChordsCheck.Name = "VideoMultitrackForChordsCheck";
			this.VideoMultitrackForChordsCheck.Size = new System.Drawing.Size(91, 24);
			this.VideoMultitrackForChordsCheck.TabIndex = 9;
			this.VideoMultitrackForChordsCheck.Text = "复音多轨";
			this.VideoMultitrackForChordsCheck.UseVisualStyleBackColor = true;
			// 
			// SheetTab
			// 
			this.SheetTab.AutoScroll = true;
			this.SheetTab.BackColor = System.Drawing.Color.Transparent;
			this.SheetTab.Controls.Add(this.groupBox1);
			this.SheetTab.Controls.Add(this.StaffParamsGroup);
			this.SheetTab.Controls.Add(this.flowLayoutPanel8);
			this.SheetTab.Controls.Add(this.SheetConfigInfoLabel);
			this.SheetTab.Location = new System.Drawing.Point(4, 29);
			this.SheetTab.Margin = new System.Windows.Forms.Padding(2);
			this.SheetTab.Name = "SheetTab";
			this.SheetTab.Padding = new System.Windows.Forms.Padding(5);
			this.SheetTab.Size = new System.Drawing.Size(658, 607);
			this.SheetTab.TabIndex = 3;
			this.SheetTab.Text = "五线谱";
			this.SheetTab.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.Controls.Add(this.tableLayoutPanel7);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(5, 241);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
			this.groupBox1.Size = new System.Drawing.Size(648, 93);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "谱线参数";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.ColumnCount = 4;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel7.Controls.Add(this.StaffGenerateCheck, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.StaffLineColorBtn, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.StaffLineColorLbl, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.StaffLineThicknessBox, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.StaffLineThicknessLbl, 0, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(638, 63);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// StaffGenerateCheck
			// 
			this.StaffGenerateCheck.AutoSize = true;
			this.StaffGenerateCheck.Checked = true;
			this.StaffGenerateCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tableLayoutPanel7.SetColumnSpan(this.StaffGenerateCheck, 4);
			this.StaffGenerateCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffGenerateCheck.Enabled = false;
			this.StaffGenerateCheck.Location = new System.Drawing.Point(2, 2);
			this.StaffGenerateCheck.Margin = new System.Windows.Forms.Padding(2);
			this.StaffGenerateCheck.Name = "StaffGenerateCheck";
			this.StaffGenerateCheck.Size = new System.Drawing.Size(634, 24);
			this.StaffGenerateCheck.TabIndex = 0;
			this.StaffGenerateCheck.Text = "生成五线谱";
			this.StaffGenerateCheck.UseVisualStyleBackColor = true;
			// 
			// StaffLineColorBtn
			// 
			this.StaffLineColorBtn.BackColor = System.Drawing.Color.White;
			this.StaffLineColorBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineColorBtn.Enabled = false;
			this.StaffLineColorBtn.ForeColor = System.Drawing.Color.Black;
			this.StaffLineColorBtn.Location = new System.Drawing.Point(394, 30);
			this.StaffLineColorBtn.Margin = new System.Windows.Forms.Padding(2);
			this.StaffLineColorBtn.Name = "StaffLineColorBtn";
			this.StaffLineColorBtn.Size = new System.Drawing.Size(242, 31);
			this.StaffLineColorBtn.TabIndex = 13;
			this.StaffLineColorBtn.Text = "#FFFFFF";
			this.StaffLineColorBtn.UseVisualStyleBackColor = false;
			this.StaffLineColorBtn.Click += new System.EventHandler(this.StaffLineColorBtn_Click);
			// 
			// StaffLineColorLbl
			// 
			this.StaffLineColorLbl.AutoSize = true;
			this.StaffLineColorLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineColorLbl.Location = new System.Drawing.Point(321, 28);
			this.StaffLineColorLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffLineColorLbl.Name = "StaffLineColorLbl";
			this.StaffLineColorLbl.Size = new System.Drawing.Size(69, 35);
			this.StaffLineColorLbl.TabIndex = 12;
			this.StaffLineColorLbl.Text = "谱线颜色";
			this.StaffLineColorLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffLineThicknessLbl
			// 
			this.StaffLineThicknessLbl.AutoSize = true;
			this.StaffLineThicknessLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineThicknessLbl.Location = new System.Drawing.Point(2, 28);
			this.StaffLineThicknessLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffLineThicknessLbl.Name = "StaffLineThicknessLbl";
			this.StaffLineThicknessLbl.Size = new System.Drawing.Size(69, 35);
			this.StaffLineThicknessLbl.TabIndex = 5;
			this.StaffLineThicknessLbl.Text = "谱线粗细";
			this.StaffLineThicknessLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffParamsGroup
			// 
			this.StaffParamsGroup.AutoSize = true;
			this.StaffParamsGroup.Controls.Add(this.tableLayoutPanel10);
			this.StaffParamsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.StaffParamsGroup.Location = new System.Drawing.Point(5, 87);
			this.StaffParamsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.StaffParamsGroup.Name = "StaffParamsGroup";
			this.StaffParamsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.StaffParamsGroup.Size = new System.Drawing.Size(648, 154);
			this.StaffParamsGroup.TabIndex = 6;
			this.StaffParamsGroup.TabStop = false;
			this.StaffParamsGroup.Text = "音符参数";
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.AutoSize = true;
			this.tableLayoutPanel10.ColumnCount = 4;
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.Controls.Add(this.StaffRelativeValueCheck, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.StaffClefLbl, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this.StaffClefCombo, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.StaffLineSpacingLbl, 2, 1);
			this.tableLayoutPanel10.Controls.Add(this.StaffLineSpacingBox, 3, 1);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfacePositionBox, 3, 2);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfaceWidthLbl, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfaceWidthBox, 1, 2);
			this.tableLayoutPanel10.Controls.Add(this.StaffSurfacePositionLbl, 2, 2);
			this.tableLayoutPanel10.Controls.Add(this.StaffNotesShiftLbl, 0, 3);
			this.tableLayoutPanel10.Controls.Add(this.StaffNotesShiftBox, 1, 3);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 4;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(638, 124);
			this.tableLayoutPanel10.TabIndex = 0;
			// 
			// StaffClefLbl
			// 
			this.StaffClefLbl.AutoSize = true;
			this.StaffClefLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffClefLbl.Location = new System.Drawing.Point(2, 31);
			this.StaffClefLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffClefLbl.Name = "StaffClefLbl";
			this.StaffClefLbl.Size = new System.Drawing.Size(69, 31);
			this.StaffClefLbl.TabIndex = 0;
			this.StaffClefLbl.Text = "谱号";
			this.StaffClefLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.StaffClefCombo.Location = new System.Drawing.Point(75, 33);
			this.StaffClefCombo.Margin = new System.Windows.Forms.Padding(2);
			this.StaffClefCombo.Name = "StaffClefCombo";
			this.StaffClefCombo.Size = new System.Drawing.Size(242, 28);
			this.StaffClefCombo.TabIndex = 6;
			// 
			// StaffLineSpacingLbl
			// 
			this.StaffLineSpacingLbl.AutoSize = true;
			this.StaffLineSpacingLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffLineSpacingLbl.Location = new System.Drawing.Point(321, 31);
			this.StaffLineSpacingLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffLineSpacingLbl.Name = "StaffLineSpacingLbl";
			this.StaffLineSpacingLbl.Size = new System.Drawing.Size(69, 31);
			this.StaffLineSpacingLbl.TabIndex = 1;
			this.StaffLineSpacingLbl.Text = "谱线间距";
			this.StaffLineSpacingLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffSurfaceWidthLbl
			// 
			this.StaffSurfaceWidthLbl.AutoSize = true;
			this.StaffSurfaceWidthLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfaceWidthLbl.Location = new System.Drawing.Point(2, 62);
			this.StaffSurfaceWidthLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffSurfaceWidthLbl.Name = "StaffSurfaceWidthLbl";
			this.StaffSurfaceWidthLbl.Size = new System.Drawing.Size(69, 31);
			this.StaffSurfaceWidthLbl.TabIndex = 5;
			this.StaffSurfaceWidthLbl.Text = "谱面宽度";
			this.StaffSurfaceWidthLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffSurfacePositionLbl
			// 
			this.StaffSurfacePositionLbl.AutoSize = true;
			this.StaffSurfacePositionLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffSurfacePositionLbl.Location = new System.Drawing.Point(321, 62);
			this.StaffSurfacePositionLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffSurfacePositionLbl.Name = "StaffSurfacePositionLbl";
			this.StaffSurfacePositionLbl.Size = new System.Drawing.Size(69, 31);
			this.StaffSurfacePositionLbl.TabIndex = 2;
			this.StaffSurfacePositionLbl.Text = "谱面位置";
			this.StaffSurfacePositionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffNotesShiftLbl
			// 
			this.StaffNotesShiftLbl.AutoSize = true;
			this.StaffNotesShiftLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffNotesShiftLbl.Location = new System.Drawing.Point(2, 93);
			this.StaffNotesShiftLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.StaffNotesShiftLbl.Name = "StaffNotesShiftLbl";
			this.StaffNotesShiftLbl.Size = new System.Drawing.Size(69, 31);
			this.StaffNotesShiftLbl.TabIndex = 12;
			this.StaffNotesShiftLbl.Text = "音符偏移";
			this.StaffNotesShiftLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StaffNotesShiftBox
			// 
			this.StaffNotesShiftBox.Constrain = new decimal(new int[] {
			0,
			0,
			0,
			0});
			this.StaffNotesShiftBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffNotesShiftBox.Enabled = false;
			this.StaffNotesShiftBox.EnableDecimalPlaces = true;
			this.StaffNotesShiftBox.Location = new System.Drawing.Point(75, 95);
			this.StaffNotesShiftBox.Margin = new System.Windows.Forms.Padding(2);
			this.StaffNotesShiftBox.Maximum = new decimal(new int[] {
			120,
			0,
			0,
			0});
			this.StaffNotesShiftBox.Minimum = new decimal(new int[] {
			120,
			0,
			0,
			-2147483648});
			this.StaffNotesShiftBox.Name = "StaffNotesShiftBox";
			this.StaffNotesShiftBox.Size = new System.Drawing.Size(242, 27);
			this.StaffNotesShiftBox.Suffix = "key";
			this.StaffNotesShiftBox.TabIndex = 13;
			// 
			// flowLayoutPanel8
			// 
			this.flowLayoutPanel8.AutoSize = true;
			this.flowLayoutPanel8.Controls.Add(this.StaffVisualizerConfigCheck);
			this.flowLayoutPanel8.Controls.Add(this.StaffLegacyEngineCheck);
			this.flowLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Top;
			this.flowLayoutPanel8.Location = new System.Drawing.Point(5, 55);
			this.flowLayoutPanel8.Margin = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel8.Name = "flowLayoutPanel8";
			this.flowLayoutPanel8.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
			this.flowLayoutPanel8.Size = new System.Drawing.Size(648, 32);
			this.flowLayoutPanel8.TabIndex = 5;
			// 
			// StaffVisualizerConfigCheck
			// 
			this.StaffVisualizerConfigCheck.AutoSize = true;
			this.StaffVisualizerConfigCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StaffVisualizerConfigCheck.Location = new System.Drawing.Point(7, 4);
			this.StaffVisualizerConfigCheck.Margin = new System.Windows.Forms.Padding(2);
			this.StaffVisualizerConfigCheck.Name = "StaffVisualizerConfigCheck";
			this.StaffVisualizerConfigCheck.Size = new System.Drawing.Size(181, 24);
			this.StaffVisualizerConfigCheck.TabIndex = 4;
			this.StaffVisualizerConfigCheck.Text = "启用五线谱可视化效果";
			this.StaffVisualizerConfigCheck.UseVisualStyleBackColor = true;
			// 
			// SheetConfigInfoLabel
			// 
			this.SheetConfigInfoLabel.AutoSize = true;
			this.SheetConfigInfoLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.SheetConfigInfoLabel.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.SheetConfigInfoLabel.Location = new System.Drawing.Point(5, 5);
			this.SheetConfigInfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.SheetConfigInfoLabel.Name = "SheetConfigInfoLabel";
			this.SheetConfigInfoLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.SheetConfigInfoLabel.Size = new System.Drawing.Size(384, 50);
			this.SheetConfigInfoLabel.TabIndex = 4;
			this.SheetConfigInfoLabel.Text = "欲开启五线谱视觉效果，需要先开启“生成视频”选项。\r\n开启本功能会禁用视频视觉效果和视频拉伸选项。";
			this.SheetConfigInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// YtpTab
			// 
			this.YtpTab.AutoScroll = true;
			this.YtpTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			this.YtpTab.Controls.Add(this.YtpParamsGroup);
			this.YtpTab.Controls.Add(this.YtpEffectsGroup);
			this.YtpTab.Controls.Add(this.YtpSelectInfo);
			this.YtpTab.Controls.Add(this.YtpLbl);
			this.YtpTab.Location = new System.Drawing.Point(4, 29);
			this.YtpTab.Margin = new System.Windows.Forms.Padding(2);
			this.YtpTab.Name = "YtpTab";
			this.YtpTab.Padding = new System.Windows.Forms.Padding(5);
			this.YtpTab.Size = new System.Drawing.Size(658, 607);
			this.YtpTab.TabIndex = 5;
			this.YtpTab.Text = "YTP";
			this.YtpTab.UseVisualStyleBackColor = true;
			// 
			// YtpParamsGroup
			// 
			this.YtpParamsGroup.AutoSize = true;
			this.YtpParamsGroup.Controls.Add(this.tableLayoutPanel16);
			this.YtpParamsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.YtpParamsGroup.Location = new System.Drawing.Point(5, 242);
			this.YtpParamsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.YtpParamsGroup.Name = "YtpParamsGroup";
			this.YtpParamsGroup.Padding = new System.Windows.Forms.Padding(5);
			this.YtpParamsGroup.Size = new System.Drawing.Size(648, 92);
			this.YtpParamsGroup.TabIndex = 12;
			this.YtpParamsGroup.TabStop = false;
			this.YtpParamsGroup.Text = "参数";
			// 
			// tableLayoutPanel16
			// 
			this.tableLayoutPanel16.AutoSize = true;
			this.tableLayoutPanel16.ColumnCount = 4;
			this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel16.Controls.Add(this.YtpMinLenLbl, 0, 0);
			this.tableLayoutPanel16.Controls.Add(this.YtpClipsCountLbl, 0, 1);
			this.tableLayoutPanel16.Controls.Add(this.YtpClipsCountBox, 1, 1);
			this.tableLayoutPanel16.Controls.Add(this.YtpMinLenBox, 1, 0);
			this.tableLayoutPanel16.Controls.Add(this.YtpMaxLenLbl, 2, 0);
			this.tableLayoutPanel16.Controls.Add(this.YtpMaxLenBox, 3, 0);
			this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel16.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel16.Name = "tableLayoutPanel16";
			this.tableLayoutPanel16.RowCount = 2;
			this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel16.Size = new System.Drawing.Size(638, 62);
			this.tableLayoutPanel16.TabIndex = 0;
			// 
			// YtpMinLenLbl
			// 
			this.YtpMinLenLbl.AutoSize = true;
			this.YtpMinLenLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpMinLenLbl.Location = new System.Drawing.Point(2, 0);
			this.YtpMinLenLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.YtpMinLenLbl.Name = "YtpMinLenLbl";
			this.YtpMinLenLbl.Size = new System.Drawing.Size(69, 31);
			this.YtpMinLenLbl.TabIndex = 5;
			this.YtpMinLenLbl.Text = "最小长度";
			this.YtpMinLenLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// YtpClipsCountLbl
			// 
			this.YtpClipsCountLbl.AutoSize = true;
			this.YtpClipsCountLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpClipsCountLbl.Location = new System.Drawing.Point(2, 31);
			this.YtpClipsCountLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.YtpClipsCountLbl.Name = "YtpClipsCountLbl";
			this.YtpClipsCountLbl.Size = new System.Drawing.Size(69, 31);
			this.YtpClipsCountLbl.TabIndex = 10;
			this.YtpClipsCountLbl.Text = "剪辑数目";
			this.YtpClipsCountLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// YtpClipsCountBox
			// 
			this.YtpClipsCountBox.Constrain = new decimal(new int[] {
			30,
			0,
			0,
			0});
			this.YtpClipsCountBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpClipsCountBox.EnableDecimalPlaces = true;
			this.YtpClipsCountBox.Location = new System.Drawing.Point(75, 33);
			this.YtpClipsCountBox.Margin = new System.Windows.Forms.Padding(2);
			this.YtpClipsCountBox.Maximum = new decimal(new int[] {
			300,
			0,
			0,
			0});
			this.YtpClipsCountBox.Minimum = new decimal(new int[] {
			1,
			0,
			0,
			0});
			this.YtpClipsCountBox.Name = "YtpClipsCountBox";
			this.YtpClipsCountBox.Size = new System.Drawing.Size(242, 27);
			this.YtpClipsCountBox.TabIndex = 11;
			this.YtpClipsCountBox.Value = new decimal(new int[] {
			30,
			0,
			0,
			0});
			// 
			// YtpMaxLenLbl
			// 
			this.YtpMaxLenLbl.AutoSize = true;
			this.YtpMaxLenLbl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.YtpMaxLenLbl.Location = new System.Drawing.Point(321, 0);
			this.YtpMaxLenLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.YtpMaxLenLbl.Name = "YtpMaxLenLbl";
			this.YtpMaxLenLbl.Size = new System.Drawing.Size(69, 31);
			this.YtpMaxLenLbl.TabIndex = 2;
			this.YtpMaxLenLbl.Text = "最大长度";
			this.YtpMaxLenLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// YtpEffectsGroup
			// 
			this.YtpEffectsGroup.AutoSize = true;
			this.YtpEffectsGroup.Controls.Add(this.YtpEnableAllEffectsCheck);
			this.YtpEffectsGroup.Controls.Add(this.YtpEffectsCheckList);
			this.YtpEffectsGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.YtpEffectsGroup.Location = new System.Drawing.Point(5, 80);
			this.YtpEffectsGroup.Margin = new System.Windows.Forms.Padding(2);
			this.YtpEffectsGroup.Name = "YtpEffectsGroup";
			this.YtpEffectsGroup.Padding = new System.Windows.Forms.Padding(9, 2, 6, 6);
			this.YtpEffectsGroup.Size = new System.Drawing.Size(648, 162);
			this.YtpEffectsGroup.TabIndex = 11;
			this.YtpEffectsGroup.TabStop = false;
			this.YtpEffectsGroup.Text = "效果";
			// 
			// YtpEnableAllEffectsCheck
			// 
			this.YtpEnableAllEffectsCheck.AutoSize = true;
			this.YtpEnableAllEffectsCheck.Checked = true;
			this.YtpEnableAllEffectsCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.YtpEnableAllEffectsCheck.Dock = System.Windows.Forms.DockStyle.Top;
			this.YtpEnableAllEffectsCheck.Location = new System.Drawing.Point(9, 22);
			this.YtpEnableAllEffectsCheck.Margin = new System.Windows.Forms.Padding(2);
			this.YtpEnableAllEffectsCheck.Name = "YtpEnableAllEffectsCheck";
			this.YtpEnableAllEffectsCheck.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
			this.YtpEnableAllEffectsCheck.Size = new System.Drawing.Size(633, 24);
			this.YtpEnableAllEffectsCheck.TabIndex = 0;
			this.YtpEnableAllEffectsCheck.Text = "开启所有效果";
			this.YtpEnableAllEffectsCheck.CheckedChanged += new System.EventHandler(this.YtpEnableAllEffectsCheck_CheckedChanged);
			// 
			// YtpEffectsCheckList
			// 
			this.YtpEffectsCheckList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
			this.YtpEffectsCheckList.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.YtpEffectsCheckList.CheckOnClick = true;
			this.YtpEffectsCheckList.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.YtpEffectsCheckList.FormattingEnabled = true;
			this.YtpEffectsCheckList.HorizontalScrollbar = true;
			this.YtpEffectsCheckList.Items.AddRange(new object[] {
			"合唱",
			"更改音调",
			"颤音（概率性附加波浪效果）",
			"倒放",
			"延迟",
			"更改速度",
			"更改色相",
			"旋转色相",
			"黑白",
			"反转颜色（概率性附加降调效果）",
			"高频重复",
			"随机调音（附加左右翻转效果）",
			"放大（附加增加音量）",
			"球面化",
			"镜像",
			"高对比（附加增加音量）",
			"过饱和（概率性附加升调效果）",
			"重说三（附加放大聚焦效果）"});
			this.YtpEffectsCheckList.Location = new System.Drawing.Point(9, 46);
			this.YtpEffectsCheckList.Margin = new System.Windows.Forms.Padding(2);
			this.YtpEffectsCheckList.Name = "YtpEffectsCheckList";
			this.YtpEffectsCheckList.Size = new System.Drawing.Size(633, 110);
			this.YtpEffectsCheckList.TabIndex = 1;
			this.YtpEffectsCheckList.Click += new System.EventHandler(this.YtpEffectsCheckList_SelectedIndexChanged);
			this.YtpEffectsCheckList.SelectedIndexChanged += new System.EventHandler(this.YtpEffectsCheckList_SelectedIndexChanged);
			this.YtpEffectsCheckList.DoubleClick += new System.EventHandler(this.YtpEffectsCheckList_SelectedIndexChanged);
			// 
			// YtpSelectInfo
			// 
			this.YtpSelectInfo.AutoSize = true;
			this.YtpSelectInfo.Dock = System.Windows.Forms.DockStyle.Top;
			this.YtpSelectInfo.Location = new System.Drawing.Point(5, 52);
			this.YtpSelectInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
			this.YtpSelectInfo.Name = "YtpSelectInfo";
			this.YtpSelectInfo.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
			this.YtpSelectInfo.Size = new System.Drawing.Size(160, 28);
			this.YtpSelectInfo.TabIndex = 13;
			this.YtpSelectInfo.Text = "已选中 0 项媒体素材。";
			this.YtpSelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// YtpLbl
			// 
			this.YtpLbl.AutoSize = true;
			this.YtpLbl.Dock = System.Windows.Forms.DockStyle.Top;
			this.YtpLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.YtpLbl.Location = new System.Drawing.Point(5, 5);
			this.YtpLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.YtpLbl.Name = "YtpLbl";
			this.YtpLbl.Padding = new System.Windows.Forms.Padding(0, 5, 0, 2);
			this.YtpLbl.Size = new System.Drawing.Size(514, 47);
			this.YtpLbl.TabIndex = 10;
			this.YtpLbl.Text = "在当前选项卡下单击“完成”按钮，将会生成 YTP 而不是音 MAD / YTPMV。\r\n除“生成音频”“生成视频”外其它的参数设置并不会在 YTP 中使用。";
			// 
			// HelperTab
			// 
			this.HelperTab.AutoScroll = true;
			this.HelperTab.BackColor = System.Drawing.Color.Transparent;
			this.HelperTab.Controls.Add(this.toolsTableLayoutPanel);
			this.HelperTab.Controls.Add(this.tableLayoutPanel19);
			this.HelperTab.Location = new System.Drawing.Point(4, 29);
			this.HelperTab.Margin = new System.Windows.Forms.Padding(2);
			this.HelperTab.Name = "HelperTab";
			this.HelperTab.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
			this.HelperTab.Size = new System.Drawing.Size(658, 607);
			this.HelperTab.TabIndex = 4;
			this.HelperTab.Text = "工具";
			this.HelperTab.UseVisualStyleBackColor = true;
			// 
			// toolsTableLayoutPanel
			// 
			this.toolsTableLayoutPanel.AutoSize = true;
			this.toolsTableLayoutPanel.ColumnCount = 1;
			this.toolsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.toolsTableLayoutPanel.Controls.Add(this.CloseAfterOpenHelperCheck, 0, 0);
			this.toolsTableLayoutPanel.Controls.Add(this.AutoLayoutTracksGroup, 0, 1);
			this.toolsTableLayoutPanel.Controls.Add(this.QuickSelectIntervalBtn, 0, 2);
			this.toolsTableLayoutPanel.Controls.Add(this.QuickNormalizeBtn, 0, 3);
			this.toolsTableLayoutPanel.Controls.Add(this.ReplaceClipsBtn, 0, 4);
			this.toolsTableLayoutPanel.Controls.Add(this.ChangeTuneMethodBtn, 0, 5);
			this.toolsTableLayoutPanel.Controls.Add(this.BatchSubtitleGenerationBtn, 0, 6);
			this.toolsTableLayoutPanel.Controls.Add(this.FindClipsBtn, 0, 7);
			this.toolsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolsTableLayoutPanel.Location = new System.Drawing.Point(2, 29);
			this.toolsTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.toolsTableLayoutPanel.Name = "toolsTableLayoutPanel";
			this.toolsTableLayoutPanel.RowCount = 8;
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
			this.toolsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.toolsTableLayoutPanel.Size = new System.Drawing.Size(633, 850);
			this.toolsTableLayoutPanel.TabIndex = 8;
			// 
			// CloseAfterOpenHelperCheck
			// 
			this.CloseAfterOpenHelperCheck.AutoSize = true;
			this.CloseAfterOpenHelperCheck.Checked = true;
			this.CloseAfterOpenHelperCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CloseAfterOpenHelperCheck.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CloseAfterOpenHelperCheck.Location = new System.Drawing.Point(8, 8);
			this.CloseAfterOpenHelperCheck.Margin = new System.Windows.Forms.Padding(8);
			this.CloseAfterOpenHelperCheck.Name = "CloseAfterOpenHelperCheck";
			this.CloseAfterOpenHelperCheck.Size = new System.Drawing.Size(617, 24);
			this.CloseAfterOpenHelperCheck.TabIndex = 0;
			this.CloseAfterOpenHelperCheck.Text = "操作完成之后关闭本对话框";
			this.CloseAfterOpenHelperCheck.UseVisualStyleBackColor = true;
			// 
			// AutoLayoutTracksGroup
			// 
			this.AutoLayoutTracksGroup.AutoSize = true;
			this.AutoLayoutTracksGroup.Controls.Add(this.tableLayoutPanel14);
			this.AutoLayoutTracksGroup.Dock = System.Windows.Forms.DockStyle.Top;
			this.AutoLayoutTracksGroup.Location = new System.Drawing.Point(2, 42);
			this.AutoLayoutTracksGroup.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksGroup.Name = "AutoLayoutTracksGroup";
			this.AutoLayoutTracksGroup.Padding = new System.Windows.Forms.Padding(5);
			this.AutoLayoutTracksGroup.Size = new System.Drawing.Size(629, 146);
			this.AutoLayoutTracksGroup.TabIndex = 6;
			this.AutoLayoutTracksGroup.TabStop = false;
			this.AutoLayoutTracksGroup.Text = "自动布局轨道";
			// 
			// tableLayoutPanel14
			// 
			this.tableLayoutPanel14.AutoSize = true;
			this.tableLayoutPanel14.ColumnCount = 1;
			this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel14.Controls.Add(this.AutoLayoutTracksLbl, 0, 0);
			this.tableLayoutPanel14.Controls.Add(this.AutoLayoutTracksSelectInfo, 0, 1);
			this.tableLayoutPanel14.Controls.Add(this.AutoLayoutTracksButtons, 0, 2);
			this.tableLayoutPanel14.Controls.Add(this.tableLayoutPanel15, 0, 3);
			this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel14.Location = new System.Drawing.Point(5, 25);
			this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel14.Name = "tableLayoutPanel14";
			this.tableLayoutPanel14.RowCount = 4;
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel14.Size = new System.Drawing.Size(619, 116);
			this.tableLayoutPanel14.TabIndex = 2;
			// 
			// AutoLayoutTracksLbl
			// 
			this.AutoLayoutTracksLbl.AutoSize = true;
			this.AutoLayoutTracksLbl.Dock = System.Windows.Forms.DockStyle.Top;
			this.AutoLayoutTracksLbl.Location = new System.Drawing.Point(2, 0);
			this.AutoLayoutTracksLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
			this.AutoLayoutTracksLbl.Name = "AutoLayoutTracksLbl";
			this.AutoLayoutTracksLbl.Size = new System.Drawing.Size(615, 20);
			this.AutoLayoutTracksLbl.TabIndex = 1;
			this.AutoLayoutTracksLbl.Text = "类 YTPMV 风格自动布局选中的轨道。";
			this.AutoLayoutTracksLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// AutoLayoutTracksSelectInfo
			// 
			this.AutoLayoutTracksSelectInfo.AutoSize = true;
			this.tableLayoutPanel14.SetColumnSpan(this.AutoLayoutTracksSelectInfo, 2);
			this.AutoLayoutTracksSelectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AutoLayoutTracksSelectInfo.Location = new System.Drawing.Point(2, 22);
			this.AutoLayoutTracksSelectInfo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 2);
			this.AutoLayoutTracksSelectInfo.Name = "AutoLayoutTracksSelectInfo";
			this.AutoLayoutTracksSelectInfo.Size = new System.Drawing.Size(615, 20);
			this.AutoLayoutTracksSelectInfo.TabIndex = 7;
			this.AutoLayoutTracksSelectInfo.Text = "已选中 0 个视频轨道。";
			this.AutoLayoutTracksSelectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
			this.AutoLayoutTracksButtons.Location = new System.Drawing.Point(0, 44);
			this.AutoLayoutTracksButtons.Margin = new System.Windows.Forms.Padding(0);
			this.AutoLayoutTracksButtons.Name = "AutoLayoutTracksButtons";
			this.AutoLayoutTracksButtons.RowCount = 1;
			this.AutoLayoutTracksButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.AutoLayoutTracksButtons.Size = new System.Drawing.Size(619, 36);
			this.AutoLayoutTracksButtons.TabIndex = 8;
			// 
			// GradientTracksBtn
			// 
			this.GradientTracksBtn.AutoSize = true;
			this.GradientTracksBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.GradientTracksBtn.Location = new System.Drawing.Point(278, 2);
			this.GradientTracksBtn.Margin = new System.Windows.Forms.Padding(2);
			this.GradientTracksBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.GradientTracksBtn.Name = "GradientTracksBtn";
			this.GradientTracksBtn.Size = new System.Drawing.Size(134, 32);
			this.GradientTracksBtn.TabIndex = 5;
			this.GradientTracksBtn.Text = "渐变轨道...";
			this.GradientTracksBtn.UseVisualStyleBackColor = true;
			this.GradientTracksBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// AutoLayoutTracksBox3dBtn
			// 
			this.AutoLayoutTracksBox3dBtn.AutoSize = true;
			this.AutoLayoutTracksBox3dBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.AutoLayoutTracksBox3dBtn.Location = new System.Drawing.Point(140, 2);
			this.AutoLayoutTracksBox3dBtn.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksBox3dBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.AutoLayoutTracksBox3dBtn.Name = "AutoLayoutTracksBox3dBtn";
			this.AutoLayoutTracksBox3dBtn.Size = new System.Drawing.Size(134, 32);
			this.AutoLayoutTracksBox3dBtn.TabIndex = 4;
			this.AutoLayoutTracksBox3dBtn.Text = "3D 方盒布局...";
			this.AutoLayoutTracksBox3dBtn.UseVisualStyleBackColor = true;
			this.AutoLayoutTracksBox3dBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// AutoLayoutTracksGridBtn
			// 
			this.AutoLayoutTracksGridBtn.AutoSize = true;
			this.AutoLayoutTracksGridBtn.Dock = System.Windows.Forms.DockStyle.Left;
			this.AutoLayoutTracksGridBtn.Location = new System.Drawing.Point(2, 2);
			this.AutoLayoutTracksGridBtn.Margin = new System.Windows.Forms.Padding(2);
			this.AutoLayoutTracksGridBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.AutoLayoutTracksGridBtn.Name = "AutoLayoutTracksGridBtn";
			this.AutoLayoutTracksGridBtn.Size = new System.Drawing.Size(134, 32);
			this.AutoLayoutTracksGridBtn.TabIndex = 3;
			this.AutoLayoutTracksGridBtn.Text = "网格布局...";
			this.AutoLayoutTracksGridBtn.UseVisualStyleBackColor = true;
			this.AutoLayoutTracksGridBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// tableLayoutPanel15
			// 
			this.tableLayoutPanel15.AutoSize = true;
			this.tableLayoutPanel15.ColumnCount = 4;
			this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel15.Controls.Add(this.TrackLegatoBtn, 2, 0);
			this.tableLayoutPanel15.Controls.Add(this.ClearTrackMotionBtn, 0, 0);
			this.tableLayoutPanel15.Controls.Add(this.ClearTrackEffectBtn, 1, 0);
			this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 80);
			this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel15.Name = "tableLayoutPanel15";
			this.tableLayoutPanel15.RowCount = 1;
			this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel15.Size = new System.Drawing.Size(619, 36);
			this.tableLayoutPanel15.TabIndex = 9;
			// 
			// TrackLegatoBtn
			// 
			this.TrackLegatoBtn.AutoSize = true;
			this.TrackLegatoBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TrackLegatoBtn.ForeColor = System.Drawing.Color.Red;
			this.TrackLegatoBtn.Location = new System.Drawing.Point(278, 2);
			this.TrackLegatoBtn.Margin = new System.Windows.Forms.Padding(2);
			this.TrackLegatoBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.TrackLegatoBtn.Name = "TrackLegatoBtn";
			this.TrackLegatoBtn.Padding = new System.Windows.Forms.Padding(0, 0, 14, 0);
			this.TrackLegatoBtn.Size = new System.Drawing.Size(134, 32);
			this.TrackLegatoBtn.TabIndex = 2;
			this.TrackLegatoBtn.Text = "填补轨道间隙";
			this.TrackLegatoBtn.UseVisualStyleBackColor = true;
			this.TrackLegatoBtn.Paint += new System.Windows.Forms.PaintEventHandler(this.TrackLegatoBtn_Paint);
			this.TrackLegatoBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TrackLegatoBtn_MouseDown);
			// 
			// ClearTrackMotionBtn
			// 
			this.ClearTrackMotionBtn.AutoSize = true;
			this.ClearTrackMotionBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClearTrackMotionBtn.ForeColor = System.Drawing.Color.Red;
			this.ClearTrackMotionBtn.Location = new System.Drawing.Point(2, 2);
			this.ClearTrackMotionBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ClearTrackMotionBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.ClearTrackMotionBtn.Name = "ClearTrackMotionBtn";
			this.ClearTrackMotionBtn.Size = new System.Drawing.Size(134, 32);
			this.ClearTrackMotionBtn.TabIndex = 0;
			this.ClearTrackMotionBtn.Text = "清除轨道运动";
			this.ClearTrackMotionBtn.UseVisualStyleBackColor = true;
			this.ClearTrackMotionBtn.Click += new System.EventHandler(this.ClearTrackMotionBtn_Click);
			// 
			// ClearTrackEffectBtn
			// 
			this.ClearTrackEffectBtn.AutoSize = true;
			this.ClearTrackEffectBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ClearTrackEffectBtn.ForeColor = System.Drawing.Color.Red;
			this.ClearTrackEffectBtn.Location = new System.Drawing.Point(140, 2);
			this.ClearTrackEffectBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ClearTrackEffectBtn.MaximumSize = new System.Drawing.Size(300, 32);
			this.ClearTrackEffectBtn.Name = "ClearTrackEffectBtn";
			this.ClearTrackEffectBtn.Size = new System.Drawing.Size(134, 32);
			this.ClearTrackEffectBtn.TabIndex = 1;
			this.ClearTrackEffectBtn.Text = "清除轨道效果";
			this.ClearTrackEffectBtn.UseVisualStyleBackColor = true;
			this.ClearTrackEffectBtn.Click += new System.EventHandler(this.ClearTrackEffectBtn_Click);
			// 
			// QuickSelectIntervalBtn
			// 
			this.QuickSelectIntervalBtn.CommandLink = true;
			this.QuickSelectIntervalBtn.CommandLinkNote = "本功能旨在辅助用户每隔一个或几个选中一个素材，然后可以执行“粘贴视频事件”等操作。\r\n已选中 0 个轨道剪辑。";
			this.QuickSelectIntervalBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuickSelectIntervalBtn.Location = new System.Drawing.Point(2, 192);
			this.QuickSelectIntervalBtn.Margin = new System.Windows.Forms.Padding(2);
			this.QuickSelectIntervalBtn.Name = "QuickSelectIntervalBtn";
			this.QuickSelectIntervalBtn.Size = new System.Drawing.Size(629, 106);
			this.QuickSelectIntervalBtn.TabIndex = 11;
			this.QuickSelectIntervalBtn.Text = "快速间隔选择";
			this.QuickSelectIntervalBtn.UseVisualStyleBackColor = true;
			this.QuickSelectIntervalBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// QuickNormalizeBtn
			// 
			this.QuickNormalizeBtn.CommandLink = true;
			this.QuickNormalizeBtn.CommandLinkNote = "将选中的多个音频轨道剪辑全部规范化音量。\r\n已选中 0 个音频轨道剪辑。";
			this.QuickNormalizeBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.QuickNormalizeBtn.Location = new System.Drawing.Point(2, 302);
			this.QuickNormalizeBtn.Margin = new System.Windows.Forms.Padding(2);
			this.QuickNormalizeBtn.Name = "QuickNormalizeBtn";
			this.QuickNormalizeBtn.Size = new System.Drawing.Size(629, 106);
			this.QuickNormalizeBtn.TabIndex = 12;
			this.QuickNormalizeBtn.Text = "快速规范音量";
			this.QuickNormalizeBtn.UseVisualStyleBackColor = true;
			this.QuickNormalizeBtn.Click += new System.EventHandler(this.QuickNormalizeBtn_Click);
			// 
			// ReplaceClipsBtn
			// 
			this.ReplaceClipsBtn.CommandLink = true;
			this.ReplaceClipsBtn.CommandLinkNote = "将多个轨道剪辑替换为指定的新轨道剪辑。\r\n已选中 0 个轨道剪辑。";
			this.ReplaceClipsBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ReplaceClipsBtn.Location = new System.Drawing.Point(2, 412);
			this.ReplaceClipsBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ReplaceClipsBtn.Name = "ReplaceClipsBtn";
			this.ReplaceClipsBtn.Size = new System.Drawing.Size(629, 106);
			this.ReplaceClipsBtn.TabIndex = 13;
			this.ReplaceClipsBtn.Text = "替换轨道素材";
			this.ReplaceClipsBtn.UseVisualStyleBackColor = true;
			this.ReplaceClipsBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// ChangeTuneMethodBtn
			// 
			this.ChangeTuneMethodBtn.CommandLink = true;
			this.ChangeTuneMethodBtn.CommandLinkNote = "将多个音频轨道剪辑统一更改为指定的调音算法。\r\n已选中 0 个音频轨道剪辑。";
			this.ChangeTuneMethodBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ChangeTuneMethodBtn.Location = new System.Drawing.Point(2, 522);
			this.ChangeTuneMethodBtn.Margin = new System.Windows.Forms.Padding(2);
			this.ChangeTuneMethodBtn.Name = "ChangeTuneMethodBtn";
			this.ChangeTuneMethodBtn.Size = new System.Drawing.Size(629, 106);
			this.ChangeTuneMethodBtn.TabIndex = 14;
			this.ChangeTuneMethodBtn.Text = "更改调音算法";
			this.ChangeTuneMethodBtn.UseVisualStyleBackColor = true;
			this.ChangeTuneMethodBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// BatchSubtitleGenerationBtn
			// 
			this.BatchSubtitleGenerationBtn.CommandLink = true;
			this.BatchSubtitleGenerationBtn.CommandLinkNote = "预先设定好“字幕和文字”的预设，然后在此添加多行文本。";
			this.BatchSubtitleGenerationBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BatchSubtitleGenerationBtn.Location = new System.Drawing.Point(2, 632);
			this.BatchSubtitleGenerationBtn.Margin = new System.Windows.Forms.Padding(2);
			this.BatchSubtitleGenerationBtn.Name = "BatchSubtitleGenerationBtn";
			this.BatchSubtitleGenerationBtn.Size = new System.Drawing.Size(629, 106);
			this.BatchSubtitleGenerationBtn.TabIndex = 15;
			this.BatchSubtitleGenerationBtn.Text = "批量生成字幕";
			this.BatchSubtitleGenerationBtn.UseVisualStyleBackColor = true;
			this.BatchSubtitleGenerationBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// FindClipsBtn
			// 
			this.FindClipsBtn.CommandLink = true;
			this.FindClipsBtn.CommandLinkNote = "根据指定的条件（如剪辑名称、与选中剪辑相同的素材等）选中符合条件的所有轨道剪辑。";
			this.FindClipsBtn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FindClipsBtn.Location = new System.Drawing.Point(2, 742);
			this.FindClipsBtn.Margin = new System.Windows.Forms.Padding(2);
			this.FindClipsBtn.Name = "FindClipsBtn";
			this.FindClipsBtn.Size = new System.Drawing.Size(629, 106);
			this.FindClipsBtn.TabIndex = 16;
			this.FindClipsBtn.Text = "查找轨道素材";
			this.FindClipsBtn.UseVisualStyleBackColor = true;
			this.FindClipsBtn.Click += new System.EventHandler(this.ReadyToShowHelperDialog);
			// 
			// tableLayoutPanel19
			// 
			this.tableLayoutPanel19.AutoSize = true;
			this.tableLayoutPanel19.ColumnCount = 1;
			this.tableLayoutPanel19.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel19.Controls.Add(this.HelperLbl, 0, 0);
			this.tableLayoutPanel19.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel19.Location = new System.Drawing.Point(2, 4);
			this.tableLayoutPanel19.Margin = new System.Windows.Forms.Padding(2);
			this.tableLayoutPanel19.Name = "tableLayoutPanel19";
			this.tableLayoutPanel19.RowCount = 1;
			this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel19.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel19.Size = new System.Drawing.Size(633, 25);
			this.tableLayoutPanel19.TabIndex = 7;
			// 
			// HelperLbl
			// 
			this.HelperLbl.AutoSize = true;
			this.HelperLbl.Dock = System.Windows.Forms.DockStyle.Top;
			this.HelperLbl.Font = new System.Drawing.Font("微软雅黑", 9F);
			this.HelperLbl.Location = new System.Drawing.Point(2, 0);
			this.HelperLbl.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.HelperLbl.Name = "HelperLbl";
			this.HelperLbl.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.HelperLbl.Size = new System.Drawing.Size(629, 25);
			this.HelperLbl.TabIndex = 2;
			this.HelperLbl.Text = "以下功能只是一些独立的辅助功能，与其它生成音视频的参数无关。";
			// 
			// TrackLegatoMenu
			// 
			this.TrackLegatoMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.TrackLegatoMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.stackingTracksToolStripMenuItem,
			this.limitStretchLegatoTracksToolStripMenuItem,
			this.stretchLegatoTracksToolStripMenuItem,
			this.lengthenLegatoToolStripMenuItem});
			this.TrackLegatoMenu.Name = "TrackLegatoMenu";
			this.TrackLegatoMenu.Size = new System.Drawing.Size(334, 100);
			// 
			// stackingTracksToolStripMenuItem
			// 
			this.stackingTracksToolStripMenuItem.Name = "stackingTracksToolStripMenuItem";
			this.stackingTracksToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
			this.stackingTracksToolStripMenuItem.Text = "堆积素材";
			this.stackingTracksToolStripMenuItem.Click += new System.EventHandler(this.TrackLegatoMenuItems_Click);
			// 
			// limitStretchLegatoTracksToolStripMenuItem
			// 
			this.limitStretchLegatoTracksToolStripMenuItem.Name = "limitStretchLegatoTracksToolStripMenuItem";
			this.limitStretchLegatoTracksToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
			this.limitStretchLegatoTracksToolStripMenuItem.Text = "拉伸素材（限制在拉伸极限范围之内）";
			this.limitStretchLegatoTracksToolStripMenuItem.Click += new System.EventHandler(this.TrackLegatoMenuItems_Click);
			// 
			// stretchLegatoTracksToolStripMenuItem
			// 
			this.stretchLegatoTracksToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold);
			this.stretchLegatoTracksToolStripMenuItem.Name = "stretchLegatoTracksToolStripMenuItem";
			this.stretchLegatoTracksToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
			this.stretchLegatoTracksToolStripMenuItem.Text = "拉伸素材";
			this.stretchLegatoTracksToolStripMenuItem.Click += new System.EventHandler(this.TrackLegatoMenuItems_Click);
			// 
			// lengthenLegatoToolStripMenuItem
			// 
			this.lengthenLegatoToolStripMenuItem.Name = "lengthenLegatoToolStripMenuItem";
			this.lengthenLegatoToolStripMenuItem.Size = new System.Drawing.Size(333, 24);
			this.lengthenLegatoToolStripMenuItem.Text = "改变素材持续时间";
			this.lengthenLegatoToolStripMenuItem.Click += new System.EventHandler(this.TrackLegatoMenuItems_Click);
			// 
			// ConfigForm
			// 
			this.AcceptButton = this.OkBtn;
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.CancelBtn;
			this.ClientSize = new System.Drawing.Size(682, 721);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.menu);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
			this.Location = new System.Drawing.Point(40, 40);
			this.MainMenuStrip = this.menu;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(700, 768);
			this.Name = "ConfigForm";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Otomad Helper for Vegas - 配置";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
			this.Resize += new System.EventHandler(this.ConfigForm_Resize);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PreviewBeepDurationBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineThicknessBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffLineSpacingBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfacePositionBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.StaffSurfaceWidthBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.YtpMinLenBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.YtpMaxLenBox)).EndInit();
			this.menu.ResumeLayout(false);
			this.menu.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.Tabs.ResumeLayout(false);
			this.SourceTab.ResumeLayout(false);
			this.SourceTab.PerformLayout();
			this.MidiConfigGroup.ResumeLayout(false);
			this.MidiConfigGroup.PerformLayout();
			this.MidiConfigTablePanel.ResumeLayout(false);
			this.MidiConfigTablePanel.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.flowLayoutPanel3.ResumeLayout(false);
			this.flowLayoutPanel3.PerformLayout();
			this.MidiBpmFlowPanel.ResumeLayout(false);
			this.MidiBpmFlowPanel.PerformLayout();
			this.MidiDynamicTempoFlow.ResumeLayout(false);
			this.MidiDynamicTempoFlow.PerformLayout();
			this.flowLayoutPanel2.ResumeLayout(false);
			this.flowLayoutPanel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MidiCustomBpmBox)).EndInit();
			this.tableLayoutPanel12.ResumeLayout(false);
			this.tableLayoutPanel12.PerformLayout();
			this.SourceConfigGroup.ResumeLayout(false);
			this.SourceConfigGroup.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.flowLayoutPanel9.ResumeLayout(false);
			this.flowLayoutPanel9.PerformLayout();
			this.flowLayoutPanel11.ResumeLayout(false);
			this.flowLayoutPanel11.PerformLayout();
			this.AudioTab.ResumeLayout(false);
			this.AudioTab.PerformLayout();
			this.AudioParamsGroup.ResumeLayout(false);
			this.AudioParamsGroup.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.AudioTuneGroup.ResumeLayout(false);
			this.AudioTuneGroup.PerformLayout();
			this.AudioTuneTablePanel.ResumeLayout(false);
			this.AudioTuneTablePanel.PerformLayout();
			this.flowLayoutPanel10.ResumeLayout(false);
			this.flowLayoutPanel10.PerformLayout();
			this.flowLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel17.ResumeLayout(false);
			this.tableLayoutPanel17.PerformLayout();
			this.AudioPreviewAttrLayoutPanel.ResumeLayout(false);
			this.AudioPreviewAttrLayoutPanel.PerformLayout();
			this.flowLayoutPanel5.ResumeLayout(false);
			this.flowLayoutPanel5.PerformLayout();
			this.VideoTab.ResumeLayout(false);
			this.VideoTab.PerformLayout();
			this.VideoParamsGroup.ResumeLayout(false);
			this.VideoParamsGroup.PerformLayout();
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.VideoEffectsGroup.ResumeLayout(false);
			this.VideoEffectsGroup.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.flowLayoutPanel7.ResumeLayout(false);
			this.flowLayoutPanel7.PerformLayout();
			this.SheetTab.ResumeLayout(false);
			this.SheetTab.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.StaffParamsGroup.ResumeLayout(false);
			this.StaffParamsGroup.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.StaffNotesShiftBox)).EndInit();
			this.flowLayoutPanel8.ResumeLayout(false);
			this.flowLayoutPanel8.PerformLayout();
			this.YtpTab.ResumeLayout(false);
			this.YtpTab.PerformLayout();
			this.YtpParamsGroup.ResumeLayout(false);
			this.YtpParamsGroup.PerformLayout();
			this.tableLayoutPanel16.ResumeLayout(false);
			this.tableLayoutPanel16.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.YtpClipsCountBox)).EndInit();
			this.YtpEffectsGroup.ResumeLayout(false);
			this.YtpEffectsGroup.PerformLayout();
			this.HelperTab.ResumeLayout(false);
			this.HelperTab.PerformLayout();
			this.toolsTableLayoutPanel.ResumeLayout(false);
			this.toolsTableLayoutPanel.PerformLayout();
			this.AutoLayoutTracksGroup.ResumeLayout(false);
			this.AutoLayoutTracksGroup.PerformLayout();
			this.tableLayoutPanel14.ResumeLayout(false);
			this.tableLayoutPanel14.PerformLayout();
			this.AutoLayoutTracksButtons.ResumeLayout(false);
			this.AutoLayoutTracksButtons.PerformLayout();
			this.tableLayoutPanel15.ResumeLayout(false);
			this.tableLayoutPanel15.PerformLayout();
			this.tableLayoutPanel19.ResumeLayout(false);
			this.tableLayoutPanel19.PerformLayout();
			this.TrackLegatoMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public System.Windows.Forms.Button OkBtn;
		public System.Windows.Forms.Button CancelBtn;
		public System.Windows.Forms.Button AboutBtn;
		public System.Windows.Forms.LinkLabel UserHelpLink;
		public System.Windows.Forms.ColorDialog StaffLineColorDialog;
		public System.Windows.Forms.ToolTip Balloon;
		public System.Windows.Forms.MenuStrip menu;
		public System.Windows.Forms.ToolStripMenuItem fileMenuItem;
		public System.Windows.Forms.ToolStripMenuItem saveConfigToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem resetConfigToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		public System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		public System.Windows.Forms.ToolStripMenuItem languageToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem githubToolStripMenuItem;
		public System.Windows.Forms.TabControl Tabs;
		public System.Windows.Forms.TabPage SourceTab;
		public System.Windows.Forms.Label WarningInfoLabel;
		public System.Windows.Forms.GroupBox MidiConfigGroup;
		public System.Windows.Forms.TableLayoutPanel MidiConfigTablePanel;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		public System.Windows.Forms.Label MidiStartSecondLbl;
		public TimecodeBox MidiStartSecondBox;
		public System.Windows.Forms.Label MidiEndSecondLbl;
		public TimecodeBox MidiEndSecondBox;
		public System.Windows.Forms.Label MidiBpmLbl;
		public System.Windows.Forms.Label MidiChannelLbl;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		public System.Windows.Forms.Button ChooseMidiBtn;
		public System.Windows.Forms.TextBox ChooseMidiText;
		public System.Windows.Forms.Label ChooseMidiLbl;
		public System.Windows.Forms.FlowLayoutPanel MidiBpmFlowPanel;
		public Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton MidiMidiBpmCheck;
		public Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton MidiProjectBpmCheck;
		public System.Windows.Forms.GroupBox SourceConfigGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel9;
		public Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton GenerateAtBeginRadio;
		public Otomad.VegasScript.OtomadHelper.V4.GroupedRadioButton GenerateAtCursorRadio;
		public System.Windows.Forms.Label GenerateAtLbl;
		public System.Windows.Forms.Label ChooseSourceLbl;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		public System.Windows.Forms.ComboBox ChooseSourceCombo;
		public System.Windows.Forms.Button ChooseSourceBtn;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		public System.Windows.Forms.Label SourceStartTimeLbl;
		public TimecodeBox SourceStartTimeText;
		public System.Windows.Forms.Label SourceEndTimeLbl;
		public TimecodeBox SourceEndTimeText;
		public System.Windows.Forms.TabPage AudioTab;
		public System.Windows.Forms.GroupBox AudioParamsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		public System.Windows.Forms.ComboBox AudioFadeOutCurveCombo;
		public System.Windows.Forms.Label AudioFadeInLbl;
		public System.Windows.Forms.Label AudioFadeOutLbl;
		public IntegerTrackWithBox AudioFadeInBox;
		public IntegerTrackWithBox AudioFadeOutBox;
		public System.Windows.Forms.ComboBox AudioFadeInCurveCombo;
		public System.Windows.Forms.GroupBox AudioTuneGroup;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
		public System.Windows.Forms.CheckBox AudioConfigCheck;
		public System.Windows.Forms.CheckBox AudioScratchCheck;
		public System.Windows.Forms.CheckBox AudioLoopCheck;
		public System.Windows.Forms.CheckBox AudioNormalizeCheck;
		public System.Windows.Forms.TabPage VideoTab;
		public System.Windows.Forms.GroupBox VideoParamsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		public IntegerTrackWithBox VideoEndVerticalTransBox;
		public IntegerTrackWithBox VideoStartVerticalTransBox;
		public IntegerTrackWithBox VideoEndHorizontalTransBox;
		public IntegerTrackWithBox VideoStartHorizontalTransBox;
		public IntegerTrackWithBox VideoEndRotationBox;
		public IntegerTrackWithBox VideoStartRotationBox;
		public IntegerTrackWithBox VideoEndSizeBox;
		public System.Windows.Forms.ComboBox VideoStartSizeCurveCombo;
		public IntegerTrackWithBox VideoStartSizeBox;
		public System.Windows.Forms.Label VideoEndVerticalTransLbl;
		public System.Windows.Forms.Label VideoStartHorizontalTransLbl;
		public System.Windows.Forms.Label VideoStartVerticalTransLbl;
		public System.Windows.Forms.Label VideoEndHorizontalTransLbl;
		public System.Windows.Forms.Label VideoEndRotationLbl;
		public System.Windows.Forms.Label VideoStartRotationLbl;
		public System.Windows.Forms.Label VideoEndSizeLbl;
		public System.Windows.Forms.Label VideoStartSizeLbl;
		public System.Windows.Forms.ComboBox VideoFadeOutCurveCombo;
		public System.Windows.Forms.Label VideoFadeInLbl;
		public System.Windows.Forms.Label VideoFadeOutLbl;
		public IntegerTrackWithBox VideoFadeInBox;
		public IntegerTrackWithBox VideoFadeOutBox;
		public System.Windows.Forms.ComboBox VideoFadeInCurveCombo;
		public System.Windows.Forms.GroupBox VideoEffectsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		public System.Windows.Forms.ComboBox VideoEffectInitialValueCombo;
		public System.Windows.Forms.Label VideoEffectLbl;
		public System.Windows.Forms.Label VideoEffectInitialValueLbl;
		public System.Windows.Forms.ComboBox VideoEffectCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel7;
		public System.Windows.Forms.CheckBox VideoConfigCheck;
		public System.Windows.Forms.CheckBox VideoScratchCheck;
		public System.Windows.Forms.CheckBox VideoLoopCheck;
		public System.Windows.Forms.CheckBox VideoFreezeFirstFrameCheck;
		public System.Windows.Forms.CheckBox VideoFreezeLastFrameCheck;
		public System.Windows.Forms.CheckBox VideoLegatoCheck;
		public System.Windows.Forms.TabPage SheetTab;
		public System.Windows.Forms.TabPage HelperTab;
		public ToolStripRadioButtonMenuItem chineseToolStripMenuItem;
		public ToolStripRadioButtonMenuItem englishToolStripMenuItem;
		public ToolStripRadioButtonMenuItem japaneseToolStripMenuItem;
		public ToolStripRadioButtonMenuItem russianToolStripMenuItem;
		public System.Windows.Forms.Panel panel1;
		public ToolStripRadioButtonMenuItem tchineseToolStripMenuItem;
		public System.Windows.Forms.CheckBox AudioFreezeLastFrameCheck;
		public System.Windows.Forms.CheckBox AudioLegatoCheck;
		public System.Windows.Forms.TableLayoutPanel AudioTuneTablePanel;
		public System.Windows.Forms.Label AudioTuneMethodLbl;
		public System.Windows.Forms.Label AudioBasePitchLbl;
		public System.Windows.Forms.ComboBox AudioTuneMethodCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
		public System.Windows.Forms.ComboBox AudioMainKeyCombo;
		public System.Windows.Forms.ComboBox AudioMainOctaveCombo;
		public System.Windows.Forms.ComboBox VideoGlowCurveCombo;
		public IntegerTrackWithBox VideoGlowBox;
		public System.Windows.Forms.Label VideoGlowLbl;
		public IntegerTrackWithBox VideoGlowBrightBox;
		public System.Windows.Forms.Label VideoGlowBrightLbl;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
		public System.Windows.Forms.Button PreviewBasePitchBtn;
		public System.Windows.Forms.Button PreviewAudioBtn;
		public System.Windows.Forms.TabPage YtpTab;
		public System.Windows.Forms.ToolStripMenuItem exitDiscardingChangesToolStripMenuItem;
		public System.Windows.Forms.ComboBox AudioStretchAttrCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel10;
		public System.Windows.Forms.CheckBox AudioLockStretchPitchCheck;
		public System.Windows.Forms.CheckBox AudioReserveFormantCheck;
		public System.Windows.Forms.Label AudioStretchAttrLbl;
		public System.Windows.Forms.Label AudioPreviewLbl;
		public System.Windows.Forms.Label AudioLockAttrLbl;
		public System.Windows.Forms.TableLayoutPanel toolsTableLayoutPanel;
		public System.Windows.Forms.GroupBox AutoLayoutTracksGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
		public System.Windows.Forms.Label AutoLayoutTracksSelectInfo;
		public System.Windows.Forms.Label AutoLayoutTracksLbl;
		public System.Windows.Forms.TableLayoutPanel AutoLayoutTracksButtons;
		public System.Windows.Forms.Button GradientTracksBtn;
		public System.Windows.Forms.Button AutoLayoutTracksBox3dBtn;
		public System.Windows.Forms.Button AutoLayoutTracksGridBtn;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
		public System.Windows.Forms.Button ClearTrackMotionBtn;
		public System.Windows.Forms.Button ClearTrackEffectBtn;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel19;
		public System.Windows.Forms.Label HelperLbl;
		public System.Windows.Forms.GroupBox YtpParamsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel16;
		public NumericUpDownWithUnit YtpClipsCountBox;
		public System.Windows.Forms.Label YtpClipsCountLbl;
		public NumericUpDownWithUnit YtpMaxLenBox;
		public NumericUpDownWithUnit YtpMinLenBox;
		public System.Windows.Forms.Label YtpMinLenLbl;
		public System.Windows.Forms.Label YtpMaxLenLbl;
		public System.Windows.Forms.GroupBox YtpEffectsGroup;
		public System.Windows.Forms.CheckBox YtpEnableAllEffectsCheck;
		public System.Windows.Forms.CheckedListBox YtpEffectsCheckList;
		public System.Windows.Forms.Label YtpLbl;
		public System.Windows.Forms.GroupBox StaffParamsGroup;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		public NumericUpDownWithUnit StaffNotesShiftBox;
		public NumericUpDownWithUnit StaffSurfacePositionBox;
		public NumericUpDownWithUnit StaffSurfaceWidthBox;
		public System.Windows.Forms.Label StaffSurfaceWidthLbl;
		public System.Windows.Forms.Label StaffSurfacePositionLbl;
		public System.Windows.Forms.Label StaffClefLbl;
		public System.Windows.Forms.Label StaffLineSpacingLbl;
		public System.Windows.Forms.ComboBox StaffClefCombo;
		public NumericUpDownWithUnit StaffLineSpacingBox;
		public System.Windows.Forms.Label StaffNotesShiftLbl;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel8;
		public System.Windows.Forms.CheckBox StaffVisualizerConfigCheck;
		public System.Windows.Forms.Label SheetConfigInfoLabel;
		public System.Windows.Forms.FlowLayoutPanel AudioPreviewAttrLayoutPanel;
		public System.Windows.Forms.ComboBox PreviewBeepWaveFormCombo;
		public NumericUpDownWithUnit PreviewBeepDurationBox;
		public System.Windows.Forms.CheckBox PreviewTuneAudioCheck;
		public System.Windows.Forms.Label AudioPreviewAttrLbl;
		public CommandLinkButton QuickSelectIntervalBtn;
		public CommandLinkButton ReplaceClipsBtn;
		public CommandLinkButton ChangeTuneMethodBtn;
		public System.Windows.Forms.ToolStripMenuItem whyOkBtnIsDisabledToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem checkUpdateToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem versionToolStripMenuItem;
		public System.Windows.Forms.Label YtpSelectInfo;
		public System.Windows.Forms.CheckBox CloseAfterOpenHelperCheck;
		public System.Windows.Forms.Button TrackLegatoBtn;
		public System.Windows.Forms.ContextMenuStrip TrackLegatoMenu;
		public System.Windows.Forms.ToolStripMenuItem stackingTracksToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem limitStretchLegatoTracksToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem stretchLegatoTracksToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem lengthenLegatoToolStripMenuItem;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
		public System.Windows.Forms.Label MidiBeatLbl;
		public System.Windows.Forms.Label MidiBeatTxt;
		public System.Windows.Forms.ComboBox PreviewBeepEngineCombo;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		public GroupedRadioButton MidiCustomBpmCheck;
		public NumericUpDownWithUnit MidiCustomBpmBox;
		public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel11;
		public GroupedRadioButton GenerateAtCustomRadio;
		public System.Windows.Forms.TextBox GenerateAtCustomText;
		public System.Windows.Forms.FlowLayoutPanel MidiDynamicTempoFlow;
		public GroupedRadioButton MidiDynamicMidiBpmCheck;
		public System.Windows.Forms.ComboBox MidiDynamicMidiBpmFormCombo;
		public System.Windows.Forms.ToolStripMenuItem pitchShiftPresetMenuItem;
		public System.Windows.Forms.ToolStripMenuItem loadPresetsToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem unloadPresetsToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		public System.Windows.Forms.ToolStripMenuItem formSizeToolStripMenuItem;
		public System.Windows.Forms.CheckBox CreateEventGroupCheck;
		public CommandLinkButton BatchSubtitleGenerationBtn;
		public CommandLinkButton FindClipsBtn;
		public CommandLinkButton QuickNormalizeBtn;
		public System.Windows.Forms.ToolStripMenuItem restoreDefaultFormSizeToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem rememberFormSizeToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		public System.Windows.Forms.CheckBox VideoMultitrackForChordsCheck;
		public System.Windows.Forms.ToolStripMenuItem rememberOnceFormSizeToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem updateLogsToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		public System.Windows.Forms.ToolStripMenuItem englishDocumentationsToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem documentationEnglishToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem tutorialVideoEnglishToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem chineseDocumentationsToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem troubleShootingToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		public System.Windows.Forms.ToolStripMenuItem releaseNotesV4_9_25_0ToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem releaseNotesV4_10_17_0ToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		public System.Windows.Forms.ToolStripMenuItem documentationV0_1ToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem staffVisualizerV0_1ToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem tutorialVideoV0_1ToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem roadmapToolStripMenuItem;
		public IntegerTrackWithBox VideoEndContrastBox;
		public IntegerTrackWithBox VideoEndSaturationBox;
		public IntegerTrackWithBox VideoEndHueBox;
		public IntegerTrackWithBox VideoEndThresholdBox;
		public IntegerTrackWithBox VideoStartContrastBox;
		public IntegerTrackWithBox VideoStartSaturationBox;
		public IntegerTrackWithBox VideoStartThresholdBox;
		public IntegerTrackWithBox VideoStartHueBox;
		public System.Windows.Forms.Label VideoStartThresholdLbl;
		public System.Windows.Forms.Label VideoEndThresholdLbl;
		public System.Windows.Forms.Label VideoEndSaturationLbl;
		public System.Windows.Forms.Label VideoEndContrastLbl;
		public System.Windows.Forms.Label VideoStartContrastLbl;
		public System.Windows.Forms.Label VideoStartHueLbl;
		public System.Windows.Forms.Label VideoStartSaturationLbl;
		public System.Windows.Forms.Label VideoEndHueLbl;
		public System.Windows.Forms.ComboBox VideoStartContrastCurveCombo;
		public System.Windows.Forms.ComboBox VideoStartHueCurveCombo;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		public System.Windows.Forms.ComboBox MidiChannelCombo;
		public System.Windows.Forms.Button MidiChannelAdvancedBtn;
		public System.Windows.Forms.GroupBox groupBox1;
		public System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		public System.Windows.Forms.CheckBox StaffGenerateCheck;
		public System.Windows.Forms.Button StaffLineColorBtn;
		public System.Windows.Forms.Label StaffLineColorLbl;
		public NumericUpDownWithUnit StaffLineThicknessBox;
		public System.Windows.Forms.Label StaffLineThicknessLbl;
		public System.Windows.Forms.CheckBox StaffRelativeValueCheck;
		public System.Windows.Forms.CheckBox StaffLegacyEngineCheck;
		public System.Windows.Forms.ComboBox VideoStartThresholdCurveCombo;
		public System.Windows.Forms.ComboBox VideoStartSaturationCurveCombo;
	}
	#endregion

	public partial class ConfigForm : Form, IConfigIniUser, IInterpret {
		public bool AcceptConfig = false;
		public static Icon icon;
		#if VEGAS_ENVIRONMENT
		public ConfigIni configIni { get { return parent.configIni; } set { parent.configIni = value; } }
		public readonly EntryPoint parent;
		private Vegas vegas { get { return parent.vegas; } }
		public readonly Timecode originalCursorPosition;
		public bool IsGenerateYtp = false;
		private bool NeedSaveIni = true;
		public readonly string ScriptPath;
		private string browseClipName = "";
		private bool openReplaceClipDirectly = false;
		private bool isAlertedUnsupport = false;
		#endif

		/// <summary>
		/// ConfigForm 脚本对话框窗体的入口方法。
		/// </summary>
		/// <param name="entryPoint">调用本对象的父对象，也就是 Vegas 脚本的入口类</param>
		public ConfigForm(EntryPoint entryPoint) {
			InitializeComponent();
			parent = entryPoint;
			originalCursorPosition = vegas.Transport.CursorPosition;

			#region 国际化
			ScriptPath = Script.File;
			configIni = new ConfigIni(Path.r(
				vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData),
				new Path(ScriptPath) { Extension = "ini" }.FullFileName
			).FullPath, this);
			GetSystemLanguage();
			try {
				LanguageBackup = Language = configIni.Read("Language", "", "Personalize");
			} catch (Exception) { }
			#if INTERNATIONALIZED
			Translate();
			#endif
			#endregion

			#region 检查版本支持情况
			if (!isAlertedUnsupport && parent.Supported == SupportVegasVersionState.UNSUPPORTED) {
				EntryPoint.AlertUnsupportVersion();
				isAlertedUnsupport = true;
			}
			#endregion

			#region MIDI 速度控制点击事件
			MidiCustomBpmCheck.CheckedChanged += (sender, e) => { MidiCustomBpmBox.Enabled = MidiCustomBpmCheck.Checked; };
			GenerateAtCustomRadio.CheckedChanged += (sender, e) => { GenerateAtCustomText.Enabled = GenerateAtCustomRadio.Checked; };
			MidiProjectBpmCheck.Text = Lang.str.midi_project_bpm + Lang.str.colon + ProcessBpmDouble(parent.ProjectBpm);
			MidiCustomBpmBox.Value = (decimal)Math.Max(parent.ProjectBpm, (double)MidiCustomBpmBox.Minimum);
			GenerateAtBeginRadio.Text = Lang.str.generate_at_begin + Lang.str.colon + Timecode.FromMilliseconds(0).ToPositionString();
			GenerateAtCursorRadio.Text = Lang.str.generate_at_cursor + Lang.str.colon + vegas.Transport.CursorPosition.ToPositionString();
			#endregion

			#region 浏览并打开媒体文件
			parent.audioVideoEnabledTable = new EntryPoint.AudioVideoEnabledTable(parent);
			ChooseSourceCombo_InitSourceNames();
			ChooseSourceCombo_SelectedIndexChanged(null, null);
			HelperSelectedCount();
			#endregion

			#region 菜单选项
			saveConfigToolStripMenuItem.Click += (sender, e) => { SaveIni(); };
			exitToolStripMenuItem.Click += new EventHandler(CancelBtn_Click);
			aboutToolStripMenuItem.Click += new EventHandler(AboutBtn_Click);
			BindMenuLink(checkUpdateToolStripMenuItem, Links.GITHUB_LATEST);
			BindMenuLink(githubToolStripMenuItem, Links.REPOSITORY);
			BindMenuLink(roadmapToolStripMenuItem, Links.ROADMAP);
			BindMenuLink(updateLogsToolStripMenuItem, Links.UPDATE_LOGS);
			BindMenuLink(documentationEnglishToolStripMenuItem, Links.DOCUMENTATION_ENGLISH);
			BindMenuLink(tutorialVideoEnglishToolStripMenuItem, Links.TUTORIAL_VIDEO_ENGLISH);
			BindMenuLink(troubleShootingToolStripMenuItem, Links.TROUBLE_SHOOTING);
			BindMenuLink(releaseNotesV4_9_25_0ToolStripMenuItem, Links.RELEASE_NOTES_V4_9_25_0);
			BindMenuLink(releaseNotesV4_10_17_0ToolStripMenuItem, Links.RELEASE_NOTES_V4_10_17_0);
			BindMenuLink(documentationV0_1ToolStripMenuItem, Links.DOCUMENTATION_V0_1);
			BindMenuLink(staffVisualizerV0_1ToolStripMenuItem, Links.STAFF_VISUALIZER_DOCUMENTATION_V0_1);
			BindMenuLink(tutorialVideoV0_1ToolStripMenuItem, Links.TUTORIAL_VIDEO_V0_1);

			foreach (ToolStripItem item in languageToolStripMenuItem.DropDownItems)
				item.Click += new EventHandler(LanguageStripMenuItem_Click);
			TrackLegatoMenu.Renderer = menu.Renderer = new Windows10StyledContextMenuStripRenderer();
			#endregion

			#region 复选框设置、下拉菜单默认值
			{
				EventHandler e = new EventHandler(SetCheckedEnabled);
				VideoConfigCheck.CheckedChanged += e;
				AudioConfigCheck.CheckedChanged += e;
				StaffVisualizerConfigCheck.CheckedChanged += e;
				StaffGenerateCheck.CheckedChanged += e;
				AudioTuneMethodCombo.SelectedIndexChanged += e;
				AudioStretchAttrCombo.SelectedIndexChanged += e;
				AudioLockStretchPitchCheck.CheckedChanged += e;
				VideoEffectCombo.SelectedIndexChanged += e;
				Tabs.SelectedIndexChanged += e;
				PreviewBeepEngineCombo.SelectedIndexChanged += e;
			}
			AudioMainKeyCombo.MouseWheel += AudioMainKeyCombo_MouseWheel;
			SourceConfigGroup.AllowDrop = MidiConfigGroup.AllowDrop = true;
			for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++)
				YtpEffectsCheckList.SetItemChecked(i, true);
			VideoEffectCombo.MaxDropDownItems = 50;
			MidiChannelCombo.DisplayMember = "DisplayValue"; // 本脚本基于 bug 运行。
			#if VEGAS_ENVIRONMENT
			ReadIni();
			#endif
			#endregion

			#region 预听音频计时器
			previewAudioTimer = new System.Windows.Forms.Timer { Interval = 1000 };
			previewAudioTimer.Tick += PreviewAudioTimer_Tick;
			#endregion

			#region 程序图标
			#if VEGAS_ENVIRONMENT
			try {
				//icon = Icon = Icon.ExtractAssociatedIcon(System.IO.Path.ChangeExtension(ScriptPath, "ico"));
				icon = Icon = new Icon(System.IO.Path.ChangeExtension(ScriptPath, "ico"), new Size(16, 16));
			} catch (Exception) { } // 如果路径不存在则不受影响
			#else
			icon = Icon = Properties.Resources.Otomad_Helper;
			#endif
			ConfigForm_Resize(null, null);
			#endregion

			#region 直接进入替换轨道素材页面
			if (openReplaceClipDirectly) {
				if (CloseAfterOpenHelperCheck.Checked) Opacity = 0;
				openReplaceClipDirectly = false;
				Load += OpenReplaceClipDirectlyEvent;
			}
			#endregion
		}

		private void OpenReplaceClipDirectlyEvent(object sender, EventArgs e) {
			ReadyToShowHelperDialog(ReplaceClipsBtn, null);
			Load -= OpenReplaceClipDirectlyEvent;
		}

		private void BindMenuLink(ToolStripMenuItem menu, string link) {
			menu.Click += (sender, e) => { OpenLink(link); };
		}

		public void ReadIni() {
			#if VEGAS_ENVIRONMENT
			try {
				#region 音频配置
				configIni.StartSection("Audio");
				AudioScratchCheck.Checked = configIni.Read("Scratch", false);
				AudioLoopCheck.Checked = configIni.Read("Loop", false);
				AudioNormalizeCheck.Checked = configIni.Read("Normalize", true);
				AudioFreezeLastFrameCheck.Checked = configIni.Read("FreezeLastFrame", false);
				AudioLegatoCheck.Checked = configIni.Read("Legato", false);
				AudioFadeInBox.SetValue(configIni.Read("FadeIn", 0), 0);
				AudioFadeOutBox.SetValue(configIni.Read("FadeOut", 0), 0);
				AudioFadeInCurveCombo.SetIndex(configIni.Read("FadeInCurve", 1), 1);
				AudioFadeOutCurveCombo.SetIndex(configIni.Read("FadeOutCurve", 2), 2);
				SetBasePitchCombo(configIni.Read("BasePitch", "C5"));
				AudioTuneMethodCombo.SetIndex(configIni.Read("TuneMethod", 2), 2);
				AudioStretchAttrCombo.SetIndex(configIni.Read("StretchAttr", 1), 1);
				AudioLockStretchPitchCheck.Checked = configIni.Read("LockStretchPitch", false);
				AudioReserveFormantCheck.Checked = configIni.Read("ReserveFormant", false);
				CreateEventGroupCheck.Checked = configIni.Read("CreateEventGroup", false);
				configIni.EndSection();
				#endregion

				#region 视频配置
				configIni.StartSection("Video");
				VideoEffectCombo.SetIndex(configIni.Read("VisualEffect", 1), 1);
				selectedVisualEffectBackupIndex = VideoEffectCombo.SelectedIndex;
				VideoScratchCheck.Checked = configIni.Read("Scratch", false);
				VideoLoopCheck.Checked = configIni.Read("Loop", true);
				VideoFreezeFirstFrameCheck.Checked = configIni.Read("FreezeFirstFrame", false);
				VideoFreezeLastFrameCheck.Checked = configIni.Read("FreezeLastFrame", false);
				VideoLegatoCheck.Checked = configIni.Read("Legato", true);
				VideoMultitrackForChordsCheck.Checked = configIni.Read("MultitrackForChords", false);
				VideoFadeInBox.SetValue(configIni.Read("FadeIn", 0), 0);
				VideoFadeOutBox.SetValue(configIni.Read("FadeOut", 0), 0);
				VideoGlowBox.SetValue(configIni.Read("Glow", 0), 0);
				VideoGlowBrightBox.SetValue(configIni.Read("GlowBrightness", 100), 100);
				VideoStartSizeBox.SetValue(configIni.Read("StartSize", 90), 90);
				VideoEndSizeBox.SetValue(configIni.Read("EndSize", 100), 100);
				VideoStartRotationBox.SetValue(configIni.Read("StartRotation", 0), 0);
				VideoEndRotationBox.SetValue(configIni.Read("EndRotation", 0), 0);
				VideoStartHorizontalTransBox.SetValue(configIni.Read("StartHorizontalTrans", 0), 0);
				VideoEndHorizontalTransBox.SetValue(configIni.Read("EndHorizontalTrans", 0), 0);
				VideoStartVerticalTransBox.SetValue(configIni.Read("StartVerticalTrans", 0), 0);
				VideoEndVerticalTransBox.SetValue(configIni.Read("EndVerticalTrans", 0), 0);
				VideoStartHueBox.SetValue(configIni.Read("StartHue", 0), 0);
				VideoEndHueBox.SetValue(configIni.Read("EndHue", 0), 0);
				VideoStartSaturationBox.SetValue(configIni.Read("StartSaturation", 100), 100);
				VideoEndSaturationBox.SetValue(configIni.Read("EndSaturation", 100), 100);
				VideoStartContrastBox.SetValue(configIni.Read("StartContrast", 0), 0);
				VideoEndContrastBox.SetValue(configIni.Read("EndContrast", 0), 0);
				VideoStartThresholdBox.SetValue(configIni.Read("StartThreshold", 50), 50);
				VideoEndThresholdBox.SetValue(configIni.Read("EndThreshold", 50), 50);
				VideoStartSizeCurveCombo.SetIndex(configIni.Read("StartSizeCurve", 1), 1);
				VideoFadeInCurveCombo.SetIndex(configIni.Read("FadeInCurve", 3), 3);
				VideoFadeOutCurveCombo.SetIndex(configIni.Read("FadeOutCurve", 3), 3);
				VideoGlowCurveCombo.SetIndex(configIni.Read("GlowCurve", 1), 1);
				VideoStartHueCurveCombo.SetIndex(configIni.Read("StartHueCurve", 1), 1);
				VideoStartSaturationCurveCombo.SetIndex(configIni.Read("StartSaturationCurve", 1), 1);
				VideoStartContrastCurveCombo.SetIndex(configIni.Read("StartContrastCurve", 1), 1);
				VideoStartThresholdCurveCombo.SetIndex(configIni.Read("StartThresholdCurve", 1), 1);
				configIni.EndSection();
				#endregion

				#region 素材配置
				configIni.StartSection("Source");
				GenerateAt = (GenerateAt)configIni.Read("GenerateAt", 0);
				GenerateAtCustomText.Text = configIni.Read("GenerateAtCustomValue", Timecode.FromMilliseconds(0).ToPositionString()); ;
				if (parent.SuggestSelectedSourceFrom == MediaSourceFrom.LAST_USER_PREFERENCE || parent.SuggestSelectedSourceFrom == MediaSourceFrom.NOTHING_SELECTED) {
					int lastMediaSourceFrom = configIni.Read("LastMediaSourceFrom", 2);
					if (lastMediaSourceFrom == 0 || lastMediaSourceFrom == 1) ChooseSourceCombo.SelectedIndex = lastMediaSourceFrom;
					else if (parent.SuggestSelectedSourceFrom != MediaSourceFrom.NOTHING_SELECTED) ChooseSourceCombo.SelectedIndex = (int)MediaSourceFrom.SELECTED_CLIP;
					else ChooseSourceCombo.SelectedIndex = (int)MediaSourceFrom.SELECTED_CLIP; //SELECTED_MEDIA;
				} else ChooseSourceCombo.SelectedIndex = (int)parent.SuggestSelectedSourceFrom;
				if (OpenMidiFile(configIni.Read("LastMidiFile", null), true))
					MidiChannelCombo.SetIndex(configIni.Read("LastMidiChannel", -1), -1);
				else RemoveLastMidiConfig();
				MidiDynamicMidiBpmFormCombo.SetIndex(configIni.Read("DynamicMidiBpmForm", 0), 0);
				MidiUseBpm_int = configIni.Read("MidiUseBpm", 0);
				configIni.EndSection();
				#endregion

				#region 五线谱配置
				configIni.StartSection("Staff");
				StaffVisualizerConfigCheck.Checked = configIni.Read("Enable", false);
				StaffGenerateCheck.Checked = configIni.Read("GenerateStaff", true);
				StaffRelativeValueCheck.Checked = configIni.Read("RelativeValue", true);
				StaffClefCombo.SetIndex(configIni.Read("Clef", 0), 0);
				StaffLineSpacingBox.SetValue(configIni.Read("Gap", 44), 44); // 45
				StaffSurfaceWidthBox.SetValue(configIni.Read("Width", 1500), 1500); // 1000
				StaffSurfacePositionBox.SetValue(configIni.Read("Position", 225), 225); // 0
				StaffLineThicknessBox.SetValue(configIni.Read("Thickness", 25), 25);
				StaffNotesShiftBox.SetValue(configIni.Read("Shift", 0), 0);
				GetStaffLineColor(configIni.Read("Color", "#FFFFFF"));
				StaffLegacyEngineCheck.Checked = configIni.Read("LegacyEngine", false);
				configIni.EndSection();
				#endregion

				#region YTP 配置
				configIni.StartSection("YTP");
				YtpMinLenBox.SetValue(configIni.Read("MinLength", 10), 10);
				YtpMaxLenBox.SetValue(configIni.Read("MaxLength", 5000), 5000);
				YtpClipsCountBox.SetValue(configIni.Read("ClipsCount", 30), 30);
				YtpEffectsCheckList.BatchSet(configIni.Read("Effects", ""));
				YtpEffectsCheckList_SelectedIndexChanged(null, null);
				configIni.EndSection();
				#endregion

				#region 音频预览配置
				configIni.StartSection("PreviewAudio");
				PreviewBeepWaveFormCombo.SetIndex(configIni.Read("BeepWaveForm", 0), 0);
				PreviewBeepDurationBox.SetValue(configIni.Read("BeepDuration", 500), 500);
				PreviewTuneAudioCheck.Checked = configIni.Read("IsTuneAudio", false);
				PreviewBeepEngineCombo.SelectedIndex = configIni.Read("PreviewBeepEngine", 2);
				MidiDynamicMidiBpmFormCombo.SelectedIndex = configIni.Read("DynamicMidiBpmForm", 0);
				configIni.EndSection();
				#endregion

				#region 个性化配置
				configIni.StartSection("Personalize");
				CloseAfterOpenHelperCheck.Checked = configIni.Read("CloseAfterOpenHelper", true);
				if (configIni.HasKey("FormSize")) {
					rememberFormSizeToolStripMenuItem.Checked = true;
					string formSize_string = configIni.Read("FormSize", "");
					Size size;
					if (ReadSize(formSize_string, out size)) Size = size;
				}
				configIni.EndSection();
				#endregion

				#region 工具配置
				openReplaceClipDirectly = configIni.Read("OpenToolDirectlyNext", false, "ReplaceClips");
				#endregion
			} catch (Exception e) {
				EntryPoint.ShowError(new Exceptions.ReadConfigFailException(), e);
				configIni.Delete(true);
				//AcceptConfig = false;
				//Close();
			}
			#endif
		}

		public void SaveIni() {
			#if VEGAS_ENVIRONMENT
			#region 音频配置
			configIni.StartSection("Audio");
			configIni.Write("Scratch", AudioScratchCheck.Checked);
			configIni.Write("Loop", AudioLoopCheck.Checked);
			configIni.Write("Normalize", AudioNormalizeCheck.Checked);
			configIni.Write("FreezeLastFrame", AudioFreezeLastFrameCheck.Checked);
			configIni.Write("Legato", AudioLegatoCheck.Checked);
			configIni.Write("FadeIn", AudioFadeInBox.Value);
			configIni.Write("FadeOut", AudioFadeOutBox.Value);
			configIni.Write("FadeInCurve", AudioFadeInCurveCombo.SelectedIndex);
			configIni.Write("FadeOutCurve", AudioFadeOutCurveCombo.SelectedIndex);
			configIni.Write("BasePitch", AudioMainKeyCombo.SelectedItem.ToString() + AudioMainOctaveCombo.SelectedItem.ToString());
			configIni.Write("TuneMethod", AudioTuneMethodCombo.SelectedIndex);
			configIni.Write("StretchAttr", AudioStretchAttrCombo.SelectedIndex);
			configIni.Write("LockStretchPitch", AudioLockStretchPitchCheck.Checked);
			configIni.Write("ReserveFormant", AudioReserveFormantCheck.Checked);
			configIni.Write("CreateEventGroup", CreateEventGroupCheck.Checked);
			configIni.EndSection();
			#endregion

			#region 视频配置
			configIni.StartSection("Video");
			if (!StaffVisualizerConfigCheck.Checked)
				configIni.Write("VisualEffect", VideoEffectCombo.SelectedIndex);
			else if (IsSelectedVisualEffectBackupIndexLegal())
				configIni.Write("VisualEffect", selectedVisualEffectBackupIndex);
			configIni.Write("Scratch", VideoScratchCheck.Checked);
			configIni.Write("Loop", VideoLoopCheck.Checked);
			configIni.Write("FreezeFirstFrame", VideoFreezeFirstFrameCheck.Checked);
			configIni.Write("FreezeLastFrame", VideoFreezeLastFrameCheck.Checked);
			configIni.Write("Legato", VideoLegatoCheck.Checked);
			configIni.Write("MultitrackForChords", VideoMultitrackForChordsCheck.Checked);
			configIni.Write("FadeIn", VideoFadeInBox.Value);
			configIni.Write("FadeOut", VideoFadeOutBox.Value);
			configIni.Write("Glow", VideoGlowBox.Value);
			configIni.Write("GlowBrightness", VideoGlowBrightBox.Value);
			configIni.Write("StartSize", VideoStartSizeBox.Value);
			configIni.Write("EndSize", VideoEndSizeBox.Value);
			configIni.Write("StartRotation", VideoStartRotationBox.Value);
			configIni.Write("EndRotation", VideoEndRotationBox.Value);
			configIni.Write("StartHorizontalTrans", VideoStartHorizontalTransBox.Value);
			configIni.Write("EndHorizontalTrans", VideoEndHorizontalTransBox.Value);
			configIni.Write("StartVerticalTrans", VideoStartVerticalTransBox.Value);
			configIni.Write("EndVerticalTrans", VideoEndVerticalTransBox.Value);
			configIni.Write("StartHue", VideoStartHueBox.Value);
			configIni.Write("EndHue", VideoEndHueBox.Value);
			configIni.Write("StartSaturation", VideoStartSaturationBox.Value);
			configIni.Write("EndSaturation", VideoEndSaturationBox.Value);
			configIni.Write("StartContrast", VideoStartContrastBox.Value);
			configIni.Write("EndContrast", VideoEndContrastBox.Value);
			configIni.Write("StartThreshold", VideoStartThresholdBox.Value);
			configIni.Write("EndThreshold", VideoEndThresholdBox.Value);
			configIni.Write("StartSizeCurve", VideoStartSizeCurveCombo.SelectedIndex);
			configIni.Write("FadeInCurve", VideoFadeInCurveCombo.SelectedIndex);
			configIni.Write("FadeOutCurve", VideoFadeOutCurveCombo.SelectedIndex);
			configIni.Write("GlowCurve", VideoGlowCurveCombo.SelectedIndex);
			configIni.Write("StartHueCurve", VideoStartHueCurveCombo.SelectedIndex);
			configIni.Write("StartSaturationCurve", VideoStartSaturationCurveCombo.SelectedIndex);
			configIni.Write("StartContrastCurve", VideoStartContrastCurveCombo.SelectedIndex);
			configIni.Write("StartThresholdCurve", VideoStartThresholdCurveCombo.SelectedIndex);
			configIni.EndSection();
			#endregion

			#region 素材配置
			configIni.StartSection("Source");
			configIni.Write("GenerateAt", (int)GenerateAt);
			configIni.Write("GenerateAtCustomValue", GenerateAtCustomText.Text);
			if (AcceptConfig) configIni.Write("LastMediaSourceFrom", ChooseSourceCombo.SelectedIndex);
			if (!ChooseMidiText.Text.Contains("<")) { // 是否不为 "<未选择 MIDI 文件>"
				configIni.Write("LastMidiFile", ChooseMidiText.Text);
				int lastMidiChannelIndex = MidiChannelCombo.SelectedIndex;
				if (MidiChannelCombo.SelectedItem is MidiChannels) {
					MidiChannels channels = MidiChannelCombo.SelectedItem as MidiChannels;
					if (channels.Count != 0)
						lastMidiChannelIndex = MidiChannelCombo.Items.IndexOf(channels[0]);
				}
				configIni.Write("LastMidiChannel", lastMidiChannelIndex);
			} else RemoveLastMidiConfig();
			configIni.Write("DynamicMidiBpmForm", MidiDynamicMidiBpmFormCombo.SelectedIndex);
			configIni.Write("MidiUseBpm", MidiUseBpm_int);
			configIni.EndSection();
			#endregion

			#region 五线谱配置
			configIni.StartSection("Staff");
			configIni.Write("Enable", StaffVisualizerConfigCheck.Checked);
			configIni.Write("GenerateStaff", StaffGenerateCheck.Checked);
			configIni.Write("RelativeValue", StaffRelativeValueCheck.Checked);
			configIni.Write("Clef", StaffClefCombo.SelectedIndex);
			configIni.Write("Gap", StaffLineSpacingBox.Value);
			configIni.Write("Width", StaffSurfaceWidthBox.Value);
			configIni.Write("Position", StaffSurfacePositionBox.Value);
			configIni.Write("Thickness", StaffLineThicknessBox.Value);
			configIni.Write("Color", StaffLineColorBtn.Text);
			configIni.Write("Shift", StaffNotesShiftBox.Value);
			configIni.Write("LegacyEngine", StaffLegacyEngineCheck.Checked);
			configIni.EndSection();
			#endregion

			#region YTP 配置
			configIni.StartSection("YTP");
			configIni.Write("MinLength", YtpMinLenBox.Value);
			configIni.Write("MaxLength", YtpMaxLenBox.Value);
			configIni.Write("ClipsCount", YtpClipsCountBox.Value);
			configIni.Write("Effects", YtpEffectsCheckList.BatchGet());
			configIni.EndSection();
			#endregion

			#region 音频预览配置
			configIni.StartSection("PreviewAudio");
			configIni.Write("BeepWaveForm", PreviewBeepWaveFormCombo.SelectedIndex);
			configIni.Write("BeepDuration", PreviewBeepDurationBox.Value);
			configIni.Write("IsTuneAudio", PreviewTuneAudioCheck.Checked);
			configIni.Write("PreviewBeepEngine", PreviewBeepEngineCombo.SelectedIndex);
			configIni.EndSection();
			#endregion

			#region 个性化配置
			configIni.StartSection("Personalize");
			configIni.Write("Language", Language);
			configIni.Write("CloseAfterOpenHelper", CloseAfterOpenHelperCheck.Checked);
			if (rememberFormSizeToolStripMenuItem.Checked) configIni.Write("FormSize", WriteSize(Size));
			// else configIni.DeleteKey("FormSize");
			configIni.EndSection();
			#endregion
			#endif
		}

		private void ChooseSourceBtn_Click(object sender, EventArgs e) {
			OpenSourceFile(parent.SelectVideoClip(), true);
		}

		private void OpenSourceFile(string clipName = "", bool inSilence = false) {
			if (!inSilence) clipName = parent.SelectVideoClip(clipName);
			if (string.IsNullOrWhiteSpace(clipName)) return;
			browseClipName = clipName;
			ChooseSourceCombo_SelectedIndexChanged(null, null);
			if (ChooseSourceCombo.Items.Count < 3)
				ChooseSourceCombo.Items.Add("");
			ChooseSourceCombo.Items[2] = browseClipName;
			ChooseSourceCombo.SelectedIndex = 2;
		}

		private void ChooseMidiBtn_Click(object sender, EventArgs e) {
			string midiFileName = parent.SelectMidiFile();
			if (!string.IsNullOrWhiteSpace(midiFileName)) OpenMidiFile(midiFileName);
		}

		private bool OpenMidiFile(string filePath, bool inSilence = false) {
			MIDI midi;
			bool ok = parent.OpenMidiFile(filePath, out midi, inSilence);
			if (ok) {
				#region 生成每个 MIDI 音轨的统计信息
				MidiChannelCombo.Items.Clear();
				foreach (MIDI.TrackInfo info in midi.TrackInfos)
					MidiChannelCombo.Items.Add(info);
				MidiChannelCombo.SelectedIndex = 0;
				ChooseMidiText.Text = filePath;
				UpdateMidiInfo(midi);
				SetCheckedEnabled(null, null);
				#endregion
			}
			return ok;
		}

		private void RemoveLastMidiConfig() {
			#if VEGAS_ENVIRONMENT
			configIni.DeleteKey("LastMidiFile");
			configIni.DeleteKey("LastMidiChannel");
			#endif
		}

		private string GetSystemLanguage() {
			string lang = CultureInfo.InstalledUICulture.Name;
			return Language = lang;
		}

		private string Language {
			get {
				if (chineseToolStripMenuItem.Checked) return "zhs";
				else if (tchineseToolStripMenuItem.Checked) return "zht";
				else if (japaneseToolStripMenuItem.Checked) return "ja";
				else if (russianToolStripMenuItem.Checked) return "ru";
				else return "en";
			}
			set {
				if (value == "") return;
				string lang = value.Trim().ToLower();
				if (lang.StartsWith("zh")) {
					if (lang == "zh-tw" || lang == "zh-hk" || lang == "zh-mo" || lang == "zht")
						tchineseToolStripMenuItem.Checked = true;
					else chineseToolStripMenuItem.Checked = true;
				}
				else if (lang.StartsWith("ja")) japaneseToolStripMenuItem.Checked = true;
				else if (lang.StartsWith("ru")) russianToolStripMenuItem.Checked = true;
				else englishToolStripMenuItem.Checked = true;
				Lang.SetLanguage(lang);
			}
		}
		private string LanguageBackup;

		private void LanguageStripMenuItem_Click(object sender, EventArgs e) {
			Language = Language; // 禁止套娃！
			DialogResult result = MessageBoxEx.Show(Lang.str.restart_to_effect_language, Lang.str.__name__, MessageBoxButtons.YesNoCancel, new string[] { Lang.str.yes_to_restart_to_effect_language, Lang.str.no_to_restart_to_effect_language, Lang.str.cancel }, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
			if (result == DialogResult.Cancel) {
				Language = LanguageBackup;
				return;
			}
			LanguageBackup = Language;
			if (result == DialogResult.No) Translate();
			else Close();
		}

		public void Translate() {
			Lang str = Lang.str;
			TrackLegatoMenu.Font = menu.Font = Font = new Font(str.ui_font, 9F);
			WarningInfoLabel.Font = new Font(str.info_label_font, 11F, FontStyle.Bold);
			Label[] infoLabels = { SheetConfigInfoLabel, YtpLbl, HelperLbl };
			foreach (Label label in infoLabels)
				label.Font = new Font(str.info_label_font, 9F);
			stretchLegatoTracksToolStripMenuItem.Font = new Font(str.ui_font, 9F, FontStyle.Bold);
			versionToolStripMenuItem.Text = Lang.str.version_number + Lang.str.colon + EntryPoint.VERSION;
			OkBtn.Text = str.complete;
			CancelBtn.Text = IsIrreversibleCancel ? str.close : str.cancel;
			AboutBtn.Text = str.about;
			UserHelpLink.Text = str.user_help + str.dialog_sign;
			Balloon.ToolTipTitle = str.balloon_title;
			Balloon.SetToolTip(StaffSurfacePositionBox, str.sheet_position_tooltip);
			Balloon.SetToolTip(StaffSurfaceWidthBox, str.sheet_width_tooltip);
			Balloon.SetToolTip(StaffLineSpacingBox, str.sheet_gap_tooltip);
			Balloon.SetToolTip(StaffRelativeValueCheck, str.sheet_relative_tooltip);
			Balloon.SetToolTip(StaffLineThicknessBox, str.sheet_line_thickness_tooltip);
			Balloon.SetToolTip(AudioTuneMethodCombo, str.tune_method_tooltip);
			StaffRelativeValueCheck.Text = str.sheet_relative;
			AudioTuneMethodCombo.Items[0] = str.no_tune;
			AudioTuneMethodCombo.Items[1] = str.pitch_shift_plugin;
			AudioTuneMethodCombo.Items[2] = str.elastique_method;
			AudioTuneMethodCombo.Items[3] = str.classic_method;
			AudioTuneMethodCombo.Items[4] = str.fool_tuning_method;
			PreviewBeepWaveFormCombo.Items[0] = str.sine_wave;
			PreviewBeepWaveFormCombo.Items[1] = str.triangle_wave;
			PreviewBeepWaveFormCombo.Items[2] = str.square_wave;
			PreviewBeepWaveFormCombo.Items[3] = str.sawtooth_wave;
			PreviewBasePitchBtn.Text = str.preview_base_pitch;
			Balloon.SetToolTip(YtpMaxLenBox, str.ytp_max_length_tooltip);
			Balloon.SetToolTip(YtpMinLenBox, str.ytp_min_length_tooltip);
			Balloon.SetToolTip(MidiStartSecondBox, str.midi_start_second_tooltip);
			Balloon.SetToolTip(MidiEndSecondBox, str.midi_end_second_tooltip);
			Balloon.SetToolTip(SourceStartTimeText, str.source_start_time_tooltip);
			Balloon.SetToolTip(SourceEndTimeText, str.source_end_time_tooltip);
			Balloon.SetToolTip(AudioLockStretchPitchCheck, str.audio_lock_stretch_pitch_tooltip);
			Balloon.SetToolTip(PreviewBeepDurationBox, str.preview_beep_duration_tooltip);
			Balloon.SetToolTip(PreviewTuneAudioCheck, str.preview_tune_audio_tooltip);
			AudioLockStretchPitchCheck.Text = str.audio_lock_stretch_pitch;
			fileMenuItem.Text = str.file;
			saveConfigToolStripMenuItem.Text = str.save_config;
			resetConfigToolStripMenuItem.Text = str.reset_config;
			exitDiscardingChangesToolStripMenuItem.Text = str.exit_discarding_changes;
			exitToolStripMenuItem.Text = str.exit;
			pitchShiftPresetMenuItem.Text = str.pitch_shift_preset;
			loadPresetsToolStripMenuItem.Text = str.load_presets + str.dialog_sign;
			unloadPresetsToolStripMenuItem.Text = str.unload_presets + str.dialog_sign;
			formSizeToolStripMenuItem.Text = str.form_size;
			rememberFormSizeToolStripMenuItem.Text = str.remember_form_size;
			restoreDefaultFormSizeToolStripMenuItem.Text = str.restore_default_form_size;
			helpToolStripMenuItem.Text = str.help;
			aboutToolStripMenuItem.Text = str.about;
			troubleShootingToolStripMenuItem.Text = str.trouble_shooting;
			githubToolStripMenuItem.Text = str.repository_link;
			checkUpdateToolStripMenuItem.Text = str.check_update;
			whyOkBtnIsDisabledToolStripMenuItem.Text = str.why_ok_btn_is_disabled;
			MidiConfigGroup.Text = str.midi_settings;
			MidiStartSecondLbl.Text = str.midi_start_time;
			MidiEndSecondLbl.Text = str.midi_end_time;
			MidiBpmLbl.Text = str.bpm_setting;
			MidiBeatLbl.Text = str.midi_beat;
			MidiBeatTxt.Text = str.none;
			MidiChannelLbl.Text = str.midi_channel_setting;
			ChooseMidiBtn.Text = ChooseSourceBtn.Text = str.browse;
			ChooseMidiText.Text = str.no_midi_selected;
			ChooseMidiLbl.Text = str.choose_midi_file;
			MidiDynamicMidiBpmCheck.Text = str.midi_dynamic_midi_bpm;
			MidiMidiBpmCheck.Text = str.midi_midi_bpm;
			MidiProjectBpmCheck.Text = str.midi_project_bpm;
			GenerateAtCustomRadio.Text = MidiCustomBpmCheck.Text = str.midi_custom_bpm;
			SourceConfigGroup.Text = str.source_settings;
			GenerateAtBeginRadio.Text = str.generate_at_begin;
			GenerateAtCursorRadio.Text = str.generate_at_cursor;
			GenerateAtLbl.Text = str.generate_position;
			ChooseSourceLbl.Text = str.choose_source_file;
			ChooseSourceCombo.Items[0] = str.selected_media;
			ChooseSourceCombo.Items[1] = str.selected_clip;
			SourceStartTimeLbl.Text = str.source_start_time;
			SourceEndTimeLbl.Text = str.source_end_time;
			SourceTab.Text = str.media;
			AudioTab.Text = str.audio;
			AudioParamsGroup.Text = VideoParamsGroup.Text = StaffParamsGroup.Text = YtpParamsGroup.Text = str.parameters;
			ComboBox[] combos = {
				AudioFadeInCurveCombo, AudioFadeOutCurveCombo, VideoFadeInCurveCombo, VideoFadeOutCurveCombo, VideoStartSizeCurveCombo, VideoGlowCurveCombo
			};
			foreach (ComboBox combo in combos) {
				combo.Items.Clear();
				combo.Items.AddRange(new string[] {
					str.linear, str.fast, str.slow, str.smooth, str.sharp
				});
			}
			MidiDynamicMidiBpmFormCombo.Items.Clear();
			MidiDynamicMidiBpmFormCombo.Items.AddRange(new string[] {
				str.hold, str.linear
			});
			AudioFadeInLbl.Text = VideoFadeInLbl.Text = str.fade_in;
			AudioFadeOutLbl.Text = VideoFadeOutLbl.Text = str.fade_out;
			AudioTuneGroup.Text = str.tune;
			AudioPreviewLbl.Text = str.preview_listen;
			AudioLockAttrLbl.Text = str.lock_attr;
			AudioPreviewAttrLbl.Text = str.preview_listen_attr;
			PreviewTuneAudioCheck.Text = str.preview_tune_audio;
			AudioReserveFormantCheck.Text = str.reserve_formant;
			AudioTuneMethodLbl.Text = str.tune_method;
			AudioBasePitchLbl.Text = str.base_pitch;
			PreviewAudioBtn.Text = str.preview_audio;
			AudioStretchAttrLbl.Text = str.stretch_attr;
			AudioConfigCheck.Text = str.aconfig;
			AudioScratchCheck.Text = str.audio_stretch;
			AudioLoopCheck.Text = str.audio_loop;
			AudioNormalizeCheck.Text = str.audio_normalize;
			AudioFreezeLastFrameCheck.Text = VideoFreezeLastFrameCheck.Text = str.freeze_last_frame;
			AudioLegatoCheck.Text = VideoLegatoCheck.Text = str.legato;
			CreateEventGroupCheck.Text = str.create_event_group;
			VideoTab.Text = str.video;
			VideoGlowBrightLbl.Text = str.video_glow_bright;
			VideoGlowLbl.Text = str.video_glow;
			VideoStartSizeLbl.Text = str.video_start_size;
			VideoEndSizeLbl.Text = str.video_end_size;
			VideoStartRotationLbl.Text = str.video_start_rotation;
			VideoEndRotationLbl.Text = str.video_end_rotation;
			VideoStartHorizontalTransLbl.Text = str.video_start_h_trans;
			VideoEndHorizontalTransLbl.Text = str.video_end_h_trans;
			VideoStartVerticalTransLbl.Text = str.video_start_v_trans;
			VideoEndVerticalTransLbl.Text = str.video_end_v_trans;
			VideoEffectsGroup.Text = YtpEffectsGroup.Text = str.effect;
			VideoEffectLbl.Text = str.visual_effect;
			VideoEffectInitialValueLbl.Text = str.initial_visual_effect;
			VideoEffectCombo.Items.Clear();
			VideoEffectCombo.Items.AddRange(new string[] {
				str.no_effects, str.h_flip, str.v_flip,
				str.ccw_flip, str.cw_flip,
				str.ccw_rotate, str.cw_rotate, str.turned,
				str.h_mirror, str.v_mirror,
				str.ccw_mirror, str.cw_mirror,
				str.negative, str.lumin_invert, str.hue_invert,
				string.Format(str.step_change_hue, 3),
				string.Format(str.step_change_hue, 4),
				string.Format(str.step_change_hue, 5),
				string.Format(str.step_change_hue, 6),
				string.Format(str.step_change_hue, 7),
				string.Format(str.step_change_hue, 8),
				str.chromatic_and_monochrome, str.pingpong, str.whirl,
				str.vertical_expansion,
				str.vertical_expansion_with_rebound,
				str.vertical_compression,
				str.vertical_compression_with_rebound,
				str.oblique_expansion_and_compression,
				str.puyo_puyo, str.pendulum,
				str.gaussian_blur, str.radial_blur,
			});
			VideoConfigCheck.Text = str.vconfig;
			VideoScratchCheck.Text = str.video_stretch;
			VideoLoopCheck.Text = str.video_loop;
			VideoFreezeFirstFrameCheck.Text = str.freeze_first_frame;
			SheetTab.Text = str.staff;
			StaffSurfaceWidthLbl.Text = str.sheet_width;
			StaffLineThicknessLbl.Text = str.sheet_thickness;
			StaffLineColorLbl.Text = str.sheet_color;
			StaffSurfacePositionLbl.Text = str.sheet_position;
			StaffClefLbl.Text = str.sheet_clef;
			StaffLineSpacingLbl.Text = str.sheet_gap;
			StaffClefCombo.Items[0] = str.sheet_g_clef;
			StaffClefCombo.Items[1] = str.sheet_f_clef;
			StaffNotesShiftLbl.Text = str.sheet_notes_shift;
			StaffVisualizerConfigCheck.Text = str.sheet_config;
			StaffGenerateCheck.Text = str.sheet_generate;
			SheetConfigInfoLabel.Text = str.sheet_config_info;
			YtpTab.Text = str.ytp;
			YtpClipsCountLbl.Text = str.ytp_clips_count;
			YtpMinLenLbl.Text = str.ytp_min_length;
			YtpMaxLenLbl.Text = str.ytp_max_length;
			YtpEnableAllEffectsCheck.Text = str.enable_all_effects;
			YtpEffectsCheckList.Items.Clear();
			YtpEffectsCheckList.Items.AddRange(new string[] {
				str.ytp_chorus, str.ytp_pitch_change, str.ytp_vibrato, str.ytp_reverse,
				str.ytp_delay, str.ytp_speed_change, str.ytp_hue_change, str.ytp_hue_rotate,
				str.ytp_monochrome, str.ytp_negative, str.ytp_high_freq_repeat, str.ytp_random_tone,
				str.ytp_enlarge, str.ytp_spherize, str.ytp_mirror, str.ytp_high_contrast,
				str.ytp_oversaturation, str.ytp_emphasize_thrice,
			});
			YtpLbl.Text = str.ytp_info;
			YtpSelectInfo.Text = str.select_source_count_info;
			HelperTab.Text = str.helper;
			QuickSelectIntervalBtn.Text = str.quick_select_interval;
			QuickSelectIntervalBtn.CommandLinkNote = str.select_interval_configform_info + '\n' + str.select_events_count_info;
			QuickNormalizeBtn.Text = str.quick_normalize;
			QuickNormalizeBtn.CommandLinkNote = str.quick_normalize_configform_info + '\n' + str.select_audioevents_count_info;
			ReplaceClipsBtn.Text = str.replace_clips;
			ReplaceClipsBtn.CommandLinkNote = str.replace_clips_configform_info + '\n' + str.select_events_count_info;
			ChangeTuneMethodBtn.Text = str.change_tune_method;
			ChangeTuneMethodBtn.CommandLinkNote = str.change_tune_method_configform_info + '\n' + str.select_audioevents_count_info;
			#if !VER_GEQ_16
				ChangeTuneMethodBtn.CommandLinkNote = str.change_tune_method_configform_info + '\n' + string.Format(str.unsupported_vegas_version_feature, 16);
			#endif
			AutoLayoutTracksGroup.Text = str.auto_layout_tracks;
			AutoLayoutTracksSelectInfo.Text = str.select_videotracks_count_info;
			AutoLayoutTracksLbl.Text = str.auto_layout_tracks_configform_info;
			GradientTracksBtn.Text = str.gradient_tracks + str.dialog_sign;
			AutoLayoutTracksGridBtn.Text = str.grid_layout + str.dialog_sign;
			AutoLayoutTracksBox3dBtn.Text = str.box_3d_layout + str.dialog_sign;
			ClearTrackMotionBtn.Text = str.clear_tracks_motion;
			ClearTrackEffectBtn.Text = str.clear_tracks_effect;
			TrackLegatoBtn.Text = str.track_legato;
			stackingTracksToolStripMenuItem.Text = str.track_legato_stacking;
			limitStretchLegatoTracksToolStripMenuItem.Text = str.track_legato_limit_stretch;
			stretchLegatoTracksToolStripMenuItem.Text = str.track_legato_stretch;
			lengthenLegatoToolStripMenuItem.Text = str.track_legato_lengthen;
			BatchSubtitleGenerationBtn.Text = str.batch_subtitle_generation;
			BatchSubtitleGenerationBtn.CommandLinkNote = str.batch_subtitle_generation_configform_info;
			FindClipsBtn.Text = str.find_clips;
			FindClipsBtn.CommandLinkNote = str.find_clips_configform_info;
			HelperLbl.Text = str.helper_info;
			CloseAfterOpenHelperCheck.Text = str.close_after_open_helper;
			Text = str.otomad_helper_config;
		}

		/// <summary>
		/// 获得本窗体的句柄。
		/// </summary>
		/// <returns>本窗体的句柄</returns>
		[DllImport("user32.dll", EntryPoint = "GetForegroundWindow", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetF();

		/// <summary>
		/// 设置此窗体为活动窗体。
		/// </summary>
		/// <param name="hWnd">窗体的句柄</param>
		/// <returns>操作是否成功</returns>
		[DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
		private static extern bool SetF(IntPtr hWnd);

		/// <summary>
		/// 请求强制聚焦本对话框。
		/// </summary>
		public void FocusOn(object sender, EventArgs e) {
			SetF(Handle);
		}

		/// <summary>
		/// 执行系统命令行命令。
		/// </summary>
		/// <param name="exeName">运行命令</param>
		/// <param name="operType">窗口显示方式</param>
		/// <returns>操作是否成功</returns>
		[DllImport("kernel32.dll")]
		public static extern int WinExec(string exeName, int operType);

		private void CancelBtn_Click(object sender, EventArgs e) {
			AcceptConfig = false;
			Close();
		}

		private void OkBtn_Click(object sender, EventArgs e) {
			if (ChooseSourceCombo.SelectedIndex == 2)
				if (!parent.OpenMedia(browseClipName)) return;
			parent.MidiConfigTrack = IsMultiMidiChannel ? midiChannels : new MidiChannels { MidiChannelCombo.SelectedItem as MIDI.TrackInfo };
			AcceptConfig = true;
			SaveIni();
			// 特殊处理部分
			if (StaffVisualizerConfigCheck.Checked && StaffVisualizerConfigCheck.Enabled) {
				VideoEffectCombo.SelectedIndex = 0; // TO-DO: 那边把这里锁死了改不了。
				VideoScratchCheck.Checked = false;
				VideoLegatoCheck.Checked = false;
			}
			if (!StaffGenerateCheck.Enabled) StaffGenerateCheck.Checked = false;
			if (Tabs.SelectedTab == YtpTab) IsGenerateYtp = true;
			Close();
		}
		private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e) {
			if (IsPreviewingAudio) PreviewAudioBtn_Click(false);
			if (!AcceptConfig && NeedSaveIni) SaveIni();
			// Environment.Exit(0); // 可以顺带一块ㄦ把 Vegas 干掉。
		}

		private void exitDiscardingChangesToolStripMenuItem_Click(object sender, EventArgs e) {
			NeedSaveIni = false;
			Close();
		}

		private void resetConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			DialogResult result = MessageBox.Show(Lang.str.sure_to_reset_config, Lang.str.reset_config_successful_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
			if (result == DialogResult.No) return;
			configIni.Delete(true);
			exitDiscardingChangesToolStripMenuItem_Click(null, null);
			MessageBox.Show(Lang.str.reset_config_successful, Lang.str.reset_config_successful_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public new void ShowDialog() {
			AcceptConfig = false;
			NeedSaveIni = true;
			RequestToShowHelperDialog = null;
			foreach (Control _control in toolsTableLayoutPanel.Controls)
				if (_control is CommandLinkButton)
					(_control as CommandLinkButton).UpdateCommandLink();
			base.ShowDialog();
		}

		private class StaffVisualizerInVisualEffect {
			public override string ToString() { return Lang.str.staff; }
		}
		private readonly StaffVisualizerInVisualEffect staffVisualizerInVisualEffect = new StaffVisualizerInVisualEffect();
		private int selectedVisualEffectBackupIndex = -1;
		private bool IsSelectedVisualEffectBackupIndexLegal() {
			return IsVisualEffectIndexLegal(selectedVisualEffectBackupIndex);
		}
		private bool IsVisualEffectIndexLegal(int visualEffectIndex) {
			return visualEffectIndex >= 0 && visualEffectIndex < PvVisualEffect.InitialValues.Length;
		}

		/// <summary>
		/// 控件的使能和失能控制。
		/// </summary>
		internal void SetCheckedEnabled(object sender, EventArgs e) {
			bool isVConfigOn = VideoConfigCheck.Checked;
			SetEnabled(VideoTab, isVConfigOn, new Control[] { VideoConfigCheck, VideoScratchCheck });
			StaffVisualizerConfigCheck.Enabled = isVConfigOn;
			if (!isVConfigOn) StaffVisualizerConfigCheck.Checked = false;
			if (VideoEffect == PvVisualEffectType.PINGPONG || VideoEffect == PvVisualEffectType.WHIRL) {
				VideoScratchCheck.Checked = true;
				VideoScratchCheck.Enabled = false;
			} else {
				if (!VideoScratchCheck.Enabled && VideoConfigCheck.Enabled) VideoScratchCheck.Checked = false;
				VideoScratchCheck.Enabled = isVConfigOn;
			}

			bool isAConfigOn = AudioConfigCheck.Checked;
			SetEnabled(AudioTab, isAConfigOn, new Control[] { AudioConfigCheck });
			AudioTuneMethod method = (AudioTuneMethod)AudioTuneMethodCombo.SelectedIndex;
			AudioLockStretchPitchCheck.Enabled = AudioMainKeyCombo.Enabled = AudioMainOctaveCombo.Enabled
				= PreviewBasePitchBtn.Enabled = AudioPreviewAttrLayoutPanel.Enabled
				= isAConfigOn && method != AudioTuneMethod.NO_TUNE && method != AudioTuneMethod.FOOL_TUNING;
			if (method == AudioTuneMethod.FOOL_TUNING) {
				AudioLockStretchPitchCheck.Enabled = AudioScratchCheck.Enabled = false;
				AudioLockStretchPitchCheck.Checked = AudioScratchCheck.Checked = true;
			}
			bool isPitchChangeMethod = method == AudioTuneMethod.ELASTIQUE || method == AudioTuneMethod.CLASSIC;
			AudioStretchAttrCombo.Enabled = isPitchChangeMethod;
			AudioReserveFormantCheck.Enabled = false;
			if (isPitchChangeMethod && AudioLockStretchPitchCheck.Checked)
				AudioScratchCheck.Checked = AudioScratchCheck.Enabled = false;
			#if VER_GEQ_16
				if (method == AudioTuneMethod.ELASTIQUE) {
					ElastiqueStretchAttributes attr = (ElastiqueStretchAttributes)AudioStretchAttrCombo.SelectedIndex;
					if (attr == ElastiqueStretchAttributes.Pro) AudioReserveFormantCheck.Enabled = true;
					else if (attr == ElastiqueStretchAttributes.Soloist_Monophonic) AudioReserveFormantCheck.Checked = true;
					else AudioReserveFormantCheck.Checked = false;
				} else AudioReserveFormantCheck.Checked = false;
			#endif
			if (AudioLockStretchPitchCheck.Checked) AudioStretchAttrCombo.Enabled = AudioReserveFormantCheck.Enabled = false;

			#if VEGAS_ENVIRONMENT
			{
				bool mediaConfigOn = isVConfigOn || isAConfigOn;
				bool midiConfigOn = parent.midi != null;
				if (Tabs.SelectedTab == HelperTab) OkBtn_Enabled = false;
				else if (Tabs.SelectedTab == YtpTab) OkBtn_Enabled = mediaConfigOn;
				else OkBtn_Enabled = mediaConfigOn && midiConfigOn;
			}
			#else
				OkBtn_Enabled = (isVConfigOn || isAConfigOn) && Tabs.SelectedTab != HelperTab;
			#endif

			bool isSheetConfigOn = StaffVisualizerConfigCheck.Checked;
			VideoEffectInitialValueCombo.Visible = VideoEffectInitialValueLbl.Visible = !isSheetConfigOn;
			if (isSheetConfigOn) {
				VideoEffectCombo.Enabled = VideoEffectInitialValueCombo.Enabled = VideoLegatoCheck.Enabled =
				VideoScratchCheck.Checked = VideoScratchCheck.Enabled = false;
				if (IsVisualEffectIndexLegal(VideoEffectCombo.SelectedIndex))
					selectedVisualEffectBackupIndex = VideoEffectCombo.SelectedIndex;
				if (!VideoEffectCombo.Items.Contains(staffVisualizerInVisualEffect))
					VideoEffectCombo.Items.Add(staffVisualizerInVisualEffect);
				VideoEffectCombo.SelectedItem = staffVisualizerInVisualEffect;
			} else {
				if (VideoEffectCombo.SelectedItem == staffVisualizerInVisualEffect && IsSelectedVisualEffectBackupIndexLegal())
					VideoEffectCombo.SelectedIndex = selectedVisualEffectBackupIndex;
				while (VideoEffectCombo.Items.Contains(staffVisualizerInVisualEffect))
					VideoEffectCombo.Items.Remove(staffVisualizerInVisualEffect);
			}
			SetEnabled(SheetTab, isSheetConfigOn, new Control[] { StaffVisualizerConfigCheck, SheetConfigInfoLabel });
			if (!StaffGenerateCheck.Checked)
				StaffLineThicknessBox.Enabled = StaffLineColorBtn.Enabled = false;

			PreviewBeepWaveFormCombo.Enabled = PreviewBeepEngineCombo.SelectedIndex == 2;
			
			#if !VER_GEQ_16
			{
				AudioStretchAttrCombo.Enabled = false;
				AudioStretchAttrCombo.SelectedIndex = -1;
				AudioReserveFormantCheck.Enabled = AudioReserveFormantCheck.Checked = false;
				PreviewTuneAudioCheck.Enabled = PreviewTuneAudioCheck.Checked = false;
				int INVALID_METHOD_LESS_THAN_16 = 2;
				if (AudioTuneMethodCombo.SelectedIndex >= INVALID_METHOD_LESS_THAN_16)
					AudioTuneMethodCombo.SelectedIndex = INVALID_METHOD_LESS_THAN_16 - 1;
				while (AudioTuneMethodCombo.Items.Count > INVALID_METHOD_LESS_THAN_16)
					AudioTuneMethodCombo.Items.RemoveAt(INVALID_METHOD_LESS_THAN_16);
			}
			#endif

			ConfigForm_Resize(null, null);
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
				whyOkBtnIsDisabledToolStripMenuItem.Visible = !value;
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
		/// <param name="midi">MIDI 对象。</param>
		public void UpdateMidiInfo(MIDI midi) {
			MidiStartSecondBox.Enabled
				= MidiEndSecondBox.Enabled
				= MidiMidiBpmCheck.Enabled
				= MidiProjectBpmCheck.Enabled
				= MidiCustomBpmCheck.Enabled
				= MidiChannelCombo.Enabled
				= MidiChannelAdvancedBtn.Enabled
				= MidiBeatTxt.Enabled
				= true;
			Lang str = Lang.str;
			string bpm_str = ProcessBpmDouble(midi.Bpm);
			IsDynamicMidiBpm = midi.IsDynamicBpm;
			MidiMidiBpmCheck.Text = str.midi_midi_bpm + str.colon + bpm_str;
			MidiDynamicMidiBpmCheck.Text = str.midi_dynamic_midi_bpm + str.colon + string.Format(str.dynamic_midi_bpm_info, bpm_str);
			MidiBeatTxt.Text = midi.IsDynamicBeat ? string.Format(str.dynamic_midi_beat_info, midi.TimeSignature) : midi.TimeSignature;
		}

		public static class Links {
			public const string
				GITHUB_LATEST = "https://github.com/otomad/VegasScripts/releases/latest",
				REPOSITORY = "https://github.com/otomad/VegasScripts",
				REPOSITORY_ORIG = "https://github.com/Chaosinism/vegas_scripts",
				ROADMAP = "https://github.com/otomad/VegasScripts/blob/winform/ROADMAP.md",
				UPDATE_LOGS = "https://github.com/otomad/VegasScripts/releases",
				TROUBLE_SHOOTING = "https://www.bilibili.com/read/cv495309",
				DOCUMENTATION_V0_1 = "https://www.bilibili.com/read/cv392013",
				STAFF_VISUALIZER_DOCUMENTATION_V0_1 = "https://www.bilibili.com/read/cv1027442",
				TUTORIAL_VIDEO_V0_1 = "https://www.bilibili.com/video/av22226321",
				RELEASE_NOTES_V4_9_25_0 = "http://www.bilibili.com/read/cv13335178",
				RELEASE_NOTES_V4_10_17_0 = "https://www.bilibili.com/read/cv13614419",
				DOCUMENTATION_ENGLISH = "https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng/edit",
				TUTORIAL_VIDEO_ENGLISH = "https://www.youtube.com/watch?v=8vSpzgL_86A";
		}

		private void UserHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			OpenLink(Links.DOCUMENTATION_ENGLISH);
		}

		public void OpenLink(string link) {
			Process.Start("explorer.exe", link);
		}

		private void AboutBtn_Click(object sender, EventArgs e) {
			Lang str = Lang.str;
			string[,] pairs = {
				{ str.version_number, EntryPoint.VERSION.ToString() },
				{ str.revision_date, EntryPoint.REVISION_DATE.ToString("D") },
				{ str.vegas_version, vegas.Version },
				{ str.script_supported_vegas_version, EntryPoint.GetScriptSupportedVersionRange() },
				{ "", "" },
				{ str.script_author, "淅琳雨" },
				{ str.script_original_author , "Chaosinism" },
				{ "", "" },
				{ str.repository_link , Links.REPOSITORY },
				{ str.latest_version_link , Links.GITHUB_LATEST },
			};
			StringBuilder text = new StringBuilder();
			for (int i = 0; i < pairs.GetLength(0); i++) {
				string key = pairs[i, 0], value = pairs[i, 1];
				if (string.IsNullOrWhiteSpace(value)) text.AppendLine();
				else text.AppendLine(key + str.colon + value);
			}
			MessageBox.Show(text.ToString(), str.about_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		public PvVisualEffectType VideoEffect { get { return VideoEffectCombo.SelectedIndex < PvVisualEffect.InitialValues.Length ? (PvVisualEffectType)VideoEffectCombo.SelectedIndex : PvVisualEffectType.NONE; } }

		public void ChooseSourceCombo_InitSourceNames() {
			EntryPoint.AudioVideoEnabledTable table = parent.audioVideoEnabledTable;
			if (!string.IsNullOrWhiteSpace(table.SelectMediaName)) ChooseSourceCombo.Items[0] += Lang.str.colon + table.SelectMediaName;
			if (!string.IsNullOrWhiteSpace(table.SelectClipName)) ChooseSourceCombo.Items[1] += Lang.str.colon + table.SelectClipName;
		}

		private bool isValidSource = true;
		public void ChooseSourceCombo_SelectedIndexChanged(object sender, EventArgs e) {
			#if VEGAS_ENVIRONMENT
			MediaSourceFrom SourceConfigFrom = (MediaSourceFrom)ChooseSourceCombo.SelectedIndex;
			var table = parent.audioVideoEnabledTable;
			var group = SourceConfigFrom == MediaSourceFrom.SELECTED_MEDIA ? table.FromSelectedMedia :
						SourceConfigFrom == MediaSourceFrom.SELECTED_CLIP ? table.FromSelectedClip : table.FromBrowseFile;
			AudioConfigCheck.Enabled = AudioConfigCheck.Checked = group.AudioEnabled;
			VideoConfigCheck.Enabled = VideoConfigCheck.Checked = group.VideoEnabled;
			WarningInfoLabel.Text =
				SourceConfigFrom == MediaSourceFrom.SELECTED_MEDIA && parent.audioVideoEnabledTable.SelectNoMedia ? Lang.str.no_selected_media_warning :
				SourceConfigFrom == MediaSourceFrom.SELECTED_CLIP && parent.audioVideoEnabledTable.SelectNoEvents ? Lang.str.no_selected_clip_warning : "";
			isValidSource = string.IsNullOrWhiteSpace(WarningInfoLabel.Text);
			int selectSourceCountForYtp =
				SourceConfigFrom == MediaSourceFrom.SELECTED_MEDIA ? parent.GetSelectedMedia().Length :
				SourceConfigFrom == MediaSourceFrom.SELECTED_CLIP ? parent.GetSelectedEvents().Length : 1;
			YtpSelectInfo.Text = string.Format(Lang.str.select_source_count_info, selectSourceCountForYtp);
			#endif
		}

		public void VideoEffectCombo_SelectedIndexChanged(object sender, EventArgs e) {
			if (VideoEffectCombo.SelectedItem == staffVisualizerInVisualEffect) return;
			#if VEGAS_ENVIRONMENT
			int VideoEffectIndex = VideoEffectCombo.SelectedIndex;
			string[] VideoEffectInitialValues = PvVisualEffect.InitialValues[VideoEffectIndex];
			VideoEffectInitialValueCombo.Items.Clear();
			VideoEffectInitialValueCombo.Items.AddRange(VideoEffectInitialValues);
			VideoEffectInitialValueCombo.SelectedIndex = 0;
			#endif
		}

		public static void SetTrackBarValue(TrackBar track, NumericUpDown numeric, int value, int def = -1) {
			int min = track.Minimum, max = track.Maximum;
			if (value < min || value > max) {
				if (def < min || def > max) return;
				numeric.Value = track.Value = def;
			} else numeric.Value = track.Value = value;
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
			AudioMainOctaveCombo.SetIndex(oct, 5);
		}

		private void HelperSelectedCount() {
			Lang str = Lang.str;
			
			#region 选中剪辑
			int selectedClipsCount = parent.GetSelectedEvents().Length;
			string selectInfo = string.Format(str.select_events_count_info, selectedClipsCount);
			QuickSelectIntervalBtn.CommandLinkNote = str.select_interval_configform_info + '\n' + selectInfo;
			ReplaceClipsBtn.CommandLinkNote = str.replace_clips_configform_info + '\n' + selectInfo;
			if (selectedClipsCount == 0)
				ReplaceClipsBtn.Enabled = QuickSelectIntervalBtn.Enabled = false;
			#endregion
			
			#region 选中音频剪辑
			int selectedAudioClipsCount = parent.GetSelectedAudioEvents().Length;
			string selectAudioInfo = string.Format(str.select_audioevents_count_info, selectedAudioClipsCount);
			ChangeTuneMethodBtn.CommandLinkNote = str.change_tune_method_configform_info + '\n' + selectAudioInfo;
			QuickNormalizeBtn.CommandLinkNote = str.quick_normalize_configform_info + '\n' + selectAudioInfo;
			if (selectedAudioClipsCount == 0)
				ChangeTuneMethodBtn.Enabled = QuickNormalizeBtn.Enabled = false;
			
			#if !VER_GEQ_16
				ChangeTuneMethodBtn.Enabled = false;
				ChangeTuneMethodBtn.CommandLinkNote = str.change_tune_method_configform_info + '\n' +
					string.Format(str.unsupported_vegas_version_feature, 16);
			#endif
			#endregion
			
			#region 选中轨道
			int selectedTracksCount = parent.GetSelectedTracks().Length;
			if (selectedTracksCount == 0)
				TrackLegatoBtn.Enabled = ClearTrackEffectBtn.Enabled = false;
			#endregion
			
			#region 选中视频轨道
			int selectedVideoTracksCount = parent.GetSelectedVideoTracks().Length;
			AutoLayoutTracksSelectInfo.Text = string.Format(str.select_videotracks_count_info, selectedVideoTracksCount);
			if (selectedVideoTracksCount == 0)
				AutoLayoutTracksButtons.Enabled = ClearTrackMotionBtn.Enabled = false;
			#endregion
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

		private void GetStaffLineColor(string _color) {
			MatchCollection matches = Regex.Matches(_color.ToUpper(), @"#[0-9A-F]{6}");
			string color = matches.Count != 0 ? matches[0].ToString() : "#000000";
			int r = Convert.ToInt16(color.Substring(1, 2), 16),
				g = Convert.ToInt16(color.Substring(3, 2), 16),
				b = Convert.ToInt16(color.Substring(5, 2), 16);
			Color c = Color.FromArgb(r, g, b);
			StaffLineColorDialog.Color = c;
			Update_StaffLineColorBtn_Color();
		}

		[Obsolete]
		private void ReadyToShowHelperDialog_Legacy() {
			System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 30 };
			const double bite = 0.05;
			timer.Tick += (sender, e) => {
				if (Opacity >= bite) Opacity -= bite;
				else {
					timer.Stop();
					timer.Dispose();
					Opacity = 0;
					Close();
				}
			};
			timer.Start();
		}

		private void ReadyToShowHelperDialog(object sender, EventArgs e) {
			Button btn = sender as Button;
			if (btn == null) return;
			Dictionary<Button, Type> map = new Dictionary<Button, Type> {
				{ QuickSelectIntervalBtn, typeof(SelectIntervalForm) },
				{ ReplaceClipsBtn, typeof(ReplaceClipsForm) },
				{ AutoLayoutTracksGridBtn, typeof(AutoLayoutTracksGridForm) },
				{ AutoLayoutTracksBox3dBtn, typeof(AutoLayoutTracksBox3dForm) },
				{ GradientTracksBtn, typeof(GradientTracksForm) },
				{ ChangeTuneMethodBtn, typeof(ChangeTuneMethodForm) },
				{ BatchSubtitleGenerationBtn, typeof(BatchSubtitleGenerationForm) },
				{ FindClipsBtn, typeof(FindClipsForm) },
			};
			map.TryGetValue(btn, out RequestToShowHelperDialog);
			Close();
		}

		public Type RequestToShowHelperDialog = null;

		private void AudioMainKeyCombo_MouseWheel(object sender, MouseEventArgs e) {
			ComboBox combo = AudioMainKeyCombo, combo2 = AudioMainOctaveCombo;
			HandledMouseEventArgs hme = e as HandledMouseEventArgs;
			if (hme != null) hme.Handled = true;
			int count = combo.Items.Count;
			if (count == 0) return;
			int selected = combo.SelectedIndex;
			if (selected == -1) {
				combo.SelectedIndex = 0;
				return;
			}
			if (e.Delta > 0) { // 上滑
				selected--;
				if (selected < 0) {
					if (combo2.SelectedIndex == 0) return;
					else combo2.SelectedIndex--;
					selected = count - 1;
				}
			}
			else if (e.Delta < 0) { // 下滑
				selected++;
				if (selected >= count) {
					if (combo2.SelectedIndex == combo2.Items.Count - 1) return;
					else combo2.SelectedIndex++;
					selected = 0;
				}
			}
			combo.SelectedIndex = (selected + count) % count;
		}

		private bool IsDoing_AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged = false;
		private void AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged(object sender, EventArgs e) {
			if (IsDoing_AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged) return;
			IsDoing_AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged = true;
			CheckBox check = sender as CheckBox;
			CheckBox[] checks = { AudioLegatoCheck, AudioFreezeLastFrameCheck };
			foreach (CheckBox checkBox in checks) {
				if (checkBox == check) continue;
				checkBox.Checked = false;
			}
			IsDoing_AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged = false;
		}

		public void BaseTrackMotionKeyframe_Reset(BaseTrackMotionKeyframe frame, bool isParentTrack = false) {
			Timecode begin = Timecode.FromMilliseconds(0);
			frame.Position = begin;
			frame.Selected = false;
			frame.PositionX = 0;
			frame.PositionY = 0;
			frame.Width = isParentTrack ? vegas.Project.Video.Height : vegas.Project.Video.Width;
			frame.Height = vegas.Project.Video.Height;
			frame.RotationZ = 0;
			frame.RotationOffsetX = 0;
			frame.RotationOffsetY = 0;
			frame.OrientationZ = 0;
		}

		public void TrackMotionKeyframe_Reset(TrackMotionKeyframe frame, bool isParentTrack = false) {
			BaseTrackMotionKeyframe_Reset(frame, isParentTrack);
			const double DEPTH_RATIO = 20;
			try {
				frame.PositionZ = 0;
				frame.Depth = isParentTrack ? vegas.Project.Video.Height : vegas.Project.Video.Height / DEPTH_RATIO;
				frame.RotationX = 0;
				frame.RotationY = 0;
				frame.RotationOffsetZ = 0;
				frame.OrientationX = 0;
				frame.OrientationY = 0;
			} catch (Exception) { }
		}

		private void ClearTrackMotionBtn_Click(object sender, EventArgs e) {
			foreach (VideoTrack track in parent.GetSelectedVideoTracks()) {
				List<TrackMotion> motions = new List<TrackMotion> { track.TrackMotion };
				if (track.IsCompositingParent) motions.Add(track.ParentTrackMotion);
				for (int i = 0; i < motions.Count; i++) {
					TrackMotion motion = motions[i];
					motion.MotionKeyframes.Clear();
					motion.ShadowKeyframes.Clear();
					motion.GlowKeyframes.Clear();
					try {
						motion.ShadowEnabled = false;
						motion.GlowEnabled = false;
					} catch (Exception) { }
					TrackMotionKeyframe_Reset(motion.MotionKeyframes[0], i == 1);
					BaseTrackMotionKeyframe_Reset(motion.ShadowKeyframes[0]);
					BaseTrackMotionKeyframe_Reset(motion.GlowKeyframes[0]);
				}
			}
			ClearTrackMotionBtn.Enabled = false;
			IsIrreversibleCancel = true;
			vegas.UpdateUI();
		}

		private void ClearTrackEffectBtn_Click(object sender, EventArgs e) {
			foreach (Track track in parent.GetSelectedTracks())
				track.Effects.Clear();
			ClearTrackEffectBtn.Enabled = false;
			IsIrreversibleCancel = true;
			vegas.UpdateUI();
		}

		public double Semitone2Freq(int semitone) {
			return 440.0 * Math.Pow(2, semitone / 12.0);
		}

		private void PreviewBasePitchBtn_MouseDown(object sender, EventArgs e) {
			int semitone = (!PreviewTuneAudioCheck.Checked ? BasePitch : REFERENCE_PITCH) - EntryPoint.PitchMap("A", "4");
			int freq = (int)Semitone2Freq(semitone);
			if (freq < 37 || freq > 32767) { // 频率超出 37~32767 Hz 这个范围会崩溃，反正这个频率你也不一定听得到。
				Asterisk.Play();
				return;
			}
			if (PreviewBeepEngineCombo.SelectedIndex == 2) // 使用 NAudio 库蜂鸣。
				NAudioBeep(freq, PreviewBasePitchBeepDuration);
			else if (PreviewBeepEngineCombo.SelectedIndex == 1) // 使用 PowerShell 进行蜂鸣。
				WinExec(string.Format("powershell [Console]::Beep({0:D},{1:D})", freq, PreviewBasePitchBeepDuration), 0);
			else // 使用 C# 自带 API 蜂鸣。
				new Thread(() => Console.Beep(freq, PreviewBasePitchBeepDuration)).Start();
		}
		public System.Media.SystemSound Asterisk = System.Media.SystemSounds.Asterisk;
		private int PreviewBasePitchBeepDuration { get { return (int)PreviewBeepDurationBox.Value; } }
		public bool IsPreviewingAudio = false;
		private AudioTrack previewAudioTrack;
		private readonly System.Windows.Forms.Timer previewAudioTimer;
		private DateTime startPreviewAudioTime;
		private Timecode projectEndTimecode;
		private Timecode stopPreviewAudioTimecode;
		private void PreviewAudioBtn_Click(object sender, EventArgs e) { PreviewAudioBtn_Click(null); }
		private readonly bool DEBUG_PREVIEW_AUDIO_TRACK_SHOW = true; // 关闭后有界面错位并且丢失音频前几秒的问题。
		public static readonly int REFERENCE_PITCH = EntryPoint.PitchMap("C", "6"); // 指定中央标准音调。
		public int BasePitch {
			get {
				return EntryPoint.PitchMap(
					AudioMainKeyCombo.SelectedItem.ToString() ?? "C",
					AudioMainOctaveCombo.SelectedItem.ToString() ?? "5"
				);
			}
		}
		private void PreviewAudioBtn_Click(bool? requestPlaying) {
			IsPreviewingAudio = !IsPreviewingAudio;
			if (requestPlaying != null) IsPreviewingAudio = (bool)requestPlaying;
			if (IsPreviewingAudio) {
				projectEndTimecode = vegas.Project.Length + Timecode.FromMilliseconds(1000); // 避免闪现视频尾帧。
				vegas.Transport.LoopMode = false;
				previewAudioTrack = new AudioTrack(vegas.Project, 0, Lang.str.preview_audio_track_name);
				vegas.Project.Tracks.Add(previewAudioTrack);
				AudioEvent previewAudio;
				bool putOk = parent.PutPreviewAudioEvent(previewAudioTrack, projectEndTimecode, out previewAudio);
				if (!putOk) goto NotOk;
				if (PreviewTuneAudioCheck.Checked) ApplyTuningToAudioEvent(previewAudio);
				vegas.Transport.CursorPosition = projectEndTimecode;
				stopPreviewAudioTimecode = previewAudio.End;
				vegas.Transport.Play();
				startPreviewAudioTime = DateTime.Now;
				previewAudioTimer.Start();
			} else {
				vegas.Transport.Stop();
				vegas.Transport.CursorPosition = originalCursorPosition;
				if (previewAudioTrack == null) goto NotOk;
				vegas.Project.Tracks.Remove(previewAudioTrack);
				previewAudioTrack = null;
				previewAudioTimer.Stop();
			}
			PreviewAudioBtn.Text = IsPreviewingAudio ? Lang.str.stop_preview_audio : Lang.str.preview_audio;
			if (DEBUG_PREVIEW_AUDIO_TRACK_SHOW) vegas.UpdateUI();
			return;

		NotOk:
			IsPreviewingAudio = false;
			Asterisk.Play();
			return;
		}

		private void PreviewAudioTimer_Tick(object sender, EventArgs e) {
			if (!IsPreviewingAudio) previewAudioTimer.Stop();
			TimeSpan playTimeSpan = DateTime.Now - startPreviewAudioTime;
			Timecode playTimecode = projectEndTimecode + Timecode.FromMilliseconds(playTimeSpan.TotalMilliseconds);
			if (playTimecode > stopPreviewAudioTimecode && IsPreviewingAudio) PreviewAudioBtn_Click(false);
		}

		private void ApplyTuningToAudioEvent(AudioEvent @event) {
			#if VER_GEQ_16 // Vegas 版本号 15 及以下无法调整。
				AudioTuneMethod method = (AudioTuneMethod)AudioTuneMethodCombo.SelectedIndex;
				if (method == AudioTuneMethod.NO_TUNE) return;
				@event.Method = method == AudioTuneMethod.CLASSIC ? TimeStretchPitchShift.Classic : TimeStretchPitchShift.Elastique;
				if (method == AudioTuneMethod.ELASTIQUE) {
					ElastiqueStretchAttributes elastiqueAttr = @event.ElastiqueAttribute = (ElastiqueStretchAttributes)AudioStretchAttrCombo.SelectedIndex;
					if (elastiqueAttr == ElastiqueStretchAttributes.Pro) @event.FormantLock = AudioReserveFormantCheck.Checked;
				} else if (method == AudioTuneMethod.CLASSIC)
					@event.ClassicAttribute = (ClassicStretchAttributes)AudioStretchAttrCombo.SelectedIndex;
				try {
					@event.PitchLock = @event.PitchLock;
				} catch (Exception e) {
					EntryPoint.ShowError(new Exceptions.NoTimeStretchPitchShiftException(), e); return;
				}
				int pitchDelta = REFERENCE_PITCH - BasePitch;
				if (!AudioLockStretchPitchCheck.Checked) @event.PitchSemis += pitchDelta;
				else {
					double origPitch = @event.PitchSemis;
					@event.PitchLock = true;
					double rate = EntryPoint.Pitch2Stretch(origPitch + pitchDelta);
					@event.AdjustPlaybackRate(rate, true);
					@event.Length = @event.Length.Multiply(1 / rate);
				}
			#endif
		}

		private void YtpLenBox_ValueChanged(object sender, EventArgs e) {
			NumericUpDown min = YtpMinLenBox, max = YtpMaxLenBox, cur = sender as NumericUpDown;
			if (min.Value > max.Value) {
				if (cur == min) min.Value = max.Value;
				else max.Value = min.Value;
			}
		}

		private void YtpEnableAllEffectsCheck_CheckedChanged(object sender, EventArgs e) {
			CheckBox check = sender as CheckBox;
			if (check.CheckState != CheckState.Indeterminate)
				for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++)
					YtpEffectsCheckList.SetItemChecked(i, check.Checked);
			YtpEffectsCheckList_SelectedIndexChanged(null, null);
		}

		public YtpEffectType[] selectedYtpEffects;
		private void YtpEffectsCheckList_SelectedIndexChanged(object sender, EventArgs e) {
			CheckState? state = null;
			List<YtpEffectType> selected = new List<YtpEffectType>();
			for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++) {
				bool isChecked = YtpEffectsCheckList.GetItemChecked(i);
				if (isChecked) selected.Add((YtpEffectType)i);
				if (isChecked && (state == null || state == CheckState.Checked)) state = CheckState.Checked;
				else if (!isChecked && (state == null || state == CheckState.Unchecked)) state = CheckState.Unchecked;
				else state = CheckState.Indeterminate;
			}
			selectedYtpEffects = selected.ToArray();
			if (state == null) return;
			YtpEnableAllEffectsCheck.CheckState = (CheckState)state;
			YtpEffectsGroup.Text = Lang.str.effect + " (" + selected.Count + ")";
		}

		private void ConfigForm_Resize(object sender, EventArgs e) {
			const int MARGIN = 6;
			SheetConfigInfoLabel.MaximumSize = YtpLbl.MaximumSize = new Size(Tabs.SelectedTab.Size.Width - MARGIN, 0);
			WarningInfoLabel.MaximumSize = new Size(Tabs.SelectedTab.Size.Width - MARGIN, 0);

			// 处理 FlowLayoutPanel 的换行问题。
			AudioTuneTablePanel.RowStyles[AudioTuneTablePanel.GetRow(AudioPreviewAttrLayoutPanel)] = new RowStyle();
			AudioTuneTablePanel.RowStyles[AudioTuneTablePanel.GetRow(AudioPreviewAttrLayoutPanel)] = new RowStyle(SizeType.Absolute, AudioPreviewAttrLayoutPanel.Height);
			MidiConfigTablePanel.RowStyles[MidiConfigTablePanel.GetRow(MidiBpmFlowPanel)] = new RowStyle();
			MidiConfigTablePanel.RowStyles[MidiConfigTablePanel.GetRow(MidiBpmFlowPanel)] = new RowStyle(SizeType.Absolute, MidiBpmFlowPanel.Height);
		}

		private AudioTuneMethod previousAudioTuneMethod = new AudioTuneMethod();
		private void AudioTuneMethodCombo_SelectedIndexChanged(object sender, EventArgs e) {
			ComboBox attrs = AudioStretchAttrCombo;
			AudioTuneMethod method = (AudioTuneMethod)AudioTuneMethodCombo.SelectedIndex;
			if (method == previousAudioTuneMethod) return;
			else previousAudioTuneMethod = method;
			attrs.Items.Clear();
			if (method == AudioTuneMethod.ELASTIQUE) {
				attrs.Items.AddRange(ChangeTuneMethodForm.ElastiqueAttrArray);
				attrs.SelectedIndex = 1;
			} else if (method == AudioTuneMethod.CLASSIC) {
				attrs.Items.AddRange(ChangeTuneMethodForm.ClassicAttrArray);
				attrs.SelectedIndex = 2;
			}
		}

		private void WhyOkBtnIsDisabledToolStripMenuItem_Click(object sender, EventArgs e) {
			Lang str = Lang.str;
			List<string> reasons = new List<string>();
			if (Tabs.SelectedTab == HelperTab) reasons.Add(str.why_ok_btn_is_disabled_in_helper_tab);
			if (!AudioConfigCheck.Checked && !VideoConfigCheck.Checked)
				reasons.Add(isValidSource ? str.why_ok_btn_is_disabled_no_audio_and_video_enabled : str.why_ok_btn_is_disabled_no_media_take);
			if (parent.midi == null && Tabs.SelectedTab != YtpTab) reasons.Add(str.why_ok_btn_is_disabled_no_midi_select);
			string resultInfo = str.why_ok_btn_is_disabled_unknown_problem;
			if (reasons.Count != 0) {
				StringBuilder reason = new StringBuilder(str.why_ok_btn_is_disabled_info + "\n\n");
				for (int i = 0; i < reasons.Count; i++)
					reason.AppendLine(i + 1 + ". " + reasons[i]);
				resultInfo = reason.ToString();
			}
			MessageBox.Show(resultInfo, str.why_ok_btn_is_disabled, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void NAudioBeep(int freq, int duration) {
			PreviewBasePitchBtn.Enabled = false;
			SignalGeneratorType type = new SignalGeneratorType[] {
				SignalGeneratorType.Sin, SignalGeneratorType.Triangle, SignalGeneratorType.Square, SignalGeneratorType.SawTooth
			}[PreviewBeepWaveFormCombo.SelectedIndex];
			ISampleProvider beep = new SignalGenerator() {
				Gain = 0.2, // 保护听力用
				Frequency = freq,
				Type = type
			}.Take(TimeSpan.FromMilliseconds(duration));
			WaveOutEvent waveOut = new WaveOutEvent();
			waveOut.Init(beep);
			waveOut.Play();
			waveOut.PlaybackStopped += (sender, e) => PreviewBasePitchBtn.Enabled = true;
		}

		public GenerateAt GenerateAt {
			get {
				return (GenerateAt)(GenerateAtCustomRadio.Checked ? 2 : GenerateAtCursorRadio.Checked ? 1 : 0);
			}
			set {
				GenerateAtBeginRadio.Related.Selected = value == GenerateAt.BEGIN ? GenerateAtBeginRadio :
					value == GenerateAt.CURSOR ? GenerateAtCursorRadio :
					value == GenerateAt.CUSTOM ? GenerateAtCustomRadio : GenerateAtBeginRadio.Related.Selected;
			}
		}
		
		public MidiUseBpm MidiUseBpm {
			get {
				return MidiMidiBpmCheck.Checked ? MidiUseBpm.MIDI :
					MidiProjectBpmCheck.Checked ? MidiUseBpm.PROJECT :
					MidiCustomBpmCheck.Checked ? MidiUseBpm.CUSTOM : MidiUseBpm.DYNAMIC_MIDI;
			}
			set {
				MidiMidiBpmCheck.Related.Selected = value == MidiUseBpm.MIDI ? MidiMidiBpmCheck :
					value == MidiUseBpm.PROJECT ? MidiProjectBpmCheck :
					value == MidiUseBpm.CUSTOM ? MidiCustomBpmCheck : MidiDynamicMidiBpmCheck;
			}
		}
		
		private int MidiUseBpm_int {
			get { return (int)MidiUseBpm; }
			set { MidiUseBpm = (MidiUseBpm)value; }
		}

		public Timecode GenerateAtCustomTimecode = Timecode.FromMilliseconds(0);

		private void GenerateAtCustomText_Leave(object sender, EventArgs e) {
			GenerateAtCustomTimecode = Timecode.FromPositionString(GenerateAtCustomText.Text);
			GenerateAtCustomText.Text = GenerateAtCustomTimecode.ToPositionString();
		}

		private void TrackLegatoBtn_MouseDown(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left) {
				Button button = sender as Button;
				TrackLegatoMenu.Show(button, new Point(0, button.Height));
			}
		}

		private void TrackLegatoMenuItems_Click(object sender, EventArgs e) {
			if (sender == stackingTracksToolStripMenuItem) parent.StackingLegatoTracks();
			else if (sender == limitStretchLegatoTracksToolStripMenuItem) parent.StretchLegatoTracks(false);
			else if (sender == stretchLegatoTracksToolStripMenuItem) parent.StretchLegatoTracks(true);
			else if (sender == lengthenLegatoToolStripMenuItem) parent.LengthenLegatoTracks();
			TrackLegatoBtn.Enabled = false;
			IsIrreversibleCancel = true;
			vegas.UpdateUI();
		}

		private bool _isIrreversibleCancel = false;
		private bool IsIrreversibleCancel {
			get { return _isIrreversibleCancel; }
			set {
				_isIrreversibleCancel = value;
				CancelBtn.Text = value ? Lang.str.close : Lang.str.cancel;
			}
		}

		private bool _isDynamicMidiBpm = false;
		public bool IsDynamicMidiBpm {
			get { return _isDynamicMidiBpm; }
			set {
				if (_isDynamicMidiBpm != value) {
					MidiDynamicTempoFlow.Visible = value;
					if (MidiMidiBpmCheck.Checked || MidiDynamicMidiBpmCheck.Checked)
						MidiMidiBpmCheck.Related.Selected = value ? MidiDynamicMidiBpmCheck : MidiMidiBpmCheck;
				}
				_isDynamicMidiBpm = value;
				if (!value && MidiDynamicMidiBpmCheck.Checked)
					MidiMidiBpmCheck.Related.Selected = MidiMidiBpmCheck;
			}
		}

		private void LoadPresetsToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
			bool isLoad;
			if (menuItem == loadPresetsToolStripMenuItem) isLoad = true;
			else if (menuItem == unloadPresetsToolStripMenuItem) isLoad = false;
			else return;
			if (MessageBox.Show(isLoad ? Lang.str.ensure_load_presets : Lang.str.ensure_unload_presets, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.OK) return;
			if (isLoad) parent.LoadPitchShiftPresets();
			else parent.UnloadPitchShiftPresets();
		}

		/// <summary>
		/// 将 Size 转化为字符串。
		/// </summary>
		/// <param name="size">Size 对象。</param>
		/// <returns>以逗号分隔的宽度和长度。</returns>
		private static string WriteSize(Size size) {
			return size.Width + "," + size.Height;
		}

		/// <summary>
		/// 将字符串转化为 Size。
		/// </summary>
		/// <param name="size_string">将 Size 对象处理过后的字符串。</param>
		/// <param name="size">返回 Size 对象。</param>
		/// <returns>是否成功读取数据。</returns>
		private static bool ReadSize(string size_string, out Size size) {
			try {
				Exception e = new Exception();
				string[] arr = size_string.Split(',');
				if (arr.Length != 2) throw e;
				int width = int.Parse(arr[0].Trim());
				int height = int.Parse(arr[1].Trim());
				size = new Size(width, height);
				return true;
			} catch (Exception) {
				size = new Size();
				return false;
			}
		}

		private void TrackLegatoBtn_Paint(object sender, PaintEventArgs e) {
			Button button = sender as Button;
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			const int ICON_SIZE = 8, MARGIN_RIGHT = 10;
			Rectangle r = new Rectangle(button.Width - MARGIN_RIGHT - ICON_SIZE, (button.Height - ICON_SIZE / 2) / 2, ICON_SIZE, ICON_SIZE / 2);
			e.Graphics.DrawLines(button.Enabled ? new Pen(button.ForeColor) : Pens.Gray, new Point[] {
				new Point(r.Left, r.Top),
				new Point(r.Left + r.Width / 2, r.Bottom),
				new Point(r.Right, r.Top)
			});
		}

		private void OnDragDrop(object sender, DragEventArgs e) {
			string fileName = e.GetFileNames()[0];
			if (sender == MidiConfigGroup)
				OpenMidiFile(fileName);
			if (sender == SourceConfigGroup)
				OpenSourceFile(fileName);
			OnDragLeave(sender, null);
		}

		private void OnDragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.None;
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string ext = new Path(e.GetFileNames()[0]).Extension.ToLower();
				var IsMediaExtension = new Func<string, bool>(extension => {
					string[] extensions = EntryPoint.MEDIA_EXTENSION.Split(';');
					foreach (string _extension in extensions)
						if (extension == _extension.Substring(1))
							return true;
					return false;
				});
				GroupBox group = sender as GroupBox;
				if (group == MidiConfigGroup && (ext == ".mid" || ext == ".midi") ||
					group == SourceConfigGroup && IsMediaExtension(ext)) {
					e.Effect = DragDropEffects.Link;
					group.BackColor = Color.FromArgb(128, Color.White);
					group.ForeColor = Color.Gray;
				}
			}
		}

		private void OnDragLeave(object sender, EventArgs e) {
			GroupBox group = sender as GroupBox;
			if (group == null) return;
			group.BackColor = Color.Transparent;
			group.ForeColor = SystemColors.ControlText;
		}

		private void QuickNormalizeBtn_Click(object sender, EventArgs e) {
			Cursor = Cursors.WaitCursor;
			parent.NormalizeSelectedAudioEvents();
			if (CloseAfterOpenHelperCheck.Checked) CancelBtn_Click(null, null);
			else {
				vegas.UpdateUI();
				MessageBox.Show(Lang.str.quick_normalize_complete, Lang.str.quick_normalize, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			Cursor = Cursors.Default;
		}

		private void RememberOnceFormSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			configIni.Write("FormSize", WriteSize(Size), "Personalize");
		}

		private void RestoreDefaultFormSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			rememberFormSizeToolStripMenuItem.Checked = false;
			configIni.DeleteKey("FormSize", "Personalize");
		}

		public MidiChannels midiChannels;
		public AutoLayoutTracksInfos layoutInfos;
		public bool IsMultiMidiChannel { get { return midiChannels != null; } }
		private void MidiChannelAdvancedBtn_Click(object sender, EventArgs e) {
			MidiChannelAdvancedForm advanced = new MidiChannelAdvancedForm(this, midiChannels, layoutInfos);
			if (advanced.ShowDialog() != DialogResult.OK || advanced.Channels.Count == 0) return;
			else if (advanced.Channels.Count == 1) {
				MidiChannelCombo.SelectedItem = advanced.Channels[0];
				return;
			}
			for (int i = MidiChannelCombo.Items.Count - 1; i >= 0; i--) {
				object item = MidiChannelCombo.Items[i];
				if (item is MidiChannels)
					MidiChannelCombo.Items.Remove(item);
			}
			midiChannels = advanced.Channels;
			layoutInfos = advanced.LayoutInfos;
			MidiChannelCombo.Items.Add(midiChannels);
			MidiChannelCombo.SelectedItem = midiChannels;
		}

		private void MidiChannelCombo_SelectedIndexChanged(object sender, EventArgs e) {
			// 直接边 foreach 边删除可能会导致遗漏。
			for (int i = MidiChannelCombo.Items.Count - 1; i >= 0; i--) {
				object item = MidiChannelCombo.Items[i];
				if (item is MidiChannels && MidiChannelCombo.SelectedItem != item) {
					MidiChannelCombo.Items.Remove(item);
					if (item == midiChannels) {
						midiChannels = null;
						layoutInfos = null;
					}
				}
			}
		}
	}

	#region 翻译
	/// <summary>
	/// 多语言翻译类
	/// </summary>
	public class Lang {
		public static Lang SChinese;
		public static Lang TChinese;
		public static Lang English;
		public static Lang Japanese;
		public static Lang Russian;

		/// <summary>
		/// 获取当前语言的字符串值。
		/// </summary>
		public static Lang str = SChinese;
		/// <summary>
		/// 设定当前语言。
		/// </summary>
		/// <param name="lang">语言编码名称</param>
		public static void SetLanguage(string lang) {
			lang = lang.Trim().ToLower();
			if (lang.StartsWith("zh")) {
				if (lang == "zh-tw" || lang == "zh-hk" || lang == "zh-mo" || lang == "zht") str = TChinese;
				else str = SChinese;
			} else if (lang.StartsWith("ja")) str = Japanese;
			else if (lang.StartsWith("ru")) str = Russian;
			else str = English;
		}
		/// <summary>
		/// 获取当前语言名称。
		/// </summary>
		/// <returns></returns>
		public string GetLanguage() {
			return __name__;
		}

		public readonly string dialog_sign = "...";
		public readonly string dropdown_sign = "▾";
		public string __name__ = "简体中文",
			info_label_font = "Microsoft Yahei",
			ui_font = "Microsoft YaHei UI",
			restart_to_effect_language = "重新启动以使语言生效？",
			yes_to_restart_to_effect_language = "立即手动重启",
			no_to_restart_to_effect_language = "不重启以生效",
			version_number = "版本号",
			revision_date = "最后修订日期",
			vegas_version = "Vegas 版本",
			script_supported_vegas_version = "脚本支持版本",
			unsupported_vegas_version = "警告：当前脚本与您当前所使用的 Vegas 版本不匹配，部分功能可能不能正常工作！请重新安装正确版本的脚本。\n脚本支持的版本为：Vegas Pro {0}\n当前软件的版本为：Vegas Pro {1}",
			unsupported_vegas_version_title = "版本不匹配",
			unsupported_vegas_version_feature = "低于 Vegas Pro {0} 的版本不支持本功能。",
			midi_file_name = "MIDI 序列",
			all_files = "所有文件",
			choose_a_midi_file = "请选择一个 MIDI 文件",
			media_file_name = "支持的媒体文件",
			choose_a_source_file = "请选择一个视频或图片素材片段",
			error = "错误",
			details = "详细信息：",
			brightness_and_contrast = "亮度和对比度",
			invert = "反转",
			black_and_white = "黑白",
			lab_adjust = "LAB 调整",
			hsl_adjust = "HSL 调整",
			mirror = "镜像",
			pic_in_pic = "画中画",
			crop = "修剪",
			check_pitch_shift_presets = "正在检查移调插件的预设是否可用⋯⋯",
			no_pitch_shift_presets = "由于您试图使用“移调”效果插件调音，但是系统发现您并没有完全配置好所需的所有音效预设。您可尝试由脚本尝试为您自动添加预设，可能会添加失败。如果失败，则请按照使用教程的说明来手动操作。是否由脚本为您自动添加预设？",
			yes_to_add_pitch_shift_presets = "自动添加",
			no_to_add_pitch_shift_presets = "返回配置",
			no_pitch_shift_presets_title = "未找到所有的移调音效预设",
			add_pitch_shift_presets_successful = "添加预设完成！",
			add_pitch_shift_presets_fail = "添加预设失败！",
			add_pitch_shift_presets_fail_title = "很遗憾",
			ensure_load_presets = "确定加载预设？",
			ensure_unload_presets = "确定卸载预设？",
			reverse_suffix_tag = "（反转）",
			effect_init_forward = "正",
			effect_init_reversed = "反",
			effect_init_turned = "倒",
			effect_init_left = "左",
			effect_init_right = "右",
			effect_init_up = "上",
			effect_init_down = "下",
			effect_init_left_up = "左上",
			effect_init_right_up = "右上",
			effect_init_left_down = "左下",
			effect_init_right_down = "右下",
			effect_init_invert = "负",
			effect_init_conform = "符",
			effect_init_opposite = "对",
			effect_init_chromatic = "彩",
			effect_init_monochrome = "灰",
			effect_init_counter = "逆",
			effect_init_stepon = "继",
			effect_init_flat = "扁",
			effect_init_thin = "细",
			effect_init_blur = "糊",
			enable_all_effects = "开启所有效果",
			chorus = "合唱",
			vibrato = "颤音",
			wave = "波浪",
			multi_beat_delay = "多拍延迟",
			spherize = "球面化",
			warning_missing_plugin = "警告：找不到插件 “{0}”！",
			midi_channel = "通道",
			midi_notes_count = "音符数",
			midi_begin_note = "起音",
			instrument = "乐器",
			error_code = "错误代码：",
			processing_otomad = "正在生成音 MAD / YTPMV⋯⋯",
			processing_ytp = "正在生成 YTP⋯⋯",
			processing_it = "正在处理它",
			real_time_update = "实时更新当前进度（会减慢生成速度）",
			replacer_is = "指定的替换项为",
			replacer_info = "请先在轨道窗口中选中替换与被替换的素材，然后指定一个素材为替换的素材，剩余素材均为被替换素材。\n请先将替换素材的音视频创建分组，并确保替换素材放置在时间靠后的位置并且尽量不与其它被替换素材位于同一轨道。",
			replace_clips = "替换轨道素材",
			replaced_info = "则剩余 {0} 项轨道剪辑将被替换为选定素材。",
			replaced_label = "被替换项",
			replacer_label = "替换项",
			separation_replaced_info = "已选中 {0} 个轨道素材，其中 {1} 个音频剪辑，{2} 个视频剪辑。",
			separation_set_replaced = "将选中的 {0} 个素材设为被替换项",
			separation_set_replacer = "将选中的 {0} 个素材设为替换项",
			separation_back_to_select = "返回 Vegas 选定素材",
			separation_use_clip_group = "同时替换分组内其它剪辑",
			separation_reserve_original_name = "保留原剪辑名称",
			replacements_classic_specify = "同时指定",
			replacements_separation_specify = "分别指定",
			view = "查看",
			selected_items = "选中项",
			replace_clips_form_ensure_to_close = "在“分别指定”中已指定有部分轨道素材。\n退出之后系统可能不会保留您的更改。",
			track = "轨道",
			submit_select = "设定选中(&S)",
			every_few = "每几个选择一个",
			which_one = "选择每组第几个",
			select_how_many = "每次要选取几个",
			reset_select = "重置选择(&R)",
			quick_select_interval = "快速间隔选择",
			select_interval_info = "请先在 Vegas 轨道中选中一些素材，然后再打开本对话框，使用下面的功能。",
			select_events_count_info = "已选中 {0} 个轨道剪辑。",
			select_videotracks_count_info = "已选中 {0} 个视频轨道。",
			select_audioevents_count_info = "已选中 {0} 个音频轨道剪辑。",
			select_source_count_info = "已选中 {0} 项媒体素材。",
			square = "平方",
			custom = "自定义",
			row_count = "行数",
			column_count = "列数",
			fill = "填充",
			adapt = "适应",
			increase_padding = "增加边距",
			auto_layout_tracks = "自动布局轨道",
			grid_layout = "网格布局",
			box_3d_layout = "3D 方盒布局",
			selected_tracks_too_much = "您所选取的轨道数目过多，有 {0} 个，超出使用功能范围。\n请选取少量轨道后重试。",
			selected_tracks_too_much_title = "选取轨道数目过多",
			bottom_surface = "底面",
			top_surface = "顶面",
			right_surface = "右面",
			left_surface = "左面",
			back_surface = "后面",
			front_surface = "前面",
			box_3d_layout_info = "由于脚本功能限制，将会新建轨道并将选定轨道中的剪辑移动过去，原轨道中的轨道运动、效果等信息将会丢失。\n请在下方选定立方体的各个面所使用的轨道，如果为空则表示不设定该面。",
			delete_original_tracks = "删除原轨道",
			use_video_longer_side = "使用视频的长边作为立方体的棱长",
			use_video_longer_side_tooltip = "勾选后，将以项目设置中视频最长的一条边（即宽度与高度的最大值）作为立方体的棱长。",
			gradient_tracks = "渐变轨道",
			gradient_tracks_info = "选择一种渐变效果应用到所选的视频轨道：",
			rainbow_color = "彩虹色",
			gradually_saturated = "逐渐饱和",
			gradually_contrasted = "逐渐对比",
			threshold = "阈值",
			alternately_chromatic = "彩灰交替",
			alternately_negative = "正负交替",
			descending = "降序排序",
			ascending = "升序排序",
			change_tune_method = "更改调音算法",
			change_tune_method_info = "仅支持音频事件属性中的调音方法，不支持“移调”插件中的调音方法。",
			time_stretch_pitch_shift = "时间拉伸/音调转换",
			formant_change = "共振峰移位",
			pitch_change = "音调更改",
			method = "方法",
			pitch_lock = "锁定以拉伸",
			lock_pitch_instead_of_rate = "锁定音高而不是速度",
			none = "无",
			elastique = "Élastique",
			classic = "古典",
			elastique_pro = "专业",
			elastique_efficient = "高效",
			elastique_soloist_monophonic = "独奏（单声道）",
			elastique_soloist_speech = "独奏（语音）",
			classic_a01 = "A01. 音乐 1（最小变调，可能有回音）",
			classic_a02 = "A02. 音乐 2",
			classic_a03 = "A03. 音乐 3（回音较小）",
			classic_a04 = "A04. 音乐 4（快速，适合低音）",
			classic_a05 = "A05. 音乐 5",
			classic_a06 = "A06. 音乐 6",
			classic_a07 = "A07. 语音 1",
			classic_a08 = "A08. 语音 2",
			classic_a09 = "A09. 语音 3（快速）",
			classic_a10 = "A10. 独奏乐器 1",
			classic_a11 = "A11. 独奏乐器 2",
			classic_a12 = "A12. 独奏乐器 3",
			classic_a13 = "A13. 独奏乐器 4（回音较小）",
			classic_a14 = "A14. 独奏乐器 5",
			classic_a15 = "A15. 独奏乐器 6",
			classic_a16 = "A16. 独奏乐器 7（快速）",
			classic_a17 = "A17. 鼓，无音高（最小回音）",
			classic_a18 = "A18. 鼓（更适用于通鼓）",
			classic_a19 = "A19. 鼓（微弱回音）",
			batch_subtitle_generation = "批量生成字幕",
			batch_subtitle_generation_presets = "选择一个预先设定好的“字幕和文字”媒体发生器的预设：",
			batch_subtitle_generation_subtitles = "输入要插入的字幕文本（一行一个，忽略空行）：",
			batch_subtitle_generation_single_duration = "每个字幕持续时间",
			batch_subtitle_generation_suggestion_info = "稍后可开启“自动跟进”功能以便后续调整时间。",
			find_clips = "查找轨道素材",
			find_clips_select_info = "选中的第一个轨道剪辑",
			find_clips_info = "在上方选中相匹配的剪辑，确定之后将会选中这些剪辑。",
			find_clips_header_name = "名称",
			find_clips_header_num = "数目",
			find_clips_match_source = "与选中轨道剪辑相同的所有剪辑",
			find_clips_match_source_and_offset = "与选中轨道剪辑相同且开始偏移量相等的所有剪辑",
			find_clips_match_name = "与指定名称相匹配的剪辑",
			replace = "替换(&R)",
			apply = "应用(&A)",
			close = "关闭(&C)",
			complete = "完成(&O)",
			cancel = "取消(&C)",
			about = "关于(&A)",
			ok = "确定(&O)",
			balloon_title = "填写说明",
			midi_start_second_tooltip = "用于截取 MIDI 音乐的一部分。\n单位：秒。",
			midi_end_second_tooltip = "此处填写需要读取 MIDI 文件的时间长度。\n注意如果填写的值过小，将截去多余时间部分的音符。\n如果此处填写的值比起始秒数小或相等，则始终表示持续到整个音乐时长末尾。\n单位：秒。",
			midi_beat_conbo_tooltip = "目前仅用于五线谱的分页功能。\n暂时无法通过 MIDI 文件自动推测。",
			source_start_time_tooltip = "此处填写媒体素材裁剪的开始时间。\n单位：秒。",
			source_end_time_tooltip = "注意如果此处填写的数值比入点秒数小或相等，则始终表示持续到素材时间末尾。\n单位：秒。",
			no_tune = "不调音",
			pitch_shift_plugin = "移调效果插件",
			elastique_method = "弹性音调更改",
			classic_method = "古典音调更改",
			fool_tuning_method = "瞎调音",
			sine_wave = "正弦波",
			triangle_wave = "三角波",
			square_wave = "方波",
			sawtooth_wave = "锯齿波",
			tune_method_tooltip = "“移调效果插件”表示使用“音频 FX”中的“移调”效果插件改变音调，需要配置预设。\n“弹性音调更改”表示使用“Élastique”拉伸方式改变音调，也就是键盘上 +、- 键直接改变音调，\n有音高范围限制。",
			audio_lock_stretch_pitch_tooltip = "采用重采样方式，随着速度变化而改变音高。如果使用的是“弹性音调\n更改”方法，那么将会禁用拉伸音频功能。",
			preview_beep_duration_tooltip = "预听标准音高所持续的时间。\n单位：毫秒。",
			preview_tune_audio_tooltip = "勾选后，预听音频时会将音频素材调整到主音高中央 C。\n否则，预听标准音高将会播放原始音高处所设定的音高。",
			sheet_position_tooltip = "五线谱中间第三根线到屏幕中心的距离，上正下负。\n单位：像素。",
			sheet_width_tooltip = "将在屏幕中间所填的宽度内显示音符，用于左右留白，给左侧的谱号留间距。\n单位：像素。",
			sheet_gap_tooltip = "五线谱线与线之间的间距。\n单位：像素。",
			sheet_line_thickness_tooltip = "生成五线谱的谱线时，五线谱谱线的粗细。当粗细值达到 100 % 后，谱线将会占满整个谱线间距。\n单位：百分比。",
			sheet_relative_tooltip = "勾选后，下方所填参数的像素单位将以相对于 1920 × 1080\n的尺寸进行定位；反之则以项目尺寸定位。",
			sheet_relative = "使用相对值",
			preview_base_pitch_tooltip = "请确保开启声音并且未将声音方案设置为无声。\n如果仍没有声音，请重启系统。",
			ytp_max_length_tooltip = "指定单个轨道剪辑的最大长度。\n单位：毫秒。",
			ytp_min_length_tooltip = "指定单个轨道剪辑的最小长度。\n单位：毫秒。",
			file = "文件(&F)",
			save_config = "保存用户配置(&S)",
			reset_config = "重置用户配置(&R)",
			exit_discarding_changes = "放弃更改并退出(&D)",
			exit = "退出(&X)",
			pitch_shift_preset = "移调插件预设(&P)",
			load_presets = "加载预设",
			unload_presets = "卸载预设",
			form_size = "窗体大小",
			remember_form_size = "记住窗体大小",
			remember_always_form_size = "每次都记住窗体大小",
			remember_once_form_size = "记住一次窗体大小",
			restore_default_form_size = "下次恢复默认窗体大小",
			help = "帮助(&H)",
			user_help = "使用说明",
			trouble_shooting = "疑难解答",
			update_info = "更新说明",
			repository_link = "仓库地址",
			latest_version_link = "最新版本链接",
			roadmap = "路线图",
			update_logs = "更新日志",
			tutorial_video = "教程视频",
			release_notes = "发行说明",
			staff_visualizer_documentation = "五线谱可视化文档",
			english_documentations = "英语说明文档",
			chinese_documentations = "中文说明文档",
			check_update = "检查更新(&U)",
			why_ok_btn_is_disabled = "为什么无法点击完成按钮？",
			media = "媒体",
			audio = "音频",
			video = "视频",
			staff = "五线谱",
			ytp = "YTP",
			helper = "工具",
			midi_settings = "MIDI 属性",
			midi_start_time = "起始秒数",
			midi_end_time = "终止秒数",
			bpm_setting = "设定 BPM 速度为",
			midi_beat = "节拍　　",
			midi_channel_setting = "使用 MIDI 通道",
			browse = "浏览...",
			no_midi_selected = "<未选择 MIDI 文件>",
			choose_midi_file = "选择 MIDI 文件",
			midi_dynamic_midi_bpm = "动态 MIDI 速度",
			midi_midi_bpm = "MIDI 速度",
			midi_project_bpm = "项目速度",
			midi_custom_bpm = "自定义",
			dynamic_midi_bpm_info = "{0} 起始的动态速度",
			dynamic_midi_beat_info = "{0} 起始的动态节拍",
			colon = "：",
			semicolon = "；",
			source_settings = "素材属性",
			generate_at_begin = "项目开始处",
			generate_at_cursor = "光标处",
			generate_position = "设定生成开始位置",
			choose_source_file = "选择媒体素材",
			selected_media = "选中的媒体文件",
			selected_clip = "选中的轨道素材",
			source_start_time = "入点秒数",
			source_end_time = "出点秒数",
			parameters = "参数",
			linear = "线性",
			fast = "快速",
			slow = "慢速",
			smooth = "平滑",
			sharp = "急剧",
			hold = "保留",
			fade_in = "渐入　　",
			fade_out = "渐出　　",
			tune = "调音",
			tune_method = "调音方法",
			base_pitch = "原始音高",
			preview_listen = "预听",
			preview_base_pitch = "预听标准音高(&B)",
			preview_audio = "预听音频(&P)",
			stop_preview_audio = "停止预听(&P)",
			lock_attr = "锁定属性",
			preview_listen_attr = "预听属性",
			preview_tune_audio = "使音频调整到主音高",
			reserve_formant = "保持共振峰",
			stretch_attr = "拉伸属性",
			create_event_group = "创建分组",
			aconfig = "生成音频",
			audio_stretch = "拉伸音频",
			audio_loop = "循环音频",
			audio_normalize = "规范化音量",
			audio_lock_stretch_pitch = "锁定伸缩与音调",
			video_glow_bright = "发光亮度",
			video_glow = "发光",
			video_start_size = "起始尺寸",
			video_end_size = "终止尺寸",
			video_start_rotation = "起始旋转",
			video_end_rotation = "终止旋转",
			video_start_h_trans = "起始平移",
			video_end_h_trans = "终止平移",
			video_start_v_trans = "起始直移",
			video_end_v_trans = "终止直移",
			video_start_hue = "起始色相",
			video_end_hue = "终止色相",
			video_start_saturation = "起始饱和",
			video_end_saturation = "终止饱和",
			video_start_contrast = "起始对比",
			video_end_contrast = "终止对比",
			video_start_threshold = "起始阈值",
			video_end_threshold = "终止阈值",
			video_multitrack_for_chords = "复音多轨",
			effect = "效果",
			visual_effect = "视觉效果",
			initial_visual_effect = "初始值",
			no_effects = "无效果",
			h_flip = "水平翻转",
			v_flip = "垂直翻转",
			ccw_flip = "逆时针翻转",
			cw_flip = "顺时针翻转",
			ccw_rotate = "逆时针旋转",
			cw_rotate = "顺时针旋转",
			turned = "颠倒",
			h_mirror = "水平镜像",
			v_mirror = "垂直镜像",
			ccw_mirror = "逆时针镜像",
			cw_mirror = "顺时针镜像",
			negative = "颜色反转",
			lumin_invert = "亮度反转",
			hue_invert = "色相反转",
			step_change_hue = "{0} 步色差",
			chromatic_and_monochrome = "彩灰",
			pingpong = "乒乓效应",
			whirl = "爱的魔力转圈圈",
			vertical_expansion = "垂直扩张",
			vertical_expansion_with_rebound = "垂直扩张并回弹",
			vertical_compression = "垂直压缩",
			vertical_compression_with_rebound = "垂直压缩并回弹",
			oblique_expansion_and_compression = "斜向扩缩",
			puyo_puyo = "魔法气泡",
			pendulum = "左右摇摆",
			gaussian_blur = "高斯模糊",
			radial_blur = "径向模糊",
			vconfig = "生成视频",
			video_stretch = "拉伸视频",
			video_loop = "循环视频",
			freeze_first_frame = "静态画面",
			freeze_last_frame = "禁止延长",
			legato = "填补间隙",
			sheet_width = "谱面宽度",
			sheet_thickness = "谱线粗细",
			sheet_color = "谱线颜色",
			sheet_position = "谱面位置",
			sheet_clef = "谱号",
			sheet_gap = "谱线间距",
			sheet_g_clef = "高音",
			sheet_f_clef = "低音",
			sheet_notes_shift = "音符偏移",
			sheet_config = "启用五线谱可视化效果",
			sheet_generate = "生成五线谱",
			sheet_config_info = "欲开启五线谱视觉效果，需要先开启“生成视频”选项。\n开启本功能会禁用视频视觉效果和视频拉伸选项。",
			sheet_notes_params = "音符参数",
			sheet_line_sheet_params = "谱线参数",
			sheet_legacy_engine = "旧版引擎",
			sheet_legacy_engine_tooltip = "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\n即音符和谱线的位置和形状由轨道运动定位。",
			ytp_clips_count = "剪辑数目",
			ytp_min_length = "最小长度",
			ytp_max_length = "最大长度",
			ytp_chorus = "合唱",
			ytp_pitch_change = "更改音调",
			ytp_vibrato = "颤音（概率性附加波浪效果）",
			ytp_reverse = "倒放",
			ytp_delay = "延迟",
			ytp_speed_change = "更改速度",
			ytp_hue_change = "更改色相",
			ytp_hue_rotate = "旋转色相",
			ytp_monochrome = "黑白",
			ytp_negative = "反转颜色（概率性附加降调效果）",
			ytp_high_freq_repeat = "高频重复",
			ytp_random_tone = "随机调音（附加水平翻转效果）",
			ytp_enlarge = "放大（附加增加音量）",
			ytp_spherize = "球面化",
			ytp_mirror = "镜像",
			ytp_high_contrast = "高对比（附加增加音量）",
			ytp_oversaturation = "过饱和（概率性附加升调效果）",
			ytp_emphasize_thrice = "重说三（附加放大聚焦效果）",
			ytp_info = "在当前选项卡下单击“完成”按钮，将会生成 YTP 而不是音 MAD / YTPMV。\n除“生成音频”“生成视频”外其它的参数设置并不会在 YTP 中使用。",
			select_interval = "间隔选择",
			select_interval_configform_info = "本功能旨在辅助用户每隔一个或几个选中一个素材，然后可以执行“粘贴视频事件”等操作。",
			quick_normalize = "快速规范音量",
			quick_normalize_configform_info = "将选中的多个音频轨道剪辑全部规范化音量。",
			quick_normalize_complete = "完成规范化音量。",
			replace_clips_configform_info = "将多个轨道剪辑替换为指定的新轨道剪辑。",
			auto_layout_tracks_configform_info = "类 YTPMV 风格自动布局选中的轨道。",
			change_tune_method_configform_info = "将多个音频轨道剪辑统一更改为指定的调音算法。",
			batch_subtitle_generation_configform_info = "预先设定好“字幕和文字”的预设，然后在此添加多行文本。",
			find_clips_configform_info = "根据指定的条件（如剪辑名称、与选中剪辑相同的素材等）选中符合条件的所有轨道剪辑。",
			clear_tracks_motion = "清除轨道运动",
			clear_tracks_effect = "清除轨道效果",
			track_legato = "填补轨道间隙",
			track_legato_stacking = "堆积素材",
			track_legato_limit_stretch = "拉伸素材（限制在拉伸极限范围之内）",
			track_legato_stretch = "拉伸素材",
			track_legato_lengthen = "改变素材持续时间",
			helper_info = "以下功能只是一些独立的辅助功能，与其它生成音视频的参数无关。",
			helper_info_warning = "注意：操作之后将会关闭本对话框，您可以稍后再重新打开，部分您未保存的更改可能会丢失！",
			close_after_open_helper = "操作完成之后关闭本对话框",
			otomad_helper_config = "Otomad Helper for Vegas - 配置",
			reset_config_successful = "重置完成，请重新启动脚本。",
			reset_config_successful_title = "重置用户配置",
			sure_to_reset_config = "确定要重置用户配置吗？\n\n重置后您的用户配置数据将会丢失。",
			about_title = "关于",
			script_author = "脚本作者",
			script_original_author = "脚本原作者",
			documentation = "说明文档",
			why_ok_btn_is_disabled_info = "请按照下列步骤依次检查问题：",
			why_ok_btn_is_disabled_no_audio_and_video_enabled = "“生成音频”与“生成视频”被同时取消勾选。请至少勾选生成其中一项。",
			why_ok_btn_is_disabled_no_media_take = "所选的媒体素材来源不包含任何有效媒体资源。",
			why_ok_btn_is_disabled_no_midi_select = "若要生成音 MAD / YTPMV，请先选择一个 MIDI 序列文件。",
			why_ok_btn_is_disabled_in_helper_tab = "为避免误操作，切勿在“工具”选项卡下进行提交生成操作。",
			why_ok_btn_is_disabled_unknown_problem = "未知原因。",
			no_selected_media_warning = "警告：您没有在项目媒体窗口中选中任何有效媒体素材！",
			no_selected_clip_warning = "警告：您没有在轨道窗口中选中任何剪辑片段！",
			preview_audio_track_name = "预听音频轨道（应该被删除！）",
			no_midi_exception = "错误：未选择 MIDI 文件。\n\n请重新打开脚本参数配置对话框，然后在“MIDI 属性”分组中点击“浏览”按钮，打开一个有效的 MIDI 文件。",
			no_media_exception = "错误：未选择媒体文件。\n\n请重新打开脚本参数配置对话框，然后在“媒体属性”分组中点击“浏览”按钮，打开一个有效的媒体文件。",
			no_track_info_exception = "错误：没有 MIDI 音轨。\n\n可能的原因：\n1. 您没有选择一个 MIDI 音轨；\n2. 该 MIDI 文件中没有任何音轨；\n3. 该 MIDI 文件已损坏或文件格式不受支持。",
			no_plugin_pitch_shift_exception = "错误：无法调用移调插件。\n\n请按照教程文档 {0} 的指引正确操作。\n不过，根据这个更新版本的脚本，按理应当是中英文版本均可正常运行的。\n因此很有可能您是使用其它语言的 Vegas 造成的（逃",
			no_plugin_presets_exception = "错误：无法调用移调插件的预设效果。\n\n请按照教程文档 {0} 的指引正确操作。\n确保在移调插件中手动添加了所有的 25 个预设，且命名正确。\n\n补充说明：具体可见上述链接专栏中对于安装方法的说明。这 25 个预设是上下一个八度以内的所有变调种类，\n缺少任何一个都有可能出错。手动添加预设的确非常麻烦，但 Vegas 无法使用脚本来指定变调的具体参数，\n因此只好绕这个弯子。",
			no_plugin_name_exception = "错误：无法调用{0}插件。\n\n可能您使用的是非中文版的 Vegas 或其它尚未测试版本的 Vegas。",
			no_take_exception_ps = "补充说明：若仍不能解决，说明该素材文件可能是 Vegas 不支持的格式，\n可以手动把该文件拖入 Vegas 中看一下是否视频音频都正常。",
			no_audio_take_exception = "错误：无法读取音频媒体流。\n\n在设置界面，纯视频/图片素材不要勾选“生成音频”。\n\n",
			no_video_take_exception = "错误：无法读取视频媒体流。\n\n在设置界面，纯音频素材不要勾选“生成视频”。\n\n",
			no_media_take_exception = "错误：无法读取媒体。\n\n您所选的文件格式不受 Vegas 支持，请检查该媒体文件是否损坏，或未安装对应的 Vegas 解码器。\n\n",
			not_a_midi_file_exception = "错误：无法读取 MIDI 文件。\n\n解决方法：用宿主软件导入该 MIDI，然后重新输出一个新的 MIDI 文件。\n\n补充说明：MIDI 文件有多种格式，脚本不保证都能够正确读取。所幸主流宿主软件在\n默认设置下导出的 MIDI 文件一般是可以读取的。（目前测试过 FL Studio、LMMS \n与 Music Studio for iPad。）",
			no_selected_exception_ps = "补充说明：如果您想手动在文件夹中选择一个媒体素材，那么请点击其右边的“浏览”按钮，\n选择一个媒体素材。并确保左侧的下拉菜单中选中的是您所选文件所在的路径。",
			no_selected_media_exception = "错误：没有在项目媒体窗口中选择任何媒体。\n\n请在项目媒体窗口中选择一个媒体，然后重新打开参数配置窗口，并在素材设置中选择“选中的媒体文件”。\n\n",
			no_selected_clip_exception = "错误：没有在轨道中选择任何剪辑。\n\n请在轨道中选择一个剪辑，然后重新打开参数配置窗口，并在素材设置中选择“选中的轨道素材”。\n\n",
			no_time_stretch_pitch_shift_exception = "错误：选定素材音调转换方法被设置为不调音。\n\n很有可能您使用的是“选中的轨道素材”。出现了这个错误不怪你，要怪就怪 Vegas 这个脑残设计。\n\n解决方法：请重新选中您的轨道素材，右键音频部分，选择底部的“属性”。将“时间拉伸/音调转换”的“方法”设定为“élastique”。\n然后点击确定即可。\n\n补充说明：如果某个音频事件没有进行变调操作，然后打开了它的属性，那么其属性中的“时间拉伸/音调转换”的“方法”会被\n自动修改为“无”，点击确定就会生效。这时你会发现键盘上的 +、- 键调音操作无效了。这时必须重新打开音频事件的属性，\n将“时间拉伸/音调转换”的“方法”设定为“élastique”，不必设置“音调更改”，点击确定即可。",
			read_config_fail_exception = "错误：读取参数配置文件失败。\n\n很遗憾您遇到了这个不可预见的错误。我们将会清除用户配置设置并恢复为默认值以便解决问题。\n建议将这个错误告诉作者以便快速解决问题。\n将会退出此脚本，然后劳烦阁下手动重新打开此脚本。",
			fail_to_select_clips_exception = "错误：选取轨道剪辑出错。\n\n请先在轨道窗口中选取部分轨道剪辑。",
			fail_to_select_tracks_exception = "错误：选取轨道出错。\n\n请先在轨道窗口中选取部分视频轨道。",
			ytp_over_length_exception = "错误：指定的 YTP 最小长度超过了媒体长度。\n\n指定的 YTP 最小长度过大，请尝试更小的值。或所选媒体素材长度过小。",
			ytp_in_media_generator_exception = "错误：对媒体生成器产生的媒体应用 YTP。\n\n应用 YTP 必须使用本地媒体文件，不要使用媒体生成器生成的媒体。",
			ytp_eliminate_duplicates_finally_null_exception = "技术异常：对 YTP 素材列表进行去重操作，最后列表为空了！（雾‽）\n\n这是一个不应该发生的错误。",
			unknown_exception = "错误：未知异常。\n\n请展开详细信息查看具体错误内容，并将错误信息反馈给作者。",
			use_pic_in_pic_on_unsupported_vegas_exception = "错误：不支持在低版本 Vegas 中使用该画中画插件的效果。\n\n根本原因：Vegas 在新版本“画中画”效果插件中增加了一些新的功能和参数，这些新参数不能在低版本的 Vegas 插件中使用。\n\n解决方法：在当前 Vegas 版本不能使用该 PV 节奏视觉效果（如扩缩类等），请使用其它视觉效果。或更新 Vegas 软件。";

		static Lang() {
			SChinese = new Lang();
			English = new Lang {
				__name__ = "English",
				info_label_font = "Segoe UI",
				ui_font = "Segoe UI",
				restart_to_effect_language = "Restart for language to take effect?",
				yes_to_restart_to_effect_language = "Restart",
				no_to_restart_to_effect_language = "Re-render",
				version_number = "Version",
				revision_date = "Last revision date",
				vegas_version = "Vegas version",
				script_supported_vegas_version = "Script supported version",
				unsupported_vegas_version = "Warning: The current script does not match the version of Vegas you are currently using. Some features may not work properly! Please reinstall the correct version of the script.\nSupported version: Vegas Pro {0}\nCurrent version: Vegas Pro {1}",
				unsupported_vegas_version_title = "Version mismatch",
				unsupported_vegas_version_feature = "Versions below Vegas Pro {0} do not support this feature.",
				midi_file_name = "MIDI sequence",
				all_files = "All files",
				choose_a_midi_file = "Please select a MIDI file",
				media_file_name = "Supported media files",
				choose_a_source_file = "Please select a video or picture clip",
				error = "Error",
				details = "Details:",
				brightness_and_contrast = "Brightness and Contrast",
				invert = "Invert",
				black_and_white = "Black and White",
				lab_adjust = "LAB Adjust",
				hsl_adjust = "HSL Adjust",
				mirror = "Mirror",
				pic_in_pic = "Picture in Picture",
				crop = "Crop",
				check_pitch_shift_presets = "Checking whether the presets of the Pitch Shift plug-in are available...",
				no_pitch_shift_presets = "Since you tried to tune using the \"Pitch Shift\" effect plug-in, the system found that you did not fully configure all the audio presets you needed. The script can attempt to add presets for you, which may fail. If that fails, follow the instructions for using the tutorial manually. Would you like the script to automatically add presets for you?",
				yes_to_add_pitch_shift_presets = "Auto Add",
				no_to_add_pitch_shift_presets = "Back",
				no_pitch_shift_presets_title = "Not all Pitch Shift presets found",
				add_pitch_shift_presets_successful = "Finish adding presets!",
				add_pitch_shift_presets_fail = "Failed to add presets!",
				add_pitch_shift_presets_fail_title = "Unfortunately",
				ensure_load_presets = "Are you sure you want to load presets?",
				ensure_unload_presets = "Are you sure you want to unload presets?",
				reverse_suffix_tag = "(Reversed)",
				effect_init_forward = "Forward",
				effect_init_reversed = "Reversed",
				effect_init_turned = "Turned",
				effect_init_left = "Left",
				effect_init_right = "Right",
				effect_init_up = "Top",
				effect_init_down = "Bottom",
				effect_init_left_up = "Upper left",
				effect_init_right_up = "Upper right",
				effect_init_left_down = "Lower left",
				effect_init_right_down = "Lower right",
				effect_init_invert = "Invert",
				effect_init_conform = "Conform",
				effect_init_opposite = "Opposite",
				effect_init_chromatic = "Chromatic",
				effect_init_monochrome = "Monochrome",
				effect_init_counter = "Counter",
				effect_init_stepon = "Continue",
				effect_init_flat = "Flat",
				effect_init_thin = "Thin",
				effect_init_blur = "Blur",
				enable_all_effects = "Enable All Effects",
				chorus = "Chorus",
				vibrato = "Vibrato",
				wave = "Wave",
				multi_beat_delay = "Multi-beat Delay",
				spherize = "Spherize",
				warning_missing_plugin = "Warning: The plug-in \"{0}\" could not be found!",
				midi_channel = "CH",
				midi_notes_count = "Notes Count",
				midi_begin_note = "Begin Note",
				instrument = "Inst",
				error_code = "Error Code:",
				processing_otomad = "Generating Otomad/YTPMV...",
				processing_ytp = "Generating YTP...",
				processing_it = "Processing it",
				real_time_update = "Real time update current progress (it will slow down the generation)",
				replacer_is = "The specified replacement is",
				replacer_info = "Please select the clips which to be replaced and replaced in the track window first, and then specify a clip as the replacement clip, and the remaining clips are all replaced clips.\nPlease create a group for the audio and video of the replacement clip first, and make sure that the replacement clip is placed later in time and not on the same track as other replaced clips.",
				replace_clips = "Replace Track Events",
				replaced_info = "Then the remaining {0} track clips will be replaced with the selected clip.",
				replaced_label = "Replaced",
				replacer_label = "Replacement",
				separation_replaced_info = "{0} track clips selected, containing {1} audio clip, {2} video clips.",
				separation_set_replaced = "Set the selected {0} clips as the replaced",
				separation_set_replacer = "Set the selected {0} clips as the replacement",
				separation_back_to_select = "Back to Vegas to select clips",
				separation_use_clip_group = "Also replace other clips in the track group",
				separation_reserve_original_name = "Reserve original clip name",
				replacements_classic_specify = "Specify simultaneously",
				replacements_separation_specify = "Specify separately",
				view = "View",
				selected_items = "Selected",
				replace_clips_form_ensure_to_close = "Some track clips have been specified in \"Specify separately\".\nYour changes may not be retained after exit.",
				track = "Track",
				submit_select = "&Set selected",
				every_few = "Select one for every few",
				which_one = "Select which one of group",
				select_how_many = "Select how many at a time",
				reset_select = "&Reset selection",
				quick_select_interval = "Quick Select Interval",
				select_interval_info = "Please select some clips in the Vegas track window first, and then open this dialog box to use the following functions.",
				select_events_count_info = "{0} track events have been selected.",
				select_videotracks_count_info = "{0} video tracks have been selected.",
				select_audioevents_count_info = "{0} audio track events have been selected.",
				select_source_count_info = "{0} media sources has been selected.",
				square = "Square",
				custom = "Custom",
				row_count = "Rows",
				column_count = "Columns",
				fill = "Fill",
				adapt = "Fit",
				increase_padding = "Increase padding",
				auto_layout_tracks = "Auto Layout Tracks",
				grid_layout = "Grid Layout",
				box_3d_layout = "3D Box Layout",
				selected_tracks_too_much = "You've selected {0}tracks, which is beyond the scope of available functions. That's too many!\nPlease select fewer tracks and try again.",
				selected_tracks_too_much_title = "Too many tracks selected",
				bottom_surface = "Bottom",
				top_surface = "Top",
				right_surface = "Right",
				left_surface = "Left",
				back_surface = "Back",
				front_surface = "Front",
				box_3d_layout_info = "Due to the limitation of the script function, a new track will be created and the clips in the selected track will be moved over, and the track motion, effects or other in the original track will be lost.\nPlease select the track used by each face of the cube below. If it is empty, it means that the face is unset.",
				delete_original_tracks = "Delete original tracks",
				use_video_longer_side = "Use longer side of edge length",
				use_video_longer_side_tooltip = "After checking, the longest side of the video in the project settings (that is, the maximum of width and height) will be used as the edge length of the cube.",
				gradient_tracks = "Gradient Tracks",
				gradient_tracks_info = "Choose a gradient effect to apply to the selected video tracks:",
				rainbow_color = "Rainbow Colors",
				gradually_saturated = "Gradually Saturated",
				gradually_contrasted = "Gradually Contrasted",
				threshold = "Threshold",
				alternately_chromatic = "Alternately Chromatic",
				alternately_negative = "Alternate Negative",
				descending = "Descending",
				ascending = "Ascending",
				change_tune_method = "Change Tuning Method",
				change_tune_method_info = "Only tuning methods in audio event properties are supported, not in the Pitch Shift plug-in.",
				time_stretch_pitch_shift = "Time stretch / pitch shift",
				formant_change = "Formant shift",
				pitch_change = "Pitch semis",
				method = "Method",
				pitch_lock = "Pitch lock",
				lock_pitch_instead_of_rate = "Lock pitch instead of rate",
				none = "None",
				classic = "Classic",
				elastique_pro = "Professional",
				elastique_efficient = "Efficient",
				elastique_soloist_monophonic = "Soloist (Monophonic)",
				elastique_soloist_speech = "Soloist (Speech)",
				classic_a01 = "A01. Music 1 (minimum flange, may echo)",
				classic_a02 = "A02. Music 2",
				classic_a03 = "A03. Music 3 (less echo)",
				classic_a04 = "A04. Music 4 (fast, good for bass)",
				classic_a05 = "A05. Music 5",
				classic_a06 = "A06. Music 6",
				classic_a07 = "A07. Speech 1",
				classic_a08 = "A08. Speech 2",
				classic_a09 = "A09. Speech 3 (fast)",
				classic_a10 = "A10. Solo instruments 1",
				classic_a11 = "A11. Solo instruments 2",
				classic_a12 = "A12. Solo instruments 3",
				classic_a13 = "A13. Solo instruments 4 (less echo)",
				classic_a14 = "A14. Solo instruments 5",
				classic_a15 = "A15. Solo instruments 6",
				classic_a16 = "A16. Solo instruments 7 (fast)",
				classic_a17 = "A17. Drums, unpitched (munimum echo)",
				classic_a18 = "A18. Drums (better for toms)",
				classic_a19 = "A19. Drums (tiny echo)",
				batch_subtitle_generation = "Batch Subtitle Generation",
				batch_subtitle_generation_presets = "Select a Title & Text media generator preset:",
				batch_subtitle_generation_subtitles = "Enter the subtitle text (line by line, blank lines ignored):",
				batch_subtitle_generation_single_duration = "Duration of each subtitle",
				batch_subtitle_generation_suggestion_info = "Turn on the Auto Follow-up function to adjust the time later.",
				find_clips = "Find Track Events",
				find_clips_select_info = "First track event selected",
				find_clips_info = "Select the matching clips above and they will be selected when you are sure.",
				find_clips_header_name = "Name",
				find_clips_header_num = "Number",
				find_clips_match_source = "Matching the same source",
				find_clips_match_source_and_offset = "Matching the same source with the same starting offset",
				find_clips_match_name = "Matching the specified name",
				replace = "&Replace",
				apply = "&Apply",
				close = "&Close",
				complete = "C&omplete",
				cancel = "&Cancel",
				about = "&About",
				ok = "&OK",
				balloon_title = "Filling Instructions",
				midi_start_second_tooltip = "Used to intercept part of MIDI music.\nUnit: seconds.",
				midi_end_second_tooltip = "Fill in the length of time needed to read the MIDI file here.\r\nNote that if the value filled in is too small, the notes in the excess time will be cut off.\r\nIf the value entered here is less than or equal to the start seconds, it always means that it lasts to the end of the entire music duration.\r\nUnit: seconds.",
				midi_beat_conbo_tooltip = "Currently only used for the pagination function of staff.\nAutomatic speculation from MIDI files is temporarily unavailable.",
				source_start_time_tooltip = "Fill in the start time of media material cutting here.\nUnit: seconds.",
				source_end_time_tooltip = "Note that if the value entered here is less than or equal to the number of start seconds, it always means that it lasts until the end of the media time.\nUnit: seconds.",
				no_tune = "No Tuning",
				pitch_shift_plugin = "Pitch Shift Audio Effect Plug-in",
				elastique_method = "Elastic Pitch Change",
				classic_method = "Classic Pitch Change",
				fool_tuning_method = "Fiddle with Tuning",
				sine_wave = "Sinusoid",
				triangle_wave = "Triangle",
				square_wave = "Square",
				sawtooth_wave = "Sawtooth",
				tune_method_tooltip = "\"Pitch Shift Audio Effect Plug-in\" means to use the \"Pitch Shift\" effect plug-in in \"Audio FX\" to change the pitch, and the presets needs to be configured.\r\n\"Elastic Pitch Change\" means to use the \"Élastique\" stretching method to change the pitch, that is, the + and-keys on the keyboard directly change the pitch, and the pitch range is limited.",
				audio_lock_stretch_pitch_tooltip = "Use resampling to change pitch as speed changes. Stretch audio will be disabled if the \"Elastic Pitch Change\" method is used.",
				preview_beep_duration_tooltip = "The duration of pre-listening to the base pitch.\nUnit: milliseconds.",
				preview_tune_audio_tooltip = "If checked, the audio source will be tuned to the tonic central C when pre-listening the audio.\nOtherwise, the tone which set by the base pitch will be produced when pre-listening the base pitch.",
				sheet_position_tooltip = "The distance from the third line in the middle of the staff to the center of the screen. Up plus down minus.\nUnit: pixel.",
				sheet_width_tooltip = "The notes will be displayed in the width filled in the middle of the screen, used for left and right white space, and left space for the clef on the left.\nUnit: pixel.",
				sheet_gap_tooltip = "The distance between the lines in the staff.\nUnit: pixel.",
				sheet_line_thickness_tooltip = "The thickness of the sheet lines while generating staff. When the thickness reaches 100%, the sheet lines will occupy the entire sheet lines spacing.\nUnit: percentage.",
				sheet_relative_tooltip = "After checking, the pixel unit of the parameters filled below will be positioned relative to the size of 1920 × 1080;\r\notherwise, it will be positioned based on the project size.",
				sheet_relative = "Using relative values",
				preview_base_pitch_tooltip = "Please make sure that the sound is turned on and the sound scheme is not set to the silent.\nIf there is still no work, please restart your system.",
				ytp_max_length_tooltip = "Specify the maximum length of a single track clip.\nUnit: milliseconds.",
				ytp_min_length_tooltip = "Specify the minimum length of a single track clip.\nUnit: milliseconds.",
				file = "&File",
				save_config = "&Save user configuration",
				reset_config = "&Reset user configuration",
				exit_discarding_changes = "&Discard changes and exit",
				exit = "E&xit",
				pitch_shift_preset = "&Pitch shift plug-in presets",
				load_presets = "Load presets",
				unload_presets = "Unload presets",
				form_size = "Form size",
				remember_form_size = "Remember form size",
				remember_always_form_size = "Remember form size each time",
				remember_once_form_size = "Remember form size once",
				restore_default_form_size = "Restore default form size next time",
				help = "&Help",
				user_help = "Instructions",
				trouble_shooting = "Troubleshoot",
				update_info = "Release notes",
				repository_link = "Repository link",
				latest_version_link = "Latest version link",
				roadmap = "路线图",
				update_logs = "更新日志",
				tutorial_video = "教程视频",
				release_notes = "发行说明",
				staff_visualizer_documentation = "五线谱可视化文档",
				english_documentations = "英语说明文档",
				chinese_documentations = "中文说明文档",
				check_update = "Check for &updates",
				why_ok_btn_is_disabled = "Why is the Complete button disabled?",
				media = "Media",
				audio = "Audio",
				video = "Video",
				staff = "Staff",
				ytp = "YTP",
				helper = "Tools",
				midi_settings = "MIDI configuration",
				midi_start_time = "Start seconds",
				midi_end_time = "End seconds",
				bpm_setting = "Set the BPM tempo to",
				midi_beat = "Beat",
				midi_channel_setting = "Using MIDI channel",
				browse = "Browse...",
				no_midi_selected = "<No MIDI file selected>",
				choose_midi_file = "Select MIDI file",
				midi_dynamic_midi_bpm = "Dynamic MIDI tempo",
				midi_midi_bpm = "MIDI tempo",
				midi_project_bpm = "Project tempo",
				midi_custom_bpm = "Custom",
				dynamic_midi_bpm_info = "Dynamic tempo from {0}",
				dynamic_midi_beat_info = "Dynamic beat from {0}",
				colon = ": ",
				semicolon = "; ",
				source_settings = "Source configuration",
				generate_at_begin = "Project start",
				generate_at_cursor = "Cursor",
				generate_position = "Generate at",
				choose_source_file = "Select media file",
				selected_media = "Selected media file",
				selected_clip = "Selected track event",
				source_start_time = "Start seconds",
				source_end_time = "End seconds",
				parameters = "Parameters",
				linear = "Linear",
				fast = "Fast",
				slow = "Slow",
				smooth = "Smooth",
				sharp = "Sharp",
				hold = "Hold",
				fade_in = "Fade in",
				fade_out = "Fade out",
				tune = "Tuning",
				tune_method = "Tuning method",
				base_pitch = "Base pitch",
				preview_listen = "Preview",
				preview_base_pitch = "Preview the &base pitch",
				preview_audio = "&Preview audio",
				stop_preview_audio = "Stop &previewing",
				lock_attr = "Lock attrs",
				preview_listen_attr = "Preview attrs",
				preview_tune_audio = "Adjust audio to base pitch",
				reserve_formant = "Reserve formant",
				stretch_attr = "Stretch attrs",
				create_event_group = "Create groups",
				aconfig = "Enabled",
				audio_stretch = "Stretch",
				audio_loop = "Loop",
				audio_normalize = "Normalize",
				audio_lock_stretch_pitch = "Lock stretch and pitch",
				video_glow_bright = "Glow brightness",
				video_glow = "Glow",
				video_start_size = "Start size",
				video_end_size = "End size",
				video_start_rotation = "Start rotation",
				video_end_rotation = "End rotation",
				video_start_h_trans = "Start horizontal shift",
				video_end_h_trans = "End horizontal shift",
				video_start_v_trans = "Start vertical shift",
				video_end_v_trans = "End vertical shift",
				video_start_hue = "起始色相",
				video_end_hue = "终止色相",
				video_start_saturation = "起始饱和",
				video_end_saturation = "终止饱和",
				video_start_contrast = "起始对比",
				video_end_contrast = "终止对比",
				video_start_threshold = "起始阈值",
				video_end_threshold = "终止阈值",
				video_multitrack_for_chords = "复音多轨",
				effect = "Effects",
				visual_effect = "Visual effects",
				initial_visual_effect = "Initial value",
				no_effects = "No Effects",
				h_flip = "Horizontal Flip",
				v_flip = "Vertical Flip",
				ccw_flip = "Counterclockwise Flip",
				cw_flip = "Clockwise Flip",
				ccw_rotate = "Counterclockwise Rotation",
				cw_rotate = "Clockwise Rotation",
				turned = "Turned",
				h_mirror = "Horizontal Mirror",
				v_mirror = "Vertical Mirror",
				ccw_mirror = "Counterclockwise Mirror",
				cw_mirror = "Clockwise Mirror",
				negative = "Negative",
				lumin_invert = "Luminance Invert",
				hue_invert = "Hue Invert",
				step_change_hue = "{0} Steps Chromatic Aberration",
				chromatic_and_monochrome = "Chromatic and Monochrome",
				pingpong = "Ping-pong Effect",
				whirl = "Whirl",
				vertical_expansion = "Vertical Expansion",
				vertical_expansion_with_rebound = "Vertical Expansion with Rebound",
				vertical_compression = "Vertical Compression",
				vertical_compression_with_rebound = "Vertical Compression with Rebound",
				oblique_expansion_and_compression = "Oblique Expansion and Compression",
				puyo_puyo = "Puyo Pop",
				pendulum = "Play Pendulum",
				gaussian_blur = "Gaussian Blur",
				radial_blur = "Radial Blur",
				vconfig = "Enabled",
				video_stretch = "Stretch",
				video_loop = "Loop",
				freeze_first_frame = "Static visual",
				freeze_last_frame = "No lengthening",
				legato = "Legato",
				sheet_width = "Surface width",
				sheet_thickness = "Line thickness",
				sheet_color = "Line color",
				sheet_position = "Surface position",
				sheet_clef = "Staff clef",
				sheet_gap = "Line gap",
				sheet_g_clef = "Treble clef",
				sheet_f_clef = "Bass clef",
				sheet_notes_shift = "Notes shift",
				sheet_config = "Enabled",
				sheet_generate = "Generate staff",
				sheet_config_info = "If you want to turn on the staff visual effect, you need to enabled video option.\r\nEnabling this option will disable video visual effects and video stretching option.",
				sheet_notes_params = "音符参数",
				sheet_line_sheet_params = "谱线参数",
				sheet_legacy_engine = "旧版引擎",
				sheet_legacy_engine_tooltip = "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\n即音符和谱线的位置和形状由轨道运动定位。",
				ytp_clips_count = "Number of clips",
				ytp_min_length = "Minimum length",
				ytp_max_length = "Maximum length",
				ytp_chorus = "Chorus",
				ytp_pitch_change = "Change Pitch",
				ytp_vibrato = "Vibrato (probabily attach wave effect)",
				ytp_reverse = "Reverse",
				ytp_delay = "Delay",
				ytp_speed_change = "Change Speed",
				ytp_hue_change = "Change Hue",
				ytp_hue_rotate = "Rotate Hue",
				ytp_monochrome = "Black and White",
				ytp_negative = "Negative (probabily attach pitch-down effect)",
				ytp_high_freq_repeat = "High Frequency Repeat",
				ytp_random_tone = "Random Tuning (attach horizontal flip effect)",
				ytp_enlarge = "Enlarge (attach loud)",
				ytp_spherize = "Sphericalization",
				ytp_mirror = "Mirroring",
				ytp_high_contrast = "High Contrast (attach loud)",
				ytp_oversaturation = "Oversaturation (probabily attach pitch-up effect)",
				ytp_emphasize_thrice = "Thrice to Emphasize (attach enlarge focus effect)",
				ytp_info = "Click the \"Complete\" button under the current tab, the YTP will be generated instead of Otomad/YTPMV.\nThe parameter settings other than \"Enabled Audio\" and \"Enabled Video\" will not effective in YTP.",
				select_interval = "Select interval",
				select_interval_configform_info = "This function is designed to assist the user to select clips every one or more few, and then perform operations such as \"Paste Video Events\".",
				quick_normalize = "Quick Normalize",
				quick_normalize_configform_info = "Normalize all selected audio events volume.",
				quick_normalize_complete = "Completed normalize.",
				replace_clips_configform_info = "Replace multiple track clips with specified new track clips.",
				auto_layout_tracks_configform_info = "Automatic layout of selected tracks in YTPMV-like style.",
				change_tune_method_configform_info = "Change multiple audio events to the specified tuning algorithm.",
				batch_subtitle_generation_configform_info = "Preset the \"Titles & Text\" preset, and then add multiple lines of text here.",
				find_clips_configform_info = "Select all track events that match the specified, such as the clip name, the same source as the selected clip, etc.",
				clear_tracks_motion = "Clear Tracks Motion",
				clear_tracks_effect = "Clear Tracks Effect",
				track_legato = "Tracks Legato",
				track_legato_stacking = "Stacking clips",
				track_legato_limit_stretch = "Stretch clips (limit to stretch ranges)",
				track_legato_stretch = "Stretch clips",
				track_legato_lengthen = "Lengthen clips",
				helper_info = "The following functions are just some independent auxiliary functions, and have nothing to do with other parameters that generate audio and video.",
				helper_info_warning = "Note: This dialog box will be closed after the operation, you can reopen it later, and some unsaved changes may be lost!\n",
				close_after_open_helper = "Close this dialog after the operation completed",
				otomad_helper_config = "Otomad Helper for Vegas - Config",
				reset_config_successful = "The reset is complete, please restart the script.",
				reset_config_successful_title = "Reset User Configuration",
				sure_to_reset_config = "Are you sure you want to reset the user configuration?\n\nYour user configuration data will be lost.",
				about_title = "About",
				script_author = "Author",
				script_original_author = "Original author",
				documentation = "Documentation",
				why_ok_btn_is_disabled_info = "Please follow these steps to check the problem in turn:",
				why_ok_btn_is_disabled_no_audio_and_video_enabled = "Enabled Audio and Enabled Video are both unchecked. Please check to enable at least one of them.",
				why_ok_btn_is_disabled_no_media_take = "The selected media sources does not contain any valid media takes.",
				why_ok_btn_is_disabled_no_midi_select = "To generate Otomad/YTPMV, select a MIDI sequence file first.",
				why_ok_btn_is_disabled_in_helper_tab = "To avoid misoperation, do not submit a build under the Tools tab.",
				why_ok_btn_is_disabled_unknown_problem = "Unknown reason.",
				no_selected_media_warning = "Warning: You have not selected any valid media in the project media window!",
				no_selected_clip_warning = "Warning: You have not selected any clips in the track window!",
				preview_audio_track_name = "Preview Audio Track (Should be DELETED!)",
				no_midi_exception = "Error: No MIDI file selected.\n\nPlease reopen the script configuration dialog box, and then click the \"Browse\" button in the \"MIDI Configuration\" group to open a valid MIDI file.",
				no_media_exception = "Error: No media file selected.\n\nPlease reopen the script configuration dialog box, and then click the \"Browse\" button in the \"Media Configuration\" group to open a valid media file.",
				no_track_info_exception = "Error: There is no MIDI track.\n\nPossible reasons:\n1. You did not select a MIDI track;\n2. There is no channel in the MIDI file;\n3. The MIDI file is damaged or the file format is not supported.",
				no_plugin_pitch_shift_exception = "Error: Unable to call the Pitch Shift plugin.\n\nPlease follow the instructions of the tutorial document {0} to operate correctly.\nHowever, according to this updated version of the script, the Chinese and English versions should work normally.\nSo it is very likely that you are using Vegas in other languages.",
				no_plugin_presets_exception = "Error: Cannot call the preset effect of the Pitch Shift plugin.\n\nPlease follow the instructions of the tutorial document {0} to operate correctly.\nMake sure that all 25 presets are manually added in the transposition plugin and named correctly.\n\nSupplementary explanation: For details, please refer to the explanation of the installation method in the above link column. These 25 presets are all the types of pitch changes within the next octave.\nMissing any of them may cause errors. It is indeed very troublesome to manually add presets, but Vegas cannot use scripts to specify the specific parameters of\nthe pitch shift, so I had to go around this trick.",
				no_plugin_name_exception = "Error: The {0} plugin could not be called.\n\nMaybe you are using a non-Chinese version of Vegas or another version of Vegas that has not yet been tested.",
				no_take_exception_ps = "Supplementary note: If it still cannot be resolved, it means that the media file may be in a format not supported by Vegas.\nYou can manually drag the file into Vegas to see if the video and audio are normal.",
				no_audio_take_exception = "Error: Unable to read audio media stream.\n\nIn the setting interface, do not check \"Enabled Audio\" for pure video/picture media.\n\n",
				no_video_take_exception = "Error: Unable to read the video media stream.\n\nIn the settings user interface, do not check \"Enabled Video\" for pure audio media.\n\n",
				no_media_take_exception = "Error: Unable to read the media.\n\nThe file format you selected is not supported by Vegas. Please check if the media file is damaged or the corresponding Vegas decoder is not installed.\n\n",
				not_a_midi_file_exception = "Error: Unable to read MIDI file.\n\nSolution: Import the MIDI with the host software, and then re-output a new MIDI file.\n\nSupplementary note: There are multiple formats of MIDI files, and the script does not guarantee that all of them can be read correctly. Fortunately,\nMIDI files exported by mainstream host software under default settings are generally readable. (Currently tested FL Studio, LMMS\nand Music Studio for iPad.)",
				no_selected_exception_ps = "Additional note: If you want to manually select a media in the folder, please click the \"Browse\" button on the right to\nselect a media. And make sure that the path of the file you selected is selected in the drop-down menu on the left.",
				no_selected_media_exception = "Error: No media is selected in the project media window.\n\nPlease select a media in the project media window, then reopen the configuration dialog, and select \"selected media file\" in the source settings.\n\n",
				no_selected_clip_exception = "Error: No clips are selected in the track.\n\nPlease select a clip in the track, then reopen the configuration dialog, and select \"selected track clips\" in the source settings.\n\n",
				no_time_stretch_pitch_shift_exception = "Error: The pitch conversion method of the selected clip is set to no tuning.\n\nMost likely you are using \"selected track clips\". You are not to blame for this error, but for the brain-dead design of Vegas.\n\nSolution: Please reselect your track clips, right-click the audio part, and select \"Properties\" at the bottom. Set the \"Method\" of \"Time Stretch/Pitch Conversion\" to \"élastique\".\nThen click OK.\n\nSupplementary note: If an audio event has not been transposed and its properties are opened, then the “Method” of “Time Stretch/Pitch Conversion” in its properties will be\nautomatically modified to “None”, and click OK. Take effect. At this time, you will find that the + and-key tuning operations on the keyboard are invalid. At this time, you must reopen the properties of the audio event,\nset the \"Method\" of \"Time Stretch/Pitch Conversion\" to \"élastique\", you don't need to set \"Pitch Change\", just click OK.",
				read_config_fail_exception = "Error: Failed to read the parameter configuration file.\n\nUnfortunately you encountered this unforeseen error. We will clear the user configuration settings and restore them to default settings in order to solve the problem.\nIt is recommended to tell the author of this error in order to solve the problem quickly.\nThis script will be exited, and then I will bother you to reopen it manually.",
				fail_to_select_clips_exception = "Error: Error selecting track clips.\n\nPlease select some track clips in the track window first.",
				fail_to_select_tracks_exception = "Error: Error selecting tracks.\n\nPlease select some video tracks in the track window first.",
				ytp_over_length_exception = "Error: The specified YTP minimum length exceeds the media length.\n\nThe specified YTP minimum length is too large, please try a smaller value. Or the length of the selected media is too small.",
				ytp_in_media_generator_exception = "Error: Apply YTP to the media generated by the media generator.\n\nThe application of YTP must use local media files, do not use the media generated by the media generator.",
				ytp_eliminate_duplicates_finally_null_exception = "Technical Exception: Remove duplicate from YTP source list. Finally, the list is empty!\n\nThis is an error that should not happen.",
				unknown_exception = "Error: Unknown exception.\n\nPlease expand the details to see the specific error content and feed the error information back to the author.",
				use_pic_in_pic_on_unsupported_vegas_exception = "Error: Use of the Picture-in-Picture plug-in effect in lower versions of Vegas is not supported.\n\nRoot cause: Vegas has added some new features and parameters to the new Picture-in-Picture effect plug-in, which cannot be used in lower versions of Vegas plug-in.\n\nSolution: The current version of Vegas cannot use this PV rhythm visual effects (such as expansion aspects), please use other visual effects. Or update your Vegas software."
			};
			TChinese = new Lang {
				__name__ = "繁體中文",
				info_label_font = "Microsoft JhengHei",
				ui_font = "Microsoft JhengHei UI",
				restart_to_effect_language = "重新啟動以使語言生效？",
				yes_to_restart_to_effect_language = "立即手動重啟",
				no_to_restart_to_effect_language = "不重啟以生效",
				version_number = "版本號",
				revision_date = "最後修訂日期",
				vegas_version = "Vegas 版本",
				script_supported_vegas_version = "腳本支持版本",
				unsupported_vegas_version = "警告：當前腳本與您當前所使用的 Vegas 版本不匹配，部分功能可能不能正常工作！請重新安裝正確版本的腳本。\n腳本支持的版本為：Vegas Pro {0}\n當前軟件的版本為：Vegas Pro {1}",
				unsupported_vegas_version_title = "版本不匹配",
				unsupported_vegas_version_feature = "低於 Vegas Pro {0} 的版本不支持本功能。",
				midi_file_name = "MIDI 序列",
				all_files = "所有檔案",
				choose_a_midi_file = "請選擇一個 MIDI 檔案",
				media_file_name = "支持的媒體檔案",
				choose_a_source_file = "請選擇一個視頻或圖片素材片段",
				error = "錯誤",
				details = "詳細資訊：",
				brightness_and_contrast = "亮度和對比度",
				invert = "反轉",
				black_and_white = "黑白",
				lab_adjust = "LAB 調整",
				hsl_adjust = "HSL 調整",
				mirror = "鏡像",
				pic_in_pic = "畫中畫",
				crop = "修剪",
				check_pitch_shift_presets = "正在檢查移調插件的預設是否可用⋯⋯",
				no_pitch_shift_presets = "由於您試圖使用「移調」效果插件調音，但是系統發現您並沒有完全配置好所需的所有音效預設。您可嘗試由腳本嘗試為您自動添加預設，可能會添加失敗。如果失敗，則請按照使用教程的說明來手動操作。是否由腳本為您自動添加預設？",
				yes_to_add_pitch_shift_presets = "自動添加",
				no_to_add_pitch_shift_presets = "返回配置",
				no_pitch_shift_presets_title = "未找到所有的移調音效預設",
				add_pitch_shift_presets_successful = "添加預設完成！",
				add_pitch_shift_presets_fail = "添加預設失敗！",
				add_pitch_shift_presets_fail_title = "很遺憾",
				ensure_load_presets = "確定加載預設？",
				ensure_unload_presets = "確定卸載預設？",
				reverse_suffix_tag = "（反轉）",
				effect_init_forward = "正",
				effect_init_reversed = "反",
				effect_init_turned = "倒",
				effect_init_left = "左",
				effect_init_right = "右",
				effect_init_up = "上",
				effect_init_down = "下",
				effect_init_left_up = "左上",
				effect_init_right_up = "右上",
				effect_init_left_down = "左下",
				effect_init_right_down = "右下",
				effect_init_invert = "負",
				effect_init_conform = "符",
				effect_init_opposite = "對",
				effect_init_chromatic = "彩",
				effect_init_monochrome = "灰",
				effect_init_counter = "逆",
				effect_init_stepon = "繼",
				effect_init_flat = "扁",
				effect_init_thin = "細",
				effect_init_blur = "糊",
				enable_all_effects = "開啟所有效果",
				chorus = "合唱",
				vibrato = "顫音",
				wave = "波浪",
				multi_beat_delay = "多拍延遲",
				spherize = "球面化",
				warning_missing_plugin = "警告：找不到插件「{0}」！",
				midi_channel = "通道",
				midi_notes_count = "音符數",
				midi_begin_note = "起音",
				instrument = "樂器",
				error_code = "錯誤代碼：",
				processing_otomad = "正在生成音 MAD / YTPMV⋯⋯",
				processing_ytp = "正在生成 YTP⋯⋯",
				processing_it = "正在處理它",
				real_time_update = "即時更新當前進度（會减慢生成速度）",
				replacer_is = "指定的替換項為",
				replacer_info = "請先在軌道視窗中選中替換與被替換的素材，然後指定一個素材為替換的素材，剩餘素材均為被替換素材。\n請先將替換素材的音視頻創建分組，並確保替換素材放置在時間靠後的位置並且儘量不與其它被替換素材位於同一軌道。",
				replace_clips = "替換軌道素材",
				replaced_info = "則剩餘 {0} 項軌道剪輯將被替換為選定素材。",
				replaced_label = "被替換項",
				replacer_label = "替換項",
				separation_replaced_info = "已選中 {0} 個軌道素材，其中 {1} 個音訊剪輯，{2} 個視訊剪輯。",
				separation_set_replaced = "將選中的 {0} 個素材設為被替換項",
				separation_set_replacer = "將選中的 {0} 個素材設為替換項",
				separation_back_to_select = "返回 Vegas 選定素材",
				separation_use_clip_group = "同時替換分組內其它剪輯",
				separation_reserve_original_name = "保留原始剪輯名稱",
				replacements_classic_specify = "同時指定",
				replacements_separation_specify = "分別指定",
				view = "查看",
				selected_items = "選中項",
				replace_clips_form_ensure_to_close = "在「分別指定」中已指定有部分軌道素材。\n退出之後系統可能不會保留您的更改。",
				track = "軌道",
				submit_select = "設定選中(&S)",
				every_few = "每幾個選擇一個",
				which_one = "選擇每組第幾個",
				select_how_many = "每次要選取幾個",
				reset_select = "重置選擇(&R)",
				quick_select_interval = "快速間隔選擇",
				select_interval_info = "請先在 Vegas 軌道中選中一些素材，然後再啟動本對話方塊，使用下麵的功能。",
				select_events_count_info = "已選中 {0} 個軌道剪輯。",
				select_videotracks_count_info = "已選中 {0} 個視頻軌道。",
				select_audioevents_count_info = "已选中 {0} 個音訊軌道剪輯。",
				select_source_count_info = "已選中 {0} 項媒體素材。",
				square = "平方",
				custom = "自定義",
				row_count = "行數",
				column_count = "列數",
				fill = "填充",
				adapt = "適應",
				increase_padding = "增加邊距",
				auto_layout_tracks = "自動佈局軌道",
				grid_layout = "網格佈局",
				box_3d_layout = "3D 方盒佈局",
				selected_tracks_too_much = "您所選取的軌道數目過多，有 {0} 個，超出使用功能範圍。\n請選取少量軌道後重試。",
				selected_tracks_too_much_title = "選取軌道數目過多",
				bottom_surface = "底面",
				top_surface = "頂面",
				right_surface = "右面",
				left_surface = "左面",
				back_surface = "後面",
				front_surface = "前面",
				box_3d_layout_info = "由於腳本功能限制，將會新建軌道並將選定軌道中的剪輯移動過去，原軌道中的軌道運動、效果等訊息將會遺失。\n請在下方選定立方體的各個面所使用的軌道，如果為空則表示不設定該面。",
				delete_original_tracks = "删除原軌道",
				use_video_longer_side = "使用視頻的長邊作為立方體的棱長",
				use_video_longer_side_tooltip = "勾選後，將以項目設定中視頻最長的一條邊（即寬度與高度的最大值）作為立方體的棱長。",
				gradient_tracks = "漸變軌道",
				gradient_tracks_info = "選擇一種漸變效果應用到所選的視頻軌道：",
				rainbow_color = "彩虹色",
				gradually_saturated = "逐漸飽和",
				gradually_contrasted = "逐漸對比",
				threshold = "閾值",
				alternately_chromatic = "彩灰交替",
				alternately_negative = "正負交替",
				descending = "降序排序",
				ascending = "昇序排序",
				change_tune_method = "更改調音算灋",
				change_tune_method_info = "僅支持音訊事件内容中的調音方法，不支持「移調」插件中的調音方法。",
				time_stretch_pitch_shift = "時間拉伸/音調轉換",
				formant_change = "共振峰移位",
				pitch_change = "音調更改",
				method = "方法",
				pitch_lock = "鎖定以拉伸",
				lock_pitch_instead_of_rate = "鎖定音高而不是速度",
				none = "無",
				classic = "古典",
				elastique_pro = "專業",
				elastique_efficient = "高效",
				elastique_soloist_monophonic = "獨奏（單聲道）",
				elastique_soloist_speech = "獨奏（語音）",
				classic_a01 = "A01. 音樂 1（最小變調，可能有回音）",
				classic_a02 = "A02. 音樂 2",
				classic_a03 = "A03. 音樂 3（回音較小）",
				classic_a04 = "A04. 音樂 4（快速，適合低音）",
				classic_a05 = "A05. 音樂 5",
				classic_a06 = "A06. 音樂 6",
				classic_a07 = "A07. 語音 1",
				classic_a08 = "A08. 語音 2",
				classic_a09 = "A09. 語音 3（快速）",
				classic_a10 = "A10. 獨奏樂器 1",
				classic_a11 = "A11. 獨奏樂器 2",
				classic_a12 = "A12. 獨奏樂器 3",
				classic_a13 = "A13. 獨奏樂器 4（回音較小）",
				classic_a14 = "A14. 獨奏樂器 5",
				classic_a15 = "A15. 獨奏樂器 6",
				classic_a16 = "A16. 獨奏樂器 7（快速）",
				classic_a17 = "A17. 鼓，無音高（最小回音）",
				classic_a18 = "A18. 鼓（更適用於通鼓）",
				classic_a19 = "A19. 鼓（微弱回音）",
				batch_subtitle_generation = "批量生成字幕",
				batch_subtitle_generation_presets = "選擇一個預先設定好的“字幕和文字”媒體發生器的預設：",
				batch_subtitle_generation_subtitles = "輸入要插入的字幕文字（一行一個，忽略空行）：",
				batch_subtitle_generation_single_duration = "每個字幕持續時間",
				batch_subtitle_generation_suggestion_info = "稍後可開啟“自動跟進”功能以便後續調整時間。",
				find_clips = "查找軌道素材",
				find_clips_select_info = "選中的第一個軌道剪輯",
				find_clips_info = "在上方選中相匹配的剪輯，確定之後將會選中這些剪輯。",
				find_clips_header_name = "名稱",
				find_clips_header_num = "數目",
				find_clips_match_source = "與選中軌道剪輯相同的所有剪輯",
				find_clips_match_source_and_offset = "與選中軌道剪輯相同且開始偏移量相等的所有剪輯",
				find_clips_match_name = "與指定名稱相匹配的剪輯",
				replace = "替換(&R)",
				apply = "應用(&A)",
				close = "關閉(&C)",
				complete = "完成(&I)",
				cancel = "取消(&C)",
				about = "關於(&A)",
				ok = "確定(&O)",
				balloon_title = "填寫說明",
				midi_start_second_tooltip = "用於截取 MIDI 音樂的一部分。\n單位：秒。",
				midi_end_second_tooltip = "此處填寫需要讀取MIDI檔案的時間長度。\n注意如果填寫的值過小，將截去多餘時間部分的音符。\n如果此處填寫的值比起始秒數小或相等，則始終表示持續到整個音樂時長末尾。\n單位：秒。",
				midi_beat_conbo_tooltip = "現時僅用於五線譜的分頁功能。\n暫時無法通過 MIDI 檔案自動推測。",
				source_start_time_tooltip = "此處填寫媒體素材裁剪的開始時間。\n單位：秒。",
				source_end_time_tooltip = "注意如果此處填寫的數值比入點秒數小或相等，則始終表示持續到素材時間末尾。\n單位：秒。",
				no_tune = "不調音",
				pitch_shift_plugin = "移調效果插件",
				elastique_method = "彈性音調更改",
				classic_method = "古典音調更改",
				fool_tuning_method = "瞎調音",
				sine_wave = "正弦波",
				triangle_wave = "三角波",
				square_wave = "方波",
				sawtooth_wave = "鋸齒波",
				tune_method_tooltip = "「移調效果插件」表示使用「音訊FX」中的「移調」效果插件改變音調，需要配置預設。\n「彈性音調更改」表示使用“Élastique”拉伸管道改變音調，也就是鍵盤上 +、- 鍵直接改變音調，\n有音高範圍限制。",
				audio_lock_stretch_pitch_tooltip = "採用重採樣管道，隨著速度變化而改變音高。如果使用的是「彈性音調\n更改」方法，那麼將會禁用拉伸音訊功能。",
				preview_beep_duration_tooltip = "預聽標準音高所持續的時間。\n單位：毫秒。",
				preview_tune_audio_tooltip = "勾選後，預聽音訊時會將音訊素材調整到主音高中央 C。\n否則，預聽標準音高將會播放原始音高處所設定的音高。",
				sheet_position_tooltip = "五線譜中間第三根線到荧幕中心的距離，上正下負。\n單位：點數。",
				sheet_width_tooltip = "將在荧幕中間所填的寬度內顯示音符，用於左右留白，給左側的譜號留間距。\n單位：點數。",
				sheet_gap_tooltip = "五線譜線與線之間的間距。\n單位：點數。",
				sheet_line_thickness_tooltip = "生成五線譜的譜線時，五線譜譜線的粗細。 當粗細值達到 100 % 後，譜線將會占滿整個譜線間距。\n單位：百分比。",
				sheet_relative_tooltip = "勾選後，下方所填參數的點數單位將以相對於 1920 × 1080\n的尺寸進行定位；反之則以項目尺寸定位。",
				sheet_relative = "使用相對值",
				preview_base_pitch_tooltip = "請確保開啟聲音並且未將聲音方案設定為無聲。\n如果仍沒有聲音，請重啟系統。",
				ytp_max_length_tooltip = "指定單個軌道剪輯的最大長度。\n單位：毫秒。",
				ytp_min_length_tooltip = "指定單個軌道剪輯的最小長度。\n單位：毫秒。",
				file = "檔案(&F)",
				save_config = "保存用戶配置(&S)",
				reset_config = "重置用戶配置(&R)",
				exit_discarding_changes = "放弃更改並退出(&D)",
				exit = "退出(&X)",
				pitch_shift_preset = "移調插件預設(&P)",
				load_presets = "加載預設",
				unload_presets = "卸載預設",
				form_size = "表單大小",
				remember_form_size = "記住表單大小",
				remember_always_form_size = "每次都記住表單大小",
				remember_once_form_size = "記住一次表單大小",
				restore_default_form_size = "下次恢復默認表單大小",
				help = "幫助(&H)",
				user_help = "使用說明",
				trouble_shooting = "疑難排解",
				update_info = "更新說明",
				repository_link = "倉庫地址",
				latest_version_link = "最新版本連結",
				roadmap = "路线图",
				update_logs = "更新日志",
				tutorial_video = "教程视频",
				release_notes = "发行说明",
				staff_visualizer_documentation = "五线谱可视化文档",
				english_documentations = "英语说明文档",
				chinese_documentations = "中文说明文档",
				check_update = "檢查更新(&U)",
				why_ok_btn_is_disabled = "為什麼無法點擊完成按鈕？",
				media = "媒體",
				audio = "音訊",
				video = "視頻",
				staff = "五線譜",
				ytp = "YTP",
				helper = "工具",
				midi_settings = "MIDI 配置",
				midi_start_time = "起始秒數",
				midi_end_time = "終止秒數",
				bpm_setting = "設定 BPM 速度為",
				midi_beat = "節拍",
				midi_channel_setting = "使用 MIDI 軌道",
				browse = "瀏覽...",
				no_midi_selected = "<未選擇 MIDI 檔案>",
				choose_midi_file = "選擇 MIDI 檔案",
				midi_dynamic_midi_bpm = "動態 MIDI 速度",
				midi_midi_bpm = "MIDI 速度",
				midi_project_bpm = "項目速度",
				midi_custom_bpm = "自定義",
				dynamic_midi_bpm_info = "{0} 起始的動態速度",
				dynamic_midi_beat_info = "{0} 起始的動態節拍",
				colon = "：",
				semicolon = "；",
				source_settings = "素材配置",
				generate_at_begin = "項目開始處",
				generate_at_cursor = "光標處",
				generate_position = "設定生成開始位置",
				choose_source_file = "選擇媒體素材",
				selected_media = "選中的媒體檔案",
				selected_clip = "選中的軌道素材",
				source_start_time = "入點秒數",
				source_end_time = "出點秒數",
				parameters = "參數",
				linear = "線性",
				fast = "快速",
				slow = "慢速",
				smooth = "平滑",
				sharp = "急劇",
				hold = "保留",
				fade_in = "漸入　　",
				fade_out = "漸出　　",
				tune = "調音",
				tune_method = "調音管道",
				base_pitch = "原始音高",
				preview_listen = "預聽",
				preview_base_pitch = "預聽標準音高(&B)",
				preview_audio = "預聽音訊(&P)",
				stop_preview_audio = "停止預聽(&P)",
				lock_attr = "鎖定內容",
				preview_listen_attr = "預聽内容",
				preview_tune_audio = "使音訊調整到主音高",
				reserve_formant = "保留共振峰",
				stretch_attr = "拉伸內容",
				create_event_group = "創建分組",
				aconfig = "生成音訊",
				audio_stretch = "拉伸音訊",
				audio_loop = "迴圈音訊",
				audio_normalize = "規範化音量",
				audio_lock_stretch_pitch = "鎖定伸縮與音調",
				video_glow_bright = "發光亮度",
				video_glow = "發光",
				video_start_size = "起始尺寸",
				video_end_size = "終止尺寸",
				video_start_rotation = "起始旋轉",
				video_end_rotation = "終止旋轉",
				video_start_h_trans = "起始平移",
				video_end_h_trans = "終止平移",
				video_start_v_trans = "起始直移",
				video_end_v_trans = "終止直移",
				video_start_hue = "起始色相",
				video_end_hue = "终止色相",
				video_start_saturation = "起始饱和",
				video_end_saturation = "终止饱和",
				video_start_contrast = "起始对比",
				video_end_contrast = "终止对比",
				video_start_threshold = "起始阈值",
				video_end_threshold = "终止阈值",
				video_multitrack_for_chords = "复音多轨",
				effect = "效果",
				visual_effect = "視覺效果",
				initial_visual_effect = "初始值",
				no_effects = "無效果",
				h_flip = "水平翻轉",
				v_flip = "垂直翻轉",
				ccw_flip = "逆時針翻轉",
				cw_flip = "順時針翻轉",
				ccw_rotate = "逆時針旋轉",
				cw_rotate = "順時針旋轉",
				turned = "顛倒",
				h_mirror = "水平鏡像",
				v_mirror = "垂直鏡像",
				ccw_mirror = "逆時針鏡像",
				cw_mirror = "順時針鏡像",
				negative = "顏色反轉",
				lumin_invert = "亮度反轉",
				hue_invert = "色相反轉",
				step_change_hue = "{0} 步色差",
				chromatic_and_monochrome = "彩灰",
				pingpong = "桌球效應",
				whirl = "愛的魔力轉圈圈",
				vertical_expansion = "垂直擴張",
				vertical_expansion_with_rebound = "垂直擴張並回彈",
				vertical_compression = "垂直壓縮",
				vertical_compression_with_rebound = "垂直壓縮並回彈",
				oblique_expansion_and_compression = "斜向擴縮",
				puyo_puyo = "魔法氣泡",
				pendulum = "左右搖擺",
				gaussian_blur = "高斯模糊",
				radial_blur = "徑向模糊",
				vconfig = "生成視頻",
				video_stretch = "拉伸視頻",
				video_loop = "迴圈視頻",
				freeze_first_frame = "靜態畫面",
				freeze_last_frame = "禁止延長",
				legato = "填補間隙",
				sheet_width = "譜面寬度",
				sheet_thickness = "譜線粗細",
				sheet_color = "譜線顏色",
				sheet_position = "譜面位置",
				sheet_clef = "譜號",
				sheet_gap = "譜線間距",
				sheet_g_clef = "高音",
				sheet_f_clef = "低音",
				sheet_notes_shift = "音符偏移",
				sheet_config = "啟用五線譜視覺化效果",
				sheet_generate = "生成五線譜",
				sheet_config_info = "欲開啟五線譜視覺效果，需要先開啟「生成視頻」選項。\n開啟本功能會禁用視頻視覺效果和視頻拉伸選項。",
				sheet_notes_params = "音符参数",
				sheet_line_sheet_params = "谱线参数",
				sheet_legacy_engine = "旧版引擎",
				sheet_legacy_engine_tooltip = "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\n即音符和谱线的位置和形状由轨道运动定位。",
				ytp_clips_count = "剪輯數目",
				ytp_min_length = "最小長度",
				ytp_max_length = "最大長度",
				ytp_chorus = "合唱",
				ytp_pitch_change = "更改音調",
				ytp_vibrato = "顫音（概率性附加波浪效果）",
				ytp_reverse = "倒放",
				ytp_delay = "延遲",
				ytp_speed_change = "更改速度",
				ytp_hue_change = "更改色相",
				ytp_hue_rotate = "旋轉色相",
				ytp_monochrome = "黑白",
				ytp_negative = "反轉顏色（概率性附加降調效果）",
				ytp_high_freq_repeat = "高頻重複",
				ytp_random_tone = "隨機調音（附加水平翻轉效果）",
				ytp_enlarge = "放大（附加增大音量）",
				ytp_spherize = "球面化",
				ytp_mirror = "鏡像",
				ytp_high_contrast = "高對比（附加增大音量）",
				ytp_oversaturation = "過飽和（概率性附加升調效果）",
				ytp_emphasize_thrice = "重說三（附加放大聚焦效果）",
				ytp_info = "在當前選項卡下按一下「完成」按鈕，將會生成 YTP 而不是音 MAD / YTPMV。\n除「生成音訊」「生成視頻」外其它的參數設置並不會在 YTP 中使用。",
				select_interval = "間隔選擇",
				select_interval_configform_info = "本功能旨在輔助用戶每隔一個或幾個選中一個素材，然後可以執行「粘貼視頻事件」等操作。",
				quick_normalize = "快速規範音量",
				quick_normalize_configform_info = "將選中的多個音訊軌道剪輯全部規範化音量。",
				quick_normalize_complete = "完成規範化音量。",
				replace_clips_configform_info = "將多個軌道剪輯替換為指定的新軌道剪輯。",
				auto_layout_tracks_configform_info = "類 YTPMV 風格自動佈局選中的軌道。",
				change_tune_method_configform_info = "將多個音訊軌道剪輯統一更改為指定的調音算灋。",
				batch_subtitle_generation_configform_info = "預先設定好“字幕和文字”的預設，然後在此添加多行文字。",
				find_clips_configform_info = "根據指定的條件（如剪輯名稱、與選中剪輯相同的素材等）選中符合條件的所有軌道剪輯。",
				clear_tracks_motion = "清除軌道運動",
				clear_tracks_effect = "清除軌道效果",
				track_legato = "填補軌道間隙",
				track_legato_stacking = "堆積素材",
				track_legato_limit_stretch = "拉伸素材（限制在拉伸極限範圍之內）",
				track_legato_stretch = "拉伸素材",
				track_legato_lengthen = "改變素材持續時間",
				helper_info = "以下功能只是一些獨立的協助工具，與其它生成音視頻的參數無關。",
				helper_info_warning = "注意：操作之後將會關閉本對話方塊，您可以稍後再重新啟動，部分您未保存的更改可能會遺失！\n",
				close_after_open_helper = "操作完成之後關閉本對話方塊",
				otomad_helper_config = "Otomad Helper for Vegas - 配置",
				reset_config_successful = "重置完成，請重新啟動腳本。",
				reset_config_successful_title = "重置用戶配置",
				sure_to_reset_config = "確定要重置用戶配置嗎？\n\n重置後您的用戶配置數據將會遺失。",
				about_title = "關於",
				script_author = "腳本作者",
				script_original_author = "腳本原作者",
				documentation = "說明檔案",
				why_ok_btn_is_disabled_info = "請按照下列步驟依次檢查問題：",
				why_ok_btn_is_disabled_no_audio_and_video_enabled = "「生成音訊」與「生成視頻」被同時取消勾選。請至少勾選生成其中一項。",
				why_ok_btn_is_disabled_no_media_take = "所選的媒體素材來源不包含任何有效媒體資源。",
				why_ok_btn_is_disabled_no_midi_select = "若要生成音 MAD / YTPMV，請先選擇一個 MIDI 序列檔案。",
				why_ok_btn_is_disabled_in_helper_tab = "為避免誤操作，切勿在「工具」選項卡下進行提交生成操作。",
				why_ok_btn_is_disabled_unknown_problem = "未知原因。",
				no_selected_media_warning = "警告：您沒有在項目媒體視窗中選中任何有效媒體素材！",
				no_selected_clip_warning = "警告：您沒有在軌道視窗中選中任何剪輯片段！",
				preview_audio_track_name = "預聽音訊軌道（應該被删除！）",
				no_midi_exception = "錯誤：未選擇 MIDI 檔案。\n\n請重新啟動腳本參數配置對話方塊，然後在「MIDI 配置」分組中點擊「瀏覽」按鈕，打開一個有效的MIDI 檔案。",
				no_media_exception = "錯誤：未選擇媒體檔案。\n\n請重新啟動腳本參數配置對話方塊，然後在「媒體配置」分組中點擊「瀏覽」按鈕，打開一個有效的媒體檔案。",
				no_track_info_exception = "錯誤：沒有 MIDI 音軌。\n\n可能的原因：\n1.您沒有選擇一個 MIDI 音軌；\n2.該 MIDI 檔案中沒有任何音軌；\n3.該 MIDI 檔案已損壞或檔案格式不受支持。",
				no_plugin_pitch_shift_exception = "錯誤：無法調用移調插件。\n\n請按照教程檔案 {0} 的指引正確操作。\n不過，根據這個更新版本的腳本，按理應當是中英文版本均可正常運行的。\n囙此很有可能您是使用其它語言的 Vegas 造成的（逃",
				no_plugin_presets_exception = "錯誤：無法調用移調插件的預設效果。\n\n請按照教程檔案 {0} 的指引正確操作。\n確保在移調插件中手動添加了所有的 25 個預設，且命名正確。\n\n補充說明：具體可見上述連結專欄中對於安裝方法的說明。這 25 個預設是上下一個八度以內的所有變調種類，\n缺少任何一個都有可能出錯。手動添加預設的確非常麻煩，但 Vegas 無法使用腳本來指定變調的具體參數，\n囙此只好繞這個彎子。",
				no_plugin_name_exception = "錯誤：無法調用 {0} 插件。\n\n可能您使用的是非中文版的 Vegas 或其它尚未測試版本的 Vegas。",
				no_take_exception_ps = "補充說明：若仍不能解决，說明該素材檔案可能是 Vegas 不支持的格式，\n可以手動把該檔案拖入 Vegas 中看一下是否視頻音訊都正常。",
				no_audio_take_exception = "錯誤：無法讀取音訊媒體流。\n\n在設定介面，純視頻/圖片素材不要勾選「生成音訊」。\n\n",
				no_video_take_exception = "錯誤：無法讀取視頻媒體流。\n\n在設定介面，純音訊素材不要勾選「生成視頻」。\n\n",
				no_media_take_exception = "錯誤：無法讀取媒體。\n\n您所選的檔案格式不受 Vegas 支持，請檢查該媒體檔案是否損壞，或未安裝對應的 Vegas 解碼器。\n\n",
				not_a_midi_file_exception = "錯誤：無法讀取 MIDI 檔案。\n\n解決方法：用宿主軟件導入該 MIDI，然後重新輸出一個新的 MIDI 檔案。\n\n補充說明：MIDI 檔案有多種格式，腳本不保證都能够正確讀取。所幸主流宿主軟件在\n默認設置下匯出的 MIDI 檔案一般是可以讀取的。（現時測試過 FL Studio、LMMS\n與 Music Studio for iPad。）",
				no_selected_exception_ps = "補充說明：如果您想手動在資料夾中選擇一個媒體素材，那麼請點擊其右邊的「瀏覽」按鈕，\n選擇一個媒體素材。並確保左側的下拉式功能表中選中的是您所選檔案所在的路徑。",
				no_selected_media_exception = "錯誤：沒有在項目媒體視窗中選擇任何媒體。\n\n請在項目媒體視窗中選擇一個媒體，然後重新啟動參數配置視窗，並在素材設定中選擇「選中的媒體檔案」。\n\n",
				no_selected_clip_exception = "錯誤：沒有在軌道中選擇任何剪輯。\n\n請在軌道中選擇一個剪輯，然後重新啟動參數配置視窗，並在素材設定中選擇「選中的軌道素材」。\n\n",
				no_time_stretch_pitch_shift_exception = "錯誤：選定素材音調轉換方法被設定為不調音。\n\n很有可能您使用的是「選中的軌道素材」。出現了這個錯誤不怪你，要怪就怪 Vegas 這個腦殘設計。\n\n解決方法：請重新選中您的軌道素材，右鍵音訊部分，選擇底部的「內容」。將「時間拉伸/音調轉換」的「方法」設定為“élastique”。\n然後點擊確定即可。\n\n補充說明：如果某個音訊事件沒有進行變調操作，然後打開了它的內容，那麼其內容中的「時間拉伸/音調轉換」的「方法」會被\n自動修改為「無」，點擊確定就會生效。這時你會發現鍵盤上的 +、- 鍵調音操作無效了。這時必須重新打開音訊事件的內容，\n將「時間拉伸/音調轉換」的「方法」設定為“élastique”，不必設定「音調更改」，點擊確定即可。",
				read_config_fail_exception = "錯誤：讀取參數設定檔失敗。\n\n很遺憾您遇到了這個不可預見的錯誤。我們將會清除用戶配置設定並恢復為預設值以便解决問題。\n建議將這個錯誤告訴作者以便快速解决問題。\n將會退出此腳本，然後勞煩閣下手動重新啟動此腳本。",
				fail_to_select_clips_exception = "錯誤：選取軌道剪輯出錯。\n\n請先在軌道視窗中選取部分軌道剪輯。",
				fail_to_select_tracks_exception = "錯誤：選取軌道出錯。\n\n請先在軌道視窗中選取部分視頻軌道。",
				ytp_over_length_exception = "錯誤：指定的 YTP 最小長度超過了媒體長度。\n\n指定的 YTP 最小長度過大，請嘗試更小的值。或所選媒體素材長度過小。",
				ytp_in_media_generator_exception = "錯誤：對媒體生成器產生的媒體應用 YTP。\n\n應用 YTP 必須使用本地媒體檔案，不要使用媒體生成器生成的媒體。",
				ytp_eliminate_duplicates_finally_null_exception = "技術异常：對 YTP 素材清單進行去重操作，最後清單為空了！\n\n這是一個不應該被發生的錯誤。",
				unknown_exception = "錯誤：未知异常。\n\n請展開詳細資訊查看具體錯誤內容，並將錯誤資訊回饋給作者。",
				use_pic_in_pic_on_unsupported_vegas_exception = "錯誤：不支持在低版本 Vegas 中使用該畫中畫插件的效果。\n\n根本原因：Vegas 在新版本“畫中畫”效果插件中新增了一些新的功能和參數，這些新參數不能在低版本的 Vegas 插件中使用。\n\n解決方法：在當前 Vegas 版本不能使用該 PV 節奏視覺效果（如擴縮類等），請使用其它視覺效果。或更新您的 Vegas 軟件。"
			};
			Japanese = new Lang {
				__name__ = "日本語",
				info_label_font = "Yu Gothic",
				ui_font = "Yu Gothic UI",
				restart_to_effect_language = "言語を有効にするために再起動しますか？",
				yes_to_restart_to_effect_language = "今すぐ手動で再起動",
				no_to_restart_to_effect_language = "有効にするために再起動しません",
				version_number = "バージョン番号",
				revision_date = "最終改訂日",
				vegas_version = "Vegasバージョン",
				script_supported_vegas_version = "スクリプトでサポートされているバージョン",
				unsupported_vegas_version = "警告：現在のスクリプトは現在使用しているVegasバージョンと一緻せず、一部の機能が正常に動作しない可能性があります！正しいバージョンのスクリプトを再インストールしてください。\nサポートされているバージョン：Vegas Pro {0}\n現在のバージョン：Vegas Pro {1}",
				unsupported_vegas_version_title = "バージョンが一緻しません",
				unsupported_vegas_version_feature = "Vegas Pro {0} 以下のバージョンは、本機能をサポートしていません。",
				midi_file_name = "MIDIシーケンス",
				all_files = "すべてのファイル",
				choose_a_midi_file = "MIDIファイルを選択してください",
				media_file_name = "サポートされているメディアファイル",
				choose_a_source_file = "ビデオまたは画像クリップを選択してください",
				error = "エラー",
				details = "詳細：",
				brightness_and_contrast = "明るさとコントラスト",
				invert = "反転",
				black_and_white = "黒と白",
				lab_adjust = "LAB調整",
				hsl_adjust = "HSL調整",
				mirror = "鏡像",
				pic_in_pic = "ピクチャーインピクチャー",
				crop = "トリミング",
				check_pitch_shift_presets = "PitchShiftプラグインのプリセットが使用可能かどうかを確認しています...",
				no_pitch_shift_presets = "「ピッチシフト」エフェクトプラグインを使用してチューニングしようとしたため、システムは、必要なすべてのオーディオプリセットを完全に構成していないことを検出しました。スクリプトはプリセットの追加を試みることができますが、失敗する可能性があります。それが失敗した場合は、チュートリアルを手動で使用するための指示に従ってください。スクリプトでプリセットを自動的に追加しますか？",
				yes_to_add_pitch_shift_presets = "自動的に追加",
				no_to_add_pitch_shift_presets = "構成に戻る",
				no_pitch_shift_presets_title = "すべてのピッチシフトプリセットが見つからない",
				add_pitch_shift_presets_successful = "プリセットの追加を完了してください！",
				add_pitch_shift_presets_fail = "プリセットの追加に失敗しました！",
				add_pitch_shift_presets_fail_title = "不幸にも",
				ensure_load_presets = "プリセットをロードしてもよろしいですか？",
				ensure_unload_presets = "プリセットをアンロードしてもよろしいですか？",
				reverse_suffix_tag = "（反転）",
				effect_init_forward = "正",
				effect_init_reversed = "反",
				effect_init_turned = "逆",
				effect_init_left = "左",
				effect_init_right = "右",
				effect_init_up = "上",
				effect_init_down = "下",
				effect_init_left_up = "左上",
				effect_init_right_up = "右上",
				effect_init_left_down = "左下",
				effect_init_right_down = "右下",
				effect_init_invert = "負",
				effect_init_conform = "適",
				effect_init_opposite = "対",
				effect_init_chromatic = "色",
				effect_init_monochrome = "灰",
				effect_init_counter = "逆",
				effect_init_stepon = "続",
				effect_init_flat = "幅",
				effect_init_thin = "細",
				effect_init_blur = "ぼかし",
				enable_all_effects = "すべての効果をオンにします",
				chorus = "コーラス",
				vibrato = "ビブラート",
				wave = "波形",
				multi_beat_delay = "マルチビート遅延",
				spherize = "球形化",
				warning_missing_plugin = "警告：プラグイン「{0}」が見つかりませんでした！",
				midi_channel = "チャネル",
				midi_notes_count = "ノートカウント",
				midi_begin_note = "開始メモ",
				instrument = "樂器",
				error_code = "エラーコード：",
				processing_otomad = "音MAD/YTPMVを生成中...",
				processing_ytp = "YTPを生成中...",
				processing_it = "それを処理する",
				real_time_update = "現在の進捗状況をリアルタイムで更新（生成速度が低下）",
				replacer_is = "指定された代替品は",
				replacer_info = "最初にトラックウィンドウで置換および置換するクリップを選択してから、置換クリップとしてクリップを指定してください。残りのクリップはすべて置換されたクリップです。\n最初に交換用クリップのオーディオとビデオのグループを作成し、交換用クリップが他の交換済みクリップと同じトラック上に配置されていないことを確認してください。",
				replace_clips = "トラックイベントを置き換える",
				replaced_info = "次に、残りの{0}トラッククリップが選択したクリップに置き換えられます。",
				replaced_label = "置換されたみ",
				replacer_label = "置換項",
				separation_replaced_info = "{0}トラッククリップが選択されました。そのうち{1}オーディオクリップ、{2}ビデオクリップ。",
				separation_set_replaced = "選択した{0}アセットを置換されたみ",
				separation_set_replacer = "選択した{0}アセットを置換项",
				separation_back_to_select = "Vegasに戻る選択した素材",
				separation_use_clip_group = "グループ内の他のクリップも交換してください",
				separation_reserve_original_name = "元のクリップ名を保持",
				replacements_classic_specify = "同時指定",
				replacements_separation_specify = "個別指定",
				view = "表示",
				selected_items = "選択項",
				replace_clips_form_ensure_to_close = "「個別指定」でレール素材の一部が指定されています。\n終了しても変更は保持されない場合があります。",
				track = "追跡",
				submit_select = "選択した値を送信(&S)",
				every_few = "数個ごとに1つ選択してください",
				which_one = "グループのどれを選択します",
				select_how_many = "一度にいくつ選択する",
				reset_select = "リセット選択(&R)",
				quick_select_interval = "クイックセレクト間隔",
				select_interval_info = "最初にVegasトラックウィンドウでいくつかのクリップを選択してから、このダイアログボックスを開いて次の機能を使用してください。",
				select_events_count_info = "{0}トラックイベントが選択されました。",
				select_videotracks_count_info = "{0}ビデオトラックが選択されました。",
				select_audioevents_count_info = "{0}オーディオトラックイベントが選択されました。",
				select_source_count_info = "{0}メディア素材が選択されました。",
				square = "四角",
				custom = "カスタマイズ",
				row_count = "行の数",
				column_count = "列の数",
				fill = "充填",
				adapt = "適応",
				increase_padding = "パディングを増やす",
				auto_layout_tracks = "自動レイアウトトラック",
				grid_layout = "グリッドレイアウト",
				box_3d_layout = "3Dボックスレイアウト",
				selected_tracks_too_much = "利用可能な機能の範囲を超えている{0}トラックを選択しました。多すぎる！\n選択するトラックの数を減らして、もう一度やり直してください。",
				selected_tracks_too_much_title = "選択したトラックが多すぎます",
				bottom_surface = "下の方",
				top_surface = "上の方",
				right_surface = "右の方",
				left_surface = "左の方",
				back_surface = "後の方",
				front_surface = "前の方",
				box_3d_layout_info = "スクリプト機能の制限により、新しいトラックが作成され、選択したトラックのクリップが移動し、元のトラックのトラックモーション、エフェクトなどが失われます。\n以下の立方体の各面で使用されるトラックを選択してください。空の場合は、顔が設定されていないことを意味します。",
				delete_original_tracks = "元のトラックを削除する",
				use_video_longer_side = "立方体のエッジの長さとして長い方の辺を使用します",
				use_video_longer_side_tooltip = "チェック後、プロジェクト設定のビデオの最も長い辺（つまり、幅と高さの最大値）が立方体のエッジの長さとして使用されます。",
				gradient_tracks = "グラデーショントラック",
				gradient_tracks_info = "選択したビデオトラックに適用するグラデーション効果を選択します。",
				rainbow_color = "レインボーカラー",
				gradually_saturated = "徐々に飽和",
				gradually_contrasted = "徐々に対照的",
				threshold = "しきい値",
				alternately_chromatic = "交互にクロマチック",
				alternately_negative = "代替ネガティブ",
				descending = "降順",
				ascending = "昇順",
				change_tune_method = "トーンアルゴリズムの変更",
				change_tune_method_info = "オーディオイベントのプロパティでのみチューニングメソッドは、ピッチシフトプラグインではサポートされていません。",
				time_stretch_pitch_shift = "時間引張り／トーン変換",
				formant_change = "ホルマントシフト",
				pitch_change = "トーン変更",
				method = "方法",
				pitch_lock = "ロックを伸ばします",
				lock_pitch_instead_of_rate = "速度ではなくピッチをロックする",
				none = "無",
				elastique = "エラスティック",
				classic = "古典",
				elastique_pro = "プロ",
				elastique_efficient = "高効率",
				elastique_soloist_monophonic = "ソロ（モノラル）",
				elastique_soloist_speech = "ソロ（音声）",
				classic_a01 = "A01. 音楽1（最小変调、エコーがあるかもしれない）",
				classic_a02 = "A02. 音楽2",
				classic_a03 = "A03. 音楽3（こだまが小さい）",
				classic_a04 = "A04. 音楽4（快速、低音に適した）",
				classic_a05 = "A05. 音楽5",
				classic_a06 = "A06. 音楽6",
				classic_a07 = "A07. 音声1",
				classic_a08 = "A08. 音声2",
				classic_a09 = "A09. 音声3（高速）",
				classic_a10 = "A10. ソロ楽器1",
				classic_a11 = "A11. ソロ楽器2",
				classic_a12 = "A12. ソロ楽器3",
				classic_a13 = "A13. ソロ楽器4（こだまが小さい）",
				classic_a14 = "A14. ソロ楽器5",
				classic_a15 = "A15. ソロ楽器6",
				classic_a16 = "A16. ソロ楽器7（高速）",
				classic_a17 = "A17. ドラム、無音高（最小こだま）",
				classic_a18 = "A18. ドラム（ドラムを通すのにもっと適しています）",
				classic_a19 = "A19. ドラム（弱音こだま）",
				batch_subtitle_generation = "字幕の一括生成",
				batch_subtitle_generation_presets = "予め設定された「字幕と文字」メディアジェネレータのプリセットを選択します。",
				batch_subtitle_generation_subtitles = "挿入する字幕テキストを入力します（1行1字幕、空白行は無視）：",
				batch_subtitle_generation_single_duration = "各字幕期間",
				batch_subtitle_generation_suggestion_info = "後で[自動フォローアップ]機能をオンにして、時間を調整します。",
				find_clips = "レール素材を探す",
				find_clips_select_info = "最初に選んだイベント",
				find_clips_info = "一致するクリップを上から選択し、確定したら選択します。",
				find_clips_header_name = "名",
				find_clips_header_num = "数",
				find_clips_match_source = "選択したトラッククリップと同じすべてのクリップ",
				find_clips_match_source_and_offset = "選択したトラッククリップと同じオフセット開始量のすべてのクリップ",
				find_clips_match_name = "指定した名前に一致するクリップ",
				replace = "交換(&R)",
				apply = "申し込み(&A)",
				close = "選ぶ(&C)",
				complete = "完了(&O)",
				cancel = "キャンセル(&C)",
				about = "だいたい(&A)",
				ok = "&OK",
				balloon_title = "記入手順",
				midi_start_second_tooltip = "MIDI音楽の一部を傍受するために使用されます。\n単位：秒。",
				midi_end_second_tooltip = "MIDIファイルの読み取りに必要な時間をここに入力します。\n入力した値が小さすぎると、超過時間の音符が途切れる場合がありますのでご注意ください。\nここに入力した値が開始秒以下の場合、それは常に音楽の全持続時間の終わりまで続くことを意味します。\n単位：秒。",
				midi_beat_conbo_tooltip = "現在、スタッフのページネーション機能にのみ使用されています。\nMIDIファイルからの自動推測は一時的に利用できません。",
				source_start_time_tooltip = "メディア素材のカットの開始時間をここに入力します。\n単位：秒。",
				source_end_time_tooltip = "ここに入力された値が開始秒数以下の場合、それは常にメディア時間の終わりまで続くことを意味することに注意してください。\n単位：秒。",
				no_tune = "チューニングなし",
				pitch_shift_plugin = "ピッチシフトオーディオエフェクトプラグイン",
				elastique_method = "弾性ピッチ変化",
				classic_method = "古典ピッチ変化",
				fool_tuning_method = "チューニングでばか",
				sine_wave = "正弦波",
				triangle_wave = "三角波",
				square_wave = "方形波",
				sawtooth_wave = "鋸歯状波",
				tune_method_tooltip = "「ピッチシフトオーディオエフェクトプラグイン」とは、「オーディオFX」の「ピッチシフト」エフェクトプラグインを使用してピッチを変更することを意味し、プリセットを設定する必要があります。\n「エラスティックピッチチェンジ」とは、「エラスティック」ストレッチ方式でピッチを変更することを意味します。つまり、キーボードの+キーと-キーで直接ピッチを変更し、ピッチ範囲を制限します。",
				audio_lock_stretch_pitch_tooltip = "速度の変化に応じてピッチを変更するためにリサンプリング方式を採用していることを意味します。「エラスティックピッチチェンジ」を使用している場合、\nストレッチオーディオオプションは無効になります。",
				preview_beep_duration_tooltip = "ベースピッチにプレビューの持続時間。\n単位：ミリ秒。",
				preview_tune_audio_tooltip = "チェックを付けると、オーディオの予聴時に、オーディオ素材を主音の高い中央Cに調整します。\n標準音の高さを予聴すると、元の音の高さに設定された音の高さが再生されます。",
				sheet_position_tooltip = "スタッフ中央の3行目から画面中央までの距離。アッププラスダウンマイナス。\n単位：ピクセル。",
				sheet_width_tooltip = "メモは、画面の中央に入力された幅で表示され、左右の空白に使用され、左側の音部記号の左側のスペースに使用されます。\n単位：ピクセル。",
				sheet_gap_tooltip = "スタッフの線の間の距離。\n単位：ピクセル。",
				sheet_line_thickness_tooltip = "スタッフ生成時のシートラインの太さ。 厚さが100％に達すると、シートラインはシートラインの間隔全体を占めます。\n単位：パーセンテージ。",
				sheet_relative_tooltip = "チェック後、以下に入力されたパラメータのピクセル単位は、1920×1080のサイズを基準にして配置されます。\nそれ以外の場合は、プロジェクトのサイズに基づいて配置されます。",
				sheet_relative = "相対値の使用",
				preview_base_pitch_tooltip = "サウンドがオンになっていて、サウンドスキームがサイレントに設定されていないことを確認してください。\nそれでも問題が解決しない場合は、システムを再起動してください。",
				ytp_max_length_tooltip = "1つのトラッククリップの最大長を指定します。\n単位：ミリ秒。",
				ytp_min_length_tooltip = "単一のトラッククリップの最小の長さを指定します。\n単位：ミリ秒。",
				file = "ファイル(&F)",
				save_config = "ユーザー構成の保存(&S)",
				reset_config = "ユーザー構成のリセット(&R)",
				exit_discarding_changes = "変更を破棄して終了します(&D)",
				exit = "終了(&X)",
				pitch_shift_preset = "ピッチシフトプラグインプリセット(&P)",
				load_presets = "プリセットをロード",
				unload_presets = "プリセットをアンロード",
				form_size = "フォームサイズ",
				remember_form_size = "フォームサイズを記憶する",
				remember_always_form_size = "フォームサイズを毎回記憶する",
				remember_once_form_size = "フォームサイズを1回記憶する",
				restore_default_form_size = "次回デフォルトのフォームサイズに戻す",
				help = "ヘルプ(&H)",
				user_help = "手順",
				trouble_shooting = "トラブルシューティング",
				update_info = "リリースノート",
				repository_link = "リポジトリリンク",
				latest_version_link = "最新バージョンのリンク",
				roadmap = "路线图",
				update_logs = "更新日志",
				tutorial_video = "教程视频",
				release_notes = "发行说明",
				staff_visualizer_documentation = "五线谱可视化文档",
				english_documentations = "英语说明文档",
				chinese_documentations = "中文说明文档",
				check_update = "更新を確認(&U)",
				why_ok_btn_is_disabled = "なぜ［完了］ボタンをクリックできないですか？",
				media = "メディア",
				audio = "オーディオ",
				video = "ビデオ",
				staff = "スタッフ",
				ytp = "ユーチューブポープ",
				helper = "ツール",
				midi_settings = "MIDI構成",
				midi_start_time = "秒を開始",
				midi_end_time = "秒を终了",
				bpm_setting = "BPMテンポをに設定します",
				midi_beat = "ビート",
				midi_channel_setting = "MIDIチャネルの使用",
				browse = "参照...",
				no_midi_selected = "<MIDIファイルが選択されていません>",
				choose_midi_file = "MIDIファイルを選択",
				midi_dynamic_midi_bpm = "ダイナミックMIDIテンポ",
				midi_midi_bpm = "MIDIテンポ",
				midi_project_bpm = "プロジェクトテンポ",
				midi_custom_bpm = "カスタム",
				dynamic_midi_bpm_info = "{0}からのダイナミックテンポ",
				dynamic_midi_beat_info = "{0}からのダイナミックビート",
				colon = "：",
				semicolon = "；",
				source_settings = "素材構成",
				generate_at_begin = "プロジェクト開始",
				generate_at_cursor = "カーソル",
				generate_position = "どこを生成しますか",
				choose_source_file = "メディアファイルを選択します",
				selected_media = "選択したメディアファイル",
				selected_clip = "選択されたトラッククリップ",
				source_start_time = "秒を開始",
				source_end_time = "秒を终了",
				parameters = "パラメーター",
				linear = "線形",
				fast = "速い",
				slow = "遅い",
				smooth = "スムーズ",
				sharp = "シャープ",
				hold = "保つ",
				fade_in = "フェードイン",
				fade_out = "フェードアウト",
				tune = "チューニング",
				tune_method = "チューニング方法",
				base_pitch = "ベースピッチ",
				preview_listen = "プレビュー",
				preview_base_pitch = "ベースピッチをプレビュー(&B)",
				preview_audio = "音声のプレビュー(&P)",
				stop_preview_audio = "プレビューを停止(&P)",
				lock_attr = "ロック属性",
				preview_listen_attr = "プレビュー属性",
				preview_tune_audio = "オーディオを主音の高さに調整する",
				reserve_formant = "フォルマントを保持",
				stretch_attr = "引張り属性",
				create_event_group = "グループの作成",
				aconfig = "オーディオを有効",
				audio_stretch = "ストレッチ",
				audio_loop = "ループ",
				audio_normalize = "正常化する",
				audio_lock_stretch_pitch = "伸縮と音調をロックする",
				video_glow_bright = "グローの明るさ",
				video_glow = "グロー",
				video_start_size = "開始サイズ",
				video_end_size = "終了サイズ",
				video_start_rotation = "開始回転",
				video_end_rotation = "終了回転",
				video_start_h_trans = "開始水平に移動",
				video_end_h_trans = "終了水平に移動",
				video_start_v_trans = "開始垂直に移動",
				video_end_v_trans = "終了垂直に移動",
				video_start_hue = "起始色相",
				video_end_hue = "终止色相",
				video_start_saturation = "起始饱和",
				video_end_saturation = "终止饱和",
				video_start_contrast = "起始对比",
				video_end_contrast = "终止对比",
				video_start_threshold = "起始阈值",
				video_end_threshold = "终止阈值",
				video_multitrack_for_chords = "复音多轨",
				effect = "効果",
				visual_effect = "視覚効果",
				initial_visual_effect = "初期値",
				no_effects = "効果なし",
				h_flip = "水平に反転",
				v_flip = "垂直に反転",
				ccw_flip = "反時計回りに反転",
				cw_flip = "時計回りに反転",
				ccw_rotate = "反時計回りの回転",
				cw_rotate = "時計回りの回転",
				turned = "向きを変えた",
				h_mirror = "水平の鏡像",
				v_mirror = "垂直の鏡像",
				ccw_mirror = "反時計回りの鏡像",
				cw_mirror = "時計回りの鏡像",
				negative = "色を反転",
				lumin_invert = "輝度を反転",
				hue_invert = "色相を反転",
				step_change_hue = "{0} 段階色収差",
				chromatic_and_monochrome = "彩色と単色",
				pingpong = "ピンポン効果",
				whirl = "愛の魔力が輪回し",
				vertical_expansion = "縦拡張",
				vertical_expansion_with_rebound = "縦拡張とリバウンド",
				vertical_compression = "縦圧縮",
				vertical_compression_with_rebound = "縦圧縮とリバウンド",
				oblique_expansion_and_compression = "斜め拡縮",
				puyo_puyo = "ぷよぷよ",
				pendulum = "振り子",
				gaussian_blur = "ガウスぼかし",
				radial_blur = "輻射ぼかし",
				vconfig = "ビデオを有効",
				video_stretch = "ストレッチ",
				video_loop = "ループ",
				freeze_first_frame = "静止画像",
				freeze_last_frame = "延長しない",
				legato = "間隙を埋める",
				sheet_width = "表面幅",
				sheet_thickness = "線の太さ",
				sheet_color = "線の色",
				sheet_position = "表面位置",
				sheet_clef = "譜表記号",
				sheet_gap = "線のギャップ",
				sheet_g_clef = "ト音記号",
				sheet_f_clef = "低音部記号",
				sheet_notes_shift = "ノートシフト",
				sheet_config = "スタッフの効果を有効",
				sheet_generate = "スタッフを生成する",
				sheet_config_info = "スタッフの視覚効果をオンにする場合は、ビデオオプションを有効にする必要があります。\nこのオプションを有効にすると、ビデオの視覚効果とビデオのストレッチオプションが無効になります。",
				sheet_notes_params = "音符参数",
				sheet_line_sheet_params = "谱线参数",
				sheet_legacy_engine = "旧版引擎",
				sheet_legacy_engine_tooltip = "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\n即音符和谱线的位置和形状由轨道运动定位。",
				ytp_clips_count = "クリップの数",
				ytp_min_length = "最小の長さ",
				ytp_max_length = "最大の長さ",
				ytp_chorus = "コーラス",
				ytp_pitch_change = "ピッチを変更する",
				ytp_vibrato = "ビブラート（おそらく波の効果を付ける）",
				ytp_reverse = "逆再生",
				ytp_delay = "遅れ",
				ytp_speed_change = "速度を変更する",
				ytp_hue_change = "色相を変更する",
				ytp_hue_rotate = "色相を回転させる",
				ytp_monochrome = "黒と白",
				ytp_negative = "ネガティブ（おそらくピッチダウン効果を付ける）",
				ytp_high_freq_repeat = "高周波リピート",
				ytp_random_tone = "ランダムチューニング（水平フリップ効果を付ける）",
				ytp_enlarge = "拡大（大声で添付）",
				ytp_spherize = "球形化",
				ytp_mirror = "鏡像",
				ytp_high_contrast = "ハイコントラスト（大声で添付）",
				ytp_oversaturation = "過飽和（おそらくピッチアップ効果を付ける）",
				ytp_emphasize_thrice = "三回強調する（焦点を拡大効果を付ける）",
				ytp_info = "現在のタブの下にある［完了］ボタンをクリックすると、音MAD/YTPMVの代わりにYTPが生成されます。\n「オーディオを有効」と「ビデオを有効」以外のパラメータ設定は、YTPでは有効になりません。",
				select_interval = "間隔を選択",
				select_interval_configform_info = "この機能は、ユーザーが1つまたは複数のクリップごとにクリップを選択し、「ビデオイベントの貼り付け」などの操作を実行できるように設計されています。",
				quick_normalize = "クイック正規化する",
				quick_normalize_configform_info = "選択した複数のオーディオトラッククリップをすべて音量に正規化します。",
				quick_normalize_complete = "音量の正規化を完了します。",
				replace_clips_configform_info = "複数のトラッククリップを指定された新しいトラッククリップと交換します。",
				auto_layout_tracks_configform_info = "選択したトラックをYTPMVのようなスタイルで自動レイアウトします。",
				change_tune_method_configform_info = "複数のオーディオトラックのクリップを一括して指定されたトーンアルゴリズムに変更します。",
				batch_subtitle_generation_configform_info = "「字幕と文字」のプリセットを設定し、ここに複数行の文字を追加します。",
				find_clips_configform_info = "指定した条件（クリップ名、選択したクリップと同じ素材など）に基づいて、条件に一致するすべてのトラッククリップを選択します。",
				clear_tracks_motion = "クリアトラックモーション",
				clear_tracks_effect = "クリアトラック効果",
				track_legato = "ラックの間隙を埋める",
				track_legato_stacking = "素材を積み上げる",
				track_legato_limit_stretch = "引張り素材（引張り限界の範囲に制限する）",
				track_legato_stretch = "引張り素材",
				track_legato_lengthen = "素材の持続時間を変更します",
				helper_info = "以下の機能は、いくつかの独立した補助機能であり、オーディオとビデオを生成する他のパラメーターとは関係ありません。",
				helper_info_warning = "注：このダイアログボックスは操作後に閉じられます。後で再度開くことができ、保存されていない変更の一部が失われる可能性があります。\n",
				close_after_open_helper = "操作後にこのダイアログを閉じます",
				otomad_helper_config = "Otomad Helper for Vegas - 設定",
				reset_config_successful = "リセットが完了しました。スクリプトを再起動してください。",
				reset_config_successful_title = "ユーザ設定のリセット",
				sure_to_reset_config = "ユーザー構成をリセットしてもよろしいですか？\n\nリセット後、ユーザープロファイルデータは失われます。",
				about_title = "だいたい",
				script_author = "著者",
				script_original_author = "原著者",
				documentation = "説明文書",
				why_ok_btn_is_disabled_info = "次の手順に従って順番に問題を確認してください：",
				why_ok_btn_is_disabled_no_audio_and_video_enabled = "「オーディオを有効」と「ビデオを有効」は同時にチェックアウトされます。少なくとも一つを有効にしてください。",
				why_ok_btn_is_disabled_no_media_take = "選択されたメディア素材のソースには、有効なメディアリソースが含まれていません。",
				why_ok_btn_is_disabled_no_midi_select = "音MAD/YTPMVを生成するには、まずMIDIシーケンスファイルを選択してください。",
				why_ok_btn_is_disabled_in_helper_tab = "誤操作を避けるためには、「ツール」タブで作成操作を提出しないでください。",
				why_ok_btn_is_disabled_unknown_problem = "原因は不明です。",
				no_selected_media_warning = "警告：プロジェクトメディアウィンドウで有効なメディアを選択していません！",
				no_selected_clip_warning = "警告：トラックウィンドウでクリップを選択していません！",
				preview_audio_track_name = "オーディオトラックのプレビュー（削除する必要があります！）",
				no_midi_exception = "エラー：MIDIファイルが選択されていません。\n\nスクリプト設定ダイアログボックスを再度開き、「MIDI設定」グループの「参照」ボタンをクリックして、有効なMIDIファイルを開いてください。",
				no_media_exception = "エラー：メディアファイルが選択されていません。\n\nスクリプト設定ダイアログボックスを再度開き、「メディア設定」グループの「参照」ボタンをクリックして、有効なメディアファイルを開いてください。",
				no_track_info_exception = "エラー：MIDIトラックがありません。\n\n考えられる理由：\n1.MIDIトラックを選択しなかった。\n2.MIDIファイルにチャンネルがありません。\n3. MIDIファイルが破損しているか、ファイル形式がサポートされていません。",
				no_plugin_pitch_shift_exception = "エラー：ピッチシフトプラグインを呼び出すことができません。\n\n正しく動作するには、チュートリアルドキュメント{0}の指示に従ってください。\nただし、この更新されたバージョンのスクリプトによると、中国語と英語のバージョンは正常に機能するはずです。\nしたがって、他の言語でVegasを使用している可能性が非常に高くなります。",
				no_plugin_presets_exception = "エラー：ピッチシフトプラグインのプリセットエフェクトを呼び出すことはできません。\n\n正しく動作するには、チュートリアルドキュメント{0}の指示に従ってください。\n25個のプリセットすべてが転置プラグインに手動で追加され、正しく名前が付けられていることを確認してください。\n\n補足：詳細については、上記リンク欄の設置方法の説明を参照してください。これらの25のプリセットは、次のオクターブ内のすべてのタイプのピッチ変更です。\nそれらのいずれかが欠落していると、エラーが発生する可能性があります。手動でプリセットを追加するのは確かに非常に面倒ですが、Vegasはスクリプトを使用し\nてピッチシフトの特定のパラメーターを指定できないため、このトリックを回避する必要がありました。",
				no_plugin_name_exception = "エラー：{0}プラグインを呼び出すことができませんでした。\n\n中国語以外のバージョンのVegas、またはまだテストされていない別のバージョンのVegasを使用している可能性があります。",
				no_take_exception_ps = "補足：それでも解決できない場合は、メディアファイルがVegasでサポートされていない形式である可能性があることを意味します。\nファイルを手動でVegasにドラッグして、ビデオとオーディオが正常かどうかを確認できます。",
				no_audio_take_exception = "エラー：オーディオメディアストリームを読み取ることができません。\n\n設定インターフェイスで、純粋なビデオ／画像メディアの「有効なオーディオ」をチェックしないでください。\n\n",
				no_video_take_exception = "エラー：ビデオメディアストリームを読み取ることができません。\n\n設定画面で、純粋なオーディオメディアの[有効なビデオ]をチェックしないでください。\n\n",
				no_media_take_exception = "エラー：メディアを読み取ることができません。\n\n選択したファイル形式はVegasではサポートされていません。メディアファイルが破損していないか、対応するVegasデコーダーがインストールされていないか確認してください。\n\n",
				not_a_midi_file_exception = "エラー：MIDIファイルを読み取ることができません。\n\n解決策：ホストソフトウェアでMIDIをインポートしてから、新しいMIDIファイルを再出力します。\n\n補足：MIDIファイルには複数の形式があり、スクリプトはそれらすべてが正しく読み取れることを保証するものではありません。幸い、\nデフォルト設定で主流のホストソフトウェアによってエクスポートされたMIDIファイルは一般的に読み取り可能です。（現在テスト済みのFL Studio、LMMS、\nおよびMusic Studio for iPadです。）",
				no_selected_exception_ps = "追記：フォルダ内のメディアを手動で選択する場合は、右側の\n[参照]ボタンをクリックしてメディアを選択してください。また、左側のドロップダウンメニューで、選択したファイルのパスが選択されていることを確認してください。",
				no_selected_media_exception = "エラー：プロジェクトメディアウィンドウでメディアが選択されていません。\n\nプロジェクトメディアウィンドウでメディアを選択してから、構成ダイアログを再度開き、素材設定で「選択したメディアファイル」を選択してください。\n\n",
				no_selected_clip_exception = "エラー：トラックでクリップが選択されていません。\n\nトラック内のクリップを選択してから、構成ダイアログを再度開き、素材設定で「選択したトラッククリップ」を選択してください。\n\n",
				no_time_stretch_pitch_shift_exception = "エラー：選択したクリップのピッチ変換方法がチューニングなしに設定されています。\n\n「選択したトラッククリップ」を使用している可能性があります。あなたはこのエラーのせいではなく、ベガスの脳死したデザインのせいです。\n\n解決策：トラッククリップを再度選択し、オーディオパーツを右クリックして、下部にある[プロパティ]を選択してください。「タイムストレッチ／ピッチ変換」の「方法」を「エラスティック」に設定します。\n次に、[OK]をクリックします。\n\n補足：オーディオイベントが移調されておらず、そのプロパティが開かれている場合、そのプロパティの「タイムストレッチ／ピッチ変換」の「メソッド」は次のようになります。\n自動的に「なし」に変更され、「OK」をクリックします。有効にします。現時点では、キーボードの+および-キーのチューニング操作が無効であることがわかります。このとき、オーディオイベントのプロパティを\n再度開き、「タイムストレッチ／ピッチ変換」の「方法」を「エラスティック」に設定する必要があります。「ピッチ変更」を設定する必要はありません。「OK」をクリックするだけです。",
				read_config_fail_exception = "エラー：パラメータ構成ファイルの読み取りに失敗しました。\n\n残念ながら、この予期しないエラーが発生しました。問題を解決するために、ユーザー構成設定をクリアしてデフォルト設定に復元します。\n問題を迅速に解決するために、このエラーの作成者に伝えることをお勧めします。\nこのスクリプトは終了します。その後、手動で再度開く必要があります。",
				fail_to_select_clips_exception = "エラー：トラッククリップの選択中にエラーが発生しました。\n\n最初にトラックウィンドウでいくつかのトラッククリップを選択してください。",
				fail_to_select_tracks_exception = "エラー：トラックの選択中にエラーが発生しました。\n\n最初にトラックウィンドウでいくつかのビデオトラックを選択してください。",
				ytp_over_length_exception = "エラー：指定されたYTPの最小の長さがメディアの長さを超えています。\n\n指定されたYTPの最小の長さが長すぎます。小さい値を試してください。または、選択したメディアの長さが短すぎます。",
				ytp_in_media_generator_exception = "エラー：メディアジェネレーターによって生成されたメディアにYTPを適用します。\n\nYTPのアプリケーションは、ローカルメディアファイルを使用する必要があります。メディアジェネレーターによって生成されたメディアは使用しないでください。",
				ytp_eliminate_duplicates_finally_null_exception = "技術異常：YTP素材リストの重複値除去操作を行い、最後のリストが空きました。\n\nこれは起こるべきではないエラーです。",
				unknown_exception = "エラー：不明の異常です。\n\n詳細を展開して、具体的なエラー内容を確認し、エラー情報を著者にフィードバックしてください。",
				use_pic_in_pic_on_unsupported_vegas_exception = "エラー：低バージョンVegasでは、この絵にプラグインを描く効果はサポートされていません。\n\n根本的な理由：Vegasは新しいバージョンの「絵に描いた」効果プラグインに新しい機能とパラメータを追加しました。これらの新しいパラメータは低バージョンのVegasプラグインでは使用できません。\n\n解決方法：現在のVegasバージョンでは、このPVリズム視覚効果（拡張クラスなど）は使用できません。他の視覚効果を使用してください。またはVegasソフトウェアを更新します。"
			};
			Russian = new Lang {
				__name__ = "Русский",
				info_label_font = "Segoe UI",
				ui_font = "Segoe UI",
				restart_to_effect_language = "Перезагрузить, чтобы язык вступил в силу?",
				yes_to_restart_to_effect_language = "Ручной перезапуск сейчас",
				no_to_restart_to_effect_language = "Не перезагружается чтобы вступить в силу",
				version_number = "Номер версии",
				revision_date = "Дата последнего изменения",
				vegas_version = "Версия Vegas",
				script_supported_vegas_version = "Версия с поддержкой скрипта",
				unsupported_vegas_version = "Предупреждение: Текущий скрипт не соответствует версии Vegas, которую вы сейчас используете. Некоторые функции могут работать некорректно! Пожалуйста, переустановите правильную версию скрипта.\nПоддерживаемая версия: Vegas Pro {0}\nТекущая версия: Vegas Pro {1}",
				unsupported_vegas_version_title = "Несоответствие версий",
				unsupported_vegas_version_feature = "Версии ниже Vegas Pro {0} не поддерживают эту функцию.",
				midi_file_name = "MIDI последовательность",
				all_files = "Все файлы",
				choose_a_midi_file = "Пожалуйста, выберите файл MIDI",
				media_file_name = "Поддерживаемые медиа-файлы",
				choose_a_source_file = "Пожалуйста, выберите видео или картинку",
				error = "Ошибка",
				details = "Подробности:",
				brightness_and_contrast = "Яркость и контраст",
				invert = "Инвертировать",
				black_and_white = "Черное и белое",
				lab_adjust = "Настройка LAB",
				hsl_adjust = "Настройка HSL",
				mirror = "Зеркало",
				pic_in_pic = "Картинка в картинке",
				crop = "Обрезать",
				check_pitch_shift_presets = "Проверка доступности пресетов плагина Pitch Shift...",
				no_pitch_shift_presets = "Поскольку вы пытались настроиться с помощью плагина эффекта «Pitch Shift», система обнаружила, что вы не полностью настроили все необходимые звуковые пресеты. Сценарий может попытаться добавить для вас пресеты, что может привести к сбою. Если это не помогло, следуйте инструкциям по использованию учебника вручную. Хотите, чтобы скрипт автоматически добавлял пресеты для вас?",
				yes_to_add_pitch_shift_presets = "добавить автоматически",
				no_to_add_pitch_shift_presets = "Назад к конфигурации",
				no_pitch_shift_presets_title = "Найдены не все предустановки Pitch Shift",
				add_pitch_shift_presets_successful = "Завершите добавление пресетов!",
				add_pitch_shift_presets_fail = "Не удалось добавить предустановки!",
				add_pitch_shift_presets_fail_title = "к несчастью",
				ensure_load_presets = "Вы уверены, что хотите загрузить пресеты?",
				ensure_unload_presets = "Вы уверены, что хотите выгрузить пресеты?",
				reverse_suffix_tag = "(В обратном порядке)",
				effect_init_forward = "Вперед",
				effect_init_reversed = "Обратный",
				effect_init_turned = "Повернулся",
				effect_init_left = "Левый",
				effect_init_right = "Верно",
				effect_init_up = "Верхний",
				effect_init_down = "Нижний",
				effect_init_left_up = "Верхний левый",
				effect_init_right_up = "Верхний правый",
				effect_init_left_down = "Нижний левый",
				effect_init_right_down = "Нижний правый",
				effect_init_invert = "Инвертировать",
				effect_init_conform = "Соответствующий",
				effect_init_opposite = "Противоположный",
				effect_init_chromatic = "Хроматический",
				effect_init_monochrome = "Монохромный",
				effect_init_counter = "Прилавок",
				effect_init_stepon = "Продолжать",
				effect_init_flat = "Плоский",
				effect_init_thin = "Тонкий",
				effect_init_blur = "Размывание",
				enable_all_effects = "Открыть все эффекты",
				chorus = "Хор",
				vibrato = "Вибрато",
				wave = "Волна",
				multi_beat_delay = "Многобитовая задержка",
				spherize = "Сфериз",
				warning_missing_plugin = "Предупреждение: не удалось найти подключаемый модуль \"{0}\"!",
				midi_channel = "Канал",
				midi_notes_count = "Количество нот",
				midi_begin_note = "Начать заметку",
				instrument = "Инструмент",
				error_code = "Код ошибки:",
				processing_otomad = "Создание Отомад / УТРМѴ ...",
				processing_ytp = "Создание УТР ...",
				processing_it = "Обработка его",
				real_time_update = "обновление текущего прогресса в реальном времени (замедлит темпы генерации)",
				replacer_is = "Указанная замена",
				replacer_info = "Сначала выберите клипы, которые необходимо заменить и заменить в окне дорожки, а затем укажите клип в качестве замещающего клипа, а остальные клипы будут замененными клипами.\nСначала создайте группу для аудио и видео замещающего клипа , и убедитесь, что замещающий клип размещен позже по времени и не на той же дорожке, что и другие замененные клипы.",
				replace_clips = "Заменить отслеживание событий",
				replaced_info = "Затем оставшиеся клипы дорожки {0} будут заменены выбранным клипом.",
				replaced_label = "Заменены",
				replacer_label = "Замена",
				separation_replaced_info = "Выбрано {0} треков, из них {1} аудиоклипов, {2} видеоклипов.",
				separation_set_replaced = "Сделать выбранные ресурсы {0} для заменены",
				separation_set_replacer = "Сделать выбранные объекты {0} для замена",
				separation_back_to_select = "Вернитесь в Вегас чтобы выбрать клип",
				separation_use_clip_group = "Также заменить другие клипы в группе",
				separation_reserve_original_name = "Зарезервировать исходное имя клипа",
				replacements_classic_specify = "Указать одновременно",
				replacements_separation_specify = "Уточняйте отдельно",
				view = "Осмотр",
				selected_items = "Избранный член",
				replace_clips_form_ensure_to_close = "Часть орбитального материала указана в разделе \"Уточняйте отдельно\".\nПосле выхода система может не сохранять изменения.",
				track = "Отслеживать",
				submit_select = "&Установить выбранное",
				every_few = "Выберите по одному на каждые несколько",
				which_one = "Выберите из какой группы",
				select_how_many = "Выберите, сколько за раз",
				reset_select = "&Сбросить выбор",
				quick_select_interval = "Интервал быстрого выбора",
				select_interval_info = "Сначала выберите несколько клипов в окне трека Vegas, а затем откройте это диалоговое окно, чтобы использовать следующие функции.",
				select_events_count_info = "Выбрано событий отслеживания {0}.",
				select_videotracks_count_info = "Выбрано {0} видеодорожек.",
				select_audioevents_count_info = "Выбран набор дорожек для аудио {0}.",
				select_source_count_info = "Выделен материал для медиа {0}.",
				square = "Квадрат",
				custom = "Настроить",
				row_count = "Количество рядов",
				column_count = "Число столбцов",
				fill = "Заполнение",
				adapt = "Адаптировать",
				increase_padding = "Увеличить заполнение",
				auto_layout_tracks = "Автоматическая компоновка дорожки",
				grid_layout = "Макет сетки",
				box_3d_layout = "Макет 3D-бокса",
				selected_tracks_too_much = "Вы выбрали треков: {0}, что выходит за рамки доступных функций. Слишком много!\nПожалуйста, выберите меньше треков и попробуйте еще раз.",
				selected_tracks_too_much_title = "Выбрано слишком много треков",
				bottom_surface = "Нижний",
				top_surface = "верхний",
				right_surface = "Верно",
				left_surface = "левый",
				back_surface = "Назад",
				front_surface = "Передний",
				box_3d_layout_info = "Из-за ограничений функции скрипта будет создана новая дорожка, и клипы на выбранной дорожке будут перемещены, а движение дорожки, эффекты и другие эффекты в исходной дорожке будут потеряны.\nПожалуйста, выберите дорожку, используемую каждой гранью куба ниже. Если он пустой, это означает, что лицо не установлено.",
				delete_original_tracks = "Удалить исходную дорожку",
				use_video_longer_side = "Используйте более длинную сторону как длину ребра куба.",
				use_video_longer_side_tooltip = "После проверки самая длинная сторона видео в настройках проекта (то есть максимальная ширина и высота) будет использоваться в качестве длины ребра куба.",
				gradient_tracks = "Градиентные дорожки",
				gradient_tracks_info = "Выберите эффект градиента для применения к выбранным видеодорожкам:",
				rainbow_color = "Цвета радуги",
				gradually_saturated = "Постепенно насыщенный",
				gradually_contrasted = "Постепенно контрастирующий",
				threshold = "Порог",
				alternately_chromatic = "Альтернативно хроматический",
				alternately_negative = "Альтернативный отрицательный",
				descending = "По убыванию",
				ascending = "Восходящий",
				change_tune_method = "Change Tuning Method",
				change_tune_method_info = "поддерживает только настройки в атрибутах звуковых событий, но не поддерживает настройки модуля преобразования тональности.",
				time_stretch_pitch_shift = "временное растяжение / высота звука",
				formant_change = "резонансный пик сдвига",
				pitch_change = "высокие ноты",
				method = "метод",
				pitch_lock = "замок тангажа",
				lock_pitch_instead_of_rate = "Блокировка шага вместо скорости",
				none = "нет",
				elastique = "эластик",
				classic = "классический",
				elastique_pro = "профессиональный",
				elastique_efficient = "полезный груз",
				elastique_soloist_monophonic = "солист (монолог)",
				elastique_soloist_speech = "солист (речь)",
				classic_a01 = "A01. музыка 1 (минимальный фланец, отзыв)",
				classic_a02 = "A02. музыка 2",
				classic_a03 = "A03. музыка 3 (уменьшение эха)",
				classic_a04 = "A04. музыка 4 (быстро, подходит для басов)",
				classic_a05 = "A05. музыка 5",
				classic_a06 = "A06. музыка 6",
				classic_a07 = "A07. речь 1",
				classic_a08 = "A08. речь 2",
				classic_a09 = "A09. речь 3 (быстро)",
				classic_a10 = "A10. одиночный музыкальный инструмент 1",
				classic_a11 = "A11. одиночный музыкальный инструмент 2",
				classic_a12 = "A12. одиночный музыкальный инструмент 3",
				classic_a13 = "A13. одиночный музыкальный инструмент 4 (уменьшение эхо)",
				classic_a14 = "A14. одиночный музыкальный инструмент 5",
				classic_a15 = "A15. одиночный музыкальный инструмент 6",
				classic_a16 = "A16. одиночный музыкальный инструмент 7 (быстро)",
				classic_a17 = "A17. барабан (munimum echo)",
				classic_a18 = "A18. барабан (лучше для Тома)",
				classic_a19 = "A19. барабан (микроэхо)",
				batch_subtitle_generation = "Серийное Создание Субтитров",
				batch_subtitle_generation_presets = "Выберите предварительно установленную \"Субтитры и текст\" для медиа - генератора:",
				batch_subtitle_generation_subtitles = "Введите текст для вставки субтитров (одна строка, Игнорируемая пустая строка):",
				batch_subtitle_generation_single_duration = "Продолжительность каждого субтитра",
				batch_subtitle_generation_suggestion_info = "Затем может быть запущена функция \"автоматического контроля\" для последующей корректировки времени.",
				find_clips = "Найти Отслеживать События",
				find_clips_select_info = "Выбрано первое событие трека",
				find_clips_info = "Выберите соответствующие клипы выше, и они будут выбраны, когда вы уверены.",
				find_clips_header_name = "Имя",
				find_clips_header_num = "Количество",
				find_clips_match_source = "Соответствие тому же источнику",
				find_clips_match_source_and_offset = "Сопоставление одного и того же источника с одинаковым начальным смещением",
				find_clips_match_name = "Соответствие указанному имени",
				replace = "За&менять",
				apply = "&Подать заявление",
				close = "&Закрывать",
				complete = "&Полный",
				cancel = "О&тмена",
				about = "&О",
				ok = "К&онечно",
				balloon_title = "Инструкции по заполнению",
				midi_start_second_tooltip = "Используется для перехвата части MIDI-музыки.\nЕдиница: секунды.",
				midi_end_second_tooltip = "Введите здесь время, необходимое для чтения файла MIDI.\nОбратите внимание, что если введенное значение слишком мало, ноты в лишнее время будут обрезаны.\nЕсли введенное здесь значение меньше или равно начальным секундам, это всегда означает, что оно длится до конца всей продолжительности музыки.\nЕдиница: секунды.",
				midi_beat_conbo_tooltip = "В настоящее время используется только для функции разбивки на страницы персонала.\nАвтоматическое вычисление из файлов MIDI временно недоступно.",
				source_start_time_tooltip = "Введите здесь время начала резки медиа-материала.\nЕдиница: секунды.",
				source_end_time_tooltip = "Обратите внимание: если введенное здесь значение меньше или равно количеству стартовых секунд, это всегда означает, что оно длится до конца медиа-времени.\nЕдиница: секунды.",
				no_tune = "Нет настройки",
				pitch_shift_plugin = "Плагин звукового эффекта Pitch Shift",
				elastique_method = "Эластик изменение шага",
				classic_method = "Классический изменение шага",
				fool_tuning_method = "Глухой тон",
				sine_wave = "синусоидальная",
				triangle_wave = "треугольная",
				square_wave = "прямоугольная",
				sawtooth_wave = "пилообразная",
				tune_method_tooltip = "«Подключаемый модуль звукового эффекта« Pitch Shift »» означает использование подключаемого модуля эффекта «Pitch Shift» в «Audio FX» для изменения высоты звука, а предустановки должны быть настроены.\n«Эластичное изменение высоты звука» означает использование метода растяжения «Élastique» для изменения высоты звука, то есть клавиши + и -на клавиатуре напрямую изменяют высоту звука, а диапазон высоты звука ограничен.",
				audio_lock_stretch_pitch = "Что используется метод повторной выборки для изменения высоты звука при изменении скорости. Если вы используете «Эластичное изменение высоты тона»,\nопция растягивания звука будет отключена.",
				sheet_position_tooltip = "Расстояние от третьей строки посередине нотоносца до центра экрана. Вверх плюс вниз минус.\nЕдиница: пиксель.",
				sheet_width_tooltip = "Ноты будут отображаться по ширине, заполненной в середине экрана, используемой для левого и правого белого пространства, и левого пространства для ключа слева.\nЕдиница: пиксель.",
				sheet_gap_tooltip = "Расстояние между строками в кадре.\nЕдиница: пиксель.",
				sheet_line_thickness_tooltip = "Толщина линий листа при генерации кадров. Когда толщина достигает 100%, линии листа занимают все расстояние между линиями листа.\nЕдиница: процент.",
				sheet_relative_tooltip = "После проверки пиксельная единица параметров, заполненных ниже, будет расположена относительно размера 1920 × 1080;\nв противном случае он будет размещен в зависимости от размера проекта.",
				sheet_relative = "Использование относительных значений",
				preview_base_pitch_tooltip = "Убедитесь, что звук включен и звуковая схема не установлена на беззвучный режим.\nЕсли по-прежнему нет работы, перезагрузите систему.",
				ytp_max_length_tooltip = "Укажите максимальную длину клипа на одну дорожку.\nЕдиница: миллисекунды.",
				ytp_min_length_tooltip = "Укажите минимальную длину клипа на одну дорожку.\nЕдиница: миллисекунды.",
				file = "&Файл",
				save_config = "&Сохранить конфигурацию пользователя",
				reset_config = "С&бросить конфигурацию пользователя",
				exit_discarding_changes = "&Отменить изменения и выйти",
				exit = "&Выход",
				pitch_shift_preset = "&Пресеты плагина Pitch Shift",
				load_presets = "Загрузить пресеты",
				unload_presets = "Выгрузить пресеты",
				form_size = "Размер формы",
				remember_form_size = "Запомнить размер формы",
				remember_always_form_size = "Запомнить размер формы каждый раз",
				remember_once_form_size = "Запомнить размер формы один раз",
				restore_default_form_size = "Восстановить размер формы по умолчанию далее",
				help = "&Помощь",
				user_help = "Инструкции",
				trouble_shooting = "Устранение неполадок",
				update_info = "Примечания к выпуску",
				repository_link = "Ссылка на репозиторий",
				latest_version_link = "Ссылка на последнюю версию",
				roadmap = "路线图",
				update_logs = "更新日志",
				tutorial_video = "教程视频",
				release_notes = "发行说明",
				staff_visualizer_documentation = "五线谱可视化文档",
				english_documentations = "英语说明文档",
				chinese_documentations = "中文说明文档",
				check_update = "Проверять &Обновление",
				why_ok_btn_is_disabled = "Почему невозможно нажать кнопку завершения?",
				media = "СМИ",
				audio = "Аудио",
				video = "видео",
				staff = "Сотрудники",
				ytp = "УТР",
				helper = "инструмент",
				midi_settings = "Конфигурация MIDI",
				midi_start_time = "Начало секунд",
				midi_end_time = "Конец секунд",
				bpm_setting = "Установите темп BPM на",
				midi_beat = "Бить",
				midi_channel_setting = "Использование MIDI-канала",
				browse = "Просматривать...",
				no_midi_selected = "<MIDI-файл не выбран>",
				choose_midi_file = "Выберите файл MIDI",
				midi_dynamic_midi_bpm = "динамический MIDI темп",
				midi_midi_bpm = "MIDI темп",
				midi_project_bpm = "Время проекта",
				midi_custom_bpm = "Обычай",
				dynamic_midi_bpm_info = "динамический ритм от {0}",
				dynamic_midi_beat_info = "динамический такт от {0}",
				colon = ": ",
				semicolon = "; ",
				source_settings = "Исходная конфигурация",
				generate_at_begin = "Начало проекта",
				generate_at_cursor = "Курсор",
				generate_position = "Сгенерировать в",
				choose_source_file = "Выберите медиафайл",
				selected_media = "Выбранный медиафайл",
				selected_clip = "Выбранное событие трека",
				source_start_time = "Начало секунд",
				source_end_time = "Конец секунд",
				parameters = "Параметры",
				linear = "Линейный",
				fast = "Быстро",
				slow = "Медленный",
				smooth = "Гладкий; плавный",
				sharp = "Острый",
				hold = "Держать",
				fade_in = "Исчезать",
				fade_out = "Исчезать",
				tune = "Тюнинг",
				tune_method = "Метод настройки",
				base_pitch = "Базовый шаг",
				preview_listen = "Просмотр",
				preview_base_pitch = "Предварительный просмотр &базового шага",
				preview_audio = "&Предварительный просмотр аудио",
				stop_preview_audio = "Остановить &предварительный просмотр",
				lock_attr = "Заблокировать атрибут",
				preview_listen_attr = "Свойства подслушивания",
				preview_tune_audio = "Настройка звука на громкость",
				reserve_formant = "зарезервированный резонансный пик",
				stretch_attr = "Свойства растяжения",
				create_event_group = "Создать группировку",
				aconfig = "Включено",
				audio_stretch = "Потягиваться",
				audio_loop = "Петля",
				audio_normalize = "Нормализовать",
				audio_lock_stretch_pitch_tooltip = "Заблокировать расширение и тональность",
				preview_beep_duration_tooltip = "время ожидания стандартного фонетического сигнала.\nв миллисекундах.",
				preview_tune_audio_tooltip = "после включения, при предварительном прослушивании аудио материал будет скорректирован на основной звук средней школы с.\nВ противном случае стандартная высота звука будет воспроизвести заданную высота звука.",
				video_glow_bright = "Яркость свечения",
				video_glow = "Светиться",
				video_start_size = "Начальный размер",
				video_end_size = "Конечный размер",
				video_start_rotation = "Начать вращение",
				video_end_rotation = "Конец вращения",
				video_start_h_trans = "Начать перевод",
				video_end_h_trans = "Конец перевода",
				video_start_v_trans = "Начать переключение",
				video_end_v_trans = "Конец переключения",
				video_start_hue = "起始色相",
				video_end_hue = "终止色相",
				video_start_saturation = "起始饱和",
				video_end_saturation = "终止饱和",
				video_start_contrast = "起始对比",
				video_end_contrast = "终止对比",
				video_start_threshold = "起始阈值",
				video_end_threshold = "终止阈值",
				video_multitrack_for_chords = "复音多轨",
				effect = "Эффекты",
				visual_effect = "Визуальные эффекты",
				initial_visual_effect = "Начальное значение",
				no_effects = "Без эффектов",
				h_flip = "Горизонтальный флип",
				v_flip = "Вертикальный переворот",
				ccw_flip = "Переворот против часовой стрелки",
				cw_flip = "Переворот по часовой стрелке",
				ccw_rotate = "Вращение против часовой стрелки",
				cw_rotate = "Вращение по часовой стрелке",
				turned = "Повернулся",
				h_mirror = "Горизонтальное зеркало",
				v_mirror = "Вертикальное зеркало",
				ccw_mirror = "Зеркало против часовой стрелки",
				cw_mirror = "Зеркало по часовой стрелке",
				negative = "Отрицательный",
				lumin_invert = "Инвертировать яркость",
				hue_invert = "Инвертировать оттенок",
				step_change_hue = "{0} ступенчатая аберрация",
				chromatic_and_monochrome = "цветной и монохромный",
				pingpong = "Эффект пинг-понга",
				whirl = "Вращение",
				vertical_expansion = "Вертикальное расширение",
				vertical_expansion_with_rebound = "Вертикальное расширение и отскок",
				vertical_compression = "Вертикальное сжатие",
				vertical_compression_with_rebound = "Вертикальное сжатие и отскок",
				oblique_expansion_and_compression = "Косое расширение",
				puyo_puyo = "Пуйо Пуйо",
				pendulum = "Маятник",
				gaussian_blur = "Размытие по Гауссу",
				radial_blur = "Радиальное размытие",
				vconfig = "Включено",
				video_stretch = "Потягиваться",
				video_loop = "Петля",
				freeze_first_frame = "Статический визуальный",
				freeze_last_frame = "Без удлинения",
				legato = "Связали",
				sheet_width = "Ширина поверхности",
				sheet_thickness = "Толщина линии",
				sheet_color = "Цвет линии",
				sheet_position = "Положение на поверхности",
				sheet_clef = "Персональный ключ",
				sheet_gap = "Line Gap",
				sheet_g_clef = "Скрипичный ключ",
				sheet_f_clef = "Басовый ключ",
				sheet_notes_shift = "Сдвиг нот",
				sheet_config = "Включено",
				sheet_generate = "Создать персонал",
				sheet_config_info = "Если вы хотите включить визуальный эффект персонала, вам необходимо включить опцию видео.\nВключение этой опции отключает видео визуальные эффекты и опцию растяжения видео.",
				sheet_notes_params = "音符参数",
				sheet_line_sheet_params = "谱线参数",
				sheet_legacy_engine = "旧版引擎",
				sheet_legacy_engine_tooltip = "将使用与旧版脚本五线谱可视化 v0.1 相同的引擎。\n即音符和谱线的位置和形状由轨道运动定位。",
				ytp_clips_count = "Количество клипов",
				ytp_min_length = "Минимальная длина",
				ytp_max_length = "Максимальная длина",
				ytp_chorus = "хор",
				ytp_pitch_change = "Изменить высоту",
				ytp_vibrato = "Вибрато (вероятно, придать волновой эффект)",
				ytp_reverse = "Задний ход",
				ytp_delay = "Задерживать",
				ytp_speed_change = "Изменить скорость",
				ytp_hue_change = "Изменить оттенок",
				ytp_hue_rotate = "Повернуть оттенок",
				ytp_monochrome = "Черное и белое",
				ytp_negative = "Отрицательный (возможно, добавлен эффект понижения высоты тона)",
				ytp_high_freq_repeat = "Высокочастотное повторение",
				ytp_random_tone = "Случайная настройка (добавление эффекта горизонтального переворота)",
				ytp_enlarge = "Увеличить (приложить громко)",
				ytp_spherize = "Сферизация",
				ytp_mirror = "Зеркальное отображение",
				ytp_high_contrast = "Высокая контрастность (добавляйте громко)",
				ytp_oversaturation = "Перенасыщение (возможно, добавление эффекта тангажа)",
				ytp_emphasize_thrice = "тройной акцент (дополнительный эффект усиления)",
				ytp_info = "Нажмите кнопку «Завершить» под текущей вкладкой, YTP будет создан вместо Otomad / YTPMV.\nНастройки параметров, отличные от «Enabled Audio» и «Enabled Video», не будут действовать в YTP.",
				select_interval = "Выберите интервал",
				select_interval_configform_info = "Эта функция разработана, чтобы помочь пользователю выбирать клипы каждые несколько или несколько, а затем выполнять такие операции, как «вставка видео-события».",
				quick_normalize = "Быстрая нормализация",
				quick_normalize_configform_info = "Нормализация громкости всех выбранных аудиособытий.",
				quick_normalize_complete = "Завершена нормализация.",
				replace_clips_configform_info = "Замените несколько клипов дорожки указанными новыми клипами дорожки.",
				auto_layout_tracks_configform_info = "Автоматическая компоновка выбранных треков в стиле YTPMV.",
				change_tune_method_configform_info = "преобразовать несколько звуковых дорожек в указанные алгоритмы тонального регулирования.",
				batch_subtitle_generation_configform_info = "Заранее установите предустановку \"Субтитры и текст\", после чего добавьте несколько строк текста.",
				find_clips_configform_info = "Выберите все клипы дорожки, соответствующие указанным критериям, таким как название клипа, тот же материал, что и у выбранного клипа и т.д.",
				clear_tracks_motion = "Очистить отслеживание движения",
				clear_tracks_effect = "Эффект удаления траекторнн",
				track_legato = "Заполнение орбитального зазора",
				track_legato_stacking = "Сборочный монтаж",
				track_legato_limit_stretch = "Эластичный зажим (Предел растяжения)",
				track_legato_stretch = "Эластичный зажим",
				track_legato_lengthen = "Удлиненный зажим",
				helper_info = "Следующие ниже функции являются лишь некоторыми независимыми вспомогательными функциями и не имеют ничего общего с другими параметрами, генерирующими аудио и видео.",
				helper_info_warning = "Примечание: это диалоговое окно будет закрыто после операции, вы можете открыть его позже, и некоторые несохраненные изменения могут быть потеряны!\n",
				close_after_open_helper = "Закрыть диалог после завершения операции",
				otomad_helper_config = "Otomad Helper for Vegas - Конфигурация",
				reset_config_successful = "Сброс завершен, перезапустите скрипт.",
				reset_config_successful_title = "Сбросить конфигурацию пользователя",
				sure_to_reset_config = "Вы уверены, что хотите сбросить настройки пользователя?\n\nДанные вашего профиля пользователя будут потеряны после сброса.",
				about_title = "О",
				script_author = "Автор",
				script_original_author = "Автор оригинала",
				documentation = "Описание документа",
				why_ok_btn_is_disabled_info = "Пожалуйста, проверьте вопрос по порядку:",
				why_ok_btn_is_disabled_no_audio_and_video_enabled = "создание аудио и создание видео были отменены одновременно. Выберите хотя бы один из них.",
				why_ok_btn_is_disabled_no_media_take = "выбранный источник материалов для СМИ не содержит никаких эффективных средств массовой информации.",
				why_ok_btn_is_disabled_no_midi_select = "для создания Otomad/YTPMV Выберите файл последовательности MIDI.",
				why_ok_btn_is_disabled_in_helper_tab = "во избежание неправильной операции не следует представлять генерирующие операции на вкладке \"инструмент\".",
				why_ok_btn_is_disabled_unknown_problem = "Неизвестная причина.",
				no_selected_media_warning = "Предупреждение: Вы не выбрали ни одного допустимого носителя в окне мультимедиа проекта!",
				no_selected_clip_warning = "Предупреждение: Вы не выбрали ни одного клипа в окне трека!",
				preview_audio_track_name = "Предварительный просмотр аудиодорожки (УДАЛЕНО!)",
				no_midi_exception = "Ошибка: не выбран MIDI-файл.\n\nПожалуйста, снова откройте диалоговое окно конфигурации сценария, а затем нажмите кнопку «Обзор» в группе «Конфигурация MIDI», чтобы открыть допустимый файл MIDI.",
				no_media_exception = "Ошибка: медиафайл не выбран.\n\nСнова откройте диалоговое окно конфигурации сценария, а затем нажмите кнопку «Обзор» в группе «Конфигурация мультимедиа», чтобы открыть допустимый файл мультимедиа.",
				no_track_info_exception = "Ошибка: отсутствует MIDI-трек.\n\nВозможные причины:\n1. Вы не выбрали MIDI-трек;\n2. В MIDI-файле нет канала;\n3. Файл MIDI поврежден или формат файла не поддерживается.",
				no_plugin_pitch_shift_exception = "Ошибка: невозможно вызвать плагин Pitch Shift.\n\nДля правильной работы следуйте инструкциям учебного документа {0}.\nОднако, согласно этой обновленной версии скрипта, китайская и английская версии должны работать нормально.\nПоэтому очень вероятно, что вы используете Vegas на других языках.",
				no_plugin_presets_exception = "Ошибка: невозможно вызвать предустановленный эффект плагина Pitch Shift.\n\nДля правильной работы следуйте инструкциям учебного документа {0}.\nУбедитесь, что все 25 пресетов вручную добавлены в плагин транспонирования и названы правильно.\n\nДополнительное объяснение: Для получения подробной информации, пожалуйста, обратитесь к объяснению метода установки в приведенном выше столбце ссылок. Эти 25 предустановок представляют собой все типы изменения высоты звука в пределах следующей октавы.\nОтсутствие любого из них может вызвать ошибки. Действительно, добавлять пресеты вручную очень сложно, но Vegas не может использовать скрипты для определения конкретных параметров\nизменения высоты звука, поэтому мне пришлось обойти этот трюк.",
				no_plugin_name_exception = "Ошибка: не удалось вызвать подключаемый модуль {0}.\n\nВозможно, вы используете некитайскую версию Vegas или другую версию Vegas, которая еще не была протестирована.",
				no_take_exception_ps = "Дополнительное примечание: если проблема по-прежнему не может быть решена, это означает, что медиа-файл может иметь формат, не поддерживаемый Vegas.\nВы можете вручную перетащить файл в Лас-Вегас, чтобы проверить, в порядке ли видео и звук.",
				no_audio_take_exception = "Ошибка: невозможно прочитать аудиопоток.\n\nВ интерфейсе настроек не устанавливайте флажок «Включено аудио» для чистых видео / изображений.\n\n",
				no_video_take_exception = "Ошибка: невозможно прочитать видеопоток.\n\nВ пользовательском интерфейсе настроек не устанавливайте флажок «Включенное видео» для чистого аудио.\n\n",
				no_media_take_exception = "Ошибка: невозможно прочитать носитель.\n\nВыбранный формат файла не поддерживается Vegas. Пожалуйста, проверьте, не поврежден ли медиафайл или не установлен ли соответствующий декодер Vegas.\n\n",
				not_a_midi_file_exception = "Ошибка: невозможно прочитать файл MIDI.\n\nРешение: импортируйте MIDI с помощью программного обеспечения хоста, а затем повторно выведите новый файл MIDI.\n\nДополнительное примечание: существует несколько форматов файлов MIDI, и сценарий не гарантирует, что все они могут быть правильно прочитаны. К счастью,\nфайлы MIDI, экспортированные основным программным обеспечением хоста с настройками по умолчанию, обычно читаются. (В настоящее время протестированы FL Studio, LMMS\nи Music Studio для iPad.)",
				no_selected_exception_ps = "Дополнительное примечание: если вы хотите вручную выбрать носитель в папке, нажмите кнопку «Обзор» справа, чтобы\nвыбрать носитель. И убедитесь, что путь к выбранному вами файлу выбран в раскрывающемся меню слева.",
				no_selected_media_exception = "Ошибка: в окне мультимедиа проекта не выбран ни один носитель.\n\nВыберите носитель в окне мультимедиа проекта, затем снова откройте диалоговое окно конфигурации и выберите «выбранный файл мультимедиа» в настройках источника.\n\n",
				no_selected_clip_exception = "Ошибка: на дорожке не выбраны клипы.\n\nПожалуйста, выберите клип на дорожке, затем снова откройте диалоговое окно конфигурации и выберите «выбранные клипы дорожки» в настройках источника.\n\n",
				no_time_stretch_pitch_shift_exception = "Ошибка: метод преобразования высоты звука выбранного клипа не настроен.\n\nСкорее всего, вы используете «выбранные клипы треков». Вы виноваты не в этой ошибке, а в безумном дизайне Вегаса.\n\nРешение: повторно выберите клипы дорожки, щелкните правой кнопкой мыши по аудиочасти и выберите «Свойства» внизу. Установите «Метод» «Преобразование растяжения во времени / шага» на «élastique».\nЗатем нажмите ОК.\n\nДополнительное примечание: если звуковое событие не было транспонировано и его свойства открыты, тогда «Метод» «Преобразование растяжения / высоты звука» в его свойствах будет\nавтоматически изменяется на «Нет», и нажмите «ОК». Возьмите эффект. В это время вы обнаружите, что операции настройки + и-клавиши на клавиатуре недействительны. В это время вы должны повторно открыть свойства звукового события,\nустановить «Метод» «Преобразование растяжения / высоты звука» на «élastique», вам не нужно устанавливать «Изменение высоты тона», просто нажмите «ОК».",
				read_config_fail_exception = "Ошибка: не удалось прочитать файл конфигурации параметров.\n\nК сожалению, вы столкнулись с этой непредвиденной ошибкой. Мы очистим пользовательские настройки конфигурации и восстановим их до настроек по умолчанию, чтобы решить проблему.\nРекомендуется сообщить автору об этой ошибке, чтобы быстро решить проблему.\nЭтот скрипт будет закрыт, и я побеспокою вас, чтобы вы снова открыли его вручную.",
				fail_to_select_clips_exception = "Ошибка: ошибка при выборе клипов дорожки.\n\nПожалуйста, сначала выберите несколько дорожек клипов в окне дорожек.",
				fail_to_select_tracks_exception = "Ошибка: ошибка при выборе треков.\n\nПожалуйста, сначала выберите несколько видеодорожек в окне дорожек.",
				ytp_over_length_exception = "Ошибка: указанная минимальная длина YTP превышает длину носителя.\n\nУказанная минимальная длина YTP слишком велика, попробуйте меньшее значение. Или длина выбранного носителя слишком мала.",
				ytp_in_media_generator_exception = "Ошибка: примените YTP к носителю, созданному генератором мультимедиа.\n\nПриложение YTP должно использовать локальные медиа-файлы, а не медиа, созданные медиа-генератором.",
				ytp_eliminate_duplicates_finally_null_exception = "Технические Аномалии: для удаления повторяющихся значений в списке материалов YTP последний список пуст! \n\nЭто ошибка, которая не должна была произойти.",
				unknown_exception = "Ошибка: неизвестная ошибка.\n\nВключите подробную информацию, чтобы просмотреть содержимое конкретной ошибки, и отправьте сообщение об ошибке автору.",
				use_pic_in_pic_on_unsupported_vegas_exception = "Ошибка: Использование эффекта плагина «Картинка в картинке» в более ранних версиях Vegas не поддерживается.\n\nОсновная причина: Vegas добавила некоторые новые функции и параметры в новый подключаемый модуль эффекта «картинка в картинке», которые нельзя использовать в более ранних версиях подключаемого модуля Vegas.\n\nРешение: Текущая версия Vegas не может использовать визуальные эффекты этого ритма PV (например, аспекты расширения), пожалуйста, используйте другие визуальные эффекты. Или обновите программное обеспечение Vegas."
			};
		}
	}
	#endregion
}
