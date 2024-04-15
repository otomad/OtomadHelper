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
	 * @returns An array in string form containing each character in the source string.
	 */
	toArray(): string[];

	/**
	 * Insert a character between every two characters in a string.
	 * @param sep - Separator. The default is `,`.
	 * @returns The processed string.
	 */
	inTwo(sep?: string): string;

	/**
	 * Is the string a certain string in the function parameters.
	 * @param list - To test whether a string array is included.
	 * @returns Included.
	 */
	in<const T extends string>(...list: (T | undefined | null)[]): this is T;
}
