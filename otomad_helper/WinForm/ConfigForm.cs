using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using ScriptPortal.Vegas;

namespace Otomad.VegasScript.OtomadHelper.V4 {

	public partial class ConfigForm : Form {

		public bool AcceptConfig = false;
		public static Icon icon;
		#if VEGAS_ENVIRONMENT
		// public IniFile configIni { get { return parent.configIni; } set { parent.configIni = value; } }
		// public readonly EntryPoint parent;
		// private Vegas vegas { get { return parent.vegas; } }
		#endif

		/// <summary>
		/// ConfigForm 脚本对话框窗体的入口方法。
		/// </summary>
		/// <param name="entryPoint">调用本对象的父对象，也就是 Vegas 脚本的入口类</param>
		public ConfigForm(/* EntryPoint entryPoint */) {
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
			
			#region 菜单选项
			saveConfigToolStripMenuItem.Click += (sender, e) => { SaveIni(); };
			resetConfigToolStripMenuItem.Click += (sender, e) => {
				// configIni.Delete(true);
				Close();
				MessageBox.Show("重置完成，请重新启动脚本。", "重置用户配置", MessageBoxButtons.OK, MessageBoxIcon.Information);
			};
			exitToolStripMenuItem.Click += new EventHandler(CancelBtn_Click);
			aboutToolStripMenuItem.Click += new EventHandler(AboutBtn_Click);
			//userHelpToolStripMenuItem.Click += (sender, e) => { OpenLink(aboutHelpLink); };
			troubleShootingToolStripMenuItem.Click += (sender, e) => { OpenLink(troubleShootingLink); };
			//updateInfoToolStripMenuItem.Click += (sender, e) => { OpenLink(updateInfoLink); };
			githubToolStripMenuItem.Click += (sender, e) => { OpenLink(githubLink); };
			GetSystemLanguage();
			TrackLegatoMenu.Renderer = menu.Renderer = new Windows10StyledContextMenuStripRenderer();
			#endregion

			#region 复选框设置、下拉菜单默认值
			EventHandler setCheckedEnabled = new EventHandler(SetCheckedEnabled);
			VideoConfigCheck.CheckedChanged += setCheckedEnabled;
			AudioConfigCheck.CheckedChanged += setCheckedEnabled;
			StaffVisualizerConfigCheck.CheckedChanged += setCheckedEnabled;
			StaffGenerateLinesCheck.CheckedChanged += setCheckedEnabled;
			AudioTuneMethodCombo.SelectedIndexChanged += setCheckedEnabled;
			VideoEffectCombo.SelectedIndexChanged += setCheckedEnabled;
			Tabs.SelectedIndexChanged += setCheckedEnabled;
			AudioMainKeyCombo.MouseWheel += AudioMainKeyCombo_MouseWheel;
			for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++)
				YtpEffectsCheckList.SetItemChecked(i, true);
			#if VEGAS_ENVIRONMENT
			string configIniName = "otomad_helper.ini";
			configIni = new IniFile(Path.r(vegas.GetApplicationDataPath(Environment.SpecialFolder.ApplicationData), configIniName).FullPath, this);
			ReadIni();
			#else
			VideoEffectLbl.Click += (sender, e) => {
				StaffVisualizerConfigCheck.Checked = !StaffVisualizerConfigCheck.Checked;
				bool isStaff = StaffVisualizerConfigCheck.Checked;
				VideoEffectInitialValueCombo.Visible = VideoEffectInitialValueLbl.Visible = !isStaff;
				VideoEffectCombo.DropDownStyle = isStaff ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
				if (isStaff) VideoEffectCombo.Text = "五线谱";
				VideoEffectCombo.Enabled = !isStaff;
			};
			#endif
			SourceConfigGroup.AllowDrop = MidiConfigGroup.AllowDrop = true;
			#endregion

