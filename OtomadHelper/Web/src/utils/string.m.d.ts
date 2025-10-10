declare interface String {
	/**
	 * Count the number of specific characters or substrings in a string.
	 *
	 * @param substrings - The characters or substrings to search for.
	 * @returns Number of occurrences.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".count("l")); // Output: 3
	 * console.log("pen pineapple apple pen".count("apple")); // Output: 7
	 * console.log("pen pineapple apple pen".count("apple")); // Output: 2
	 * ```
	 */
	count(...substrings: string[]): number;

	/**
	 * Reverse the order of strings.
	 *
	 * @remarks
	 * Using `Array.from()` can avoid the problem of characters other than Unicode BMP not be reversed properly.
	 *
	 * @returns Reversed string.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".reverse()); // Output: "!dlrow olleh"
	 * console.log("🌸🐔".reverse()); // Output: "🐔🌸"
	 * console.log("🌸🐔".split("").reverse().join("")); // Output: "\uDC14🜸\uD83C"
	 * ```
	 */
	reverse(): string;

	/**
	 * Convert the string to a boolean.
	 *
	 * Used to solve problems such as `"false" == false` being false.
	 *
	 * @returns Boolean.
	 *
	 * @example
	 * ```javascript
	 * console.log("  fAlSe   ".toBoolean()); // Output: false
	 * console.log("something else".toBoolean()); // Output: true
	 * ```
	 */
	toBoolean(): boolean;

	/**
	 * Convert a string to a character array similar to C, where each item in the array corresponds to each character in the string.
	 *
	 * The function uses `Array.from()` which can avoid the problem of characters other than Unicode BMP are split into UTF-16 surrogate pairs.
	 *
	 * @deprecated Use `[...string]` instead.
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
	 * Insert a separator between every two characters in a string.
	 *
	 * @param separator - Separator. Defaults to `,`.
	 * @returns The processed string.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".interpose("|")); // Output: "h|e|l|l|o| |w|o|r|l|d|!"
	 * console.log("hello world!".interpose()); // Output: "h,e,l,l,o, ,w,o,r,l,d,!"
	 * ```
	 */
	interpose(separator?: string): string;

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
	 * (["foo", "bar"] as const).includes(a) ? // Error: Type '"baz"' is not assignable to type '"foo" | "bar"'
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
	 * console.log("hello world ! ! !".removeSpaces()); // Output: "helloworld!!!"
	 * ```
	 */
	removeSpaces(): string;

	/**
	 * The hole is `[holeStart, holeEnd)`.
	 *
	 * And `String.prototype.hole(holeStart, holeEnd)` will create a substring which contains `[0, holeStart) ∪ [holeEnd, end]`.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".hole(3, 6)); // Output: "helworld!"
	 * ```
	 */
	hole(holeStart: number, holeEnd: number): string;
	/**
	 * Create a substring which contains `[0, hole) ∪ (hole, end]`.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".hole(4)); // Output: "hell world!"
	 * ```
	 */
	hole(hole: number): string;

	/**
	 * A string that gets so long you need to break it over multiple lines.
	 * Luckily dedent is here to keep it readable without lots of spaces ending up in the string itself.
	 *
	 * Leading and trailing lines will be trimmed, so you can write something like this and have it work as you expect:
	 *
	 *   * how convenient it is
	 *   * that I can use an indented list
	 *     - and still have it do the right thing
	 *
	 * That's all.
	 *
	 * @see https://www.npmjs.com/package/dedent
	 */
	dedent(): string;

	/**
	 * Capitalizes the first character of a string.
	 *
	 * @param keepCase - If true, maintains the case of characters after the first one. If false, converts them to lowercase. Default is false.
	 * @returns A new string with the first character capitalized and the rest either maintained or converted to lowercase based on the `keepCase` parameter.
	 *
	 * @example
	 * ```javascript
	 * console.log("getElementById".toCapitalized()); // "Getelementbyid"
	 * console.log("getElementById".toCapitalized(true)); // "GetElementById"
	 * ```
	 */
	toCapitalized(keepCase?: boolean): string;

	/**
	 * Make sure there are no line breaks between every two words in the string.
	 * Applicable to languages that use spaces as a boundary between words.
	 *
	 * This will replace all the space characters in the string with the No-Break Space (U+00A0 / `&nbsp;`) characters.
	 * Note that other white space characters are not affected.
	 *
	 * @example
	 * ```javascript
	 * console.log("hello world!".nowrapPerWord()); // "hello\xA0world!"
	 * ```
	 */
	nowrapPerWord(): string;

	/**
	 * Make sure there are no line breaks between any two characters in the string.
	 * Applicable to languages that do not use spaces as a boundary between words.
	 *
	 * This will insert a Word Joiner (U+2060; formerly Zero-Width No-Break Space, U+FEFF) character between any two characters in the string.
	 *
	 * @example
	 * ```javascript
	 * console.log("你好世界！".nowrapPerChar()); // "你\u2060好\u2060世\u2060界\u2060！"
	 * ```
	 */
	nowrapPerChar(): string;

	/**
	 * Check if the string starts with the specified substring. If it is, replace it with a new substring.
	 * @param start - The starting substring to check.
	 * @param replacement - The new substring to replace. Defaults to empty string.
	 * @returns If the string does not start with the specified substring, return the original string;
	 * otherwise, return the replaced new string.
	 * @example
	 * ```javascript
	 * console.log("id".replaceStart("data-")); // Output: "id"
	 * console.log("data-id".replaceStart("data-")); // Output: "id"
	 * console.log("data-id".replaceStart("data-", "v-bind:")); // Output: "v-bind:id"
	 * ```
	 */
	replaceStart(start: string, replacement?: string): string;

	/**
	 * Check if the string ends with the specified substring. If it is, replace it with a new substring.
	 * @param end - The ending substring to check.
	 * @param replacement - The new substring to replace. Defaults to empty string.
	 * @returns If the string does not end with the specified substring, return the original string;
	 * otherwise, return the replaced new string.
	 * @example
	 * ```javascript
	 * console.log("id".replaceEnd("-id")); // Output: "id"
	 * console.log("data-id".replaceEnd("-id")); // Output: "data"
	 * console.log("data-id".replaceEnd("-id", "-for")); // Output: "data-for"
	 * ```
	 */
	replaceEnd(end: string, replacement?: string): string;

	/**
	 * Similar to C, replacing the character of an index in a string.
	 *
	 * @remarks
	 * Copies the string, then overwrites the character at the provided index with the given value.
	 * If the index is negative, then it replaces from the end of the array.
	 *
	 * @note
	 * Since the string in JavaScript belong to primitive type, it cannot be directly modified like C,
	 * and must be assigned to itself.
	 *
	 * @example
	 * ```javascript
	 * let str = "hello world!";
	 * str = str.with(1, "a"); // "hallo world!"
	 * ```
	 *
	 * @param index - The index of the character to overwrite.
	 * If the index is negative, then it replaces from the end of the string.
	 * @param character - The character to write into the copied string.
	 * @returns The copied string with the updated character.
	 */
	with(index: number, character: string): string;

	/**
	 * Return a centered string of length width.
	 * Padding is done using the specified fill character (default is a space).
	 *
	 * @remarks
	 * Pads a string on both sides with a specified fill string until it reaches the desired maximum length.
	 * If the total padding required is uneven, the `uneven` parameter determines whether the extra character is added to the start or end.
	 *
	 * This is the JavaScript version of `str.center` function in Python.
	 *
	 * @param maxLength - The desired total length of the resulting string after padding.
	 * @param fillString - The string to use for padding. Defaults to a space.
	 * @param uneven - Determines where to place the extra padding character if the padding is uneven.
	 * Use `"start"` to add the extra character to the start, or `"end"` to add it to the end. Defaults to `"start"`.
	 * @returns The padded string, or the original string if it is already at least `maxLength` characters long.
	 */
	padBoth(maxLength: number, fillString?: string, uneven?: "start" | "end"): string;

	/**
	 * Converts a string to title case, capitalizing the first letter of each word.
	 *
	 * @note It will just capitalize the first letter of each word brainlessly and will not follow the title case rules
	 * in English grammar (such as only dealing with real words, etc.). If you want to get a more standard title case,
	 * please visit [the website](https://titlecaseconverter.com/).
	 *
	 * @returns The input string with the first letter of each word capitalized.
	 *
	 * @example
	 * ```javascript
	 * "hello world".toTitleCase(); // Returns "Hello World"
	 * ```
	 */
	toTitleCase(): string;

	/**
	 * Applies a conversion function to each word in the input string and returns the resulting string.
	 *
	 * A "word" is defined as a sequence of non-whitespace (space, line break, tab, etc.) characters.
	 *
	 * @param convert - A function that takes a word as input and returns the converted word.
	 * @returns The input string with each word replaced by the result of the `convert` function.
	 *
	 * @example
	 * ```javascript
	 * "hello world".mapWords(word => word[0].toUpperCase() + word.slice(1)); // "Hello World"
	 * ```
	 */
	mapWords(convert: (word: string) => string): string;

	/**
	 * Check if all letters in the string are uppercase (ignoring numbers, punctuation, etc.).
	 * @returns Are all letters uppercase?
	 *
	 * @example
	 * ```javascript
	 * "HEllO WORlD".areAllUpper(); // false
	 * "HELLO WORLD".areAllUpper(); // true
	 * ```
	 */
	areAllUpper(): boolean;

	/**
	 * Gets the real length (Unicode characters code length) of the string, without regard to surrogate pairs.
	 */
	readonly realLength: number;

	/**
	 * Returns the real character at the specified index, without regard to surrogate pairs.
	 *
	 * @param index - The zero-based index of the desired code unit. A negative index will count back from the last item.
	 */
	realCharAt(index: number): string;

	/**
	 * Returns the real Unicode value of the character at the specified location, without regard to surrogate pairs.
	 *
	 * @param index - The zero-based index of the desired character. A negative index will count back from the last item.
	 */
	realCodePointAt(index: number): number;

	/**
	 * Returns a section of a string, without regard to surrogate pairs.
	 *
	 * @param start - The index to the beginning of the specified portion of string.
	 * @param end - The index to the end of the specified portion of string. The substring includes the characters up to,
	 * but not including, the character indicated by end. If this value is not specified, the substring continues to the
	 * end of string.
	 */
	realSlice(start?: number, end?: number): string;
}
