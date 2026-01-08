import type { useAriaIdRefState } from "utils/object";

/**
 * A custom hook that returns a function that triggers a re-render of the component.
 *
 * This is useful when you need to force a re-render of a component without using the `useState` hook.
 *
 * @returns A function that triggers a re-render of the component.
 */
export function useForceUpdate() {
	return React.useReducer(() => ({}), {})[1] as () => void;
}

/**
 * Forward the ref from a local ref.
 * @template T - The value type that wrapped by the ref.
 * @param forwardedRef - Forwarded ref argument from `ref` prop (Or `forwardedRef` prop from the `forwardRef` function in React 18 and lower versions).
 * @param localRef - Local `useDomRef` variable.
 */
export function useImperativeHandleRef<T>(forwardedRef: React.ForwardedRef<T> | undefined, localRef: RefObject<T | null | undefined>) {
	useImperativeHandle(forwardedRef, () => localRef.current!);
}

/**
 * Forward the aria ID ref from a aria ID which get by `useID`.
 * @see {@link useAriaIdRefState}
 * @template T - The value type that wrapped by the ref.
 * @param forwardedAriaIdRef - Forwarded aria ID ref argument.
 * @param ariaId - Local aria ID which get by `useID` hook.
 */
export function useImperativeHandleAriaId(forwardedAriaIdRef: AriaIdRef | undefined, ariaId: string) {
	useImperativeHandle(forwardedAriaIdRef, () => ariaId, [ariaId]);
}
export type AriaIdRef = MiscRef<string | undefined | null>;

/**
 * If user click a button that inside another button, do not trigger outside button event.
 * @param handler - Mouse event handler.
 * @returns onClick handler.
 */
export function useOnNestedButtonClick(handler?: MouseEventHandler) {
	return useCallback<MouseEventHandler>(e => {
		const path = getPath(e);
		const currentTargetIndex = path.indexOf(e.currentTarget);
		if (currentTargetIndex !== -1) {
			if (path.slice(0, currentTargetIndex).find(element => element.tagName === "BUTTON")) return;
			if (path.slice(currentTargetIndex + 1).find(element => element.tagName === "BUTTON")) stopEvent(e);
		}
		handler?.(e);
	}, [handler]);
}

/**
 * A custom hook that controls the keyboard events for radio and checkbox elements.
 *
 * @param element - A reference to the HTML DOM element.
 * @param type - The type of the element, either "radio" or "checkbox".
 * @param handleCheck - A function that handles the change of the checkbox or radio button state.
 *
 * @returns A cleanup function that removes the event listeners when the component unmounts.
 */
export function useOnFormKeyDown(element: RefObject<HTMLElement | null>, { handleCheck = null, parent: parentSelector, item: itemSelector = ':not([tabindex="-1"])', focus: focusSelector = itemSelector, changeWhenMoveFocus, preventSpace = !!handleCheck, disableUpDown = false, disabled = false }: {
	handleCheck?: (() => void) | null;
	parent?: string;
	item?: string;
	focus?: string;
	changeWhenMoveFocus?: boolean;
	preventSpace?: boolean;
	disableUpDown?: boolean;
	disabled?: boolean;
} = {}) {
	const CUSTOM_CHANGE_EVENT = "customchange";

	useEventListener(element, "keydown", e => {
		if (disabled) return;
		const { code } = e;
		if (preventSpace && code === "Space") {
			stopEvent(e);
			return;
		}
		if (code.in("ArrowLeft", "ArrowUp", "ArrowRight", "ArrowDown", "Home", "End")) {
			if (disableUpDown && code.in("ArrowUp", "ArrowDown")) return;
			e.preventDefault();
			const target = (e.currentTarget ?? e.target) as HTMLElement | null;
			let parent = parentSelector ? target?.closest(parentSelector) : target?.parentElement;
			if (parent?.matches(".sortable-item") && !parentSelector) { // Specialized: If is implicit parent selector and the direct parent element is sortable item, then select the sortable view element as the parent.
				parent = parent.closest("." + nameof.kebab({ SortableView }));
				itemSelector = ".sortable-item";
			}
			const index = target?.indexIn(parent) ?? -1;
			if (index === -1 || !parent || !target) return;
			const items = [...parent.children].filter(element => (itemSelector === "*" || element.matches(itemSelector)) && !element.hasAttribute("disabled"));
			const immediate = parent.children[index] as HTMLElement;
			const indexInItems = items.indexOf(immediate);
			if (indexInItems === -1) return;
			const itemEl = code === "Home" ? items[0] : code === "End" ? items[items.length - 1] :
				getLayoutNeighbor(immediate, ({ ArrowLeft: "left", ArrowRight: "right", ArrowUp: "top", ArrowDown: "bottom" } as const)[code], items);
			const focusEl = itemEl?.querySelectorWithSelf(focusSelector) as HTMLElement;
			focusEl?.focus?.();
			if (changeWhenMoveFocus) itemEl?.dispatchEvent(new CustomEvent(CUSTOM_CHANGE_EVENT));
		}
	}, undefined, null);

	useEventListener(element, "keyup", e => {
		if (disabled) return;
		if (preventSpace && e.code.in("Space", "Enter")) {
			stopEvent(e);
			handleCheck?.();
		}
	}, undefined, null);

	useEventListener(element, CUSTOM_CHANGE_EVENT, () => handleCheck?.(), undefined, null);
}
