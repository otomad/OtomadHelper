using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	/// <summary>
	/// 阻止控件引发自动滚动的容器。
	/// </summary>
	public class NoAutoScrollPanel : Panel {
		/// <summary>
		/// 要阻止引发自动滚动行为的控件列表。<br />
		/// 如果为空，则会阻止所有子控件。
		/// </summary>
		public List<Control> NoAutoScrollControls { get; private set; }

		public NoAutoScrollPanel() {
			// 启用自动滚动
			AutoScroll = true;
			NoAutoScrollControls = new List<Control>();
		}

		/// <summary>
		/// 阻止容器将控件滚动到视野中的核心方法。
		/// </summary>
		/// <remarks>
		/// 当子控件获得焦点请求滚动时，返回当前视口相对于容器的真实坐标。<br />
		/// 这样系统会计算出“当前视口距离等于目标距离”，从而实现 0 像素位移。
		/// </remarks>
		protected override Point ScrollToControl(Control activeControl) {
			// 如果 NoAutoScrollControls 为空，则阻止所有子控件引发的自动滚动。
			if (NoAutoScrollControls.Count == 0)
				return AutoScrollPosition;
			else {
				// 仅阻止特定控件的行为。
				if (NoAutoScrollControls.Contains(activeControl))
					return AutoScrollPosition;
				return base.ScrollToControl(activeControl);
			}
		}
	}
}