			#region 优化界面颜色和外观
			List<Label> labels = new List<Label>();
			int maxSheetTabLabelWidth = 0;
			foreach (Control control_i in SheetTab.Controls)
				if (control_i is GroupBox)
					foreach (Control control_j in control_i.Controls)
						if (control_j is TableLayoutPanel)
							foreach (Control control_k in control_j.Controls)
								if (control_k is Label) {
									Label label = control_k as Label;
									labels.Add(label);
									if (label.Width > maxSheetTabLabelWidth)
										maxSheetTabLabelWidth = label.Width;
								}
			foreach (Label label in labels) {
				Size minSize = label.MinimumSize;
				minSize.Width = maxSheetTabLabelWidth;
				label.MinimumSize = minSize;
			}
			YtpEffectsCheckList.MinimumSize = new Size(0, YtpEnableAllEffectsCheck.Height * 5);
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
			#endregion

			/*int trueValue = 0x01, falseValue = 0x00;
			SetWindowTheme(this.Handle, "DarkMode_Explorer", null);
			DwmSetWindowAttribute(this.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
			DwmSetWindowAttribute(this.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));*/
		}

		[DllImport("uxtheme.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]

		public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
		[DllImport("dwmapi.dll")]
		public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
		[Flags]
		public enum DwmWindowAttribute : uint {
			DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
			DWMWA_MICA_EFFECT = 1029
		}

		/*protected override void OnHandleCreated(EventArgs e) {
			// Use e.g. Color.FromArgb(128, Color.Lime) for a 50% opacity green tint.
			WindowUtils.EnableAcrylic(this, Color.Transparent);

			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.Clear(Color.Transparent);
		}*/

		/*private void Window_ContentRendered(object sender, System.EventArgs e) {
			// Apply Mica brush
			UpdateStyleAttributes((HwndSource)sender);
		}

		public static void UpdateStyleAttributes(HwndSource hwnd) {
			int trueValue = 0x01;
			DwmSetWindowAttribute(hwnd.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
		}

		private void Window_Loaded(object sender, EventArgs e) {
			// Get PresentationSource
			PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);

			// Subscribe to PresentationSource's ContentRendered event
			presentationSource.ContentRendered += Window_ContentRendered;
		}*/

		private void ThemeAllControls(Control parent = null) {
			parent = parent ?? this;
			Func<Color, Color> InvertColor = new Func<Color, Color>(orig => {
				int r = 255 - orig.R, g = 255 - orig.G, b = 255 - orig.B;
				return Color.FromArgb(r, g, b);
			});
			Action<Control> Theme = new Action<Control>(control => {
				int trueValue = 0x01, falseValue = 0x00;
				SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
				DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
				DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
				control.BackColor = Color.Black;
				control.ForeColor = Color.White;
			});
			if (parent == this) Theme(this);
			foreach (Control control in parent.Controls) {
				Theme(control);
				if (control.Controls.Count != 0)
					ThemeAllControls(control);
			}
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
				VideoFreezeLastFrameCheck.Checked = configIni.ReadBool("FreezeLastFrame", false);
				VideoLegatoCheck.Checked = configIni.ReadBool("Legato", false);
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
			
				#region 个性化配置
				configIni.StartSection("Personalize");
				Language = configIni.Read("Language", "");
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
			configIni.Write("FreezeLastFrame", VideoFreezeLastFrameCheck.Checked);
			configIni.Write("Legato", VideoLegatoCheck.Checked);
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
			
			#region 个性化配置
			configIni.StartSection("Personalize");
			configIni.Write("Language", Language);
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
		
		private string GetSystemLanguage() {
			string lang = System.Globalization.CultureInfo.InstalledUICulture.Name;
			Language = lang;
			return lang;
		}
		
		private string Language {
			get {
				if (chineseToolStripMenuItem.Checked) return "zh";
				else if (japaneseToolStripMenuItem.Checked) return "ja";
				else if (russianToolStripMenuItem.Checked) return "ru";
				else return "en";
			}
			set {
				if (value == "") return;
				string lang = value.ToLower();
				if (lang.StartsWith("zh")) chineseToolStripMenuItem.Checked = true;
				else if (lang.StartsWith("ja")) japaneseToolStripMenuItem.Checked = true;
				else if (lang.StartsWith("ru")) russianToolStripMenuItem.Checked = true;
				else englishToolStripMenuItem.Checked = true;
			}
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
				//VideoLegatoCheck.Checked = false;
			}
			if (!StaffGenerateLinesCheck.Enabled) StaffGenerateLinesCheck.Checked = false;
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
				if (!VideoScratchCheck.Enabled && VideoConfigCheck.Enabled) VideoScratchCheck.Checked = false;
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
				//VideoLegatoCheck.Enabled =
				VideoScratchCheck.Checked = VideoScratchCheck.Enabled = false;
			SetEnabled(SheetTab, isSheetConfigOn, new Control[] { StaffVisualizerConfigCheck, SheetConfigInfoLabel });
			if (!StaffGenerateLinesCheck.Checked)
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
		public const string troubleShootingLink = "https://www.bilibili.com/read/cv495309";
		public const string updateInfoLink = "http://www.bilibili.com/read/cv13335178";
		public const string githubLink = "https://github.com/otomad/Otomad.VegasScript.OtomadHelper.V4s";
		private void UserHelpLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			OpenLink(aboutHelpLink);
		}
		
		public void OpenLink(string link) {
			System.Diagnostics.Process.Start("explorer.exe", link);
		}

		private void AboutBtn_Click(object sender, EventArgs e) {
			MessageBox.Show(
				"脚本作者：淅琳雨\n" +
				"说明文档：" + updateInfoLink + "\n" +
				"仓库地址：" + githubLink + "\n\n" +
				"脚本原作者：Chaosinism\n" +
				"说明文档：" + aboutHelpLink + "\n" +
				"疑难解答：" + troubleShootingLink + "\n" +
				"仓库地址：https://github.com/Chaosinism/vegas_scripts\n",
				"关于",
				MessageBoxButtons.OK, MessageBoxIcon.Information);
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

		private void QuickSelectIntervalBtn_Click(object sender, EventArgs e) {
			CancelBtn_Click(sender, null);
			new SelectIntervalForm().Show();
			#if VEGAS_ENVIRONMENT
			parent.SelectInterval((int)SelectOneEveryFewBox.Value, (int)SelectWhichEachGroupBox.Value);
			#endif
		}

		/*private void SelectOneEveryFewBox_ValueChanged(object sender, EventArgs e) {
			int divisor = (int)SelectOneEveryFewBox.Value, remainder = (int)SelectWhichEachGroupBox.Value;
			if (remainder > divisor) SelectWhichEachGroupBox.Value = remainder = divisor;
			SelectWhichEachGroupBox.Maximum = new decimal(new int[] { divisor, 0, 0, 0 });
		}

		internal void DisabledSelectIntervalPart() {
			SelectOneEveryFewBox.Enabled = SelectWhichEachGroupBox.Enabled = QuickSelectIntervalBtn.Enabled = false;
			HelperTab.Parent = null; // 隐藏选项卡
		}*/

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

		private void AudioMainKeyCombo_MouseWheel(object sender, MouseEventArgs e) {

		}

		private void AudioLegatoCheck_Or_AudioFreezeLastFrameCheck_CheckedChanged(object sender, EventArgs e) {
			CheckBox check = sender as CheckBox;
			CheckBox[] checks = { /*AudioLegatoCheck,*/ AudioFreezeLastFrameCheck };
			foreach (CheckBox checkBox in checks) {
				if (checkBox == check) continue;
				checkBox.Checked = false;
			}
		}

		private void ClearTrackMotionBtn_Click(object sender, EventArgs e) {
			ClearTrackMotionBtn.Enabled = false;
		}

		private void ClearTrackEffectBtn_Click(object sender, EventArgs e) {
			ClearTrackEffectBtn.Enabled = false;
		}

		private void PreviewAudioBtn_Click(object sender, EventArgs e) {
			Console.Beep();
		}

		private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e) {
			AcceptConfig = false;
		}

		private void PreviewBasePitchBtn_MouseDown(object sender, MouseEventArgs e) {
			Console.Beep();
		}

		private void exitDiscardingChangesToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void resetConfigToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void YtpLenBox_ValueChanged(object sender, EventArgs e) {
			NumericUpDown min = YtpMinLenBox, max = YtpMaxLenBox, cur = sender as NumericUpDown;
			if (min.Value > max.Value) {
				if (cur == min) min.Value = max.Value;
				else max.Value = min.Value;
			}
		}

		private void ConfigForm_MouseClick(object sender, MouseEventArgs e) {

		}

		private void YtpEnableAllEffectsCheck_CheckedChanged(object sender, EventArgs e) {
			CheckBox check = sender as CheckBox;
			if (check.CheckState != CheckState.Indeterminate)
				for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++)
					YtpEffectsCheckList.SetItemChecked(i, check.Checked);
		}

		int[] selectedYtpEffects;
		private void YtpEffectsCheckList_SelectedIndexChanged(object sender, EventArgs e) {
			CheckState? state = null;
			List<int> selected = new List<int>();
			for (int i = 0; i < YtpEffectsCheckList.Items.Count; i++) {
				bool isChecked = YtpEffectsCheckList.GetItemChecked(i);
				if (isChecked) selected.Add(i);
				if (isChecked && (state == null || state == CheckState.Checked)) state = CheckState.Checked;
				else if (!isChecked && (state == null || state == CheckState.Unchecked)) state = CheckState.Unchecked;
				else state = CheckState.Indeterminate;
			}
			selectedYtpEffects = selected.ToArray();
			if (state == null) return;
			YtpEnableAllEffectsCheck.CheckState = (CheckState)state;
		}

		private void ConfigForm_Resize(object sender, EventArgs e) {
			int tabWidth = Tabs.SelectedTab.Size.Width;
			const int MARGIN = 6;
			int maxWidth = tabWidth - MARGIN;
			SheetConfigInfoLabel.MaximumSize = YtpLbl.MaximumSize = new Size(maxWidth, 0);

			// 处理 FlowLayoutPanel 的换行问题。
			AudioTuneTablePanel.RowStyles[AudioTuneTablePanel.GetRow(AudioPreviewAttrLayoutPanel)] = new RowStyle();
			AudioTuneTablePanel.RowStyles[AudioTuneTablePanel.GetRow(AudioPreviewAttrLayoutPanel)] = new RowStyle(SizeType.Absolute, AudioPreviewAttrLayoutPanel.Height);
			MidiConfigTablePanel.RowStyles[MidiConfigTablePanel.GetRow(MidiBpmFlowPanel)] = new RowStyle();
			MidiConfigTablePanel.RowStyles[MidiConfigTablePanel.GetRow(MidiBpmFlowPanel)] = new RowStyle(SizeType.Absolute, MidiBpmFlowPanel.Height);
		}

		private void PreviewBasePitchBtn_MouseDown(object sender, EventArgs e) {
			//const int MARGIN = 6;
			Close();
		}

		private void AudioTuneMethodCombo_SelectedIndexChanged(object sender, EventArgs e) {
			AudioStretchAttrCombo.Items.Clear();
		}

		private void WhyOkBtnIsDisabledToolStripMenuItem_Click(object sender, EventArgs e) {
			MessageBox.Show("sb");
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
			Type form;
			map.TryGetValue(btn, out form);
			if (form != null)
				(Activator.CreateInstance(form) as Form).ShowDialog();
			Close();
		}

		private void GenerateAtCustomText_Leave(object sender, EventArgs e) {
			GenerateAtCustomText.Text = "00000";
		}

		private void TrackLegatoMenuItems_Click(object sender, EventArgs e) {
			TrackLegatoBtn.Enabled = false;
		}

		/*[StructLayout(LayoutKind.Sequential)]
		public struct CompositionMargins {
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}

		[DllImport("dwmapi.dll", PreserveSig = false)]
		static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref CompositionMargins margins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		static extern bool DwmIsCompositionEnabled();

		protected override void OnLoad(EventArgs e) {
			if (DwmIsCompositionEnabled()) {
				CompositionMargins margins = new CompositionMargins();
				margins.Right = margins.Left = margins.Top = margins.Bottom = Width + Height;
				DwmExtendFrameIntoClientArea(Handle, ref margins);
			}
			base.OnLoad(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			base.OnPaintBackground(e);
			if (DwmIsCompositionEnabled())
				e.Graphics.Clear(Color.Black);
		}*/

		/*protected override void OnHandleCreated(EventArgs e) {
			// Use e.g. Color.FromArgb(128, Color.Lime) for a 50% opacity green tint.
			WindowUtils.EnableAcrylic(this, Color.Transparent);
			base.OnHandleCreated(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e) {
			e.Graphics.Clear(Color.Transparent);
		}*/

		[DllImport("dwmapi.dll")]
		private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);

