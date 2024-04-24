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
	public static dynamic PNMod(dynamic a, dynamic b) {
		if (b == 0) return double.NaN;
		b = Math.Abs(b);
		int i = 0;
		while (a + b * i < 0)
			i++;
		return (a + b * i) % b;
	}
}
