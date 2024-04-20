declare interface String {
	/**
	 * Find the number of times a specified character appears in a string.
	 *
	 * @param chars - The characters to search for.
	 * @returns Find the number of specified characters.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".countChar("l")); // Output: 3
	 * ```
	 */
	countChar(...chars: string[]): number;

	/**
	 * Reverse the order of strings.
	 *
	 * Using `Array.from()` can avoid the problem of characters other than Unicode BMP not be reversed properly.
	 *
	 * @returns Reversed string.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".reverse()); // Output: "!dlrow olleh"
	 * console.log("🌸🐔".reverse()); // Output: "🐔🌸"
	 * ```
	 */
	reverse(): string;

	/**
	 * Convert the string to a boolean.
	 *
	 * Used to solve problems such as `"false" == false` being false.
	 *
	 * @returns Boolean.
	 */
	toBoolean(): boolean;

	/**
	 * Convert a string to a character array similar to C, where each item in the array corresponds to each character in the string.
	 *
	 * The function uses `Array.from()` which can avoid the problem of characters other than Unicode BMP are split into UTF-16 surrogate pairs.
	 *
	 * @returns An array in string form containing each character in the source string.
	 *
	 * @example
	 * ```javascript
	 * console.log("🌸🐔".toArray()); // Output: ["🌸", "🐔"]
	 *
	 * const string = "🌸🐔";
	 * const array = string.toArray();
	 *
	 * // Use indexer in the array
	 * console.log(array[0]); // Output: "🌸"
	 * console.log(array[1]); // Output: "🐔"
	 *
	 * // Use indexer in the string directly
	 * console.log(string[0]); // Output: "\uD83C"
	 * console.log(string[1]); // Output: "\uDF38"
	 * // As you can see, this will not return the original characters we expected.
	 * ```
	 */
	toArray(): string[];

	/**
	 * Insert a character between every two characters in a string.
	 *
	 * @param sep - Separator. Defaults to `,`.
	 * @returns The processed string.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".inTwo("|")); // Output: "h|e|l|l|o| |w|o|r|l|d|!"
	 * ```
	 */
	inTwo(sep?: string): string;

	/**
	 * Is the string a certain string in the function parameters.
	 *
	 * This can be more conveniently used in TypeScript's type guard.
	 *
	 * @param list - To test whether a string array is included.
	 * @returns Included.
	 *
	 * @example
	 * ```typescript
	 * declare const a: "foo" | "bar" | "baz";
	 *
	 * // Use type guard
	 * a.in("foo", "bar") ?
	 *     a : // Type: "foo" | "bar";
	 *     a;  // Type: "baz";
	 *
	 * // Use `Array.prototype.includes` directly, this will never guard the type
	 * ["foo", "bar"].includes(a) ?
	 *     a : // Type: "foo" | "bar" | "baz";
	 *     a;  // Type: "foo" | "bar" | "baz";
	 *
	 * // Use `Array.prototype.includes` with a constant array, this will raise an error
	 * (["foo", "bar"] as const).includes(a) ? // Error: Type '"c"' is not assignable to type '"a" | "b"'
	 *     a : // Type: "foo" | "bar" | "baz";
	 *     a;  // Type: "foo" | "bar" | "baz";
	 * ```
	 */
	in<const T extends string>(...list: (T | undefined | null)[]): this is T;

	/**
	 * Remove all the white space characters in the string.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world ! ! !".removeSpace()); // Output: "helloworld!!!"
	 * ```
	 */
	removeSpace(): string;

	/**
	 * The hole is `[holeStart, holeEnd)`.
	 *
	 * And `String.prototype.holeString(holeStart, holeEnd)` will create a substring which contains `[0, holeStart) + [holeEnd, end]`.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".holeString(3, 6)); // Output: "helworld!"
	 * ```
	 */
	holeString(holeStart: number, holeEnd: number): string;
}
