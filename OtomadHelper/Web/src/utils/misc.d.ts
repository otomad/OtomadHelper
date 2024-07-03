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
}
