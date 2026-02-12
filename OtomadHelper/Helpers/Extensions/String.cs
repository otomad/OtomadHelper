using System.Globalization;

namespace OtomadHelper.Helpers;

public static partial class Extensions {
	extension(string input) {
		/// <inheritdoc cref="Regex.Replace(string, string, string)"/>
		public string Replace(Regex pattern, string replacement) =>
			pattern.Replace(input, replacement);

		/// <inheritdoc cref="Regex.Replace(string, string, MatchEvaluator)"/>
		public string Replace(Regex pattern, MatchEvaluator evaluator) =>
			pattern.Replace(input, evaluator);

		/// <inheritdoc cref="Regex.Match(string, string)"/>
		public Match Match(Regex pattern) =>
			pattern.Match(input);

		/// <inheritdoc cref="Regex.Matches(string, string)"/>
		public MatchCollection Matches(Regex pattern) =>
			pattern.Matches(input);

		/// <inheritdoc cref="Regex.IsMatch(string, string)"/>
		public bool IsMatch(Regex pattern) =>
			pattern.IsMatch(input);

		/// <summary>
		/// Repeat the <paramref name="input" /> string <paramref name="count" /> times.
		/// </summary>
		/// <remarks>
		/// If you are trying to just repeat a character, please consider to use <c>char.Repeat(count)</c> instead of
		/// <c>string.Repeat(count)</c> for better performance.
		/// </remarks>
		/// <param name="input">The string to repeat.</param>
		/// <param name="count">Repeat count.</param>
		/// <returns>The repeated new string.</returns>
		public string Repeat(int count) =>
			string.Concat(Enumerable.Repeat(input, count));

		/// <summary>
		/// Generate stream from string.
		/// </summary>
		/// <returns><see cref="MemoryStream" /></returns>
		public MemoryStream ToStream() {
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			MemoryStream stream = new(bytes);
			return stream;
		}

		/// <inheritdoc cref="string.TrimStart(char[])" />
		public string TrimStart(string prefix, StringComparison comparisonType = StringComparison.InvariantCulture) =>
			input.StartsWith(prefix, comparisonType) ? input[prefix.Length..] : input;

		/// <inheritdoc cref="string.TrimEnd(char[])" />
		public string TrimEnd(string suffix, StringComparison comparisonType = StringComparison.InvariantCulture) =>
			input.EndsWith(suffix, comparisonType) ? input[0..^suffix.Length] : input;

		/// <summary>
		/// If <paramref name="value" /> is <see langword="null" /> or empty string (<c>""</c>), return <see langword="null" />; otherwise, return <paramref name="value" />.
		/// </summary>
		/// <remarks>
		/// <example>
		/// <code>
		/// string.DefaultIfEmpty(maybeEmptyString) ?? "default string"
		/// </code>
		/// </example>
		/// </remarks>
		/// <param name="value">The string to test.</param>
		public static string? DefaultIfEmpty(string? value) =>
			string.IsNullOrEmpty(value) ? null : value;

		/// <summary>
		/// If <paramref name="value" /> is <see langword="null" /> or empty string (<c>""</c>), return <paramref name="def" />; otherwise, return <paramref name="value" />.
		/// </summary>
		/// <remarks>
		/// <example>
		/// <code>
		/// string.DefaultIfEmpty(maybeEmptyString, "default string")
		/// </code>
		/// </example>
		/// </remarks>
		/// <param name="def">Fallback default string.</param>
		/// <inheritdoc cref="DefaultIfEmpty(string?)" />
		public static string DefaultIfEmpty(string? value, string def) =>
			string.IsNullOrEmpty(value) ? def : value!;

		/// <summary>
		/// Capitalizes the first character of a string.
		/// </summary>
		/// <param name="keepCase">If <see langword="true" />, maintains the case of characters after the first one.
		/// If <see langword="false" />, converts them to lowercase. Defaults to <see langword="false" />.</param>
		/// <returns>A new string with the first character capitalized and the rest either maintained or converted to lowercase based on the <paramref name="keepCase"/> parameter.</returns>
		public string ToCapitalized(bool keepCase = false) {
			string decapitated = input[1..];
			return input[0].ToUpper() + (keepCase ? decapitated : decapitated.ToLower());
		}
	}

	extension(IEnumerable<string> values) {
		/// <inheritdoc cref="string.Join(string, IEnumerable{string})"/>
		public string Join(string separator) =>
			string.Join(separator, values);

		/// <inheritdoc cref="string.Join(string, IEnumerable{string})"/>
		public string Join(char separator) =>
			string.Join(separator.ToString(), values);
	}

	extension(char input) {
		/// <summary>
		/// Repeat the <paramref name="input" /> char <paramref name="count" /> times.
		/// </summary>
		/// <param name="input">The char to repeat.</param>
		/// <param name="count">Repeat count.</param>
		/// <returns>The repeated new string.</returns>
		public string Repeat(int count) =>
			new(input, count);

		/// <inheritdoc cref="char.ToUpper(char)" />
		public char ToUpper() => char.ToUpper(input);

		/// <inheritdoc cref="char.ToUpper(char, CultureInfo)" />
		public char ToUpper(CultureInfo culture) => char.ToUpper(input, culture);

		/// <inheritdoc cref="char.ToUpperInvariant(char)" />
		public char ToUpperInvariant() => char.ToUpperInvariant(input);

		/// <inheritdoc cref="char.ToLower(char)" />
		public char ToLower() => char.ToLower(input);

		/// <inheritdoc cref="char.ToLower(char, CultureInfo)" />
		public char ToLower(CultureInfo culture) => char.ToLower(input, culture);

		/// <inheritdoc cref="char.ToLowerInvariant(char)" />
		public char ToLowerInvariant() => char.ToLowerInvariant(input);
	}
}
