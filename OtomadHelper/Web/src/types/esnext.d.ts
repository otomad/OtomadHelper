export { };

// #region Misc
declare global {
	interface Element {
		/**
		 * The Element.scrollIntoViewIfNeeded() method scrolls the current element into the visible area of the
		 * browser window if it's not already within the visible area of the browser window. If the element is
		 * already within the visible area of the browser window, then no scrolling takes place. This method is
		 * a proprietary variation of the standard Element.scrollIntoView() method.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Element/scrollIntoViewIfNeeded)
		 */
		scrollIntoViewIfNeeded(centerIfNeeded?: boolean): void;
	}

	interface ScreenOrientation {
		/**
		 * The `lock()` property of the `ScreenOrientation` interface locks the orientation of the containing document to the specified orientation.
		 *
		 * [MDN Reference](https://developer.mozilla.org/en-US/docs/Web/API/ScreenOrientation/lock)
		 * @see https://github.com/microsoft/TypeScript-DOM-lib-generator/issues/1615 Microsoft unexpected remove it from TypeScript library.
		 *
		 * @param type - An orientation lock type.
		 * @returns A Promise that resolves after locking succeeds.
		 */
		lock(type: "any" | "natural" | "landscape" | "portrait" | OrientationType): Promise<void>;
	}
}
// #endregion

// #region Compatible with Internet Explorer
declare global {
	interface Window {
		/**
		 * The ActiveX object that is only available in Internet Explorer, higher versions of browsers will return undefined.
		 *
		 * [Microsoft Reference](https://learn.microsoft.com/openspecs/ie_standards/ms-es5ex/64528856-d0ab-4639-a8a0-625040a88c93)
		 */
		ActiveXObject: undefined;
	}

	interface Document {
		/**
		 * Text selection objects available in Internet Explorer.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Selection)
		 */
		selection: Selection | null;
	}
}
// #endregion

// #region View Transitions API
declare global {
	interface Document {
		/**
		 * The startViewTransition() method of the Document interface starts a new view transition and returns
		 * a ViewTransition object to represent it.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Document/startViewTransition)
		 */
		startViewTransition(updateCallback: () => Promise<void | unknown> | void | unknown): ViewTransition;
	}
}

interface ViewTransition {
	finished: Promise<void>;
	ready: Promise<void>;
	updateCallbackDone: Promise<void>;
	skipTransition(): void;
}

interface CSSStyleDeclaration {
	viewTransitionName: string;
}
// #endregion

// #region Set Methods
declare global {
	interface SetLike<T> {
		/**
		 * The number of (unique) elements in Set.
		 */
		readonly size: number;
		/**
		 * @returns A boolean indicating whether an element with the specified value exists in the Set or not.
		 */
		has(value: T): boolean;
		/**
		 * Despite its name, returns an iterable of the values in the set.
		 */
		keys(): IterableIterator<T>;
	}

	interface Set<T> {
		/**
		 * The intersection() method of Set instances takes a set and returns a new set containing elements in
		 * both this set and the given set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/intersection)
		 */
		intersection(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The union() method of Set instances takes a set and returns a new set containing elements which are
		 * in either or both of this set and the given set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/union)
		 */
		union(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The difference() method of Set instances takes a set and returns a new set containing elements in
		 * this set but not in the given set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/difference)
		 */
		difference(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The symmetricDifference() method of Set instances takes a set and returns a new set containing elements
		 * which are in either this set or the given set, but not in both.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/symmetricDifference)
		 */
		symmetricDifference(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The isSubsetOf() method of Set instances takes a set and returns a boolean indicating if all elements of
		 * this set are in the given set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/isSubsetOf)
		 */
		isSubsetOf(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The isSupersetOf() method of Set instances takes a set and returns a boolean indicating if all elements
		 * of the given set are in this set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/isSupersetOf)
		 */
		isSupersetOf(other: Set<T> | SetLike<T>): Set<T>;

		/**
		 * The isDisjointFrom() method of Set instances takes a set and returns a boolean indicating if this set has
		 * no elements in common with the given set.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/JavaScript/Reference/Global_Objects/Set/isDisjointFrom)
		 */
		isDisjointFrom(other: Set<T> | SetLike<T>): Set<T>;
	}
}
// #endregion
