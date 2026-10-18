using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScripts.OtomadHelper.V4 {
	public class RememberedComboBox : ComboBox {
		// 内部使用的锁定模式枚举，不对外公开
		private enum LockMode {
			Unlocked = -1,
			LockedWithIndex,
			LockedWithItem
		}

		private bool isInternalChanging = false;
		private LockMode internalLockMode = LockMode.Unlocked;

		public RememberedComboBox() {
			// 当用户在未锁定状态下手动改变选择时，同步更新用户真实设定的值
			SelectedIndexChanged += (sender, e) => {
				if (internalLockMode == LockMode.Unlocked && !isInternalChanging) {
					userSelectedIndex = base.SelectedIndex;
					userSelectedItem = base.SelectedItem;
				}
			};
		}

		#region 用户原始值存储区（保证双向数据匹配）

		private int userSelectedIndex = -1;
		[Category("Behavior"), DefaultValue(-1), Description("获取或设置用户设定的选择索引。与 UserSelectedItem 保持同步匹配。")]
		public int UserSelectedIndex {
			get { return userSelectedIndex; }
			set {
				userSelectedIndex = value;
				if (internalLockMode == LockMode.Unlocked) {
					ApplyValueInternally(() => {
						base.SelectedIndex = value;
						userSelectedItem = base.SelectedItem; // 同步获取匹配的对象
					});
				}
			}
		}

		private object userSelectedItem = null;
		[Category("Behavior"), DefaultValue(null), Description("获取或设置用户设定的选择项。与 UserSelectedIndex 保持同步匹配。")]
		public object UserSelectedItem {
			get { return userSelectedItem; }
			set {
				userSelectedItem = value;
				if (internalLockMode == LockMode.Unlocked) {
					ApplyValueInternally(() => {
						base.SelectedItem = value;
						userSelectedIndex = base.SelectedIndex; // 同步获取匹配的索引
					});
				}
			}
		}

		#endregion

		#region 锁定状态控制与公开接口

		/// <summary>
		/// 获取或设置组件当前是否处于锁定状态。
		/// 支持读取 true/false，但仅支持写入 false 用于解锁，写入 true 会触发异常。
		/// </summary>
		[Category("Appearance"), DefaultValue(false), Description("获取组件当前是否处于锁定状态。也可以通过设为 false 来解锁。")]
		public bool Lock {
			get { return internalLockMode != LockMode.Unlocked; }
			set {
				if (value) {
					// 试图直接设为 true 时报错
					throw new InvalidOperationException("不能直接将 Lock 属性设置为 true。请通过设置 LockSelectedIndex 或 LockSelectedItem 来指定锁定的目标选项。");
				} else {
					// 显式设置为 false 时执行解锁
					Unlock();
				}
			}
		}

		private int lockSelectedIndex = -1;
		[Category("Behavior"), DefaultValue(-1), Description("设置该属性将立即锁定控件到指定的索引。设为 -1 会锁定到空选项。")]
		public int LockSelectedIndex {
			get { return lockSelectedIndex; }
			set {
				lockSelectedIndex = value;
				internalLockMode = LockMode.LockedWithIndex;
				Enabled = false;

				// 强制应用锁定索引，并提取出匹配的 Item 对象（如果是 -1 则匹配为 null）
				ApplyValueInternally(() => {
					base.SelectedIndex = value;
					lockSelectedItem = base.SelectedItem;
				});
			}
		}

		private object lockSelectedItem = null;
		[Category("Behavior"), DefaultValue(null), Description("设置该属性将立即锁定控件到指定的对象。设为 null 会锁定到空选项。")]
		public object LockSelectedItem {
			get { return lockSelectedItem; }
			set {
				lockSelectedItem = value;
				internalLockMode = LockMode.LockedWithItem;
				Enabled = false;

				// 强制应用锁定项，并提取出匹配的 Index 索引（如果是 null 则匹配为 -1）
				ApplyValueInternally(() => {
					base.SelectedItem = value;
					lockSelectedIndex = base.SelectedIndex;
				});
			}
		}

		/// <summary>
		/// 解除控件的锁定状态，恢复控件的启用状态并还原用户原先的选择。
		/// </summary>
		public void Unlock() {
			lockSelectedIndex = -1;
			lockSelectedItem = null;
			internalLockMode = LockMode.Unlocked;
			Enabled = true;

			// 还原用户原先的选择
			ApplyValueInternally(() => {
				base.SelectedIndex = userSelectedIndex;
				if (userSelectedIndex == -1 && userSelectedItem != null) {
					base.SelectedItem = userSelectedItem;
				}
			});
		}

		#endregion

		#region 重写原生选择属性

		[Category("Behavior"), Description("获取或设置当前选中的索引。如果处于锁定状态，赋值将不会更改用户的原始选择记录。")]
		public new int SelectedIndex {
			get { return base.SelectedIndex; }
			set {
				if (internalLockMode == LockMode.Unlocked) {
					ApplyValueInternally(() => base.SelectedIndex = value);
					userSelectedIndex = base.SelectedIndex;
					userSelectedItem = base.SelectedItem; // 强制保持 User 匹配
				} else {
					// 处于锁定状态时，忽略外部对原生属性的修改
					RestoreLockedValue();
				}
			}
		}

		[Category("Behavior"), Description("获取或设置当前选中的条目。如果处于锁定状态，赋值将不会更改用户的原始选择记录。")]
		public new object SelectedItem {
			get { return base.SelectedItem; }
			set {
				if (internalLockMode == LockMode.Unlocked) {
					ApplyValueInternally(() => base.SelectedItem = value);
					userSelectedItem = base.SelectedItem;
					userSelectedIndex = base.SelectedIndex; // 强制保持 User 匹配
				} else {
					// 处于锁定状态时，忽略外部对原生属性的修改
					RestoreLockedValue();
				}
			}
		}

		#endregion

		#region 内部辅助方法

		/// <summary>
		/// 当处于锁定状态时，用于拒绝并恢复正确的锁定显示值
		/// </summary>
		private void RestoreLockedValue() {
			ApplyValueInternally(() => {
				if (internalLockMode == LockMode.LockedWithIndex) {
					base.SelectedIndex = lockSelectedIndex;
				} else if (internalLockMode == LockMode.LockedWithItem) {
					base.SelectedItem = lockSelectedItem;
				}
			});
		}

		/// <summary>
		/// 安全地执行内部的值改变，避免触发内部事件导致的死循环
		/// </summary>
		private void ApplyValueInternally(Action action) {
			isInternalChanging = true;
			try {
				action?.Invoke();
			} finally {
				isInternalChanging = false;
			}
		}

		#endregion
	}
}
