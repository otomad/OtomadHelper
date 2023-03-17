using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;

namespace Otomad.VegasScript.OtomadHelper.V4 {
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
}
