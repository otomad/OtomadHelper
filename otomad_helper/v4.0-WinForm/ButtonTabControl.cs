using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

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
		private List<RadioButton> radioButtons = new List<RadioButton>(0);

		public ButtonTabControl() {
			// 默认设置一个美观的初始大小
			Size = new Size(400, 300);
		}

		// 暴露给外部的属性：在按钮栏开头处显示的标题
		/// <summary>
		/// 在选项卡按钮开头处显示的标题文本，为空则自动隐藏。
		/// </summary>
		[Category("Appearance"), Description("在选项卡按钮开头处显示的标题文本，为空则自动隐藏。"), DefaultValue("")]
		public string HeaderTitle {
			get { return headerTitle; }
			set {
				// 运行期同步更新 Label
				headerTitle = value;
				if (titleLabel == null) return;
				titleLabel.Text = value;
				titleLabel.Visible = !string.IsNullOrEmpty(value);
			}
		}

		#region 公开控制方法：启用/禁用特定 Tab
		/// <summary>
		/// 根据 TabPage 对象控制该标签是否禁用
		/// </summary>
		public void SetEnabled(TabPage page, bool enabled) {
			if (page == null) return;

			// 1. 同步更新运行时的 RadioButton 状态
			RadioButton radio = radioButtons.FirstOrDefault(r => r != null && r.Tag == page);
			if (radio != null) radio.Enabled = enabled;

			// 2. 如果当前选中的页面正好被禁用了，自动跳到下一个可用的页面
			if (!enabled && SelectedTab == page) {
				SelectNextAvailableTab();
			}
		}

		/// <summary>
		/// 根据索引控制该标签是否禁用
		/// </summary>
		public void SetEnabled(int index, bool enabled) {
			if (index >= 0 && index < TabCount)
				SetEnabled(TabPages[index], enabled);
		}

		/// <summary>
		/// 检查某个 TabPage 是否处于启用状态
		/// </summary>
		public bool GetEnabled(TabPage page) {
			if (page == null) return false;
			RadioButton radio = radioButtons.FirstOrDefault(r => r != null && r.Tag == page);
			if (radio != null) return radio.Enabled;
			return false;
		}

		/// <summary>
		/// 外部修改了 TabPage 的 Text 或 ToolTipText 后，可调用此方法手动触发立即同步刷新
		/// </summary>
		public void RefreshPageDisplay(TabPage page) {
			if (page == null || buttonPanel == null) return;
			SyncButtonTextAndToolTip(page);
		}

		private void SelectNextAvailableTab() {
			for (int i = 0; i < TabCount; i++) {
				if (GetEnabled(TabPages[i])) {
					SelectedIndex = i;
					return;
				}
			}
		}

		#endregion

		// 防止通过快捷键或外部代码强行切换到已禁用的页面
		protected override void OnSelecting(TabControlCancelEventArgs e) {
			if (e.TabPage != null && !GetEnabled(e.TabPage) && !DesignMode) {
				e.Cancel = true; // 直接拦截核心切换事件
				return;
			}
			base.OnSelecting(e);
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

		// 核心魔法 2：换到这个生命周期钩子！此时控件属性及子页面已全部由设计器组装完毕
		protected override void OnHandleCreated(EventArgs e) {
			base.OnHandleCreated(e);

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

			// 捕捉并记录外部多层父容器可能存在的滚动条位置
			Dictionary<ScrollableControl, Point> parentScrollPositions = new Dictionary<ScrollableControl, Point>();
			Control currentParent = originalParent;
			while (currentParent != null) {
				ScrollableControl sc = currentParent as ScrollableControl;
				if (sc != null && sc.AutoScroll) {
					parentScrollPositions[sc] = sc.AutoScrollPosition;
				}
				currentParent = currentParent.Parent;
			}

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
				AutoSize = true,
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
				WrapContents = true, // 开启多行换行排列
				AutoScroll = false, // 关闭单行滚动
				AutoSize = true, // 开启自适应高度
				Dock = DockStyle.Top,
				Margin = new Padding(0),
				Padding = new Padding(2),
			};

			// 7. 在 FlowLayoutPanel 最前面添加标题 Label
			titleLabel = new Label {
				Text = HeaderTitle,
				AutoSize = true,
				Anchor = AnchorStyles.None, // 垂直居中对齐
				Font = new Font(originalParent.Font, FontStyle.Bold),
				TextAlign = ContentAlignment.MiddleLeft,
				Margin = new Padding(2, 4, 4, 4), // 适当的边距让它更好看
				Visible = !string.IsNullOrEmpty(HeaderTitle),
			};
			buttonPanel.Controls.Add(titleLabel);

			// 8. 动态生成对应的系统原生按钮
			radioButtons = new List<RadioButton>(TabCount);
			for (int i = 0; i < TabCount; i++) {
				TabPage page = TabPages[i];

				RadioButton radio = new RadioButton {
					Appearance = System.Windows.Forms.Appearance.Button,
					Text = page.Text,
					Tag = page, // 此时将 Tag 直接绑定到 TabPage 对象本身，方便后续动态找寻
					Height = 28,
					AutoSize = true,
					TextAlign = ContentAlignment.MiddleCenter,
					Margin = new Padding(2),
				};

				if (!string.IsNullOrEmpty(page.ToolTipText))
					toolTipProvider.SetToolTip(radio, page.ToolTipText);

				// 按钮被勾选时 -> 切页面
				radio.CheckedChanged += (s, e) => {
					if (isSyncing) return;
					RadioButton btn = s as RadioButton;
					if (btn != null && btn.Checked && btn.Tag is TabPage) {
						// 锁定并保持父容器的滚动条位置，彻底根治切换Tab时的抖动
						LockAndExecuteWithNoScroll(parentScrollPositions, () => {
							isSyncing = true;
							SelectedTab = (TabPage)btn.Tag;
							isSyncing = false;
						});
					}
				};

				buttonPanel.Controls.Add(radio);
				radioButtons.Add(radio);

				// 绑定 TabPage 的属性改变事件，实现 Text 和 ToolTipText 的单向多语言同步
				page.TextChanged += (s, e) => {
					if (s is TabPage) SyncButtonTextAndToolTip((TabPage)s);
				};
			}

			// 9. 监听 TabControl 页面改变 -> 同步高亮单选按钮
			SelectedIndexChanged += (s, e) => {
				if (isSyncing) return;
				LockAndExecuteWithNoScroll(parentScrollPositions, () => {
					isSyncing = true;
					UpdateSelectedButton();
					isSyncing = false;
				});
			};

			// 10. 组装：将按钮栏和 TabControl 放入包装容器中
			Dock = DockStyle.Top; // 强行让 TabControl 铺满容器剩余部分
			wrapperPanel.Controls.Add(this); // 此时移入新容器引发的 OnInitLayout 会直接在开头被 isInitialized 拦截
			wrapperPanel.Controls.Add(buttonPanel); // 按照 Dock=Top 紧贴在最上方

			// 11. 默认勾选当前页按钮
			isSyncing = true;
			UpdateSelectedButton();
			isSyncing = false;

			// 恢复原父容器的布局计算并强制刷新
			originalParent.ResumeLayout(true);

			// 完美恢复初始化时所有祖先容器的滚动条原位置
			foreach (KeyValuePair<ScrollableControl, Point> kvp in parentScrollPositions) {
				kvp.Key.AutoScrollPosition = new Point(Math.Abs(kvp.Value.X), Math.Abs(kvp.Value.Y));
			}
		}

		// 核心辅助机制 1：无滚动强制执行区（确保切换时外部容器滚动条纹丝不动）
		private void LockAndExecuteWithNoScroll(Dictionary<ScrollableControl, Point> scrollPositions, Action action) {
			// 1. 刷新和快照当前的实际滚动位置
			Dictionary<ScrollableControl, Point> currentPositions = new Dictionary<ScrollableControl, Point>();
			foreach (ScrollableControl key in scrollPositions.Keys) {
				currentPositions[key] = key.AutoScrollPosition;
				SuspendDrawing(key);
			}
			// 2. 执行核心页面切换动作
			action();
			// 3. 强行还原位置，消除抖动
			foreach (KeyValuePair<ScrollableControl, Point> kvp in currentPositions) {
				kvp.Key.AutoScrollPosition = new Point(Math.Abs(kvp.Value.X), Math.Abs(kvp.Value.Y));
				ResumeDrawing(kvp.Key);
			}
		}

		[DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
		internal const int WM_SETREDRAW = 0xB;

		/// <summary>
		/// 暂停刷新。
		/// </summary>
		public static void SuspendDrawing(Control control) {
			SendMessage(control.Handle, WM_SETREDRAW, false, 0);
		}

		/// <summary>
		/// 恢复刷新。
		/// </summary>
		public static void ResumeDrawing(Control control) {
			SendMessage(control.Handle, WM_SETREDRAW, true, 0);
			control.Refresh(); // 必须调用一次 Refresh，否则界面会停留在空白或残影状态
		}

		// 核心修复 2：同步更新文本与 ToolTip 的底层方法
		// 由于 TabPage 没有 ToolTipTextChanged 事件，此时约定翻译时必须先更新 ToolTipText 再更新 Text。
		private void SyncButtonTextAndToolTip(TabPage page) {
			foreach (RadioButton radio in radioButtons) {
				if (radio != null && radio.Tag == page) {
					radio.Text = page.Text;
					// 重新绑定最新的 ToolTip 提示
					toolTipProvider.SetToolTip(radio.Parent is Panel ? radio.Parent : radio, string.IsNullOrEmpty(page.ToolTipText) ? null : page.ToolTipText);
					break;
				}
			}
		}

		// 针对没有自带事件的属性（如 ToolTipText）提供一个全局自动捕捉机制
		// 每当页面发生实际切换或重绘时，对当前活跃页和历史页的属性重新对齐刷新
		private void UpdateSelectedButton() {
			foreach (RadioButton radio in radioButtons) {
				if (radio != null && radio.Tag == SelectedTab) {
					radio.Checked = true;
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
