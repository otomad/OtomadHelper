type ReactElementType = string | React.JSXElementConstructor<Any>;

/**
 * Determine whether a component is an instance of a certain component type.
 * @remarks It seems that the specified component type appears to require placement in a separate file,
 * rather than in the same file as where the function is invoked.
 * @template T - React element type.
 * @param instance - Component instance.
 * @param component - Component class or function component.
 * @param looseness - Specify how to determine the equality of the type of instance and the component.
 * - `"strong"` - Check if they point to pointers to the same function.
 * - `"weak"` - Check if their function content strings are same.
 * - `"weakest"` - Check if their functions' name and argument count are same.
 * @returns Is its instance?
 */
export function isReactInstance<T extends ReactElementType>(instance: ReactNode, component: T, looseness: "strong" | "weak" | "weakest" = "strong"): instance is ReactElementOf<T> {
	return React.isValidElement(instance) && isObject(instance) && "type" in instance && (instance.type === component ||
		looseness === "weak" ? areFunctionsApproxEqual(instance.type, component) :
		looseness === "weakest" ? areFunctionsGenerallyEqual(instance.type, component) :
		false
	);
}

/**
 * Check if a React Node is a valid React Element and **not a React Fragment**.
 *
 * @remarks
 * In React 19, `React.Fragment` won't accept any unknown props, when you unexpected pass other props to
 * `React.Fragment`, you will get a warning:
 *
 * > Invalid prop supplied to `React.Fragment`. React.Fragment can only have `key` and `children` props.
 *
 * It is best to disqualify the `React.Fragment` when you are testing `React.isValidElement()`.
 *
 * @template P - Asserts the props of the component.
 * @param object - The React Node to test.
 * @returns The React Node is a valid React Element and **not a React Fragment**?
 */
export function isValidElement<P>(object: {} | null | undefined): object is ReactElement<P> {
	return React.isValidElement(object) && object?.type !== React.Fragment;
}

/**
 * Prevent event bubbling and default behavior simultaneously.
 * @param event - Event (Vanilla JavaScript event or React wrapped event).
 */
export function stopEvent(event?: Pick<Event, "preventDefault" | "stopPropagation">) {
	if (!event) return;
	// React's self-declared Event type does not match the built-in Event type.
	event.preventDefault();
	event.stopPropagation();
}

type AdditionalProps = AnyObject | ((props: AnyObject, element: ReactElement) => AnyObject);
/**
 * Clones the provided children, replacing any refs with the provided nodeRef.
 * @remarks Both render tree parent and declaration tree parent own the ref of child.
 * @param children - The ReactNode to clone and replace refs.
 * @param nodeRef - The RefObject to use as the new ref for any cloned elements with a ref.
 * @param additionalProps - Add additional custom props. You can also use a handler to return by getting the exist props, or keys.
 * @returns The cloned children with updated refs.
 */
export function cloneRef(children: ReactNode, nodeRef: MiscRef<Element | null>, additionalProps?: AdditionalProps): ReactNode {
	return h(
		Fragment,
		null,
		flattenReactChildren(children).map((child: ReactNode) => {
			if (!isValidElement<RefAttributes>(child)) return child;
			const existedRef = child.props.ref;
			const ref = mergeRefs(nodeRef, existedRef);
			if (typeof additionalProps === "function") additionalProps = additionalProps(child.props, child);
			return React.cloneElement(child, { ref, key: child.key, ...additionalProps });
		}),
	);
}

interface IsInPathOptions {
	/** If the detection requirements are still not met after bubbling to these HTML DOM nodes, stop bubbling. */
	stopAt?: DetectInPathType[];
}

/**
 * Converts a given target to an `Element` if possible.
 *
 * This function handles several types of input:
 * - If the target is a ref object, it extracts its value.
 * - If the target is an `Event`, it uses the event's target.
 * - If the resulting target is not an `Element`, it returns `null`.
 *
 * @template TElement - Element type.
 * @param target - The target to convert, which can be a ref object, an `Event`, or an `Element`.
 * @returns The corresponding `Element` if conversion is possible, otherwise `null`.
 */
