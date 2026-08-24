/**
 * Clamps a value within a specified range.
 *
 * It will limit a value between an upper limit and a lower limit. When the value exceeds the range of the minimum
 * and maximum values, select a value between the minimum and maximum values to use.
 *
 * @template TNumber - A numeric type that extends either `number` or `bigint`.
 * @param value - The value to be clamped.
 * @param min - The minimum value of the range. If not provided, the value will be compared with the maximum value.
 * @param max - The maximum value of the range. If not provided, the value will be compared with the minimum value.
 * @returns The clamped value within the specified range.
 * @throws {RangeError} If `min` is greater than `max`, an error is thrown with a message indicating the invalid range.
 */
export function clamp<TNumber extends number | bigint>(value: TNumber, min: TNumber, max: TNumber): TNumber;
/**
 * Clamps a value with a minimum value.
 *
 * @template TNumber - A numeric type that extends either `number` or `bigint`.
 * @param value - The value to be clamped.
 * @param min - Provide the minimum value, if it is greater than the value, the function will return it.
 * @returns The `value`, or `min` unless the `value` is less than `min`.
 */
export function clamp<TNumber extends number | bigint>(value: TNumber, min: TNumber): TNumber;
/**
 * Clamps a value with a maximum value.
 *
 * @template TNumber - A numeric type that extends either `number` or `bigint`.
 * @param value - The value to be clamped.
 * @param min - *Do not provide a minimum value, explicitly pass an `undefined`.*
 * @param max - Provide the maximum value, if it is less than the value, the function will return it.
 * @returns The `value`, or `max` unless the `value` is greater than `max`.
 */
export function clamp<TNumber extends number | bigint>(value: TNumber, min: undefined, max: TNumber): TNumber;
export function clamp<TNumber extends number | bigint>(value: TNumber, min?: TNumber, max?: TNumber) {
	if (min !== undefined && max !== undefined && min > max)
		throw new RangeError(`Invalid min or max value, the min value cannot greater than the max value, got range ${min} ~ ${max}`);
	if (min !== undefined && value < min) value = min;
	if (max !== undefined && value > max) value = max;
	return value;
}

/**
 * ### Inaccurate Thermometer
 *
 * Maps a value from one range to another, with a linear relationship between the old and new ranges and
 * not necessarily a proportional relationship, such as the relationship between Celsius and Fahrenheit,
 * and return the corresponding new value.
 *
 * This function takes a value `x` within a range `[min, max]` and maps it to a new range `[a, b]`.
 * The mapping is done linearly, meaning that the ratio of the new range to the old range is preserved.
 *
 * For example, changing a color value from 0 to 255 to a value from 0 to 100.
 *
 * @param x - The value within the old range to be mapped.
 * @param min - The minimum value of the old range.
 * @param max - The maximum value of the old range.
 * @param a - The minimum value of the new range.
 * @param b - The maximum value of the new range.
 * @returns The mapped value within the new range.
 */
export function map(x: number, min: number, max: number, a: number, b: number) {
	return (b - a) * (x - min) / (max - min) + a;
}

/**
 * This function combine {@link clamp} and {@link map} into one.
 *
 * @param x - The value within the old range to be mapped.
 * @param min - The minimum value of the old range.
 * @param max - The maximum value of the old range.
 * @param a - The minimum value of the new range.
 * @param b - The maximum value of the new range.
 * @returns The clamp-mapped value within the new range.
 */
export function clampMap(x: number, min: number, max: number, a: number, b: number) {
	return map(clamp(x, min, max), min, max, a, b);
}

/**
 * Generates a random integer between the specified range.
 *
 * This function generates a random integer within the specified range `[min, max]` (inclusive).
 * It uses the `Math.random()` function to generate a random decimal between 0 (inclusive) and 1 (exclusive),
 * then scales and biases it to produce a value in the specified range.
 *
 * @param min - The minimum value of the range.
 * @param max - The maximum value of the range.
 * @returns A random integer within the specified range.
 */
export const randBetween = (min: number, max: number) => Math.floor(Math.random() * (max + 1 - min) + min);

/**
 * Calculates the floor modulus of two numbers.
 *
 * This function computes the modulus operation where the result has the same sign as the divisor.
 * It's particularly useful for handling negative numbers in modular arithmetic, making it more suitable for
 * practical use.
 *
 * For example. When a random angle is given, but in reality, only taking the remainder obtained by dividing
 * it by 360° is the true angle we need, we don't care about how many turns we have made. However, when the
 * dividend is negative, using the `%` operator directly can cause some changes. We hope that the result got
 * in this way is also a positive number that is more in line with practical use.
 *
 * @param x - The dividend (the number to be divided).
 * @param y - The divisor (the number to divide by).
 * @returns The floor modulus of x and y. The result will have the same sign as y, and its absolute value will
 * be less than the absolute value of y.
 */
export function floorMod(x: number, y: number) {
	let result = x % y;
	if (result !== 0 && x < 0 !== y < 0)
		result += y;
	return result;
}

/**
 * Sums up a list of numbers.
 * @param numbers - Numbers.
 * @returns Sum in number type.
 */
export function sum(...numbers: number[]): number;
/**
 * Sums up a list of big integers.
 * @param bigints - Big integers.
 * @returns Sum in big integer type.
 */
export function sum(...bigints: bigint[]): bigint;
/**
 * Sums up a list of mixed numbers and big integers.
 * @note Mixing numbers and big integers will return a number type.
 * @param numbers - Numbers or big integers.
 * @returns Sum in number type.
 */
export function sum(...numbers: (number | bigint)[]): number;
export function sum(...numbers: (number | bigint)[]): number | bigint {
	const returnsBigInt = numbers.every(number => typeof number === "bigint");
	return numbers.reduce((/** accumulator */ a, /** currentValue */ b) =>
		returnsBigInt ? BigInt(a) + BigInt(b) : Number(a) + Number(b),
	returnsBigInt ? 0n : 0);
}

/**
 * Gets the average of a list of numbers.
 * @param numbers - Numbers.
 * @returns Average in number type.
 */
export function average(...numbers: number[]) {
	if (numbers.length === 0) return NaN;
	return numbers.reduce((a, b) => a + b, 0) / numbers.length;
}

/**
 * Gets the median of a list of numbers.
 * @param numbers - Numbers.
 * @returns Median in number type.
 */
export function median(...numbers: number[]) {
	numbers.sort((a, b) => a - b);
	const mid = Math.floor(numbers.length / 2);
	return numbers.length % 2 ? numbers[mid] : (numbers[mid - 1] + numbers[mid]) / 2;
}
