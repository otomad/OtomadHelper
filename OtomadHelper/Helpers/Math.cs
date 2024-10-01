namespace OtomadHelper.Helpers;

public static class MathEx {
	/// <summary>
	/// Mainly targeting a proposed modulus for negative numbers, making it more suitable for practical use,
	/// and returning a non negative number.
	/// </summary>
	/// <remarks>
	/// For example. When a random angle is given, but in reality, only taking the remainder obtained by dividing
	/// it by 360° is the true angle we need, we don't care about how many turns we have made. However, when the
	/// dividend is negative, using the `%` operator directly can cause some changes. We hope that the result got
	/// in this way is also a positive number that is more in line with practical use.
	/// </remarks>
	/// <param name="a">Dividend.</param>
	/// <param name="b">Divisor.</param>
	/// <returns>Remainder.</returns>
	private static dynamic _PNMod(dynamic a, dynamic b) {
		if (b == 0) return double.NaN;
		b = Math.Abs(b);
		int i = 0;
		while (a + b * i < 0)
			i++;
		return (a + b * i) % b;
	}

	/// <inheritdoc cref="_PNMod"/>
	public static int PNMod(int a, int b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static long PNMod(long a, long b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static float PNMod(float a, float b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(double a, double b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static float PNMod(int a, float b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static float PNMod(float a, int b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static float PNMod(long a, float b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static float PNMod(float a, long b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(int a, double b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(double a, int b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(long a, double b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(double a, long b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(float a, double b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static double PNMod(double a, float b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static long PNMod(int a, long b) => _PNMod(a, b);
	/// <inheritdoc cref="_PNMod"/>
	public static long PNMod(long a, int b) => _PNMod(a, b);

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
		Math.Max((dynamic)min!, Math.Min((dynamic)value!, (dynamic)max!));

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
	public static TOutput Map<TOutput>(decimal x, decimal min, decimal max, TOutput a, TOutput b) =>
		(TOutput)(dynamic)(((decimal)(dynamic)b! - (decimal)(dynamic)a!) * (x - min) / (max - min) + (decimal)(dynamic)a!);

}