// eslint-disable-next-line @typescript-eslint/no-unnecessary-type-parameters
export function targetToElement<TElement extends Element = Element>(target: TargetType): TElement | null;
/**
 * Resolves a given target to a DOM Element or a CSS selector string, or returns null if not applicable.
 *
 * @param target - The target to resolve. Can be a ref object, an object with a `target` property, a DOM Element, or a CSS selector string.
 * @param acceptCssSelectorQueryString - If true, allows returning a CSS selector string as a valid result.
 * @returns The resolved DOM Element, a CSS selector string (if accepted), or null if the target cannot be resolved.
 */
export function targetToElement(target: DetectInPathType, acceptCssSelectorQueryString: true): Element | string | null;
export function targetToElement(target: DetectInPathType, acceptCssSelectorQueryString?: boolean) {
	if (isRefObject(target)) target = toValue(target);
	if (isObject(target) && "target" in target) target = target.target;
	if (target instanceof Element || acceptCssSelectorQueryString && typeof target === "string") return target;
	return null;
}

/**
 * Get an array from the specified element that traces back to the root element.
 *
 * Used to find the event target and bubble up to find the required element.
 *
 * @param target - HTML DOM node.
 * @returns An array of the specified element that traces back to the root element.
 */
export function getPath(target: TargetType): Element[] {
	target = targetToElement(target);
	if (!target) return [];
	const path: Element[] = [];
	while (target instanceof Element) {
		path.push(target);
		target = target.parentElement;
	}
	return path;
}

/**
 * According to the target node of the mouse event, find whether the element to be queried is or is its
 * ancestor node. For example, find whether the element is clicked, etc.
 *
 * @param target - The target HTML DOM node in the click event.
 * @param elements - The bubbling HTML DOM node to find. If it is a string, it represents the CSS selector
 * to be queried.
 * @returns The element to be queried is or is its ancestor node.
 */
export function isInPath(target: TargetType, ...elements: DetectInPathType[]): boolean;
export function isInPath(...args: [target: TargetType, ...elements: DetectInPathType[], options: IsInPathOptions]): boolean;
export function isInPath(target: TargetType, ...args: (DetectInPathType | IsInPathOptions)[]): boolean {
	const path = getPath(target);
	const [elements, stopAt] = (() => {
		const [elements, { stopAt = [] }] = (() => {
			const last = args.last();
			let options: IsInPathOptions = {}, elements = args as DetectInPathType[];
			if (isObject(last) && typeof last !== "string" && !(last instanceof EventTarget) && !("preventDefault" in last) && !isRefObject(last)) {
				options = last;
				elements = args.toPopped() as DetectInPathType[];
			}
			return [elements, options] as const;
		})();
		const filter = (element: DetectInPathType) => targetToElement(element, true);
		return [elements.map(filter).toCompacted(), stopAt.map(filter).toCompacted()];
	})();
	const matches = (parent: Element, elements: (string | Element)[]) => elements.some(element => {
		if (typeof element === "string") return parent.matches(element);
		return parent === element;
	});
	for (const parent of path) {
		if (matches(parent, elements)) return true;
		if (matches(parent, stopAt)) return false;
	}
	return false;
}

/**
 * Checks if the specified element is a "contents" display type.
 *
 * A "contents" display type means that the element will not create a new block formatting context,
 * and its children will be part of the parent's formatting context.
 *
 * @param element - HTML DOM element.
 * @returns Is the specified element a "contents" display type?
 */
export function isElementContents(element: Element | undefined | null) {
	return !!(element && getComputedStyle(element).display === "contents");
}

/**
 * Checks if the specified element is hidden.
 *
 * This function checks if the specified element is hidden by checking it and its ancestor elements for any hidden
 * reasons, which considered hidden whether any of them have no parent, the `hidden` attribute is set to true, the
 * `display` CSS property is set to `none`, or the `visibility` CSS property is not set to `visible`.
 *
 * @param element - HTML DOM element to check for hidden status.
 * @returns Is the specified element is hidden?
 */
export function isElementHidden(element: Element | undefined | null): element is undefined | null {
	/*
	 * In fact, there is another simpler way to determine whether it and its ancestor are hidden, which is to check
	 * whether the offsetParent property of the specified element is null. However this way has many flaws. For example,
	 * if the parent node of the element to be determined is the root element or node, such as `<body>`, `<head>, `<html>`,
	 * it will be determined incorrectly. And it cannot also determine the situation of the visibility CSS property.
	 */
	const hiddenElement = getPath(element).find(element =>
		!element || // Passed a empty value to the parameter
		(element as HTMLElement).hidden || // `hidden` attribute is set to true
		getComputedStyle(element).display === "none" || // `display` CSS property is set to `none`
		getComputedStyle(element).visibility !== "visible", // `visibility` CSS property is set to `hidden` or `collapse`
	);
	return !!hiddenElement;
}

