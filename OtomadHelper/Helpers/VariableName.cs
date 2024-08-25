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
			words =
				value.Contains("_") ? value.Split('_') :
				value.Contains('-') ? value.Split('-') :
				value
					.Replace(new Regex(@"(?!^)([A-Z])"), " $1")
					.Replace(new Regex(@"(\d+)"), " $1 ")
					.Replace(new Regex(@"\s+(?=\s)|^\s+|\s+$"), "")
					.Split(' ');
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
			char.ToUpperInvariant(str[0]) + str.Substring(1).ToLowerInvariant();

	public override string ToString() => string.Join(" ", words);

	public string this[int index] {
		get => words[index];
		set => words[index] = value;
	}
}
