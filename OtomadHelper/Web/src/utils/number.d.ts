declare interface Number {
	/**
	 * Checks if the number is positive (greater than 0 or equal to **positive 0 `+0`**).
	 *
	 * @example
	 * ```typescript
	 * console.log((5).isPositive); // Output: true
	 * console.log((-3).isPositive); // Output: false
	 * console.log((0).isPositive); // Output: true
	 * console.log((-0).isPositive); // Output: false
	 * ```
	 */
	readonly isPositive: boolean;

	/**
	 * Check if the number is an integer.
	 */
	readonly isInteger: boolean;

	/**
	 * Check if the number is a safe integer.
	 */
	readonly isSafeInteger: boolean;

	/**
	 * Check if the number is finite.
	 */
	readonly isFinite: boolean;

	/**
	 * Check if the number is the reserved value NaN (not a number).
	 */
	readonly isNaN: boolean;

	/**
	 * Returns a number in fixed-point notation, instead of string.
	 * @param fractionDigits - Number of digits after the decimal point. Must be in the range 0 - 20, inclusive.
	 */
	toFixedNumber(fractionDigits?: number): number;

	/**
	 * Get the number of decimals in a number.
	 */
	countDecimals(): number;
}
