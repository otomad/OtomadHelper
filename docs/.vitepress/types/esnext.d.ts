// Polyfill for element scoped view transition API.
// How about directly add it to Node (Element and Document's shared ancestor)?
declare interface Node {
	startViewTransition(callbackOptions?: ViewTransitionUpdateCallback): void;
}
