using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public class RememberedCheckBox : CheckBox {
		private bool locked = false;

		public RememberedCheckBox() {
			CheckedChanged += (sender, e) => Checked = base.Checked;
		}

		private bool userChecked = false;
		[Category("Appearance"), DefaultValue(false), Description("设置用户设定的选中状态值。")]
		public bool UserChecked {
			get { return userChecked; }
			set { userChecked = value; }
		}

		private StatusType status = StatusType.Unlocked;
		[Category("Appearance"), DefaultValue(typeof(StatusType), "Unlocked"), Description("设置组件的锁定状态。")]
		public StatusType Status {
			get { return status; }
			set {
				if (status == value) return;
				else if (status != StatusType.Unlocked && value != StatusType.Unlocked)
					Checked = value == StatusType.True;
				else if (status == StatusType.Unlocked && value != StatusType.Unlocked) {
					locked = true;
					Enabled = false;
					Checked = value == StatusType.True;
				} else if (status != StatusType.Unlocked && value == StatusType.Unlocked) {
					locked = false;
					Enabled = true;
					Checked = userChecked;
				}
				status = value;
			}
		}

		[Category("Appearance"), DefaultValue(false), Description("指定组件是否处于选中状态。")]
		public new bool Checked {
			get { return base.Checked; }
			set {
				base.Checked = value;
				if (!locked) UserChecked = value;
			}
		}

		public enum StatusType {
			Unlocked = -1,
			False,
			True,
		}
	}
}
