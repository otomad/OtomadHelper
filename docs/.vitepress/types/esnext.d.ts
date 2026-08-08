// Polyfill for element scoped view transition API.
// How about directly add it to Node (Element and Document's shared ancestor)?
declare interface Node {
	startViewTransition(callbackOptions?: ViewTransitionUpdateCallback): void;
}

declare interface Element {
	/**
	 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Element/scrollIntoView)
	 */
	scrollIntoView(arg?: boolean | ScrollIntoViewOptions): Promise<{ interrupted: boolean }>;
}
