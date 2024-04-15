/**
 * Variable name helper class.
 *
 * Used to easily convert a variable name into camelCase, kebab-case, and so on.
 */
export default class VariableName {
	#words: string[] = [];

	constructor(str: string) {
		this.value = str;
	}

	/**
	 * Reset the value.
	 * @param str - Any form of variable name.
	 */
	set value(str: string) {
		str = str.trim();
		if (str.includes("_"))
			this.#words = str.split("_");
		else if (str.includes("-"))
			this.#words = str.split("-");
		else {
			const splitted = str.replaceAll(/(?!^)([A-Z])/g, " $1").replaceAll(/(\d+)/g, " $1 ").replaceAll(/\s+(?=\s)|^\s+|\s+$/g, "");
			this.#words = splitted.split(" ");
		}
	}

	/**
	 * Convert to kebab-case.
	 */
	get kebab() {
		return this.#words.join("-").toLowerCase();
	}

	/**
	 * Convert to snake_case.
	 */
	get snake() {
		return this.#words.join("_").toLowerCase();
	}

	/**
	 * Convert to CONSTANT_CASE.
	 */
	get const() {
		return this.#words.join("_").toUpperCase();
	}

	/**
	 * Convert to PascalCase.
	 */
	get pascal() {
		return this.#words.map(word => capitalize(word)).join("");
	}

	/**
	 * Convert to camelCase.
	 */
	get camel() {
		return this.#words.map((word, i) => i ? capitalize(word) : word.toLowerCase()).join("");
	}

	/**
	 * Convert to lowercase without any separators.
	 */
	get lower() {
		return this.#words.join("").toLowerCase();
	}

	/**
	 * Convert to UPPERCASE without any separators.
	 */
	get upper() {
		return this.#words.join("").toUpperCase();
	}

	/**
	 * Convert to word case, separated by spaces, all in lowercase.
	 */
	get words() {
		return this.#words.join(" ").toLowerCase();
	}

	/**
	 * Convert to Sentence case, separated by spaces, with only the first letter of the sentence capitalized.
	 */
	get sentence() {
		return this.#words.map((word, i) => !i ? capitalize(word) : word.toLowerCase()).join(" ");
	}

	/**
	 * Convert to Title Case, separated by spaces, with all first letters of words capitalized.
	 */
	get title() {
		return this.#words.map(word => capitalize(word)).join(" ");
	}

	/**
	 * Convert to --css-custom-property-name-form, which is kebab-case with two dashes as a prefix.
	 */
	get cssVar() {
		return "--" + this.kebab;
	}
}

/**
 * Convert a word to uppercase the first letter and lowercase other letters.
 * @param str - Word.
 * @returns Capitalize the first letter and lowercase other letters.
 */
function capitalize(str: string) {
	return str[0].toUpperCase() + str.slice(1).toLowerCase();
}
