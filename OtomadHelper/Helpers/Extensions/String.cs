namespace OtomadHelper.Helpers;

public static partial class Extensions {
	/// <inheritdoc cref="Regex.Replace(string, string, string)"/>
	public static string Replace(this string input, Regex pattern, string replacement) =>
		pattern.Replace(input, replacement);

	/// <inheritdoc cref="Regex.Replace(string, string, MatchEvaluator)"/>
	public static string Replace(this string input, Regex pattern, MatchEvaluator evaluator) =>
		pattern.Replace(input, evaluator);

	/// <inheritdoc cref="Regex.Match(string, string)"/>
	public static Match Match(this string input, Regex pattern) =>
		pattern.Match(input);

	/// <inheritdoc cref="Regex.Matches(string, string)"/>
	public static MatchCollection Matches(this string input, Regex pattern) =>
		pattern.Matches(input);

	/// <inheritdoc cref="Regex.IsMatch(string, string)"/>
	public static bool IsMatch(this string input, Regex pattern) =>
		pattern.IsMatch(input);

	/// <inheritdoc cref="string.Join(string, IEnumerable{string})"/>
	public static string Join(this IEnumerable<string> values, string separator) =>
		string.Join(separator, values);

	/// <inheritdoc cref="string.Join(string, IEnumerable{string})"/>
	public static string Join(this IEnumerable<string> values, char separator) =>
		string.Join(separator.ToString(), values);
}