		/*protected override void OnHandleCreated(EventArgs e) {
			DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4);
		}*/

		private void ChooseSourceBtn_Click(object sender, EventArgs e) {
			ChooseSourceCombo.SelectedIndex = 2;
		}

		private void ChooseMidiBtn_Click(object sender, EventArgs e) {
			ChooseSourceCombo.SelectedIndex = 2;
		}

		private void LoadPresetsToolStripMenuItem_Click(object sender, EventArgs e) {
			MessageBox.Show("确定添加预设？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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
			bool? receiveMedia = null;
			if (sender == MidiConfigGroup) receiveMedia = false;
			if (sender == SourceConfigGroup) receiveMedia = true;
			Console.WriteLine(e.GetFileNames()[0]);
			OnDragLeave(sender, null);
		}

		private void OnDragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.None;
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				string ext = Path.GetExtension(e.GetFileNames()[0]).ToLower();
				GroupBox group = sender as GroupBox;
				if (group == MidiConfigGroup && (ext == ".mid" || ext == ".midi") ||
					group == SourceConfigGroup) {
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
			OkBtn_Click(null, null);
		}

		private void RememberOnceFormSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			Console.WriteLine(1);
		}

		private void RestoreDefaultFormSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			Console.WriteLine(0);
		}

		private void MidiChannelAdvancedBtn_Click(object sender, EventArgs e) {
			new MidiChannelAdvancedForm(this).ShowDialog();
		}

		private void MidiChannelCombo_SelectedIndexChanged(object sender, EventArgs e) {
			Console.WriteLine("変わった！");
		}

		private void ExperimentalThemeToolStripMenuItem_Click(object sender, EventArgs e) {
			ThemeAllControls();
			ToolStripMenuItem me = experimentalThemeToolStripMenuItem;
			me.Checked = true;
			me.Enabled = false;
		}

		private void QuickConfigMidiChannelsToolStripMenuItem_Click(object sender, EventArgs e) {
			MidiChannelCombo_SelectedIndexChanged(null, null);
		}

		private void CheckUpdateToolStripMenuItem_Click(object sender, EventArgs e) {
			Console.WriteLine("更新");
		}

		private void EffectToSelectedEventsToolStripMenuItem_Click(object sender, EventArgs e) {
			ToolStripMenuItem menu = sender as ToolStripMenuItem, info = trackLegatoSelectInfoToolStripMenuItem;
			if (!menu.Checked) {
				info.Text = "已选中 0 个轨道。";
			} else {
				info.Text = "已选中 0 个轨道剪辑。";
			}
		}

		private void IncreaseSpacingToolStripMenuItem_Click(object sender, EventArgs e) {
			new IncreaseSpacingDialog().ShowDialog();
		}

		private bool isMousedown = false;
		private void MouseDownMapToClick(object sender, MouseEventArgs e) {
			Button button = sender as Button;
			if (button == null) return;
			if (e.Button == MouseButtons.Left) {
				button.PerformClick();
				isMousedown = true;
			}
		}
		private void MouseUpMapToClick(object sender, MouseEventArgs e) {
			isMousedown = false;
		}

		private void TrackLegatoBtn_Click(object sender, EventArgs e) {
			if (isMousedown) return;
			Button button = sender as Button;
			TrackLegatoMenu.Show(button, new Point(0, button.Height));
		}

		private void VideoParamsPresetsBtn_Click(object sender, EventArgs e) {
			Console.WriteLine("默认");
		}

		private void SetComboBoxToolTipWhenOverflowText(object sender, EventArgs e) {
			ComboBox comboBox = sender as ComboBox;
			string text = comboBox.SelectedItem.ToString();
			float iSize = comboBox.CreateGraphics().MeasureString(text, comboBox.Font).Width + comboBox.Height; // 去掉右边下拉图标的宽度
			if (iSize > comboBox.Width) OverflowToolTip.SetToolTip(comboBox, text);
			else OverflowToolTip.SetToolTip(comboBox, "");
		}
	}

	public static class Extensions {
		public static string[] GetFileNames(this DragEventArgs e) {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return null;
			return e.Data.GetData(DataFormats.FileDrop) as string[];
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
