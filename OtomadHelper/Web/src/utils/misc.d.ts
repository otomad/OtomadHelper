declare interface Body {
	/**
	 * Fetch data with XML.
	 * @see https://stackoverflow.com/questions/37693982
	 */
	xml(): Promise<Document>;
}
