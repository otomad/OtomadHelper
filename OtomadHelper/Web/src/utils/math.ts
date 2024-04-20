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
 * @throws If `min` is greater than `max`, an error is thrown with a message indicating the invalid range.
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
		throw new Error(`Invalid min or max value, the min value cannot greater than the max value, got range ${min} ~ ${max}`);
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
 * ### 不准温度计
 * 即将一个数值从一个标度单位转移到另一个标度单位，新旧单位成线性关系，且不一定成正比关系，比如说摄氏度和华氏度的关系，返回对应的新值。
 *
 * 比如说，将一个取值范围为 0 ~ 255 的颜色值转到 0 ~ 100 的值。
 *
 * 这将会返回一个 CSS `calc()` 声明。
 * @param x - 待转换的原标度数值。
 * @param min - 原标度值（小）。
 * @param max - 原标度值（大）。
 * @param a - 新标度值（小）。
 * @param b - 新标度值（大）。
 * @returns 转换后的新标度数值的 CSS `calc()` 声明。
 */
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
 * This will return a CSS `calc()` formula declaration.
 *
 * @param x - The value within the old range to be mapped.
 * @param min - The minimum value of the old range.
 * @param max - The maximum value of the old range.
 * @param a - The minimum value of the new range.
 * @param b - The maximum value of the new range.
 * @returns A CSS `calc()` formula declaration that will be calculated as the mapped value within the new range.
 */
export function mapCssCalc(x: string | number, min: string | number, max: string | number, a: string | number, b: string | number) {
	return `calc(((${b} - ${a}) * (${x} - ${min}) / (${max} - ${min})) + ${a})`;
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
 * Mainly targeting a proposed modulus for negative numbers, making it more suitable for practical use,
 * and returning a non negative number.
 *
 * For example. When a random angle is given, but in reality, only taking the remainder obtained by dividing
 * it by 360° is the true angle we need, we don't care about how many turns we have made. However, when the
 * dividend is negative, using the `%` operator directly can cause some changes. We hope that the result got
 * in this way is also a positive number that is more in line with practical use.
 *
 * @param a - Dividend.
 * @param b - Divisor.
 * @returns Remainder.
 */
export function PNMod(a: number, b: number) {
	if (b === 0) return NaN;
	b = Math.abs(b);
	let i = 0;
	while (a + b * i < 0)
		i++;
	return (a + b * i) % b;
}
