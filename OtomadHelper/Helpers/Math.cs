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
}
