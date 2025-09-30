/**
 * Compares two functions to determine if they are approximately equal.
 *
 * This function checks if both values are of type `function` and either:
 * - They are strictly equal (`===`);
 * - They are not native functions;
 * - Their `toString()` representations are equal.
 *
 * @param a - The first value to compare.
 * @param b - The second value to compare.
 * @returns Are both functions approximately equal?
 */
export function areFunctionsApproxEqual(a: unknown, b: unknown) {
	return typeof a === "function" && typeof b === "function" && (a === b ||
		!lodash.isNative(a) && !lodash.isNative(b) && a.toString() === b.toString()
	);
}

/**
 * Compares two functions to determine if they have the same name and number of parameters.
 *
 * @param a - The first value to compare.
 * @param b - The second value to compare.
 * @returns Are both functions have the same name and parameter count?
 */
export function areFunctionsGenerallyEqual(a: unknown, b: unknown) {
	return typeof a === "function" && typeof b === "function" &&
		a.name === b.name && a.length === b.length;
}

/**
 * Check if a function is async.
 * @param test - The function to test.
 * @returns Is the function async?
 */
export function isAsyncFunction(test: unknown): test is (...args: Any[]) => Promise<Any> {
	return test instanceof Function && test.constructor.name === "AsyncFunction";
}

/**
 * Determines whether the provided value is an `arguments` object of the function.
 *
 * @param test - The value to test.
 * @returns Is the value an `arguments` object?
 */
export function isArguments(test: unknown): test is IArguments {
	return Object.prototype.toString.call(test) === "[object Arguments]";
}