/**
 * Returns the index of the given element in the child list of its parent, or -1 if the element has no parent.
 * @param element - HTML DOM element.
 * @returns The index of the given element in the child list of its parent.
 */
export function getElementIndex(element: Element) {
	if (!element.parentElement) return -1;
	return [...element.parentElement.children].indexOf(element);
}

/**
 * Create a new image from URL.
 * @param url - Image URL.
 * @returns A Promise that resolves with the result HTML image element.
 */
export function createImageFromUrl(url: string) {
	const image = new Image();
	image.src = url;
	return image.waitForLoaded();
}

type WithAttrsProps<TTag> = Partial<(TTag extends keyof ElementTagNameMap ? FCP<{}, TTag> : PropsOf<TTag>)>;
/**
 * You can reuse the component with some same props or attrs.
 * @note You have to declare it at the top level of a module. You should not declare it in a function or component,
 * otherwise every time that component changes, this entire component will be rerendered.
 * @template TTag - HTML element tag (like `"div"`) or React component constructor (like `MyComponent`).
 * @param tag - HTML element tag (like `"div"`) or React component constructor (like `MyComponent`).
 * @param withProps - Part of the props or attrs of the element.
 * @returns A new component containing the partial props or attrs.
 */
export function withAttrs<TTag extends keyof ElementTagNameMap | React.FC>(tag: TTag, withProps: WithAttrsProps<TTag>) {
	return (props: WithAttrsProps<TTag>) => h(tag, { ...withProps, ...props }) as unknown as TTag;
}

/**
 * If the react node is a string or something like this, then apply aria label.
 * @param node - React node, maybe a string, number, bigint, i18n item, or something else.
 * @returns Plain string or undefined.
 */
export function applyAriaLabel(node: ReactNode) {
	return (typeof node).in("string", "number", "bigint") || isI18nItem(node) ? String(node) : undefined;
}

/**
 * Convert from `CheckState` to `aria-checked`.
 * `CheckState` | `aria-checked`
 * --- | ---
 * checked | true
 * unchecked | false
 * indeterminate | mixed
 * @param checkState - Check box selection status.
 * @returns Aria checked.e
 */
export function checkStateToAriaChecked(checkState: CheckState): React.AriaAttributes["aria-checked"] {
	return ({
		checked: true,
		unchecked: false,
		indeterminate: "mixed",
	} as const)[checkState];
}

/**
 * Returns a new array with all sub-array and sub-React-fragment React nodes concatenated into it recursively.
 * @param children - React children.
 * @returns The flattened React children.
 */
export function flattenReactChildren(children: ReactNode) {
	const flattened: ReactNode[] = [];
	React.Children.forEach(children, child => {
		if (
			React.isValidElement<{ children?: ReactNode }>(child) &&
			child.type === React.Fragment &&
			"children" in child.props
		)
			flattened.push(...flattenReactChildren(child.props.children));
		else if (Array.isArray(child))
			flattened.push(...child.flat(Infinity));
		else
			flattened.push(child);
	});
	return flattened;
}

/**
 * Makes the browser "ignore" user input events for the element?
 * @param inert - Inert?
 */
export function setRootInert(inert: boolean) {
	const root = document.getElementById("root");
	if (root) root.inert = inert;
}

/**
 * Retrieves the first focusable element in the container.
 * @param container - The container element to search within.
 * @returns The first focusable element, or null if none is found.
 */
export function findFirstFocusableElement(container: MaybeRef<Element | null>) {
	container = toValue(container);
	const focusableSelectors = [
		"a[href]",
		"button",
		"textarea",
		"input",
		"select",
		"datalist",
		"[tabindex]",
	];
	const nonFocusableSelectors = [
		"[disabled]",
		"[hidden]",
		"[inert]",
		'[tabindex="-1"]',
		'[aria-disabled="true"]',
		'input[type="hidden"]',
	];
	return container?.querySelector<HTMLElement>(`:is(${focusableSelectors.join(",")}):not(${nonFocusableSelectors.join(",")})`) ?? null;
}

