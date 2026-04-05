using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	public partial class HorizontalDivider : Label {
		public HorizontalDivider() {
			InitializeComponent();
			base.Text = "";
			base.BorderStyle = BorderStyle.Fixed3D;
			base.AutoSize = false;
			base.MaximumSize = new Size(int.MaxValue, HEIGHT);
			base.Height = HEIGHT;
			Margin = new Padding(0, 3, 0, 3);
			Dock = DockStyle.Top;
		}

		private const int HEIGHT = 2;

		#region 隐藏属性
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DefaultValue("")]
		public new string Text {
			get { return base.Text; }
			set { base.Text = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DefaultValue(BorderStyle.Fixed3D)]
		public new BorderStyle BorderStyle {
			get { return base.BorderStyle; }
			set { base.BorderStyle = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DefaultValue(false)]
		public new bool AutoSize {
			get { return base.AutoSize; }
			set { base.AutoSize = value; }
		}

		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never), DefaultValue(HEIGHT)]
		public new int Height {
			get { return base.Height; }
			set { base.Height = value; }
		}
		#endregion
	}
}
