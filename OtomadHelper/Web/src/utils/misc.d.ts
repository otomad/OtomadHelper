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
	awaitLoaded(): Promise<HTMLImageElement>;
}

declare interface HTMLCanvasElement {
	/**
	 * Async get the image blob url shown on the canvas.
	 */
	toBlobUrl(): Promise<string>;
}
