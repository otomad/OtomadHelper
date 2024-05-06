namespace OtomadHelper.Helpers;

/// <summary>
/// Variable name helper class.<br />
/// Used to easily convert a variable name into camelCase, kebab-case, and so on.
/// </summary>
public class VariableName {
	private string[] words = null!;

	public VariableName(string value) {
		Value = value;
	}

	/// <summary>
	/// Reset the value with any form of the variable name.
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
	/// Convert to kebab-case.
	/// </summary>
	public string Kebab => string.Join("-", words).ToLowerInvariant();

	/// <summary>
	/// Convert to snake_case.
	/// </summary>
	public string Snake => string.Join("_", words).ToLowerInvariant();

	/// <summary>
	/// Convert to CONSTANT_CASE.
	/// </summary>
	public string Const => string.Join("_", words).ToUpperInvariant();

	/// <summary>
	/// Convert to PascalCase.
	/// </summary>
	public string Pascal => string.Join("", words.Select(Capitalize));

	/// <summary>
	/// Convert to camelCase.
	/// </summary>
	public string Camel => string.Join("", words.Select((word, i) => i == 0 ? word.ToLowerInvariant() : Capitalize(word)));

	/// <summary>
	/// Convert to lowercase without any separators.
	/// </summary>
	public string Lower => string.Join("", words).ToLowerInvariant();

	/// <summary>
	/// Convert to UPPERCASE without any separators.
	/// </summary>
	public string Upper => string.Join("", words).ToUpperInvariant();

	/// <summary>
	/// Convert to word case, separated by spaces, all in lowercase.
	/// </summary>
	public string Words => string.Join(" ", words).ToLowerInvariant();

	/// <summary>
	/// Convert to Sentence case, separated by spaces, with only the first letter of the sentence capitalized.
	/// </summary>
	public string Sentence => string.Join(" ", words.Select((word, i) => i != 0 ? word.ToLowerInvariant() : Capitalize(word)));

	/// <summary>
	/// Convert to Title Case, separated by spaces, with all first letters of words capitalized.
	/// </summary>
	public string Title => string.Join(" ", words.Select(Capitalize));

	/// <summary>
	/// Convert a word to uppercase the first letter and lowercase other letters.
	/// </summary>
	/// <param name="str">Word.</param>
	/// <returns>Capitalize the first letter and lowercase other letters.</returns>
	private static string Capitalize(string str) =>
		string.IsNullOrEmpty(str) ? "" :
			str[0].ToString().ToUpperInvariant() + str.Substring(1).ToLowerInvariant();

	public override string ToString() => string.Join(" ", words);

	public string this[int index] {
		get => words[index];
		set => words[index] = value;
	}
}