/**
 * Retrieves the neighboring element of a given element based on layout (grid, flex, or block).
 *
 * The function determines the neighbor based on the layout of the parent element:
 * - For non-grid layouts, it considers the order of siblings and adjusts for `flex-direction` and RTL settings.
 * - For grid layouts, it calculates the closest neighbor based on the bounding rectangle of the elements.
 *
 * If `autoWrap` is enabled, the function handles edge cases where the element is at the start or end of the sibling list or row.
 *
 * @param el - The target element for which the neighbor is to be found. Can be `null`.
 * @param neighbor - The direction of the neighbor to retrieve. Can be `"left"`, `"right"`, `"top"`, or `"bottom"`.
 * @param siblings - An optional array of sibling elements (children of the parent element).
 * You can provide partially filtered siblings. If not provided, all siblings are used.
 * @param autoWrap - A boolean indicating whether to handle wrapping behavior for edge cases. Defaults to `true`.
 * @returns The neighboring element in the specified direction, or `null` if no neighbor exists.
 */
export function getLayoutNeighbor(el: Element | null, neighbor: "left" | "right" | "top" | "bottom", siblings?: Element[], autoWrap = true) {
	const parent = el?.parentElement;
	if (!el || !parent) return null;
	siblings ??= [...parent.children];
	const index = siblings.indexOf(el);
	if (index === -1) return null;
	const rtl = isRtl();
	const { display, flexDirection } = getComputedStyle(parent);
	if (display !== "grid") {
		let left = siblings.circularAt(index - 1) ?? null, top = left;
		let right = siblings.circularAt(index + 1) ?? null, bottom = right;
		if (display === "flex")
			if (flexDirection === "row-reverse") [left, right] = [right, left];
			else if (flexDirection === "column-reverse") [top, bottom] = [bottom, top];
		if (rtl) [left, right] = [right, left];
		return { left, right, top, bottom }[neighbor];
	} else {
		const opposite = ({ left: "right", right: "left", top: "bottom", bottom: "top" } as const)[neighbor];
		const isLeftTop = neighbor.in("left", "top") ? -1 : 1;
		const currentRect = el.getBoundingClientRect();
		const siblingRects = siblings
			.map(sibling => [sibling.getBoundingClientRect(), sibling] as const)
			.filter(([rect]) => (rect[opposite] - currentRect[neighbor]) * isLeftTop >= 0)
			.sort(([a], [b]) => (a[opposite] - b[opposite]) * isLeftTop)
			.sort(([a], [b]) => neighbor.in("left", "right") ?
				Math.hypot(a.top - currentRect.top, a.bottom - currentRect.bottom) - Math.hypot(b.top - currentRect.top, b.bottom - currentRect.bottom) :
				Math.hypot(a.left - currentRect.left, a.right - currentRect.right) - Math.hypot(b.left - currentRect.left, b.right - currentRect.right));
		const visualNeighbor = siblingRects[0]?.[1] ?? null;
		if (autoWrap) {
			if (index === 0 && neighbor === (!rtl ? "left" : "right") ||
				index === siblings.length - 1 && neighbor === (!rtl ? "right" : "left")) return null;
			if (visualNeighbor === null && neighbor.in("left", "right")) {
				let left = siblings[index - 1] ?? null, right = siblings[index + 1] ?? null;
				if (rtl) [left, right] = [right, left];
				return { left, right }[neighbor];
			}
		}
		return visualNeighbor;
	}
}

/**
 * Sets a reference to a given value. This utility function handles both callback refs
 * and object refs, ensuring compatibility with React's `ref` system.
 *
 * @template T - The type of the value to set on the ref.
 * @param ref - The React ref to set. It can be either a callback ref or an object ref.
 * @param value - The value to assign to the ref.
 */
function setRef<T>(ref: MiscRef<T>, value: T) {
	if (typeof ref === "function") ref(value);
	else if (isObject(ref)) ref.current = value;
}

const MERGED_REF_SYMBOL = Symbol.for("react-transition-group-fc.merged_ref");
type MergedRef<T = Element | null> = React.RefCallback<T> & {
	refs: Set<MiscRef<T>>;
	[MERGED_REF_SYMBOL]: true;
};

/**
 * Checks if the given ref is returned by function `mergeRefs`.
 * @template T - The value type that wrapped by the ref.
 * @param ref - The ref to check.
 * @returns Is the specified ref a merged ref?
 */
