import dedent from "dedent";
import { spacing } from "pangu";

{ // Init string extensions
	String.prototype.count = function (...substrings) {
		return sum(...substrings.map(substring => this.split(substring).length - 1));
	};

	String.prototype.reverse = function () {
		return Array.from(this).reverse().join("");
	};

	String.prototype.toBoolean = function () {
		return this.trim().toLowerCase() !== "false";
	};

	String.prototype.toArray = function () {
		return Array.from(this);
	};

	String.prototype.interpose = function (separator = ",") {
		// return this.replaceAll(/(?<=.)(?=.)/gu, separator);
		return Array.from(this).join(separator);
	};

	String.prototype.in = function (this: undefined, ...list) {
		return list.includes(this);
	} as string["in"];

	String.prototype.removeSpaces = function () {
		return this.replaceAll(/\s/g, "");
	};

	String.prototype.holeString = function (start, end?: number) {
		return this.realSlice(0, start) + this.realSlice(end ?? start + 1);
	};

	String.prototype.dedent = function () {
		return dedent(this.valueOf());
	};

	String.prototype.toCapitalized = function (keepCase = false) {
		const decapitated = this.realSlice(1);
		return this.realCharAt(0).toLocaleUpperCase() + (keepCase ? decapitated : decapitated.toLocaleLowerCase());
	};

	String.prototype.nowrapPerWord = function () {
		return this.replaceAll(" ", "\xa0");
	};

	String.prototype.nowrapPerChar = function () {
		return this.interpose("\u2060");
	};

	String.prototype.replaceStart = function (start, replacement = "") {
		if (!this.startsWith(start)) return this.valueOf();
		else return replacement + this.slice(start.length);
	};

	String.prototype.replaceEnd = function (end, replacement = "") {
		if (!this.endsWith(end)) return this.valueOf();
		else return this.slice(0, -end.length) + replacement;
	};

	String.prototype.with = function (index, character) {
		return Array.from(this).with(index, String(character)).join("");
	};

	String.prototype.padBoth = function (maxLength: number, fillString = " ", uneven: "start" | "end" = "start"): string {
		const padMethods = ["padStart", "padEnd"] as const satisfies string[];
		if (uneven === "start") padMethods.reverse();
		return maxLength <= this.length ? this.valueOf() : this[padMethods[0]]((this.length + maxLength) / 2, fillString)[padMethods[1]](maxLength, fillString);
	};

	String.prototype.toTitleCase = function () {
		return this.mapWords(word => word.realCharAt(0).toLocaleUpperCase() + word.realSlice(1));
	};

	String.prototype.mapWords = function (convert) {
		return this.replaceAll(/\S+/g, convert);
	};

	String.prototype.areAllUpper = function () {
		return this.toLocaleUpperCase() === this.valueOf();
	};

	defineGetterInPrototype(String, "realLength", function () {
		return this[Symbol.iterator]().length;
	});

	String.prototype.realCharAt = function (index) {
		return this[Symbol.iterator]().at(index);
	};

	String.prototype.realCodePointAt = function (index) {
		return this.realCharAt(index).codePointAt(0)!;
	};

	String.prototype.realSlice = function (from, end) {
		return [...this].slice(from, end).join("");
	};

	makePrototypeKeysNonEnumerable(String);
}

/**
 * Verify that the object's toString output is meaningful and human-readable, rather than the default implementation "[object *]".
 * @param test - The object to test.
 * @returns The object's toString output doesn't match "[object *]".
 */
// eslint-disable-next-line @typescript-eslint/no-wrapper-object-types
export const canToString = (test: Object | undefined | null): test is string => !!test && !test.toString().match(/^\[object .*\]$/);

export { default as replacerWithGroups } from "helpers/replacerWithGroups";

/**
 * Adds spacing between Chinese and English characters in the given text.
 *
 * @param text - The input text string to process.
 * @returns The processed text with spacing added.
 */
export function panguSpacing(text: string): string;
/**
 * Adds spacing between Chinese and English characters in the given text.
 *
 * @param text - The input text to process. Can be a string, number, or bigint.
 * If the input is not a string, it will be returned as-is.
 * @returns The processed text with spacing added if the input is a string,
 * or the original input if it is not a string.
 */
export function panguSpacing(text: Readable): Readable;
export function panguSpacing(text: Readable) {
	if (typeof text !== "string") return text;
	return spacing(text);
}
