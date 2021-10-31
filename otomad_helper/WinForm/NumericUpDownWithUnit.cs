using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
//using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VegasScript {

	public class NumericUpDownWithUnit : NumericUpDown {

		#region| Fields |
		private string suffix = "";
		private string prefix = "";
		private bool enableDecimalPlaces = false;
		#endregion

		#region| Properties |
		[Description("后缀单位。"), Category("Appearance"), DefaultValue("")]
		public string Suffix {
			get {
				return suffix;
			}
			set {
				suffix = value;
				UpdateEditText();
			}
		}

		[Description("前缀单位。"), Category("Appearance"), DefaultValue("")]
		public string Prefix {
			get {
				return prefix;
			}
			set {
				prefix = value;
				UpdateEditText();
			}
		}

		/// </summary>
		/// 返回限制在最小值和最大值内的提供值。这与基类中的值完全相同（基类是私有的，因此我们不能直接使用它）。
		/// </summary>
		[Description("返回限制在最小值和最大值内的提供值。这与基类中的值完全相同（基类是私有的，因此我们不能直接使用它）。"), Category("Appearance")]
		public decimal Constrain {
			get {
				return Value;
			}
			set {
				if (value < Minimum) Value = Minimum;
				else if (value > Maximum) Value = Maximum;
				else Value = value;
			}
		}

		[Description("是否启用数字显示框中要显示的十进制位数。"), Category("Data"), DefaultValue(false)]
		public bool EnableDecimalPlaces {
			get {
				return enableDecimalPlaces;
			}
			set {
				enableDecimalPlaces = value;
				decimal _value = Value;
				Value = Minimum;
				Value = _value;
			}
		}
		#endregion

		#region| Methods |

		/// <summary>
		/// 更新 NumericUpDown 文本时调用的方法。
		/// </summary>
		protected override void UpdateEditText() {
			List<string> list = new List<string> { enableDecimalPlaces ? Value.ToString("F" + DecimalPlaces) : Regex.Replace(Value.ToString(), @"(?<=\..*)0+$", "") };
			if (!string.IsNullOrWhiteSpace(suffix)) list.Add(suffix);
			if (!string.IsNullOrWhiteSpace(prefix)) list.Insert(0, prefix);
			Text = string.Join(" ", list);
		}

		/// <summary>
		/// 在实际更新文本之前验证调用的方法。这与基类完全相同，但它将使用该类中的新 ParseEditText。
		/// </summary>
		protected override void ValidateEditText() {
			// 考虑到标签单位，查看编辑文本是否解析为有效的小数
			ParseEditText();
			ToFixed();
			UpdateEditText();
		}

		/// <summary>
		/// 将 NumericUpDown 控件中显示的文本转换为数值并对其求值。
		/// </summary>
		protected new void ParseEditText() {
			try {
				// 此方法与基本方法的唯一区别在于，文本直接替换为属性文本，而不是使用正则表达式。
				// 现在，我们知道文本框上可能只有我们提供的单位中的字符。因为 NumericUpDown 为我们处理来自用户的无效输入。
				// 这就是魔法发生的地方。此正则表达式将匹配单位中的所有字符（因此单位不能有数字）。
				// 您可以更改此正则表达式以满足您的需要。
				Regex regex = new Regex(string.Format(@"[^(?!{0}{1} )]+", suffix, prefix));
				Match match = regex.Match(Text);
				if (match.Success) {
					string text = match.Value;
					// VSWhidbey 173332: 在尝试设置Value属性之前，请验证用户没有以“-”开头字符串，
					// 因为“-”是用于表示负数的字符串的有效字符。
					if (!string.IsNullOrEmpty(text) && !(text.Length == 1 && text == "-")) {
						if (Hexadecimal) Constrain = Convert.ToDecimal(Convert.ToInt32(Text, 16));
						else Constrain = decimal.Parse(text, CultureInfo.CurrentCulture);
					}
				}
			} catch {
				// 保持原样
			} finally {
				UserEdit = false;
				ToFixed();
			}
		}

		private void ToFixed() {
			if (enableDecimalPlaces)
				Constrain = decimal.Round(Constrain, DecimalPlaces);
		}
		#endregion
	}
}
