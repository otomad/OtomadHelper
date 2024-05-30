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
}
