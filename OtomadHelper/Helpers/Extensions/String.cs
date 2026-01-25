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
	}
}
