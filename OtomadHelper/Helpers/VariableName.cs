namespace OtomadHelper.Helpers;

/// <summary>
/// 变量名辅助类。<br />
/// 用于方便地把变量名称转换成驼峰、连字符等。
/// </summary>
public class VariableName {
	private string[] words = null!;

	public VariableName(string value) {
		Value = value;
	}

	/// <summary>
	/// 以任意形式的变量名重新设值。
	/// </summary>
	public string Value {
		set {
			value = value.Trim();
			if (value.Contains("_"))
				words = value.Split('_');
			else if (value.Contains('-'))
				words = value.Split('-');
			else {
				string splited = Regex.Replace(value, @"(?!^)([A-Z])", " $1");
				splited = Regex.Replace(splited, @"(\d+)", " $1 ");
				splited = Regex.Replace(splited, @"\s+(?=\s)|^\s+|\s+$", "");
				words = splited.Split(' ');
			}
		}
	}

	/// <summary>
	/// 转换为连字符式。
	/// </summary>
	public string Kebab => string.Join("-", words).ToLowerInvariant();

	/// <summary>
	/// 转换为下划线式。
	/// </summary>
	public string Snake => string.Join("_", words).ToLowerInvariant();

	/// <summary>
	/// 转换为常量式。
	/// </summary>
	public string Const => string.Join("_", words).ToUpperInvariant();

	/// <summary>
	/// 转换为大驼峰式。
	/// </summary>
	public string Pascal => string.Join("", words.Select(Capitalize));

	/// <summary>
	/// 转换为小驼峰式。
	/// </summary>
	public string Camel => string.Join("", words.Select((word, i) => i == 0 ? word.ToLowerInvariant() : Capitalize(word)));

	/// <summary>
	/// 转换为小写字母式，且无任何分隔符。
	/// </summary>
	public string Lower => string.Join("", words).ToLowerInvariant();

	/// <summary>
	/// 转换为大写字母式，且无任何分隔符。
	/// </summary>
	public string Upper => string.Join("", words).ToUpperInvariant();

	/// <summary>
	/// 转换为单词式，空格分隔，全部小写。
	/// </summary>
	public string Words => string.Join(" ", words).ToLowerInvariant();

	/// <summary>
	/// 转换为句子式，空格分隔，仅句首字母大写。
	/// </summary>
	public string Sentence => string.Join(" ", words.Select((word, i) => i != 0 ? word.ToLowerInvariant() : Capitalize(word)));

	/// <summary>
	/// 转换为标题式，空格分隔，所有单词首字母大写。
	/// </summary>
	public string Title => string.Join(" ", words.Select(Capitalize));

	/// <summary>
	/// 将单词转换为首字母大写，其它字母为小写。
	/// </summary>
	/// <param name="str">单词。</param>
	/// <returns>首字母大写，其它字母为小写的单词。</returns>
	private static string Capitalize(string str) =>
		string.IsNullOrEmpty(str) ? "" :
			str[0].ToString().ToUpperInvariant() + str.Substring(1).ToLowerInvariant();

	public override string ToString() => string.Join(" ", words);

	public string this[int index] {
		get => words[index];
		set => words[index] = value;
	}
}