function isMergedRef<T>(ref: MiscRef<T>): ref is MergedRef<T> {
	return typeof ref === "function" && MERGED_REF_SYMBOL in ref;
}

/**
 * Merges multiple React refs into a single ref callback.
 * This allows you to pass multiple refs to a single element, ensuring all refs are updated
 * with the same element reference.
 *
 * @param refs - An array of React refs to be merged. Each ref can be a callback ref or a ref object.
 * @returns A ref callback function that updates all provided refs with the given element.
 */
export function mergeRefs(...refs: (MiscRef<Element | null> | undefined | null)[]) {
	const mergedRef = (el => {
		for (const ref of mergedRef.refs)
			setRef(ref, el);
	}) as MergedRef;
	mergedRef.refs = new Set();
	for (const ref of refs)
		if (!ref) continue;
		else if (isMergedRef(ref)) mergedRef.refs.adds(...ref.refs);
		else mergedRef.refs.add(ref);
	mergedRef[MERGED_REF_SYMBOL] = true;
	return mergedRef;
}

const scrollableOverflowValues = ["scroll", "auto", "overlay"];
/**
 * Finds the nearest scrollable ancestor of a given DOM element in the specified direction(s).
 *
 * @param element - The starting element whose scrollable parent is to be found.
 * @param direction - The scroll direction(s) to consider: "x" (horizontal), "y" (vertical), or "xy" (both). Defaults to "xy".
 * @returns The nearest ancestor element that is scrollable in the specified direction(s), or `null` if none is found.
 */
export function getScrollParent(element: Element | undefined | null, direction: "x" | "y" | "xy" = "xy") {
	element = element?.parentElement;
	while (element) {
		const style = getComputedStyle(element);
		const directionSet = Array.from(direction, dir => dir.toUpperCase() as "X" | "Y"); // ["X"], ["Y"], or ["X", "Y"]
		if (
			directionSet.some(dir => scrollableOverflowValues.includes(style[`overflow${dir}`])) /* ||
			directionSet.some(dir => style[`overscrollBehavior${dir}`] !== "auto") ||
			style.scrollbarGutter !== "auto" */
		)
			return element;
		element = element.parentElement;
	}
	return null;
}

/**
 * This is a special `scrollIntoView` function for horizontal tab bars and filters that require horizontal scrolling
 * and contain flippers components.
 *
 * Because the `clip-path` property is used in the scroll container to match `scroll-state` container query, even if
 * `scrollIntoViewIfNeeded` is used, part of it will still be masked (occluded by `clip-path`). Therefore, a special
 * `scrollIntoView` function needs to be re-customized.
 *
 * @param element - The HTML DOM element that will be scroll into view.
 * @param enableAlt - When false, it will directly call `scrollIntoViewIfNeeded` instead of this function.
 */
export function scrollIntoViewAlt(element: MaybeRef<HTMLElement | null>, enableAlt: boolean = true) {
	element = toValue(element);
	if (!element) return;
	if (!enableAlt) {
		element.scrollIntoViewIfNeeded();
		return;
	}
	const scrollParent = getScrollParent(element) as HTMLElement | null;
	if (!scrollParent) return;
	const flipperWidth = parseFloat(getComputedStyle(scrollParent).getPropertyValue("--flipper-width"));
	if (!flipperWidth || !scrollParent.querySelector(":scope > .flipper-wrapper")?.checkVisibility()) { // Not the target of consideration.
		element.scrollIntoViewIfNeeded();
		return;
	}
	const rtl = isRtl();
	const scrollLeft = { // Compatible with scrollLeft value in RTL layout.
		get value() {
			return !rtl ? scrollParent.scrollLeft : scrollParent.scrollWidth + scrollParent.scrollLeft - scrollParent.offsetWidth;
		},
		set value(value) {
			scrollParent.scrollLeft = !rtl ? value : value + scrollParent.offsetWidth - scrollParent.scrollWidth;
		},
	};
	if (scrollLeft.value > element.offsetLeft) {
		scrollLeft.value = element.offsetLeft;
		return;
	}
	const scrollClientWidth = scrollParent.clientWidth - flipperWidth * 2;
	const elementOffsetRight = element.offsetLeft + element.offsetWidth;
	if (scrollLeft.value + scrollClientWidth < elementOffsetRight)
		scrollLeft.value = elementOffsetRight - scrollClientWidth;
}
