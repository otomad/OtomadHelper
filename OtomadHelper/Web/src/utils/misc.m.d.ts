declare interface Body {
	/**
	 * Fetch data with XML.
	 * @see https://stackoverflow.com/questions/37693982
	 */
	xml(): Promise<Document>;
}

declare interface HTMLImageElement {
	/**
	 * Wait for the image is just loaded.
	 */
	waitForLoaded(): Promise<HTMLImageElement>;
}

declare interface HTMLCanvasElement {
	/**
	 * Async get the image blob url shown on the canvas.
	 */
	toBlobURL(): Promise<string>;
}

declare interface Element {
	/**
	 * Get an array from the specified element that traces back to the root element.
	 *
	 * Used to find the event target and bubble up to find the required element.
	 *
	 * @returns An array of the specified element that traces back to the root element.
	 */
	readonly path: Element[];

	/**
	 * Get the index of the current element within the specified ancestor's child elements.
	 *
	 * This method differs from standard DOM traversal by strictly considering **elements**
	 * (nodes of type `ELEMENT_NODE`), ignoring other node types (e.g. `TextNode`, `CommentNode`).
	 *
	 * @param parent - Optional target ancestor element or CSS selector string.
	 *
	 * When undefined:
	 * - Uses immediate parent element.
	 * - Returns `-1` if no parent exists.
	 *
	 * When specified:
	 * - Traverses ancestor chain to find matching element.
	 * - Returns index within first matching ancestor's child elements.
	 * - Returns `-1` if no matching ancestor found.
	 *
	 * @returns Zero-based index position in these scenarios:
	 * - Current element's position in parent/ancestor's child elements.
	 * - `-1` if: no parent exists | invalid ancestor | element not found.
	 *
	 * @example Basic usage
	 * ```html
	 * <div id="grandfather">
	 *     <p id="uncle"></p>
	 *     <p id="father">
	 *         <span id="brother"></span>
	 *         <span id="sister"></span>
	 *         <span id="me"></span>
	 *     </p>
	 * </div>
	 * ```
	 * ```javascript
	 * me.indexIn(); // Returns 2 (position in p#father's child elements)
	 * me.indexIn(grandfather); // Returns 1 (position of p#father in div#grandfather's children)
	 * me.indexIn("#grandfather"); // Returns [1, div#grandfather] (also returns the ancestor element)
	 * ```
	 *
	 * @example Node type filtering
	 * ```html
	 * <div>
	 *     Text
	 *     <!-- Comments -->
	 *     <span></span>
	 * </div>
	 * ```
	 * ```javascript
	 * span.indexIn(); // Returns 0 (only counts <span> as valid element sibling)
	 * ```
	 *
	 * @example Ancestor search
	 * ```javascript
	 * document.documentElement.indexIn("body"); // Returns -1 (html is not child of body)
	 * ```
	 */
	indexIn(parent?: Node | null): number;
	/**
	 * Get the index of the current element within the specified ancestor's child elements.
	 *
	 * @param selector - Optional target ancestor element or CSS selector string.
	 * - Traverses ancestor chain to find matching element.
	 * - Returns index within first matching ancestor's child elements.
	 * - Returns `-1` if no matching ancestor found.
	 *
	 * @returns A tuple.
	 * - index: Zero-based index position in these scenarios:
	 *   - Current element's position in parent/ancestor's child elements.
	 *   - `-1` if: no parent exists | invalid ancestor | element not found.
	 * - ancestor: The ancestor element matches the selector string.
	 */
	indexIn(selector: string): [index: number, ancestor: Element];

	/**
	 * Enhanced query selector that checks the current element first before
	 * searching descendants. Combines the functionality of `Element.matches()` and
	 * `Element.querySelector()` in a single method call.
	 *
	 * @param selector - CSS selector to test against.
	 * @returns Returns:
	 * - Current element if it matches selector.
	 * - First matching descendant element (via querySelector).
	 * - null if no matches found in either case.
	 *
	 * @example
	 * ```html
	 * <div class="container"><p class="target"></p></div>
	 * ```
	 * ```javascript
	 * const div = document.querySelector(".container");
	 * div.querySelector(".container"); // Returns null
	 * div.querySelectorWithSelf(".container"); // Returns div element itself
	 * div.querySelectorWithSelf(".target"); // Returns p element
	 * div.querySelectorWithSelf("section"); // Returns null
	 * ```
	 */
	querySelectorWithSelf: Element["querySelector"];
}

declare interface DOMTokenList {
	/**
	 * Returns true if any of tokens are present, and false for none of them.
	 *
	 * @example
	 * ```typescript
	 * el.classList.containsAny("foo", "bar");
	 * // Equivalent to
	 * (el.classList.contains("foo") || el.classList.contains("bar"));
	 * ```
	 */
	containsAny(...tokens: string[]): boolean;
}

declare interface Console {
	/**
	 * Show image in console.
	 * @see https://gist.github.com/mikamboo/96fcba65822cf088ce410a95dce9f5b5
	 */
	image(url: string, size?: number): Promise<void>;
}

declare interface HTMLInputElement {
	/**
	 * Select all text, scroll into view if needed, and focus the input element.
	 */
	selectAndFocus(): void;
}

declare interface Symbol {
	/**
	 * Determines whether the provided symbol is a global symbol.
	 *
	 * A global symbol is one that was created using `Symbol.for()`, which registers the symbol
	 * in the global symbol registry. This getter checks if the symbol exists in the registry.
	 *
	 * @returns The symbol is a global symbol?
	 *
	 * @example
	 * ```javascript
	 * Symbol("foo").isGlobal; // false
	 * Symbol.for("foo").isGlobal; // true
	 * ```
	 */
	readonly isGlobal: boolean;
}
