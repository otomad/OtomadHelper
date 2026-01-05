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
	 * @example
	 * ```typescript
	 * console.log((10 / 5).isInteger); // Output: true; Value: 2
	 * console.log((11 / 5).isInteger); // Output: false; Value: 2.2
	 * ```
	 */
	readonly isInteger: boolean;

	/**
	 * Check if the number is a safe integer.
	 * @example
	 * ```typescript
	 * console.log((2 ** 53).isSafeInteger); // Output: false
	 * console.log((2 ** 53 - 1).isSafeInteger); // Output: true
	 * ```
	 */
	readonly isSafeInteger: boolean;

	/**
	 * Check if the number is finite.
	 * @example
	 * ```typescript
	 * console.log((1 / 0).isFinite); // Output: false; Value: Infinity
	 * console.log((10 / 5).isFinite); // Output: true; Value: 2
	 * console.log((0 / 0).isFinite); // Output: false; Value: NaN
	 * ```
	 */
	readonly isFinite: boolean;

	/**
	 * Check if the number is the reserved value NaN (not a number).
	 * @example
	 * ```typescript
	 * console.log((100).isNaN); // Output: false
	 * console.log(NaN.isNaN); // Output: true
	 * ```
	 */
	readonly isNaN: boolean;

	/**
	 * Returns a number in fixed-point notation, instead of string.
	 * @param fractionDigits - Number of digits after the decimal point. Must be in the range 0 - 20, inclusive.
	 * @example
	 * ```typescript
	 * console.log((123.456).toFixedNumber(2)); // Output: 123.46
	 * console.log((0.004).toFixedNumber(2)); // Output: 0
	 * console.log((1.23e+5).toFixedNumber(2)); // Output: 123000
	 * ```
	 */
	toFixedNumber(fractionDigits?: number): number;

	/**
	 * Get the number of decimals in a number.
	 * @example
	 * ```typescript
	 * console.log((3.1415926).countDecimals(2)); // Output: 7
	 * console.log((3).countDecimals(2)); // Output: 0
	 * ```
	 */
	countDecimals(): number;
}
