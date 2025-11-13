#pragma warning disable CS8600

namespace OtomadHelper.Helpers;

public static class MathEx {
	/// <summary>
	/// Calculates the floor modulus of two numbers.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This function computes the modulus operation where the result has the same sign as the divisor.
	/// It's particularly useful for handling negative numbers in modular arithmetic, making it more suitable for
	/// practical use.
	/// </para>
	/// <para>
	/// For example. When a random angle is given, but in reality, only taking the remainder obtained by dividing
	/// it by 360° is the true angle we need, we don't care about how many turns we have made. However, when the
	/// dividend is negative, using the `%` operator directly can cause some changes. We hope that the result got
	/// in this way is also a positive number that is more in line with practical use.
	/// </para>
	/// </remarks>
	/// <param name="x">The dividend (the number to be divided).</param>
	/// <param name="y">The divisor (the number to divide by).</param>
	/// <returns>
	/// The floor modulus of x and y. The result will have the same sign as y, and its absolute value will
	/// be less than the absolute value of y.
	/// </returns>
	[SuppressMessage("Style", "IDE1006")]
	private static dynamic _FloorMod(dynamic x, dynamic y) {
		dynamic result = x % y;
		if (result != 0 && x < 0 != y < 0)
			result += y;
		return result;
	}

	extension(Math) {
		/// <inheritdoc cref="_FloorMod"/>
		public static int FloorMod(int x, int y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static long FloorMod(long x, long y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static float FloorMod(float x, float y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(double x, double y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static float FloorMod(int x, float y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static float FloorMod(float x, int y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static float FloorMod(long x, float y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static float FloorMod(float x, long y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(int x, double y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(double x, int y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(long x, double y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(double x, long y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(float x, double y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static double FloorMod(double x, float y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static long FloorMod(int x, long y) => _FloorMod(x, y);
		/// <inheritdoc cref="_FloorMod"/>
		public static long FloorMod(long x, int y) => _FloorMod(x, y);

		/// <summary>
		/// Returns <paramref name="value"/> clamped to the inclusive range of <paramref name="min"/> and <paramref name="max"/>.
		/// </summary>
		/// <typeparam name="T">Number type.</typeparam>
		/// <param name="value">The value to be clamped.</param>
		/// <param name="min">The lower bound of the result.</param>
		/// <param name="max">The upper bound of the result.</param>
		/// <returns>
		/// <list type="bullet">
		/// <item><paramref name="value"/> if <paramref name="min"/> ≤ <paramref name="value"/> ≤ <paramref name="max"/>.</item>
		/// <item><paramref name="min"/> if <paramref name="value"/> &lt; <paramref name="min"/>.</item>
		/// <item><paramref name="max"/> if <paramref name="max"/> &lt; <paramref name="value"/>.</item>
		/// </list>
		/// </returns>
		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T> =>
			value.CompareTo(min) < 0 ? min : value.CompareTo(max) > 0 ? max : value;

		/// <summary>
		/// <para><b>Inaccurate Thermometer</b></para>
		///
		/// <para>
		/// Maps a value from one range to another, with a linear relationship between the old and new ranges and
		/// not necessarily a proportional relationship, such as the relationship between Celsius and Fahrenheit,
		/// and return the corresponding new value.
		/// </para>
		///
		/// <para>
		/// This function takes a value <c><paramref name="x"/></c> within a range <c>[<paramref name="min"/>, <paramref name="max"/>]</c>
		/// and maps it to a new range <c>[<paramref name="a"/>, <paramref name="b"/>]</c>.
		/// The mapping is done linearly, meaning that the ratio of the new range to the old range is preserved.
		/// </para>
		///
		/// <para>
		/// For example, changing a color value from 0 to 255 to a value from 0 to 100.
		/// </para>
		/// </summary>
		/// <param name="x">The value within the old range to be mapped.</param>
		/// <param name="min">The minimum value of the old range.</param>
		/// <param name="max">The maximum value of the old range.</param>
		/// <param name="a">The minimum value of the new range.</param>
		/// <param name="b">The maximum value of the new range.</param>
		/// <returns>The mapped value within the new range.</returns>
		public static TOutput Map<TOutput>(double x, double min, double max, TOutput a, TOutput b) where TOutput : IComparable<TOutput> =>
			min == (double)(dynamic)a && max == (double)(dynamic)b ? (TOutput)(dynamic)x :
			(TOutput)(dynamic)(((double)(dynamic)b - (double)(dynamic)a) * (x - min) / (max - min) + (double)(dynamic)a);

		/// <summary>
		/// This function combine <see cref="Clamp{T}(T, T, T)" /> and <see cref="Map{TOutput}(double, double, double, TOutput, TOutput)" /> into one.
		/// </summary>
		/// <param name="x">The value within the old range to be mapped.</param>
		/// <param name="min">The minimum value of the old range.</param>
		/// <param name="max">The maximum value of the old range.</param>
		/// <param name="a">The minimum value of the new range.</param>
		/// <param name="b">The maximum value of the new range.</param>
		/// <returns>The clamp-mapped value within the new range.</returns>
		public static TOutput ClampMap<TOutput>(double x, double min, double max, TOutput a, TOutput b) where TOutput : IComparable<TOutput> =>
			Clamp((dynamic)Map(x, min, max, a, b), a, b);

		///// <inheritdoc cref="Enumerable.Max{TSource}(IEnumerable{TSource})" />
		//public static T Max<T>(params T?[] values) where T : IComparable<T> => Enumerable.Max(values);

		///// <inheritdoc cref="Enumerable.Min{TSource}(IEnumerable{TSource})" />
		//public static T Min<T>(params T?[] values) where T : IComparable<T> => Enumerable.Min(values);
	}
}
