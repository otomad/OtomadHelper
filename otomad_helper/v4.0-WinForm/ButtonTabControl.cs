using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	/// <summary>
	/// 你说得对，但是带有 <see cref="TabControl.Appearance" /> = <see cref="TabAppearance.Buttons" /> 的 <see cref="TabControl" /> 真的很丑。
	/// </summary>
	[ToolboxItem(true)]
	public class ButtonTabControl : TabControl {
		private Panel wrapperPanel;
		private FlowLayoutPanel buttonPanel;
		private Label titleLabel;
		private ToolTip toolTipProvider;
		private bool isSyncing = false; // 防止双向绑定时陷入死循环的旗标
		private string headerTitle = string.Empty;
		private bool isInitialized = false; // 引入防重复执行旗标

		public ButtonTabControl() {
			// 默认设置一个美观的初始大小
			Size = new Size(400, 300);
		}

		// 暴露给外部的属性：在按钮栏开头处显示的标题
		[Category("Appearance"), Description("在选项卡按钮开头处显示的标题文本，为空则自动隐藏"), DefaultValue("")]
		public string HeaderTitle {
			get { return headerTitle; }
			set {
				// 运行期同步更新 Label
				titleLabel.Text = headerTitle = value;
				titleLabel.Visible = !string.IsNullOrEmpty(value);
			}
		}

		// 核心魔法 1：拦截 Windows 消息
		// 运行期：彻底剥离原生标签栏，将其内容区域扩大到整个控件
		// 设计期：放行消息，让开发者可以正常开发
		protected override void WndProc(ref Message m) {
			// 0x1328 = TCM_ADJUSTRECT
			if (m.Msg == 0x1328 && !DesignMode) {
				m.Result = (IntPtr)1;
				return;
			}
			if ((m.Msg == 0x130E || m.Msg == 0x132C) && !DesignMode) {
				return;
			}
			base.WndProc(ref m);
		}

		// 核心魔法 2：运行期初始化（此时它的父窗体和位置、网格属性均已确定）
		protected override void InitLayout() {
			base.InitLayout();

			// 如果是设计模式，什么都不做，保持原生 TabControl 的外观供开发使用
			if (DesignMode) return;

			InitializeRuntimeComponents();
		}

		private void InitializeRuntimeComponents() {
			// 如果已经初始化过，直接拦截，彻底打破死循环
			if (isInitialized) return;
			isInitialized = true;

			Control originalParent = Parent;
			if (originalParent == null) return;

			// 1. 保存原控件的所有布局和容器属性
			int myIndex = originalParent.Controls.GetChildIndex(this);
			Point origLocation = Location;
			Size origSize = Size;
			DockStyle origDock = Dock;
			AnchorStyles origAnchor = Anchor;
			int origTabIndex = TabIndex;

			// 尝试获取 TableLayoutPanel 的网格布局属性
			TableLayoutPanelCellPosition cellPos = new TableLayoutPanelCellPosition(-1, -1);
			int columnSpan = 1, rowSpan = 1;
			if (originalParent is TableLayoutPanel) {
				TableLayoutPanel tlp = (TableLayoutPanel)originalParent;
				cellPos = tlp.GetPositionFromControl(this);
				columnSpan = tlp.GetColumnSpan(this);
				rowSpan = tlp.GetRowSpan(this);
			}

			// 2. 从原父容器中移除当前 TabControl，准备塞入包装容器中
			originalParent.Controls.Remove(this);

			// 3. 创建运行时的包装容器 Wrapper Panel，全盘接收原 TabControl 的布局属性
			wrapperPanel = new Panel {
				Location = origLocation,
				Size = origSize,
				Dock = origDock,
				Anchor = origAnchor,
				TabIndex = origTabIndex,
			};

			// 4. 将包装容器放入原父容器的“原位置”
			originalParent.Controls.Add(wrapperPanel);
			originalParent.Controls.SetChildIndex(wrapperPanel, myIndex);

			// 如果是在 TableLayoutPanel 中，恢复其网格和跨度属性
			if (originalParent is TableLayoutPanel && cellPos.Column != -1 && cellPos.Row != -1) {
				TableLayoutPanel targetTlp = (TableLayoutPanel)originalParent;
				targetTlp.SetCellPosition(wrapperPanel, cellPos);
				targetTlp.SetColumnSpan(wrapperPanel, columnSpan);
				targetTlp.SetRowSpan(wrapperPanel, rowSpan);
			}

			// 5. 初始化 ToolTip 组件
			toolTipProvider = new ToolTip();

			// 6. 创建支持多行换行、自适应高度的 FlowLayoutPanel
			buttonPanel = new FlowLayoutPanel {
				FlowDirection = FlowDirection.LeftToRight,
				WrapContents = true,  // 开启多行换行排列
				AutoScroll = false,   // 关闭单行滚动
				AutoSize = true,      // 开启自适应高度
				Dock = DockStyle.Top,
				Margin = new Padding(0),
				Padding = new Padding(2),
			};

			// 7. 在 FlowLayoutPanel 最前面添加标题 Label
			titleLabel = new Label {
				Text = HeaderTitle,
				AutoSize = true,
				Anchor = AnchorStyles.None, // 垂直居中对齐
				Font = new Font(Font, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleLeft,
				Margin = new Padding(4, 8, 8, 4), // 适当的边距让它更好看
				Visible = !string.IsNullOrEmpty(HeaderTitle)
			};
			buttonPanel.Controls.Add(titleLabel);

			// 8. 动态生成对应的系统原生按钮
			for (int i = 0; i < TabCount; i++) {
				TabPage page = TabPages[i];

				RadioButton rdoBtn = new RadioButton {
					Appearance = System.Windows.Forms.Appearance.Button,
					Text = page.Text,
					Tag = page, // 此时将 Tag 直接绑定到 TabPage 对象本身，方便后续动态找寻
					Height = 28,
					AutoSize = true,
					TextAlign = ContentAlignment.MiddleCenter,
					Margin = new Padding(2)
				};

				if (!string.IsNullOrEmpty(page.ToolTipText))
					toolTipProvider.SetToolTip(rdoBtn, page.ToolTipText);

				// 按钮被勾选时 -> 切页面
				rdoBtn.CheckedChanged += (s, e) => {
					if (isSyncing) return;
					RadioButton btn = s as RadioButton;
					if (btn != null && btn.Checked && btn.Tag is TabPage) {
						isSyncing = true;
						SelectedTab = (TabPage)btn.Tag;
						isSyncing = false;
					}
				};

				buttonPanel.Controls.Add(rdoBtn);

				// 绑定 TabPage 的属性改变事件，实现 Text 和 ToolTipText 的单向多语言同步
				page.TextChanged += (s, e) => {
					if (s is TabPage)
						SyncButtonTextAndToolTip((TabPage)s);
				};
			}

			// 9. 监听 TabControl 页面改变 -> 同步高亮单选按钮
			SelectedIndexChanged += (s, e) => {
				if (isSyncing)
					return;
				isSyncing = true;
				UpdateSelectedButton();
				isSyncing = false;
			};

			// 10. 组装：将按钮栏和 TabControl 放入包装容器中
			Dock = DockStyle.Fill; // 强行让 TabControl 铺满容器剩余部分
			wrapperPanel.Controls.Add(this); // 此时移入新容器引发的 OnInitLayout 会直接在开头被 isInitialized 拦截
			wrapperPanel.Controls.Add(buttonPanel); // 按照 Dock=Top 紧贴在最上方

			// 11. 默认勾选当前页按钮
			isSyncing = true;
			UpdateSelectedButton();
			isSyncing = false;

			// 恢复原父容器的布局计算并强制刷新
			originalParent.ResumeLayout(true);
		}

		// 辅助方法：多语言切换时，单向同步更新按钮上的文本和提示
		private void SyncButtonTextAndToolTip(TabPage page) {
			if (buttonPanel == null) return;

			foreach (Control ctrl in buttonPanel.Controls) {
				RadioButton btn = ctrl as RadioButton;
				if (btn != null && btn.Tag == page) {
					btn.Text = page.Text;
					toolTipProvider.SetToolTip(btn, page.ToolTipText);
					break;
				}
			}
		}

		// 辅助方法：页面变化时勾选对应按钮
		private void UpdateSelectedButton() {
			if (buttonPanel == null || SelectedTab == null) return;

			foreach (Control ctrl in buttonPanel.Controls) {
				RadioButton btn = ctrl as RadioButton;
				if (btn != null && btn.Tag == SelectedTab) {
					btn.Checked = true;
					break;
				}
			}
		}

		// 释放资源
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (toolTipProvider != null) toolTipProvider.Dispose();
				if (buttonPanel != null) buttonPanel.Dispose();
				if (wrapperPanel != null) wrapperPanel.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
