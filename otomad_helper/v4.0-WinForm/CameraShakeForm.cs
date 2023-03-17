using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public partial class CameraShakeForm : Form {
		public CameraShakeForm() {
			InitializeComponent();
		}

		public Config GetConfig() {
			return new Config {
				SkewIn = (double)SkewInBox.Value,
				SkewXY = (double)SkewXYBox.Value,
				SkewOut = (float)SkewOutBox.Value,
				XToYRatio = (float)XToYBox.Value,
				ShouldResetPan = ShouldResetPanCheck.Checked,
				ShouldClearFrames = ShouldClearFramesCheck.Checked,
			};
		}

		public struct Config {
			//Rate of shake multiplier
			public double SkewIn;
			//Synchronicity factor between horizontal and vertical movements
			public double SkewXY;
			//Number of pixels to displace
			public float SkewOut;
			//Affect displacement horizontally as a proportion of vertical displacement
			public float XToYRatio;
			//Reset Pan on first frame
			public bool ShouldResetPan;
			//Start all over?
			public bool ShouldClearFrames;
		}
	}
}
