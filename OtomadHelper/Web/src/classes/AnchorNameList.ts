/**
 * A utility class for managing CSS anchor-name property values.
 *
 * Behaves like `classList`, supports multiple anchor names, CSS escaping, and array-like / set-like operations.
 *
 * @see https://developer.mozilla.org/en-US/docs/Web/CSS/anchor-name
 */
export class AnchorNameList implements Iterable<string> {
	/** Internal storage for normalized, escaped anchor names */
	readonly #anchors: Set<string> = new Set();

	/**
	 * Create an AnchorNameList instance.
	 * @param initialValue - Raw `anchor-name` string from `element.style.anchorName` (e.g., "--name, --another").
	 */
	constructor(initialValue: string = "") {
		if (!initialValue) return;

		const parsed = initialValue
			.split(",")
			.map(item => item.trim())
			.filter(Boolean);

		this.#anchors.adds(...parsed);
	}

	/**
	 * Get the number of registered anchor names.
	 */
	get size(): number {
		return this.#anchors.size;
	}

	/**
	 * Add one or more anchor names (automatically escaped & deduplicated).
	 * @param names - Anchor names to add (will be escaped with `CSS.escape`).
	 */
	add(...names: string[]): void {
		for (const rawName of names) {
			const escaped = CSS.escape(rawName);
			this.#anchors.add(escaped);
		}
	}

	/**
	 * Remove one or more anchor names (supports strings or predicate callback)
	 * @param names - Anchor names to remove (strings) OR a predicate function to filter names
	 */
	remove(...names: (string | ((name: string) => boolean))[]): void {
		for (const name of names) {
			if (typeof name === "string") {
				const escaped = CSS.escape(name);
				this.#anchors.delete(escaped);
			}

			if (typeof name === "function")
				for (const anchor of this.#anchors)
					if (name(anchor))
						this.#anchors.delete(anchor);
		}
	}

	/**
	 * Check if an anchor name exists.
	 * @param name - Anchor name to check (auto-escaped).
	 * @returns True if the anchor name exists.
	 */
	has(name: string): boolean {
		return this.#anchors.has(CSS.escape(name));
	}

	/**
	 * Toggle an anchor name (add if missing, remove if present).
	 * @param name - Anchor name to toggle (auto-escaped).
	 * @param force - Optional: force add (true) or force remove (false).
	 * @returns True if the anchor exists after toggle.
	 */
	toggle(name: string, force?: boolean): boolean {
		const escaped = CSS.escape(name);
		const exists = this.#anchors.has(escaped);

		if (force || !exists) {
			this.add(name);
			return true;
		} else {
			this.remove(name);
			return false;
		}
	}

	/**
	 * Convert the anchor list back to a valid CSS anchor-name string.
	 * @returns Comma-separated string (e.g., "--name, --another").
	 */
	toString(): string {
		return [...this.#anchors].join(", ");
	}

	/**
	 * Support iteration (for...of loops).
	 * @returns Iterator for escaped anchor names.
	 */
	[Symbol.iterator](): Iterator<string> {
		return this.#anchors[Symbol.iterator]();
	}
}
