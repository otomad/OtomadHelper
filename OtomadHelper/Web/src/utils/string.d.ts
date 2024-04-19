declare interface String {
	/**
	 * Find the number of times a specified character appears in a string.
	 *
	 * @param chars - The characters to search for.
	 * @returns Find the number of specified characters.
	 */
	countChar(...chars: string[]): number;

	/**
	 * Reverse the order of strings.
	 *
	 * Using `Array.from()` can avoid the problem of characters other than Unicode BMP not be reversed properly.
	 *
	 * @example
	 * ```javascript
	 * "hello world!".reverse(); // "!dlrow olleh"
	 * "🌸🐔".reverse(); // "🐔🌸"
	 * ```
	 *
	 * @returns Reversed string.
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
	 * @example
	 * ```javascript
	 * "🌸🐔".toArray(); // ["🌸", "🐔"]
	 * ```
	 *
	 * @returns An array in string form containing each character in the source string.
	 */
	toArray(): string[];

	/**
	 * Insert a character between every two characters in a string.
	 *
	 * @example
	 * ```javascript
	 * "hello world!".inTwo("|"); // "h|e|l|l|o| |w|o|r|l|d|!"
	 * ```
	 *
	 * @param sep - Separator. The default is `,`.
	 * @returns The processed string.
	 */
	inTwo(sep?: string): string;

	/**
	 * Is the string a certain string in the function parameters.
	 *
	 * @param list - To test whether a string array is included.
	 * @returns Included.
	 */
	in<const T extends string>(...list: (T | undefined | null)[]): this is T;

	/**
	 * Remove all the white space characters in the string.
	 *
	 * @example
	 * ```javascript
	 * "hello world ! ! !".removeSpace(); // "helloworld!!!"
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
	 * "hello world!".holeString(3, 6); // "helworld!"
	 * ```
	 */
	holeString(holeStart: number, holeEnd: number): string;
}
