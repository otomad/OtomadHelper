using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Otomad.VegasScript.OtomadHelper.V4 {
	public class GroupedRadioButton : RadioButton {
		public GroupedRadioButton() : base() {
			CheckedChanged += OnCheckedChanged;
		}

		private string _group = "";
		/// <summary>
		/// 指定单选按钮的分组名称。
		/// </summary>
		[Description("指定单选按钮的分组名称。"), Category("Behavior"), DefaultValue("")]
		public string Group {
			get { return _group; }
			set {
				_group = value;
				if (string.IsNullOrWhiteSpace(value)) _group = "";
			}
		}

		private Form Form { get { return FindForm(); } }

		private void OnCheckedChanged(object sender, EventArgs e) {
			if (isOnCheckedChanged) return;
			isOnCheckedChanged = true;
			if (string.IsNullOrWhiteSpace(Group)) goto End;
			if (Form == null) goto End;
			ForEachInGroup(Form, radio => {
				if (radio.Group == Group) radio.Checked = false;
				if (radio == this) Checked = true;
			});
		End:
			isOnCheckedChanged = false;
		}

		private static void ForEachInGroup(Control container, Action<GroupedRadioButton> action) {
			foreach (Control control in container.Controls) {
				if (control is GroupedRadioButton) {
					GroupedRadioButton radio = control as GroupedRadioButton;
					action(radio);
				}
				if (control.Controls.Count != 0) ForEachInGroup(control, action);
			}
		}

		private static bool isOnCheckedChanged = false;

		/// <summary>
		/// 单选按钮组。
		/// </summary>
		public class RadioButtonGroup {
			internal RadioButtonGroup(GroupedRadioButton currentRadio) {
				if (string.IsNullOrWhiteSpace(currentRadio.Group)) {
					_array = new GroupedRadioButton[0];
					return;
				}
				List<GroupedRadioButton> group = new List<GroupedRadioButton>();
				ForEachInGroup(currentRadio.Form, radio => {
					if (!string.IsNullOrWhiteSpace(radio.Group) && radio.Group == currentRadio.Group)
						group.Add(radio);
				});
				_array = group.ToArray();
			}
			/// <summary>
			/// 设定或读取该单选按钮组中所选中的单选按钮。如果没有勾选任何单选按钮，返回 null。
			/// </summary>
			public GroupedRadioButton Selected {
				get {
					foreach (GroupedRadioButton radio in this)
						if (radio.Checked)
							return radio;
					return null;
				}
				set {
					foreach (GroupedRadioButton radio in this)
						if (radio == value)
							radio.Checked = true;
				}
			}

			private readonly GroupedRadioButton[] _array;

			public int Count { get { return ((ICollection<GroupedRadioButton>)_array).Count; } }
			public bool IsReadOnly { get { return ((ICollection<GroupedRadioButton>)_array).IsReadOnly; } }
			public GroupedRadioButton this[int index] { get { return ((IList<GroupedRadioButton>)_array)[index]; } set { ((IList<GroupedRadioButton>)_array)[index] = value; } }
			public int IndexOf(GroupedRadioButton item) { return ((IList<GroupedRadioButton>)_array).IndexOf(item); }
			protected void Insert(int index, GroupedRadioButton item) { ((IList<GroupedRadioButton>)_array).Insert(index, item); }
			protected void RemoveAt(int index) { ((IList<GroupedRadioButton>)_array).RemoveAt(index); }
			protected void Add(GroupedRadioButton item) { ((ICollection<GroupedRadioButton>)_array).Add(item); }
			protected void Clear() { ((ICollection<GroupedRadioButton>)_array).Clear(); }
			public bool Contains(GroupedRadioButton item) { return ((ICollection<GroupedRadioButton>)_array).Contains(item); }
			protected void CopyTo(GroupedRadioButton[] array, int arrayIndex) { ((ICollection<GroupedRadioButton>)_array).CopyTo(array, arrayIndex); }
			protected bool Remove(GroupedRadioButton item) { return ((ICollection<GroupedRadioButton>)_array).Remove(item); }
			public IEnumerator<GroupedRadioButton> GetEnumerator() { return ((IEnumerable<GroupedRadioButton>)_array).GetEnumerator(); }
		}

		/// <summary>
		/// 获取单选按钮所在的单选按钮组。
		/// </summary>
		public RadioButtonGroup GetGroup() {
			return new RadioButtonGroup(this);
		}

		/// <summary>
		/// 获取单选按钮所在的单选按钮组。
		/// </summary>
		public RadioButtonGroup Related { get { return GetGroup(); } }
	}
}
