using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public static class Extensions {
		public static string[] GetFileNames(this DragEventArgs e) {
			if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return null;
			return e.Data.GetData(DataFormats.FileDrop) as string[];
		}
	}

	/// <summary>
	/// 视频的动画效果枚举。
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

	public class PvVisualEffect {
		public static Dictionary<string, PvVisualEffectType[]> Classes {
			get {
				return new Dictionary<string, PvVisualEffectType[]> {
					{ "翻转类", new PvVisualEffectType[] { PvVisualEffectType.H_FLIP, PvVisualEffectType.V_FLIP, PvVisualEffectType.CCW_FLIP, PvVisualEffectType.CW_FLIP } },
					{ "旋转类", new PvVisualEffectType[] { PvVisualEffectType.CCW_ROTATE, PvVisualEffectType.CW_ROTATE, PvVisualEffectType.TURNED } },
					{ "镜像类", new PvVisualEffectType[] { PvVisualEffectType.H_MIRROR, PvVisualEffectType.V_MIRROR, PvVisualEffectType.CCW_MIRROR, PvVisualEffectType.CW_MIRROR } },
					{ "反转类", new PvVisualEffectType[] { PvVisualEffectType.NEGATIVE, PvVisualEffectType.LUMIN_INVERT } },
					{ "色相类", new PvVisualEffectType[] { PvVisualEffectType.HUE_INVERT, PvVisualEffectType.STEP_3_CHANGE_HUE, PvVisualEffectType.STEP_4_CHANGE_HUE, PvVisualEffectType.STEP_5_CHANGE_HUE, PvVisualEffectType.STEP_6_CHANGE_HUE, PvVisualEffectType.STEP_7_CHANGE_HUE, PvVisualEffectType.STEP_8_CHANGE_HUE } },
					{ "单色类", new PvVisualEffectType[] { PvVisualEffectType.GREY } },
					{ "时间类", new PvVisualEffectType[] { PvVisualEffectType.PINGPONG, PvVisualEffectType.WHIRL } },
					{ "扩缩类", new PvVisualEffectType[] { PvVisualEffectType.VERTICAL_EXPANSION, PvVisualEffectType.VERTICAL_EXPANSION_WITH_REBOUND, PvVisualEffectType.VERTICAL_COMPRESSION, PvVisualEffectType.VERTICAL_COMPRESSION_WITH_REBOUND, PvVisualEffectType.OBLIQUE_EXPANSION_AND_COMPRESSION, PvVisualEffectType.PUYO_PUYO} },
					{ "摇摆类", new PvVisualEffectType[] { PvVisualEffectType.PENDULUM } },
					{ "模糊类", new PvVisualEffectType[] { PvVisualEffectType.GAUSSIAN_BLUR, PvVisualEffectType.RADIAL_BLUR } },
				};
			}
		}
	}

	public class SonarItem : ListViewItem {
		private readonly ConfigForm configForm;
		private ComboBox ShapeCombo { get { return configForm.SonarShapeCombo; } }
		private ListView SonarList { get { return configForm.SonarList; } }
		public SonarItem(
			ConfigForm configForm,
			string drumSound,
			int shape,
			Color color,
			double border,
			double size,
			double xPos,
			double yPos,
			double xOffset,
			double yOffset,
			double rotation,
			int curve,
			double fadeIn,
			double fadeOut
		) {
			this.configForm = configForm;
			int columnCount = SonarList.Columns.Count;
			while (SubItems.Count < columnCount)
				SubItems.Add("");

			DrumSound = drumSound;
			Shape = shape;
			Color = color;
			Border = border;
			Size = size;
			XPos = xPos;
			YPos = yPos;
			XOffset = xOffset;
			YOffset = yOffset;
			Rotation = rotation;
			Curve = curve;
			FadeIn = fadeIn;
			FadeOut = fadeOut;
		}
		public SonarItem(ConfigForm configForm) : this(configForm, "", 0, Color.White, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0) { }

		private string drumSound;
		private int shape;
		private Color color;
		private double border;
		private double size;
		private double xPos;
		private double yPos;
		private double xOffset;
		private double yOffset;
		private double rotation;
		private int curve;
		private double fadeIn;
		private double fadeOut;

		public string DrumSound {
			get { return drumSound; }
			set {
				drumSound = value;
				SubItems[0].Text = value;
			}
		}
		public int Shape {
			get { return shape; }
			set {
				if (value < 0 || value >= ShapeCombo.Items.Count)
					value = 0;
				shape = value;
				SubItems[1].Text = ShapeCombo.Items[value].ToString();
			}
		}
		public Color Color {
			get { return color; }
			set {
				color = value;
				BackColor = value;
				ForeColor = ColorButton.GetForeColor(value);
			}
		}
		public double Border { get { return border; } set { border = value; } }
		public double Size { get { return size; } set { size = value; } }
		public double XPos { get { return xPos; } set { xPos = value; } }
		public double YPos { get { return yPos; } set { yPos = value; } }
		public double XOffset { get { return xOffset; } set { xOffset = value; } }
		public double YOffset { get { return yOffset; } set { yOffset = value; } }
		public double Rotation { get { return rotation; } set { rotation = value; } }
		public int Curve { get { return curve; } set { curve = value; } }
		public double FadeIn { get { return fadeIn; } set { fadeIn = value; } }
		public double FadeOut { get { return fadeOut; } set { fadeOut = value; } }
	}
}
